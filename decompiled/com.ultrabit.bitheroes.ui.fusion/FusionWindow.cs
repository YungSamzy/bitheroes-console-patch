using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.tradelist;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fusion;

public class FusionWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public TextMeshProUGUI dropTxt;

	public TextMeshProUGUI emptyTxt;

	public Image rarityDropdown;

	public Toggle ownedCheckbox;

	public TMP_InputField searchTxt;

	public TradeList tradeList;

	private Transform window;

	public const float SEARCH_WAIT_DELAY = 0f;

	private List<MyDropdownItemModel> _sortOptions = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedSort;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = ItemRef.GetItemNamePlural(7);
		ownedCheckbox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_owned");
		ownedCheckbox.SetIsOnWithoutNotify(value: true);
		emptyTxt.text = Language.GetString("ui_fusion_list_empty_filter");
		CreateDropdown();
		CreateTiles();
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(base.OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || !GameData.instance.PROJECT.character.tutorial.GetState(66) || GameData.instance.PROJECT.character.tutorial.GetState(67))
		{
			return;
		}
		com.ultrabit.bitheroes.ui.lists.tradelist.MyListItemViewsHolder fusionTile = GetFusionTile();
		GameData.instance.PROJECT.character.tutorial.SetState(67);
		GameData.instance.PROJECT.CheckTutorialChanges();
		if (fusionTile != null && fusionTile.craftBtn.interactable)
		{
			if (fusionTile.craftBtn.enabled)
			{
				tradeList.tutorial = true;
				GameData.instance.tutorialManager.ShowTutorialForButton(fusionTile.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(67), 3, fusionTile.craftBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
			else
			{
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(fusionTile.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(67), 3, fusionTile.craftBtn.gameObject), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			}
		}
	}

	private com.ultrabit.bitheroes.ui.lists.tradelist.MyListItemViewsHolder GetFusionTile()
	{
		for (int i = 0; i < tradeList.Data.Count; i++)
		{
			if (tradeList.Data[i].tradeRef != null && tradeList.Data[i].tradeRef.resultItem.id == VariableBook.tutorialBootyId)
			{
				return tradeList.GetItemViewsHolderIfVisible(i);
			}
		}
		return null;
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		UpdateAfterSearch();
	}

	private void UpdateAfterSearch()
	{
		CreateTiles();
		Sort();
		DoUpdate();
	}

	public void OnOwnedCheckbox()
	{
		CreateTiles();
	}

	private void CreateDropdown()
	{
		MyDropdownItemModel item = (_selectedSort = new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all"),
			data = null
		});
		_sortOptions.Add(item);
		for (int i = 0; i < RarityBook.size; i++)
		{
			RarityRef rarityRef = RarityBook.LookupID(i);
			if (rarityRef != null && FusionBook.GetRarityCount(rarityRef) > 0)
			{
				_sortOptions.Add(new MyDropdownItemModel
				{
					id = i,
					title = rarityRef.coloredName,
					data = rarityRef
				});
			}
		}
		dropTxt.text = _selectedSort.title;
	}

	public void OnSortDropdown()
	{
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedSort.id, OnRarityDropdownClick);
		componentInChildren.Data.InsertItemsAtEnd(_sortOptions);
	}

	public void OnRarityDropdownClick(MyDropdownItemModel model)
	{
		_selectedSort = model;
		dropTxt.text = _selectedSort.title;
		Sort();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateTiles()
	{
		tradeList.InitList();
		tradeList.ClearList();
		List<TradeItem> list = new List<TradeItem>();
		string text = searchTxt.text;
		RarityRef rarityRef = _selectedSort.data as RarityRef;
		int num = 0;
		int num2 = 0;
		Dictionary<int, bool> seenSchematics = GameData.instance.SAVE_STATE.GetSeenSchematics(GameData.instance.PROJECT.character.id);
		foreach (FusionRef item2 in FusionBook.GetAllFusionsRef())
		{
			if (item2 == null)
			{
				continue;
			}
			TradeItem item = new TradeItem
			{
				tradeRef = item2.tradeRef,
				sourceRef = item2,
				parentWindow = base.gameObject,
				seen = seenSchematics.ContainsKey(item2.tradeRef.resultItem.itemRef.id)
			};
			bool flag = false;
			if (rarityRef == null || (rarityRef != null && item2.rarityRef.id == rarityRef.id))
			{
				num2++;
				if (!item2.isLocked())
				{
					num++;
				}
				flag = !ownedCheckbox.isOn || !item2.isLocked();
			}
			if (flag && text.Length > 0 && !ItemTradeContainsWord(text, item2.tradeRef))
			{
				flag = false;
			}
			if (flag)
			{
				list.Add(item);
			}
		}
		List<SortItem> list2 = new List<SortItem>();
		foreach (TradeItem item3 in list)
		{
			SortItem sortItem = new SortItem();
			sortItem.item = item3;
			sortItem.id = item3.sourceRef.id;
			sortItem.unlocked = !isLocked(item3.sourceRef);
			sortItem.rarity = rarity(item3.tradeRef, item3.sourceRef);
			list2.Add(sortItem);
		}
		list2 = Util.SortVector(list2, new string[3] { "unlocked", "rarity", "id" }, Util.ARRAY_DESCENDING);
		list.Clear();
		foreach (SortItem item4 in list2)
		{
			list.Add(item4.item);
		}
		if (list.Count > 0)
		{
			list.Add(new TradeItem
			{
				unlocks = true,
				unlocksText = new string[2]
				{
					Language.GetString("ui_fusion_unlocks_1"),
					Language.GetString("ui_fusion_unlocks_2")
				}
			});
		}
		tradeList.Data.InsertItemsAtEnd(list);
		if (!ownedCheckbox.isOn)
		{
			countTxt.text = Util.NumberFormat(num) + "/" + Util.NumberFormat(num2);
		}
		else
		{
			countTxt.text = Util.NumberFormat(num) + "/" + Util.NumberFormat(num);
		}
		emptyTxt.gameObject.SetActive(tradeList.Data.Count == 0);
	}

	private bool ItemTradeContainsWord(string word, ItemTradeRef itemTradeRef)
	{
		for (int i = 0; i < itemTradeRef.requiredItems.Count; i++)
		{
			if (itemTradeRef.requiredItems[i].itemRef.name.ToLowerInvariant().IndexOf(word.ToLowerInvariant()) >= 0)
			{
				return true;
			}
		}
		if (itemTradeRef.resultItem.itemRef.name.ToLowerInvariant().IndexOf(word.ToLowerInvariant()) >= 0)
		{
			return true;
		}
		return false;
	}

	private void OnEquipmentChange()
	{
		DoUpdate();
	}

	private void OnInventoryChange()
	{
		DoUpdate();
	}

	private void DoUpdate()
	{
		tradeList.Refresh();
	}

	private void Sort()
	{
		CreateTiles();
	}

	private int rarity(ItemTradeRef tradeRef, BaseRef sourceRef)
	{
		if (sourceRef is FusionRef)
		{
			return (sourceRef as FusionRef).rarity;
		}
		return tradeRef.resultItem.itemRef.rarity;
	}

	private bool isLocked(BaseRef sourceRef)
	{
		if (sourceRef is FusionRef)
		{
			return (sourceRef as FusionRef).isLocked();
		}
		return false;
	}

	public override void OnClose()
	{
		GameData.instance.SAVE_STATE.SetSeenSchematics(GameData.instance.PROJECT.character.id, tradeList.schematicsSeen);
		FamiliarsWindow familiarsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(FamiliarsWindow)) as FamiliarsWindow;
		if (familiarsWindow != null)
		{
			familiarsWindow.UpdateText();
		}
		MenuInterfaceFamiliarTile menuInterfaceFamiliarTile = GameData.instance.PROJECT.menuInterface.GetButton(typeof(MenuInterfaceFamiliarTile)) as MenuInterfaceFamiliarTile;
		if (menuInterfaceFamiliarTile != null)
		{
			menuInterfaceFamiliarTile.UpdateText();
		}
		base.OnClose();
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		rarityDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		rarityDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
