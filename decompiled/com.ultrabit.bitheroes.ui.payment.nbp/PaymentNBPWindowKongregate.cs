using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.payment.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.payment.nbp;

public class PaymentNBPWindowKongregate : PaymentNBPWindowHandler
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
			_nbpWindow.DoInventoryCheck();
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(_nbpWindow.paymentRef.getPaymentStatType, _nbpWindow.paymentRef.credits));
		}
		else
		{
			_nbpWindow.OnPaymentFailed();
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(_nbpWindow.paymentRef.getPaymentStatType));
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
