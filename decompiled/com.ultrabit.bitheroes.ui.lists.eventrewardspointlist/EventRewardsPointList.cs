using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.eventrewardspointlist;

public class EventRewardsPointList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Sprite normalBox;

	public Sprite highlightBox;

	public bool currentEvent;

	public int currentPoints;

	private GameObject p;

	public SimpleDataHelper<EventRewardItemPoints> Data { get; private set; }

	public void StartList(GameObject _parent)
	{
		p = _parent;
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventRewardItemPoints>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventRewardItemPoints>(this);
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
		EventRewardItemPoints eventRewardItemPoints = Data[newOrRecycled.ItemIndex];
		string text = null;
		text = ((eventRewardItemPoints.minRank <= 0) ? "0" : Util.NumberFormat((float)eventRewardItemPoints.minRank / eventRewardItemPoints.divider));
		text = text + "<br>" + Language.GetString("ui_points");
		newOrRecycled.UIRank.text = text;
		newOrRecycled.UIBorder.sprite = ((currentEvent && currentPoints >= eventRewardItemPoints.minRank && currentPoints <= eventRewardItemPoints.maxRank) ? highlightBox : normalBox);
		for (int i = 0; i < newOrRecycled.itemContainer.childCount; i++)
		{
			bool flag = i + 1 <= eventRewardItemPoints.items.Count;
			newOrRecycled.itemContainer.GetChild(i).gameObject.SetActive(flag);
			if (flag)
			{
				ItemIcon itemIcon = newOrRecycled.itemContainer.GetChild(i).gameObject.GetComponent<ItemIcon>();
				if (itemIcon == null)
				{
					itemIcon = newOrRecycled.itemContainer.GetChild(i).gameObject.AddComponent<ItemIcon>();
				}
				itemIcon.SetItemData(eventRewardItemPoints.items[i]);
			}
		}
	}

	public void AddItemsAt(int index, IList<EventRewardItemPoints> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<EventRewardItemPoints> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		EventRewardItemPoints[] newItems = new EventRewardItemPoints[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		EventRewardItemPoints[] newItems = new EventRewardItemPoints[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(EventRewardItemPoints[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
