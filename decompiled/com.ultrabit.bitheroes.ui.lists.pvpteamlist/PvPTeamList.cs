using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.pvp;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.pvpteamlist;

public class PvPTeamList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<PvPTeamItem> _regularClickCallback;

	private PvPEventWindow _pvpEventWindow;

	public SimpleDataHelper<PvPTeamItem> Data { get; private set; }

	public void InitList(UnityAction<PvPTeamItem> regularClickCallback, PvPEventWindow pvpEventWindow)
	{
		_regularClickCallback = regularClickCallback;
		_pvpEventWindow = pvpEventWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<PvPTeamItem>(this);
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
		PvPTeamItem model = Data[newOrRecycled.ItemIndex];
		ItemData data = ((model.data is ItemRef) ? new ItemData(model.data as ItemRef, 1) : null);
		bool num = model.data is CharacterData;
		RarityRef forcedRarity = ((num && ((CharacterData)model.data).isIMXG0) ? ((CharacterData)model.data).nftRarity : null);
		newOrRecycled.itemIcon.SetItemData(data, locked: false, -1, tintRarity: true, null, showRanks: true, emptySlotFull: false, isCosmetic: false, forcedRarity);
		if (num)
		{
			CharacterDisplay characterDisplay = ((CharacterData)model.data).toCharacterDisplay(0.8f, displayMount: false, null, enableLoading: false);
			characterDisplay.HideMaskedElements();
			characterDisplay.ConvertToIcon(_pvpEventWindow.sortingLayer + 1 + newOrRecycled.ItemIndex, newOrRecycled.itemIcon.transform);
			newOrRecycled.itemIcon.SetItemActionType(10, delegate
			{
				_regularClickCallback(model);
			});
		}
		else if (model.data is FamiliarRef)
		{
			newOrRecycled.itemIcon.SetItemActionType(7);
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		CharacterDisplay componentInChildren = inRecycleBinOrVisible.root.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			Object.Destroy(componentInChildren.gameObject);
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<PvPTeamItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<PvPTeamItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		PvPTeamItem[] newItems = new PvPTeamItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		PvPTeamItem[] newItems = new PvPTeamItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(PvPTeamItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
