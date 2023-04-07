using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.dropdownlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public Button btn;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descText;

	public Button btnHelp;

	public override void CollectViews()
	{
		base.CollectViews();
		btn = root.GetComponent<Button>();
		root.GetComponentAtPath<TextMeshProUGUI>("LabelTxt", out titleText);
		root.GetComponentAtPath<TextMeshProUGUI>("DescTxt", out descText);
		root.GetComponentAtPath<Button>("HelpBtn", out btnHelp);
	}
}
