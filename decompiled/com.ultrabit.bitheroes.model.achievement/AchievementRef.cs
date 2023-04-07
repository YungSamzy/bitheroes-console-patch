using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.zone;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.achievement;

[DebuggerDisplay("{name} (AchievementRef)")]
public class AchievementRef : BaseRef, IEquatable<AchievementRef>, IComparable<AchievementRef>
{
	public const int ACHIEVEMENT_TYPE_LEVEL = 1;

	public const int ACHIEVEMENT_TYPE_FAMILIAR = 2;

	public const int ACHIEVEMENT_TYPE_FAMILIAR_COUNT = 3;

	public const int ACHIEVEMENT_TYPE_ZONE_COMPLETE = 4;

	public const int ACHIEVEMENT_TYPE_ZONE_STARS = 5;

	public const int ACHIEVEMENT_TYPE_FUSION = 6;

	public const int ACHIEVEMENT_TYPE_FRIEND = 7;

	public const int ACHIEVEMENT_TYPE_GUILD = 8;

	private static Dictionary<string, int> ACHIEVEMENT_TYPES = new Dictionary<string, int>
	{
		["level"] = 1,
		["familiar"] = 2,
		["familiarcount"] = 3,
		["zonecomplete"] = 4,
		["zonestars"] = 5,
		["fusion"] = 6,
		["friend"] = 7,
		["guild"] = 8
	};

	private int _type;

	private int _value;

	private string _achievementID;

	private int[] _revealIDs;

	public int type => _type;

	public int value => _value;

	public string achievementID => _achievementID;

	public AchievementRef(int id, int type, int value, string achievementID, int[] revealIDs)
		: base(id)
	{
		_type = type;
		_value = value;
		_achievementID = achievementID;
		_revealIDs = revealIDs;
	}

	public int getSteps()
	{
		if (GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null)
		{
			return 0;
		}
		return _type switch
		{
			1 => GameData.instance.PROJECT.character.level, 
			3 => FamiliarBook.GetOwnedCount(), 
			5 => GameData.instance.PROJECT.character.zones.getTotalStars(), 
			_ => 0, 
		};
	}

	public bool getCompleted()
	{
		if (GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null)
		{
			return false;
		}
		int steps = getSteps();
		switch (_type)
		{
		case 1:
		case 3:
		case 5:
			return steps >= _value;
		case 2:
			return FamiliarBook.GetOwnedCount() > 0;
		case 4:
			return GameData.instance.PROJECT.character.zones.zoneIsCompleted(ZoneBook.Lookup(_value));
		case 6:
			return FamiliarBook.GetOwnedFusionCount() > 0;
		case 7:
			return GameData.instance.PROJECT.character.friends.Count > 0;
		case 8:
			return GameData.instance.PROJECT.character.guildData != null;
		default:
			return false;
		}
	}

	public float getCompletePercentage()
	{
		if (getCompleted())
		{
			return 1f;
		}
		if (_value <= 0)
		{
			return 0f;
		}
		return (float)getSteps() / (float)_value;
	}

	public List<AchievementRef> getRevealRefs()
	{
		List<AchievementRef> list = new List<AchievementRef>();
		if (_revealIDs == null || _revealIDs.Length == 0)
		{
			return list;
		}
		int[] revealIDs = _revealIDs;
		foreach (int num in revealIDs)
		{
			list.Add(AchievementBook.Lookup(num));
		}
		return list;
	}

	public static int getAchievementType(string type)
	{
		return ACHIEVEMENT_TYPES[type.ToLowerInvariant()];
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(AchievementRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AchievementRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
