using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.battlestatslist;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleStatsWindow : WindowsMain
{
	[HideInInspector]
	public UnityEvent COMPLETE = new UnityEvent();

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI idListTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI teamListTxt;

	public TextMeshProUGUI totalTxt;

	public Image dropdownFilter;

	public BattleStatsList battleStatsList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private List<BattleStat> _battleStats = new List<BattleStat>();

	private List<MyDropdownItemModel> _dropdownObjs = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedFilter;

	private Transform window;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<BattleStat> battleStats)
	{
		_battleStats = battleStats;
		topperTxt.text = Language.GetString("ui_stats");
		nameListTxt.text = Language.GetString("ui_name");
		teamListTxt.text = Language.GetString("ui_team");
		CreateDropDown();
		battleStatsList.InitList(this);
		CreateTiles();
		ListenForBack(OnClose);
		ListenForForward(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 2 + base.sortingLayer + battleStatsList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = base.sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 2 + base.sortingLayer + battleStatsList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = base.sortingLayer;
		battleStatsList.Refresh();
	}

	private void CreateDropDown()
	{
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 0,
			title = Language.GetString("ui_battle_stats_filter_damage_done"),
			data = "ui_battle_stats_filter_damage_done"
		});
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 1,
			title = Language.GetString("ui_battle_stats_filter_damage_taken"),
			data = "ui_battle_stats_filter_damage_taken"
		});
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 2,
			title = Language.GetString("ui_battle_stats_filter_healing_done"),
			data = "ui_battle_stats_filter_healing_done"
		});
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 3,
			title = Language.GetString("ui_battle_stats_filter_healing_taken"),
			data = "ui_battle_stats_filter_healing_taken"
		});
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 4,
			title = Language.GetString("ui_battle_stats_filter_damage_block"),
			data = "ui_battle_stats_filter_damage_block"
		});
		_dropdownObjs.Add(new MyDropdownItemModel
		{
			id = 5,
			title = Language.GetString("ui_battle_stats_filter_shielding"),
			data = "ui_battle_stats_filter_shielding"
		});
		_selectedFilter = _dropdownObjs[0];
		dropdownFilter.GetComponentInChildren<TextMeshProUGUI>().text = _selectedFilter.title;
	}

	public void OnFilterDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_battle_stats"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedFilter.id, OnDropdownChange);
		componentInChildren.Data.InsertItemsAtEnd(_dropdownObjs);
	}

	private void OnDropdownChange(MyDropdownItemModel model)
	{
		_selectedFilter = model;
		dropdownFilter.GetComponentInChildren<TextMeshProUGUI>().text = _selectedFilter.title;
		CreateTiles();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateTiles()
	{
		battleStatsList.ClearList();
		int num = 0;
		foreach (BattleStat battleStat in _battleStats)
		{
			if (battleStat != null)
			{
				switch (_selectedFilter.data as string)
				{
				case "ui_battle_stats_filter_damage_done":
					battleStat.value = battleStat.damageDone;
					break;
				case "ui_battle_stats_filter_damage_taken":
					battleStat.value = battleStat.damageTaken;
					break;
				case "ui_battle_stats_filter_healing_done":
					battleStat.value = battleStat.healingDone;
					break;
				case "ui_battle_stats_filter_healing_taken":
					battleStat.value = battleStat.healingTaken;
					break;
				case "ui_battle_stats_filter_damage_block":
					battleStat.value = battleStat.damageBlocked;
					break;
				case "ui_battle_stats_filter_shielding":
					battleStat.value = battleStat.shielding;
					break;
				default:
					battleStat.value = -1;
					break;
				}
				if (num < battleStat.value)
				{
					num = battleStat.value;
				}
			}
		}
		List<BattleStat> list = Util.SortVector(_battleStats, new string[1] { "value" }, Util.ARRAY_DESCENDING);
		List<BattleStatsItem> list2 = new List<BattleStatsItem>();
		foreach (BattleStat item2 in list)
		{
			if (item2 != null)
			{
				item2.max = num;
				BattleStatsItem item = new BattleStatsItem
				{
					data = item2,
					filter = (_selectedFilter.data?.ToString() ?? "")
				};
				list2.Add(item);
			}
		}
		battleStatsList.Data.InsertItemsAtEnd(list2);
	}

	public override void OnClose()
	{
		if (!base.disabled && base.gameObject.activeSelf)
		{
			base.OnClose();
			COMPLETE.Invoke();
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		dropdownFilter.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		dropdownFilter.GetComponent<EventTrigger>().enabled = false;
	}
}
