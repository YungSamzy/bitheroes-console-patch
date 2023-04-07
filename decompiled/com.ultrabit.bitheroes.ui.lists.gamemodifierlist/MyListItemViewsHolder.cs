using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;

namespace com.ultrabit.bitheroes.ui.lists.gamemodifierlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI bonus;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("BonusDescTxt", out bonus);
	}
}
