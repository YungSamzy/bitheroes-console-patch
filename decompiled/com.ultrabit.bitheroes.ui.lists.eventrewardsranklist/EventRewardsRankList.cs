using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.eventrewardsranklist;

public class EventRewardsRankList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Sprite normalBox;

	public Sprite highlightBox;

	private GameObject p;

	public SimpleDataHelper<EventRewardItemRank> Data { get; private set; }

	public void StartList(GameObject _parent)
	{
		p = _parent;
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventRewardItemRank>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventRewardItemRank>(this);
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
		EventRewardItemRank eventRewardItemRank = Data[newOrRecycled.ItemIndex];
		string text = "";
		if (eventRewardItemRank.displayRank)
		{
			text = Language.GetString("ui_rank") + "<br>";
			text = ((eventRewardItemRank.rewardRef.max >= int.MaxValue) ? (text + Util.NumberFormat(eventRewardItemRank.rewardRef.min) + "+") : (text + ((eventRewardItemRank.rewardRef.max != eventRewardItemRank.rewardRef.min) ? (Util.NumberFormat(eventRewardItemRank.rewardRef.min) + "-" + Util.NumberFormat(eventRewardItemRank.rewardRef.max)) : Util.NumberFormat(eventRewardItemRank.rewardRef.max))));
			if (eventRewardItemRank.rank >= eventRewardItemRank.rewardRef.min && eventRewardItemRank.rank <= eventRewardItemRank.rewardRef.max && eventRewardItemRank.currentEvent && eventRewardItemRank.currentZone)
			{
				newOrRecycled.UIBorder.sprite = highlightBox;
			}
			else
			{
				newOrRecycled.UIBorder.sprite = normalBox;
			}
		}
		else
		{
			float num = 1f;
			if (eventRewardItemRank.eventRef.eventType == 6)
			{
				num = eventRewardItemRank.eventRef.divider;
			}
			text = ((eventRewardItemRank.rewardRef.min <= 0) ? "0" : Util.NumberFormat((float)eventRewardItemRank.rewardRef.min / num));
			text = text + "<br>" + Language.GetString("ui_points");
			if (eventRewardItemRank.points >= eventRewardItemRank.rewardRef.min && eventRewardItemRank.points <= eventRewardItemRank.rewardRef.max && eventRewardItemRank.currentEvent && eventRewardItemRank.currentZone)
			{
				newOrRecycled.UIBorder.sprite = highlightBox;
			}
			else
			{
				newOrRecycled.UIBorder.sprite = normalBox;
			}
		}
		newOrRecycled.UIRank.text = text;
		if (newOrRecycled.UIBorder.sprite == highlightBox)
		{
			newOrRecycled.UIRank.color = Color.yellow;
		}
		else if (newOrRecycled.UIBorder.sprite == normalBox)
		{
			newOrRecycled.UIRank.color = Color.white;
		}
		for (int i = 0; i < newOrRecycled.itemContainer.childCount; i++)
		{
			bool flag = i + 1 <= eventRewardItemRank.rewardRef.items.Count;
			newOrRecycled.itemContainer.GetChild(i).gameObject.SetActive(flag);
			if (flag)
			{
				ItemIcon itemIcon = newOrRecycled.itemContainer.GetChild(i).gameObject.GetComponent<ItemIcon>();
				if (itemIcon == null)
				{
					itemIcon = newOrRecycled.itemContainer.GetChild(i).gameObject.AddComponent<ItemIcon>();
				}
				itemIcon.SetItemData(eventRewardItemRank.rewardRef.items[i]);
				if (eventRewardItemRank.rewardRef.items[i].itemRef.isViewable() && eventRewardItemRank.rewardRef.items[i].itemRef.hasContents())
				{
					itemIcon.SetItemActionType(7);
				}
				else
				{
					itemIcon.SetItemActionType(0);
				}
			}
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<EventRewardItemRank> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<EventRewardItemRank> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		EventRewardItemRank[] newItems = new EventRewardItemRank[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		EventRewardItemRank[] newItems = new EventRewardItemRank[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(EventRewardItemRank[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
