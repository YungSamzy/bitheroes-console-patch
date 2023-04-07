using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.gauntlet;

[DebuggerDisplay("{name} (GauntletEventRef)")]
public class GauntletEventRef : EventRef, IEquatable<GauntletEventRef>, IComparable<GauntletEventRef>
{
	public const int GAUNTLET_TYPE_HIGHEST = 0;

	public const int GAUNTLET_TYPE_POINTS = 1;

	private static Dictionary<string, int> GAUNTLET_TYPES = new Dictionary<string, int>
	{
		["highest"] = 0,
		["points"] = 1
	};

	private static List<string> GAUNTLET_TYPE_NAMES = new List<string> { "ui_highest", "ui_points" };

	private BaseEventBookData.Event _eventData;

	public int gauntletType => GetGauntletType(_eventData.type);

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

	public GauntletEventRef(BaseEventBookData.Event eventData)
		: base(3, eventData)
	{
		_eventData = eventData;
	}

	public int getTokens(CurrencyBonusRef bonusRef)
	{
		if (bonusRef != null)
		{
			return tokens * bonusRef.multiplier;
		}
		return tokens;
	}

	public int getCurrentValue(int highest, int points)
	{
		if (gauntletType == 0)
		{
			return highest;
		}
		return points;
	}

	public int getAltValue(int highest, int points)
	{
		if (gauntletType == 0)
		{
			return points;
		}
		return highest;
	}

	public string getCurrentTypeName()
	{
		return GetGauntletTypeName(gauntletType);
	}

	public string getAltTypeName()
	{
		if (gauntletType == 0)
		{
			return GetGauntletTypeName(0);
		}
		return GetGauntletTypeName(1);
	}

	public static int GetGauntletType(string type)
	{
		if (type == null || !GAUNTLET_TYPES.ContainsKey(type))
		{
			return 0;
		}
		return GAUNTLET_TYPES[type.ToLowerInvariant()];
	}

	public static string GetGauntletTypeName(int type)
	{
		return Language.GetString(GAUNTLET_TYPE_NAMES[type]);
	}

	protected override int GetCurrency()
	{
		return tokens;
	}

	public bool Equals(GauntletEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(GauntletEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
