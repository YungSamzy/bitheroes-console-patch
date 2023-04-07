using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.fishingshoplist;

public class FishingShopList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public SimpleDataHelper<FishingShopItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<FishingShopItem>(this);
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
		FishingShopItem fishingShopItem = Data[newOrRecycled.ItemIndex];
		int qty = GetQty(fishingShopItem);
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new FishingShopItemRefModelData(fishingShopItem.fishingItemRef, qty), locked: false, 0, tintRarity: true, newOrRecycled.UIItemIconColor);
		itemIcon.SetItemActionType(4);
		itemIcon.setQty(fishingShopItem.fishingItemRef.itemData.qty * qty);
		newOrRecycled.UIName.text = fishingShopItem.fishingItemRef.itemData.itemRef.coloredName;
		newOrRecycled.UICost.text = Util.NumberFormat(fishingShopItem.fishingItemRef.cost * qty);
	}

	private int GetQty(FishingShopItem model)
	{
		if (model.fishingItemRef.itemData.itemRef.allowQty && model.fishingShopPanel != null && model.fishingShopPanel.fishingShopWindow != null)
		{
			return model.fishingShopPanel.fishingShopWindow.GetQty();
		}
		return 1;
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<FishingShopItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<FishingShopItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FishingShopItem[] newItems = new FishingShopItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		FishingShopItem[] newItems = new FishingShopItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FishingShopItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
