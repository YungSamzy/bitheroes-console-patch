using System;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment.utility;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public class PaymentCreditsPanelIOS : PaymentCreditsPanel
{
	public override void LoadDetails(bool refreshable = false)
	{
		base.LoadDetails(refreshable);
	}

	protected override void doPayment()
	{
		if (selectedPaymentData == null)
		{
			D.LogError("all", $"IAPFlow {GetType()} - Error: Process cannot start because no paymentdata found");
			return;
		}
		D.Log("all", "IAPFlow " + GetPaymentID() + " - Start", forceLoggly: true);
		GameData.instance.main.ShowLoading();
		GameData.instance.transactionIAPMobile.InitPurchaseProcess(selectedPaymentData.paymentRef.paymentID, OnPurchaseProcessed);
	}

	public void OnPurchaseProcessed(TransactionIAPMobile.TransactionIAPMobileResult result)
	{
		if (result == null)
		{
			D.LogError("all", "IAPFlow " + GetPaymentID() + " - Payment Failed  - Invalid Response in OnPurchaseProcessed (Result is null)");
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
			return;
		}
		try
		{
			D.Log("all", $"IAPFlow {GetPaymentID()} - Payment Complete. Result Process Start {result.code}", forceLoggly: true);
			switch (result.code)
			{
			case 1:
				D.Log("all", "IAPFlow " + GetPaymentID() + " - Payment Success. Starting Receipt Validation", forceLoggly: true);
				GameData.instance.main.paymentUtilityBehaviour.oniOSValidationPurchaseComplete.AddListener(OnValidationResult);
				GameData.instance.main.paymentUtilityBehaviour.ValidateiOSReceipt(result);
				break;
			case 2:
				D.LogError("all", "IAPFlow " + GetPaymentID() + " - Payment Failed - " + result.failureReason);
				GameData.instance.main.HideLoading();
				GameData.instance.windowGenerator.ShowError(result.failureReason);
				KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
				break;
			}
			D.Log("all", $"IAPFlow {GetPaymentID()} - Payment Complete. Result Process End {result.code}", forceLoggly: true);
		}
		catch (Exception ex)
		{
			GameData.instance.main.HideLoading();
			if (result.code.Equals(1))
			{
				GameData.instance.transactionIAPMobile.ConfirmPendingPurchase(GetPaymentID());
			}
			D.LogException(ex.Message, ex);
		}
	}

	private void OnValidationResult(PaymentUtilityBehaviour.ValidateiOSReceiptResponse response)
	{
		GameData.instance.main.HideLoading();
		try
		{
			GameData.instance.main.paymentUtilityBehaviour.oniOSValidationPurchaseComplete.RemoveListener(OnValidationResult);
		}
		catch (Exception e)
		{
			D.LogException($"{GetType()}", e);
		}
		if (response == null)
		{
			D.LogError("all", "IAPFlow " + GetPaymentID() + " - Receipt Validation Complete  - Error: Invalid Response in OnValidationResult (Response is null)");
			KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
		}
		else
		{
			try
			{
				D.Log("all", $"IAPFlow {GetPaymentID()} - Receipt Validation - Response Process Start {response.code} ", forceLoggly: true);
				switch (response.code)
				{
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.SUCCESS:
					D.Log("all", "IAPFlow " + GetPaymentID() + " - Receipt Validation Complete - Success", forceLoggly: true);
					ShowSuccess(selectedPaymentData.paymentRef.credits);
					KongregateAnalytics.trackPaymentSuccess(response.receiptObject.transactionID, KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType, selectedPaymentData.paymentRef.credits));
					break;
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.TRANSACTION_ALREADY_EXISTS:
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.VALIDATION_ERROR:
					D.LogError("all", "IAPFlow " + GetPaymentID() + " - Receipt Validation Complete - Error: " + response.message);
					KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
					GameData.instance.windowGenerator.ShowError(response.message);
					break;
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.UNKNOWN:
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.NETWORK_ERROR:
					D.LogError("all", $"IAPFlow {GetPaymentID()} - Receipt Validation Complete - Network/Unknown Error {response.code}");
					GameData.instance.windowGenerator.ShowError(Language.GetString("ui_checking_connection_error"));
					break;
				}
				D.Log("all", $"IAPFlow {GetPaymentID()} - Receipt Validation - Response Process End {response.code} ", forceLoggly: true);
			}
			catch (Exception ex)
			{
				D.LogException(ex.Message, ex);
			}
		}
		GameData.instance.transactionIAPMobile.ConfirmPendingPurchase(GetPaymentID());
		CleanUpProcess();
	}

	private void OnDestroy()
	{
		GameData.instance.main.paymentUtilityBehaviour.oniOSValidationPurchaseComplete.RemoveListener(OnValidationResult);
	}
}
