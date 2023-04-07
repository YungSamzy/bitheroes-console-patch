using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.ranklist;

public class RankList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Sprite normalBox;

	public Sprite highlightBox;

	public TrophyHandler trophyHandler;

	public SimpleDataHelper<LeaderboardItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<LeaderboardItem>(this);
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
		LeaderboardItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
		if (!IsMine(model))
		{
			newOrRecycled.UIHighlight.overrideSprite = normalBox;
			newOrRecycled.UIPoints.color = Color.white;
			newOrRecycled.UIPosition.color = Color.white;
			newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
			{
				OnTileClick(model);
			});
		}
		else
		{
			newOrRecycled.UIHighlight.overrideSprite = highlightBox;
			newOrRecycled.UIPoints.color = Color.yellow;
			newOrRecycled.UIPosition.color = Color.yellow;
		}
		newOrRecycled.UIPoints.text = model.points;
		newOrRecycled.UIPosition.text = model.position;
		newOrRecycled.UIName.text = model.name;
		trophyHandler.ReplaceTrophy(newOrRecycled.UItrohpy.gameObject, int.Parse(model.position));
	}

	private void OnTileClick(LeaderboardItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (model.data.type == 0)
		{
			GameData.instance.windowGenerator.ShowPlayer(model.data.id);
		}
	}

	public bool IsMine(LeaderboardItem model)
	{
		switch (model.data.type)
		{
		case 0:
			return model.data.id == GameData.instance.PROJECT.character.id;
		case 1:
			if (GameData.instance.PROJECT.character.guildData == null)
			{
				return false;
			}
			return model.data.id == GameData.instance.PROJECT.character.guildData.id;
		default:
			return false;
		}
	}

	public void AddItemsAt(int index, IList<LeaderboardItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<LeaderboardItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		LeaderboardItem[] newItems = new LeaderboardItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		LeaderboardItem[] newItems = new LeaderboardItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(LeaderboardItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
