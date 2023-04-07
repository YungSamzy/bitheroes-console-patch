using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.ability;

[DebuggerDisplay("{typeLink} (AbilityPositionRef)")]
public class AbilityPositionRef : IEquatable<AbilityPositionRef>, IComparable<AbilityPositionRef>
{
	public const int POSITION_NONE = 0;

	public const int POSITION_CENTER = 1;

	public const int POSITION_TARGET = 2;

	public const int POSITION_TARGET_FRONT = 3;

	public const int POSITION_TARGET_BACK = 4;

	public const int POSITION_ENEMY_TEAM_CENTER = 5;

	public const int POSITION_ENEMY_TEAM_FRONT = 6;

	public const int POSITION_ENEMY_TEAM_BACK = 7;

	public const int POSITION_ENEMY_TEAM_TOP = 8;

	public const int POSITION_ENEMY_TEAM_BOTTOM = 9;

	public const int POSITION_SELF_TEAM_CENTER = 10;

	public const int POSITION_SELF_TEAM_FRONT = 11;

	public const int POSITION_SELF_TEAM_BACK = 12;

	public const int POSITION_SELF_TEAM_TOP = 13;

	public const int POSITION_SELF_TEAM_BOTTOM = 14;

	private static Dictionary<string, int> POSITION_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["center"] = 1,
		["target"] = 2,
		["targetfront"] = 3,
		["targetback"] = 4,
		["enemyteamcenter"] = 5,
		["enemyteamfront"] = 6,
		["enemyteamback"] = 7,
		["enemyteamtop"] = 8,
		["enemyteambottom"] = 9,
		["selfteamcenter"] = 10,
		["selfteamfront"] = 11,
		["selfteamback"] = 12,
		["selfteamtop"] = 13,
		["selfteambottom"] = 14
	};

	private AbilityBookData.Position positionData;

	public int id => positionData.id;

	public string link => positionData.link;

	public int type => getType(positionData.type);

	private string typeLink => positionData.type;

	public Vector2 offset => Util.GetVector2FromStringProperty(positionData.offset);

	public bool flip => positionData.flip;

	public string startAnimation
	{
		get
		{
			if (positionData.startAnimation == null)
			{
				return "";
			}
			return positionData.startAnimation;
		}
	}

	public float startSpeed => positionData.startSpeed;

	public bool startSpeedScale => positionData.startSpeedScale;

	public float startDelay => positionData.startDelay;

	public string endAnimation => positionData.endAnimation;

	public float endSpeed => positionData.endSpeed;

	public bool endSpeedScale => positionData.endSpeedScale;

	public float endDelay => positionData.endDelay;

	public AbilityPositionRef(AbilityBookData.Position positionData)
	{
		this.positionData = positionData;
	}

	public static int getType(string type)
	{
		return POSITION_TYPES[type.ToLowerInvariant()];
	}

	public bool Equals(AbilityPositionRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(AbilityPositionRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
