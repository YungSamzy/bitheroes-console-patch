using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.payment.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public class PaymentCreditsPanelKongregate : PaymentCreditsPanel
{
	private GameObject paymentResponseHandler;

	private PaymentUtilityBehaviour paymentUtilityKongregateResponseHandler;

	private bool _purchasing;

	private void Start()
	{
		_purchasing = false;
	}

	private void OnPurchaseResult(bool status)
	{
		_purchasing = false;
		GameData.instance.main.paymentUtilityBehaviour.onKongregatePaymentComplete.RemoveListener(OnPurchaseResult);
		if (status)
		{
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType, selectedPaymentData.paymentRef.credits));
			ShowSuccess(selectedPaymentData.paymentRef.credits);
		}
		else
		{
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
		}
	}

	protected override void doPayment()
	{
		if (!_purchasing)
		{
			_purchasing = true;
			Invoke("SetPurchasingValue", 2f);
		}
	}

	private void SetPurchasingValue()
	{
		_purchasing = false;
	}

	private void OnDestroy()
	{
		CancelInvoke("SetPurchasingValue");
	}
}
