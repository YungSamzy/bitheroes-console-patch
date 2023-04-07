using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.fishingshoplist;

public class MyGridItemViewsHolder : CellViewsHolder
{
	public TextMeshProUGUI UIName;

	public Image UIBg;

	public Image UIItemIconColor;

	public Image UIBlueButtonBack;

	public TextMeshProUGUI UICost;

	public override void CollectViews()
	{
		base.CollectViews();
		views.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		views.GetComponentAtPath<Image>("BackgroundImg", out UIBg);
		views.GetComponentAtPath<Image>("ItemIcon/Views/ItemIconColor", out UIItemIconColor);
		views.GetComponentAtPath<Image>("BlueButtonImg", out UIBlueButtonBack);
		views.GetComponentAtPath<TextMeshProUGUI>("CostTxt", out UICost);
	}
}
