using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;

namespace com.ultrabit.bitheroes.model.pvp;

[DebuggerDisplay("{name} (PvPEventRef)")]
public class PvPEventRef : EventRef, IEquatable<PvPEventRef>, IComparable<PvPEventRef>
{
	private string _battleBGPVP;

	private MusicRef _battleMusicPVP;

	private int _tickets;

	public int tickets => _tickets;

	public PvPEventRef(BaseEventBookData.Event eventData)
		: base(1, eventData)
	{
		_tickets = ((eventData.tickets <= 0) ? 1 : eventData.tickets);
	}

	public Asset getBattleBGAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	protected override int GetCurrency()
	{
		return tickets;
	}

	public bool Equals(PvPEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(PvPEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
