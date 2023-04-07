using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.admincharacterplatformslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI userIDTxt;

	public TextMeshProUGUI nameTxt;

	public Image bg;

	public Button disableBtn;

	public Button enableBtn;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("UserIDTxt", out userIDTxt);
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<Image>("Bg", out bg);
		root.GetComponentAtPath<Button>("DisableBtn", out disableBtn);
		root.GetComponentAtPath<Button>("EnableBtn", out enableBtn);
	}
}
