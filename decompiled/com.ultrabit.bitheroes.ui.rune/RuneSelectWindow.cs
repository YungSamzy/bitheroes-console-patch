using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.runeslist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rune;

public class RuneSelectWindow : WindowsMain
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

	public Image sortDropdown;

	private Transform dropdownWindow;

	private RunesList _runesList;

	private MyDropdownItemModel _selectedFilter;

	private List<MyDropdownItemModel> _filters = new List<MyDropdownItemModel>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = ItemRef.GetItemNamePlural(9);
		_runesList = GetComponentInChildren<RunesList>();
		_runesList.InitList();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		PrepareDropdown();
		CreateTiles();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnInventoryChange()
	{
		CreateTiles();
	}

	private void PrepareDropdown()
	{
		_filters.Clear();
		_selectedFilter = new MyDropdownItemModel
		{
			id = 0,
			title = Language.GetString("ui_all")
		};
		_filters.Add(_selectedFilter);
		foreach (KeyValuePair<string, int> rUNE_TYPE in RuneRef.RUNE_TYPES)
		{
			_filters.Add(new MyDropdownItemModel
			{
				id = rUNE_TYPE.Value,
				title = Util.FirstCharToUpper(rUNE_TYPE.Key)
			});
		}
		dropdownLabel.text = _selectedFilter.title;
	}

	public void OnSortDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_filter"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedFilter.id, OnSortSelected);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtEnd(_filters);
	}

	private void OnSortSelected(MyDropdownItemModel model)
	{
		_selectedFilter = model;
		dropdownLabel.text = _selectedFilter.title;
		CreateTiles();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateTiles()
	{
		_runesList.ClearList();
		List<RunesListItem> list = new List<RunesListItem>();
		foreach (ItemData item in (from item in GameData.instance.PROJECT.character.inventory.GetItemsByType(9)
			orderby item.itemRef.name, item.rarity descending
			select item).ToList())
		{
			if (item == null)
			{
				continue;
			}
			if (_selectedFilter.id > 0)
			{
				if ((item.itemRef as RuneRef).runeType == _selectedFilter.id)
				{
					list.Add(new RunesListItem
					{
						itemData = item
					});
				}
			}
			else
			{
				list.Add(new RunesListItem
				{
					itemData = item
				});
			}
		}
		_runesList.Data.InsertItemsAtEnd(list);
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
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.DoDestroy();
	}
}
