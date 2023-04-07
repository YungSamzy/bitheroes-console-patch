using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.chatmuteloglist;

public class ChatMuteLogList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	[HideInInspector]
	private UnityAction<object> onTileSelectedCallback;

	private const uint COLOR_MIN = 65280u;

	private const uint COLOR_MAX = 16711680u;

	public SimpleDataHelper<ChatMuteLogItem> Data { get; private set; }

	public void InitList(UnityAction<object> onTileSelectedCallback)
	{
		this.onTileSelectedCallback = onTileSelectedCallback;
		if (Data == null)
		{
			Data = new SimpleDataHelper<ChatMuteLogItem>(this);
			base.Start();
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
		ChatMuteLogItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.button.onClick.AddListener(delegate
		{
			OnTileSelected(model);
		});
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		int chatMuteSecondsIndex = VariableBook.GetChatMuteSecondsIndex(model.muteData.duration);
		int num = VariableBook.chatMuteSeconds.Count - 1;
		float progress = (float)chatMuteSecondsIndex / (float)num;
		Color color = Util.UIntToUnityColor(Util.InterpolateColor(65280u, 16711680u, progress));
		newOrRecycled.dateTxt.text = model.muteData.date.ToString();
		newOrRecycled.characterNameTxt.text = model.muteData.characterName;
		newOrRecycled.moderatorNameTxt.text = model.muteData.moderatorName;
		newOrRecycled.durationTxt.text = Util.colorString(Util.TimeFormatShort(model.muteData.duration), ColorUtility.ToHtmlStringRGB(color));
		newOrRecycled.reasonTxt.text = VariableBook.GetChatMuteReason(model.muteData.reason);
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnTileSelected(ChatMuteLogItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		onTileSelectedCallback(model);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.button.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ChatMuteLogItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ChatMuteLogItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ChatMuteLogItem[] newItems = new ChatMuteLogItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ChatMuteLogItem[] newItems = new ChatMuteLogItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ChatMuteLogItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
