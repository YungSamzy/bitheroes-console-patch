using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.craft;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionReforge : ItemActionBase
{
	public ItemActionReforge(BaseModelData itemData)
		: base(itemData, 12)
	{
	}

	public override void Execute()
	{
		base.Execute();
		GameData.instance.windowGenerator.NewItemReforgeWindow(itemData.itemRef as EquipmentRef, (GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) as CraftWindow).gameObject);
	}
}
