using System;
using Com.TheFallenGames.OSA.Core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.multiplePrefabsAchievementsList;

[Serializable]
public class MyParams : BaseParams
{
	public RectTransform AchievementTile;

	public RectTransform AchievementDificultyViewTile;

	public override void InitIfNeeded(IOSA iAdapter)
	{
		base.InitIfNeeded(iAdapter);
	}
}
