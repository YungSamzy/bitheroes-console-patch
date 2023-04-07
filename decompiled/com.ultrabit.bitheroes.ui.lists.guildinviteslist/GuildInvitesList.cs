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

namespace com.ultrabit.bitheroes.ui.lists.guildinviteslist;

public class GuildInvitesList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<GuildInviteItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildInviteItem>(this);
			base.Start();
		}
		if (emptyText == null)
		{
			emptyText = base.transform.Find("EmptyTxt").GetComponent<TextMeshProUGUI>();
			D.LogWarning(GetType().Name + " :: Empty text prefab not found", forceLoggly: true);
		}
		emptyText.text = Language.GetString("ui_item_list_empty");
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
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
		GuildInviteItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIAccept.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
		newOrRecycled.UIDecline.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		newOrRecycled.UIAccept.onClick.AddListener(delegate
		{
			OnAcceptBtn(model);
		});
		newOrRecycled.UIDecline.onClick.AddListener(delegate
		{
			OnDeclineBtn(model);
		});
		newOrRecycled.UIName.text = Util.ParseName(model.guildInfo.name, model.guildInfo.initials);
		newOrRecycled.UIMembers.text = Util.NumberFormat(model.guildInfo.members);
		newOrRecycled.UILevel.text = Util.NumberFormat(model.guildInfo.level);
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnAcceptBtn(GuildInviteItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoGuildInviteAccept(model.guildInfo.id);
	}

	private void OnDeclineBtn(GuildInviteItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoGuildInviteDecline(model.guildInfo.id);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIAccept.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIDecline.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildInviteItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildInviteItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildInviteItem[] newItems = new GuildInviteItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildInviteItem[] newItems = new GuildInviteItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildInviteItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
