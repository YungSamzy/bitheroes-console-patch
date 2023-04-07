using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.dungeonentitieslist;

public class MyViewsHolder : BaseItemViewsHolder
{
	public DraggableItem draggableComponent;

	public RectTransform scalableViews;

	public override void CollectViews()
	{
		base.CollectViews();
		draggableComponent = root.GetComponent<DraggableItem>();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
	}
}
