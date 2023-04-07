using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.battleresultslist;

public class BattleResultsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public SimpleDataHelper<BattleResultsItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<BattleResultsItem>(this);
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
		BattleResultsItem battleResultsItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		if (battleResultsItem.itemData.type == 1)
		{
			itemIcon.SetEquipmentData(battleResultsItem.itemData, null);
		}
		else
		{
			itemIcon.SetItemData(battleResultsItem.itemData);
		}
		if (GameData.instance.SAVE_STATE.GetEquipOnResultsTypes(GameData.instance.PROJECT.character.id, battleResultsItem.battleType))
		{
			itemIcon.SetItemActionType(battleResultsItem.itemData.itemRef.getItemTooltipType());
		}
		else
		{
			itemIcon.SetItemActionType(0);
		}
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<BattleResultsItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<BattleResultsItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		BattleResultsItem[] newItems = new BattleResultsItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		BattleResultsItem[] newItems = new BattleResultsItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(BattleResultsItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
