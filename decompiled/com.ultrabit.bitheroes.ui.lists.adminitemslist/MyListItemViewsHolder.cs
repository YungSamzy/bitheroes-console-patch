using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.adminitemslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public RectTransform itemIcon;

	public TMP_InputField qtyTxt;

	public Button removeBtn;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<RectTransform>("ItemIcon", out itemIcon);
		root.GetComponentAtPath<TMP_InputField>("QtyTxt", out qtyTxt);
		root.GetComponentAtPath<Button>("RemoveBtn", out removeBtn);
	}
}
