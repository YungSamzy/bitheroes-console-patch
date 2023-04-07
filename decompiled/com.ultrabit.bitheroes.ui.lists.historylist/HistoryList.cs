using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.historylist;

public class HistoryList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Sprite normalBox;

	public Sprite highlightBox;

	public SimpleDataHelper<HistoryListModel> Data { get; private set; }

	protected override void Start()
	{
		Data = new SimpleDataHelper<HistoryListModel>(this);
		base.Start();
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
		HistoryListModel model = Data[newOrRecycled.ItemIndex];
		if (model.id != GameData.instance.PROJECT.character.id)
		{
			newOrRecycled.image.sprite = normalBox;
		}
		else
		{
			newOrRecycled.image.sprite = highlightBox;
		}
		newOrRecycled.UIName.text = model.name;
		newOrRecycled.UIPoint.text = model.point;
		newOrRecycled.UITime.text = model.time;
		newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
		newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
		{
			GameData.instance.windowGenerator.ShowPlayer(model.id);
		});
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<HistoryListModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<HistoryListModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		HistoryListModel[] newItems = new HistoryListModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		HistoryListModel[] newItems = new HistoryListModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(HistoryListModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
