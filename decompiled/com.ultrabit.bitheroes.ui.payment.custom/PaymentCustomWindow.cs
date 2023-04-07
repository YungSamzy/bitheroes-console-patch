using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.nbpitemslist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.payment.custom;

public class PaymentCustomWindow : WindowsMain
{
	private const float DISPLAY_OFFSET = -63f;

	private const float DISPLAY_MOUNT_OFFSET = -22f;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 38f;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI descTxtR;

	public Button buyBtn;

	public RectTransform placeholderDisplay;

	public RectTransform placeholderStats;

	[SerializeField]
	private Button eulaBtn;

	public NBPItemsList itemsList;

	public Image goldIcon;

	public Image creditsIcon;

	public ItemTooltipStatTile itemTooltipStatTilePrefab;

	private PaymentRef _paymentRef;

	private ItemData _itemData;

	private bool _loaded;

	private List<ItemData> _items;

	private List<ItemTooltipStatTile> _statTiles;

	private int _consumableId = -1;

	private ConsumableRef _newConsumable;

	private SortingGroup _characterSorting;

	private CharacterDisplay _characterDisplay;

	private PaymentCustomWindowHandler paymentHandler;

	private bool allowPurchase
	{
		get
		{
			List<ConsumableModifierData> consumableModifiers = GameData.instance.PROJECT.character.consumableModifiers;
			if (consumableModifiers != null)
			{
				List<int> list = new List<int> { 3033, 3034, 3035 };
				foreach (ConsumableModifierData item in consumableModifiers)
				{
					if (list.Contains(item.consumableRef.id) && item.isActive())
					{
						return false;
					}
				}
			}
			if (!_loaded || (_itemData.itemRef.unique && GameData.instance.PROJECT.character.inventory.hasOwnedItem(_itemData.itemRef)))
			{
				return false;
			}
			return true;
		}
	}

	public PaymentRef paymentRef => _paymentRef;

