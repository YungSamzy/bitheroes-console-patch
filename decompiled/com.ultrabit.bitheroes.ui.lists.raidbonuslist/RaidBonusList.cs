using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.raidbonuslist;

public class RaidBonusList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<RaidBonusItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<RaidBonusItem>(this);
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
		RaidBonusItem raidBonusItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.txtBonus.text = raidBonusItem.bonus;
	}

	public void AddItemsAt(int index, IList<RaidBonusItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<RaidBonusItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		RaidBonusItem[] newItems = new RaidBonusItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		RaidBonusItem[] newItems = new RaidBonusItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(RaidBonusItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
