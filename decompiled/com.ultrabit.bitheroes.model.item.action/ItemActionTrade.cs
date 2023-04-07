using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionTrade : ItemActionBase
{
	public ItemActionTrade(BaseModelData itemData)
		: base(itemData, 15)
	{
	}

	public override void Execute()
	{
		base.Execute();
		GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetItemTradeRefs(itemData.itemRef));
	}
}
