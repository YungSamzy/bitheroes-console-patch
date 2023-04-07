using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.cosmeticslist;

public class CosmeticsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	[HideInInspector]
	public UnityAction<BaseModelData> onCosmeticSelected;

	[HideInInspector]
	public ItemRef cosmeticRef;

	public SimpleDataHelper<CosmeticItem> Data { get; private set; }

	public void InitList(UnityAction<BaseModelData> onCosmeticSelected)
	{
		this.onCosmeticSelected = onCosmeticSelected;
		if (Data == null)
		{
			Data = new SimpleDataHelper<CosmeticItem>(this);
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
		CosmeticItem cosmeticItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		_ = cosmeticItem.itemRef.itemType;
		itemIcon.SetItemData(new ItemData(cosmeticItem.itemRef, 1), locked: false, -1, tintRarity: true, null, showRanks: false, emptySlotFull: false, isCosmetic: true);
		itemIcon.SetItemActionType(10, onCosmeticSelected);
		itemIcon.ResetHidden();
		bool flag = GameData.instance.PROJECT.character.inventory.hasOwnedItem(cosmeticItem.itemRef);
		itemIcon.SetHidden(!flag);
		itemIcon.SetClickable(flag);
		if (cosmeticItem.itemRef == cosmeticRef)
		{
			itemIcon.PlayComparison("E");
		}
		else
		{
			itemIcon.PlayComparison("=");
		}
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<CosmeticItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<CosmeticItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		CosmeticItem[] newItems = new CosmeticItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		CosmeticItem[] newItems = new CosmeticItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(CosmeticItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
