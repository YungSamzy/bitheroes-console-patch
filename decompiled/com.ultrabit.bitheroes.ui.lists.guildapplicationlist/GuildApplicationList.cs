using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.guildapplicationlist;

public class GuildApplicationList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<GuildApplicationItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildApplicationItem>(this);
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
		GuildApplicationItem model = Data[newOrRecycled.ItemIndex];
		if (model.guildInfo.open)
		{
			newOrRecycled.UIJoin.gameObject.SetActive(value: true);
			newOrRecycled.UIJoin.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_join");
			newOrRecycled.UIJoin.onClick.AddListener(delegate
			{
				OnJoinBtn(model);
			});
			newOrRecycled.UIApply.gameObject.SetActive(value: false);
		}
		else
		{
			newOrRecycled.UIApply.gameObject.SetActive(value: true);
			newOrRecycled.UIApply.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_apply");
			newOrRecycled.UIApply.onClick.AddListener(delegate
			{
				OnApplyBtn(model);
			});
			newOrRecycled.UIJoin.gameObject.SetActive(value: false);
		}
		Util.SetButton(newOrRecycled.UIJoin, !model.selected);
		Util.SetButton(newOrRecycled.UIApply, !model.selected);
		newOrRecycled.UIName.text = Util.ParseName(model.guildInfo.name, model.guildInfo.initials);
		newOrRecycled.UIMembers.text = Util.NumberFormat(model.guildInfo.members);
		newOrRecycled.UILevel.text = Util.NumberFormat(model.guildInfo.level);
	}

	private void OnJoinBtn(GuildApplicationItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		model.selected = true;
		GameData.instance.PROJECT.DoGuildApply(model.guildInfo.id);
	}

	private void OnApplyBtn(GuildApplicationItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		model.selected = true;
		GameData.instance.PROJECT.DoGuildApply(model.guildInfo.id);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIApply.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIJoin.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildApplicationItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildApplicationItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildApplicationItem[] newItems = new GuildApplicationItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildApplicationItem[] newItems = new GuildApplicationItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildApplicationItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
