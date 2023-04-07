using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.boober;

[DebuggerDisplay("{name} (BobberRef)")]
public class BobberRef : ItemRef, IEquatable<BobberRef>, IComparable<BobberRef>
{
	public string asset { get; private set; }

	public List<GameModifier> modifiers { get; private set; }

	public BobberRef(int id, BobberBookData.Bobber bobberData)
		: base(id, 14)
	{
		asset = bobberData.asset;
		modifiers = GameModifier.GetGameModifierFromData(bobberData.modifiers, bobberData.lstModifier);
	}

	public bool Equals(BobberRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(BobberRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
