using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.guildperkslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Image UIItemIcon;

	public TextMeshProUGUI UIRank;

	public TextMeshProUGUI UICurrentName;

	public TextMeshProUGUI UINextName;

	public TextMeshProUGUI UICost;

	public Button UICurrentModifier;

	public Button UINextModifier;

	public Button UIUpgrade;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<Image>("ItemIcon", out UIItemIcon);
		root.GetComponentAtPath<TextMeshProUGUI>("RankTxt", out UIRank);
		root.GetComponentAtPath<TextMeshProUGUI>("CurrentNameTxt", out UICurrentName);
		root.GetComponentAtPath<TextMeshProUGUI>("NextNameTxt", out UINextName);
		root.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out UICost);
		root.GetComponentAtPath<Button>("CurrentGameModifierBtn", out UICurrentModifier);
		root.GetComponentAtPath<Button>("NextGameModifierBtn", out UINextModifier);
		root.GetComponentAtPath<Button>("UpgradeBtn", out UIUpgrade);
	}
}
