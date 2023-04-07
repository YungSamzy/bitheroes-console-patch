using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.paymentlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button btn;

	public TextMeshProUGUI nameTxt;

	public Image logoAsset;

	public TextMeshProUGUI descTxt;

	public TextMeshProUGUI costTxt;

	public Image asset;

	public override void CollectViews()
	{
		base.CollectViews();
		btn = root.GetComponent<Button>();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<Image>("LogoAsset", out logoAsset);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out descTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out costTxt);
		root.GetComponentAtPath<Image>("Asset", out asset);
	}
}
