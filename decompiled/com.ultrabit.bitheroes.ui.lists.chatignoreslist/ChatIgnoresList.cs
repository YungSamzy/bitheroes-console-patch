using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.chatignoreslist;

public class ChatIgnoresList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<ChatIgnoreItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<ChatIgnoreItem>(this);
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
		ChatIgnoreItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIName.text = model.playerData.name;
		newOrRecycled.root.gameObject.GetComponent<Button>().onClick.AddListener(delegate
		{
			OnTileClick(model);
		});
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnTileClick(ChatIgnoreItem model)
	{
		GameData.instance.windowGenerator.ShowPlayer(model.playerData.charID);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
		inRecycleBinOrVisible.root.gameObject.GetComponent<HoverImage>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ChatIgnoreItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ChatIgnoreItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ChatIgnoreItem[] newItems = new ChatIgnoreItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ChatIgnoreItem[] newItems = new ChatIgnoreItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ChatIgnoreItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
