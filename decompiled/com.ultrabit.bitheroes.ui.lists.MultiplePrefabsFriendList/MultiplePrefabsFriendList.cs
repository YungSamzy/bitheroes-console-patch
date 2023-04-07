using System;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.MultiplePrefabsFriendList;

public class MultiplePrefabsFriendList : OSA<MyParams, BaseVH>
{
	public TextMeshProUGUI emptyText;

	private int _IndexOfCurrentlyExpandedItem;

	public SimpleDataHelper<BaseModel> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<BaseModel>(this);
			base.Start();
		}
		if (emptyText == null)
		{
			emptyText = base.transform.Find("EmptyTxt").GetComponent<TextMeshProUGUI>();
			D.LogWarning(GetType().Name + " :: Empty text prefab not found", forceLoggly: true);
		}
		emptyText.SetText(Language.GetString("ui_friend_empty_list"));
	}

	protected override void Start()
	{
		StartList();
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

	public override void ChangeItemsCount(ItemCountChangeMode changeMode, int itemsCount, int indexIfInsertingOrRemoving = -1, bool contentPanelEndEdgeStationary = false, bool keepVelocity = false)
	{
		_IndexOfCurrentlyExpandedItem = -1;
		base.ChangeItemsCount(changeMode, itemsCount, indexIfInsertingOrRemoving, contentPanelEndEdgeStationary, keepVelocity);
	}

	protected override BaseVH CreateViewsHolder(int itemIndex)
	{
		Type cachedType = Data[itemIndex].CachedType;
		if (cachedType == typeof(FriendTileModel))
		{
			FriendTileVH friendTileVH = new FriendTileVH();
			friendTileVH.Init(_Params.FriendTile, _Params.Content, itemIndex);
			return friendTileVH;
		}
		if (cachedType == typeof(FriendRequestViewTileModel))
		{
			FriendRequestViewTileVH friendRequestViewTileVH = new FriendRequestViewTileVH();
			friendRequestViewTileVH.Init(_Params.FriendRequestViewTile, _Params.Content, itemIndex);
			return friendRequestViewTileVH;
		}
		throw new InvalidOperationException("Unrecognized model type: " + cachedType.Name);
	}

	protected override void UpdateViewsHolder(BaseVH newOrRecycled)
	{
		BaseModel model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UpdateViews(model);
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	protected override void OnItemIndexChangedDueInsertOrRemove(BaseVH shiftedViewsHolder, int oldIndex, bool wasInsert, int removeOrInsertIndex)
	{
		Debug.Log("OnItemIndexChangedDueInsertOrRemove");
		base.OnItemIndexChangedDueInsertOrRemove(shiftedViewsHolder, oldIndex, wasInsert, removeOrInsertIndex);
	}

	protected override bool IsRecyclable(BaseVH potentiallyRecyclable, int indexOfItemThatWillBecomeVisible, double sizeOfItemThatWillBecomeVisible)
	{
		BaseModel baseModel = Data[indexOfItemThatWillBecomeVisible];
		return potentiallyRecyclable.CanPresentModelType(baseModel.CachedType);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
