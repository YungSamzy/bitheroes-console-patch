using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.segmented;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{name} (EventRef)")]
public abstract class EventRef : IEquatable<EventRef>, IComparable<EventRef>
{
	public const int EVENT_TYPE_NONE = 0;

	public const int EVENT_TYPE_PVP = 1;

	public const int EVENT_TYPE_RIFT = 2;

	public const int EVENT_TYPE_GAUNTLET = 3;

	public const int EVENT_TYPE_GVG = 4;

	public const int EVENT_TYPE_INVASION = 5;

	public const int EVENT_TYPE_FISHING = 6;

	public const int EVENT_TYPE_GVE = 7;

	public const int EVENT_RANK_TYPE_HIGHEST = 0;

	public const int EVENT_RANK_TYPE_POINTS = 1;

	public const int MAX_EVENTS_HISTORY = 2;

	public const string EVENT_REWARD_BOOK = "EventRewardBook";

	private static Dictionary<string, int> EVENT_RANK_TYPES = new Dictionary<string, int>
	{
		["highest"] = 0,
		["points"] = 1
	};

	private static string[] EVENT_RANK_TYPE_NAMES;

	private int _eventType;

	private EventRewards _rankRewards;

	private EventRewards _pointRewards;

	private EventRewards _guildRankRewards;

	private EventRewards _guildPointRewards;

	private List<InvasionEventTierRef> _tierRewards;

	private TeamRules _teamRules;

	protected DateRef dateRef;

	private GvEZoneRef _zoneRef;

	private string _battleBG;

	private MusicRef _battleMusic;

	private SegmentedRewards _segmentedRanksReward;

	private SegmentedRewards _segmentedPointsReward;

	private EventSegmentedRewards _eventSegmentedRanksReward;

	private EventSegmentedRewards _eventSegmentedPointsReward;

	private int _id;

	private string _rawName;

	private string _name;

	private string _rawDesc;

	private string _desc;

	private float _divider;

	private string _eventRankType;

	private bool _loadLocal;

	private string _book;

	public int id => _id;

	public int eventType => _eventType;

	public string name
	{
		get
		{
			if (_name == null)
			{
				_name = Language.GetString(_rawName);
			}
			return _name;
		}
	}

	public string desc
	{
		get
		{
			if (_desc == null)
			{
				_desc = Language.GetString(_rawDesc);
			}
			return _desc;
		}
	}

	public float divider => _divider;

	public bool hasSegmentedRewards
	{
		get
		{
			if (_segmentedPointsReward == null && _segmentedRanksReward == null && _eventSegmentedRanksReward == null)
			{
				return _eventSegmentedPointsReward != null;
			}
			return true;
		}
	}

	public EventRewards guildRankRewards => _guildRankRewards;

	public EventRewards guildPointRewards => _guildPointRewards;

	public List<InvasionEventTierRef> tierRewards => _tierRewards;

	public TeamRules teamRules => _teamRules;

	public bool loadLocal => _loadLocal;

	public GvEZoneRef zoneRef => _zoneRef;

	public string battleBGURL
	{
		get
		{
			if (_battleBG == null)
			{
				return null;
			}
			return _battleBG;
		}
	}

	public MusicRef battleMusic => _battleMusic;

