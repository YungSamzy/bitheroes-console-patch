using System;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.team;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;

public class MultiplePrefabsTeamList : OSA<MyParams, BaseVH>
{
	private int groupSize;

	private int totalSlots;

	private int _IndexOfCurrentlyExpandedItem;

	public SimpleDataHelper<BaseModel> Data { get; private set; }

	public void InitList(int groupSize, int totalSlots)
	{
		if (Data == null)
		{
			this.groupSize = groupSize;
			this.totalSlots = totalSlots;
			Data = new SimpleDataHelper<BaseModel>(this);
			base.Start();
		}
	}

	public List<TeammateData> GetTeammateDataList()
	{
		List<TeammateData> list = new List<TeammateData>();
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data[i].GetType() == typeof(TeamListItemModel))
			{
				TeammateData teammateData = (Data[i] as TeamListItemModel).teammateData;
				list.Add(teammateData);
			}
		}
		return list;
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

	private void SwapUp(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		BaseVH itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible == null || itemViewsHolderIfVisible.ItemIndex <= 0 || Data.Count <= 0)
		{
			return;
		}
		bool flag = false;
		if (Data[itemViewsHolderIfVisible.ItemIndex - 1].GetType() == typeof(GroupTileModel))
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 2);
			SwapSlot(Data[itemViewsHolderIfVisible.ItemIndex] as TeamListItemModel, Data[itemViewsHolderIfVisible.ItemIndex - 2] as TeamListItemModel);
			flag = true;
		}
		else if (Data[itemViewsHolderIfVisible.ItemIndex - 1].GetType() == typeof(TeamListItemModel))
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 1);
			SwapSlot(Data[itemViewsHolderIfVisible.ItemIndex] as TeamListItemModel, Data[itemViewsHolderIfVisible.ItemIndex - 1] as TeamListItemModel);
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void SwapDown(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		BaseVH itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible == null || itemViewsHolderIfVisible.ItemIndex >= Data.Count - 1)
		{
			return;
		}
		bool flag = false;
		if (Data[itemViewsHolderIfVisible.ItemIndex + 1].GetType() == typeof(GroupTileModel))
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 2);
			SwapSlot(Data[itemViewsHolderIfVisible.ItemIndex] as TeamListItemModel, Data[itemViewsHolderIfVisible.ItemIndex + 2] as TeamListItemModel);
			flag = true;
		}
		else if (Data[itemViewsHolderIfVisible.ItemIndex + 1].GetType() == typeof(TeamListItemModel))
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 1);
			SwapSlot(Data[itemViewsHolderIfVisible.ItemIndex] as TeamListItemModel, Data[itemViewsHolderIfVisible.ItemIndex + 1] as TeamListItemModel);
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
	}

	private void SwapSlot(TeamListItemModel a, TeamListItemModel b)
	{
		int slot = a.slot;
		a.slot = b.slot;
		b.slot = slot;
	}

	public override void ChangeItemsCount(ItemCountChangeMode changeMode, int itemsCount, int indexIfInsertingOrRemoving = -1, bool contentPanelEndEdgeStationary = false, bool keepVelocity = false)
	{
		_IndexOfCurrentlyExpandedItem = -1;
		base.ChangeItemsCount(changeMode, itemsCount, indexIfInsertingOrRemoving, contentPanelEndEdgeStationary, keepVelocity);
	}

	protected override BaseVH CreateViewsHolder(int itemIndex)
	{
		Type cachedType = Data[itemIndex].CachedType;
		if (cachedType == typeof(TeamListItemModel))
		{
			TeamTileVH teamTileVH = new TeamTileVH();
			teamTileVH.Init(_Params.TeamTile, _Params.Content, itemIndex);
			return teamTileVH;
		}
		if (cachedType == typeof(GroupTileModel))
		{
			GroupViewTileVH groupViewTileVH = new GroupViewTileVH();
			groupViewTileVH.Init(_Params.GroupViewTile, _Params.Content, itemIndex);
			return groupViewTileVH;
		}
		throw new InvalidOperationException("Unrecognized model type: " + cachedType.Name);
	}

	protected override void UpdateViewsHolder(BaseVH newOrRecycled)
	{
		BaseModel baseModel = Data[newOrRecycled.ItemIndex];
		if (baseModel != null)
		{
			newOrRecycled.UpdateViews(baseModel);
		}
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
