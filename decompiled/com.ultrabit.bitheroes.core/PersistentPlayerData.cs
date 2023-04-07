using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.security;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.core;

public class PersistentPlayerData
{
	[Serializable]
	public class ArrayData
	{
		public List<string> dialogsSeen;

		public List<string> boosterBadge;

		public List<IntBoolCombination> _animationTypes;

		public List<IntBoolCombination> _familiarRarities;

		public List<IntBoolCombination> _merchantRarities;

		public List<IntBoolCombination> _equipOnResultsTypes;

		public List<IntIntCombination> _analytics;

		public List<IntBoolCombination> _exchangeFilterRarities;

		public List<IntBoolCombination> _exchangeFilterEquipment;

		public List<IntBoolCombination> _exchangeFilterTypes;

		public List<IntBoolCombination> _inventoryFilterRarities;

		public List<IntBoolCombination> _inventoryFilterEquipment;

		public List<IntBoolCombination> _inventoryFilterTypes;

		public List<IntBoolCombination> _tradeFilterRarities;

		public List<IntBoolCombination> _tradeFilterTypes;

		public List<IntBoolCombination> _augmentsFilterRarities;

		public List<IntBoolCombination> _augmentsFilterAugment;

		public List<IntBoolCombination> _upgradeFilterRarities;

		public List<IntBoolCombination> _upgradeFilterEquipment;

		public List<IntBoolCombination> _reforgeFilterRarities;

		public List<IntBoolCombination> _reforgeFilterEquipment;

		public List<IntBoolCombination> _persuadeFamiliarsGoldRarities;

		public List<IntBoolCombination> _persuadeFamiliarsGemsRarities;

		public List<IntBoolCombination> _seenSchematics;

		public List<IntBoolCombination> _seenRecipes;

		public List<IntBoolCombination> _seenFamiliars;

		public List<IntIntIntCombination> _dungeonLoot;

		public List<string> familiarAssets;

