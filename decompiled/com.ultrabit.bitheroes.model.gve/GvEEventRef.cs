using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.gve;

[DebuggerDisplay("{name} (GvEEventRef)")]
public class GvEEventRef : EventRef, IEquatable<GvEEventRef>, IComparable<GvEEventRef>
{
	private int _badges;

	private GvEZoneRef _zoneRef;

	private EventRewards _guildRankRewards;

	private EventRewards _guildPointRewards;

	public int badges => _badges;

	public GvEEventRef(BaseEventBookData.Event eventData)
		: base(7, eventData)
	{
		_badges = ((eventData.badges <= 0) ? 1 : eventData.badges);
	}

	protected override int GetCurrency()
	{
		return badges;
	}

	public bool Equals(GvEEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(GvEEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
