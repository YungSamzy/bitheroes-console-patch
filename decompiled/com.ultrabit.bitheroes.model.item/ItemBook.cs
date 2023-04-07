using System.Collections.Generic;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.fish;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.material;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.model.item;

public class ItemBook
{
	public static ItemRef Lookup(int id, string type)
	{
		return Lookup(id, ItemRef.getItemType(type));
	}

	public static ItemRef Lookup(int id, int type)
	{
		switch (type)
		{
		case 2:
			return MaterialBook.Lookup(id);
		case 1:
		case 16:
			return EquipmentBook.Lookup(id);
		case 3:
			return CurrencyBook.Lookup(id);
		case 4:
			return ConsumableBook.Lookup(id);
		case 5:
			return ServiceBook.Lookup(id);
		case 9:
		case 19:
			return RuneBook.Lookup(id);
		case 15:
			return AugmentBook.Lookup(id);
		case 13:
			return BaitBook.Lookup(id);
		case 14:
			return BobberBook.Lookup(id);
		case 8:
		case 17:
			return MountBook.Lookup(id);
		case 7:
			return FusionBook.Lookup(id);
		case 6:
			return FamiliarBook.Lookup(id);
		case 11:
		case 18:
			return EnchantBook.Lookup(id);
		case 10:
			return GuildBook.LookupItem(id);
		case 12:
			return FishBook.Lookup(id);
		default:
			D.LogWarning("ItemBook Lookup, Failed to find element for id: " + id + " with type: " + type);
			return null;
		}
	}

	public static List<ItemRef> GetAllItems()
	{
		List<ItemRef> list = new List<ItemRef>();
		for (int i = 1; i < 22; i++)
		{
			List<ItemRef> itemsByType = GetItemsByType(i, int.MinValue, int.MaxValue, int.MaxValue);
			if (itemsByType == null || itemsByType.Count <= 0)
			{
				continue;
			}
			foreach (ItemRef item in itemsByType)
			{
				if (!(item == null))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static List<ItemRef> GetItemsByType(int type, int minID = int.MinValue, int maxID = int.MaxValue, int minRank = 0, int minTier = 0, bool getUpgrades = true)
	{
		List<ItemRef> list = new List<ItemRef>();
		int num = 0;
		switch (type)
		{
		case 1:
			list.AddRange(EquipmentBook.GetFullEquipmentList());
			break;
		case 2:
			list.AddRange(MaterialBook.GetAllPossibleMaterials());
			break;
		case 3:
			list.AddRange(CurrencyBook.GetAllPossibleCurrencies());
			break;
		case 4:
			list.AddRange(ConsumableBook.GetAllPossiblesConsumables());
			break;
		case 5:
			list.AddRange(ServiceBook.GetAllPossibleServices());
			break;
		case 6:
			list.AddRange(FamiliarBook.GetCompleteFamiliarList());
			break;
		case 7:
			list.AddRange(FusionBook.GetAllFusionsRef());
			break;
		case 8:
			list.AddRange(MountBook.GetFullMountList());
			break;
		case 9:
			for (num = 0; num <= RuneBook.size; num++)
			{
				RuneRef runeRef = RuneBook.Lookup(num);
				if (runeRef != null && runeRef.id >= minID && runeRef.id <= maxID)
				{
					list.Add(runeRef);
				}
			}
			break;
		case 10:
			for (num = 0; num <= GuildBook.sizeItems; num++)
			{
				GuildItemRef guildItemRef = GuildBook.LookupItem(num);
				if (guildItemRef != null && guildItemRef.id >= minID && guildItemRef.id <= maxID)
				{
					list.Add(guildItemRef);
				}
			}
			break;
		case 11:
			for (num = 0; num <= EnchantBook.size; num++)
			{
				EnchantRef enchantRef = EnchantBook.Lookup(num);
				if (enchantRef != null && enchantRef.id >= minID && enchantRef.id <= maxID)
				{
					list.Add(enchantRef);
				}
			}
			break;
		case 12:
			for (num = 0; num <= FishBook.size; num++)
			{
				FishRef fishRef = FishBook.Lookup(num);
				if (fishRef != null && fishRef.id >= minID && fishRef.id <= maxID)
				{
					list.Add(fishRef);
				}
			}
			break;
		case 13:
			for (num = 0; num <= BaitBook.size; num++)
			{
				BaitRef baitRef = BaitBook.Lookup(num);
				if (baitRef != null && baitRef.id >= minID && baitRef.id <= maxID)
				{
					list.Add(baitRef);
				}
			}
			break;
		case 14:
			for (num = 0; num <= BobberBook.size; num++)
			{
				BobberRef bobberRef = BobberBook.Lookup(num);
				if (bobberRef != null && bobberRef.id >= minID && bobberRef.id <= maxID)
				{
					list.Add(bobberRef);
				}
			}
			break;
		case 15:
			for (num = 0; num <= AugmentBook.size; num++)
			{
				AugmentRef augmentRef = AugmentBook.Lookup(num);
				if (augmentRef != null && augmentRef.id >= minID && augmentRef.id <= maxID)
				{
					list.Add(augmentRef);
				}
			}
			break;
		}
		return list;
	}
}
