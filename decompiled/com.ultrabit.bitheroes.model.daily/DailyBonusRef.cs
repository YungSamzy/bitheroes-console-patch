using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.daily;

[DebuggerDisplay("{dayName} (DailyBonusRef)")]
public class DailyBonusRef : BaseRef, IEquatable<DailyBonusRef>, IComparable<DailyBonusRef>
{
	public const int DAY_SUNDAY = 0;

	public const int DAY_MONDAY = 1;

	public const int DAY_TUESDAY = 2;

	public const int DAY_WEDNESDAY = 3;

	public const int DAY_THURSDAY = 4;

	public const int DAY_FRIDAY = 5;

	public const int DAY_SATURDAY = 6;

	private static Dictionary<string, int> DAYS = new Dictionary<string, int>
	{
		["sunday"] = 0,
		["monday"] = 1,
		["tuesday"] = 2,
		["wednesday"] = 3,
		["thursday"] = 4,
		["friday"] = 5,
		["saturday"] = 6
	};

	private DateTime _date;

	private List<GameModifier> _modifiers;

	private bool _hasDate;

	private int _day;

	private string _dayName;

	public bool hasDate => _hasDate;

	public int day => _day;

	public DateTime date => _date;

	public List<GameModifier> modifiers => _modifiers;

	public string dayName => _dayName;

	public DailyBonusRef(int id, DailyBonusBookData.Daily dailyData)
		: base(id)
	{
		_modifiers = GameModifier.GetGameModifierFromData(dailyData.modifiers, dailyData.lstModifier);
		_hasDate = false;
		if (dailyData.date != null)
		{
			_hasDate = true;
			_date = Util.GetDateFromString(dailyData.date);
		}
		_dayName = dailyData.day;
		_day = GetDay(_dayName);
	}

	public long GetMillisecondsRemaining()
	{
		DateTime dateTime = ServerExtension.instance.GetDate();
		if (day < 0 || dateTime.DayOfWeek != (DayOfWeek)day)
		{
			_ = _date;
			if (_date.Year != dateTime.Year || _date.Month != dateTime.Month || !(_date.Date == dateTime.Date))
			{
				return 0L;
			}
		}
		return ServerExtension.instance.GetMillisecondsTillDayEnds();
	}

	public static int GetDay(string day)
	{
		if (day == null)
		{
			return -1;
		}
		return DAYS[day.ToLowerInvariant()];
	}

	public static string getDayName(int day)
	{
		return Language.GetString("day_" + day + "_name");
	}

	public override Sprite GetSpriteIcon()
	{
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.DAILY_ICON, icon);
	}

	public bool Equals(DailyBonusRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(DailyBonusRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
