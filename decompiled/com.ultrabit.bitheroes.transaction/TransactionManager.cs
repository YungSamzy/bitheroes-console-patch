using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.material;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.payment.utility;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.transaction;

public class TransactionManager : Messenger
{
	public class PurchaseObject
	{
		public ItemRef itemRef;

		public int currencyID;

		public int currencyCost;

		public int qty;

		public string context;

		public string name;

		public ServiceRef serviceRef;
	}

	public class UnityEventTransactionComplete : UnityEvent<bool>
	{
	}

	private static TransactionManager _instance;

	private UnityAction<PurchaseObject> _onConfirmPurchaseCallback;

	private PaymentUtility _paymentUtility;

	private PurchaseObject purchaseObject;

	private string _purchaseContext;

	public UnityEventTransactionComplete onTransactionComplete = new UnityEventTransactionComplete();

	public static TransactionManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TransactionManager();
			}
			return _instance;
		}
	}

	public void DoFishingItemPurchase(FishingShopItemRef itemRef, int qty, string context = null)
	{
		GameData.instance.main.UpdateLoading();
		FishingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnFishingItemPurchase);
		FishingDALC.instance.doItemPurchase(itemRef, qty);
	}

	public void DoEventSalesItemPurchase(EventSalesShopEventRef eventRef, EventSalesShopItemRef itemRef, int qty, int purchaseRemainingQty = -1, string context = null, UnityAction callback = null)
	{
		GameData.instance.main.UpdateLoading();
		EventSalesDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventSalesItemPurchase);
		EventSalesDALC.instance.doItemPurchase(eventRef, itemRef, qty, purchaseRemainingQty);
	}

	private void ProcessItemPurchaseWithMaterials(BaseEvent baseEvent, string analyticsContext, UnityAction<int> onError)
	{
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			onTransactionComplete?.Invoke(arg0: false);
			int @int = sfsob.GetInt("err0");
			if (onError == null)
			{
				GameData.instance.windowGenerator.ShowErrorCode(@int);
			}
			else
			{
				onError(@int);
			}
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
		List<ItemData> list2 = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.addItems(list);
		GameData.instance.PROJECT.character.removeItems(list2);
		ItemRef originItem = null;
		if (list2 != null && list2.Count == 1 && list2[0] != null)
		{
			originItem = list2[0].itemRef;
		}
		KongregateAnalytics.checkEconomyTransaction("Purchase Item", list2, list, sfsob, analyticsContext, 0, currencyUpdate: false, originItem, checkCurrencyChange: false);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		onTransactionComplete?.Invoke(arg0: true);
	}

	private void OnFishingItemPurchase(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnFishingItemPurchase);
		ProcessItemPurchaseWithMaterials(baseEvent, "Fishing Shop", null);
	}

	private void OnEventSalesItemPurchase(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		EventSalesDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventSalesItemPurchase);
		ProcessItemPurchaseWithMaterials(baseEvent, "Event Shop", delegate(int errorCode)
		{
			if (errorCode.Equals(129))
			{
				ShowCustomEventSalesCurrentyErrorMessage(baseEvent);
			}
			else
			{
				GameData.instance.windowGenerator.ShowErrorCode(errorCode);
			}
		});
	}

	private void ShowCustomEventSalesCurrentyErrorMessage(BaseEvent baseEvent)
	{
		MaterialRef materialRef = MaterialBook.Lookup((baseEvent as DALCEvent).sfsob.GetInt("ite0"));
		if (materialRef != null)
		{
			string @string = Language.GetString("error_not_enough_event_sales_material", new string[1] { materialRef.name });
			GameData.instance.windowGenerator.ShowDialogMessage(Language.GetString("error_name"), @string);
		}
		else
		{
			GameData.instance.windowGenerator.ShowErrorCode(129);
		}
	}

	public void DoItemPurchase(ItemRef itemRef, int currencyID, int currencyCost, int qty = 1, UnityAction onSuccessPurchase = null, UnityAction onFailedPurchase = null, string context = null)
	{
		_purchaseContext = context;
		GameData.instance.main.UpdateLoading();
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_PURCHASE), OnItemPurchase);
		MerchantDALC.instance.doItemPurchase(itemRef.id, itemRef.itemType, currencyID, currencyCost, qty);
	}

	private void OnItemPurchase(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_PURCHASE), OnItemPurchase);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			onTransactionComplete?.Invoke(arg0: false);
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ConsumableRef consumableRef = null;
		bool flag = false;
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.addItems(list);
		GameData.instance.audioManager.PlaySoundLink("purchase");
		int @int = sfsob.GetInt("ite0");
		int int2 = sfsob.GetInt("ite1");
		ItemRef itemRef = ItemBook.Lookup(@int, int2);
		if (int2 == 5)
		{
			switch (ServiceBook.Lookup(@int).serviceType)
			{
			case 2:
				GameData.instance.PROJECT.character.energy = sfsob.GetInt("cha27");
				GameData.instance.PROJECT.character.energyMilliseconds = sfsob.GetLong("cha28");
				GameData.instance.PROJECT.character.energyCooldown = sfsob.GetLong("cha97");
				GameData.instance.PROJECT.character.updateEnergyRefillNotification();
				flag = true;
				break;
			case 3:
				GameData.instance.PROJECT.character.tickets = sfsob.GetInt("cha29");
				GameData.instance.PROJECT.character.ticketsMilliseconds = sfsob.GetLong("cha30");
				GameData.instance.PROJECT.character.ticketsCooldown = sfsob.GetLong("cha98");
				GameData.instance.PROJECT.character.updateTicketsRefillNotification();
				flag = true;
				break;
			case 6:
				GameData.instance.PROJECT.character.shards = sfsob.GetInt("cha67");
				GameData.instance.PROJECT.character.shardsMilliseconds = sfsob.GetLong("cha68");
				GameData.instance.PROJECT.character.shardsCooldown = sfsob.GetLong("cha101");
				flag = true;
				break;
			case 10:
				GameData.instance.PROJECT.character.tokens = sfsob.GetInt("cha71");
				GameData.instance.PROJECT.character.tokensMilliseconds = sfsob.GetLong("cha72");
				GameData.instance.PROJECT.character.tokensCooldown = sfsob.GetLong("cha99");
				flag = true;
				break;
			case 11:
				GameData.instance.PROJECT.character.badges = sfsob.GetInt("cha83");
				GameData.instance.PROJECT.character.badgesMilliseconds = sfsob.GetLong("cha84");
				GameData.instance.PROJECT.character.badgesCooldown = sfsob.GetLong("cha100");
				flag = true;
				break;
			case 4:
				GameData.instance.PROJECT.character.points = sfsob.GetInt("cha19");
				GameData.instance.PROJECT.character.power = sfsob.GetInt("cha6");
				GameData.instance.PROJECT.character.stamina = sfsob.GetInt("cha7");
				GameData.instance.PROJECT.character.agility = sfsob.GetInt("cha8");
				GameData.instance.windowGenerator.NewCharacterStatWindow();
				break;
			case 5:
				GameData.instance.windowGenerator.NewCharacterCustomizeWindow();
				break;
			default:
				flag = true;
				break;
			}
		}
		else
		{
			flag = true;
		}
		foreach (ItemData item in list)
		{
			if (item.itemRef.itemType == 4)
			{
				ConsumableRef consumableRef2 = item.itemRef as ConsumableRef;
				if (consumableRef2.forceConsume && item.qty > 0)
				{
					consumableRef = consumableRef2;
					break;
				}
			}
		}
		GameData.instance.PROJECT.CheckTutorialChanges();
		if (itemRef.costCredits > 0)
		{
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_SPENT, itemRef.costCredits, update: false);
		}
		else if (itemRef.costGold > 0)
		{
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_SPENT, itemRef.costGold, update: false);
		}
		KongregateAnalytics.checkEconomyTransaction("Purchase Item", null, list, sfsob, _purchaseContext, 0, currencyUpdate: false, itemRef);
		Broadcast("PURCHASE_COMPLETE");
		if (consumableRef != null)
		{
			ConsumableManager.instance.SetupConsumable(consumableRef, 1);
			ConsumableManager.instance.DoUseConsumable();
		}
		else if (flag && list.Count > 0)
		{
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, null, large: false, forceNonEquipment: false, null, -1, Language.GetString("ui_collect"), forceItemEnabled: true);
		}
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob, update: true);
		if (sfsob.ContainsKey("cha41"))
		{
			GameData.instance.PROJECT.character.shopRotationID = sfsob.GetInt("cha41");
		}
		if (sfsob.ContainsKey("cha42"))
		{
			GameData.instance.PROJECT.character.shopRotationMilliseconds = sfsob.GetLong("cha42");
		}
		_purchaseContext = null;
		onTransactionComplete?.Invoke(arg0: true);
	}

	public void DoPaymentPurchase(PaymentData paymentData)
	{
		ClearPaymentUtility();
		switch (AppInfo.platform)
		{
		case 1:
			_paymentUtility = new PaymentUtilityAndroid(paymentData);
			break;
		case 2:
			_paymentUtility = new PaymentUtilityIOS(paymentData);
			break;
		case 4:
			_paymentUtility = new PaymentUtilityKongregate(paymentData);
			break;
		case 7:
			_paymentUtility = new PaymentUtilitySteam(paymentData);
			break;
		case 8:
			_paymentUtility = new PaymentUtilityKartridge(paymentData);
			break;
		}
		if (_paymentUtility != null)
		{
			GameData.instance.main.ShowLoading();
			_paymentUtility.AddListener("SUCCESS", OnPaymentPurchaseSuccess);
			_paymentUtility.AddListener("FAIL", OnPaymentPurchaseFail);
			_paymentUtility.Init();
		}
	}

	private void ClearPaymentUtility()
	{
		if (_paymentUtility != null)
		{
			_paymentUtility.RemoveListener("SUCCESS", OnPaymentPurchaseSuccess);
			_paymentUtility.RemoveListener("FAIL", OnPaymentPurchaseFail);
			_paymentUtility.Clear();
			_paymentUtility = null;
		}
	}

	private void OnPaymentPurchaseSuccess()
	{
		GameData.instance.main.HideLoading();
		_ = _paymentUtility.paymentData;
		List<ItemData> items = _paymentUtility.items;
		if (items != null && items.Count > 0)
		{
			GameData.instance.windowGenerator.ShowItems(items, compare: true, added: true);
		}
		onTransactionComplete?.Invoke(arg0: true);
		ClearPaymentUtility();
	}

	private void OnPaymentPurchaseFail()
	{
		GameData.instance.main.HideLoading();
		onTransactionComplete?.Invoke(arg0: false);
		ClearPaymentUtility();
	}

	public DialogWindow ConfirmItemPurchase(ItemRef itemRef, string context = null, int qty = 1, string text = null, UnityAction<PurchaseObject> onConfirmCallback = null, PurchaseObject purchaseObject = null, object data = null)
	{
		int num = ((itemRef.costCreditsRaw <= 0) ? 1 : 2);
		if (data is GuildShopRef)
		{
			num = 7;
		}
		CurrencyRef currencyRef = CurrencyBook.Lookup(num);
		int cost = itemRef.getCost(num, qty);
		List<int> list = new List<int>();
		switch (num)
		{
		case 1:
			list.Add(1);
			break;
		case 2:
			list.Add(0);
			break;
		}
		if (purchaseObject == null)
		{
			this.purchaseObject = new PurchaseObject();
			this.purchaseObject.itemRef = itemRef;
			this.purchaseObject.currencyID = num;
			this.purchaseObject.currencyCost = cost;
			this.purchaseObject.qty = qty;
			this.purchaseObject.context = context;
		}
		else
		{
			this.purchaseObject = purchaseObject;
		}
		if (text == null)
		{
			text = ((qty <= 1) ? Language.GetString("purchase_confirm", new string[3]
			{
				itemRef.coloredName,
				Util.NumberFormat(cost),
				currencyRef.name
			}) : Language.GetString("purchase_quantity_confirm", new string[4]
			{
				Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(qty) }),
				itemRef.coloredName,
				Util.NumberFormat(cost),
				currencyRef.name
			}));
		}
		if (onConfirmCallback == null)
		{
			onConfirmCallback = OnItemPurchaseYes;
		}
		_onConfirmPurchaseCallback = onConfirmCallback;
		DialogWindow dialogWindow = GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), text, null, null, delegate
		{
			CheckNFTWarning(itemRef.bindType == ItemRef.BindType.hero);
		}, null, null, -1, list.ToArray());
		dialogWindow.ShowEulaButton();
		return dialogWindow;
	}

	private void CheckNFTWarning(bool isHeroBound)
	{
		if (GameData.instance.PROJECT.character.imxG0Data != null && isHeroBound && !GameData.instance.SAVE_STATE.eulaVerified)
		{
			GameData.instance.windowGenerator.NewEulaImportantWindow(OnConfirmPurchase);
		}
		else
		{
			OnConfirmPurchase();
		}
	}

	private void OnConfirmPurchase()
	{
		_onConfirmPurchaseCallback?.Invoke(purchaseObject);
	}

	private void OnItemPurchaseYes(PurchaseObject purchaseObject)
	{
		DoItemPurchase(purchaseObject.itemRef, purchaseObject.currencyID, purchaseObject.currencyCost, purchaseObject.qty, null, null, purchaseObject.context);
	}

	public void ForceUpdateConsumableModifier(bool force)
	{
		if (force)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(61), OnUseAdGorConsumableModifier);
			CharacterDALC.instance.doUpdateAdGorConsumableModifier();
		}
	}

	private void OnUseAdGorConsumableModifier(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(61), OnUseAdGorConsumableModifier);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError("TransactionManager::OnUseAdGorConsumableModifier" + sfsob.GetInt("err0"));
			return;
		}
		List<ConsumableModifierData> consumableModifiers = ConsumableModifierData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.consumableModifiers = consumableModifiers;
		Broadcast("ADGOR_UPDATE");
	}
}
