using System;
using Com.TheFallenGames.OSA.Core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsCompletedList;

[Serializable]
public class MyParams : BaseParams
{
	public RectTransform AchievementCompletedTile;

	public RectTransform AchievementDificultyViewTile;

	public override void InitIfNeeded(IOSA iAdapter)
	{
		base.InitIfNeeded(iAdapter);
	}
}
