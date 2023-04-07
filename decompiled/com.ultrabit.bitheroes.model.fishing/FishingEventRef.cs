using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.fishing;

[DebuggerDisplay("{name} (FishingEventRef)")]
public class FishingEventRef : EventRef, IEquatable<FishingEventRef>, IComparable<FishingEventRef>
{
	public const int FISHING_TYPE_HIGHEST = 0;

	public const int FISHING_TYPE_POINTS = 1;

	private static Dictionary<string, int> FISHING_TYPES = new Dictionary<string, int>
	{
		["highest"] = 0,
		["points"] = 1
	};

	private static List<string> FISHING_TYPE_NAMES = new List<string> { "ui_weight", "ui_points" };

	private int _fishingType;

	public int fishingType => _fishingType;

	public FishingEventRef(BaseEventBookData.Event data)
		: base(6, data)
	{
		_fishingType = ((data.type != null) ? getFishingType(data.type) : 0);
	}

	public int getCurrentValue(int highest, int points)
	{
		if (fishingType == 0)
		{
			return highest;
		}
		return points;
	}

	public int getAltValue(int highest, int points)
	{
		if (fishingType == 0)
		{
			return points;
		}
		return highest;
	}

	public string getCurrentTypeName()
	{
		return getFishingTypeName(fishingType);
	}

	public string getAltTypeName()
	{
		if (fishingType == 1)
		{
			return getFishingTypeName(0);
		}
		return getFishingTypeName(1);
	}

	public static int getFishingType(string type)
	{
		return FISHING_TYPES[type.ToLowerInvariant()];
	}

	public static string getFishingTypeName(int type)
	{
		return Language.GetString(FISHING_TYPE_NAMES[type]);
	}

	protected override int GetCurrency()
	{
		return -1;
	}

	public bool Equals(FishingEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(FishingEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
