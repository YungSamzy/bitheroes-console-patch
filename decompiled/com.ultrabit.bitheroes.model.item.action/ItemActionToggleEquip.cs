using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.item;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionToggleEquip : ItemActionBase
{
	public ItemActionToggleEquip(BaseModelData itemData, int type, ItemIcon itemIcon)
		: base(itemData, type, itemIcon)
	{
	}

	public override void Execute()
	{
		base.Execute();
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
		switch (type)
		{
		case 1:
			switch (base.itemType)
			{
			case 1:
			{
				EquipmentRef equipmentRef4 = EquipmentBook.Lookup(itemData.itemRef.id);
				GameData.instance.PROJECT.character.equipment.setEquipmentSlot(equipmentRef4, Equipment.getAvailableSlot(equipmentRef4.equipmentType));
				CharacterDALC.instance.doSaveEquipment(GameData.instance.PROJECT.character.equipment);
				GameData.instance.audioManager.PlaySoundLink("equip");
				type = 2;
				break;
			}
			case 16:
			{
				EquipmentRef equipmentRef3 = EquipmentBook.Lookup(itemData.itemRef.id);
				ArmoryRef equipRef = ArmoryRef.EquipmentRefToArmoryRef(equipmentRef3);
				GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.SetArmoryEquipmentSlot(equipRef, Equipment.getAvailableSlot(equipmentRef3.equipmentType));
				CharacterDALC.instance.doSaveArmory(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot);
				GameData.instance.audioManager.PlaySoundLink("equip");
				type = 2;
				break;
			}
			}
			break;
		case 2:
			switch (base.itemType)
			{
			case 1:
			{
				EquipmentRef equipmentRef2 = EquipmentBook.Lookup(itemData.itemRef.id);
				GameData.instance.PROJECT.character.equipment.setEquipmentSlot(null, Equipment.getAvailableSlot(equipmentRef2.equipmentType));
				CharacterDALC.instance.doSaveEquipment(GameData.instance.PROJECT.character.equipment);
				GameData.instance.audioManager.PlaySoundLink("unequip");
				type = 1;
				break;
			}
			case 16:
			{
				EquipmentRef equipmentRef = EquipmentBook.Lookup(itemData.itemRef.id);
				GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.SetArmoryEquipmentSlot(null, Equipment.getAvailableSlot(equipmentRef.equipmentType));
				CharacterDALC.instance.doSaveArmory(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot);
				GameData.instance.audioManager.PlaySoundLink("unequip");
				type = 1;
				break;
			}
			}
			break;
		}
	}
}