	public EventRef(int type, BaseEventBookData.Event eventData)
	{
		_id = eventData.id;
		_rawName = eventData.name;
		_rawDesc = eventData.desc;
		_divider = ((eventData.divider != 0f) ? eventData.divider : 1f);
		_eventType = type;
		_eventRankType = ((eventData.type == null) ? (eventData.type = "highest") : eventData.type);
		_loadLocal = eventData.loadLocal;
		_book = eventData.book;
		dateRef = new DateRef(eventData.startDate, eventData.endDate);
		int intFromStringProperty = Util.GetIntFromStringProperty(eventData.slots, 1);
		int intFromStringProperty2 = Util.GetIntFromStringProperty(eventData.size, intFromStringProperty);
		bool boolFromStringProperty = Util.GetBoolFromStringProperty(eventData.allowFamiliars, defaultValue: true);
		bool boolFromStringProperty2 = Util.GetBoolFromStringProperty(eventData.allowFriends, defaultValue: true);
		bool boolFromStringProperty3 = Util.GetBoolFromStringProperty(eventData.allowGuildmates, defaultValue: true);
		bool boolFromStringProperty4 = Util.GetBoolFromStringProperty(eventData.statBalance);
		float familiarMult = ((eventData.familiarMult != null) ? Util.GetFloatFromStringProperty(eventData.familiarMult) : 1f);
		List<RarityRef> familiarRarities = ((eventData.familiarRarities != null) ? RarityBook.LookupLinks(Util.getStringVectorFromString(eventData.familiarRarities)) : null);
		List<FamiliarRef> familiarsAdded = ((eventData.familiarsAdded != null) ? FamiliarBook.LookupIDs(Util.getIntVectorFromString(eventData.familiarsAdded)) : new List<FamiliarRef>());
		_teamRules = new TeamRules(intFromStringProperty, intFromStringProperty2, boolFromStringProperty, boolFromStringProperty2, boolFromStringProperty3, boolFromStringProperty4);
		_teamRules.setModifiers(familiarMult, familiarRarities, familiarsAdded);
		List<EventRewardRef> list = null;
		if (eventData.rewards != null)
		{
			list = new List<EventRewardRef>();
			foreach (BaseEventBookData.Reward item in eventData.rewards.ranks.lstReward)
			{
				list.Add(new EventRewardRef(item));
			}
			_rankRewards = new EventRewards(list);
			list = new List<EventRewardRef>();
			foreach (BaseEventBookData.Reward item2 in eventData.rewards.points.lstReward)
			{
				list.Add(new EventRewardRef(item2));
			}
			_pointRewards = new EventRewards(list);
		}
		else if (eventData.segmented != null)
		{
			if (_book == null)
			{
				_segmentedRanksReward = new SegmentedRewards(eventData.segmented.ranksReward.pool);
				_segmentedPointsReward = new SegmentedRewards(eventData.segmented.pointsReward.pool);
			}
			else if (_book.Equals("EventRewardBook"))
			{
				_eventSegmentedRanksReward = new EventSegmentedRewards(eventData.segmented.ranksReward.pool);
				_eventSegmentedPointsReward = new EventSegmentedRewards(eventData.segmented.pointsReward.pool);
			}
		}
		if (eventData.guildRewards != null && eventData.guildRewards.ranks != null && eventData.guildRewards.ranks.lstReward != null)
		{
			list = new List<EventRewardRef>();
			foreach (BaseEventBookData.Reward item3 in eventData.guildRewards.ranks.lstReward)
			{
				list.Add(new EventRewardRef(item3));
			}
			_guildRankRewards = new EventRewards(list);
		}
		if (eventData.notSegmented != null && eventData.notSegmented.ranksReward != null && eventData.notSegmented.ranksReward.pool != null)
		{
			list = new List<EventRewardRef>();
			EventGuildRewardRef eventGuildRewardRef = EventRewardBook.LookupGuildReward(eventData.notSegmented.ranksReward.pool);
			_guildRankRewards = new EventRewards(eventGuildRewardRef.rewards);
		}
		if (eventData.guildRewards != null && eventData.guildRewards.points != null && eventData.guildRewards.points.lstReward != null)
		{
			list = new List<EventRewardRef>();
			foreach (BaseEventBookData.Reward item4 in eventData.guildRewards.points.lstReward)
			{
				list.Add(new EventRewardRef(item4));
			}
			_guildPointRewards = new EventRewards(list);
		}
		if (eventData.notSegmented != null && eventData.notSegmented.pointsReward != null && eventData.notSegmented.pointsReward.pool != null)
		{
			list = new List<EventRewardRef>();
			EventGuildRewardRef eventGuildRewardRef2 = EventRewardBook.LookupGuildReward(eventData.notSegmented.pointsReward.pool);
			_guildPointRewards = new EventRewards(eventGuildRewardRef2.rewards);
		}
		if (eventData.lstTierRewards != null && eventData.lstTierRewards.Count > 0)
		{
			_tierRewards = new List<InvasionEventTierRef>();
			foreach (BaseEventBookData.InvasionTierData lstTierReward in eventData.lstTierRewards)
			{
				_tierRewards.Add(new InvasionEventTierRef(lstTierReward.id, lstTierReward));
			}
		}
		if (type == 7)
		{
			_zoneRef = GvEEventBook.LookupZone(eventData.zoneID);
		}
		_battleBG = ((eventData.battleBG != null) ? eventData.battleBG : null);
		_battleMusic = MusicBook.Lookup((eventData.battleMusic != null) ? eventData.battleMusic : "battle");
	}

	public DateRef GetDateRef()
	{
		return dateRef;
	}

