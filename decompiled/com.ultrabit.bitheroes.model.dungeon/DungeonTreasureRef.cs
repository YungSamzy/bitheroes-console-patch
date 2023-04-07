using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonTreasureRef)")]
public class DungeonTreasureRef : DungeonObjectRef, IEquatable<DungeonTreasureRef>, IComparable<DungeonTreasureRef>
{
	private bool _locked;

	public bool locked => _locked;

	public DungeonTreasureRef(int id, DungeonBookData.Treasure treasureData)
		: base(id, treasureData)
	{
		_locked = treasureData.locked != null && treasureData.locked.ToLower().Trim().Equals("true");
	}

	public bool Equals(DungeonTreasureRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(DungeonTreasureRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
