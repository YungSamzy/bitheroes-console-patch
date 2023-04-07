using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.guildinventorycosmeticslist;

public class GuildInventoryCosmeticsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<GuildCosmeticItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildCosmeticItem>(this);
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
		GuildCosmeticItem guildCosmeticItem = Data[newOrRecycled.ItemIndex];
		string @string = guildCosmeticItem.cosmeticRef.name;
		string text = Language.GetString(guildCosmeticItem.cosmeticRef.typeRef.displayName);
		newOrRecycled.UIAsset.overrideSprite = guildCosmeticItem.cosmeticRef.GetSpriteIcon();
		if (!unlocked(guildCosmeticItem))
		{
			newOrRecycled.UIName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-4.8f, 0f);
			if (!AppInfo.TESTING)
			{
				@string = Language.GetString("ui_question_marks");
			}
			text = null;
			newOrRecycled.UIName.color = new Color(0.6f, 0.6f, 0.6f);
			newOrRecycled.UILevel.color = Color.red;
			Util.SetImageAlpha(newOrRecycled.UIFrame, alpha: true);
			newOrRecycled.UIAsset.color = Color.black;
		}
		else
		{
			newOrRecycled.UIName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-6.8f, 2.8f);
			newOrRecycled.UIName.color = Color.white;
			newOrRecycled.UILevel.color = Color.white;
			Util.SetImageAlpha(newOrRecycled.UIFrame, alpha: false);
			newOrRecycled.UIAsset.color = Color.white;
		}
		if (text != null)
		{
			newOrRecycled.UIType.gameObject.SetActive(value: true);
			newOrRecycled.UIType.text = text;
		}
		else
		{
			newOrRecycled.UIType.gameObject.SetActive(value: false);
		}
		newOrRecycled.UIName.text = @string;
		newOrRecycled.UILevel.text = Language.GetString("ui_current_lvl", new string[1] { Util.NumberFormat(guildCosmeticItem.cosmeticRef.guildLvlReq) });
	}

	public bool unlocked(GuildCosmeticItem model)
	{
		return model.data.level >= model.cosmeticRef.guildLvlReq;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildCosmeticItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildCosmeticItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildCosmeticItem[] newItems = new GuildCosmeticItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildCosmeticItem[] newItems = new GuildCosmeticItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildCosmeticItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
