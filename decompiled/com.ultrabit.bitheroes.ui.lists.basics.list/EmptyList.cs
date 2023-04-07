using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;

namespace com.ultrabit.bitheroes.ui.lists.basics.list;

public class EmptyList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<MyListItemModel> Data { get; private set; }

	protected override void Start()
	{
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		return new MyListItemViewsHolder();
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
	}

	public void AddItemsAt(int index, IList<MyListItemModel> items)
	{
	}

	public void RemoveItemsFrom(int index, int count)
	{
	}

	public void SetItems(IList<MyListItemModel> items)
	{
	}

	private void RetrieveDataAndUpdate(int count)
	{
	}

	private void OnDataRetrieved(MyListItemModel[] newItems)
	{
	}
}
