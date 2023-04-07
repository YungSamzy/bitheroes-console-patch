using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.character.armory;
using com.ultrabit.bitheroes.ui.victory;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionChange : ItemActionBase
{
	public ItemActionChange(BaseModelData itemData, string altName = null)
		: base(itemData, 3, null, altName)
	{
	}

	public override void Execute()
	{
		base.Execute();
		switch (base.itemType)
		{
		case 1:
		case 16:
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(base.itemRef.id);
			if (GameData.instance.windowGenerator.GetLastDialog() is CharacterWindow || GameData.instance.windowGenerator.GetLastDialog() is VictoryWindow)
			{
				GameData.instance.windowGenerator.NewEquipmentWindow(Equipment.getAvailableSlot(equipmentRef.equipmentType), null);
			}
			else if (GameData.instance.windowGenerator.GetLastDialog() is CharacterArmoryWindow)
			{
				GameData.instance.windowGenerator.NewArmoryEquipmentWindow(Equipment.getAvailableSlot(equipmentRef.equipmentType), null);
			}
			break;
		}
		case 8:
			GameData.instance.windowGenerator.NewMountSelectWindow(GameData.instance.PROJECT.character.mounts, changeable: true, equippable: true);
			break;
		case 17:
			GameData.instance.windowGenerator.NewArmoryMountSelectWindow(GameData.instance.PROJECT.character.mounts, changeable: true, equippable: true);
			break;
		case 11:
		{
			int enchantSlot = GameData.instance.PROJECT.character.enchants.getEnchantSlot(itemData as EnchantData);
			GameData.instance.windowGenerator.NewEnchantOptionsWindow(enchantSlot);
			break;
		}
		case 15:
		{
			AugmentData augmentData = itemData as AugmentData;
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (GameData.instance.PROJECT.character.augments.getFamiliarAugmentSlot(augmentData.familiarRef, augmentData.slot) != null)
			{
				GameData.instance.windowGenerator.NewAugmentOptionsWindow(augmentData.familiarRef, augmentData.slot, augmentData);
			}
			break;
		}
		}
	}
}
