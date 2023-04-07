using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.basic.dragginglistadapter;

public class MyViewsHolder : BaseItemViewsHolder
{
	public DraggableItem draggableComponent;

	public RectTransform scalableViews;

	public Image background;

	public override void CollectViews()
	{
		base.CollectViews();
		draggableComponent = root.GetComponent<DraggableItem>();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		scalableViews.GetComponentAtPath<Image>("Background", out background);
	}
}
