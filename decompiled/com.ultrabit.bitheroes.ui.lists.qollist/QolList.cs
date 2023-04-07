using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.qollist;

public class QolList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public SimpleDataHelper<MyGridItemModel> Data { get; private set; }

	protected override void Start()
	{
		Data = new SimpleDataHelper<MyGridItemModel>(this);
		base.Start();
		RetrieveDataAndUpdate(50);
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
	}

	public void AddItemsAt(int index, IList<MyGridItemModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<MyGridItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		MyGridItemModel[] newItems = new MyGridItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		MyGridItemModel[] newItems = new MyGridItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(MyGridItemModel[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
