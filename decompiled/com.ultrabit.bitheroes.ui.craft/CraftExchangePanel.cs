using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.craftlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftExchangePanel : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public Image listBack;

	public GameObject craftListView;

	public GameObject craftListScroll;

	public TextMeshProUGUI descTxt;

	public Button allBtn;

	public Button noneBtn;

	public Button filterBtn;

	public Button exchangeBtn;

	public CraftList itemList;

	private CraftWindow _craftWindow;

	private List<ItemData> selectedItems;

	private bool _updatePending;

	private bool _hasFilteredItems;

	private ItemExchangeFilterWindow _panel;

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
		nameTxt.text = Language.GetString("ui_select_exchange_items");
		descTxt.text = Language.GetString("ui_no_exchange_items");
		filterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_filter");
		allBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_all");
		noneBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_none");
		exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		itemList.StartList(OnSelectedItem, OnDeselectItem, OnUpdateViews);
		itemList.panelForList = 0;
		selectedItems = new List<ItemData>();
		CreateTiles();
		Util.SetButton(exchangeBtn, enabled: false);
	}

	private void OnUpdateViews()
	{
		List<ItemData> list = new List<ItemData>();
		for (int i = 0; i < itemList._VisibleItems.Count; i++)
		{
			CellGroupViewsHolder<MyGridItemViewsHolder> itemViewsHolder = itemList.GetItemViewsHolder(i);
			for (int j = 0; j < itemViewsHolder.ContainingCellViewsHolders.Length; j++)
			{
				if (itemViewsHolder.ContainingCellViewsHolders[j].root.GetComponent<ItemIcon>() == null)
				{
					continue;
				}
				ItemData itemData = itemViewsHolder.ContainingCellViewsHolders[j].root.GetComponent<ItemIcon>().item as ItemData;
				for (int k = 0; k < selectedItems.Count; k++)
				{
					if (selectedItems[k].itemRef.id == itemData.itemRef.id && selectedItems[k].itemRef.itemType == itemData.itemRef.itemType && selectedItems[k].itemRef.subtype == itemData.itemRef.subtype && !list.Contains(selectedItems[k]))
					{
						list.Add(selectedItems[k]);
						break;
					}
				}
			}
		}
		Util.SetButton(exchangeBtn, list.Count > 0);
		Util.SetButton(noneBtn, selectedItems.Count != 0);
		Util.SetButton(allBtn, selectedItems.Count != itemList.Data.Count);
	}

	public void UpdateTiles()
	{
		CreateTiles();
	}

	public void DoUpdate()
	{
		CreateTiles();
	}

	private void CreateTiles()
	{
		if (itemList != null)
		{
			itemList.ClearList();
		}
		if (selectedItems != null)
		{
			selectedItems.Clear();
		}
		if (GameData.instance.PROJECT.character == null)
		{
			return;
		}
		int exchangeFilter = GameData.instance.SAVE_STATE.GetExchangeFilter(GameData.instance.PROJECT.character.id);
		AdvancedFilterSettings exchangeAdvancedFilter = GameData.instance.SAVE_STATE.GetExchangeAdvancedFilter(GameData.instance.PROJECT.character.id);
		if (GameData.instance.PROJECT.character.inventory == null || GameData.instance.PROJECT.character.inventory.items == null)
		{
			return;
		}
		List<ItemData> list = Util.SortVector(GameData.instance.PROJECT.character.inventory.items, new string[4] { "typeInverse", "rarity", "total", "id" }, Util.ARRAY_DESCENDING);
		List<CraftItemModel> list2 = new List<CraftItemModel>();
		int num = 0;
		_hasFilteredItems = false;
		foreach (ItemData item in list)
		{
			if (item == null || item.qty <= 0 || item.itemRef.isNFT || !item.itemRef.exchangeable || GameData.instance.PROJECT.character.getItemLocked(item.itemRef))
			{
				continue;
			}
			ItemData itemData = item;
			if (GameData.instance.PROJECT.character.equipment.getEquipmentCount(item.itemRef) > 0 || GameData.instance.PROJECT.character.armory.GetEquipmentCount(item.itemRef) > 0)
			{
				if (item.qty == 1)
				{
					continue;
				}
				itemData = new ItemData(item.itemRef, item.qty - 1);
			}
			if (GameData.instance.SAVE_STATE.GetIsExchangeFiltered(GameData.instance.PROJECT.character.id, item.itemRef, exchangeFilter, exchangeAdvancedFilter, ItemExchangeFilterWindow.EXCHANGE_FILTERS))
			{
				_hasFilteredItems = true;
			}
			else if (!InCraftItemsList(list2, itemData.id))
			{
				list2.Add(new CraftItemModel(itemData, 10));
				num++;
			}
		}
		descTxt.gameObject.SetActive(list2.Count <= 0);
		if (descTxt.gameObject.activeSelf)
		{
			if (_hasFilteredItems)
			{
				descTxt.SetText(Language.GetString("ui_no_filtered_items"));
			}
			else
			{
				descTxt.SetText(Language.GetString("ui_no_exchange_items"));
			}
		}
		itemList.Data.InsertItems(0, list2);
	}

	private bool InCraftItemsList(List<CraftItemModel> cList, int id)
	{
		foreach (CraftItemModel c in cList)
		{
			if (c.itemData.id == id)
			{
				return true;
			}
		}
		return false;
	}

	public void OnExchangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		List<ItemData> finals = new List<ItemData>();
		for (int i = 0; i < itemList._VisibleItems.Count; i++)
		{
			CellGroupViewsHolder<MyGridItemViewsHolder> itemViewsHolder = itemList.GetItemViewsHolder(i);
			for (int j = 0; j < itemViewsHolder.ContainingCellViewsHolders.Length; j++)
			{
				if (itemViewsHolder.ContainingCellViewsHolders[j].root.GetComponent<ItemIcon>() == null)
				{
					continue;
				}
				ItemData itemData = itemViewsHolder.ContainingCellViewsHolders[j].root.GetComponent<ItemIcon>().item as ItemData;
				for (int k = 0; k < selectedItems.Count; k++)
				{
					if (selectedItems[k].itemRef.id == itemData.itemRef.id && selectedItems[k].itemRef.itemType == itemData.itemRef.itemType && selectedItems[k].itemRef.subtype == itemData.itemRef.subtype && !finals.Contains(selectedItems[k]))
					{
						finals.Add(selectedItems[k]);
						break;
					}
				}
			}
		}
		if (finals.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_blank_exchange"));
			return;
		}
		if (finals.Count == 1)
		{
			ItemData itemData2 = finals[0];
			if (itemData2.qty > 1)
			{
				GameData.instance.windowGenerator.NewItemExchangeWindow(itemData2);
				return;
			}
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_exchange_confirm"), null, null, delegate
		{
			List<ItemData> list = new List<ItemData>();
			foreach (ItemData item in finals)
			{
				if (item.qty > 0)
				{
					list.Add(item);
				}
			}
			MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_EXCHANGE), OnItemExchange);
			MerchantDALC.instance.doItemExchange(list);
		});
	}

	private void OnItemExchange(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_EXCHANGE), OnItemExchange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
		List<ItemData> list2 = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.addItems(list);
		GameData.instance.PROJECT.character.removeItems(list2);
		KongregateAnalytics.checkEconomyTransaction("Craft Exchange", list2, list, sfsob, "Craft", 1);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		GameData.instance.audioManager.PlaySoundLink("exchange");
		UpdateTiles();
	}

	public void OnFilterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_panel = GameData.instance.windowGenerator.NewItemExchangeFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterWindowClose);
	}

	private void OnFilterWindowClose()
	{
		_panel.OnEventClose.RemoveAllListeners();
		UpdateTiles();
	}

	public void OnAllBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		itemList.OnItemSelected(null, null);
	}

	public void OnNoneBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		itemList.OnItemDeselected(null, null);
	}

	public void OnDeselectItem(CraftItemModel newOrRecycled, BaseModelData model)
	{
		if (selectedItems.Contains(model as ItemData))
		{
			selectedItems.Remove(model as ItemData);
		}
	}

	public void OnSelectedItem(CraftItemModel newOrRecycled, BaseModelData model)
	{
		if (!selectedItems.Contains(model as ItemData))
		{
			selectedItems.Add(model as ItemData);
		}
		StartCoroutine(CheckTutorialLateUpdate());
	}

	private IEnumerator CheckTutorialLateUpdate()
	{
		yield return new WaitForEndOfFrame();
		_craftWindow.CheckTutorial();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		nameTxt.gameObject.SetActive(value: true);
		listBack.gameObject.SetActive(value: true);
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
		allBtn.gameObject.SetActive(value: true);
		noneBtn.gameObject.SetActive(value: true);
		filterBtn.gameObject.SetActive(value: true);
		exchangeBtn.gameObject.SetActive(value: true);
		itemList.Refresh();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		nameTxt.gameObject.SetActive(value: false);
		listBack.gameObject.SetActive(value: false);
		craftListView.gameObject.SetActive(value: false);
		craftListScroll.gameObject.SetActive(value: false);
		descTxt.gameObject.SetActive(value: false);
		allBtn.gameObject.SetActive(value: false);
		noneBtn.gameObject.SetActive(value: false);
		filterBtn.gameObject.SetActive(value: false);
		exchangeBtn.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		allBtn.interactable = true;
		noneBtn.interactable = true;
		filterBtn.interactable = true;
		exchangeBtn.interactable = true;
	}

	public void DoDisable()
	{
		allBtn.interactable = false;
		noneBtn.interactable = false;
		filterBtn.interactable = false;
		exchangeBtn.interactable = false;
	}
}
