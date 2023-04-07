using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.avatar;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.teamlist;

public class MyViewsHolder : BaseItemViewsHolder
{
	public RectTransform scalableViews;

	public RectTransform teamObject;

	public RectTransform addBtn;

	public TextMeshProUGUI btnTxt;

	public TextMeshProUGUI numberTxt;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI agilityTxt;

	public RectTransform upBtn;

	public RectTransform downBtn;

	public RectTransform removeBtn;

	public RectTransform armoryBtn;

	public TextMeshProUGUI armoryTxt;

	public TextMeshProUGUI removeBtnTxt;

	public Image background;

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
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		scalableViews.GetComponentAtPath<Image>("Background", out background);
		scalableViews.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		scalableViews.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		scalableViews.GetComponentAtPath<RectTransform>("TeamObject", out teamObject);
		scalableViews.GetComponentAtPath<RectTransform>("AddBtn", out addBtn);
		addBtn.GetComponentAtPath<TextMeshProUGUI>("BtnTxt", out btnTxt);
		btnTxt.text = Language.GetString("ui_add");
		scalableViews.GetComponentAtPath<TextMeshProUGUI>("NumberTxt", out numberTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("PowerTxt", out powerTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("StaminaTxt", out staminaTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("AgilityTxt", out agilityTxt);
		teamObject.GetComponentAtPath<RectTransform>("UpBtn", out upBtn);
		teamObject.GetComponentAtPath<RectTransform>("DownBtn", out downBtn);
		teamObject.GetComponentAtPath<RectTransform>("RemoveBtn", out removeBtn);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("RemoveBtn/BtnTxt", out removeBtnTxt);
		teamObject.GetComponentAtPath<RectTransform>("ArmoryBtn", out armoryBtn);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("ArmoryBtn/BtnTxt", out armoryTxt);
		teamObject.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
	}
}
