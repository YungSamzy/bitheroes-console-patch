using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.instance.fishing;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.itemlist;

public class ItemList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	private UnityAction<ItemListModel> onClickDelegate;

	public SimpleDataHelper<ItemListModel> Data { get; private set; }

	public void StartList(UnityAction<ItemListModel> onClickDelegate = null)
	{
		this.onClickDelegate = onClickDelegate;
		if (Data == null)
		{
			Data = new SimpleDataHelper<ItemListModel>(this);
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

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<ItemListModel>(this);
			base.Start();
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		ItemListModel model = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		model.itemIcon = itemIcon;
		itemIcon.SetItemData(model.itemData, locked: false, model.itemData.qty, tintRarity: true, null, model.inserted);
		if (model.inserted)
		{
			if (model.compare)
			{
				itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
			}
			if (model.added)
			{
				itemIcon.SetItemActionType(model.itemData.itemRef.getItemTooltipType());
			}
			else if (model.select)
			{
				int type = model.itemData.type;
				if ((type == 1 || (uint)(type - 13) <= 1u) && GameData.instance.windowGenerator.HasDialogByClass(typeof(InstanceFishingInterface)))
				{
					itemIcon.SetItemActionType(10);
					itemIcon.AddOnItemIconActionPostExecuteCallback(delegate
					{
						onClickDelegate(model);
					});
				}
			}
			else
			{
				itemIcon.SetItemActionType(0);
			}
		}
		else
		{
			itemIcon.SetItemActionType(0);
		}
		if (itemIcon.qty <= 0 && !model.forceItemEnabled)
		{
			itemIcon.DisableItem(disable: true);
		}
		else
		{
			itemIcon.DisableItem(disable: false, changeAction: false);
		}
	}

	public void AddItemsAt(int index, IList<ItemListModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<ItemListModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ItemListModel[] newItems = new ItemListModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ItemListModel[] newItems = new ItemListModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ItemListModel[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
