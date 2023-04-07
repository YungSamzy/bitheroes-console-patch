using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;

namespace com.ultrabit.bitheroes.model.game;

[DebuggerDisplay("{typeName} (GameRequirement)")]
public class GameRequirement : IEquatable<GameRequirement>, IComparable<GameRequirement>
{
	public const int TYPE_QUEST = 0;

	public const int TYPE_PVP = 1;

	public const int TYPE_CRAFT = 2;

	public const int TYPE_RAID = 3;

	public const int TYPE_CHAT = 4;

	public const int TYPE_FRIENDS = 5;

	public const int TYPE_SHOP = 6;

	public const int TYPE_LEADERBOARD = 7;

	public const int TYPE_GUILD = 8;

	public const int TYPE_SETTINGS = 9;

	public const int TYPE_AUTO_PILOT = 10;

	public const int TYPE_DAILY_REWARDS = 11;

	public const int TYPE_NEWS = 12;

	public const int TYPE_FAMILIARS = 13;

	public const int TYPE_BATTLE_SPEED = 14;

	public const int TYPE_BATTLE_FORMATION = 15;

	public const int TYPE_BATTLE_CONSUMABLES = 16;

	public const int TYPE_DAILY_QUESTS = 17;

	public const int TYPE_AD = 18;

	public const int TYPE_FUSION = 19;

	public const int TYPE_MOUNTS = 20;

	public const int TYPE_RIFTS = 21;

	public const int TYPE_RUNE = 22;

	public const int TYPE_GAUNTLET = 23;

	public const int TYPE_KONGREGATE = 24;

	public const int TYPE_FISHING = 25;

	public const int TYPE_NBP_UNLOCK = 26;

	public const int TYPE_NBP_LOCK = 27;

	public const int TYPE_RATINGS_UNLOCK = 28;

	public const int TYPE_RATINGS_LOCK = 29;

	public const int TYPE_GVG = 30;

	public const int TYPE_FAMILIAR_STABLE = 31;

	public const int TYPE_ENCHANTS = 32;

	public const int TYPE_INVASION = 33;

	public const int TYPE_BRAWL = 34;

	public const int TYPE_GVE = 35;

	public const int TYPE_AUGMENTS = 36;

	public const int TYPE_PLAYER_VOTING = 37;

	public const int TYPE_STAT_RESET = 38;

