using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.itemlist;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemListWindow : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI emptyTxt;

	public Button helpBtn;

	public RectTransform genericPrefab;

	public ItemList itemList;

	public Button altCloseBtn;

	private string _helpTitle;

	private string _helpDesc;

	private List<ItemData> items;

	private bool _usable;

	public const int RESIZE_LIMIT = 9;

	private string _name;

	private List<ItemData> _items;

	private bool _compare;

	private bool _added;

	private bool _select;

	private string _helpText;

	private bool _forceItemEnabled;

	public UnityEvent onCloseEvent = new UnityEvent();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public virtual void LoadDetails(List<ItemData> items, bool compare = true, bool added = false, string name = null, bool select = false, string helpText = null, string closeWord = null, bool forceItemEnabled = false)
	{
		_name = name;
		_items = items;
		_compare = compare;
		_added = added;
		_select = select;
		_helpText = helpText;
		_forceItemEnabled = forceItemEnabled;
		if (_name == null)
		{
			_name = Language.GetString("ui_items");
		}
		emptyTxt.text = Language.GetString("ui_item_list_empty");
		if (_helpText != null && _helpText.Length > 0)
		{
			helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		}
		else
		{
			helpBtn.gameObject.SetActive(value: false);
		}
		closeBtn.gameObject.SetActive(closeWord == null);
		altCloseBtn.gameObject.SetActive(closeWord != null);
		if (closeWord != null)
		{
			altCloseBtn.GetComponentInChildren<TextMeshProUGUI>().text = closeWord;
		}
		topperTxt.text = _name;
		itemList.StartList(OnItemClicked);
		CreateTiles();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		if (_compare)
		{
			GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		}
		ListenForBack(OnClose);
		ListenForForward(OnClose);
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
		List<ItemIcon> visibleTiles = GetVisibleTiles();
		if (!_added && GameData.instance.PROJECT.character.tutorial.GetState(91) && !GameData.instance.PROJECT.character.tutorial.GetState(92))
		{
			ItemIcon itemTypeSubtypeTile = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 9);
			if (itemTypeSubtypeTile != null)
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(92), 4, itemTypeSubtypeTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
		}
		if (!_added)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(12))
		{
			ItemIcon equipmentTile = ItemIcon.GetEquipmentTile(upgrade: false, visibleTiles);
			if (equipmentTile != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(12);
				GameData.instance.tutorialManager.ShowTutorialForButton(equipmentTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(12), 4, equipmentTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(45))
		{
			ItemIcon abilityChangeTile = ItemIcon.GetAbilityChangeTile(visibleTiles);
			if (abilityChangeTile != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(45);
				GameData.instance.tutorialManager.ShowTutorialForButton(abilityChangeTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(45), 4, abilityChangeTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(39))
		{
			ItemIcon itemTypeSubtypeTile2 = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 4);
			if (itemTypeSubtypeTile2 != null && (itemTypeSubtypeTile2.itemRef as ConsumableRef).consumableType == 5)
			{
				itemTypeSubtypeTile2.SetItemActionType(6, forceConsume: true);
				GameData.instance.PROJECT.character.tutorial.SetState(39);
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(39), 4, itemTypeSubtypeTile2.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(109))
		{
			ItemIcon itemTypeSubtypeTile3 = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 4);
			if (itemTypeSubtypeTile3 != null && (itemTypeSubtypeTile3.itemRef as ConsumableRef).rawBox == "tutorial_schematic")
			{
				GameData.instance.PROJECT.character.tutorial.SetState(109);
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile3.gameObject, new TutorialPopUpSettings(Tutorial.GetText(109), 4, itemTypeSubtypeTile3.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(51) && GameData.instance.PROJECT.dungeon == null)
		{
			ItemIcon itemTypeSubtypeTile4 = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 8);
			if (itemTypeSubtypeTile4 != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(51);
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile4.gameObject, new TutorialPopUpSettings(Tutorial.GetText(51), 4, itemTypeSubtypeTile4.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(75) && GameData.instance.PROJECT.character.tutorial.GetState(73))
		{
			ItemIcon itemTypeSubtypeTile5 = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 15);
			if (itemTypeSubtypeTile5 != null)
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile5.gameObject, new TutorialPopUpSettings(Tutorial.GetText(74), 4, itemTypeSubtypeTile5.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(97) && visibleTiles.Count == 1)
		{
			ItemIcon itemTypeSubtypeTile6 = ItemIcon.GetItemTypeSubtypeTile(visibleTiles, 11);
			if (itemTypeSubtypeTile6 != null)
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(itemTypeSubtypeTile6.gameObject, new TutorialPopUpSettings(Tutorial.GetText(97), 4, itemTypeSubtypeTile6.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
		}
		foreach (ItemIcon item in visibleTiles)
		{
			if (!(item == null) && item.itemRef.tutorialID > 0 && !GameData.instance.PROJECT.character.tutorial.GetState(item.itemRef.tutorialID))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(item.itemRef.tutorialID);
				GameData.instance.tutorialManager.ShowTutorialForButton(item.gameObject, new TutorialPopUpSettings(Tutorial.GetText(item.itemRef.tutorialID), 4, item.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				break;
			}
		}
	}

	private List<ItemIcon> GetVisibleTiles()
	{
		List<ItemIcon> list = new List<ItemIcon>();
		for (int i = 0; i < itemList.Data.Count; i++)
		{
			MyGridItemViewsHolder cellViewsHolderIfVisible = itemList.GetCellViewsHolderIfVisible(i);
			if (cellViewsHolderIfVisible != null && cellViewsHolderIfVisible.root.GetComponent<ItemIcon>() != null)
			{
				list.Add(cellViewsHolderIfVisible.root.GetComponent<ItemIcon>());
			}
		}
		return list;
	}

	private void CreateTiles()
	{
		itemList.ClearList();
		List<ItemData> list = ItemData.SortLoot(_items);
		List<ItemListModel> list2 = new List<ItemListModel>();
		foreach (ItemData item in list)
		{
			if (item != null && item.itemRef.lootDisplay)
			{
				bool inserted = item.qty > 0;
				list2.Add(new ItemListModel
				{
					itemData = item,
					compare = _compare,
					added = _added,
					select = _select,
					inserted = inserted,
					forceItemEnabled = _forceItemEnabled
				});
			}
		}
		itemList.Data.InsertItemsAtEnd(list2);
		emptyTxt.gameObject.SetActive(itemList.Data.Count == 0);
	}

	public void OnItemClicked(ItemListModel item)
	{
		SELECT.Invoke(new object[2]
		{
			this,
			item.itemData.itemRef
		});
	}

	public void RemoveItems(List<ItemData> items)
	{
		if (!_added || items == null || items.Count <= 0)
		{
			return;
		}
		foreach (ItemData item in items)
		{
			RemoveItem(item);
		}
	}

	public void RemoveItem(ItemData itemData)
	{
		if (!_added)
		{
			return;
		}
		foreach (ItemListModel item in (IEnumerable<ItemListModel>)itemList.Data)
		{
			ItemIcon itemIcon = item.itemIcon;
			if (itemIcon.itemRef == itemData.itemRef)
			{
				itemIcon.setQty(itemIcon.qty - itemData.qty);
				if (itemIcon.qty <= 0)
				{
					itemIcon.DisableItem(disable: true);
				}
				else
				{
					itemIcon.DisableItem(disable: false, changeAction: false);
				}
				break;
			}
		}
	}

	private void OnEquipmentChange()
	{
		UpdateTiles();
	}

	private void UpdateTiles()
	{
		itemList.Refresh();
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(_name, _helpText);
	}

	public void OnAltCloseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		OnClose();
	}

	public override void OnClose()
	{
		onCloseEvent?.Invoke();
		base.OnClose();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (_compare)
		{
			GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		Util.SetButton(helpBtn);
		Util.SetButton(altCloseBtn);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		Util.SetButton(helpBtn, enabled: false);
		Util.SetButton(altCloseBtn, enabled: false);
	}
}
