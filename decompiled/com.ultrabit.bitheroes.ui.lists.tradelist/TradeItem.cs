using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.ui.lists.tradelist;

public class TradeItem
{
	public ItemTradeRef tradeRef;

	public BaseRef sourceRef;

	public object parentWindow;

	public bool seen;

	public bool notifyShown;

	public bool unlocks;

	public int lockedRecipes;

	public List<ItemRef> unlocksList = new List<ItemRef>();

	public string[] unlocksText;
}
