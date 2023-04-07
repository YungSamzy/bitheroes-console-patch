using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonAdRef)")]
public class DungeonAdRef : DungeonObjectRef, IEquatable<DungeonAdRef>, IComparable<DungeonAdRef>
{
	public DungeonAdRef(int id, DungeonBookData.Ad adData)
		: base(id, adData)
	{
	}

	public bool Equals(DungeonAdRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((DungeonObjectRef)other);
	}

	public int CompareTo(DungeonAdRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((DungeonObjectRef)other);
	}
}
