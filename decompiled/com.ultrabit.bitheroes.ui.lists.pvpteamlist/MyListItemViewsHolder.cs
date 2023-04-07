using Com.TheFallenGames.OSA.Core;
using com.ultrabit.bitheroes.ui.item;

namespace com.ultrabit.bitheroes.ui.lists.pvpteamlist;

public class MyListItemViewsHolder : BaseItemViewsHolder
{
	public ItemIcon itemIcon;

	public override void CollectViews()
	{
		base.CollectViews();
		if (!root.TryGetComponent<ItemIcon>(out itemIcon))
		{
			itemIcon = root.gameObject.AddComponent<ItemIcon>();
		}
	}
}
