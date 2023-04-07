using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.itemsearchlist;

public class ItemSearchList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	[HideInInspector]
	public bool tooltipSuggested;

	private UnityAction<BaseModelData, ItemSearchItem> onClickDelegate;

	public SimpleDataHelper<ItemSearchItem> Data { get; private set; }

	public void StartList(UnityAction<BaseModelData, ItemSearchItem> onClickDelegate = null)
	{
		this.onClickDelegate = onClickDelegate;
		if (Data == null)
		{
			Data = new SimpleDataHelper<ItemSearchItem>(this);
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
			Data = new SimpleDataHelper<ItemSearchItem>(this);
			base.Start();
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		ItemSearchItem model = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		if (model.adminWindow)
		{
			itemIcon.SetItemData(new ItemData(model.itemData.itemRef, -1));
		}
		else
		{
			itemIcon.SetItemData(model.itemData);
		}
		if (tooltipSuggested)
		{
			itemIcon.SetItemActionType(model.itemData.itemRef.getItemTooltipType(GameData.instance.PROJECT.character), delegate(BaseModelData data)
			{
				onClickDelegate(data, model);
			});
		}
		else
		{
			itemIcon.SetItemActionType(10, delegate(BaseModelData data)
			{
				onClickDelegate(data, model);
			});
		}
		if (model.adminWindow)
		{
			itemIcon.setQty(model.itemData.qty, show: true, forceStats: true);
		}
		itemIcon.SetLocked(model.locked);
		itemIcon.SetSelected(model.selected);
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ItemSearchItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<ItemSearchItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ItemSearchItem[] newItems = new ItemSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ItemSearchItem[] newItems = new ItemSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ItemSearchItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
