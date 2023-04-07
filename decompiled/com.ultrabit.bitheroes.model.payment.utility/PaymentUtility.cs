using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.payment.utility;

public class PaymentUtility : Messenger
{
	private const int CHECKS_MAX = 3;

	private const int CHECKS_DELAY = 2;

	private PaymentData _paymentData;

	private List<ItemData> _items;

	private int _qtyBefore;

	private int _checks;

	public PaymentData paymentData => _paymentData;

	public List<ItemData> items => _items;

	public PaymentUtility(PaymentData paymentData)
	{
		_paymentData = paymentData;
	}

	public virtual void Clear()
	{
	}

	public virtual void Init()
	{
	}

	protected void CheckPurchase()
	{
		List<ItemData> list = new List<ItemData>();
		switch (paymentData.paymentRef.type)
		{
		case 1:
			list.Add(new ItemData(CurrencyBook.Lookup(2), paymentData.paymentRef.credits));
			DispatchSuccess(list);
			break;
		case 2:
		case 3:
			DoInventoryCheck();
			break;
		}
	}

	protected void DoInventoryCheck()
	{
		_qtyBefore = GameData.instance.PROJECT.character.getItemQty(paymentData.paymentRef.itemData.itemRef);
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		CharacterDALC.instance.doInventoryCheck();
	}

	private void OnInventoryCheck(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			Broadcast("FAIL");
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		if (list != null && list.Count > 0)
		{
			GameData.instance.PROJECT.character.updateInventoryItems(list);
		}
		if (GameData.instance.PROJECT.character.getItemQty(paymentData.paymentRef.itemData.itemRef) > _qtyBefore)
		{
			List<ItemData> list2 = new List<ItemData>();
			list2.Add(paymentData.paymentRef.itemData.copy());
			if (paymentData.paymentRef.itemData.itemRef.itemType == 4)
			{
				ConsumableRef consumableRef = paymentData.paymentRef.itemData.itemRef as ConsumableRef;
				if (consumableRef.forceConsume)
				{
					ConsumableManager.instance.SetupConsumable(consumableRef, 1);
					ConsumableManager.instance.DoUseConsumable(1, delegate
					{
						DispatchSuccess();
					});
				}
				else
				{
					DispatchSuccess(list2, add: false);
				}
			}
			else
			{
				DispatchSuccess(list2, add: false);
			}
		}
		else if (_checks < 3)
		{
			_checks++;
			new DelayFunction().Delay(2f, DoInventoryCheck);
		}
		else
		{
			Broadcast("FAIL");
		}
	}

	protected void DispatchSuccess(List<ItemData> items = null, bool add = true)
	{
		_items = items;
		if (add && items != null)
		{
			GameData.instance.PROJECT.character.addItems(items);
		}
		Broadcast("SUCCESS");
	}

	protected void DispatchReceiptValidationFailed()
	{
		KongregateAnalytics.trackPaymentReceiptFail(KongregateAnalytics.getPaymentGameFields(paymentData.paymentRef.getPaymentStatType));
		Broadcast("FAIL");
	}

	protected void DispatchPaymentFailed()
	{
		KongregateAnalytics.trackPaymentFail(KongregateAnalytics.getPaymentGameFields(paymentData.paymentRef.getPaymentStatType));
		Broadcast("FAIL");
	}

	protected string GetPaymentID()
	{
		if (paymentData != null && paymentData.paymentRef != null)
		{
			return paymentData.paymentRef.paymentID;
		}
		return "NOT_AVAILABLE";
	}
}
