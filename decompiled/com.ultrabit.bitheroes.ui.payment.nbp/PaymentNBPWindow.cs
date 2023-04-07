using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
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

namespace com.ultrabit.bitheroes.ui.payment.nbp;

public class PaymentNBPWindow : WindowsMain
{
	private const float DISPLAY_OFFSET = -63f;

	private const float DISPLAY_MOUNT_OFFSET = -22f;

	private const float PUPPET_MAX_WORLD_Y_SIZE = 38f;

	private const float PUPPET_SCALE = 1f;

	private const float PUPPET_MOUNT_SCALE = 0.7f;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI timeTxt;

	public TimeBarColor timeBar;

	public RectTransform placeholderDisplay;

	public RectTransform placeholderStats;

	public Button buyBtn;

	public Button arrowLeftBtn;

	public Button arrowRightBtn;

	[SerializeField]
	private Button eulaBtn;

	public TextMeshProUGUI starsTxt;

	public Image bg_stars;

	public NBPItemsList nbpItemsList;

	public ItemTooltipStatTile itemTooltipStatTilePrefab;

	private PaymentRef _paymentRef;

	private List<PaymentRef> _paymentsNBPZ;

	private ItemData _itemData;

	private bool _loaded;

	private List<ItemData> _items;

	private List<ItemTooltipStatTile> _statTiles;

	private int _actualPaymentRef;

	private SortingGroup _characterSorting;

	private CharacterDisplay _characterDisplay;

	private PaymentNBPWindowHandler paymentHandler;

	private Dictionary<int, BoosterObject> _paymentsByBooster;

