using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.familiarlist;

public class MyGridItemViewsHolder : CellViewsHolder
{
	public Image asset;

	public override void CollectViews()
	{
		base.CollectViews();
		views.GetComponentAtPath<Image>("Asset", out asset);
	}
}
