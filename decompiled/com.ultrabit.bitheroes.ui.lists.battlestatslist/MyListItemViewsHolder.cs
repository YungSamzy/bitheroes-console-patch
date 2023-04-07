using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.battlestatslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public RectTransform assetPlaceholder;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI teamTxt;

	public TextMeshProUGUI statTxt;

	public Button frame;

	public Image frameImage;

	public HoverImage frameHover;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public Image redBar;

	public Image greenBar;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("TeamTxt", out teamTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("StatTxt", out statTxt);
		root.GetComponentAtPath<Button>("Background", out frame);
		root.GetComponentAtPath<Image>("Background", out frameImage);
		root.GetComponentAtPath<HoverImage>("Background", out frameHover);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		root.GetComponentAtPath<Image>("RedBar", out redBar);
		root.GetComponentAtPath<Image>("GreenBar", out greenBar);
		root.GetComponentAtPath<RectTransform>("AssetPlaceholder", out assetPlaceholder);
		root.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
	}
}
