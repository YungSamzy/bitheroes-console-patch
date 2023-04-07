using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.armory.enchant;

public class ArmoryEnchants : Enchants
{
	public const string SLOT_A_NAME = "slota";

	public const string SLOT_B_NAME = "slotb";

	public const string SLOT_C_NAME = "slotc";

	public const string SLOT_D_NAME = "slotd";

	public const string SLOT_E_NAME = "slote";

	public const string SLOT_F_NAME = "slotf";

	public ArmoryEnchants(List<long> slots, List<EnchantData> enchants)
		: base(slots, enchants)
	{
	}

	public override void setEnchantSlots(List<long> slots)
	{
		base.setEnchantSlots(slots);
	}

	public override EnchantData getEnchant(long uid)
	{
		foreach (EnchantData enchant in base.enchants)
		{
			if (enchant != null && enchant.uid == uid)
			{
				return enchant;
			}
		}
		return null;
	}

	public override void clearEnchantSlots()
	{
		for (int i = 0; i < base.slots.Count; i++)
		{
			removeEnchant(i);
			_ = base.slots[i];
			base.slots[i] = 0L;
		}
		base.enchants = new List<EnchantData>();
	}

	public override EnchantData getSlot(int slot)
	{
		if (slot < 0 || slot >= base.slots.Count)
		{
			return null;
		}
		return getArmoryEnchant(base.slots[slot]);
	}

	public virtual EnchantData getArmoryEnchant(long uid)
	{
		if (uid == 0L)
		{
			return null;
		}
		foreach (EnchantData enchant in GameData.instance.PROJECT.character.enchants.enchants)
		{
			if (uid == enchant.uid)
			{
				return enchant;
			}
		}
		return null;
	}

	public new static ArmoryEnchants fromSFSObject(ISFSObject sfsob)
	{
		ISFSObject sFSObject = sfsob.GetSFSObject("ench11");
		List<long> list = Util.arrayToNumberVector(sFSObject.GetLongArray("ench12"));
		List<EnchantData> list2 = EnchantData.listFromSFSObject(sFSObject);
		return new ArmoryEnchants(list, list2);
	}
}
