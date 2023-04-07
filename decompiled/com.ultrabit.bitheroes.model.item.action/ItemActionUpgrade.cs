using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.craft;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionUpgrade : ItemActionBase
{
	public ItemActionUpgrade(BaseModelData itemData)
		: base(itemData, 9)
	{
	}

	public override void Execute()
	{
		base.Execute();
		GameData.instance.windowGenerator.NewEquipmentUpgradeWindow(itemData.itemRef as EquipmentRef, GameData.instance.windowGenerator.GetDialogByClass(typeof(CraftWindow)) as GameObject, null);
	}
}
