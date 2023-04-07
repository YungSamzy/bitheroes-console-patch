using System;
using Com.TheFallenGames.OSA.Core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.MultiplePrefabsFriendList;

[Serializable]
public class MyParams : BaseParams
{
	public RectTransform FriendTile;

	public RectTransform FriendRequestViewTile;

	public override void InitIfNeeded(IOSA iAdapter)
	{
		base.InitIfNeeded(iAdapter);
	}
}
