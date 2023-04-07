using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.dailyquestslist;

public class DailyQuestsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<DailyQuestItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<DailyQuestItem>(this);
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
		DailyQuestItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIName.text = model.questData.questRef.rarityRef.ConvertString(model.questData.questRef.name + ":");
		newOrRecycled.UIDesc.text = Util.parseDailyQuestString(model.questData.questRef.desc, model.questData.questRef);
		if (model.questData.completed)
		{
			newOrRecycled.UILoot.gameObject.SetActive(value: true);
			newOrRecycled.UIProgressBar.gameObject.SetActive(value: false);
			newOrRecycled.UILoot.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
			newOrRecycled.UILoot.onClick.AddListener(delegate
			{
				OnLootBtn(model);
			});
		}
		else
		{
			newOrRecycled.UILoot.gameObject.SetActive(value: false);
			newOrRecycled.UIProgressBar.gameObject.SetActive(value: true);
			Color color = Color.white;
			ColorUtility.TryParseHtmlString("#" + model.questData.questRef.rarityRef.textColor, out color);
			newOrRecycled.UIProgressBarFill.color = color;
			newOrRecycled.UIProgress.text = Util.NumberFormat(model.questData.progress) + "/" + Util.NumberFormat(model.questData.questRef.amount);
			newOrRecycled.UIProgressBarFill.GetComponent<RegularBarFill>().UpdateBar(model.questData.progress, model.questData.questRef.amount);
		}
		if (model.questData.questRef.items.Count <= 0)
		{
			newOrRecycled.UIItemIcon.gameObject.SetActive(value: false);
			return;
		}
		newOrRecycled.UIItemIcon.gameObject.SetActive(value: true);
		ItemData data = model.questData.questRef.items[0];
		ItemIcon itemIcon = newOrRecycled.UIItemIcon.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.UIItemIcon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(data);
		itemIcon.SetItemActionType(0);
	}

	public void OnLootBtn(DailyQuestItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoDailyQuestLoot(model.questData);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UILoot.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<DailyQuestItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<DailyQuestItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		DailyQuestItem[] newItems = new DailyQuestItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		DailyQuestItem[] newItems = new DailyQuestItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(DailyQuestItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
