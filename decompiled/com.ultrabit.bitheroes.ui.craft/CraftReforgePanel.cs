using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.craftlist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftReforgePanel : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public Image listBack;

	public TMP_InputField searchTxt;

	public GameObject craftListView;

	public GameObject craftListScroll;

	public TextMeshProUGUI descTxt;

	public Button filterBtn;

	public TextMeshProUGUI filterTxt;

	private List<ItemData> _tiles;

	private List<ItemData> currentiles;

	public CraftList itemList;

	private Transform window;

	private ItemReforgeFilterWindow _panel;

	private static int DEFAULT_SUBTYPE = -1;

	private bool _hasFilteredItems;

	private bool _updatePending;

	private CraftWindow _craftWindow;

	public bool updatePending
	{
		get
		{
			return _updatePending;
		}
		set
		{
			_updatePending = value;
		}
	}

	public void LoadDetails(CraftWindow craftWindow)
	{
		_craftWindow = craftWindow;
		nameTxt.text = Language.GetString("ui_select_reforge_items");
		filterTxt.text = Language.GetString("ui_filter");
		itemList.StartList(null, null, null);
		DoUpdate();
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		MyGridItemViewsHolder cellViewsHolderIfVisible = itemList.GetCellViewsHolderIfVisible(0);
		if (cellViewsHolderIfVisible != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(124);
			GameData.instance.tutorialManager.ShowTutorialForButton(cellViewsHolderIfVisible.root.gameObject, new TutorialPopUpSettings(Tutorial.GetText(124), 3, cellViewsHolderIfVisible.root.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
			{
				_craftWindow.CheckTutorial();
			}, shadow: true, tween: true);
		}
	}

	public void DoUpdate()
	{
		GetItems();
		CreateTiles();
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		CreateTiles();
	}

	private void GetItems()
	{
		_tiles = new List<ItemData>();
		List<ItemData> reforgableItems = GameData.instance.PROJECT.character.inventory.getReforgableItems();
		int reforgeFilter = GameData.instance.SAVE_STATE.GetReforgeFilter(GameData.instance.PROJECT.character.id);
		AdvancedFilterSettings reforgeAdvancedFilter = GameData.instance.SAVE_STATE.GetReforgeAdvancedFilter(GameData.instance.PROJECT.character.id);
		_hasFilteredItems = false;
		foreach (ItemData item in reforgableItems)
		{
			if (item.itemRef.itemType == 1 && item.qty > 0)
			{
				if (GameData.instance.SAVE_STATE.GetIsReforgeFiltered(GameData.instance.PROJECT.character.id, item.itemRef, reforgeFilter, reforgeAdvancedFilter, ItemUpgradeFilterWindow.EQUIPMENT_SUBTYPE_FILTERS))
				{
					_hasFilteredItems = true;
				}
				else
				{
					_tiles.Add(item);
				}
			}
		}
		_tiles = Util.SortVector(_tiles, new string[5] { "typeInverse", "rarity", "total", "tier", "id" }, Util.ARRAY_DESCENDING);
	}

	private void CreateTiles()
	{
		itemList.ClearList();
		List<CraftItemModel> list = new List<CraftItemModel>();
		list.Clear();
		string text = searchTxt.text;
		currentiles = new List<ItemData>();
		foreach (ItemData tile in _tiles)
		{
			if (tile != null && !(tile.itemRef == null) && tile.qty > 0 && !GameData.instance.PROJECT.character.getItemLocked(tile.itemRef))
			{
				if (text.Length > 0 && !Language.GetString(tile.itemRef.name).ToLower().Contains(searchTxt.text.ToLower()))
				{
					_hasFilteredItems = true;
					continue;
				}
				tile.SetPrecalculateds();
				list.Add(new CraftItemModel(tile, 12));
			}
		}
		descTxt.gameObject.SetActive(list.Count <= 0);
		if (descTxt.gameObject.activeSelf)
		{
			if (_hasFilteredItems)
			{
				descTxt.SetText(Language.GetString("ui_no_filtered_items"));
			}
			else
			{
				descTxt.SetText(Language.GetString("ui_no_reforge_items"));
			}
		}
		itemList.Data.InsertItems(0, list);
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		nameTxt.gameObject.SetActive(value: true);
		listBack.gameObject.SetActive(value: true);
		searchTxt.gameObject.SetActive(value: true);
		filterBtn.gameObject.SetActive(value: true);
		craftListView.gameObject.SetActive(value: true);
		craftListScroll.gameObject.SetActive(value: true);
		if (itemList.Data.Count > 0)
		{
			descTxt.gameObject.SetActive(value: false);
		}
		else
		{
			descTxt.gameObject.SetActive(value: true);
		}
		itemList.Refresh();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		nameTxt.gameObject.SetActive(value: false);
		listBack.gameObject.SetActive(value: false);
		searchTxt.gameObject.SetActive(value: false);
		filterBtn.gameObject.SetActive(value: false);
		craftListView.gameObject.SetActive(value: false);
		craftListScroll.gameObject.SetActive(value: false);
		descTxt.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		searchTxt.interactable = true;
		filterBtn.interactable = true;
	}

	public void DoDisable()
	{
		searchTxt.interactable = false;
		filterBtn.interactable = true;
	}

	public void OnFilterButton()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_panel = GameData.instance.windowGenerator.NewItemReforgeFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterWindow);
	}

	private void OnFilterWindow()
	{
		DoUpdate();
	}
}
