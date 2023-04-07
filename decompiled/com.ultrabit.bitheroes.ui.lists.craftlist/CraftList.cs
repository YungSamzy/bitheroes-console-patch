using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.craftlist;

public class CraftList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public UnityAction<CraftItemModel, BaseModelData> onSelected;

	public UnityAction<CraftItemModel, BaseModelData> onDeselected;

	private UnityAction updateViews;

	[HideInInspector]
	public int panelForList = -1;

	public SimpleDataHelper<CraftItemModel> Data { get; private set; }

	public void StartList(UnityAction<CraftItemModel, BaseModelData> onSelected, UnityAction<CraftItemModel, BaseModelData> onDeselected, UnityAction updateViews)
	{
		this.onSelected = onSelected;
		this.onDeselected = onDeselected;
		this.updateViews = updateViews;
		if (Data == null)
		{
			Data = new SimpleDataHelper<CraftItemModel>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<CraftItemModel>(this);
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
		CraftItemModel model = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.itemData.itemRef, model.itemData.qty));
		if (panelForList == 0)
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: false);
		}
		else
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
		}
		switch (model.action)
		{
		case 10:
			itemIcon.SetSelected(selected: false);
			itemIcon.SetItemActionType(10, delegate(BaseModelData bmd)
			{
				OnItemSelected(model, bmd);
			});
			break;
		case 11:
			itemIcon.SetSelected(selected: true);
			itemIcon.SetItemActionType(11, delegate(BaseModelData bmd)
			{
				OnItemDeselected(model, bmd);
			});
			break;
		default:
			itemIcon.SetItemActionType(model.action);
			break;
		}
		model.itemIcon = itemIcon;
		if (updateViews != null)
		{
			updateViews();
		}
	}

	private void ToggleCraftItemModel(CraftItemModel craftItemModel, int action, UnityAction<CraftItemModel, BaseModelData> localCallback, UnityAction<CraftItemModel, BaseModelData> globalCallback)
	{
		bool flag = action == 11;
		craftItemModel.action = action;
		globalCallback(craftItemModel, craftItemModel.itemData);
		if (craftItemModel.itemIcon != null)
		{
			craftItemModel.itemIcon.SetSelected(!flag);
			craftItemModel.itemIcon.SetItemActionType(action, delegate(BaseModelData bmd)
			{
				localCallback(craftItemModel, bmd);
			});
		}
	}

	public void OnItemSelected(CraftItemModel newOrRecycled, BaseModelData baseModelData)
	{
		foreach (CraftItemModel item in (IEnumerable<CraftItemModel>)Data)
		{
			if (baseModelData == null || item.itemData.itemRef.Equals(baseModelData.itemRef))
			{
				ToggleCraftItemModel(item, 11, OnItemDeselected, onSelected);
			}
		}
		Refresh();
	}

	public void OnItemDeselected(CraftItemModel newOrRecycled, BaseModelData baseModelData)
	{
		foreach (CraftItemModel item in (IEnumerable<CraftItemModel>)Data)
		{
			if (baseModelData == null || item.itemData.itemRef.Equals(baseModelData.itemRef))
			{
				ToggleCraftItemModel(item, 10, OnItemSelected, onDeselected);
			}
		}
		Refresh();
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<CraftItemModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<CraftItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		CraftItemModel[] newItems = new CraftItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		CraftItemModel[] newItems = new CraftItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(CraftItemModel[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
