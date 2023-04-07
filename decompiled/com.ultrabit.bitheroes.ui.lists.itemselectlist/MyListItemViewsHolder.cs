using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.itemselectlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI nameTxt;

	public RectTransform itemIcon;

	public TextMeshProUGUI descTxt;

	public Image disabledImage;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<RectTransform>("ItemIcon", out itemIcon);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out descTxt);
		root.GetComponentAtPath<Image>("DisabledImage", out disabledImage);
	}
}
