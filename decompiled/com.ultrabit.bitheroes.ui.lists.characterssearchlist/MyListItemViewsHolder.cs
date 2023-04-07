using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.characterssearchlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI nameTxt;

	public Button borderPlain;

	public Image borderHighlight;

	public Button selectBtn;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out nameTxt);
		root.GetComponentAtPath<Button>("BorderPlain", out borderPlain);
		root.GetComponentAtPath<Image>("BorderHighlight", out borderHighlight);
		root.GetComponentAtPath<Button>("SelectBtn", out selectBtn);
	}
}
