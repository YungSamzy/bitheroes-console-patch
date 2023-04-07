using Com.TheFallenGames.OSA.Core;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.news;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public RectTransform placeholderPromo;

	public override void CollectViews()
	{
		base.CollectViews();
		root.GetComponentAtPath<RectTransform>("PlaceholderPromo", out placeholderPromo);
	}
}
