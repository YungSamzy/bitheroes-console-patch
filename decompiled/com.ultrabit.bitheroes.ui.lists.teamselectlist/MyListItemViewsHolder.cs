using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.avatar;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamselectlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI txtName;

	public TextMeshProUGUI txtPower;

	public TextMeshProUGUI txtStamina;

	public TextMeshProUGUI txtAgility;

	public Button btnSelect;

	public TextMeshProUGUI btnSelectText;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	public Button background;

	public Image backgroundImage;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out txtName);
		root.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out txtPower);
		root.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out txtStamina);
		root.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out txtAgility);
		root.GetComponentAtPath<Button>("SelectBtn", out btnSelect);
		root.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		root.GetComponentAtPath<TextMeshProUGUI>("SelectBtn/BtnTxt", out btnSelectText);
		root.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		root.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		root.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		root.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		root.GetComponentAtPath<Button>("Background", out background);
		root.GetComponentAtPath<Image>("Background", out backgroundImage);
		root.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		root.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
	}
}
