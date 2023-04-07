using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.nbpitemslist;

public class NBPItemsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public SimpleDataHelper<NBPItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<NBPItem>(this);
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		NBPItem nBPItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		if (nBPItem.itemData.type == 1)
		{
			itemIcon.SetEquipmentData(nBPItem.itemData.itemRef as EquipmentRef, null, showComparision: true);
		}
		else
		{
			itemIcon.SetItemData(nBPItem.itemData);
		}
		itemIcon.SetItemActionType(0);
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<NBPItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<NBPItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		NBPItem[] newItems = new NBPItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		NBPItem[] newItems = new NBPItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(NBPItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
