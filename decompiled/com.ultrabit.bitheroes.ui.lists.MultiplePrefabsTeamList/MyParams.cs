using System;
using Com.TheFallenGames.OSA.Core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;

[Serializable]
public class MyParams : BaseParams
{
	public RectTransform TeamTile;

	public RectTransform GroupViewTile;

	public override void InitIfNeeded(IOSA iAdapter)
	{
		base.InitIfNeeded(iAdapter);
	}
}
