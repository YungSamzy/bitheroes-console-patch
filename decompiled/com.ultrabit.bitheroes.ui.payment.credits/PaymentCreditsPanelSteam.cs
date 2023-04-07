using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.kongregate;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Steamworks;

namespace com.ultrabit.bitheroes.ui.payment.credits;

public class PaymentCreditsPanelSteam : PaymentCreditsPanel
{
	private Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

	private Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	private bool _purchasing;

	public override void LoadDetails(bool refreshable = false)
	{
		base.LoadDetails(refreshable);
		_purchasing = false;
	}

	protected override void doPayment()
	{
		if (!_purchasing && SteamManager.Initialized)
		{
			_purchasing = true;
			UnregisterCallbacks();
			GameData.instance.main.ShowLoading();
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
			MerchantDALC.instance.doPaymentSteamStart(selectedPaymentData.paymentRef);
		}
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
	}

	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t param)
	{
		_purchasing = false;
		if (param.m_bAuthorized == 1)
		{
			doPaymentSteamComplete();
		}
		else
		{
			GameData.instance.main.HideLoading();
		}
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

	private void OnDestroy()
	{
		UnregisterCallbacks();
	}

	private void UnregisterCallbacks()
	{
		if (m_MicroTxnAuthorizationResponse != null)
		{
			m_MicroTxnAuthorizationResponse.Unregister();
		}
		if (m_GameOverlayActivated != null)
		{
			m_GameOverlayActivated.Unregister();
		}
	}
}
