using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;

namespace com.ultrabit.bitheroes.model.gvg;

[DebuggerDisplay("{name} (GvGEventRef)")]
public class GvGEventRef : EventRef, IEquatable<GvGEventRef>, IComparable<GvGEventRef>
{
	private MusicRef _battleMusic;

	private EventRewards _guildRankRewards;

	private EventRewards _guildPointRewards;

	private int _badges;

	public int badges => _badges;

	public GvGEventRef(BaseEventBookData.Event eventData)
		: base(4, eventData)
	{
		_badges = ((eventData.badges <= 0) ? 1 : eventData.badges);
	}

	public Asset getBattleBGAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	protected override int GetCurrency()
	{
		return badges;
	}

	public bool Equals(GvGEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(GvGEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
