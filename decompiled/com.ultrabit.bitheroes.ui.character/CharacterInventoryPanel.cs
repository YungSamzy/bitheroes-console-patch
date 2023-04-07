using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.characterinventorylist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterInventoryPanel : MonoBehaviour
{
	private const int DEFAULT_SUBTYPE = -1;

	public Button filterBtn;

	public CharacterInventoryList characterInventoryList;

	public CharacterWindow _characterWindow;

	private ItemInventoryFilterWindow _panel;

	private CharacterData characterData;

	private List<ItemData> itemList;

	private Transform window;

	private bool refreshPending;

	private bool showingTutorial;

	public void LoadDetails()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		characterInventoryList.StartList();
		filterBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(Language.GetString("ui_filter"));
		filterBtn.onClick.AddListener(OnFilterClick);
		UpdateItemList();
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || GameData.instance.PROJECT.character.tutorial.GetState(42) || !GameData.instance.PROJECT.character.tutorial.GetState(41))
		{
			return;
		}
		EquipmentRef upgradeEquipment = GameData.instance.PROJECT.character.GetUpgradeEquipment();
		if (upgradeEquipment != null)
		{
			MyGridItemViewsHolder tile = GetTile(upgradeEquipment);
			if (tile != null)
			{
				showingTutorial = true;
				GameData.instance.PROJECT.character.tutorial.SetState(42);
				GameData.instance.tutorialManager.ShowTutorialForButton(tile.root.gameObject, new TutorialPopUpSettings(Tutorial.GetText(42), 4, tile.root.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, WaitForTutorial, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	private MyGridItemViewsHolder GetTile(ItemRef itemRef)
	{
		for (int i = 0; i < characterInventoryList.Data.Count; i++)
		{
			if (characterInventoryList.Data[i].itemData.itemRef.id == itemRef.id && characterInventoryList.Data[i].itemData.itemRef.itemType == itemRef.itemType && characterInventoryList.Data[i].itemData.itemRef.subtype == itemRef.subtype)
			{
				return characterInventoryList.GetCellViewsHolderIfVisible(i);
			}
		}
		return null;
	}

	private void WaitForTutorial(object e)
	{
		showingTutorial = false;
		if (refreshPending)
		{
			refreshPending = false;
			UpdateItemList();
		}
	}

	private void OnEquipmentChange()
	{
		if (showingTutorial)
		{
			refreshPending = true;
		}
		else
		{
			UpdateItemList(scrollTo: true);
		}
	}

	private void OnInventoryChange()
	{
		if (showingTutorial)
		{
			refreshPending = true;
		}
		else
		{
			UpdateItemList(scrollTo: true);
		}
	}

	private void UpdateItemList(bool scrollTo = false)
	{
		GetItemList();
		double virtualAbstractNormalizedScrollPosition = characterInventoryList.GetVirtualAbstractNormalizedScrollPosition();
		characterInventoryList.ClearList();
		List<CharacterInventoryListItem> list = new List<CharacterInventoryListItem>();
		foreach (ItemData item in itemList)
		{
			if (item.qty > 0 && item.itemRef.isViewable())
			{
				list.Add(new CharacterInventoryListItem
				{
					itemData = item
				});
			}
		}
		characterInventoryList.Data.InsertItems(0, list);
		if (scrollTo)
		{
			characterInventoryList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
		}
	}

	private void GetItemList()
	{
		if (itemList == null)
		{
			itemList = new List<ItemData>();
		}
		itemList.Clear();
		int inventoryFilter = GameData.instance.SAVE_STATE.GetInventoryFilter(GameData.instance.PROJECT.character.id);
		AdvancedFilterSettings inventoryAdvancedFilter = GameData.instance.SAVE_STATE.GetInventoryAdvancedFilter(GameData.instance.PROJECT.character.id);
		foreach (ItemData item in Util.SortVector(GameData.instance.PROJECT.character.inventory.items, new string[4] { "typeInverse", "rarity", "total", "id" }, Util.ARRAY_DESCENDING))
		{
			if (item != null && item.qty > 0 && !GameData.instance.SAVE_STATE.GetIsInventoryFiltered(GameData.instance.PROJECT.character.id, item.itemRef, inventoryFilter, inventoryAdvancedFilter, ItemInventoryFilterWindow.INVENTORY_FILTERS))
			{
				itemList.Add(item);
			}
		}
	}

	public void OnFilterClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_panel = GameData.instance.windowGenerator.NewItemInventoryFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterOptionSelected);
	}

	public void OnFilterOptionSelected()
	{
		UpdateItemList();
	}

	public void DoEnable()
	{
		filterBtn.interactable = true;
	}

	public void DoDisable()
	{
		filterBtn.interactable = false;
	}

	public void OnDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		if (_panel != null)
		{
			_panel.OnEventClose.RemoveListener(OnFilterOptionSelected);
		}
		filterBtn.onClick.RemoveListener(OnFilterClick);
	}
}
