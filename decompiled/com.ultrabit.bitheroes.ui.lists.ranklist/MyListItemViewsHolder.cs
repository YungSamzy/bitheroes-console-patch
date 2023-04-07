using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.ranklist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIPosition;

	public TextMeshProUGUI UIPoints;

	public TextMeshProUGUI UIName;

	public Image UIHighlight;

	public Image UItrohpy;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("RankBack/RankNumTxt", out UIPosition);
		root.GetComponentAtPath<TextMeshProUGUI>("PointsBack/PointsNumTxt", out UIPoints);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
		root.GetComponentAtPath<Image>("UserTrophyImg", out UItrohpy);
		UIHighlight = root.GetComponent<Image>();
	}
}