		public Dictionary<int, bool> animationTypes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_animationTypes);
			}
			set
			{
				_animationTypes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> familiarRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_familiarRarities);
			}
			set
			{
				_familiarRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> merchantRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_merchantRarities);
			}
			set
			{
				_merchantRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> equipOnResultsTypes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_equipOnResultsTypes);
			}
			set
			{
				_equipOnResultsTypes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, int> analytics
		{
			get
			{
				return GetIntIntDictionaryFromList(_analytics);
			}
			set
			{
				_analytics = SetListFromIntIntDictionary(value);
			}
		}

		public Dictionary<int, bool> exchangeFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_exchangeFilterRarities);
			}
			set
			{
				_exchangeFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> exchangeFilterEquipment
		{
			get
			{
				return GetIntBoolDictionaryFromList(_exchangeFilterEquipment);
			}
			set
			{
				_exchangeFilterEquipment = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> exchangeFilterTypes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_exchangeFilterTypes);
			}
			set
			{
				_exchangeFilterTypes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> inventoryFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_inventoryFilterRarities);
			}
			set
			{
				_inventoryFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> inventoryFilterEquipment
		{
			get
			{
				return GetIntBoolDictionaryFromList(_inventoryFilterEquipment);
			}
			set
			{
				_inventoryFilterEquipment = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> inventoryFilterTypes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_inventoryFilterTypes);
			}
			set
			{
				_inventoryFilterTypes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> tradeFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_tradeFilterRarities);
			}
			set
			{
				_tradeFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> tradeFilterTypes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_tradeFilterTypes);
			}
			set
			{
				_tradeFilterTypes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> augmentsFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_augmentsFilterRarities);
			}
			set
			{
				_augmentsFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> augmentsFilterAugment
		{
			get
			{
				return GetIntBoolDictionaryFromList(_augmentsFilterAugment);
			}
			set
			{
				_augmentsFilterAugment = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> upgradeFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_upgradeFilterRarities);
			}
			set
			{
				_upgradeFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> upgradeFilterEquipment
		{
			get
			{
				return GetIntBoolDictionaryFromList(_upgradeFilterEquipment);
			}
			set
			{
				_upgradeFilterEquipment = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> reforgeFilterRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_reforgeFilterRarities);
			}
			set
			{
				_reforgeFilterRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> reforgeFilterEquipment
		{
			get
			{
				return GetIntBoolDictionaryFromList(_reforgeFilterEquipment);
			}
			set
			{
				_reforgeFilterEquipment = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> persuadeFamiliarsGoldRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_persuadeFamiliarsGoldRarities);
			}
			set
			{
				_persuadeFamiliarsGoldRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> persuadeFamiliarsGemsRarities
		{
			get
			{
				return GetIntBoolDictionaryFromList(_persuadeFamiliarsGemsRarities);
			}
			set
			{
				_persuadeFamiliarsGemsRarities = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> seenSchematics
		{
			get
			{
				return GetIntBoolDictionaryFromList(_seenSchematics);
			}
			set
			{
				_seenSchematics = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> seenRecipes
		{
			get
			{
				return GetIntBoolDictionaryFromList(_seenRecipes);
			}
			set
			{
				_seenRecipes = SetListFromIntBoolDictionary(value);
			}
		}

		public Dictionary<int, bool> seenFamiliars
		{
			get
			{
				return GetIntBoolDictionaryFromList(_seenFamiliars);
			}
			set
			{
				_seenFamiliars = SetListFromIntBoolDictionary(value);
			}
		}

		public List<IntIntIntCombination> dungeonLoot
		{
			get
			{
				return _dungeonLoot;
			}
			set
			{
				_dungeonLoot = value;
			}
		}

		private Dictionary<int, int> GetIntIntDictionaryFromList(List<IntIntCombination> combination)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (combination == null || combination.Count < 0)
			{
				return dictionary;
			}
			foreach (IntIntCombination item in combination)
			{
				dictionary.Add(item.id, item.value);
			}
			return dictionary;
		}

		private Dictionary<int, bool> GetIntBoolDictionaryFromList(List<IntBoolCombination> combination)
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			if (combination == null || combination.Count < 0)
			{
				return dictionary;
			}
			foreach (IntBoolCombination item in combination)
			{
				dictionary.Add(item.id, item.value);
			}
			return dictionary;
		}

		private List<IntBoolCombination> SetListFromIntBoolDictionary(Dictionary<int, bool> values)
		{
			List<IntBoolCombination> list = new List<IntBoolCombination>();
			foreach (KeyValuePair<int, bool> value in values)
			{
				list.Add(new IntBoolCombination(value.Key, value.Value));
			}
			return list;
		}

		private List<IntIntCombination> SetListFromIntIntDictionary(Dictionary<int, int> values)
		{
			List<IntIntCombination> list = new List<IntIntCombination>();
			foreach (KeyValuePair<int, int> value in values)
			{
				list.Add(new IntIntCombination(value.Key, value.Value));
			}
			return list;
		}
	}

	[Serializable]
	public class IntBoolCombination
	{
		public int id;

		public bool value;

		public IntBoolCombination(int id, bool value)
		{
			this.id = id;
			this.value = value;
		}
	}

	[Serializable]
	public class IntIntCombination
	{
		public int id;

		public int value;

		public IntIntCombination(int id, int value)
		{
			this.id = id;
			this.value = value;
		}
	}

	[Serializable]
	public class IntIntIntCombination
	{
		public int id;

		public int subId;

		public int value;

		public IntIntIntCombination(int id, int subId, int value)
		{
			this.id = id;
			this.subId = subId;
			this.value = value;
		}
	}

	public const int DEFAULT_INT_VALUE = -1;

	public const string DEFAULT_STRING_VALUE = "";

	public const float DEFAULT_FLOAT_VALUE = -1f;

	public const bool DEFAULT_BOOL_VALUE = false;

	public const int ANIMATION_TYPE_FUSION = 0;

	public const int ANIMATION_TYPE_VICTORY = 1;

	public const int ANIMATION_TYPE_BATTLE = 2;

	public const int EQUIP_ON_RESULTS_DUNGEONS = 3;

	public const int EQUIP_ON_RESULTS_PVP = 4;

	public const int EQUIP_ON_RESULTS_RAID = 5;

	public const int EQUIP_ON_RESULTS_RIFT = 6;

	public const int EQUIP_ON_RESULTS_GAUNTLET = 7;

	public const int EQUIP_ON_RESULTS_GVG = 8;

	public const int EQUIP_ON_RESULTS_INVASION = 9;

	public const int EQUIP_ON_RESULTS_BRAWL = 10;

	public const int EQUIP_ON_RESULTS_GVE = 11;

	private const string LOCATION = "pixelquest/local";

	private const string UID = "UID";

	private const string PLATFORM_SPECIFIC_USER_ID = "PLATFORM_SPECIFIC_USER_ID";

	private const string SYSTEM = "SYSTEM";

	private const string EMAIL = "EMAIL";

	private const string PASSWORD = "PASSWORD";

	private const string LANGUAGE = "LANGUAGE";

	private const string ADMIN_PASSWORD = "ADMIN_PASSWORD";

	private const string MUSIC_VOLUME = "MUSIC_VOLUME";

	private const string SOUND_VOLUME = "SOUND_VOLUME";

	private const string BATTLE_SPEED = "BATTLE_SPEED";

	private const string BATTLE_TEXT = "BATTLE_TEXT";

	private const string BATTLE_BAR_OVERLAY = "BATTLE_BAR_OVERLAY";

	private const string GOOGLE_AUTO_SIGN_IN = "GOOGLE_AUTO_SIGN_IN";

	private const string DROPDOWN_INDEX_INVENTORY = "DROPDOWN_INDEX_INVENTORY";

	private const string SERVER_ID = "SERVER_ID";

	private const string NEWS_VERSION = "NEWS_VERSION";

	private const string LOGIN_PLATFORM = "LOGIN_PLATFORM";

	private const string NOTIFICATIONS_DISABLED = "NOTIFICATIONS_DISABLED";

	private const string NOTIFICATIONS_FRIEND = "NOTIFICATIONS_FRIEND";

	private const string NOTIFICATIONS_GUILD = "NOTIFICATIONS_GUILD";

	private const string NOTIFICATIONS_FAMILIARS = "NOTIFICATIONS_FAMILIARS";

	private const string NOTIFICATIONS_FUSIONS = "NOTIFICATIONS_FUSIONS";

	private const string NOTIFICATIONS_CRAFT = "NOTIFICATIONS_CRAFT";

	private const string NOTIFICATIONS_OTHER = "NOTIFICATIONS_OTHER";

	private const string APP_NOTIFICATIONS_DISABLED = "APP_NOTIFICATIONS_DISABLED";

	private const string FILTER_DISABLED = "FILTER_DISABLED";

	private const string CHAT_AGE_VERIFIED = "CHAT_AGE_VERIFIED";

	private const string CHAT_TOS_VERIFIED = "CHAT_TOS_VERIFIED";

	private const string ANIMATIONS = "ANIMATIONS";

	private const string DECLINE_FAMILIAR_DUPES = "DECLINE_FAMILIAR_DUPES";

	private const string DECLINE_FAMILIAR_RARITIES = "DECLINE_FAMILIAR_RARITIES";

	private const string EULA_VERIFIED = "EULA_VERIFIED";

	private const string AUTOPERSUADE_GOLD = "AUTOPERSUADE_GOLD";

	private const string AUTOPERSUADE_GOLD_RARITIES = "AUTOPERSUADE_GOLD_RARITIES";

	private const string AUTOPERSUADE_GEMS = "AUTOPERSUADE_GEMS";

	private const string AUTOPERSUADE_GEMS_RARITIES = "AUTOPERSUADE_GEMS_RARITIES";

	private const string DECLINE_MERCHANTS = "DECLINE_MERCHANTS";

	private const string DECLINE_MERCHANTS_RARITIES = "DECLINE_MERCHANTS_RARITIES";

	private const string DECLINE_TREASURES = "DECLINE_TREASURES";

	private const string IGNORE_SHRINES = "IGNORE_SHRINES";

	private const string IGNORE_BOSS = "IGNORE_BOSS";

	private const string BRAWL_REQUESTS = "BRAWL_REQUESTS";

	private const string BRAWL_REQUESTS_FRIEND = "BRAWL_REQUESTS_FRIEND";

	private const string BRAWL_REQUESTS_GUILD = "BRAWL_REQUESTS_GUILD";

	private const string BRAWL_REQUESTS_OTHER = "BRAWL_REQUESTS_OTHER";

	private const string AUTO_PILOT_DEATH_DISABLE = "AUTO_PILOT_DEATH_DISABLE";

	private const string FULLSCREEN = "FULLSCREEN";

	private const string RESOLUTION_X = "RESOLUTION_X";

	private const string RESOLUTION_Y = "RESOLUTION_Y";

	private const string LOGOS_DISABLED = "LOGOS_DISABLED";

	private const string AUTO_ENRAGE = "AUTO_ENRAGE";

	private const string ADS_DISABLED = "ADS_DISABLED";

	private const string REDUCED_EFFECTS = "REDUCED_EFFECTS";

	private const string FAMILIAR_ASSETS = "FAMILIAR_ASSETS";

	private const string EXCHANGE_FILTER_RARITIES = "EXCHANGE_FILTER_RARITIES";

	private const string EXCHANGE_FILTER_EQUIPMENT = "EXCHANGE_FILTER_EQUIPMENT";

	private const string EXCHANGE_FILTER_ITEMS = "EXCHANGE_FILTER_ITEMS";

	private const string DIALOGS_SEEN = "DIALOGS_SEEN";

	private const string RATINGS_SEEN = "RATINGS_SEEN";

	private const string RIFT_EVENT_DIFFICULTY = "RIFT_EVENT_DIFFICULTY";

	private const string GAUNTLET_EVENT_DIFFICULTY = "GAUNTLET_EVENT_DIFFICULTY";

	private const string INVASION_EVENT_DIFFICULTY = "INVASION_EVENT_DIFFICULTY";

	private const string GVE_EVENT_DIFFICULTY = "GVE_EVENT_DIFFICULTY";

	private const string PVP_EVENT_BONUS = "PVP_EVENT_BONUS";

	private const string PVP_EVENT_SORT = "PVP_EVENT_SORT";

	private const string RIFT_EVENT_BONUS = "RIFT_EVENT_BONUS";

	private const string GAUNTLET_EVENT_BONUS = "GAUNTLET_EVENT_BONUS";

	private const string INVASION_EVENT_BONUS = "INVASION_EVENT_BONUS";

	private const string GVG_EVENT_BONUS = "GVG_EVENT_BONUS";

	private const string GVG_EVENT_SORT = "GVG_EVENT_SORT";

	private const string GVE_EVENT_BONUS = "GVE_EVENT_BONUS";

	private const string RAID_SELECTED = "RAID_SELECTED";

	private const string BRAWL_SELECTED = "BRAWL_SELECTED";

	private const string BRAWL_TIER_SELECTED = "BRAWL_TIER_SELECTED";

	private const string BRAWL_DIFFICULTY_SELECTED = "BRAWL_DIFFICULTY_SELECTED";

	private const string BRAWL_PUBLIC_SELECTED = "BRAWL_PUBLIC_SELECTED";

	private const string BRAWL_FILTER = "BRAWL_FILTER";

	private const string BRAWL_TIER_FILTER = "BRAWL_TIER_FILTER";

	private const string BRAWL_DIFFICULTY_FILTER = "BRAWL_DIFFICULTY_FILTER";

	private const string FREIND_REQUEST_SORT = "FREIND_REQUEST_SORT";

	private const string GUILD_APPLICANT_SORT = "GUILD_APPLICANT_SORT";

	private const string ENCHANT_SELECT_SORT = "ENCHANT_SELECT_SORT";

	private const string MOUNT_SELECT_SORT = "MOUNT_SELECT_SORT";

	private const string ANALYTICS = "ANALYTICS";

	private const string CHARACTER_ID = "CHARACTER_ID";

	private const string CHARACTER_GUILD_ID = "CHARACTER_GUILD_ID";

	private const string CHARACTER_GOLD = "CHARACTER_GOLD";

	private const string CHARACTER_CREDITS = "CHARACTER_CREDITS";

	private const string CHARACTER_LEVEL = "CHARACTER_LEVEL";

	private const string CHARACTER_ENERGY = "CHARACTER_ENERGY";

	private const string CHARACTER_ENERGY_MAX = "CHARACTER_ENERGY_MAX";

	private const string CHARACTER_TICKETS = "CHARACTER_TICKETS";

	private const string CHARACTER_TICKETS_MAX = "CHARACTER_TICKETS_MAX";

	private const string CHARACTER_TOTAL_STATS = "CHARACTER_TOTAL_STATS";

	private const string CHARACTER_TOTAL_POWER = "CHARACTER_TOTAL_POWER";

	private const string CHARACTER_TOTAL_STAMINA = "CHARACTER_TOTAL_STAMINA";

	private const string CHARACTER_TOTAL_AGILITY = "CHARACTER_TOTAL_AGILITY";

	private const string CHARACTER_NAME = "CHARACTER_NAME";

	private const string CHARACTER_HEROTAG = "CHARACTER_HEROTAG";

	private const string CHARACTER_HEROTYPE = "CHARACTER_HEROTYPE";

	private const string FISHING_ROD = "FISHING_ROD";

	private const string FISHING_BOBBER = "FISHING_BOBBER";

	private const string FISHING_BAIT = "FISHING_BAIT";

	private const string ARRAY_DATA = "ARRAY_DATA";

	private const string INVENTORY_FILTER_TYPE = "INVENTORY_FILTER_TYPE";

	private const string INVENTORY_ADVANCED_FILTER = "INVENTORY_ADVANCED_FILTER";

	private const string EXCHANGE_FILTER_TYPE = "EXCHANGE_FILTER_TYPE";

	private const string EXCHANGE_ADVANCED_FILTER = "EXCHANGE_ADVANCED_FILTER";

	private const string TRADE_FILTER_TYPE = "TRADE_FILTER_TYPE";

	private const string TRADE_ADVANCED_FILTER = "TRADE_ADVANCED_FILTER";

	private const string AUGMENT_FILTER_SUBTYPE = "AUGMENT_FILTER_SUBTYPE";

	private const string AUGMENT_ADVANCED_FILTER = "AUGMENT_ADVANCED_FILTER";

	private const string UPGRADE_FILTER_SUBTYPE = "UPGRADE_FILTER_SUBTYPE";

	private const string UPGRADE_ADVANCED_FILTER = "UPGRADE_ADVANCED_FILTER";

	private const string REFORGE_FILTER_SUBTYPE = "REFORGE_FILTER_SUBTYPE";

	private const string REFORGE_ADVANCED_FILTER = "REFORGE_ADVANCED_FILTER";

	private const string HIDE_LEVEL_CHAT = "HIDE_LEVEL_CHAT";

	private static string VIEW_VIPGOR_PURCHASE_SUCCESS = "VIEW_VIPGOR_PURCHASE_SUCCESS";

	private const string PLAYER_ID = "PLAYER_ID";

	private const string BOOSTER_BADGE_NEWS = "BOOSTER_BADGE_NEWS";

	private const string BOOSTER_BADGE_NEWS_NOTIFICATION = "BOOSTER_BADGE_NEWS_NOTIFICATION";

	private const string SHARED_OBJECTS_LOADED = "SHARED_OBJECTS_LOADED";

	private const string SEEN_SCHEMATICS = "SEEN_SCHEMATICS";

	private const string ZONESBTN_ANIMATION = "ZONESBTN_ANIMATION";

	private const string DEFEAT_ADVICES = "DEFEAT_ADVICES";

	private const string EQUIP_ON_RESULTS = "EQUIP_ON_RESULTS";

	private const string DUNGEON_LOOT_TYPE = "DUNGEON_LOOT_TYPE";

	private const string DUNGEON_LOOT_ID = "DUNGEON_LOOT_ID";

	private const string SOURCE_FROM_DUNGEON = "SOURCE_FROM_DUNGEON";

	private const string ADID = "ADID";

	public string email
	{
		get
		{
			return RetrieveString("EMAIL");
		}
		set
		{
			Store("EMAIL", value);
		}
	}

	public string password
	{
		get
		{
			return RetrieveString("PASSWORD");
		}
		set
		{
			Store("PASSWORD", value);
		}
	}

	public bool notificationsDisabled
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_DISABLED");
		}
		set
		{
			Store("NOTIFICATIONS_DISABLED", value);
		}
	}

	public bool notificationsFriend
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_FRIEND", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_FRIEND", value);
		}
	}

	public bool notificationsGuild
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_GUILD", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_GUILD", value);
		}
	}

	public bool notificationsFamiliars
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_FAMILIARS", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_FAMILIARS", value);
		}
	}

	public bool notificationsFusions
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_FUSIONS", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_FUSIONS", value);
		}
	}

	public bool notificationsCraft
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_CRAFT", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_CRAFT", value);
		}
	}

	public bool notificationsOther
	{
		get
		{
			return RetrieveBool("NOTIFICATIONS_OTHER", defaultValue: true);
		}
		set
		{
			Store("NOTIFICATIONS_OTHER", value);
		}
	}

	public bool brawlRequests
	{
		get
		{
			return RetrieveBool("BRAWL_REQUESTS", defaultValue: true);
		}
		set
		{
			Store("BRAWL_REQUESTS", value);
		}
	}

	public bool brawlRequestsGuild
	{
		get
		{
			return RetrieveBool("BRAWL_REQUESTS_GUILD", defaultValue: true);
		}
		set
		{
			Store("BRAWL_REQUESTS_GUILD", value);
		}
	}

	public bool brawlRequestsFriend
	{
		get
		{
			return RetrieveBool("BRAWL_REQUESTS_FRIEND", defaultValue: true);
		}
		set
		{
			Store("BRAWL_REQUESTS_FRIEND", value);
		}
	}

	public bool brawlRequestsOther
	{
		get
		{
			return RetrieveBool("BRAWL_REQUESTS_OTHER", defaultValue: true);
		}
		set
		{
			Store("BRAWL_REQUESTS_OTHER", value);
		}
	}

	public bool autoEnrage
	{
		get
		{
			return RetrieveBool("AUTO_ENRAGE", defaultValue: true);
		}
		set
		{
			Store("AUTO_ENRAGE", value);
		}
	}

	public bool adsDisabled
	{
		get
		{
			return RetrieveBool("ADS_DISABLED");
		}
		set
		{
			Store("ADS_DISABLED", value);
		}
	}

	public bool reducedEffects
	{
		get
		{
			return RetrieveBool("REDUCED_EFFECTS");
		}
		set
		{
			Store("REDUCED_EFFECTS", value);
		}
	}

	public bool battleText
	{
		get
		{
			return RetrieveBool("BATTLE_TEXT", defaultValue: true);
		}
		set
		{
			Store("BATTLE_TEXT", value);
		}
	}

	public bool battleBarOverlay
	{
		get
		{
			return RetrieveBool("BATTLE_BAR_OVERLAY");
		}
		set
		{
			Store("BATTLE_BAR_OVERLAY", value);
		}
	}

	public bool animations
	{
		get
		{
			return RetrieveBool("ANIMATIONS", defaultValue: true);
		}
		set
		{
			Store("ANIMATIONS", value);
		}
	}

	public bool declineFamiliarDupes
	{
		get
		{
			return RetrieveBool("DECLINE_FAMILIAR_DUPES");
		}
		set
		{
			Store("DECLINE_FAMILIAR_DUPES", value);
		}
	}

	public bool declineMerchants
	{
		get
		{
			return RetrieveBool("DECLINE_MERCHANTS");
		}
		set
		{
			Store("DECLINE_MERCHANTS", value);
		}
	}

	public bool declineTreasures
	{
		get
		{
			return RetrieveBool("DECLINE_TREASURES");
		}
		set
		{
			Store("DECLINE_TREASURES", value);
		}
	}

	public bool fullscreen
	{
		get
		{
			return RetrieveBool("FULLSCREEN");
		}
		set
		{
			Store("FULLSCREEN", value);
		}
	}

	public string uid
	{
		get
		{
			string text = RetrieveString("UID");
			if (string.IsNullOrEmpty(text))
			{
				text = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + "-" + Util.RandomString(20f);
				Store("UID", text);
			}
			return text;
		}
		set
		{
			Store("UID", value);
		}
	}

	public string adid
	{
		get
		{
			string text = RetrieveString("ADID");
			if (string.IsNullOrEmpty(text))
			{
				text = "ad_" + (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + "_" + Guid.NewGuid().ToString();
				text = text.Substring(0, Mathf.Min(text.Length, 64));
				Store("ADID", text);
			}
			return text;
		}
		set
		{
			Store("ADID", value);
		}
	}

	public string platformSpecificUserID
	{
		get
		{
			string text = RetrieveString("PLATFORM_SPECIFIC_USER_ID");
			if (text == null || text.Equals(""))
			{
				return null;
			}
			return text;
		}
		set
		{
			Store("PLATFORM_SPECIFIC_USER_ID", value);
		}
	}

	public string system
	{
		get
		{
			return RetrieveString("SYSTEM", "Windows");
		}
		set
		{
			Store("SYSTEM", value);
		}
	}

	public string language
	{
		get
		{
			return RetrieveString("LANGUAGE", "en");
		}
		set
		{
			Store("LANGUAGE", value);
		}
	}

	public string adminPassword
	{
		get
		{
			return RetrieveString("ADMIN_PASSWORD");
		}
		set
		{
			Store("ADMIN_PASSWORD", value);
		}
	}

	public float musicVolume
	{
		get
		{
			return RetrieveFloat("MUSIC_VOLUME", 0.5f);
		}
		set
		{
			Store("MUSIC_VOLUME", value);
		}
	}

	public float soundVolume
	{
		get
		{
			return RetrieveFloat("SOUND_VOLUME", 0.5f);
		}
		set
		{
			Store("SOUND_VOLUME", value);
		}
	}

	public int boosterBadgeNotification
	{
		get
		{
			return RetrieveInt("BOOSTER_BADGE_NEWS_NOTIFICATION");
		}
		set
		{
			Store("BOOSTER_BADGE_NEWS_NOTIFICATION", value);
		}
	}

	public bool googleAutoSignIn
	{
		get
		{
			return RetrieveBool("GOOGLE_AUTO_SIGN_IN");
		}
		set
		{
			Store("GOOGLE_AUTO_SIGN_IN", value);
		}
	}

	public int serverID
	{
		get
		{
			return RetrieveInt("SERVER_ID");
		}
		set
		{
			Store("SERVER_ID", value);
		}
	}

	public string newsVersion
	{
		get
		{
			return RetrieveString("NEWS_VERSION");
		}
		set
		{
			Store("NEWS_VERSION", value);
		}
	}

	public int characterID
	{
		get
		{
			return RetrieveInt("CHARACTER_ID", 0);
		}
		set
		{
			Store("CHARACTER_ID", value);
		}
	}

	public int playerID
	{
		get
		{
			return RetrieveInt("PLAYER_ID");
		}
		set
		{
			Store("PLAYER_ID", value);
		}
	}

	public int characterGuildID
	{
		get
		{
			return RetrieveInt("CHARACTER_GUILD_ID");
		}
		set
		{
			Store("CHARACTER_GUILD_ID", value);
		}
	}

	public int characterGold
	{
		get
		{
			return RetrieveInt("CHARACTER_GOLD");
		}
		set
		{
			Store("CHARACTER_GOLD", value);
		}
	}

	public int characterCredits
	{
		get
		{
			return RetrieveInt("CHARACTER_CREDITS");
		}
		set
		{
			Store("CHARACTER_CREDITS", value);
		}
	}

	public int characterLevel
	{
		get
		{
			return RetrieveInt("CHARACTER_LEVEL");
		}
		set
		{
			Store("CHARACTER_LEVEL", value);
		}
	}

	public int characterEnergy
	{
		get
		{
			return RetrieveInt("CHARACTER_ENERGY");
		}
		set
		{
			Store("CHARACTER_ENERGY", value);
		}
	}

	public int characterEnergyMax
	{
		get
		{
			return RetrieveInt("CHARACTER_ENERGY_MAX");
		}
		set
		{
			Store("CHARACTER_ENERGY_MAX", value);
		}
	}

	public int characterTickets
	{
		get
		{
			return RetrieveInt("CHARACTER_TICKETS");
		}
		set
		{
			Store("CHARACTER_TICKETS", value);
		}
	}

	public int characterTicketsMax
	{
		get
		{
			return RetrieveInt("CHARACTER_TICKETS_MAX");
		}
		set
		{
			Store("CHARACTER_TICKETS_MAX", value);
		}
	}

	public int characterTotalStats
	{
		get
		{
			return RetrieveInt("CHARACTER_TOTAL_STATS");
		}
		set
		{
			Store("CHARACTER_TOTAL_STATS", value);
		}
	}

	public int characterTotalPower
	{
		get
		{
			return RetrieveInt("CHARACTER_TOTAL_POWER");
		}
		set
		{
			Store("CHARACTER_TOTAL_POWER", value);
		}
	}

	public int characterTotalStamina
	{
		get
		{
			return RetrieveInt("CHARACTER_TOTAL_STAMINA");
		}
		set
		{
			Store("CHARACTER_TOTAL_STAMINA", value);
		}
	}

	public int characterTotalAgility
	{
		get
		{
			return RetrieveInt("CHARACTER_TOTAL_AGILITY");
		}
		set
		{
			Store("CHARACTER_TOTAL_AGILITY", value);
		}
	}

	public string characterName
	{
		get
		{
			return RetrieveString("CHARACTER_NAME");
		}
		set
		{
			Store("CHARACTER_NAME", value);
		}
	}

	public string herotag
	{
		get
		{
			return RetrieveString("CHARACTER_HEROTAG");
		}
		set
		{
			Store("CHARACTER_HEROTAG", value);
		}
	}

	public string heroType
	{
		get
		{
			return RetrieveString("CHARACTER_HEROTYPE");
		}
		set
		{
			Store("CHARACTER_HEROTYPE", value);
		}
	}

	public int loginPlatform
	{
		get
		{
			int num = RetrieveInt("LOGIN_PLATFORM");
			if (num == -1)
			{
				return AppInfo.platform;
			}
			return num;
		}
		set
		{
			Store("LOGIN_PLATFORM", value);
		}
	}

	public bool appNotificationsDisabled
	{
		get
		{
			return RetrieveBool("APP_NOTIFICATIONS_DISABLED");
		}
		set
		{
			if (!value)
			{
				AppInfo.doCancelAllLocalNotification();
			}
			Store("APP_NOTIFICATIONS_DISABLED", value);
		}
	}

	public bool filterDisabled
	{
		get
		{
			return RetrieveBool("FILTER_DISABLED");
		}
		set
		{
			Store("FILTER_DISABLED", value);
		}
	}

	public bool chatAgeVerified
	{
		get
		{
			return RetrieveBool("CHAT_AGE_VERIFIED");
		}
		set
		{
			Store("CHAT_AGE_VERIFIED", value);
		}
	}

	public bool chatTosVerified
	{
		get
		{
			return RetrieveBool("CHAT_TOS_VERIFIED");
		}
		set
		{
			Store("CHAT_TOS_VERIFIED", value);
		}
	}

	public bool ignoreShrines
	{
		get
		{
			return RetrieveBool("IGNORE_SHRINES");
		}
		set
		{
			Store("IGNORE_SHRINES", value);
		}
	}

	public bool ignoreBoss
	{
		get
		{
			return RetrieveBool("IGNORE_BOSS");
		}
		set
		{
			Store("IGNORE_BOSS", value);
		}
	}

	public bool autoPilotDeathDisable
	{
		get
		{
			return RetrieveBool("AUTO_PILOT_DEATH_DISABLE");
		}
		set
		{
			Store("AUTO_PILOT_DEATH_DISABLE", value);
		}
	}

	public int resolutionX
	{
		get
		{
			return RetrieveInt("RESOLUTION_X");
		}
		set
		{
			Store("RESOLUTION_X", value);
		}
	}

	public int resolutionY
	{
		get
		{
			return RetrieveInt("RESOLUTION_Y");
		}
		set
		{
			Store("RESOLUTION_Y", value);
		}
	}

	public bool logosDisabled
	{
		get
		{
			return RetrieveBool("LOGOS_DISABLED");
		}
		set
		{
			Store("LOGOS_DISABLED", value);
		}
	}

	public bool ratingsSeen
	{
		get
		{
			return RetrieveBool("RATINGS_SEEN");
		}
		set
		{
			Store("RATINGS_SEEN", value);
		}
	}

	public bool sharedObjectsObtained
	{
		get
		{
			return RetrieveBool("SHARED_OBJECTS_LOADED");
		}
		set
		{
			Store("SHARED_OBJECTS_LOADED", value);
		}
	}

	public bool zonesBtnAnimation
	{
		get
		{
			return RetrieveBool("ZONESBTN_ANIMATION");
		}
		set
		{
			Store("ZONESBTN_ANIMATION", value);
		}
	}

	public bool eulaVerified
	{
		get
		{
			return RetrieveBool("EULA_VERIFIED");
		}
		set
		{
			Store("EULA_VERIFIED", value);
		}
	}

	public List<string> familiarAssets
	{
		get
		{
			string arrayDataString = GetArrayDataString(0);
			return RetrieveArrayData(arrayDataString).familiarAssets;
		}
		set
		{
			string arrayDataString = GetArrayDataString(0);
			ArrayData arrayData = RetrieveArrayData(arrayDataString);
			arrayData.familiarAssets = value;
			Store(arrayDataString, arrayData);
		}
	}

	public bool defeatAdvices
	{
		get
		{
			return RetrieveBool("DEFEAT_ADVICES", defaultValue: true);
		}
		set
		{
			Store("DEFEAT_ADVICES", value);
		}
	}

	public bool equipOnResults
	{
		get
		{
			return RetrieveBool("EQUIP_ON_RESULTS", defaultValue: true);
		}
		set
		{
			Store("EQUIP_ON_RESULTS", value);
		}
	}

	public bool hideLevelChat
	{
		get
		{
			return RetrieveBool("HIDE_LEVEL_CHAT", defaultValue: true);
		}
		set
		{
			Store("HIDE_LEVEL_CHAT", value);
		}
	}

	public bool viewVipgorPurchaseSuccess
	{
		get
		{
			return RetrieveBool(VIEW_VIPGOR_PURCHASE_SUCCESS);
		}
		set
		{
			Store(VIEW_VIPGOR_PURCHASE_SUCCESS, value);
		}
	}

	public bool autopersuadeFamiliarsGold
	{
		get
		{
			return RetrieveBool("AUTOPERSUADE_GOLD");
		}
		set
		{
			Store("AUTOPERSUADE_GOLD", value);
		}
	}

	public bool autopersuadeFamiliarsGems
	{
		get
		{
			return RetrieveBool("AUTOPERSUADE_GEMS");
		}
		set
		{
			Store("AUTOPERSUADE_GEMS", value);
		}
	}

	public string xmlHash
	{
		get
		{
			return RetrieveString("xmlhash");
		}
		set
		{
			Store("xmlhash", value);
		}
	}

	public bool isComingFromDungeon
	{
		get
		{
			return RetrieveBool("SOURCE_FROM_DUNGEON");
		}
		set
		{
			Store("SOURCE_FROM_DUNGEON", value);
		}
	}

	private void Store(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		PlayerPrefs.Save();
	}

	private string RetrieveString(string key, string defaultvalue = "")
	{
		return PlayerPrefs.GetString(key, defaultvalue);
	}

	private void Store(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		PlayerPrefs.Save();
	}

	private int RetrieveInt(string key, int defaultValue = -1)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	private void Store(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? 1 : 0);
		PlayerPrefs.Save();
	}

	private bool RetrieveBool(string key, bool defaultValue = false)
	{
		int defaultValue2 = (defaultValue ? 1 : 0);
		return RetrieveInt(key, defaultValue2) == 1;
	}

	private void Store(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		PlayerPrefs.Save();
	}

	private float RetrieveFloat(string key, float defaultValue = -1f)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	private void Store(string key, ArrayData arrayData)
	{
		string raw = JsonUtility.ToJson(arrayData);
		Store(key, Encode(raw));
	}

	private ArrayData RetrieveArrayData(string key)
	{
		string encoded = RetrieveString(key);
		ArrayData arrayData = JsonUtility.FromJson<ArrayData>(Decode(encoded));
		if (arrayData == null)
		{
			return new ArrayData();
		}
		return arrayData;
	}

	private string Encode(string raw)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
	}

	private string Decode(string encoded)
	{
		byte[] bytes = Convert.FromBase64String(encoded);
		return Encoding.UTF8.GetString(bytes);
	}

	public void SetBattleSpeed(int charID, int id)
	{
		Store(GetBattleSpeedString(charID), id);
	}

	public int GetBattleSpeed(int charID)
	{
		return RetrieveInt(GetBattleSpeedString(charID));
	}

	public string GetBattleSpeedString(int charID)
	{
		return "BATTLE_SPEED_" + charID;
	}

	public void SetDropdownIndexInventory(int index)
	{
		SetDropdownIndexInventory(GameData.instance.PROJECT.character.id, index);
	}

	public void SetDropdownIndexInventory(int charID, int index)
	{
		Store(GetDropdownIndexInventoryString(charID), index);
	}

	public int GetDropdownIndexInventory()
	{
		return GetDropdownIndexInventory(GameData.instance.PROJECT.character.id);
	}

	public int GetDropdownIndexInventory(int charID)
	{
		return RetrieveInt(GetDropdownIndexInventoryString(charID), 0);
	}

	public string GetDropdownIndexInventoryString(int charID)
	{
		return "DROPDOWN_INDEX_INVENTORY_" + charID;
	}

	private string GetArrayDataString(int charID)
	{
		return "ARRAY_DATA_" + charID;
	}

	public void SetAnimationsTypes(int charID, Dictionary<int, bool> animationTypes)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.animationTypes = animationTypes;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetAnimationsTypes(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).animationTypes;
	}

	public bool GetAnimationsType(int charID, int type, Dictionary<int, bool> animationTypes)
	{
		if (animationTypes == null || !animationTypes.ContainsKey(type))
		{
			return true;
		}
		return animationTypes[type];
	}

	public void SetEquipOnResultsTypes(int charID, Dictionary<int, bool> equipOnResultsTypes)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.equipOnResultsTypes = equipOnResultsTypes;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetEquipOnResultsTypes(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).equipOnResultsTypes;
	}

	public bool GetEquipOnResultsTypes(int charID, int battleType)
	{
		if (!equipOnResults)
		{
			return false;
		}
		Dictionary<int, bool> equipOnResultsTypes = GetEquipOnResultsTypes(charID);
		int key = 0;
		switch (battleType)
		{
		case 1:
			key = 3;
			break;
		case 2:
		case 3:
			key = 3;
			break;
		case 4:
			key = 5;
			break;
		case 5:
			key = 6;
			break;
		case 6:
			key = 7;
			break;
		case 7:
			key = 8;
			break;
		case 8:
			key = 9;
			break;
		case 9:
			key = 10;
			break;
		case 11:
			key = 11;
			break;
		}
		if (equipOnResultsTypes == null || !equipOnResultsTypes.ContainsKey(key))
		{
			return true;
		}
		return equipOnResultsTypes[key];
	}

	public bool GetEquipOnResultsTypes(int charID, int type, Dictionary<int, bool> equipOnResultsTypes)
	{
		if (equipOnResultsTypes == null || !equipOnResultsTypes.ContainsKey(type))
		{
			return true;
		}
		return equipOnResultsTypes[type];
	}

	public void SetDeclineFamiliarRarities(int charID, Dictionary<int, bool> familiarRarities)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.familiarRarities = familiarRarities;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetDeclineFamiliarRarities(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).familiarRarities;
	}

	public bool GetDeclineFamiliarRarity(int charID, RarityRef rarity, Dictionary<int, bool> familiarRarities)
	{
		if (familiarRarities == null || rarity == null || rarity.id < 0 || !familiarRarities.ContainsKey(rarity.id))
		{
			return true;
		}
		return familiarRarities[rarity.id];
	}

	public void SetDeclineMerchantsRarities(int charID, Dictionary<int, bool> merchantRarities)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.merchantRarities = merchantRarities;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetDeclineMerchantsRarities(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).merchantRarities;
	}

	public bool GetDeclineMerchantsRarity(int charID, RarityRef rarity, Dictionary<int, bool> merchantRarities)
	{
		if (merchantRarities == null || rarity == null || rarity.id < 0 || !merchantRarities.ContainsKey(rarity.id))
		{
			return true;
		}
		return merchantRarities[rarity.id];
	}

	public void SetAnalytics(int charID, Dictionary<int, int> data)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.analytics = data;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, int> GetAnalytics(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).analytics;
	}

	public bool GetIsInventoryFiltered(int charID, ItemRef item, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null)
	{
		int num = item.itemType;
		switch (item.itemType)
		{
		case 16:
			num = 1;
			break;
		case 17:
			num = 8;
			break;
		case 18:
			num = 11;
			break;
		case 19:
			num = 9;
			break;
		}
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			switch (filter)
			{
			case 0:
				foreach (int num2 in availableFilters)
				{
					if (num2 != 0)
					{
						flag = GetIsInventoryFiltered(charID, item, num2, advancedFilter, availableFilters);
						if (!flag)
						{
							break;
						}
					}
				}
				break;
			case 12:
				flag = num != 12 && num != 13 && num != 14 && (num != 1 || item.subtype != 1 || !(item as EquipmentRef).hasSubtype(VariableBook.fishingEquipmentSubtype.id));
				break;
			case 1:
				flag = (num != 1 || (item as EquipmentRef).hasSubtype(VariableBook.fishingEquipmentSubtype.id)) && num != 8;
				break;
			case 2:
			case 4:
				flag = num != filter;
				break;
			default:
				flag = true;
				break;
			}
		}
		else
		{
			bool flag2 = num == 1 && (item as EquipmentRef).hasSubtype(VariableBook.fishingEquipmentSubtype.id);
			bool num3 = !advancedFilter.IsRarityFilterOn(item.rarityRef.id);
			bool flag3 = num != 1 || !advancedFilter.IsTypeFilterOn(1) || !advancedFilter.IsEquipmentFilterOn(item.subtype);
			bool flag4 = (!advancedFilter.IsTypeFilterOn(num) && !(advancedFilter.IsTypeFilterOn(12) && !advancedFilter.IsTypeFilterOn(1) && flag2) && (!advancedFilter.IsTypeFilterOn(12) || (num != 14 && num != 13))) || (!advancedFilter.IsTypeFilterOn(12) && advancedFilter.IsTypeFilterOn(1) && flag2);
			flag = num3 || (num == 1 && !flag2 && flag3) || flag4;
		}
		return flag;
	}

	public int GetInventoryFilter(int charID)
	{
		return RetrieveInt("INVENTORY_FILTER_TYPE_" + charID, 0);
	}

	public void SetInventoryFilter(int charID, int type)
	{
		Store("INVENTORY_FILTER_TYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetInventoryAdvancedFilter(int charID)
	{
		string key = "INVENTORY_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> equipment = new Dictionary<int, bool>();
		Dictionary<int, bool> type = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.inventoryFilterRarities != null)
			{
				rarity = arrayData.inventoryFilterRarities;
			}
			if (arrayData.inventoryFilterEquipment != null)
			{
				equipment = arrayData.inventoryFilterEquipment;
			}
			if (arrayData.inventoryFilterTypes != null)
			{
				type = arrayData.inventoryFilterTypes;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, equipment, null, type);
	}

	public void SetInventoryAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.inventoryFilterRarities = settings.rarityFilters;
		arrayData.inventoryFilterEquipment = settings.equipmentFilters;
		arrayData.inventoryFilterTypes = settings.typeFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "INVENTORY_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetIsExchangeFiltered(int charID, ItemRef item, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null)
	{
		int num = item.itemType;
		switch (item.itemType)
		{
		case 16:
			num = 1;
			break;
		case 17:
			num = 8;
			break;
		case 18:
			num = 11;
			break;
		case 19:
			num = 9;
			break;
		}
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			switch (filter)
			{
			case 0:
				foreach (int num2 in availableFilters)
				{
					if (num2 != 0)
					{
						flag = GetIsExchangeFiltered(charID, item, num2, advancedFilter, availableFilters);
						if (!flag)
						{
							break;
						}
					}
				}
				break;
			case 1:
				flag = num != 1 && num != 8;
				break;
			case 9:
			case 11:
			case 15:
				flag = num != filter;
				break;
			default:
				flag = true;
				break;
			}
		}
		else
		{
			bool num3 = !advancedFilter.IsRarityFilterOn(item.rarityRef.id);
			bool flag2 = num != 1 || !advancedFilter.IsTypeFilterOn(1) || !advancedFilter.IsEquipmentFilterOn(item.subtype);
			bool flag3 = !advancedFilter.IsTypeFilterOn(num);
			flag = num3 || (num == 1 && flag2) || flag3;
		}
		return flag;
	}

	public int GetExchangeFilter(int charID)
	{
		return RetrieveInt("EXCHANGE_FILTER_TYPE_" + charID, 0);
	}

	public void SetExchangeFilter(int charID, int type)
	{
		Store("EXCHANGE_FILTER_TYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetExchangeAdvancedFilter(int charID)
	{
		string key = "EXCHANGE_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> equipment = new Dictionary<int, bool>();
		Dictionary<int, bool> type = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.exchangeFilterRarities != null)
			{
				rarity = arrayData.exchangeFilterRarities;
			}
			if (arrayData.exchangeFilterEquipment != null)
			{
				equipment = arrayData.exchangeFilterEquipment;
			}
			if (arrayData.exchangeFilterTypes != null)
			{
				type = arrayData.exchangeFilterTypes;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, equipment, null, type);
	}

	public void SetExchangeAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.exchangeFilterRarities = settings.rarityFilters;
		arrayData.exchangeFilterEquipment = settings.equipmentFilters;
		arrayData.exchangeFilterTypes = settings.typeFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "EXCHANGE_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetIsTradeFiltered(int charID, int resultType, int resultRarity, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null)
	{
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			switch (filter)
			{
			case 0:
				foreach (int num in availableFilters)
				{
					if (num != 0)
					{
						flag = GetIsTradeFiltered(charID, resultType, resultRarity, num, advancedFilter, availableFilters);
						if (!flag)
						{
							break;
						}
					}
				}
				break;
			case 1:
				flag = resultType != 1 && resultType != 8;
				break;
			case 2:
			case 3:
			case 4:
			case 9:
			case 15:
				flag = resultType != filter;
				break;
			default:
				flag = true;
				break;
			}
		}
		else
		{
			bool num2 = !advancedFilter.IsRarityFilterOn(resultRarity);
			bool flag2 = !advancedFilter.IsTypeFilterOn(resultType) && (!advancedFilter.IsTypeFilterOn(1) || resultType != 8 || (advancedFilter.IsTypeFilterOn(1) && resultType == 8));
			flag = num2 || flag2;
		}
		return flag;
	}

	public int GetTradeFilter(int charID)
	{
		return RetrieveInt("TRADE_FILTER_TYPE_" + charID, 0);
	}

	public void SetTradeFilter(int charID, int type)
	{
		Store("TRADE_FILTER_TYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetTradeAdvancedFilter(int charID)
	{
		string key = "TRADE_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> type = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.tradeFilterRarities != null)
			{
				rarity = arrayData.tradeFilterRarities;
			}
			if (arrayData.tradeFilterTypes != null)
			{
				type = arrayData.tradeFilterTypes;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, null, null, type);
	}

	public void SetTradeAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.tradeFilterRarities = settings.rarityFilters;
		arrayData.tradeFilterTypes = settings.typeFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "TRADE_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetIsAugmentFiltered(int charID, AugmentData augmentData, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null, bool hasSkippedAll = false)
	{
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			if (filter == 0)
			{
				foreach (int num in availableFilters)
				{
					flag = GetIsAugmentFiltered(charID, augmentData, num + 1, advancedFilter, availableFilters, hasSkippedAll: true);
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				flag = filter != augmentData.augmentRef.typeRef.id + 1;
			}
		}
		else
		{
			bool num2 = !advancedFilter.IsRarityFilterOn(augmentData.augmentRef.rarityRef.id);
			bool flag2 = !advancedFilter.IsAugmentFilterOn(augmentData.augmentRef.typeRef.id);
			flag = num2 || flag2;
		}
		return flag;
	}

	public int GetAugmentFilter(int charID)
	{
		return RetrieveInt("AUGMENT_FILTER_SUBTYPE_" + charID, 0);
	}

	public void SetAugmentFilter(int charID, int type)
	{
		Store("AUGMENT_FILTER_SUBTYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetAugmentAdvancedFilter(int charID)
	{
		string key = "AUGMENT_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> augment = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.augmentsFilterRarities != null)
			{
				rarity = arrayData.augmentsFilterRarities;
			}
			if (arrayData.augmentsFilterAugment != null)
			{
				augment = arrayData.augmentsFilterAugment;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, null, augment);
	}

	public void SetAugmentAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.augmentsFilterRarities = settings.rarityFilters;
		arrayData.augmentsFilterAugment = settings.augmentFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "AUGMENT_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetIsUpgradeFiltered(int charID, ItemRef itemRef, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null, bool hasSkippedAll = false)
	{
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			if (filter == 0)
			{
				foreach (int filter2 in availableFilters)
				{
					flag = GetIsUpgradeFiltered(charID, itemRef, filter2, advancedFilter, availableFilters, hasSkippedAll: true);
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				flag = filter != itemRef.subtype;
			}
		}
		else
		{
			bool num = !advancedFilter.IsRarityFilterOn(itemRef.rarity);
			bool flag2 = !advancedFilter.IsEquipmentFilterOn(itemRef.subtype);
			flag = num || flag2;
		}
		return flag;
	}

	public int GetUpgradeFilter(int charID)
	{
		return RetrieveInt("UPGRADE_FILTER_SUBTYPE_" + charID, 0);
	}

	public void SetUpgradeFilter(int charID, int type)
	{
		Store("UPGRADE_FILTER_SUBTYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetUpgradeAdvancedFilter(int charID)
	{
		string key = "UPGRADE_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> equipment = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.upgradeFilterRarities != null)
			{
				rarity = arrayData.upgradeFilterRarities;
			}
			if (arrayData.upgradeFilterEquipment != null)
			{
				equipment = arrayData.upgradeFilterEquipment;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, equipment);
	}

	public void SetUpgradeAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.upgradeFilterRarities = settings.rarityFilters;
		arrayData.upgradeFilterEquipment = settings.equipmentFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "UPGRADE_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetIsReforgeFiltered(int charID, ItemRef itemRef, int filter, AdvancedFilterSettings advancedFilter, int[] availableFilters = null, bool hasSkippedAll = false)
	{
		bool flag = true;
		if (!advancedFilter.enabled)
		{
			if (filter == 0)
			{
				foreach (int filter2 in availableFilters)
				{
					flag = GetIsUpgradeFiltered(charID, itemRef, filter2, advancedFilter, availableFilters, hasSkippedAll: true);
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				flag = filter != itemRef.subtype;
			}
		}
		else
		{
			bool num = !advancedFilter.IsRarityFilterOn(itemRef.rarity);
			bool flag2 = !advancedFilter.IsEquipmentFilterOn(itemRef.subtype);
			flag = num || flag2;
		}
		return flag;
	}

	public int GetReforgeFilter(int charID)
	{
		return RetrieveInt("REFORGE_FILTER_SUBTYPE_" + charID, 0);
	}

	public void SetReforgeFilter(int charID, int type)
	{
		Store("REFORGE_FILTER_SUBTYPE_" + charID, type);
	}

	public AdvancedFilterSettings GetReforgeAdvancedFilter(int charID)
	{
		string key = "REFORGE_ADVANCED_FILTER_" + charID;
		bool enabled = RetrieveBool(key);
		Dictionary<int, bool> rarity = new Dictionary<int, bool>();
		Dictionary<int, bool> equipment = new Dictionary<int, bool>();
		key = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(key);
		if (arrayData != null)
		{
			if (arrayData.reforgeFilterRarities != null)
			{
				rarity = arrayData.reforgeFilterRarities;
			}
			if (arrayData.reforgeFilterEquipment != null)
			{
				equipment = arrayData.reforgeFilterEquipment;
			}
		}
		return new AdvancedFilterSettings(enabled, rarity, equipment);
	}

	public void SetReforgeAdvancedFilter(int charID, AdvancedFilterSettings settings)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.reforgeFilterRarities = settings.rarityFilters;
		arrayData.reforgeFilterEquipment = settings.equipmentFilters;
		Store(arrayDataString, arrayData);
		arrayDataString = "REFORGE_ADVANCED_FILTER_" + charID;
		Store(arrayDataString, settings.enabled);
	}

	public bool GetDialogSeen(int charID, string dialog)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		if (arrayData.dialogsSeen == null)
		{
			arrayData.dialogsSeen = new List<string>();
			Store(arrayDataString, arrayData);
		}
		return arrayData.dialogsSeen.Contains(dialog);
	}

	public void SetDialogSeen(int charID, string dialog)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		if (arrayData.dialogsSeen == null)
		{
			arrayData.dialogsSeen = new List<string>();
		}
		else if (arrayData.dialogsSeen.Contains(dialog))
		{
			return;
		}
		arrayData.dialogsSeen.Add(dialog);
		Store(arrayDataString, arrayData);
	}

	public void ClearDialogsSeen(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.dialogsSeen.Clear();
		Store(arrayDataString, arrayData);
	}

	public List<string> GetBoosterBadge(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		if (arrayData.boosterBadge == null)
		{
			return new List<string>();
		}
		return arrayData.boosterBadge;
	}

	public void SetBoosterBadge(int charID, List<string> boosterBadge)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		if (arrayData.boosterBadge == null)
		{
			arrayData.boosterBadge = new List<string>();
		}
		arrayData.boosterBadge.Clear();
		arrayData.boosterBadge.AddRange(boosterBadge);
		Store(arrayDataString, arrayData);
	}

	public void SetInvasionEventDifficulty(int charID, int difficulty)
	{
		Store(GetDifficultyString(charID), difficulty);
	}

	public int GetInvasionEventDifficulty(int charID)
	{
		return RetrieveInt(GetDifficultyString(charID), 0);
	}

	private string GetDifficultyString(int charID)
	{
		return "INVASION_EVENT_DIFFICULTY_" + charID;
	}

	public void SetRiftEventDifficulty(int difficulty)
	{
		SetRiftEventDifficulty(GameData.instance.PROJECT.character.id, difficulty);
	}

	public void SetRiftEventDifficulty(int charID, int difficulty)
	{
		Store(GetRiftEventDifficultyString(charID), difficulty);
	}

	public int GetRiftEventDifficulty()
	{
		return GetRiftEventDifficulty(GameData.instance.PROJECT.character.id);
	}

	public int GetRiftEventDifficulty(int charID)
	{
		return RetrieveInt(GetRiftEventDifficultyString(charID), 0);
	}

	public string GetRiftEventDifficultyString(int charID)
	{
		return "RIFT_EVENT_DIFFICULTY_" + charID;
	}

	public void SetGauntletEventDifficulty(int difficulty)
	{
		SetGauntletEventDifficulty(GameData.instance.PROJECT.character.id, difficulty);
	}

	public void SetGauntletEventDifficulty(int charID, int difficulty)
	{
		Store(GetGauntletEventDifficultyString(charID), difficulty);
	}

	public int GetGauntletEventDifficulty()
	{
		return GetGauntletEventDifficulty(GameData.instance.PROJECT.character.id);
	}

	public int GetGauntletEventDifficulty(int charID)
	{
		return RetrieveInt(GetGauntletEventDifficultyString(charID), 0);
	}

	public string GetGauntletEventDifficultyString(int charID)
	{
		return "GAUNTLET_EVENT_DIFFICULTY_" + charID;
	}

	public void SetGvEEventDifficulty(int difficulty)
	{
		SetGvEEventDifficulty(GameData.instance.PROJECT.character.id, difficulty);
	}

	public void SetGvEEventDifficulty(int charID, int difficulty)
	{
		Store(GetGvEEventDifficultyString(charID), difficulty);
	}

	public int GetGvEEventDifficulty(int charID)
	{
		return RetrieveInt(GetGvEEventDifficultyString(charID), 0);
	}

	public string GetGvEEventDifficultyString(int charID)
	{
		return "GVE_EVENT_DIFFICULTY_" + charID;
	}

	public void SetPvPEventBonus(int difficulty)
	{
		SetPvPEventBonus(GameData.instance.PROJECT.character.id, difficulty);
	}

	public void SetPvPEventBonus(int charID, int difficulty)
	{
		Store(GetPvPEventBonusString(charID), difficulty);
	}

	public int GetPvPEventBonus()
	{
		return GetPvPEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetPvPEventBonus(int charID)
	{
		return RetrieveInt(GetPvPEventBonusString(charID));
	}

	public string GetPvPEventBonusString(int charID)
	{
		return "PVP_EVENT_BONUS_" + charID;
	}

	public void SetPvPEventSort(bool sort)
	{
		SetPvPEventSort(GameData.instance.PROJECT.character.id, sort);
	}

	public void SetPvPEventSort(int charID, bool sort)
	{
		Store(GetPvPEventSortString(charID), sort);
	}

	public bool GetPvPEventSort(int charID)
	{
		return RetrieveBool(GetPvPEventSortString(charID));
	}

	public string GetPvPEventSortString(int charID)
	{
		return "PVP_EVENT_SORT_" + charID;
	}

	public void SetRiftEventBonus(int bonus)
	{
		SetRiftEventBonus(GameData.instance.PROJECT.character.id, bonus);
	}

	public void SetRiftEventBonus(int charID, int bonus)
	{
		Store(GetRiftEventBonusString(charID), bonus);
	}

	public int GetRiftEventBonus()
	{
		return GetRiftEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetRiftEventBonus(int charID)
	{
		return RetrieveInt(GetRiftEventBonusString(charID));
	}

	public string GetRiftEventBonusString(int charID)
	{
		return "RIFT_EVENT_BONUS_" + charID;
	}

	public void SetGauntletEventBonus(int bonus)
	{
		SetGauntletEventBonus(GameData.instance.PROJECT.character.id, bonus);
	}

	public void SetGauntletEventBonus(int charID, int bonus)
	{
		Store(GetGauntletEventBonusString(charID), bonus);
	}

	public int GetGauntletEventBonus()
	{
		return GetGauntletEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetGauntletEventBonus(int charID)
	{
		return RetrieveInt(GetGauntletEventBonusString(charID));
	}

	public string GetGauntletEventBonusString(int charID)
	{
		return "GAUNTLET_EVENT_BONUS_" + charID;
	}

	public void SetInvasionEventBonus(int bonus)
	{
		SetInvasionEventBonus(GameData.instance.PROJECT.character.id, bonus);
	}

	public void SetInvasionEventBonus(int charID, int bonus)
	{
		Store(GetInvasionEventBonusString(charID), bonus);
	}

	public int GetInvasionEventBonus()
	{
		return GetInvasionEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetInvasionEventBonus(int charID)
	{
		return RetrieveInt(GetInvasionEventBonusString(charID));
	}

	public string GetInvasionEventBonusString(int charID)
	{
		return "INVASION_EVENT_BONUS_" + charID;
	}

	public void SetGvGEventBonus(int bonus)
	{
		SetGvGEventBonus(GameData.instance.PROJECT.character.id, bonus);
	}

	public void SetGvGEventBonus(int charID, int bonus)
	{
		Store(GetGvGEventBonusString(charID), bonus);
	}

	public int GetGvGEventBonus()
	{
		return GetGvGEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetGvGEventBonus(int charID)
	{
		return RetrieveInt(GetGvGEventBonusString(charID));
	}

	public string GetGvGEventBonusString(int charID)
	{
		return "GVG_EVENT_BONUS_" + charID;
	}

	public void SetGvGEventSort(bool sort)
	{
		SetGvGEventSort(GameData.instance.PROJECT.character.id, sort);
	}

	public void SetGvGEventSort(int charID, bool sort)
	{
		Store(GetGvGEventSortString(charID), sort);
	}

	public bool GetGvGEventSort(int charID)
	{
		return RetrieveBool(GetGvGEventSortString(charID));
	}

	public string GetGvGEventSortString(int charID)
	{
		return "GVG_EVENT_SORT_" + charID;
	}

	public void SetGvEEventBonus(int bonus)
	{
		SetGvEEventBonus(GameData.instance.PROJECT.character.id, bonus);
	}

	public void SetGvEEventBonus(int charID, int bonus)
	{
		Store(GetGvEEventBonusString(charID), bonus);
	}

	public int GetGvEEventBonus()
	{
		return GetGvEEventBonus(GameData.instance.PROJECT.character.id);
	}

	public int GetGvEEventBonus(int charID)
	{
		return RetrieveInt(GetGvEEventBonusString(charID));
	}

	public string GetGvEEventBonusString(int charID)
	{
		return "GVE_EVENT_BONUS_" + charID;
	}

	public void SetRaidSelected(int selected)
	{
		SetRaidSelected(GameData.instance.PROJECT.character.id, selected);
	}

	public void SetRaidSelected(int charID, int selected)
	{
		Store(GetRaidSelectedString(charID), selected);
	}

	public int GetRaidSelected()
	{
		return GetRaidSelected(GameData.instance.PROJECT.character.id);
	}

	public int GetRaidSelected(int charID)
	{
		return RetrieveInt(GetRaidSelectedString(charID));
	}

	public string GetRaidSelectedString(int charID)
	{
		return "RAID_SELECTED_" + charID;
	}

	public void SetBrawlSelected(int selected)
	{
		SetBrawlSelected(GameData.instance.PROJECT.character.id, selected);
	}

	public void SetBrawlSelected(int charID, int selected)
	{
		Store(GetBrawlSelectedString(charID), selected);
	}

	public int GetBrawlSelected(int charID)
	{
		return RetrieveInt(GetBrawlSelectedString(charID));
	}

	public int GetBrawlSelected()
	{
		return GetBrawlSelected(GameData.instance.PROJECT.character.id);
	}

	public string GetBrawlSelectedString(int charID)
	{
		return "BRAWL_SELECTED_" + charID;
	}

	public void SetBrawlTierSelected(int brawlID, int selected)
	{
		SetBrawlTierSelected(GameData.instance.PROJECT.character.id, brawlID, selected);
	}

	public void SetBrawlTierSelected(int charID, int brawlID, int selected)
	{
		Store(GetBrawlTierSelectedString(charID, brawlID), selected);
	}

	public int GetBrawlTierSelected(int charID, int brawlID)
	{
		return RetrieveInt(GetBrawlTierSelectedString(charID, brawlID));
	}

	public string GetBrawlTierSelectedString(int charID, int brawlID)
	{
		return "BRAWL_TIER_SELECTED_" + brawlID + "_" + charID;
	}

	public void SetBrawlDifficultySelected(int brawlID, int difficulty)
	{
		SetBrawlDifficultySelected(GameData.instance.PROJECT.character.id, brawlID, difficulty);
	}

	public void SetBrawlDifficultySelected(int charID, int brawlID, int difficulty)
	{
		Store(GetBrawlDifficultySelectedString(charID, brawlID), difficulty);
	}

	public int GetBrawlDifficultySelected(int charID, int brawlID)
	{
		return RetrieveInt(GetBrawlDifficultySelectedString(charID, brawlID));
	}

	public string GetBrawlDifficultySelectedString(int charID, int brawlID)
	{
		return "BRAWL_DIFFICULTY_SELECTED_" + brawlID + "_" + charID;
	}

	public void SetBrawlPublicSelected(bool selected)
	{
		SetBrawlPublicSelected(GameData.instance.PROJECT.character.id, selected);
	}

	public void SetBrawlPublicSelected(int charID, bool selected)
	{
		Store(GetBrawlPublicSelectedString(charID), selected);
	}

	public bool GetBrawlPublicSelected(int charID)
	{
		return RetrieveBool(GetBrawlPublicSelectedString(charID));
	}

	public string GetBrawlPublicSelectedString(int charID)
	{
		return "BRAWL_PUBLIC_SELECTED_" + charID;
	}

	public void SetBrawlFilter(int filter)
	{
		SetBrawlFilter(GameData.instance.PROJECT.character.id, filter);
	}

	public void SetBrawlFilter(int charID, int filter)
	{
		Store(GetBrawlFilterString(charID), filter);
	}

	public int GetBrawlFilter(int charID)
	{
		return RetrieveInt(GetBrawlFilterString(charID));
	}

	public string GetBrawlFilterString(int charID)
	{
		return "BRAWL_FILTER_" + charID;
	}

	public void SetBrawlTierFilter(int tierFilter)
	{
		SetBrawlTierFilter(GameData.instance.PROJECT.character.id, tierFilter);
	}

	public void SetBrawlTierFilter(int charID, int tierFilter)
	{
		Store(GetBrawlTierFilterString(charID), tierFilter);
	}

	public int GetBrawlTierFilter(int charID)
	{
		return RetrieveInt(GetBrawlTierFilterString(charID));
	}

	public string GetBrawlTierFilterString(int charID)
	{
		return "BRAWL_TIER_FILTER_" + charID;
	}

	public void SetBrawlDifficultyFilter(int difficultyFilter)
	{
		SetBrawlTierFilter(GameData.instance.PROJECT.character.id, difficultyFilter);
	}

	public void SetBrawlDifficultyFilter(int charID, int difficultyFilter)
	{
		Store(GetBrawlDifficultyFilterString(charID), difficultyFilter);
	}

	public int GetBrawlDifficultyFilter(int charID)
	{
		return RetrieveInt(GetBrawlDifficultyFilterString(charID));
	}

	public string GetBrawlDifficultyFilterString(int charID)
	{
		return "BRAWL_DIFFICULTY_FILTER_" + charID;
	}

	public void SetFriendRequestSort(int sort, int? charID = null)
	{
		Store(GetFriendRequestString(charID ?? GameData.instance.PROJECT.character.id), sort);
	}

	public int GetFriendRequestSort(int? charID = null)
	{
		return RetrieveInt(GetFriendRequestString(charID ?? GameData.instance.PROJECT.character.id), 0);
	}

	public string GetFriendRequestString(int charID)
	{
		return "FREIND_REQUEST_SORT_" + charID;
	}

	public void SetGuildApplicantSort(int sort)
	{
		SetGuildApplicantSort(GameData.instance.PROJECT.character.id, sort);
	}

	public void SetGuildApplicantSort(int charID, int sort)
	{
		Store(GetGuildApplicantString(charID), sort);
	}

	public int GetGuildApplicantSort(int charID)
	{
		return RetrieveInt(GetGuildApplicantString(charID), 0);
	}

	public string GetGuildApplicantString(int charID)
	{
		return "GUILD_APPLICANT_SORT_" + charID;
	}

	public void SetEnchantSelectSort(int sort)
	{
		SetEnchantSelectSort(GameData.instance.PROJECT.character.id, sort);
	}

	public void SetEnchantSelectSort(int charID, int sort)
	{
		Store(GetEnchantSelectString(charID), sort);
	}

	public int GetEnchantSelectSort(int charID)
	{
		return RetrieveInt(GetEnchantSelectString(charID), 0);
	}

	public string GetEnchantSelectString(int charID)
	{
		return "ENCHANT_SELECT_SORT_" + charID;
	}

	public void SetMountSelectSort(int sort)
	{
		SetMountSelectSort(GameData.instance.PROJECT.character.id, sort);
	}

	public void SetMountSelectSort(int charID, int sort)
	{
		Store(GetMountSelectString(charID), sort);
	}

	public int GetMountSelectSort(int charID)
	{
		return RetrieveInt(GetMountSelectString(charID), 0);
	}

	public string GetMountSelectString(int charID)
	{
		return "MOUNT_SELECT_SORT_" + charID;
	}

	public void SetFishingRod(int fishingRod)
	{
		SetFishingRod(GameData.instance.PROJECT.character.id, fishingRod);
	}

	public void SetFishingRod(int charID, int fishingRod)
	{
		Store(GetFishingRodString(charID), fishingRod);
	}

	public int GetFishingRod(int charID)
	{
		return RetrieveInt(GetFishingRodString(charID));
	}

	public string GetFishingRodString(int charID)
	{
		return "FISHING_ROD_" + charID;
	}

	public void SetFishingBobber(int fishingBobber)
	{
		SetFishingBobber(GameData.instance.PROJECT.character.id, fishingBobber);
	}

	public void SetFishingBobber(int charID, int fishingBobber)
	{
		Store(GetFishingBobberString(charID), fishingBobber);
	}

	public int GetFishingBobber(int charID)
	{
		return RetrieveInt(GetFishingBobberString(charID));
	}

	public string GetFishingBobberString(int charID)
	{
		return "FISHING_BOBBER_" + charID;
	}

	public void SetFishingBait(int fishingBait)
	{
		SetFishingBait(GameData.instance.PROJECT.character.id, fishingBait);
	}

	public void SetFishingBait(int charID, int fishingBait)
	{
		Store(GetFishingBaitString(charID), fishingBait);
	}

	public int GetFishingBait(int charID)
	{
		return RetrieveInt(GetFishingBaitString(charID));
	}

	public string GetFishingBaitString(int charID)
	{
		return "FISHING_BAIT_" + charID;
	}

	public void SetAutopersuadeFamiliarsGoldRarities(int charID, Dictionary<int, bool> rarities)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.persuadeFamiliarsGoldRarities = rarities;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetAutopersuadeFamiliarsGoldRarities(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).persuadeFamiliarsGoldRarities;
	}

	public bool GetAutopersuadeGoldFamiliarRarity(int charID, RarityRef rarity, Dictionary<int, bool> familiarRarities)
	{
		if (familiarRarities == null || rarity == null || rarity.id < 0 || !familiarRarities.ContainsKey(rarity.id))
		{
			return true;
		}
		return familiarRarities[rarity.id];
	}

	public bool GetAutopersuadeGemsFamiliarRarity(int charID, RarityRef rarity, Dictionary<int, bool> familiarRarities)
	{
		if (familiarRarities == null || rarity == null || rarity.id < 0 || !familiarRarities.ContainsKey(rarity.id))
		{
			return false;
		}
		return familiarRarities[rarity.id];
	}

	public void SetAutopersuadeFamiliarsGemsRarities(int charID, Dictionary<int, bool> rarities)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.persuadeFamiliarsGemsRarities = rarities;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetAutopersuadeFamiliarsGemsRarities(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).persuadeFamiliarsGemsRarities;
	}

	public void SetSeenSchematics(int charID, Dictionary<int, bool> schematics)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.seenSchematics = schematics;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetSeenSchematics(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).seenSchematics;
	}

	public void SetSeenRecipes(int charID, Dictionary<int, bool> recipes)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.seenRecipes = recipes;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetSeenRecipes(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).seenRecipes;
	}

	public void SetSeenFamiliars(int charID, Dictionary<int, bool> familiars)
	{
		string arrayDataString = GetArrayDataString(charID);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.seenFamiliars = familiars;
		Store(arrayDataString, arrayData);
	}

	public Dictionary<int, bool> GetSeenFamiliars(int charID)
	{
		string arrayDataString = GetArrayDataString(charID);
		return RetrieveArrayData(arrayDataString).seenFamiliars;
	}

	public string GetBookContent(string name)
	{
		try
		{
			return EncryptionHelper.Decrypt(RetrieveString(name));
		}
		catch (CryptographicException)
		{
			return null;
		}
	}

	public void SetBookContent(string name, string content)
	{
		content = EncryptionHelper.Encrypt(content);
		Store(name, content);
	}

	public List<ItemData> GetDungeonLoot(int dungeonType, int dungeonId)
	{
		int num = RetrieveInt("DUNGEON_LOOT_TYPE");
		int num2 = RetrieveInt("DUNGEON_LOOT_ID");
		if (dungeonType != num || dungeonId != num2)
		{
			return new List<ItemData>();
		}
		string arrayDataString = GetArrayDataString(GameData.instance.PROJECT.character.id);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		return GetItemDataListFromIntIntIntList(arrayData.dungeonLoot);
	}

	public void SetDungeonLoot(List<ItemData> loot, int dungeonType, int dungeonId)
	{
		Store("DUNGEON_LOOT_TYPE", dungeonType);
		Store("DUNGEON_LOOT_ID", dungeonId);
		string arrayDataString = GetArrayDataString(GameData.instance.PROJECT.character.id);
		ArrayData arrayData = RetrieveArrayData(arrayDataString);
		arrayData.dungeonLoot = GetIntIntIntListFromItemDataList(loot);
		Store(arrayDataString, arrayData);
	}

	private List<IntIntIntCombination> GetIntIntIntListFromItemDataList(List<ItemData> items)
	{
		List<IntIntIntCombination> list = new List<IntIntIntCombination>();
		if (items == null || items.Count < 0)
		{
			return list;
		}
		foreach (ItemData item in items)
		{
			list.Add(new IntIntIntCombination(item.type, item.id, item.qty));
		}
		return list;
	}

	private List<ItemData> GetItemDataListFromIntIntIntList(List<IntIntIntCombination> combinations)
	{
		List<ItemData> list = new List<ItemData>();
		if (combinations == null || combinations.Count < 0)
		{
			return list;
		}
		foreach (IntIntIntCombination combination in combinations)
		{
			list.Add(new ItemData(ItemBook.Lookup(combination.subId, combination.id), combination.value));
		}
		return list;
	}
}
