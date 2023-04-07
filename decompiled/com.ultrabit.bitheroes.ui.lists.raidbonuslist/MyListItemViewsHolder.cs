using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;

namespace com.ultrabit.bitheroes.ui.lists.raidbonuslist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public TextMeshProUGUI txtBonus;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<TextMeshProUGUI>("BonusDescTxt", out txtBonus);
	}
}
