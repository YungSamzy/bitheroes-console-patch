using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionExchange : ItemActionBase
{
	public ItemActionExchange(BaseModelData itemData)
		: base(itemData, 8)
	{
	}

	public override void Execute()
	{
		base.Execute();
		GameData.instance.windowGenerator.NewItemExchangeWindow(itemData as ItemData);
	}
}
