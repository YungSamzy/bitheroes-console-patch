using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.dailyrewardlist;

public class MyGridItemViewsHolder : CellViewsHolder
{
	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIDay;

	public Image UIBorder;

	public Image UIIconBg;

	public RectTransform UIIcon;

	public override void CollectViews()
	{
		base.CollectViews();
		views.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		views.GetComponentAtPath<TextMeshProUGUI>("DayTxt", out UIDay);
		views.GetComponentAtPath<Image>("Border", out UIBorder);
		views.GetComponentAtPath<Image>("Icon/Views/ItemIconColor", out UIIconBg);
		views.GetComponentAtPath<RectTransform>("Icon", out UIIcon);
	}
}
