using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.historylist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UITime;

	public TextMeshProUGUI UIName;

	public TextMeshProUGUI UIPoint;

	public Image image;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("TimeBackTxt/TimeTxt", out UITime);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<TextMeshProUGUI>("PointsTxtBack/PointsTxt", out UIPoint);
		image = root.GetComponent<Image>();
	}
}
