using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;

namespace com.ultrabit.bitheroes.ui.lists.chatignoreslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIName;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("NameTxt", out UIName);
	}
}
