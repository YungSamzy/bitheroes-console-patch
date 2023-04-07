using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.avatar;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.brawlteamlist;

public class MyViewsHolder : BaseItemViewsHolder
{
	public RectTransform scalableViews;

	public RectTransform teamObject;

	public Canvas statsObject;

	public RectTransform addBtn;

	public TextMeshProUGUI btnTxt;

	public TextMeshProUGUI numberTxt;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI statsTxt;

	public RectTransform upBtn;

	public RectTransform downBtn;

	public RectTransform removeBtn;

	public TextMeshProUGUI removeBtnTxt;

	public Image frame;

	public Image frameGreen;

	public Image avatarFrameGreen;

	public AvatarBackground avatarBackground;

	public AvatarGenerationBanner avatarGenerationBanner;

	public Image readyIcon;

	public Image unreadyIcon;

	public Image leaderIcon;

	public Image agilityIcon;

	public Image staminaIcon;

	public Image powerIcon;

	public RectTransform placeholderAsset;

	public SpriteMask assetMask0;

	public SpriteMask assetMask1;

	public SpriteMask assetMask2;

	public SpriteMask assetMask3;

	public SpriteMask assetMask4;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<RectTransform>("ScalableViews", out scalableViews);
		scalableViews.GetComponentAtPath<Image>("Frame", out frame);
		scalableViews.GetComponentAtPath<Image>("FrameGreen", out frameGreen);
		scalableViews.GetComponentAtPath<Image>("AvatarFrameGreen", out avatarFrameGreen);
		scalableViews.GetComponentAtPath<AvatarBackground>("AvatarBackground", out avatarBackground);
		scalableViews.GetComponentAtPath<AvatarGenerationBanner>("AvatarGenerationBanner", out avatarGenerationBanner);
		scalableViews.GetComponentAtPath<TextMeshProUGUI>("NumberTxt", out numberTxt);
		scalableViews.GetComponentAtPath<RectTransform>("InviteBtn", out addBtn);
		addBtn.GetComponentAtPath<TextMeshProUGUI>("BtnTxt", out btnTxt);
		btnTxt.text = Language.GetString("ui_invite");
		scalableViews.GetComponentAtPath<RectTransform>("UpBtn", out upBtn);
		scalableViews.GetComponentAtPath<RectTransform>("DownBtn", out downBtn);
		scalableViews.GetComponentAtPath<RectTransform>("KickBtn", out removeBtn);
		scalableViews.GetComponentAtPath<TextMeshProUGUI>("KickBtn/BtnTxt", out removeBtnTxt);
		scalableViews.GetComponentAtPath<RectTransform>("BrawlTeamObject", out teamObject);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		teamObject.GetComponentAtPath<TextMeshProUGUI>("StatsTxt", out statsTxt);
		teamObject.GetComponentAtPath<Image>("ReadyIcon", out readyIcon);
		teamObject.GetComponentAtPath<Image>("UnreadyIcon", out unreadyIcon);
		teamObject.GetComponentAtPath<Image>("LeaderIcon", out leaderIcon);
		teamObject.GetComponentAtPath<Canvas>("StatsBg", out statsObject);
		statsObject.transform.GetComponentAtPath<Image>("AgilityIcon", out agilityIcon);
		statsObject.transform.GetComponentAtPath<Image>("StaminaIcon", out staminaIcon);
		statsObject.transform.GetComponentAtPath<Image>("PowerIcon", out powerIcon);
		teamObject.GetComponentAtPath<RectTransform>("PlaceholderAsset", out placeholderAsset);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask0", out assetMask0);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask1", out assetMask1);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask2", out assetMask2);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask3", out assetMask3);
		teamObject.GetComponentAtPath<SpriteMask>("AssetMask4", out assetMask4);
	}
}
