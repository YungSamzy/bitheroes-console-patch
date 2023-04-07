using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.characterinventorylist;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class ArmoryEquipmentWindow : WindowsMain
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

	public TMP_InputField searchTxt;

	public CharacterInventoryList itemList;

	public Image sortDropdown;

	private int _slot;

	private int _equipmentType;

	private List<ItemData> currentItems;

	private Transform window;

	private int selectedSortOption;

	public override void Start()
	{
		base.Start();
		Disable();
		GameData.instance.PROJECT.character.AddListener("armoryEquipmentChange", OnArmoryEquipmentChange);
	}

	private void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("armoryEquipmentChange", OnArmoryEquipmentChange);
	}

	private void OnArmoryEquipmentChange()
	{
		UpdateTiles(scrollTo: true);
	}

	public void LoadDetails(int slot)
	{
		_slot = slot;
		SORT_NAMES.Clear();
		SORT_NAMES.Add("ui_best");
		SORT_NAMES.Add("ui_tier");
		SORT_NAMES.Add("stat_power");
		SORT_NAMES.Add("stat_stamina");
		SORT_NAMES.Add("stat_agility");
		SORT_NAMES.Add("ui_rarity");
		SORT_NAMES.Add("ui_name");
		sortTxt.text = Language.GetString("ui_sort") + ":";
		dropTxt.text = Language.GetString(SORT_NAMES[selectedSortOption]);
		searchTxt.text = "";
		itemList.StartList();
		SetSlot(slot);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnEquipmentChange()
	{
		CreateTiles(sort: true);
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
		EquipmentRef armoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(_slot);
		if (armoryEquipmentSlot == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_equipment_cosmetic"));
			return;
		}
		ArmoryRef equipmentRef = ArmoryRef.EquipmentRefToArmoryRef(armoryEquipmentSlot);
		OnClose();
		GameData.instance.windowGenerator.NewArmoryCosmeticsWindow(equipmentRef);
	}

	public void OnLeftBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_slot <= 0)
		{
			SetSlot(7);
		}
		else
		{
			SetSlot(_slot - 1);
		}
	}

	public void OnRightBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_slot >= 7)
		{
			SetSlot(0);
		}
		else
		{
			SetSlot(_slot + 1);
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
		GetItems();
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
		if (sort)
		{
			SortItems();
		}
		double virtualAbstractNormalizedScrollPosition = itemList.GetVirtualAbstractNormalizedScrollPosition();
		itemList.ClearList();
		List<CharacterInventoryListItem> list = new List<CharacterInventoryListItem>();
		foreach (ItemData currentItem in currentItems)
		{
			if (MatchSearchCriteria(currentItem.itemRef.name))
			{
				currentItem.itemRef.OverrideItemType(16);
				list.Add(new CharacterInventoryListItem
				{
					itemData = currentItem
				});
			}
		}
		itemList.Data.InsertItems(0, list);
		if (scrollTo)
		{
			itemList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
		}
	}

	private void UpdateTiles(bool scrollTo = false)
	{
		CreateTiles(sort: false, scrollTo);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		cosmeticBtn.interactable = true;
		leftBtn.interactable = true;
		rightBtn.interactable = true;
		searchTxt.interactable = true;
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
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
