using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.zonebonuslist;

public class ZoneBonusList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<ZoneBonusItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<ZoneBonusItem>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		InitList();
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
		ZoneBonusItem zoneBonusItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.desc.text = zoneBonusItem.desc;
	}

	public void AddItemsAt(int index, IList<ZoneBonusItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ZoneBonusItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ZoneBonusItem[] newItems = new ZoneBonusItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ZoneBonusItem[] newItems = new ZoneBonusItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ZoneBonusItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
