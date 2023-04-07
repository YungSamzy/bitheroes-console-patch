using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.characterinventorylist;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.equipment;

public class EquipmentWindow : WindowsMain
{
	public const int SORT_TOTAL = 0;

	public const int SORT_TIER = 1;

	public const int SORT_POWER = 2;

	public const int SORT_STAMINA = 3;

	public const int SORT_AGILITY = 4;

	public const int SORT_RARITY = 5;

	public const int SORT_NAME = 6;

	private static List<string> SORT_NAMES = new List<string>();

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI sortTxt;

	public TextMeshProUGUI dropTxt;

	public Button cosmeticBtn;

	public Button leftBtn;

	public Button rightBtn;

	public Button upgradeBtn;

	public Button reforgeBtn;

	public GameObject equipmentTile;

	public TMP_InputField searchTxt;

	public CharacterInventoryList itemList;

	public Image sortDropdown;

	private int _slot;

	private int _equipmentType;

	private List<EquipmentRef> _slots;

	private List<ItemData> currentItems;

	private Transform window;

	private int selectedSortOption;

	private EquipmentRef selectedIdEquip;

	private List<(int, bool)> _slotsAvailable;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int slot)
	{
		_slot = slot;
		_slotsAvailable = CharacterEquipmentPanel.SlotsAvailable;
		SORT_NAMES.Clear();
		SORT_NAMES.Add("ui_best");
		SORT_NAMES.Add("ui_tier");
		SORT_NAMES.Add("stat_power");
		SORT_NAMES.Add("stat_stamina");
		SORT_NAMES.Add("stat_agility");
		SORT_NAMES.Add("ui_rarity");
		SORT_NAMES.Add("ui_name");
		_slots = new List<EquipmentRef>();
		foreach (EquipmentRef value in GameData.instance.PROJECT.character.equipment.equipmentSlots.Values)
		{
			_slots.Add(value);
		}
		sortTxt.text = Language.GetString("ui_sort") + ":";
		dropTxt.text = Language.GetString(SORT_NAMES[selectedSortOption]);
		searchTxt.text = "";
		itemList.StartList();
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		reforgeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_reforge");
		Util.SetButton(upgradeBtn, enabled: false);
		Util.SetButton(reforgeBtn, enabled: false);
		Util.SetButton(cosmeticBtn, enabled: false);
		SetSlot(slot);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("pet_equip");
		if (tutorialRef != null && _slot == 7)
		{
			if (!GameData.instance.PROJECT.character.tutorial.GetState(78) && GameData.instance.PROJECT.character.tutorial.GetState(77) && tutorialRef.areConditionsMet)
			{
				if (GameData.instance.PROJECT.character.equipment.getEquipmentSlot(7) != null)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(78);
					CheckTutorial();
					return;
				}
				if (itemList.Content.childCount == 0)
				{
					return;
				}
				Button button = ((itemList.Content.GetChild(0).childCount > 0) ? itemList.Content.GetChild(0).GetChild(0).GetComponent<Button>() : null);
				if (button != null)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(78);
					GameData.instance.tutorialManager.ShowTutorialForButton(button.gameObject, new TutorialPopUpSettings(Tutorial.GetText(78), 4, button.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
					return;
				}
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(79) && GameData.instance.PROJECT.character.tutorial.GetState(78) && tutorialRef.areConditionsMet && upgradeBtn != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(79);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(upgradeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(79), 1, upgradeBtn.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("accessory_equip");
		if (tutorialRef2 != null && _slot == 6)
		{
			if (!GameData.instance.PROJECT.character.tutorial.GetState(82) && GameData.instance.PROJECT.character.tutorial.GetState(81) && tutorialRef2.areConditionsMet)
			{
				if (GameData.instance.PROJECT.character.equipment.getEquipmentSlot(6) != null)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(82);
					CheckTutorial();
					return;
				}
				if (itemList.Content.childCount == 0)
				{
					return;
				}
				Button button2 = ((itemList.Content.GetChild(0).childCount > 0) ? itemList.Content.GetChild(0).GetChild(0).GetComponent<Button>() : null);
				if (button2 != null)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(82);
					GameData.instance.tutorialManager.ShowTutorialForButton(button2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(82), 4, button2.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
					return;
				}
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(83) && GameData.instance.PROJECT.character.tutorial.GetState(82) && tutorialRef2.areConditionsMet && upgradeBtn != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(83);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(upgradeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(83), 1, upgradeBtn.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(104) && !GameData.instance.PROJECT.character.tutorial.GetState(105) && upgradeBtn != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(105);
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(upgradeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(105), 1, upgradeBtn.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void OnEquipmentChange()
	{
		StartCoroutine(OnTimerCompleteWaitTimer());
	}

	private IEnumerator OnTimerCompleteWaitTimer()
	{
		yield return new WaitForSeconds(0.05f);
		CreateTiles(sort: true, scrollTo: true);
		CheckTutorial();
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		CreateTiles(sort: false);
	}

	public void OnCosmeticsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(_slot);
		if (equipmentSlot == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_equipment_cosmetic"));
			return;
		}
		OnClose();
		GameData.instance.windowGenerator.NewCosmeticsWindow(equipmentSlot);
	}

	public void OnLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int slotToGo = ((_slot > 0) ? (_slot - 1) : 7);
		CheckSlotAvailability(ref slotToGo, isGoingRight: false);
		SetSlot(slotToGo);
	}

	public void OnRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int slotToGo = ((_slot < 7) ? (_slot + 1) : 0);
		CheckSlotAvailability(ref slotToGo, isGoingRight: true);
		SetSlot(slotToGo);
	}

	private void CheckSlotAvailability(ref int slotToGo, bool isGoingRight)
	{
		foreach (var item in _slotsAvailable)
		{
			if (item.Item1 != slotToGo)
			{
				continue;
			}
			if (item.Item2)
			{
				break;
			}
			if (isGoingRight)
			{
				if (slotToGo >= 7)
				{
					slotToGo = 0;
				}
				else
				{
					slotToGo++;
				}
			}
			else if (slotToGo <= 0)
			{
				slotToGo = 7;
			}
			else
			{
				slotToGo--;
			}
			CheckSlotAvailability(ref slotToGo, isGoingRight);
		}
	}

	public void OnSortDropdown()
	{
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, selectedSortOption, OnSortDropdownClick);
		componentInChildren.ClearList();
		for (int i = 0; i < SORT_NAMES.Count; i++)
		{
			int id = i;
			string @string = Language.GetString(SORT_NAMES[i]);
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = id,
				title = @string
			});
		}
	}

	public void OnSortDropdownClick(MyDropdownItemModel model)
	{
		selectedSortOption = model.id;
		dropTxt.text = Language.GetString(SORT_NAMES[selectedSortOption]);
		CreateTiles(sort: true);
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnUpgradeBtn()
	{
		if (selectedIdEquip != null)
		{
			GameData.instance.windowGenerator.NewEquipmentUpgradeWindow(selectedIdEquip, base.gameObject, null);
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
		}
	}

	public void OnReforgeBtn()
	{
		if (selectedIdEquip != null)
		{
			GameData.instance.windowGenerator.NewItemReforgeWindow(selectedIdEquip, base.gameObject);
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
		}
	}

	private void SetSlot(int slot)
	{
		_slot = slot;
		_equipmentType = Equipment.getSlotEquipmentType(slot);
		topperTxt.text = Equipment.getSlotName(_slot);
		int slot2 = _slot;
		if ((uint)slot2 <= 3u || (uint)(slot2 - 6) <= 1u)
		{
			cosmeticBtn.gameObject.SetActive(value: true);
		}
		else
		{
			cosmeticBtn.gameObject.SetActive(value: false);
		}
		CreateTiles(sort: true);
	}

	private void GetItems()
	{
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(1);
		currentItems = new List<ItemData>();
		foreach (ItemData item in itemsByType)
		{
			if (item.qty > 0 && (item.itemRef as EquipmentRef).equipmentType == _equipmentType)
			{
				currentItems.Add(item);
			}
		}
	}

	private void SortItems()
	{
		switch (selectedSortOption)
		{
		case 0:
			currentItems = (from item in currentItems
				orderby item.total descending, item.rarity descending
				select item).ToList();
			break;
		case 1:
			currentItems = (from item in currentItems
				orderby item.tier descending, item.total descending, item.rarity descending
				select item).ToList();
			break;
		case 2:
			currentItems = (from item in currentItems
				orderby item.power descending, item.total descending, item.rarity descending
				select item).ToList();
			break;
		case 3:
			currentItems = (from item in currentItems
				orderby item.stamina descending, item.total descending, item.rarity descending
				select item).ToList();
			break;
		case 4:
			currentItems = (from item in currentItems
				orderby item.agility descending, item.total descending, item.rarity descending
				select item).ToList();
			break;
		case 5:
			currentItems = (from item in currentItems
				orderby item.rarity descending, item.total descending
				select item).ToList();
			break;
		case 6:
			currentItems = currentItems.OrderByDescending((ItemData item) => item.itemRef.name).ToList();
			break;
		}
	}

	private bool MatchSearchCriteria(string name)
	{
		if (name != null && !(searchTxt == null) && searchTxt.text != null && !searchTxt.text.Trim().Equals(""))
		{
			return name.ToLower().Contains(searchTxt.text.ToLower());
		}
		return true;
	}

	private void CreateTiles(bool sort, bool scrollTo = false)
	{
		GetItems();
		if (sort)
		{
			SortItems();
		}
		double virtualAbstractNormalizedScrollPosition = itemList.GetVirtualAbstractNormalizedScrollPosition();
		itemList.ClearList();
		setEquipedItem();
		List<CharacterInventoryListItem> list = new List<CharacterInventoryListItem>();
		foreach (ItemData currentItem in currentItems)
		{
			if (MatchSearchCriteria(currentItem.itemRef.name))
			{
				list.Add(new CharacterInventoryListItem
				{
					itemData = currentItem
				});
				if (scrollTo)
				{
					_ = currentItem.itemRef;
				}
			}
		}
		itemList.Data.InsertItems(0, list);
		if (scrollTo)
		{
			itemList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		cosmeticBtn.interactable = true;
		leftBtn.interactable = true;
		rightBtn.interactable = true;
		searchTxt.interactable = true;
		upgradeBtn.interactable = true;
		reforgeBtn.interactable = true;
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		cosmeticBtn.interactable = false;
		leftBtn.interactable = false;
		rightBtn.interactable = false;
		searchTxt.interactable = false;
		upgradeBtn.interactable = false;
		reforgeBtn.interactable = false;
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}

	public void OnDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
	}

	private void setEquipedItem()
	{
		EquipmentRef equipmentRef = (selectedIdEquip = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(_slot));
		ItemIcon itemIcon = equipmentTile.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = equipmentTile.AddComponent<ItemIcon>();
		}
		itemIcon.SetEquipmentData(new ItemData(equipmentRef, 0), GameData.instance.PROJECT.character.equipment.getCosmeticSlot(_slot));
		if (equipmentRef != null)
		{
			Util.SetButton(upgradeBtn, equipmentRef.hasUpgrade());
			bool itemLocked = GameData.instance.PROJECT.character.getItemLocked(equipmentRef);
			List<ItemRef> reforgeableItems = equipmentRef.getReforgeableItems();
			Util.SetButton(reforgeBtn, reforgeableItems.Count >= 1 && !itemLocked);
			Util.SetButton(cosmeticBtn);
		}
		else
		{
			Util.SetButton(upgradeBtn, enabled: false);
			Util.SetButton(reforgeBtn, enabled: false);
			Util.SetButton(cosmeticBtn, enabled: false);
		}
	}
}
