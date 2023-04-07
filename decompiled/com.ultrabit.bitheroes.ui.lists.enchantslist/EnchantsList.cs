using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.enchantslist;

public class EnchantsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public UnityAction<BaseModelData> onEnchantSelected;

	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<EnchantsListItem> Data { get; private set; }

	public void InitList(UnityAction<BaseModelData> onEnchantSelected)
	{
		this.onEnchantSelected = onEnchantSelected;
		if (Data == null)
		{
			Data = new SimpleDataHelper<EnchantsListItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_item_list_empty");
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<EnchantsListItem>(this);
			base.Start();
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		EnchantsListItem enchantsListItem = Data[newOrRecycled.ItemIndex];
		if (enchantsListItem != null && enchantsListItem.enchantData != null)
		{
			ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
			if (itemIcon == null)
			{
				itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
			}
			itemIcon.SetEnchantData(enchantsListItem.enchantData);
			if (enchantsListItem.enchantData.slot == -1)
			{
				itemIcon.SetItemActionType(7);
			}
			else if (!enchantsListItem.isEquipped)
			{
				itemIcon.SetItemActionType(10, onEnchantSelected);
			}
			else
			{
				itemIcon.DisableItem(disable: true);
			}
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	public void AddItemsAt(int index, IList<EnchantsListItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<EnchantsListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		EnchantsListItem[] newItems = new EnchantsListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		EnchantsListItem[] newItems = new EnchantsListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(EnchantsListItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
