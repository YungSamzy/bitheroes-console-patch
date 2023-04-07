using System;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;
using com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsList;

public class MultiplePrefabsAchievementsList : OSA<MyParams, BaseVH>
{
	public TrophyHandler trophyHandler;

	private int _IndexOfCurrentlyExpandedItem;

	public SimpleDataHelper<BaseModel> Data { get; private set; }

	public BaseVH AchievementTileVH { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<BaseModel>(this);
			base.Start();
		}
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
		if (cachedType == typeof(AchievementTileModel))
		{
			AchievementTileVH achievementTileVH = new AchievementTileVH();
			achievementTileVH.Init(_Params.AchievementTile, _Params.Content, itemIndex);
			return achievementTileVH;
		}
		if (cachedType == typeof(AchievementDificultyViewTileModel))
		{
			AchievementDifficultyViewTileVH achievementDifficultyViewTileVH = new AchievementDifficultyViewTileVH();
			achievementDifficultyViewTileVH.Init(_Params.AchievementDificultyViewTile, _Params.Content, itemIndex);
			return achievementDifficultyViewTileVH;
		}
		throw new InvalidOperationException("Unrecognized model type: " + cachedType.Name);
	}

	protected override void UpdateViewsHolder(BaseVH newOrRecycled)
	{
		BaseModel baseModel = Data[newOrRecycled.ItemIndex];
		if (newOrRecycled is AchievementTileVH achievementTileVH)
		{
			achievementTileVH.UpdateACTViews(baseModel, trophyHandler);
		}
		else
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
