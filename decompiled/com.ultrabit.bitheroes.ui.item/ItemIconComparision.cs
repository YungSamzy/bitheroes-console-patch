using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.mount;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemIconComparision : MonoBehaviour
{
	private ItemCompareWidget itemCompareWidget;

	private BaseModelData data;

	public string currentLabel => itemCompareWidget.currentLabel;

	public void Setup(BaseModelData itemData, bool showCosmetic, bool showComparision)
	{
		data = itemData;
		HideComparison();
		if (showCosmetic)
		{
			PlayComparison("cosmetic");
		}
		else if (showComparision)
		{
			ShowComparison();
		}
	}

	private ItemCompareWidget GetItemCompareWidget()
	{
		if (itemCompareWidget == null)
		{
			itemCompareWidget = GetComponentInChildren<ItemCompareWidget>();
		}
		return itemCompareWidget;
	}

	public void HideComparison()
	{
		PlayComparison("=");
	}

	public void ShowComparison(int index = -1)
	{
		HideComparison();
		if (!(data.itemRef == null))
		{
			int num = data.itemRef.itemType;
			if (data.type == 16)
			{
				num = 16;
			}
			switch (num)
			{
			case 1:
				ShowEquipmentComparison();
				break;
			case 16:
				ShowArmoryComparison();
				break;
			case 11:
				ShowEnchantComparison();
				break;
			case 18:
				ShowArmoryEnchantComparison();
				break;
			case 8:
				ShowMountComparison();
				break;
			case 17:
				ShowArmoryMountComparison();
				break;
			}
		}
	}

	private void ShowArmoryComparison()
	{
		EquipmentRef equipmentRef = data.itemRef as EquipmentRef;
		int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
		EquipmentRef armoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(availableSlot);
		int num = ((armoryEquipmentSlot != null) ? armoryEquipmentSlot.id : (-1));
		int currentStats = ((armoryEquipmentSlot != null) ? armoryEquipmentSlot.total : 0);
		ShowStatComparison(currentStats, equipmentRef.total, num == equipmentRef.id);
	}

	private void ShowEquipmentComparison()
	{
		EquipmentRef equipmentRef = data.itemRef as EquipmentRef;
		int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
		EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(availableSlot);
		int num = ((equipmentSlot != null) ? equipmentSlot.id : (-1));
		int currentStats = ((equipmentSlot != null) ? equipmentSlot.total : 0);
		ShowStatComparison(currentStats, equipmentRef.total, num == equipmentRef.id);
	}

	private void ShowArmoryEnchantComparison()
	{
		if (data != null)
		{
			EnchantData enchantData = data as EnchantData;
			EnchantData enchantData2 = ((enchantData.slot != -1) ? GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.getSlot(enchantData.slot) : null);
			bool enchantEquipped = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants.getEnchantEquipped(data as EnchantData);
			if (enchantEquipped)
			{
				PlayComparison("E");
			}
			else if (enchantData2 != null)
			{
				ShowStatComparison(enchantData2.total, enchantData.total, enchantEquipped);
			}
		}
	}

	private void ShowEnchantComparison()
	{
		if (data != null && data is EnchantData enchantData)
		{
			EnchantData enchantData2 = ((enchantData.slot != -1) ? GameData.instance.PROJECT.character.enchants.getSlot(enchantData.slot) : null);
			bool enchantEquipped = GameData.instance.PROJECT.character.enchants.getEnchantEquipped(data as EnchantData);
			if (enchantEquipped)
			{
				PlayComparison("E");
			}
			else if (enchantData2 != null)
			{
				ShowStatComparison(enchantData2.total, enchantData.total, enchantEquipped);
			}
		}
	}

	private void ShowArmoryMountComparison()
	{
		if (data != null && data is MountData)
		{
			MountData mountData = data as MountData;
			MountData mountDataEquipped = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetMountDataEquipped();
			bool equipped = mountData.Equals(mountDataEquipped);
			int currentStats = mountDataEquipped?.getTotal(GameData.instance.PROJECT.character.tier) ?? 0;
			int newStats = mountData?.getTotal(GameData.instance.PROJECT.character.tier) ?? 0;
			ShowStatComparison(currentStats, newStats, equipped);
		}
	}

	private void ShowMountComparison()
	{
		if (data != null && data is MountData)
		{
			MountData mountData = data as MountData;
			MountData mountEquipped = GameData.instance.PROJECT.character.mounts.getMountEquipped();
			bool equipped = mountData.Equals(mountEquipped);
			int currentStats = mountEquipped?.getTotal(GameData.instance.PROJECT.character.tier) ?? 0;
			int newStats = mountData?.getTotal(GameData.instance.PROJECT.character.tier) ?? 0;
			ShowStatComparison(currentStats, newStats, equipped);
		}
	}

	private void ShowStatComparison(int currentStats, int newStats, bool equipped)
	{
		if (equipped)
		{
			PlayComparison("E");
		}
		else if (newStats > currentStats)
		{
			PlayComparison("+");
		}
		else if (newStats < currentStats)
		{
			PlayComparison("-");
		}
		else
		{
			PlayComparison("=");
		}
	}

	public void PlayComparison(string label)
	{
		if (GetItemCompareWidget() != null)
		{
			GetItemCompareWidget().SetCompare(label);
		}
	}
}
