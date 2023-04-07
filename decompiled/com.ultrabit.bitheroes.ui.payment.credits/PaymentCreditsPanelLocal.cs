using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.kongregate;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public class PaymentCreditsPanelLocal : PaymentCreditsPanel
{
	private bool _purchasing;

	public override void LoadDetails(bool refreshable = false)
	{
		base.LoadDetails(refreshable);
		_purchasing = false;
	}

	protected override void doPayment()
	{
		if (!_purchasing)
		{
			Invoke("SuccessBuy", 1f);
		}
	}

	private void SuccessBuy()
	{
		ShowSuccess(100);
	}

	private void doPaymentSteamComplete()
	{
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.PAYMENT_STEAM_COMPLETE), onPaymentSteamComplete);
		MerchantDALC.instance.doPaymentSteamComplete();
	}

	private void onPaymentSteamComplete(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.PAYMENT_STEAM_COMPLETE), onPaymentSteamComplete);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType));
			return;
		}
		int @int = sfsob.GetInt("pay4");
		if (@int > 0)
		{
			ShowSuccess(@int);
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(selectedPaymentData.paymentRef.getPaymentStatType, @int));
		}
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob);
	}
}
