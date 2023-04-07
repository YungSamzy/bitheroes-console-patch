using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.game;

[DebuggerDisplay("{desc} (GameModifier)")]
public class GameModifier : IEquatable<GameModifier>, IComparable<GameModifier>
{
	public const int TYPE_NONE = 0;

	public const int TYPE_BLOCK_CHANCE = 1;

	public const int TYPE_BLOCK_MULT = 2;

	public const int TYPE_EVADE_CHANCE = 3;

	public const int TYPE_CRIT_CHANCE = 4;

	public const int TYPE_CRIT_MULT = 5;

	public const int TYPE_MAGIC_FIND = 6;

	public const int TYPE_EXP_MULT = 7;

	public const int TYPE_GOLD_MULT = 8;

	public const int TYPE_MOVEMENT_SPEED_MULT = 9;

	public const int TYPE_ZONE_MAGIC_FIND = 10;

	public const int TYPE_ZONE_EXP_MULT = 11;

	public const int TYPE_ZONE_GOLD_MULT = 12;

	public const int TYPE_PVP_MAGIC_FIND = 13;

	public const int TYPE_PVP_EXP_MULT = 14;

	public const int TYPE_PVP_GOLD_MULT = 15;

	public const int TYPE_HEALTH_DRAIN = 16;

	public const int TYPE_POWER_MULT = 17;

	public const int TYPE_STAMINA_MULT = 18;

	public const int TYPE_AGILITY_MULT = 19;

	public const int TYPE_FAMILIAR_CHANCE = 20;

	public const int TYPE_DAMAGE_RETURN = 21;

	public const int TYPE_LOOT_ROLLS = 22;

	public const int TYPE_POINT_MULT = 23;

	public const int TYPE_TARGET_CHANGE = 24;

	public const int TYPE_DEFLECT_CHANCE = 25;

	public const int TYPE_DOUBLE_TURN_CHANCE = 26;

	public const int TYPE_DOUBLE_HIT_CHANCE = 27;

	public const int TYPE_METERLESS_CHANCE = 28;

	public const int TYPE_HEAL_GIVE_BONUS = 29;

	public const int TYPE_HEAL_RECEIVE_BONUS = 30;

	public const int TYPE_DAMAGE_GIVE_BONUS = 31;

	public const int TYPE_DAMAGE_RECEIVE_BONUS = 32;

	public const int TYPE_METER_GAIN_BONUS = 33;

	public const int TYPE_METER_DRAIN = 34;

	public const int TYPE_RICOCHET_CHANCE = 35;

	public const int TYPE_REDIRECT_CHANCE = 36;

	public const int TYPE_ENERGY_MAX = 37;

	public const int TYPE_TICKET_MAX = 38;

	public const int TYPE_SHARD_MAX = 39;

	public const int TYPE_TOKEN_MAX = 40;

	public const int TYPE_BADGE_MAX = 41;

	public const int TYPE_GUILD_MEMBERS = 42;

	public const int TYPE_ABSORB_CHANCE = 43;

	public const int TYPE_SHIELD_REGEN = 44;

	public const int TYPE_HEAL_GIVE_SHIELD_BONUS = 45;

	public const int TYPE_HEAL_RECEIVE_SHIELD_BONUS = 46;

	public const int TYPE_HEAL_GIVE_OVERSHIELD_BONUS = 47;

	public const int TYPE_HEAL_RECEIVE_OVERSHIELD_BONUS = 48;

	public const int TYPE_DOUBLE_HEAL_GIVE_CHANCE = 49;

	public const int TYPE_DOUBLE_HEAL_RECEIVE_CHANCE = 50;

	public const int TYPE_TEAM_ENRAGE = 51;

	public const int TYPE_QUAD_TURN_CHANCE = 52;

	public const int TYPE_FISHING_MAX_DISTANCE = 53;

	public const int TYPE_FISHING_BAR_SIZE = 54;

	public const int TYPE_FISHING_FIND = 55;

	public const int TYPE_BRAWL_MAGIC_FIND = 56;

	public const int TYPE_BRAWL_EXP_MULT = 57;

	public const int TYPE_BRAWL_GOLD_MULT = 58;

	public const int TYPE_GAUNTLET_MAGIC_FIND = 59;

	public const int TYPE_GAUNTLET_EXP_MULT = 60;

	public const int TYPE_GAUNTLET_GOLD_MULT = 61;

	public const int TYPE_RAID_MAGIC_FIND = 62;

	public const int TYPE_RAID_EXP_MULT = 63;

	public const int TYPE_RAID_GOLD_MULT = 64;

	public const int TYPE_RANDOM_SKILL_CHANCE = 65;

	public const int TYPE_HEAL_ENRAGE = 66;

	public const int TYPE_SET_COUNT = 67;