	public List<ItemData> getRewardItems(int zone, int rank, int points)
	{
		List<ItemData> list = new List<ItemData>();
		if (hasRankRewards())
		{
			EventRewardRef rewardRef = getRankRewards(zone).getRewardRef(rank);
			if (rewardRef != null)
			{
				foreach (ItemData item in rewardRef.items)
				{
					if (item != null)
					{
						list.Add(item);
					}
				}
			}
		}
		if (hasPointsRewards())
		{
			EventRewardRef rewardRef2 = getPointRewards(zone).getRewardRef(points);
			if (rewardRef2 != null)
			{
				foreach (ItemData item2 in rewardRef2.items)
				{
					if (item2 != null)
					{
						list.Add(item2);
					}
				}
			}
		}
		return condenseItems(list);
	}

	public int getTeamType()
	{
		return _eventType switch
		{
			1 => 2, 
			4 => 6, 
			_ => 0, 
		};
	}

	public int getCurrencyID()
	{
		return _eventType switch
		{
			1 => 5, 
			4 => 10, 
			_ => 9, 
		};
	}

	public static List<ItemData> condenseItems(List<ItemData> items)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in items)
		{
			if (item != null && !(item.itemRef == null))
			{
				ItemData itemFromList = getItemFromList(item.itemRef.id, item.itemRef.itemType, list);
				if (itemFromList != null)
				{
					itemFromList.qty += item.qty;
				}
				else
				{
					list.Add(item.copy());
				}
			}
		}
		return list;
	}

	public static ItemData getItemFromList(int id, int type, List<ItemData> items)
	{
		foreach (ItemData item in items)
		{
			if (item.itemRef.id == id && item.itemRef.itemType == type)
			{
				return item;
			}
		}
		return null;
	}

	public static string getEventTypeName(int type)
	{
		return Language.GetString("event_type_" + type + "_name");
	}

	public static string getEventTypeNameShort(int type)
	{
		return Language.GetString("event_type_" + type + "_short_name");
	}

	public static int GetEventRankType(string type)
	{
		if (EVENT_RANK_TYPES.ContainsKey(type))
		{
			return EVENT_RANK_TYPES[type];
		}
		return 0;
	}

	public static string GetEventRankTypeName(int type, int eventType = 0)
	{
		return Language.GetString(GetEventRankLanguage(eventType)[type]);
	}

	private static string[] GetEventRankLanguage(int eventType)
	{
		if (eventType != 6)
		{
			return new string[2] { "ui_highest", "ui_points" };
		}
		return new string[2] { "ui_weight", "ui_points" };
	}

	protected abstract int GetCurrency();

	public int GetCurrencyCost(CurrencyBonusRef bonus)
	{
		if (bonus != null)
		{
			return GetCurrency() * bonus.multiplier;
		}
		return GetCurrency();
	}

	public int GetCurrentValue(int highest, int points)
	{
		if (GetEventRankType(_eventRankType) == 0)
		{
			return highest;
		}
		return points;
	}

	public int GetAltValue(int highest, int points)
	{
		if (GetEventRankType(_eventRankType) == 0)
		{
			return points;
		}
		return highest;
	}

	public string GetCurrentRankTypeName()
	{
		return GetEventRankTypeName(GetEventRankType(_eventRankType), _eventType);
	}

	public string GetAltTypeName()
	{
		if (GetEventRankType(_eventRankType) == 1)
		{
			return GetEventRankTypeName(0);
		}
		return GetEventRankTypeName(1);
	}

	public bool hasRankRewards()
	{
		if (_rankRewards != null || _segmentedRanksReward != null || _eventSegmentedRanksReward != null)
		{
			return true;
		}
		return false;
	}

	public EventRewards getRankRewards(int zone)
	{
		if (_rankRewards != null)
		{
			return _rankRewards;
		}
		if (_segmentedRanksReward != null)
		{
			return _segmentedRanksReward.GetEventRewards(zone);
		}
		if (_eventSegmentedRanksReward != null)
		{
			return _eventSegmentedRanksReward.GetEventRewards(zone);
		}
		return null;
	}

	public bool hasPointsRewards()
	{
		if (_pointRewards != null || _segmentedPointsReward != null || _eventSegmentedPointsReward != null)
		{
			return true;
		}
		return false;
	}

	public EventRewards getPointRewards(int zone)
	{
		if (_pointRewards != null)
		{
			return _pointRewards;
		}
		if (_segmentedPointsReward != null)
		{
			return _segmentedPointsReward.GetEventRewards(zone);
		}
		if (_eventSegmentedPointsReward != null)
		{
			return _eventSegmentedPointsReward.GetEventRewards(zone);
		}
		return null;
	}

	public bool Equals(EventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(EventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
