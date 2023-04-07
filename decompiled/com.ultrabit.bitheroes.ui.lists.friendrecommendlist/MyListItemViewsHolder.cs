using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using com.ultrabit.bitheroes.ui.language;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.friendrecommendlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIName;

	public Image UIOnlineIcon;

	public Image UIOfflineIcon;

	public TextMeshProUGUI UILogin;

	public TextMeshProUGUI UILevel;

	public TextMeshProUGUI UIPower;

	public TextMeshProUGUI UIStamina;

	public TextMeshProUGUI UIAgility;

	public Button UIOfflineSelect;

	public Button UIOnlineSelect;

	public Button UIFrame;

	public Image UIFrameImage;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	private AsianLanguageFontManager asianLangManager;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<Image>("OnlineIcon", out UIOnlineIcon);
		root.GetComponentAtPath<Image>("OfflineIcon", out UIOfflineIcon);
		root.GetComponentAtPath<TextMeshProUGUI>("LoginTxt", out UILogin);
		root.GetComponentAtPath<TextMeshProUGUI>("LevelTxt", out UILevel);
		root.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out UIPower);
		root.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out UIStamina);
		root.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out UIAgility);
		root.GetComponentAtPath<Button>("OfflineSelectBtn", out UIOfflineSelect);
		root.GetComponentAtPath<Button>("OnlineSelectBtn", out UIOnlineSelect);
		root.GetComponentAtPath<Button>("Frame", out UIFrame);
		root.GetComponentAtPath<Image>("Frame", out UIFrameImage);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
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
