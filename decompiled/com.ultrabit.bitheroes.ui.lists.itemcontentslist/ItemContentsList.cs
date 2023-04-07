using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.itemcontentslist;

public class ItemContentsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	[HideInInspector]
	public bool displayCompare;

	[HideInInspector]
	public bool displayUnlocked;

	[HideInInspector]
	public bool displayQty;

	public SimpleDataHelper<ItemContentsListItem> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<ItemContentsListItem>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		StartList();
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
		ItemContentsListItem itemContentsListItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemContentsListItem.itemIcon = itemIcon;
		if (itemContentsListItem.itemData.type == 1)
		{
			itemIcon.SetEquipmentData(new ItemData(itemContentsListItem.itemData.itemRef, (!displayQty) ? 1 : itemContentsListItem.itemData.qty), null, showComparision: true, (!displayQty) ? 1 : itemContentsListItem.itemData.qty);
		}
		else
		{
			itemIcon.SetItemData(new ItemData(itemContentsListItem.itemData.itemRef, (!displayQty) ? 1 : itemContentsListItem.itemData.qty), locked: false, (!displayQty) ? 1 : itemContentsListItem.itemData.qty);
		}
		itemIcon.SetItemActionType(0);
		if (displayCompare)
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
		}
		else
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: false);
		}
		itemIcon.SetSelected(selected: false);
		ItemContentsWindow componentInParent = GetComponentInParent<ItemContentsWindow>();
		if (componentInParent != null && ((componentInParent.itemRef.gacha && itemIcon.qty == 0) || (itemContentsListItem.itemData.itemRef.unique && GameData.instance.PROJECT.character.inventory.hasOwnedItem(itemContentsListItem.itemData.itemRef))))
		{
			itemIcon.SetSelected(selected: true);
		}
		if (!displayUnlocked && (GameData.instance.PROJECT.character.inventory.hasOwnedItem(itemIcon.itemRef) || GameData.instance.PROJECT.character.inventory.hasOwnedItem(itemIcon.itemRef.assetsSource)))
		{
			itemIcon.SetSelected(selected: true);
		}
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ItemContentsListItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<ItemContentsListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ItemContentsListItem[] newItems = new ItemContentsListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ItemContentsListItem[] newItems = new ItemContentsListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ItemContentsListItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