	public bool loaded => _loaded;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(PaymentRef paymentRef)
	{
		_paymentRef = paymentRef;
		_itemData = _paymentRef.itemData;
		ConsumableRef consumableRef = _itemData.itemRef as ConsumableRef;
		descTxt.text = Util.ParseString(consumableRef.desc);
		descTxtR.text = Util.ParseString(consumableRef.coloredName);
		buyBtn.onClick.AddListener(OnBuyBtn);
		buyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_buy");
		itemsList.InitList();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		ShowEulaButton();
		switch (AppInfo.platform)
		{
		case 4:
			paymentHandler = base.gameObject.AddComponent<PaymentCustomWindowKongregate>();
			break;
		case 7:
			paymentHandler = base.gameObject.AddComponent<PaymentCustomWindowSteam>();
			break;
		case 1:
			paymentHandler = base.gameObject.AddComponent<PaymentCustomWindowAndroid>();
			break;
		case 2:
			paymentHandler = base.gameObject.AddComponent<PaymentCustomWindowIOS>();
			break;
		}
		paymentHandler?.SetPaymentCustomWindow(this);
		SetPaymentRef(_paymentRef);
		Util.SetButton(buyBtn, allowPurchase);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_characterSorting != null)
		{
			_characterSorting.sortingOrder = base.sortingLayer + 1;
		}
	}

	private void PurchaseComplete()
	{
		OnClose();
	}

	public void ShowEulaButton()
	{
		eulaBtn.gameObject.SetActive(GameData.instance.PROJECT.character.toCharacterData().isIMXG0);
		eulaBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("eula");
	}

	public void OnEulaBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("eula"), Language.GetString("eula_notice"), base.gameObject);
	}

	public void SetPaymentRef(PaymentRef paymentRef)
	{
		if (paymentRef != null)
		{
			_paymentRef = paymentRef;
			_itemData = paymentRef.itemData;
			itemsList.ClearList();
			_items = null;
			costTxt.text = "";
			DoItemContents();
			topperTxt.text = Language.GetString("ui_vipgor").ToUpperInvariant();
			DoUpdate();
		}
	}

	private void DoItemContents()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_CONTENTS), OnItemContents);
		MerchantDALC.instance.doItemContents(_itemData.itemRef.id, _itemData.itemRef.itemType);
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
		_items = ItemData.listFromSFSObject(sfsob);
		CreateAssets();
		Init();
	}

	private void CreateAssets()
	{
		int totalPower = GameData.instance.PROJECT.character.getTotalPower();
		int totalStamina = GameData.instance.PROJECT.character.getTotalStamina();
		int totalAgility = GameData.instance.PROJECT.character.getTotalAgility();
		int num = totalPower;
		int num2 = totalStamina;
		int num3 = totalAgility;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		Equipment equipment = new Equipment();
		foreach (ItemData item in _items)
		{
			if (item.itemRef.itemType == 1)
			{
				EquipmentRef equipmentRef = item.itemRef as EquipmentRef;
				int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
				if (equipment.getEquipmentSlot(availableSlot) == null)
				{
					equipment.setEquipmentSlot(equipmentRef, availableSlot);
					EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(availableSlot);
					int num7 = ((equipmentSlot != null) ? equipmentSlot.power : 0);
					int num8 = ((equipmentSlot != null) ? equipmentSlot.stamina : 0);
					int num9 = ((equipmentSlot != null) ? equipmentSlot.agility : 0);
					num += equipmentRef.power - num7;
					num2 += equipmentRef.stamina - num8;
					num3 += equipmentRef.agility - num9;
					num4 += equipmentRef.power;
					num5 += equipmentRef.stamina;
					num6 += equipmentRef.agility;
				}
			}
		}
		CharacterPuppetInfoDefault characterPuppetInfoDefault = new CharacterPuppetInfoDefault(CharacterPuppet.Gender.MALE, 0, 0, 0, 1f, 1f, equipment);
		_characterDisplay = GameData.instance.windowGenerator.GetCharacterDisplay(characterPuppetInfoDefault);
		_characterDisplay.SetCharacterDisplay(characterPuppetInfoDefault);
		_characterDisplay.transform.SetParent(placeholderDisplay, worldPositionStays: false);
		CharacterDisplay characterDisplay = _characterDisplay;
		characterDisplay.BOUNDS_UPDATED = (Action)Delegate.Combine(characterDisplay.BOUNDS_UPDATED, (Action)delegate
		{
			CheckAndUpdatePuppetSize();
		});
		_characterDisplay.SetLocalPosition(_characterDisplay.hasMountEquipped() ? new Vector3(0f, -22f, 0f) : new Vector3(0f, -63f, 0f));
		_characterDisplay.transform.localScale.Scale(new Vector3(-1f, 1f, 1f));
		Util.ChangeLayer(_characterDisplay.transform, "UI");
		_characterSorting = _characterDisplay.gameObject.AddComponent<SortingGroup>();
		_characterSorting.sortingLayerName = "UI";
		_characterSorting.sortingOrder = base.sortingLayer + 1;
		_statTiles = new List<ItemTooltipStatTile>();
		CreateStatTile(ItemTooltipStatTile.STAT_TYPE.POWER, num4, num - totalPower);
		CreateStatTile(ItemTooltipStatTile.STAT_TYPE.STAMINA, num5, num2 - totalStamina);
		CreateStatTile(ItemTooltipStatTile.STAT_TYPE.AGILITY, num6, num3 - totalAgility);
		CreateTiles(_items);
	}

	private void CheckAndUpdatePuppetSize()
	{
		if (_characterDisplay.bounds.size.y > 38f)
		{
			_characterDisplay.transform.localScale = Vector2.one * 38f / _characterDisplay.bounds.size.y;
		}
	}

	private void CreateStatTile(ItemTooltipStatTile.STAT_TYPE type, int stat, int compare)
	{
		ItemTooltipStatTile itemTooltipStatTile = UnityEngine.Object.Instantiate(itemTooltipStatTilePrefab);
		itemTooltipStatTile.transform.SetParent(panel.transform, worldPositionStays: false);
		itemTooltipStatTile.transform.localScale = Vector3.one;
		itemTooltipStatTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(placeholderStats.anchoredPosition.x, placeholderStats.anchoredPosition.y - placeholderStats.sizeDelta.y * (float)_statTiles.Count);
		itemTooltipStatTile.LoadDetailsAsAs3(type, stat, compare);
		_statTiles.Add(itemTooltipStatTile);
	}

	private void CreateTiles(List<ItemData> items)
	{
		foreach (ItemData item in items)
		{
			if (item != null)
			{
				itemsList.Data.InsertOneAtEnd(new NBPItem
				{
					itemData = item
				});
			}
		}
	}

	internal virtual void Init()
	{
	}

	public void DoPayment()
	{
		GameData.instance.main.ShowLoading();
		Util.SetButton(buyBtn, enabled: false);
		KongregateAnalytics.trackPaymentStart(_paymentRef.paymentID, KongregateAnalytics.getPaymentGameFields(_paymentRef.getPaymentStatType));
		paymentHandler.doPayment();
	}

	public void OnPaymentFailed()
	{
		GameData.instance.main.HideLoading();
		Util.SetButton(buyBtn);
	}

	public void DoInventoryCheck()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		CharacterDALC.instance.doInventoryCheck();
	}

	private void OnInventoryCheck(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		if (list != null && list.Count > 0)
		{
			GameData.instance.PROJECT.character.updateInventoryItems(list);
		}
		if (GameData.instance.PROJECT.character.getItemQty(_itemData.itemRef) > 0)
		{
			GameData.instance.main.HideLoading();
			if (_itemData.itemRef.itemType == 4)
			{
				ConsumableRef consumableRef = _itemData.itemRef as ConsumableRef;
				_consumableId = consumableRef.id;
				ConsumableManager.instance.SetupConsumable(consumableRef, 1);
				ConsumableManager.instance.DoUseConsumable(1, delegate
				{
					ForceUseVipgor();
				});
			}
		}
		else
		{
			Disable();
			GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 2f, CoroutineTimer.TYPE.SECONDS, DoInventoryCheck);
		}
	}

	private void ForceUseVipgor()
	{
		int num = -1;
		if (_consumableId == 3030)
		{
			num = 3033;
		}
		else if (_consumableId == 3031)
		{
			num = 3034;
		}
		else if (_consumableId == 3032)
		{
			num = 3035;
		}
		if (num != -1)
		{
			_newConsumable = ConsumableBook.Lookup(num);
			ConsumableManager.instance.SetupConsumable(_newConsumable, 1);
			ConsumableManager.instance.DoUseConsumable(1, delegate
			{
				Util.SetButton(buyBtn, allowPurchase);
			});
		}
		_consumableId = -1;
	}

	private void OnInventoryChange()
	{
		DoUpdate();
		if (!allowPurchase)
		{
			OnClose();
		}
	}

	public void SetCost(string cost)
	{
		int num = 1;
		ItemRef itemRef = _itemData.itemRef;
		ShopSaleRef shopSaleRef = ShopBook.LookupVipgor(itemRef.id);
		PaymentData paymentData = itemRef.getPaymentData();
		if (AdGor.devMode && GameData.instance.PROJECT.character.admin)
		{
			paymentData = null;
		}
		bool flag = shopSaleRef != null && paymentData == null && (double)shopSaleRef.mult != 1.0;
		int num2 = ((paymentData == null) ? ((itemRef.costCreditsRaw <= 0) ? 1 : 2) : 0);
		int num3 = ((paymentData == null) ? ((num2 == 2) ? itemRef.costCredits : itemRef.costGold) : 0);
		if (paymentData == null)
		{
			if (num2 == 2)
			{
				_ = itemRef.costCreditsRaw;
			}
			else
				_ = itemRef.costGoldRaw;
		}
		else
			_ = 0;
		num3 *= num;
		goldIcon.gameObject.SetActive(num2 == 1);
		creditsIcon.gameObject.SetActive(num2 == 2);
		if (paymentData != null)
		{
			cost = paymentData.price;
		}
		else
		{
			cost = Util.NumberFormat(num3);
			if (flag)
			{
				cost = Util.ParseString("^" + cost + "^");
			}
		}
		costTxt.text = cost;
		_loaded = true;
		DoUpdate();
	}

	public void OnBuyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (AdGor.devMode && GameData.instance.PROJECT.character.admin)
		{
			ItemRef itemRef = paymentRef.itemData.itemRef;
			_consumableId = itemRef.id;
			TransactionManager.instance.ConfirmItemPurchase(itemRef, null, 1, null, testCheckInventory);
		}
		else
		{
			DoPayment();
		}
	}

	public virtual void DoUpdate()
	{
		if (!allowPurchase)
		{
			Util.SetButton(buyBtn, enabled: false);
		}
		else
		{
			Util.SetButton(buyBtn);
		}
	}

	public override void OnClose()
	{
		CharacterDisplay characterDisplay = _characterDisplay;
		characterDisplay.BOUNDS_UPDATED = (Action)Delegate.Remove(characterDisplay.BOUNDS_UPDATED, (Action)delegate
		{
			CheckAndUpdatePuppetSize();
		});
		buyBtn.onClick.RemoveListener(OnBuyBtn);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		buyBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		buyBtn.interactable = false;
	}

	public void testCheckInventory(TransactionManager.PurchaseObject data)
	{
		ItemRef itemRef = data.itemRef;
		int currencyID = data.currencyID;
		int currencyCost = data.currencyCost;
		int qty = data.qty;
		string context = data.context;
		TransactionManager.instance.DoItemPurchase(itemRef, currencyID, currencyCost, qty, null, null, context);
		Invoke("forceUseVipgor", 1f);
	}

	private void forceUseVipgor()
	{
		int num = -1;
		if (_consumableId == 3030)
		{
			num = 3033;
		}
		else if (_consumableId == 3031)
		{
			num = 3034;
		}
		else if (_consumableId == 3032)
		{
			num = 3035;
		}
		if (num != -1)
		{
			_newConsumable = ConsumableBook.Lookup(num);
			ConsumableManager.instance.SetupConsumable(_newConsumable, 1);
			ConsumableManager.instance.DoUseConsumable(1);
		}
		_consumableId = -1;
	}
}
