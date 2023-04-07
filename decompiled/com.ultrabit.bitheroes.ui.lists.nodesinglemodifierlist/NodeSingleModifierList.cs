using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.nodesinglemodifierlist;

public class NodeSingleModifierList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<NodeSingleModifierItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<NodeSingleModifierItem>(this);
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
		NodeSingleModifierItem nodeSingleModifierItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.bonusDesc.text = nodeSingleModifierItem.desc;
		newOrRecycled.rectTransform.rect.Set(newOrRecycled.rectTransform.rect.x, newOrRecycled.rectTransform.rect.y, 50f, 50f);
	}

	public void AddItemsAt(int index, IList<NodeSingleModifierItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<NodeSingleModifierItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		NodeSingleModifierItem[] newItems = new NodeSingleModifierItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		NodeSingleModifierItem[] newItems = new NodeSingleModifierItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(NodeSingleModifierItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