	public const int TYPE_DOUBLE_RUNE = 68;

	public const int TYPE_DOUBLE_ENCHANT = 69;

	public const int TYPE_DOUBLE_MOUNT = 70;

	public const int TYPE_DAMAGE_REMAINING_SPLIT = 71;

	public const int TYPE_DAMAGE_COUNT_RESET = 72;

	public const int TYPE_HEAL_COUNT_RESET = 73;

	public const int TYPE_SHIELD_MULT = 74;

	public const int TYPE_DEFENSE_IGNORE_CHANCE = 76;

	public const int TYPE_ENERGY_REGEN_PERC = 78;

	public const int TYPE_TICKETS_REGEN_PERC = 79;

	public const int TYPE_TOKENS_REGEN_PERC = 80;

	public const int TYPE_BADGES_REGEN_PERC = 81;

	public const int TYPE_SHARDS_REGEN_PERC = 82;

	public const int TYPE_FIRE_DAMAGE = 84;

	public const int TYPE_FIRE_RESISTANCE = 85;

	public const int TYPE_WATER_DAMAGE = 86;

	public const int TYPE_WATER_RESISTANCE = 87;

	public const int TYPE_ELECTRIC_DAMAGE = 88;

	public const int TYPE_ELECTRIC_RESISTANCE = 89;

	public const int TYPE_EARTH_DAMAGE = 90;

	public const int TYPE_EARTH_RESISTANCE = 91;

	public const int TYPE_AIR_DAMAGE = 92;

	public const int TYPE_AIR_RESISTANCE = 93;

	public const int TYPE_TRIGGER_DRAIN_SP = 99;

	public const int TYPE_REMOVE_PETS = 100;

	public const int TYPE_INCREASE_COMBUSTION_DAMAGE = 101;

	public const int TYPE_STOP_TURN = 106;

	public const int TYPE_SEALS_MAX = 110;

	public const int TYPE_SEALS_REGEN_PERC = 111;

	public const int TYPE_REMOVE_STACK = 112;

	public const int TYPE_HUNTER_MARK = 119;

