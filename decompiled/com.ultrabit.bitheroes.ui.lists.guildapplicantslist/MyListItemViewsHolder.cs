using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildapplicantslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UILogin;

	public TextMeshProUGUI UILevel;

	public Image UIOnline;

	public Image UIOffline;

	public Button UIAccept;

	public Button UIDecline;

	public Button UIFrame;

	public HoverImage UIFrameHover;

	public Image UIFrameImage;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("LoginTxt", out UILogin);
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out UILevel);
		root.GetComponentAtPath<Image>("OnlineIcon", out UIOnline);
		root.GetComponentAtPath<Image>("OfflineIcon", out UIOffline);
		root.GetComponentAtPath<Button>("AcceptBtn", out UIAccept);
		root.GetComponentAtPath<Button>("DeclineBtn", out UIDecline);
		root.GetComponentAtPath<Button>("Frame", out UIFrame);
		root.GetComponentAtPath<HoverImage>("Frame", out UIFrameHover);
		root.GetComponentAtPath<Image>("Frame", out UIFrameImage);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		root.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		root.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
	}
}
