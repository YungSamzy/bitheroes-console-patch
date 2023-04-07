using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.eventtargetlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI NameTxt;

	public TextMeshProUGUI StatsTxt;

	public TextMeshProUGUI WinTxt;

	public TextMeshProUGUI LoseTxt;

	public Button FightBtn;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	public Button UIFrame;

	public HoverImage UIFrameHover;

	public Image UIFrameImage;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out NameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("StatsTxt", out StatsTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("WinTxt", out WinTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("LoseTxt", out LoseTxt);
		root.GetComponentAtPath<Button>("FightBtn", out FightBtn);
		root.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		root.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		root.GetComponentAtPath<Button>("TileBackground", out UIFrame);
		root.GetComponentAtPath<HoverImage>("TileBackground", out UIFrameHover);
		root.GetComponentAtPath<Image>("TileBackground", out UIFrameImage);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
	}
}
