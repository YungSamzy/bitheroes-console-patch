using com.ultrabit.bitheroes.model.data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionNone : ItemActionBase
{
	public ItemActionNone(BaseModelData itemData)
		: base(itemData, 0)
	{
	}

	public override void Execute()
	{
		base.Execute();
	}
}
