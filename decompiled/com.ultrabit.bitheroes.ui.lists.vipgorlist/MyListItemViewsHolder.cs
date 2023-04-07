using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.vipgorlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image ribbon;

	public TextMeshProUGUI ribbonTxt;

	public RectTransform itemIcon;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI costTxt;

	public Image creditsIcon;

	public Image goldIcon;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("Ribbon", out ribbon);
		root.GetComponentAtPath<TextMeshProUGUI>("RibbonTxt", out ribbonTxt);
		root.GetComponentAtPath<RectTransform>("ItemIcon", out itemIcon);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out costTxt);
		root.GetComponentAtPath<Image>("CreditsIcon", out creditsIcon);
		root.GetComponentAtPath<Image>("GoldIcon", out goldIcon);
	}
}
