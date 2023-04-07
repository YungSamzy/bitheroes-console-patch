using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.nodesinglemodifierlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public RectTransform rectTransform;

	public TextMeshProUGUI bonusDesc;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<RectTransform>("", out rectTransform);
		root.GetComponentAtPath<TextMeshProUGUI>("BonusDescTxt", out bonusDesc);
	}
}