	public const int TYPE_ACCURACY = 127;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		[""] = 0,
		["none"] = 0,
		["blockchance"] = 1,
		["blockmult"] = 2,
		["evadechance"] = 3,
		["critchance"] = 4,
		["critmult"] = 5,
		["magicfind"] = 6,
		["expmult"] = 7,
		["goldmult"] = 8,
		["movementspeedmult"] = 9,
		["zonemagicfind"] = 10,
		["zoneexpmult"] = 11,
		["zonegoldmult"] = 12,
		["pvpmagicfind"] = 13,
		["pvpexpmult"] = 14,
		["pvpgoldmult"] = 15,
		["healthdrain"] = 16,
		["powermult"] = 17,
		["staminamult"] = 18,
		["agilitymult"] = 19,
		["familiarchance"] = 20,
		["damagereturn"] = 21,
		["lootrolls"] = 22,
		["pointmult"] = 23,
		["targetchange"] = 24,
		["deflectchance"] = 25,
		["doubleturnchance"] = 26,
		["doublehitchance"] = 27,
		["meterlesschance"] = 28,
		["healgivebonus"] = 29,
		["healreceivebonus"] = 30,
		["damagegivebonus"] = 31,
		["damagereceivebonus"] = 32,
		["metergainbonus"] = 33,
		["meterdrain"] = 34,
		["ricochetchance"] = 35,
		["redirectchance"] = 36,
		["energymax"] = 37,
		["ticketmax"] = 38,
		["shardmax"] = 39,
		["sealsmax"] = 110,
		["tokenmax"] = 40,
		["badgemax"] = 41,
		["guildmembers"] = 42,
		["absorbchance"] = 43,
		["shieldregen"] = 44,
		["healgiveshieldbonus"] = 45,
		["healreceiveshieldbonus"] = 46,
		["healgiveovershieldbonus"] = 47,
		["healreceiveovershieldbonus"] = 48,
		["doublehealgivechance"] = 49,
		["doublehealreceivechance"] = 50,
		["teamenrage"] = 51,
		["quadturnchance"] = 52,
		["fishingmaxdistance"] = 53,
		["fishingbarsize"] = 54,
		["fishingfind"] = 55,
		["brawlmagicfind"] = 56,
		["brawlexpmult"] = 57,
		["brawlgoldmult"] = 58,
		["gauntletmagicfind"] = 59,
		["gauntletexpmult"] = 60,
		["gauntletgoldmult"] = 61,
		["raidmagicfind"] = 62,
		["raidexpmult"] = 63,
		["raidgoldmult"] = 64,
		["randomskillchance"] = 65,
		["healenrage"] = 66,
		["setcount"] = 67,
		["doublerune"] = 68,
		["doubleenchant"] = 69,
		["doublemount"] = 70,
		["damageremainingsplit"] = 71,
		["damagecountreset"] = 72,
		["healcountrreset"] = 73,
		["shieldmult"] = 74,
		["defenseignorechance"] = 76,
		["energyregenperc"] = 78,
		["ticketsregenperc"] = 79,
		["tokensregenperc"] = 80,
		["badgesregenperc"] = 81,
		["shardsregenperc"] = 82,
		["sealsregenperc"] = 111,
		["firedamage"] = 84,
		["fireresistance"] = 85,
		["waterdamage"] = 86,
		["waterresistance"] = 87,
		["electricdamage"] = 88,
		["electricresistance"] = 89,
		["earthdamage"] = 90,
		["earthresistance"] = 91,
		["airdamage"] = 92,
		["airresistance"] = 93,
		["increasecombustiondamage"] = 101,
		["removepets"] = 100,
		["huntermark"] = 119,
		["stopturn"] = 106,
		["accuracy"] = 127
	};

	private List<BattleTriggerRef> _triggers;

	private List<BattleConditionRef> _conditions;

	private List<string> _values;

	private GameModifierData _gameModifierData;

	public int type => GetType(_gameModifierData.type);

	public float value => _gameModifierData.value;

	public string desc
	{
		get
		{
			if (_gameModifierData.desc != null)
			{
				return Language.GetString(_gameModifierData.desc);
			}
			return _gameModifierData.desc;
		}
	}

	public bool tooltip => Util.GetBoolFromStringProperty(_gameModifierData.tooltip, defaultValue: true);

	public bool tooltipFull => Util.GetBoolFromStringProperty(_gameModifierData.tooltipFull, defaultValue: true);

	public List<string> values => _values;

	public List<BattleTriggerRef> triggers => _triggers;

	public List<BattleConditionRef> conditions => _conditions;

	public GameModifier(GameModifierData gameModifierData)
	{
		_gameModifierData = gameModifierData;
		_triggers = new List<BattleTriggerRef>();
		_conditions = new List<BattleConditionRef>();
		_values = new List<string>();
		if (gameModifierData == null)
		{
			return;
		}
		if (gameModifierData.lstTrigger != null && gameModifierData.lstTrigger.Count > 0)
		{
			foreach (BattleTriggerData item in gameModifierData.lstTrigger)
			{
				_triggers.Add(new BattleTriggerRef(item));
			}
		}
		if (gameModifierData.lstCondition != null && gameModifierData.lstCondition.Count > 0)
		{
			foreach (BattleConditionData item2 in gameModifierData.lstCondition)
			{
				int num = BattleConditionRef.getType(item2.type);
				float floatFromStringProperty = Util.GetFloatFromStringProperty(item2.perc, 100f);
				List<GameModifier> gameModifierFromData = GetGameModifierFromData(item2.modifiers, item2.lstModifier);
				_conditions.Add(new BattleConditionRef(item2.id, num, floatFromStringProperty, item2.value, AbilityBook.LookupAbilities(item2.abilities), gameModifierFromData));
			}
		}
		if (gameModifierData.values != null)
		{
			_values = new List<string>(Util.GetStringArrayFromStringProperty(gameModifierData.values));
		}
	}

	public static List<GameModifier> GetGameModifierFromData(GameModifiersData modifiers, List<GameModifierData> lstModifiers)
	{
		List<GameModifier> list = new List<GameModifier>();
		if (lstModifiers != null && lstModifiers.Count > 0)
		{
			foreach (GameModifierData lstModifier in lstModifiers)
			{
				list.Add(new GameModifier(lstModifier));
			}
		}
		if (modifiers != null && modifiers.lstModifier != null && modifiers.lstModifier.Count > 0)
		{
			foreach (GameModifierData item in modifiers.lstModifier)
			{
				list.Add(new GameModifier(item));
			}
			return list;
		}
		return list;
	}

	public static int GetType(string type)
	{
		if (type != null)
		{
			string key = type.ToLowerInvariant();
			if (TYPES.ContainsKey(key))
			{
				return TYPES[key];
			}
		}
		return 0;
	}

	public static string getTypeName(int type)
	{
		return Language.GetString("modifier_" + type + "_name");
	}

	public static string getTypeValueString(int type, float value, bool colored = true, bool identifier = true)
	{
		float decimals = 1000f;
		float num = ((value < 0f) ? (0f - value) : value);
		string text = Util.NumberFormat(Util.roundedValue(num, decimals));
		string text2 = Util.NumberFormat(Util.roundedValue(num * 100f, decimals));
		string text3 = text;
		bool flag = false;
		if (type == 30 || type == 32)
		{
			flag = true;
		}
		string text4 = (((flag ? (0f - value) : value) < 0f) ? "-" : "+");
		switch (type)
		{
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 17:
		case 18:
		case 19:
		case 20:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 45:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 63:
		case 64:
		case 78:
		case 79:
		case 80:
		case 81:
		case 82:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 111:
		case 119:
			text3 = text2;
			break;
		}
		if (colored)
		{
			text3 = "^" + text3 + "^";
		}
		if (identifier)
		{
			text3 = text4 + text3;
		}
		return text3;
	}

	public static string getTypeDescriptionShort(int type, float value, bool colored = true)
	{
		bool identifier = type != 23 && type != 22;
		return Language.GetString("modifier_" + type + "_short_desc", new string[1] { getTypeValueString(type, value, colored, identifier) });
	}

	public static string getTypeDescriptionLong(int type, float value, bool colored = true)
	{
		bool identifier = type == 24;
		return Language.GetString("modifier_" + type + "_long_desc", new string[1] { getTypeValueString(type, value, colored, identifier) });
	}

	public static float getTypeTotal(List<GameModifier> modifiers, int type)
	{
		float num = 0f;
		if (modifiers == null || modifiers.Count <= 0)
		{
			return num;
		}
		foreach (GameModifier modifier in modifiers)
		{
			if (modifier != null && modifier.type == type)
			{
				num += modifier.value;
			}
		}
		return num;
	}

	public static List<GameModifier> getTypeModifiers(List<GameModifier> modifiers, int type)
	{
		List<GameModifier> list = new List<GameModifier>();
		if (modifiers == null || modifiers.Count <= 0)
		{
			return list;
		}
		foreach (GameModifier modifier in modifiers)
		{
			if (modifier != null && modifier.type == type)
			{
				list.Add(modifier);
			}
		}
		return list;
	}

	public static GameModifier getFirstModifier(List<GameModifier> modifiers)
	{
		if (modifiers == null || modifiers.Count <= 0)
		{
			return null;
		}
		foreach (GameModifier modifier in modifiers)
		{
			if (modifier != null)
			{
				return modifier;
			}
		}
		return null;
	}

	public static AbilityRef getFirstAbility(List<GameModifier> modifiers)
	{
		if (modifiers != null)
		{
			foreach (GameModifier modifier in modifiers)
			{
				if (modifier == null || modifier.triggers == null)
				{
					continue;
				}
				foreach (BattleTriggerRef trigger in modifier.triggers)
				{
					if (trigger == null || trigger.abilities == null)
					{
						continue;
					}
					foreach (AbilityRef ability in trigger.abilities)
					{
						if (ability != null)
						{
							return ability;
						}
					}
				}
			}
		}
		return null;
	}

	public static List<BattleTriggerRef> getFirstTriggers(List<GameModifier> modifiers)
	{
		if (modifiers != null)
		{
			foreach (GameModifier modifier in modifiers)
			{
				if (modifier.triggers.Count > 0)
				{
					return modifier.triggers;
				}
			}
		}
		return null;
	}

	public static List<BattleConditionRef> getFirstConditions(List<GameModifier> modifiers)
	{
		foreach (GameModifier modifier in modifiers)
		{
			if (modifier != null && modifier.conditions != null && modifier.conditions.Count > 0)
			{
				return modifier.conditions;
			}
		}
		return null;
	}

	public static List<string> getFirstConditionModifierValues(List<GameModifier> modifiers)
	{
		List<string> list = new List<string>();
		List<BattleConditionRef> firstConditions = getFirstConditions(modifiers);
		if (firstConditions == null || firstConditions.Count <= 0)
		{
			return list;
		}
		foreach (BattleConditionRef item in firstConditions)
		{
			if (item == null || item.modifiers == null)
			{
				continue;
			}
			foreach (GameModifier modifier in item.modifiers)
			{
				if (modifier != null && modifier.type != 0)
				{
					list.Add(getTypeValueString(modifier.type, modifier.value, colored: false, identifier: false));
				}
			}
		}
		return list;
	}

	public static bool listHasType(List<GameModifier> modifiers, int type)
	{
		if (modifiers == null || modifiers.Count <= 0)
		{
			return false;
		}
		foreach (GameModifier modifier in modifiers)
		{
			if (modifier != null && modifier.type == type)
			{
				return true;
			}
		}
		return false;
	}

	public string GetTileDesc(object data = null)
	{
		if (desc != null)
		{
			return Util.ParseModifierString(this, data);
		}
		return getTypeDescriptionShort(type, value);
	}

	public bool Equals(GameModifier other)
	{
		if (other == null)
		{
			return false;
		}
		return _gameModifierData.id.Equals(other._gameModifierData.id);
	}

	public int CompareTo(GameModifier other)
	{
		if (other == null)
		{
			return -1;
		}
		return _gameModifierData.id.CompareTo(other._gameModifierData.id);
	}
}