	private bool allowPurchase
	{
		get
		{
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
	}

	protected virtual void Init()
	{
	}

	protected void LoadDetails(PaymentRef paymentRef, List<PaymentRef> paymentsNBPZ = null, int itemIndex = 0)
	{
		Disable();
		_actualPaymentRef = itemIndex;
		_paymentRef = ((paymentsNBPZ != null) ? paymentsNBPZ[_actualPaymentRef] : paymentRef);
		_itemData = _paymentRef.itemData;
		_paymentsNBPZ = paymentsNBPZ;
		buyBtn.onClick.AddListener(OnBuyBtn);
		arrowLeftBtn.onClick.AddListener(OnArrowLeftBtn);
		arrowRightBtn.onClick.AddListener(OnArrowRightBtn);
		buyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_buy");
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		ShowEulaButton();
		switch (AppInfo.platform)
		{
		case 4:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowKongregate>();
			break;
		case 7:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowSteam>();
			break;
		case 1:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowAndroid>();
			break;
		case 2:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowIOS>();
			break;
		}
		paymentHandler?.SetPaymentNBPWindow(this);
		nbpItemsList.InitList();
		SetPaymentRef(_paymentRef);
		ListenForBack(OnClose);
		CreateWindow();
	}

	protected void LoadDetailsBoosters(List<BoosterRef> boosters, int itemIndex = 0)
	{
		Disable();
		_paymentsNBPZ = new List<PaymentRef>();
		_paymentsByBooster = new Dictionary<int, BoosterObject>();
		int num = 0;
		foreach (BoosterRef booster in boosters)
		{
			foreach (ItemData item in booster.items)
			{
				PaymentRef paymentRefForBooster = item.itemRef.getPaymentRefForBooster();
				_paymentsNBPZ.Add(paymentRefForBooster);
				_paymentsByBooster.Add(num, new BoosterObject(booster, paymentRefForBooster));
				num++;
			}
		}
		_actualPaymentRef = itemIndex;
		_paymentRef = ((_paymentsNBPZ != null) ? _paymentsNBPZ[_actualPaymentRef] : null);
		_itemData = _paymentRef.itemData;
		buyBtn.onClick.AddListener(OnBuyBtn);
		arrowLeftBtn.onClick.AddListener(OnArrowLeftBtn);
		arrowRightBtn.onClick.AddListener(OnArrowRightBtn);
		buyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_buy");
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		ShowEulaButton();
		switch (AppInfo.platform)
		{
		case 4:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowKongregate>();
			break;
		case 7:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowSteam>();
			break;
		case 1:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowAndroid>();
			break;
		case 2:
			paymentHandler = base.gameObject.AddComponent<PaymentNBPWindowIOS>();
			break;
		}
		paymentHandler?.SetPaymentNBPWindow(this);
		nbpItemsList.InitList();
		SetPaymentRef(_paymentRef, _actualPaymentRef);
		GameData.instance.PROJECT.character.BOOSTER_CHANGED.AddListener(OnBoostersChanged);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnBoostersChanged(object e)
	{
		ReopenWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (_characterSorting != null)
		{
			_characterSorting.sortingOrder = base.sortingLayer + 1;
		}
	}

	public void SetPaymentRef(PaymentRef paymentRef, int i = 0)
	{
		if (paymentRef == null)
		{
			return;
		}
		_paymentRef = paymentRef;
		_itemData = paymentRef.itemData;
		RemoveTiles();
		_items = null;
		starsTxt.gameObject.SetActive(_paymentsNBPZ != null);
		bg_stars.gameObject.SetActive(_paymentsNBPZ != null);
		if (_paymentsNBPZ == null)
		{
			arrowLeftBtn.gameObject.SetActive(value: false);
			arrowRightBtn.gameObject.SetActive(value: false);
		}
		else
		{
			if (_actualPaymentRef != 0 && _paymentsNBPZ.Count > 1)
			{
				Util.SetButton(arrowLeftBtn);
			}
			else
			{
				Util.SetButton(arrowLeftBtn, enabled: false);
			}
			if (_actualPaymentRef != _paymentsNBPZ.Count - 1 && _paymentsNBPZ.Count > 1)
			{
				Util.SetButton(arrowRightBtn);
			}
			else
			{
				Util.SetButton(arrowRightBtn, enabled: false);
			}
		}
		costTxt.text = "";
		descTxt.text = Util.ParseString(paymentRef.desc);
		DoItemContents();
		timeBar.transform.parent.gameObject.SetActive(value: true);
		if (_paymentsNBPZ == null)
		{
			topperTxt.text = Util.ParseString(paymentRef.name);
			timeTxt.gameObject.SetActive(value: false);
			timeBar.SetMaxValueMilliseconds(VariableBook.nbpMilliseconds);
			timeBar.SetCurrentValueMilliseconds(GameData.instance.PROJECT.character.nbpMilliseconds);
			timeBar.ForceStart();
			timeBar.COMPLETE.AddListener(OnTimerComplete);
		}
		else
		{
			topperTxt.text = Util.ParseString(paymentRef.name);
			starsTxt.text = _actualPaymentRef + 1 + "/" + _paymentsNBPZ.Count;
			if (_paymentsByBooster != null)
			{
				BoosterRef boosterRef = _paymentsByBooster[i].boosterRef;
				if (boosterRef.endDate.HasValue)
				{
					timeTxt.gameObject.SetActive(value: false);
					timeBar.SetByRealEndTime(boosterRef.endDate.Value, boosterRef.startDate);
					timeBar.ForceStart();
				}
				else if (paymentRef.box != null)
				{
					timeBar.gameObject.SetActive(value: false);
					timeBar.ShowText(status: false);
					starsTxt.text = _actualPaymentRef + 1 + "/" + _paymentsNBPZ.Count;
					timeTxt.text = Util.ParseString(paymentRef.box);
				}
				else
				{
					timeBar.transform.parent.gameObject.SetActive(value: false);
					timeTxt.gameObject.SetActive(value: false);
				}
			}
			else
			{
				timeBar.gameObject.SetActive(value: false);
				timeBar.ShowText(status: false);
				starsTxt.text = _actualPaymentRef + 1 + "/" + _paymentsNBPZ.Count;
				timeTxt.text = Util.ParseString(paymentRef.box);
			}
		}
		DoUpdate();
	}

	private void OnTimerComplete()
	{
		GameData.instance.PROJECT.character.Broadcast("NBP_DATE_CHANGE");
		OnClose();
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
		Mounts mounts = null;
		foreach (ItemData item2 in _items)
		{
			switch (item2.itemRef.itemType)
			{
			case 1:
			{
				EquipmentRef equipmentRef = item2.itemRef as EquipmentRef;
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
				break;
			}
			case 8:
			{
				MountRef mountRef = item2.itemRef as MountRef;
				MountData item = new MountData(mountRef.id, mountRef, 0f, 0f, 0f, null, 0, 0);
				List<MountData> list = new List<MountData>();
				list.Add(item);
				mounts = new Mounts(mountRef.id, null, list);
				break;
			}
			}
		}
		CharacterPuppetInfoDefault characterPuppetInfoDefault = new CharacterPuppetInfoDefault(CharacterPuppet.Gender.MALE, 0, 0, 0, equipment: equipment, mounts: mounts, scale: (mounts != null) ? 0.7f : 1f, headScale: 1f, showHelm: true, showMount: true);
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
				nbpItemsList.Data.InsertOneAtEnd(new NBPItem
				{
					itemData = item
				});
			}
		}
	}

