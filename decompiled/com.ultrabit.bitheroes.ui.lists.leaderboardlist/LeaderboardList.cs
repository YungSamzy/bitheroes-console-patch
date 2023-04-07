using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.leaderboardlist;

public class LeaderboardList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
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
		newOrRecycled.root.GetComponent<HoverImages>().active = false;
		if (newOrRecycled.UIPosition.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.UIPosition.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.UIName.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.UIName.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.UIPoints.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.UIPoints.gameObject.AddComponent<TextBrightness>();
		}
		List<TextMeshProUGUI> list = new List<TextMeshProUGUI>();
		list.Add(newOrRecycled.UIPosition);
		list.Add(newOrRecycled.UIName);
		list.Add(newOrRecycled.UIPoints);
		newOrRecycled.root.GetComponent<HoverImages>().SetTexts(list);
		newOrRecycled.UIPosition.text = Util.NumberFormat(model.data.rank, abbreviate: false);
		newOrRecycled.UIName.text = model.data.parsedName;
		newOrRecycled.UIPoints.text = Util.NumberFormat(model.data.value / model.divider, abbreviate: false);
		if (isMine(model))
		{
			newOrRecycled.UIHighlight.overrideSprite = highlightBox;
			newOrRecycled.UIPosition.color = Color.yellow;
			newOrRecycled.UIPoints.color = Color.yellow;
		}
		else
		{
			if (model.data.type == 0)
			{
				newOrRecycled.root.gameObject.GetComponent<HoverImages>().active = true;
				newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
				{
					OnTileClick(model);
				});
			}
			newOrRecycled.UIPosition.color = Color.white;
			newOrRecycled.UIPoints.color = Color.white;
			newOrRecycled.UIHighlight.overrideSprite = normalBox;
		}
		trophyHandler.ReplaceTrophy(newOrRecycled.UItrohpy.gameObject, model.data.rank);
	}

	private bool isMine(LeaderboardItem model)
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

	private void OnTileClick(LeaderboardItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (model.data.type == 0)
		{
			GameData.instance.windowGenerator.ShowPlayer(model.data.id);
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<Button>().onClick.RemoveAllListeners();
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().ClearBrightness();
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().active = false;
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
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
