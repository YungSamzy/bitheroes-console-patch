using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Util.ItemDragging;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamlistondungeon;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public DraggableItem draggableComponent;

	public RectTransform scalableViews;

	public Image healthBar;

	public Image shieldBar;

	public Image meterBar;

	public Image frame;

	public Image rarityColor;

	public Image consumableIcon;

	public Image disableColor;

	public RectTransform placeholderDisplay;

	public SpriteMask assetMask0;

	public override void CollectViews()
	{
		base.CollectViews();
		draggableComponent = root.GetComponent<DraggableItem>();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		scalableViews.GetComponentAtPath<Image>("HealthBar", out healthBar);
		scalableViews.GetComponentAtPath<Image>("ShieldBar", out shieldBar);
		scalableViews.GetComponentAtPath<Image>("MeterBar", out meterBar);
		scalableViews.GetComponentAtPath<Image>("Frame", out frame);
		scalableViews.GetComponentAtPath<Image>("Frame/RarityColor", out rarityColor);
		scalableViews.GetComponentAtPath<Image>("ConsumableIcon", out consumableIcon);
		scalableViews.GetComponentAtPath<Image>("DisableColor", out disableColor);
		scalableViews.GetComponentAtPath<RectTransform>("PlaceholderDisplay", out placeholderDisplay);
		scalableViews.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
	}
}
