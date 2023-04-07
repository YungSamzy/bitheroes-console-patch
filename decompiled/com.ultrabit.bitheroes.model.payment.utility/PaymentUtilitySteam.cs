using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Steamworks;

namespace com.ultrabit.bitheroes.model.payment.utility;

public class PaymentUtilitySteam : PaymentUtility
{
	private Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

	private Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	public PaymentUtilitySteam(PaymentData paymentData)
		: base(paymentData)
	{
	}

	public override void Clear()
	{
		base.Clear();
		if (m_GameOverlayActivated != null)
		{
			m_GameOverlayActivated.Unregister();
		}
		if (m_MicroTxnAuthorizationResponse != null)
		{
			m_MicroTxnAuthorizationResponse.Unregister();
		}
	}

	public override void Init()
	{
		base.Init();
		if (SteamManager.Initialized)
		{
			GameData.instance.main.ShowLoading();
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
			MerchantDALC.instance.doPaymentSteamStart(base.paymentData.paymentRef);
		}
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
	}

	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t param)
	{
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
			KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(base.paymentData.paymentRef.getPaymentStatType));
			return;
		}
		int @int = sfsob.GetInt("pay4");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		if (@int > 0)
		{
			new List<ItemData>().Add(new ItemData(CurrencyBook.Lookup(2), base.paymentData.paymentRef.credits));
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(base.paymentData.paymentRef.getPaymentStatType, @int));
		}
		else if (list.Count > 0)
		{
			DoInventoryCheck();
			GameData.instance.PROJECT.character.addItems(list);
			KongregateAnalytics.trackPaymentSuccess("", KongregateAnalytics.getPaymentGameFields(base.paymentData.paymentRef.getPaymentStatType, base.paymentData.paymentRef.credits));
		}
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob);
	}
}
