using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.rift;

[DebuggerDisplay("{name} (RiftEventRef)")]
public class RiftEventRef : EventRef, IEquatable<RiftEventRef>, IComparable<RiftEventRef>
{
	public const int RIFT_TYPE_HIGHEST = 0;

	public const int RIFT_TYPE_POINTS = 1;

	private static Dictionary<string, int> RIFT_TYPES = new Dictionary<string, int>
	{
		["highest"] = 0,
		["points"] = 1
	};

	private static List<string> RIFT_TYPE_NAMES = new List<string> { "ui_highest", "ui_points" };

	private BaseEventBookData.Event _eventData;

	public int riftType => getRiftType(_eventData.type);

	public int tokens
	{
		get
		{
			if (_eventData.tokens <= 0)
			{
				return 1;
			}
			return _eventData.tokens;
		}
	}

	public RiftEventRef(BaseEventBookData.Event eventData)
		: base(2, eventData)
	{
		_eventData = eventData;
	}

	public int getTokens(CurrencyBonusRef bonusRef)
	{
		if (bonusRef != null)
		{
			return _eventData.tokens * bonusRef.multiplier;
		}
		return tokens;
	}

	public int getCurrentValue(int highest, int points)
	{
		if (riftType == 0)
		{
			return highest;
		}
		return points;
	}

	public int getAltValue(int highest, int points)
	{
		if (riftType == 0)
		{
			return points;
		}
		return highest;
	}

	public string getCurrentTypeName()
	{
		return getRiftTypeName(riftType);
	}

	public string getAltTypeName()
	{
		if (riftType == 1)
		{
			return getRiftTypeName(0);
		}
		return getRiftTypeName(1);
	}

	public static int getRiftType(string type)
	{
		if (type == null || !RIFT_TYPES.ContainsKey(type))
		{
			return 0;
		}
		return RIFT_TYPES[type.ToLowerInvariant()];
	}

	public static string getRiftTypeName(int type)
	{
		return Language.GetString(RIFT_TYPE_NAMES[type]);
	}

	protected override int GetCurrency()
	{
		return tokens;
	}

	public bool Equals(RiftEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(RiftEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
