using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionUse : ItemActionBase
{
	private bool _forceConsume;

	public ItemActionUse(BaseModelData itemData, bool forceConsume)
		: base(itemData, 6)
	{
		_forceConsume = forceConsume;
	}

	public override void Execute()
	{
		base.Execute();
		if (base.itemType == 4)
		{
			ConsumableManager.instance.SetupConsumable(itemData.itemRef as ConsumableRef, itemData.qty, _forceConsume);
			ConsumableManager.instance.ProcessUse();
		}
	}
}
