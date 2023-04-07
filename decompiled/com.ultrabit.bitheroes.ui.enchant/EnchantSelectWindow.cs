using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.enchantslist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantSelectWindow : WindowsMain
{
	public const int SORT_STATS = 0;

	public const int SORT_POWER = 1;

	public const int SORT_STAMINA = 2;

	public const int SORT_AGILITY = 3;

	public const int SORT_RARITY = 4;

	public const int SORT_TIER = 5;

	private static Dictionary<int, string> SORT_NAMES = new Dictionary<int, string>
	{
		{ 0, "ui_stats" },
		{ 1, "stat_power" },
		{ 2, "stat_stamina" },
		{ 3, "stat_agility" },
		{ 4, "ui_rarity" },
		{ 5, "ui_tier" }
	};

	public TextMeshProUGUI dropdownLabel;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public Image sortDropdown;

	private Enchants _enchants;

	private bool _changeable;

	private bool _isArmory;

	private int _slot;

	private Transform dropdownWindow;

	private EnchantsList enchantsList;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = ItemRef.GetItemNamePlural(11);
		int enchantSelectSort = GameData.instance.SAVE_STATE.GetEnchantSelectSort(GameData.instance.PROJECT.character.id);
		string link = (SORT_NAMES.ContainsKey(enchantSelectSort) ? SORT_NAMES[enchantSelectSort] : "ui_sort");
		dropdownLabel.text = Language.GetString(link);
	}

	public void LoadDetails(Enchants enchants, bool changeable = false, int slot = -1, bool isArmory = false)
	{
		_enchants = enchants;
		_changeable = changeable;
		_isArmory = isArmory;
		_slot = slot;
		UpdateText();
		enchantsList = GetComponentInChildren<EnchantsList>();
		enchantsList.InitList(OnEnchantWindowSelected);
		CreateTiles();
		if (_isArmory)
		{
			GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.OnChange.AddListener(OnEnchantsChange);
		}
		else if (_changeable)
		{
			GameData.instance.PROJECT.character.AddListener("ENCHANTS_CHANGE", OnEnchantsChange);
			GameData.instance.PROJECT.character.enchants.OnChange.AddListener(OnEnchantsChange);
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnEnchantsChange()
	{
		CreateTiles();
	}

	public void OnSortDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetEnchantSelectSort(GameData.instance.PROJECT.character.id), OnSortSelected);
		componentInChildren.ClearList();
		foreach (KeyValuePair<int, string> sORT_NAME in SORT_NAMES)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = sORT_NAME.Key,
				title = Language.GetString(sORT_NAME.Value)
			});
		}
	}

	private void OnSortSelected(MyDropdownItemModel model)
	{
		GameData.instance.SAVE_STATE.SetEnchantSelectSort(model.id);
		dropdownLabel.text = model.title;
		CreateTiles();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void UpdateText()
	{
		countTxt.text = Util.NumberFormat(_enchants.enchants.Count) + "/" + Util.NumberFormat(VariableBook.enchantMax);
	}

	private void CreateTiles()
	{
		enchantsList.ClearList();
		List<EnchantsListItem> list = new List<EnchantsListItem>();
		List<EnchantData> list2;
		EnchantData slot;
		if (_isArmory)
		{
			list2 = Util.SortVector(GameData.instance.PROJECT.character.enchants.enchants, GetSortingProperties(), Util.ARRAY_DESCENDING);
			slot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.getSlot(_slot);
		}
		else
		{
			list2 = Util.SortVector(_enchants.enchants, GetSortingProperties(), Util.ARRAY_DESCENDING);
			slot = GameData.instance.PROJECT.character.enchants.getSlot(_slot);
		}
		foreach (EnchantData item in list2)
		{
			item.slot = _slot;
			if (_isArmory)
			{
				item.enchantRef.OverrideItemType(18);
			}
			else
			{
				item.enchantRef.OverrideItemType(11);
			}
			list.Add(new EnchantsListItem
			{
				enchantData = item,
				isEquipped = (slot == item),
				isArmory = _isArmory
			});
		}
		enchantsList.Data.InsertItemsAtEnd(list);
		UpdateText();
	}

	private string[] GetSortingProperties()
	{
		return GameData.instance.SAVE_STATE.GetEnchantSelectSort(GameData.instance.PROJECT.character.id) switch
		{
			0 => new string[3] { "total", "rarity", "tier" }, 
			1 => new string[4] { "power", "total", "rarity", "tier" }, 
			2 => new string[4] { "stamina", "total", "rarity", "tier" }, 
			3 => new string[4] { "agility", "total", "rarity", "tier" }, 
			4 => new string[3] { "rarity", "total", "tier" }, 
			5 => new string[3] { "tier", "total", "rarity" }, 
			_ => null, 
		};
	}

	private void OnEnchantWindowSelected(BaseModelData itemData)
	{
		GameData.instance.windowGenerator.enchantsWindow.OnChangeEnchantSelected(_slot, itemData as EnchantData);
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}

	public override void DoDestroy()
	{
		if (_isArmory)
		{
			GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.OnChange.RemoveListener(OnEnchantsChange);
		}
		else if (_changeable)
		{
			GameData.instance.PROJECT.character.RemoveListener("ENCHANTS_CHANGE", OnEnchantsChange);
			GameData.instance.PROJECT.character.enchants.OnChange.RemoveListener(OnEnchantsChange);
		}
		base.DoDestroy();
	}
}
