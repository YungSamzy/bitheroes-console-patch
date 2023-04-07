using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonShrineRef)")]
public class DungeonShrineRef : DungeonObjectRef, IEquatable<DungeonShrineRef>, IComparable<DungeonShrineRef>
{
	public const int TYPE_HEALTH = 0;

	public const int TYPE_SP = 1;

	private int _shrineType;

	public int shrineType => _shrineType;

	public DungeonShrineRef(int id, DungeonBookData.Shrine shrineData)
		: base(id, shrineData)
	{
		_shrineType = shrineData.shrineType;
	}

	public bool Equals(DungeonShrineRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(DungeonShrineRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
