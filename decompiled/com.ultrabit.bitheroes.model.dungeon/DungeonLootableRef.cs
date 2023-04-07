using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonLootableRef)")]
public class DungeonLootableRef : DungeonObjectRef, IEquatable<DungeonLootableRef>, IComparable<DungeonLootableRef>
{
	public DungeonLootableRef(int id, DungeonBookData.Lootable lootableData)
		: base(id, lootableData)
	{
	}

	public bool Equals(DungeonLootableRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((DungeonObjectRef)other);
	}

	public int CompareTo(DungeonLootableRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((DungeonObjectRef)other);
	}
}
