using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.herolist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button btn;

	public TextMeshProUGUI itemName;

	public Image image;

	public TextMeshProUGUI description;

	public TextMeshProUGUI cost;

	public Image goldIcon;

	public Image creditsIcon;

	public override void CollectViews()
	{
		base.CollectViews();
		btn = root.GetComponent<Button>();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out itemName);
		root.GetComponentAtPath<Image>("ItemImg", out image);
		root.GetComponentAtPath<TextMeshProUGUI>("DescBack/DescTxt", out description);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out cost);
		root.GetComponentAtPath<Image>("GoldIcon", out goldIcon);
		root.GetComponentAtPath<Image>("CreditsIcon", out creditsIcon);
	}
}
