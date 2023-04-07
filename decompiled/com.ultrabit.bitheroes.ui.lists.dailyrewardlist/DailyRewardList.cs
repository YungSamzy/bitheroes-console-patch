using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.dailyrewardlist;

public class DailyRewardList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public Sprite normalBox;

	public Sprite highlightBox;

	public SimpleDataHelper<DailyRewardItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<DailyRewardItem>(this);
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

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		DailyRewardItem dailyRewardItem = Data[newOrRecycled.ItemIndex];
		ItemData itemData = dailyRewardItem.dailyRef.items[0];
		if (itemData != null)
		{
			newOrRecycled.UIIcon.gameObject.SetActive(value: true);
			if (newOrRecycled.root.gameObject.GetComponent<ItemIcon>() == null)
			{
				newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
			}
			newOrRecycled.root.gameObject.GetComponent<ItemIcon>().SetItemData(itemData, locked: false, itemData.qty, tintRarity: true, newOrRecycled.UIIconBg);
			newOrRecycled.root.gameObject.GetComponent<ItemIcon>().SetItemActionType(0);
		}
		else
		{
			if (newOrRecycled.root.gameObject.GetComponent<ItemIcon>() != null)
			{
				Object.Destroy(newOrRecycled.root.gameObject.GetComponent<ItemIcon>());
			}
			newOrRecycled.UIIcon.gameObject.SetActive(value: false);
		}
		newOrRecycled.UIName.text = Language.GetString("ui_day");
		int num = GameData.instance.PROJECT.character.dailyID + 1;
		if (dailyRewardItem.dailyRef.day < num)
		{
			newOrRecycled.root.GetComponent<CanvasGroup>().alpha = 0.25f;
			newOrRecycled.UIBorder.GetComponent<CanvasGroup>().alpha = 0.25f;
		}
		else
		{
			newOrRecycled.root.GetComponent<CanvasGroup>().alpha = 1f;
			newOrRecycled.UIBorder.GetComponent<CanvasGroup>().alpha = 0.5f;
		}
		if (num == dailyRewardItem.dailyRef.day)
		{
			newOrRecycled.UIDay.color = Color.yellow;
			newOrRecycled.UIBorder.overrideSprite = highlightBox;
			newOrRecycled.UIBorder.GetComponent<CanvasGroup>().alpha = 1f;
		}
		else
		{
			newOrRecycled.UIDay.color = Color.white;
			newOrRecycled.UIBorder.overrideSprite = normalBox;
		}
		newOrRecycled.UIDay.text = Util.NumberFormat(dailyRewardItem.dailyRef.day);
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<DailyRewardItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<DailyRewardItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		DailyRewardItem[] newItems = new DailyRewardItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		DailyRewardItem[] newItems = new DailyRewardItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(DailyRewardItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
