using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.admincharacterpurchaseslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI dateTxt;

	public TextMeshProUGUI costTxt;

	public Image honorIcon;

	public Image goldIcon;

	public Image creditsIcon;

	public RectTransform icon;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("DateTxt", out dateTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out costTxt);
		root.GetComponentAtPath<Image>("HonorIcon", out honorIcon);
		root.GetComponentAtPath<Image>("GoldIcon", out goldIcon);
		root.GetComponentAtPath<Image>("CreditsIcon", out creditsIcon);
		root.GetComponentAtPath<RectTransform>("ItemIcon", out icon);
	}
}
