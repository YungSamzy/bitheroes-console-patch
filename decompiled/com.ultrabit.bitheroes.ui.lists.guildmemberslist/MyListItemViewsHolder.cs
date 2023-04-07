using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildmemberslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UILevel;

	public TextMeshProUGUI UIRank;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UILogin;

	public Image UIOnline;

	public Image UIOffline;

	public Button UIMutiny;

	public TextMeshProUGUI UIMutinyTxt;

	public HoverImages hoverImages;

	public Button UIFrame;

	public Image UIFrameImage;

	public AvatarGenerationBanner avatarGenerationBanner;

	public AvatarBackground avatarBackground;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	private AsianLanguageFontManager asianLangManager;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out UILevel);
		root.GetComponentAtPath<TextMeshProUGUI>("RankTxt", out UIRank);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("LoginTxt", out UILogin);
		root.GetComponentAtPath<Image>("OnlineIcon", out UIOnline);
		root.GetComponentAtPath<Image>("OfflineIcon", out UIOffline);
		root.GetComponentAtPath<Button>("MutinyBtn", out UIMutiny);
		root.GetComponentAtPath<TextMeshProUGUI>("MutinyBtn/BtnTxt", out UIMutinyTxt);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<Button>("TileBackground", out UIFrame);
		root.GetComponentAtPath<Image>("TileBackground", out UIFrameImage);
		root.GetComponentAtPath<HoverImages>("TileBackground", out hoverImages);
		root.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		root.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		asianLangManager = root.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = root.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}
}
