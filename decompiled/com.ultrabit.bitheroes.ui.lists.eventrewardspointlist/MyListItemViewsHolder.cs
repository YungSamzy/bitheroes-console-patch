using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.eventrewardspointlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIRank;

	public Transform itemContainer;

	public Image UIBorder;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("RequiredBack/MinMaxTxt", out UIRank);
		root.GetComponentAtPath<Transform>("ItemsContainer", out itemContainer);
		UIBorder = root.GetComponent<Image>();
	}
}
