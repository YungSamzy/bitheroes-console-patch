using System;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment.utility;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;

namespace com.ultrabit.bitheroes.ui.payment.custom;

public class PaymentCustomWindowIOS : PaymentCustomWindowHandler
{
	private PaymentUtilityBehaviour paymentUtilityBehaviour;

	private bool _purchasing;

	private void Start()
	{
		_purchasing = false;
	}

	public override void doPayment()
	{
		if (!_purchasing)
		{
			_purchasing = true;
			D.Log("all", "IAPFlow " + _customWindow.paymentRef.paymentID + " - Start", forceLoggly: true);
			GameData.instance.transactionIAPMobile.InitPurchaseProcess(_customWindow.paymentRef.paymentID, OnPurchaseProcessed);
		}
	}

	public void OnPurchaseProcessed(TransactionIAPMobile.TransactionIAPMobileResult result)
	{
		if (result == null)
		{
			D.LogError("all", "IAPFlow " + GetPaymentID() + " - Payment Failed  - Invalid Response in OnPurchaseProcessed (Result is null)");
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
			_customWindow.OnPaymentFailed();
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
				KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
				_customWindow.OnPaymentFailed();
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
		_purchasing = false;
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
			KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
			_customWindow.OnPaymentFailed();
		}
		else
		{
			try
			{
				D.Log("all", $"IAPFlow {GetPaymentID()} - Receipt Validation - Response Process Start {response.code} ", forceLoggly: true);
				switch (response.code)
				{
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.SUCCESS:
					D.Log("all", "IAPFlow " + _customWindow.paymentRef.paymentID + " - Receipt Validation Complete - Success", forceLoggly: true);
					KongregateAnalytics.trackPaymentSuccess(response.receiptObject.transactionID, KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType, _customWindow.paymentRef.credits));
					_customWindow.DoInventoryCheck();
					break;
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.TRANSACTION_ALREADY_EXISTS:
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.VALIDATION_ERROR:
					D.LogError("all", "IAPFlow " + _customWindow.paymentRef.paymentID + " - Receipt Validation Complete - Error: " + response.message);
					KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
					GameData.instance.windowGenerator.ShowError(response.message);
					_customWindow.OnPaymentFailed();
					break;
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.UNKNOWN:
				case PaymentUtilityBehaviour.ValidateiOSReceiptResponse.CODE.NETWORK_ERROR:
					D.LogError("all", $"IAPFlow {_customWindow.paymentRef.paymentID} - Receipt Validation Complete - Network/Unknown Error {response.code}");
					KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
					GameData.instance.windowGenerator.ShowError(Language.GetString("ui_checking_connection_error"));
					_customWindow.OnPaymentFailed();
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
	}
}
