using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.augment;
using com.ultrabit.bitheroes.ui.familiar;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionView : ItemActionBase
{
	private TransactionManager transactionManager;

	public ItemActionView(BaseModelData itemData)
		: base(itemData, 7)
	{
	}

	public override void Execute()
	{
		base.Execute();
		switch (itemData.itemRef.itemType)
		{
		case 6:
		{
			FamiliarsWindow familiarsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(FamiliarsWindow)) as FamiliarsWindow;
			FamiliarRef familiarRef = itemData.itemRef as FamiliarRef;
			bool mine = false;
			if (GameData.instance.PROJECT.character.inventory.getItem(familiarRef.id, 6) != null)
			{
				mine = true;
			}
			GameData.instance.windowGenerator.NewFamiliarWindow(itemData.itemRef as FamiliarRef, (familiarsWindow != null) ? familiarsWindow.gameObject : null, mine);
			break;
		}
		case 8:
			GameData.instance.windowGenerator.NewMountWindow(itemData as MountData, GameData.instance.PROJECT.character.tier);
			break;
		case 17:
			GameData.instance.windowGenerator.NewArmoryMountWindow(itemData as MountData, GameData.instance.PROJECT.character.tier);
			break;
		case 11:
		case 18:
			GameData.instance.windowGenerator.NewEnchantWindow(itemData as EnchantData);
			break;
		case 15:
		{
			AugmentSelectWindow augmentSelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentSelectWindow)) as AugmentSelectWindow;
			GameData.instance.windowGenerator.NewAugmentWindow(itemData as AugmentData, (augmentSelectWindow != null) ? augmentSelectWindow.gameObject : null);
			break;
		}
		default:
			GameData.instance.windowGenerator.NewItemContentsWindow(itemData.itemRef, itemData.qty, (itemData is EventSalesShopItemRefModelData) ? (itemData as EventSalesShopItemRefModelData) : null);
			break;
		}
	}
}
