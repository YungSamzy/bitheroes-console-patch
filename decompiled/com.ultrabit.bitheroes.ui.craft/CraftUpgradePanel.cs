using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
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

public class CraftUpgradePanel : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public Image listBack;

	public TMP_InputField searchTxt;

	public GameObject craftListView;

	public GameObject craftListScroll;

	public TextMeshProUGUI descTxt;

	public Button filterBtn;

	public TextMeshProUGUI filterTxt;

	private List<ItemData> currentItems;

	public CraftList itemList;

	private ItemUpgradeFilterWindow _panel;

	private Transform window;

	private bool _updatePending;

	private CraftWindow _craftWindow;

	private bool showingTutorial;

	private bool hasFilteredItems;

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
		nameTxt.text = Language.GetString("ui_select_upgrade_items");
		filterTxt.text = Language.GetString("ui_filter");
		itemList.StartList(null, null, null);
		CreateTiles();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(25))
		{
			MyGridItemViewsHolder randomEquippedUpgradeTile = GetRandomEquippedUpgradeTile();
			if (randomEquippedUpgradeTile != null)
			{
				showingTutorial = true;
				GameData.instance.PROJECT.character.tutorial.SetState(25);
				GameData.instance.tutorialManager.ShowTutorialForButton(randomEquippedUpgradeTile.root.gameObject, new TutorialPopUpSettings(Tutorial.GetText(25), 3, randomEquippedUpgradeTile.root.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, WaitForTutorial, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	public MyGridItemViewsHolder GetRandomEquippedUpgradeTile()
	{
		List<MyGridItemViewsHolder> list = new List<MyGridItemViewsHolder>();
		for (int i = 0; i < itemList.Data.Count; i++)
		{
			MyGridItemViewsHolder cellViewsHolderIfVisible = itemList.GetCellViewsHolderIfVisible(i);
			if (cellViewsHolderIfVisible != null && cellViewsHolderIfVisible.root.GetComponent<ItemIcon>().GetComparison() == "E" && cellViewsHolderIfVisible.root.GetComponent<ItemIcon>().GetNotify())
			{
				list.Add(cellViewsHolderIfVisible);
			}
		}
		if (list.Count > 0)
		{
			return list[Util.randomInt(0, list.Count - 1)];
		}
		return null;
	}

	private void WaitForTutorial(object e)
	{
		showingTutorial = false;
		if (_updatePending)
		{
			DoUpdate();
		}
	}

	public void DoUpdate()
	{
		CreateTiles();
	}

	private void CreateTiles()
	{
		GetItems();
		itemList.ClearList();
		List<CraftItemModel> list = new List<CraftItemModel>();
		int num = 0;
		foreach (ItemData currentItem in currentItems)
		{
			if (!GameData.instance.PROJECT.character.getItemLocked(currentItem.itemRef))
			{
				if (!Language.GetString(currentItem.itemRef.name).ToLower().Contains(searchTxt.text.ToLower()) && searchTxt.text.Length >= 1)
				{
					hasFilteredItems = true;
					continue;
				}
				list.Add(new CraftItemModel(currentItem, 9));
				num++;
			}
		}
		descTxt.gameObject.SetActive(list.Count <= 0);
		if (descTxt.gameObject.activeSelf)
		{
			if (hasFilteredItems)
			{
				descTxt.SetText(Language.GetString("ui_no_filtered_items"));
			}
			else
			{
				descTxt.SetText(Language.GetString("ui_no_upgrade_items"));
			}
		}
		itemList.Data.InsertItems(0, list);
	}

	private void GetItems()
	{
		currentItems = new List<ItemData>();
		int upgradeFilter = GameData.instance.SAVE_STATE.GetUpgradeFilter(GameData.instance.PROJECT.character.id);
		AdvancedFilterSettings upgradeAdvancedFilter = GameData.instance.SAVE_STATE.GetUpgradeAdvancedFilter(GameData.instance.PROJECT.character.id);
		hasFilteredItems = false;
		foreach (ItemData item in GameData.instance.PROJECT.character.inventory.items)
		{
			if (item.itemRef.itemType == 16)
			{
				item.itemRef.OverrideItemType(1);
			}
			if (item.itemRef.itemType != 1)
			{
				continue;
			}
			EquipmentRef equipmentRef = item.itemRef as EquipmentRef;
			if (equipmentRef.upgrades != null && equipmentRef.upgrades.Count > 0 && item.qty > 0)
			{
				if (GameData.instance.SAVE_STATE.GetIsUpgradeFiltered(GameData.instance.PROJECT.character.id, item.itemRef, upgradeFilter, upgradeAdvancedFilter, ItemUpgradeFilterWindow.EQUIPMENT_SUBTYPE_FILTERS))
				{
					hasFilteredItems = true;
					continue;
				}
				item.SetPrecalculateds();
				currentItems.Add(item);
			}
		}
		currentItems = Util.SortVector(currentItems, new string[4] { "rarity", "totalCalculated", "tier", "id" }, Util.ARRAY_DESCENDING);
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
	}

	public void DoEnable()
	{
		searchTxt.interactable = true;
		filterBtn.interactable = true;
	}

	public void DoDisable()
	{
		searchTxt.interactable = false;
		filterBtn.interactable = false;
	}

	public void OnFilterButton()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_panel = GameData.instance.windowGenerator.NewItemUpgradeFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterWindow);
	}

	private void OnFilterWindow()
	{
		CreateTiles();
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		if (currentItems != null && currentItems.Count > 0)
		{
			CreateTiles();
		}
	}
}