	private void RemoveTiles()
	{
		nbpItemsList.ClearList();
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
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
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
			if (_itemData.itemRef.itemType == 4)
			{
				ConsumableRef consumableRef = _itemData.itemRef as ConsumableRef;
				ConsumableManager.instance.SetupConsumable(consumableRef, 1);
				ConsumableManager.instance.DoUseConsumable();
			}
		}
		else
		{
			Disable();
			GameData.instance.main.ShowLoading();
			GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 2f, CoroutineTimer.TYPE.SECONDS, DoInventoryCheck);
		}
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
		costTxt.text = Util.ParseString(cost);
		_loaded = true;
		DoUpdate();
	}

	public void OnBuyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoPayment();
	}

	public void OnArrowLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDestroy();
		if (_paymentsByBooster != null)
		{
			GameData.instance.windowGenerator.ShowBoosters(GetPreviousPaymentIndex());
		}
		else
		{
			GameData.instance.windowGenerator.ShowNBPZ(GetPreviousPaymentIndex());
		}
	}

	public void OnArrowRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDestroy();
		if (_paymentsByBooster != null)
		{
			GameData.instance.windowGenerator.ShowBoosters(GetNextPaymentIndex());
		}
		else
		{
			GameData.instance.windowGenerator.ShowNBPZ(GetNextPaymentIndex());
		}
	}

	private void ReopenWindow()
	{
		DoDestroy();
		GameData.instance.windowGenerator.ShowBoosters();
	}

	private PaymentRef GetPreviousPayment()
	{
		if (_paymentsNBPZ == null)
		{
			return null;
		}
		if (_paymentsNBPZ.Count <= 0)
		{
			return null;
		}
		return _paymentsNBPZ[GetPreviousPaymentIndex()];
	}

	private PaymentRef GetNextPayment()
	{
		if (_paymentsNBPZ == null)
		{
			return null;
		}
		return _paymentsNBPZ[GetNextPaymentIndex()];
	}

	private int GetPreviousPaymentIndex()
	{
		_actualPaymentRef--;
		if (_actualPaymentRef < 0)
		{
			_actualPaymentRef = _paymentsNBPZ.Count - 1;
		}
		return _actualPaymentRef;
	}

	private int GetNextPaymentIndex()
	{
		_actualPaymentRef++;
		if (_actualPaymentRef == _paymentsNBPZ.Count)
		{
			_actualPaymentRef = 0;
		}
		return _actualPaymentRef;
	}

	public void DoUpdate()
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

	public override void DoDestroy()
	{
		buyBtn.onClick.RemoveListener(OnBuyBtn);
		arrowLeftBtn.onClick.RemoveListener(OnArrowLeftBtn);
		arrowRightBtn.onClick.RemoveListener(OnArrowRightBtn);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		timeBar.COMPLETE.RemoveListener(OnTimerComplete);
		if (_paymentsByBooster != null)
		{
			GameData.instance.PROJECT.character.BOOSTER_CHANGED.AddListener(OnBoostersChanged);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		buyBtn.interactable = true;
		arrowLeftBtn.interactable = true;
		arrowRightBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		buyBtn.interactable = false;
		arrowLeftBtn.interactable = false;
		arrowRightBtn.interactable = false;
	}
}
