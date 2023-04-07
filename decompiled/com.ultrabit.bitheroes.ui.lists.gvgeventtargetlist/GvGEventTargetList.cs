using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.ui.gvg;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.gvgeventtargetlist;

public class GvGEventTargetList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private GvGEventTargetWindow eventTargetWindow;

	private UnityAction<EventTargetData> onSelectOpponent;

	public SimpleDataHelper<MyListItemModel> Data { get; private set; }

	public void InitList(UnityAction<EventTargetData> onSelectOpponent, GvGEventTargetWindow eventTargetWindow)
	{
		this.onSelectOpponent = onSelectOpponent;
		this.eventTargetWindow = eventTargetWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<MyListItemModel>(this);
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
			Data = new SimpleDataHelper<MyListItemModel>(this);
			base.Start();
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
	}

	public void AddItemsAt(int index, IList<MyListItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<MyListItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		MyListItemModel[] newItems = new MyListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		MyListItemModel[] newItems = new MyListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(MyListItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
