using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.invasion;

[DebuggerDisplay("{name} (InvasionEventRef)")]
public class InvasionEventRef : EventRef, IEquatable<InvasionEventRef>, IComparable<InvasionEventRef>
{
	public const int INVASION_TYPE_HIGHEST = 0;

	public const int INVASION_TYPE_POINTS = 1;

	private static Dictionary<string, int> INVASION_TYPES = new Dictionary<string, int>
	{
		["highest"] = 0,
		["points"] = 1
	};

	private static List<string> INVASION_TYPE_NAMES = new List<string> { "ui_highest", "ui_points" };

	private BaseEventBookData.Event _eventData;

	private List<InvasionEventTierRef> _tiers;

	private List<InvasionEventLevelRef> _levels;

	public int invasionType => GetInvasionType(_eventData.type);

	public int badges
	{
		get
		{
			if (_eventData.badges <= 0)
			{
				return 1;
			}
			return _eventData.badges;
		}
	}

	public List<InvasionEventLevelRef> levels => _levels;

	public InvasionEventRef(BaseEventBookData.Event eventData)
		: base(5, eventData)
	{
		_eventData = eventData;
		if (!base.hasSegmentedRewards)
		{
			_tiers = new List<InvasionEventTierRef>();
			{
				foreach (BaseEventBookData.InvasionTierData lstTierReward in eventData.lstTierRewards)
				{
					_tiers.Add(new InvasionEventTierRef(lstTierReward.id, lstTierReward));
				}
				return;
			}
		}
		_levels = InvasionEventBook.LookupPool(eventData.levels.pool).levels;
	}

	public int getBadges(CurrencyBonusRef bonusRef)
	{
		if (bonusRef != null)
		{
			return badges * bonusRef.multiplier;
		}
		return badges;
	}

	public int getCurrentValue(int highest, int points)
	{
		if (invasionType == 0)
		{
			return highest;
		}
		return points;
	}

	public int getAltValue(int highest, int points)
	{
		if (invasionType == 0)
		{
			return points;
		}
		return highest;
	}

	public string getCurrentTypeName()
	{
		return getInvasionTypeName(invasionType);
	}

	public string getAltTypeName()
	{
		if (invasionType == 0)
		{
			return getInvasionTypeName(0);
		}
		return getInvasionTypeName(1);
	}

	public static int GetInvasionType(string type)
	{
		if (type == null || !INVASION_TYPES.ContainsKey(type))
		{
			return 0;
		}
		return INVASION_TYPES[type.ToLowerInvariant()];
	}

	public static string getInvasionTypeName(int type)
	{
		return Language.GetString(INVASION_TYPE_NAMES[type]);
	}

	public InvasionEventTierRef GetTierRef(long points)
	{
		if (_tiers == null || _tiers.Count <= 0)
		{
			return null;
		}
		InvasionEventTierRef invasionEventTierRef = null;
		foreach (InvasionEventTierRef tier in _tiers)
		{
			if (!((float)points < tier.points) && (invasionEventTierRef == null || tier.points > invasionEventTierRef.points))
			{
				invasionEventTierRef = tier;
			}
		}
		return invasionEventTierRef;
	}

	public InvasionEventLevelRef GetLevelRef(long points)
	{
		if (_levels == null || _levels.Count <= 0)
		{
			return null;
		}
		InvasionEventLevelRef invasionEventLevelRef = null;
		foreach (InvasionEventLevelRef level in _levels)
		{
			if (points >= level.points && (invasionEventLevelRef == null || level.points > invasionEventLevelRef.points))
			{
				invasionEventLevelRef = level;
			}
		}
		return invasionEventLevelRef;
	}

	public InvasionEventTierRef GetLastTierRef()
	{
		return GetTierRef(long.MaxValue);
	}

	public InvasionEventTierRef GetFirstTierRef()
	{
		return GetTierRef(0L);
	}

	public InvasionEventLevelRef GetLastLevelRef()
	{
		return GetLevelRef(long.MaxValue);
	}

	public InvasionEventLevelRef GetFirstLevelRef()
	{
		return GetLevelRef(0L);
	}

	protected override int GetCurrency()
	{
		return badges;
	}

	public bool Equals(InvasionEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((EventRef)other);
	}

	public int CompareTo(InvasionEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((EventRef)other);
	}
}
