using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.lists.itemcontentslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemContentsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI costTxt;

	public Button probabilityBtn;

	public Button buyBtn;

	public Image goldIcon;

	public Image creditsIcon;

	public Image costBG;

	public ItemContentsList itemContentsList;

	private ItemRef _itemRef;

	private EventSalesShopItemRefModelData _eventModel;

	private int _qty;

	private object _data;

	private bool _purchaseable;

	private bool _confirm = true;

	private bool _displayQty = true;

	private bool _displayCompare = true;

	private bool _displayUnlocked = true;

	private List<ItemData> _items;

	public ItemRef itemRef => _itemRef;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ItemRef itemRef, int qty = 1, EventSalesShopItemRefModelData eventModel = null, object data = null, bool purchaseable = true)
	{
		_itemRef = itemRef;
		_qty = qty;
		_eventModel = eventModel;
		_data = data;
		_purchaseable = purchaseable;
		topperTxt.text = Language.GetString("ui_contents");
		buyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_buy");
		string text = "";
		if (itemRef.itemType == 4)
		{
			ConsumableRef consumableRef = itemRef as ConsumableRef;
			text = consumableRef.localizedSummary;
			_displayQty = consumableRef.displayQty;
			_displayCompare = consumableRef.displayCompare;
			_displayUnlocked = consumableRef.displayUnlocked;
		}
		if (itemRef.probabilityRef != null || itemRef.box != "")
		{
			probabilityBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		}
		else
		{
			probabilityBtn.gameObject.SetActive(value: false);
		}
		descTxt.text = Util.ParseString(text);
		if ((!itemRef.getPurchasable() || !_purchaseable) && eventModel == null)
		{
			buyBtn.gameObject.SetActive(value: false);
			goldIcon.gameObject.SetActive(value: false);
			creditsIcon.gameObject.SetActive(value: false);
			costTxt.gameObject.SetActive(value: false);
			costBG.gameObject.SetActive(value: false);
		}
		else
		{
			UpdateCost();
			ListenForForward(DoPurchase);
		}
		itemContentsList.StartList();
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		DoItemContents();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void DoItemContents()
	{
		GameData.instance.main.ShowLoading();
		Disable();
		GameData.instance.main.ShowLoading();
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_CONTENTS), OnItemContents);
		MerchantDALC.instance.doItemContents(_itemRef.id, _itemRef.itemType);
	}

	private void OnItemContents(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_CONTENTS), OnItemContents);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		CreateTiles(items);
	}

	private void CreateTiles(List<ItemData> items)
	{
		itemContentsList.ClearList();
		itemContentsList.displayCompare = _displayCompare;
		itemContentsList.displayUnlocked = _displayUnlocked;
		itemContentsList.displayQty = _displayQty;
		List<ItemContentsListItem> list = new List<ItemContentsListItem>();
		foreach (ItemData item in items)
		{
			if (item != null)
			{
				list.Add(new ItemContentsListItem
				{
					itemData = item
				});
			}
		}
		itemContentsList.Data.InsertItems(0, list);
		GameData.instance.main.HideLoading();
		UpdateTiles();
	}

	public void UpdateTiles()
	{
		bool flag = true;
		if (itemContentsList != null && itemContentsList.Data != null && _itemRef != null)
		{
			foreach (ItemContentsListItem item in itemContentsList.Data.List)
			{
				if (_itemRef.gacha && item.itemData.qty > 0)
				{
					flag = false;
				}
			}
		}
		if (!_itemRef.gacha || !(buyBtn != null))
		{
			return;
		}
		if (flag)
		{
			if (buyBtn != null)
			{
				Util.SetButton(buyBtn, enabled: false);
			}
		}
		else if (buyBtn != null)
		{
			Util.SetButton(buyBtn);
		}
	}

	public void OnBuyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoPurchase();
	}

	public void OnProbabilityBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoProbability();
	}

	private void DoPurchase()
	{
		if (_eventModel != null)
		{
			DoPurchaseEventSalesShopPurchase(_eventModel);
			return;
		}
		TransactionManager.instance.onTransactionComplete.AddListener(OnTransactionComplete);
		PaymentData paymentData = _itemRef.getPaymentData();
		if (paymentData != null)
		{
			Util.SetButton(buyBtn, enabled: false);
			TransactionManager.instance.DoPaymentPurchase(paymentData);
			return;
		}
		if (_confirm)
		{
			TransactionManager.instance.ConfirmItemPurchase(_itemRef, "Shop", _qty, null, null, null, _data);
		}
		else
		{
			_confirm = true;
			int currencyID = ((_itemRef.costCreditsRaw <= 0) ? 1 : 2);
			int cost = _itemRef.getCost(currencyID, _qty);
			TransactionManager.instance.DoItemPurchase(_itemRef, currencyID, cost, _qty);
		}
		GameData.instance.PROJECT.CheckTutorialChanges();
	}

	private void DoPurchaseEventSalesShopPurchase(EventSalesShopItemRefModelData eventSalesShopItemRefModelData)
	{
		string text = null;
		text = ((eventSalesShopItemRefModelData.qty <= 1) ? Language.GetString("purchase_confirm", new string[3]
		{
			itemRef.coloredName,
			Util.NumberFormat(eventSalesShopItemRefModelData.eventSalesShopItemRef.cost),
			eventSalesShopItemRefModelData.eventRef.GetMaterialRef().name
		}) : Language.GetString("purchase_quantity_confirm", new string[4]
		{
			Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(eventSalesShopItemRefModelData.qty) }),
			eventSalesShopItemRefModelData.eventSalesShopItemRef.itemData.itemRef.coloredName,
			Util.NumberFormat(eventSalesShopItemRefModelData.eventSalesShopItemRef.cost * eventSalesShopItemRefModelData.qty),
			eventSalesShopItemRefModelData.eventRef.GetMaterialRef().name
		}));
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), text, null, null, delegate
		{
			TransactionManager.instance.DoEventSalesItemPurchase(eventSalesShopItemRefModelData.eventRef, eventSalesShopItemRefModelData.eventSalesShopItemRef, eventSalesShopItemRefModelData.qty, eventSalesShopItemRefModelData.eventSalesShopItemRef.purchaseRemainingQty);
			OnClose();
		});
	}

	private void OnTransactionComplete(bool status)
	{
		TransactionManager.instance.onTransactionComplete.RemoveListener(OnTransactionComplete);
		if (status)
		{
			OnClose();
		}
	}

	private void DoProbability()
	{
		if (_itemRef.box != "")
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_help"), Language.GetString(_itemRef.box), null, null, null, 10);
			return;
		}
		ProbabilityRef probabilityRef = _itemRef.probabilityRef;
		if (probabilityRef == null)
		{
			return;
		}
		int offset = 60;
		bool outline = true;
		string text = "#FFFFFF";
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData(Language.GetString("ui_odds"), outline, offset, text);
		foreach (ProbabilityRarityRef rarity in probabilityRef.rarities)
		{
			characterInfoData.addValue(rarity.rarityRef.coloredName, Util.NumberFormat(rarity.perc) + "%", text);
		}
		list.Add(characterInfoData);
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list, _itemRef.name);
	}

	private void OnShopRotationChange()
	{
		UpdateCost();
	}

	public void UpdateCost()
	{
		if (!_itemRef.getPurchasable() && _eventModel == null)
		{
			return;
		}
		ShopSaleRef itemSaleRef = ShopBook.GetItemSaleRef(_itemRef, GameData.instance.PROJECT.character.shopRotationID);
		PaymentData paymentData = _itemRef.getPaymentData();
		bool flag = _eventModel != null;
		bool flag2 = itemSaleRef != null && paymentData == null && !flag;
		int num;
		int num2;
		if (flag)
		{
			num = 14;
			num2 = _eventModel.eventSalesShopItemRef.cost;
		}
		else
		{
			num = ((paymentData == null) ? ((_itemRef.costCredits <= 0) ? 1 : 2) : 0);
			num2 = ((paymentData == null) ? ((num == 2) ? _itemRef.costCredits : _itemRef.costGold) : 0);
		}
		num2 *= _qty;
		goldIcon.gameObject.SetActive(num == 1 || flag);
		creditsIcon.gameObject.SetActive(num == 2);
		if (flag)
		{
			goldIcon.overrideSprite = _eventModel.eventRef.GetMaterialRef().GetSpriteIcon();
		}
		string text;
		if (paymentData != null && !flag)
		{
			text = paymentData.price;
		}
		else
		{
			text = ((num2 < 10000) ? Util.NumberFormat(num2) : Util.NumberFormat(num2, abbreviate: true, shortbool: true));
			if (flag2 && itemSaleRef.mult < 1f)
			{
				text = Util.ParseString("^" + text + "^");
			}
		}
		costTxt.text = text;
		Util.SetButton(buyBtn, _itemRef.allowPurchase() || flag);
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (buyBtn != null && buyBtn.gameObject != null)
		{
			buyBtn.interactable = true;
		}
		if (probabilityBtn != null && probabilityBtn.gameObject != null)
		{
			probabilityBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (buyBtn != null && buyBtn.gameObject != null)
		{
			buyBtn.interactable = false;
		}
		if (probabilityBtn != null && probabilityBtn.gameObject != null)
		{
			probabilityBtn.interactable = false;
		}
	}
}
