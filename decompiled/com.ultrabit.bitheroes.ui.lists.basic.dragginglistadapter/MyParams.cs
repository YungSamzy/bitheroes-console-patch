using System;
using Com.TheFallenGames.OSA.CustomParams;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.basic.dragginglistadapter;

[Serializable]
public class MyParams : BaseParamsWithPrefab
{
	[Range(0f, 1f)]
	public float minDistFromEdgeToBeginScroll01 = 0.2f;

	public float maxScrollSpeedOnBoundary = 3000f;

	public Color outsideColorTint = new Color(1f, 1f, 1f, 0.8f);
}
