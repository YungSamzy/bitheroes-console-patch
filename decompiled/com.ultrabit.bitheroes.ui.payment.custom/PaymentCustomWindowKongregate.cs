using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.payment.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.payment.custom;

public class PaymentCustomWindowKongregate : PaymentCustomWindowHandler
{
	private GameObject paymentResponseHandler;

	private PaymentUtilityBehaviour paymentUtilityKongregateResponseHandler;

	private bool _purchasing;

	private void Start()
	{
		GameData.instance.main.paymentUtilityBehaviour.onKongregatePaymentComplete.AddListener(OnPurchaseResult);
		_purchasing = false;
	}

	private void OnPurchaseResult(bool status)
	{
		_purchasing = false;
		GameData.instance.main.paymentUtilityBehaviour.onKongregatePaymentComplete.RemoveListener(OnPurchaseResult);
		if (status)
		{
			_customWindow.DoInventoryCheck();
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType, _customWindow.paymentRef.credits));
		}
		else
		{
			_customWindow.OnPaymentFailed();
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(_customWindow.paymentRef.getPaymentStatType));
		}
	}

	public override void doPayment()
	{
		if (!_purchasing)
		{
			_purchasing = true;
		}
	}
}
