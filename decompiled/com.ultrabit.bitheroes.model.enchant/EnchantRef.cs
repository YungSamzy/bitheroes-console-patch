using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.enchant;

[DebuggerDisplay("{name} (EnchantRef)")]
public class EnchantRef : ItemRef, IEquatable<EnchantRef>, IComparable<EnchantRef>
{
	public new string name { get; private set; }

	public int statsMin { get; private set; }

	public int statsMax { get; private set; }

	public int modsMin { get; private set; }

	public int modsMax { get; private set; }

	public bool allowReroll { get; private set; }

	public List<EnchantModifierRef> modifiers { get; private set; }

	public EnchantRef(int id, EnchantBookData.Enchant enchantData)
		: base(id, 11)
	{
		name = enchantData.name;
		statsMin = enchantData.stats - 1;
		statsMax = enchantData.stats + 1;
		modsMin = enchantData.modsMin;
		modsMax = enchantData.modsMax;
		allowReroll = Util.GetBoolFromStringProperty(enchantData.allowReroll, defaultValue: true);
		LoadDetails(enchantData);
	}

	public bool Equals(EnchantRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EnchantRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
