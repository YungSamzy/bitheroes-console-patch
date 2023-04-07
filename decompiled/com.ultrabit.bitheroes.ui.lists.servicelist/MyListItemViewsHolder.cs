using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.utility;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.servicelist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button btn;

	public Image image;

	public TextMeshProUGUI itemName;

	public TextMeshProUGUI description;

	public TextMeshProUGUI cost;

	public TextBrightness itemNameBrightness;

	public TextBrightness descriptionBrightness;

	public TextBrightness costBrightness;

	public Image goldIcon;

	public Image creditsIcon;

	public HoverImages hoverImages;

	public override void CollectViews()
	{
		base.CollectViews();
		btn = root.GetComponent<Button>();
		root.GetComponentAtPath<Image>("ItemImg", out image);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out itemName);
		root.GetComponentAtPath<TextMeshProUGUI>("DescBack/DescTxt", out description);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out cost);
		root.GetComponentAtPath<TextBrightness>("NameTxt", out itemNameBrightness);
		root.GetComponentAtPath<TextBrightness>("DescBack/DescTxt", out descriptionBrightness);
		root.GetComponentAtPath<TextBrightness>("CostTxt", out costBrightness);
		root.GetComponentAtPath<Image>("GoldIcon", out goldIcon);
		root.GetComponentAtPath<Image>("CreditsIcon", out creditsIcon);
		root.TryGetComponent<HoverImages>(out hoverImages);
	}
}
