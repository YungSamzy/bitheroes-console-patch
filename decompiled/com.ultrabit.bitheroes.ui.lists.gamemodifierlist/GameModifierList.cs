using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.gamemodifierlist;

public class GameModifierList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<GameModifierItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GameModifierItem>(this);
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

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		GameModifierItem gameModifierItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.bonus.text = gameModifierItem.description;
	}

	public void AddItemsAt(int index, IList<GameModifierItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GameModifierItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GameModifierItem[] newItems = new GameModifierItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GameModifierItem[] newItems = new GameModifierItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GameModifierItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
