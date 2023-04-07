using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.bait;

[DebuggerDisplay("{name} (BaitRef)")]
public class BaitRef : ItemRef, IEquatable<BaitRef>, IComparable<BaitRef>
{
	public List<GameModifier> modifiers { get; private set; }

	public BaitRef(int id, BaitBookData.Bait baitData)
		: base(id, 13)
	{
		modifiers = GameModifier.GetGameModifierFromData(baitData.modifiers, baitData.lstModifiers);
	}

	public bool Equals(BaitRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(BaitRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
