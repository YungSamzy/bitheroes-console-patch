using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.kongregate;

namespace com.ultrabit.bitheroes.model.payment.utility;

public class PaymentUtilityKongregate : PaymentUtility
{
	private PaymentUtilityBehaviour paymentUtilityKongregateResponseHandler;

	public PaymentUtilityKongregate(PaymentData paymentData)
		: base(paymentData)
	{
		GameData.instance.main.paymentUtilityBehaviour.onKongregatePaymentComplete.AddListener(OnPurchaseResult);
	}

	private void OnPurchaseResult(bool status)
	{
		GameData.instance.main.paymentUtilityBehaviour.onKongregatePaymentComplete.RemoveListener(OnPurchaseResult);
		if (status)
		{
			CheckPurchase();
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(base.paymentData.paymentRef.getPaymentStatType, base.paymentData.paymentRef.credits));
		}
		else
		{
			DispatchPaymentFailed();
		}
	}

	public override void Init()
	{
		base.Init();
	}
}
