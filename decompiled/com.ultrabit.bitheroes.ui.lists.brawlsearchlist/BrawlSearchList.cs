using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.brawl;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.brawlsearchlist;

public class BrawlSearchList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private BrawlWindow brawlWindow;

	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<BrawlSearchItem> Data { get; private set; }

	public void InitList(BrawlWindow brawlWindow)
	{
		this.brawlWindow = brawlWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<BrawlSearchItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_brawl_list_empty");
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
		BrawlSearchItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIName.text = model.brawlRoom.difficultyRef.brawlRef.name;
		newOrRecycled.UITier.text = Language.GetString("ui_tier_count", new string[1] { model.brawlRoom.difficultyRef.tierRef.id.ToString() });
		newOrRecycled.UIDifficulty.text = model.brawlRoom.difficultyRef.difficultyRef.coloredName;
		newOrRecycled.UIEnergy.text = Util.NumberFormat(model.brawlRoom.difficultyRef.difficultyRef.seals);
		newOrRecycled.UIJoin.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_join");
		newOrRecycled.UIJoin.onClick.AddListener(delegate
		{
			OnJoinBtn(model);
		});
		for (int i = 1; i <= model.brawlRoom.slots.Count; i++)
		{
			int sortLayer = i + newOrRecycled.ItemIndex * 5 + brawlWindow.sortingLayer + 1;
			Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/brawl/" + typeof(BrawlSearchPlayerTile).Name));
			obj.SetParent(newOrRecycled.UIPlayers, worldPositionStays: false);
			obj.GetComponent<BrawlSearchPlayerTile>().LoadDetails(model.brawlRoom.slots[i - 1], sortLayer);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnJoinBtn(BrawlSearchItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoBrawlJoin(model.brawlRoom.index);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIJoin.onClick.RemoveAllListeners();
		for (int i = 0; i < inRecycleBinOrVisible.UIPlayers.childCount; i++)
		{
			Object.Destroy(inRecycleBinOrVisible.UIPlayers.GetChild(i).gameObject);
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<BrawlSearchItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<BrawlSearchItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		BrawlSearchItem[] newItems = new BrawlSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		BrawlSearchItem[] newItems = new BrawlSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(BrawlSearchItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