	public const int TYPE_SERVICES = 39;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		["quest"] = 0,
		["pvp"] = 1,
		["craft"] = 2,
		["raid"] = 3,
		["chat"] = 4,
		["friends"] = 5,
		["shop"] = 6,
		["leaderboard"] = 7,
		["guild"] = 8,
		["settings"] = 9,
		["autopilot"] = 10,
		["dailyrewards"] = 11,
		["news"] = 12,
		["familiars"] = 13,
		["battlespeed"] = 14,
		["battleformation"] = 15,
		["battleconsumables"] = 16,
		["dailyquests"] = 17,
		["ad"] = 18,
		["fusion"] = 19,
		["mounts"] = 20,
		["rifts"] = 21,
		["rune"] = 22,
		["gauntlet"] = 23,
		["kongregate"] = 24,
		["fishing"] = 25,
		["nbpunlock"] = 26,
		["nbplock"] = 27,
		["ratingsunlock"] = 28,
		["ratingslock"] = 29,
		["gvg"] = 30,
		["familiarstable"] = 31,
		["enchants"] = 32,
		["invasion"] = 33,
		["brawl"] = 34,
		["gve"] = 35,
		["augments"] = 36,
		["playervoting"] = 37,
		["statreset"] = 38,
		["services"] = 39
	};

	private VariableBookData.Requirement _requirement;

	private int _type = -1;

	private string typeName => _requirement.type;

	public int type
	{
		get
		{
			if (_type == -1)
			{
				_type = GetType(_requirement.type);
			}
			return _type;
		}
	}

	public bool hide => _requirement.hide;

	public GameRequirement(VariableBookData.Requirement requirement)
	{
		_requirement = requirement;
	}

	public bool RequirementsMet()
	{
		return GetRequirementsText() == null;
	}

	public DialogRef GetDialogLocked()
	{
		if (_requirement.dialogLocked == null)
		{
			return null;
		}
		return DialogBook.Lookup(_requirement.dialogLocked);
	}

	public string GetRequirementsText()
	{
		if (GameData.instance.PROJECT.character == null)
		{
			if (_requirement.message == null)
			{
				return Language.GetString("error_requirement_login");
			}
			return _requirement.message;
		}
		if (GameData.instance.PROJECT.character.level < _requirement.levelReq)
		{
			if (_requirement.message == null)
			{
				return Language.GetString("error_requirement_level", new string[1] { Util.NumberFormat(_requirement.levelReq) }, color: true);
			}
			return _requirement.message;
		}
		if (_requirement.itemReqType != null)
		{
			int itemType = ItemRef.getItemType(_requirement.itemReqType);
			if (itemType > 0 && GameData.instance.PROJECT.character.inventory.getItemTypeQty(itemType) < _requirement.itemReqQty)
			{
				if (_requirement.message == null)
				{
					return Language.GetString("error_requirement_item_type", new string[2]
					{
						Util.NumberFormat(_requirement.itemReqQty),
						ItemRef.GetItemNamePlural(itemType)
					}, color: true);
				}
				return _requirement.message;
			}
		}
		ZoneRef zoneRef = ZoneBook.Lookup(_requirement.zoneCompleteReq);
		if (zoneRef != null || _requirement.zoneCompleteReq == 0)
		{
			if (_requirement.zoneCompleteReq != 0)
			{
				if (!GameData.instance.PROJECT.character.zones.zoneIsCompleted(zoneRef))
				{
					if (_requirement.message == null)
					{
						return Language.GetString("error_requirement_complete", new string[1] { Language.GetString(zoneRef.name) });
					}
					return _requirement.message;
				}
			}
			else
			{
				zoneRef = ZoneBook.Lookup(1);
			}
			ZoneNodeRef nodeRef = zoneRef.getNodeRef(_requirement.zoneCompleteNodeReq);
			if (nodeRef != null && !GameData.instance.PROJECT.character.zones.nodeIsCompleted(nodeRef))
			{
				if (_requirement.message == null)
				{
					return Language.GetString("error_requirement_complete", new string[1] { Language.GetString(nodeRef.name) });
				}
				return _requirement.message;
			}
		}
		ZoneRef zoneRef2 = ZoneBook.Lookup(_requirement.zoneUnlockReq);
		if (zoneRef2 != null)
		{
			if (!GameData.instance.PROJECT.character.zones.zoneIsUnlocked(zoneRef2))
			{
				if (_requirement.message == null)
				{
					return Language.GetString("error_requirement_unlock", new string[1] { Language.GetString(zoneRef2.name) });
				}
				return _requirement.message;
			}
			ZoneNodeRef nodeRef2 = zoneRef2.getNodeRef(_requirement.zoneUnlockNodeReq);
			if (nodeRef2 != null && !GameData.instance.PROJECT.character.zones.nodeIsUnlocked(nodeRef2) && !GameData.instance.PROJECT.character.zones.nodeIsCompleted(nodeRef2))
			{
				if (_requirement.message == null)
				{
					return Language.GetString("error_requirement_unlock", new string[1] { Language.GetString(nodeRef2.name) });
				}
				return _requirement.message;
			}
		}
		return null;
	}

	public static int GetType(string type)
	{
		return TYPES[type.ToLowerInvariant()];
	}

	public static string GetTypeName(int value)
	{
		if (!TYPES.ContainsValue(value))
		{
			return null;
		}
		foreach (KeyValuePair<string, int> tYPE in TYPES)
		{
			if (tYPE.Value == value)
			{
				return tYPE.Key;
			}
		}
		return null;
	}

	public bool Equals(GameRequirement other)
	{
		if (other == null)
		{
			return false;
		}
		return type.Equals(other.type);
	}

	public int CompareTo(GameRequirement other)
	{
		if (other == null)
		{
			return -1;
		}
		return type.CompareTo(other.type);
	}
}
