using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;

namespace com.ultrabit.bitheroes.ui.lists.gamemodifiertimelist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI UIDesc;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("BonusDescTxt", out UIDesc);
	}
}
