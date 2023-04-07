using System;
using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.achievement;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.character.achievements;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.utility;
using Newtonsoft.Json;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Steamworks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace com.ultrabit.bitheroes.model.character;

public class Character : Messenger
{
	[Serializable]
	public class IMXG0Data
	{
		[Serializable]
		public class Puppet
		{
			public string skin;

			public string outfit;

			public string eyes;

			public string mask;

			public string hair;

			public string hat;

			public string horn;

			public string halo;
		}

		[Serializable]
		public class Card
		{
			public int gen;

			public string frame;

			public string background;
		}

		public string name;

		public string rarity;

		public Puppet puppet;

		public Card card;
	}

	public enum NFTState
	{
		basicHero,
		bitverseAvatar,
		bitverseHero,
		bitverseHeroFrozen
	}

	public const string GENDER_MALE = "M";

	public const string GENDER_FEMALE = "F";

	public const int STAT_POWER = 0;

	public const int STAT_STAMINA = 1;

	public const int STAT_AGILITY = 2;

	private static Dictionary<string, int> STAT_TYPES = new Dictionary<string, int>
	{
		["power"] = 0,
		["stamina"] = 1,
		["agility"] = 2
	};

	private static Dictionary<int, string> STAT_NAMES = new Dictionary<int, string>
	{
		[0] = "stat_power",
		[1] = "stat_stamina",
		[2] = "stat_agility"
	};

	private int _id;

	private int _playerID;

	private string _name;

	private string _gender = "";

	private int _hairID;

	private int _hairColorID;

	private int _skinColorID;

	private int _level;

	private long _exp;

	private int _gold;

	private int _credits;

	private int _points;

	private int _power;

	private int _stamina;

	private int _agility;

	private int _zoneID;

	private int _zoneCompleted;

	private int _dailyID;

	private DateTime _dailyDate;

	private DateTime _dailyFishingDate;

	private int _energy;

	private long _energyMilliseconds;

	private long _energyCooldown;

	private float _energyStartTime;

	private Coroutine _energyTimer;

	private int _energySeconds;

	private int _tickets;

	private long _ticketsMilliseconds;

	private long _ticketsCooldown;

	private float _ticketsStartTime;

	private Coroutine _ticketsTimer;

	private int _ticketsSeconds;

	private int _changename;

	private long _changenameCooldown;

	private int _shards;

	private long _shardsMilliseconds;

	private long _shardsCooldown;

	private float _shardsStartTime;

	private Coroutine _shardsTimer;

	private int _shardsSeconds;

	private int _tokens;

	private long _tokensMilliseconds;

	private long _tokensCooldown;

	private float _tokensStartTime;

	private Coroutine _tokensTimer;

	private int _tokensSeconds;

	private int _badges;

	private long _badgesMilliseconds;

	private long _badgesCooldown;

	private float _badgesStartTime;

	private Coroutine _badgesTimer;

	private int _badgesSeconds;

	private int _shopRotationID = -1;

	private long _shopRotationMilliseconds;

	private float _shopRotationStartTime;

	private Coroutine _shopRotationTimer;

	private int _shopRotationSeconds;

	private long _raidCooldownMilliseconds;

	private float _raidCooldownStartTime;

	private Coroutine _raidCooldownTimer;

	private int _raidCooldownSeconds;

	private long _adMilliseconds;

	private float _adStartTime;

	private Coroutine _adTimer;

	private ItemData _adItem;

	private DateTime? _nbpDate;

	private bool _admin;

	private bool _moderator;

	private bool _autoPilot;

	private bool _chatEnabled;

	private int _chatChannel;

	private List<ChatPlayerData> _chatIgnores;

	private bool _friendRequestsEnabled = true;

	private bool _duelRequestsEnabled = true;

	private bool _showHelm = true;

	private bool _showMount = true;

	private bool _showBody = true;

	private bool _showAccessory = true;

	private List<ItemRef> _lockedItems;

	private List<ItemData> _dungeonLootItems = new List<ItemData>();

	private Inventory _inventory;

	private Tutorial _tutorial;

	private Equipment _equipment;

	private Runes _runes;

	private Enchants _enchants;

	private Augments _augments;

	private Mounts _mounts;

	private Armory _armory;

	private Zones _zones;

	private Teams _teams;

	private DailyQuests _dailyQuests;

	private CharacterAchievements _characterAchievements;

	private FamiliarStable _familiarStable;

	private List<int> _platforms;

	private List<FriendData> _friends;

	private List<RequestData> _requests;

	private List<GuildInfo> _guildInvites;

	private List<ConsumableModifierData> _consumableModifiers;

	private CharacterGuildData _guildData;

	private List<Conversation> _conversations = new List<Conversation>();

	private int _pvpEventLootID;

	private int _riftEventLootID;

	private int _gauntletEventLootID;

	private int _gvgEventLootID;

	private int _invasionEventLootID;

	private int _fishingEventLootID;

	private int _gveEventLootID;

	private BHAnalytics _analytics;

	private long _updateMilliseconds;

	private Coroutine _updateTimer;

	private CharacterExtraInfo _extraInfo;

	private string _herotag;

	private bool _nameHasChanged;

	private List<int> _eventsWon;

	private List<TeammateData> _selectedTeammates;

	private Dungeon _rerunDungeon;

	private long _rerunArmoryID = -1L;

	private int _lastEventType = -1;

	private bool? _lastBrawlPrivateCheckbox;

	public bool adminLoggedIn;

	private string _customconsum;

	private List<BoosterRef> _activeBoosters = new List<BoosterRef>();

	private List<BoosterRef> _passiveBoosters = new List<BoosterRef>();

	private Dictionary<int, Coroutine> _boostersCoroutines = new Dictionary<int, Coroutine>();

	public UnityCustomEvent BOOSTER_CHANGED = new UnityCustomEvent();

	private List<CharacterData> _heroDatas;

	private IMXG0Data _imxG0Data;

	private int _nftState;

	private string _nftToken;

	public List<CharacterData> heroDatas
	{
		get
		{
			return _heroDatas;
		}
		set
		{
			_heroDatas = value;
		}
	}

	public int id => _id;

	public int playerID => _playerID;

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			Broadcast("NAME_CHANGE");
		}
	}

	public string herotag
	{
		get
		{
			return _herotag;
		}
		set
		{
			_herotag = value;
		}
	}

	public string nameAndHeroTag => name + "#" + herotag;

	public string heroType
	{
		get
		{
			if (imxG0Data == null)
			{
				return "Basic Hero";
			}
			return imxG0Data.name;
		}
	}

	public bool nameHasChanged
	{
		get
		{
			return _nameHasChanged;
		}
		set
		{
			_nameHasChanged = value;
		}
	}

	public string gender
	{
		get
		{
			return _gender;
		}
		set
		{
			_gender = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public int hairID
	{
		get
		{
			return _hairID;
		}
		set
		{
			_hairID = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public int hairColorID
	{
		get
		{
			return _hairColorID;
		}
		set
		{
			_hairColorID = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public int skinColorID
	{
		get
		{
			return _skinColorID;
		}
		set
		{
			_skinColorID = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public int level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
			Broadcast("LEVEL_CHANGE");
		}
	}

	public long rerunArmoryID
	{
		get
		{
			return _rerunArmoryID;
		}
		set
		{
			_rerunArmoryID = value;
		}
	}

	public int lastEventType
	{
		get
		{
			return _lastEventType;
		}
		set
		{
			_lastEventType = value;
		}
	}

	public long exp
	{
		get
		{
			return _exp;
		}
		set
		{
			_exp = value;
			long levelExp = getLevelExp(_level + 1);
			while (_exp >= levelExp)
			{
				level++;
				levelExp = getLevelExp(_level + 1);
			}
			Broadcast("EXP_CHANGE");
		}
	}

	public int gold
	{
		get
		{
			return _gold;
		}
		set
		{
			_gold = value;
			Broadcast("CURRENCY_CHANGE");
		}
	}

	public int credits
	{
		get
		{
			return _credits;
		}
		set
		{
			_credits = value;
			Broadcast("CURRENCY_CHANGE");
		}
	}

	public int points
	{
		get
		{
			return _points;
		}
		set
		{
			_points = value;
			Broadcast("POINTS_CHANGE");
		}
	}

	public int power
	{
		get
		{
			return _power;
		}
		set
		{
			_power = value;
			Broadcast("STATS_CHANGE");
		}
	}

	public int stamina
	{
		get
		{
			return _stamina;
		}
		set
		{
			_stamina = value;
			Broadcast("STATS_CHANGE");
		}
	}

	public int agility
	{
		get
		{
			return _agility;
		}
		set
		{
			_agility = value;
			Broadcast("STATS_CHANGE");
		}
	}

	public int zoneID
	{
		get
		{
			return _zoneID;
		}
		set
		{
			_zoneID = value;
		}
	}

	public int zoneCompleted
	{
		get
		{
			checkZoneCompleted();
			return _zoneCompleted;
		}
		set
		{
			_zoneCompleted = value;
		}
	}

	public int tier => getTier(zoneCompleted);

	public int dailyID
	{
		get
		{
			return _dailyID;
		}
		set
		{
			_dailyID = value;
		}
	}

	public DateTime dailyDate
	{
		get
		{
			return _dailyDate;
		}
		set
		{
			_dailyDate = value;
		}
	}

	public DateTime dailyFishingDate
	{
		get
		{
			return _dailyFishingDate;
		}
		set
		{
			_dailyFishingDate = value;
			Broadcast("DAILY_FISHING_DATE_CHANGE");
		}
	}

	public int energy
	{
		get
		{
			return _energy;
		}
		set
		{
			_energy = value;
			Broadcast("ENERGY_CHANGE");
		}
	}

	public int energyMax => (int)((float)(VariableBook.energyMax + (_level - 1) * VariableBook.energyIncrease) + Mathf.Round(GameModifier.getTypeTotal(getModifiers(), 37)));

	public int energySeconds => _energySeconds;

	public long energyMilliseconds
	{
		get
		{
			return _energyMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _energyStartTime) * 1000f);
		}
		set
		{
			_energyMilliseconds = value;
			_energyStartTime = Time.realtimeSinceStartup;
			StartEnergyTimer();
			Broadcast("ENERGY_MILLISECONDS_CHANGE");
		}
	}

	public long energyCooldown
	{
		get
		{
			return _energyCooldown;
		}
		set
		{
			_energyCooldown = value;
		}
	}

	public int tickets
	{
		get
		{
			return _tickets;
		}
		set
		{
			_tickets = value;
			Broadcast("TICKETS_CHANGE");
		}
	}

	public int ticketsMax => (int)(VariableBook.ticketsMax + Mathf.Round(GameModifier.getTypeTotal(getModifiers(), 38)));

	public int ticketsSeconds => _ticketsSeconds;

	public long ticketsMilliseconds
	{
		get
		{
			D.Log($"_ticketsMilliseconds {_ticketsMilliseconds} - Time.realtimeSinceStartup {Time.realtimeSinceStartup} --- _ticketsStartTime {_ticketsStartTime}");
			return _ticketsMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _ticketsStartTime) * 1000f);
		}
		set
		{
			_ticketsMilliseconds = value;
			_ticketsStartTime = Time.realtimeSinceStartup;
			StartTicketsTimer();
			Broadcast("TICKETS_MILLISECONDS_CHANGE");
		}
	}

	public long ticketsCooldown
	{
		get
		{
			D.Log($"return _ticketsCooldown > {_ticketsCooldown}");
			return _ticketsCooldown;
		}
		set
		{
			_ticketsCooldown = value;
			D.Log($"_ticketsCooldown = value > {_ticketsCooldown}");
		}
	}

	public int changename
	{
		get
		{
			return _changename;
		}
		set
		{
			_changename = value;
			Broadcast("CHANGENAME_CHANGE");
		}
	}

	public long changenameCooldown
	{
		get
		{
			return _changenameCooldown;
		}
		set
		{
			_changenameCooldown = value;
		}
	}

	public int shards
	{
		get
		{
			return _shards;
		}
		set
		{
			_shards = value;
			Broadcast("SHARDS_CHANGE");
		}
	}

	public int shardsMax => (int)(VariableBook.shardsMax + Mathf.Round(GameModifier.getTypeTotal(getModifiers(), 39)));

	public int shardsSeconds => _shardsSeconds;

	public long shardsMilliseconds
	{
		get
		{
			return _shardsMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _shardsStartTime) * 1000f);
		}
		set
		{
			_shardsMilliseconds = value;
			_shardsStartTime = Time.realtimeSinceStartup;
			StartShardsTimer();
			Broadcast("SHARDS_MILLISECONDS_CHANGE");
		}
	}

	public long shardsCooldown
	{
		get
		{
			return _shardsCooldown;
		}
		set
		{
			_shardsCooldown = value;
		}
	}

	public int tokens
	{
		get
		{
			return _tokens;
		}
		set
		{
			_tokens = value;
			Broadcast("TOKENS_CHANGE");
		}
	}

	public int tokensMax => (int)(VariableBook.tokensMax + Mathf.Round(GameModifier.getTypeTotal(getModifiers(), 40)));

	public int tokensSeconds => _tokensSeconds;

	public long tokensMilliseconds
	{
		get
		{
			return _tokensMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _tokensStartTime) * 1000f);
		}
		set
		{
			_tokensMilliseconds = value;
			_tokensStartTime = Time.realtimeSinceStartup;
			StartTokensTimer();
			Broadcast("TOKENS_MILLISECONDS_CHANGE");
		}
	}

	public long tokensCooldown
	{
		get
		{
			return _tokensCooldown;
		}
		set
		{
			_tokensCooldown = value;
		}
	}

	public int badges
	{
		get
		{
			return _badges;
		}
		set
		{
			_badges = value;
			Broadcast("BADGES_CHANGE");
		}
	}

	public int badgesMax => (int)(VariableBook.badgesMax + Mathf.Round(GameModifier.getTypeTotal(getModifiers(), 41)));

	public int badgesSeconds => _badgesSeconds;

	public long badgesMilliseconds
	{
		get
		{
			return _badgesMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _badgesStartTime) * 1000f);
		}
		set
		{
			_badgesMilliseconds = value;
			_badgesStartTime = Time.realtimeSinceStartup;
			StartBadgesTimer();
			Broadcast("BADGES_MILLISECONDS_CHANGE");
		}
	}

	public long badgesCooldown
	{
		get
		{
			return _badgesCooldown;
		}
		set
		{
			_badgesCooldown = value;
		}
	}

	public int shopRotationID
	{
		get
		{
			return _shopRotationID;
		}
		set
		{
			_shopRotationID = value;
			Broadcast("SHOP_ROTATION_ID_CHANGE");
		}
	}

	public int shopRotationSeconds => _shopRotationSeconds;

	public long shopRotationMilliseconds
	{
		get
		{
			return _shopRotationMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _shopRotationStartTime) * 1000f);
		}
		set
		{
			_shopRotationMilliseconds = value;
			_shopRotationStartTime = Time.realtimeSinceStartup;
			startShopRotationTimer();
			Broadcast("SHOP_ROTATION_MILLISECONDS_CHANGE");
		}
	}

	public long adMilliseconds
	{
		get
		{
			return _adMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _adStartTime) * 1000f);
		}
		set
		{
			_adMilliseconds = value;
			_adStartTime = Time.realtimeSinceStartup;
			startAdTimer();
			Broadcast("AD_MILLISECONDS_CHANGE");
		}
	}

	public ItemData adItem
	{
		get
		{
			return _adItem;
		}
		set
		{
			_adItem = value;
		}
	}

	public DateTime? nbpDate
	{
		get
		{
			return _nbpDate;
		}
		set
		{
			if (value.HasValue)
			{
				_nbpDate = value.Value;
			}
			else
			{
				_nbpDate = null;
			}
			Broadcast("NBP_DATE_CHANGE");
		}
	}

	public long nbpMilliseconds
	{
		get
		{
			if (!_nbpDate.HasValue)
			{
				return 0L;
			}
			DateTime date = ServerExtension.instance.GetDate();
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan timeSpan = date.ToUniversalTime() - dateTime.ToUniversalTime();
			long num = (long)(_nbpDate.Value.ToUniversalTime() - dateTime.ToUniversalTime()).TotalMilliseconds;
			long num2 = (long)timeSpan.TotalMilliseconds;
			return num - num2 + VariableBook.nbpMilliseconds;
		}
	}

	public bool admin
	{
		get
		{
			return _admin;
		}
		set
		{
			_admin = value;
		}
	}

	public bool moderator
	{
		get
		{
			return _moderator;
		}
		set
		{
			_moderator = value;
		}
	}

	public bool autoPilot
	{
		get
		{
			return _autoPilot;
		}
		set
		{
			_autoPilot = value;
			Broadcast("AUTO_PILOT_CHANGE");
		}
	}

	public bool setAutoPilotWithoutNotify
	{
		get
		{
			return _autoPilot;
		}
		set
		{
			_autoPilot = value;
		}
	}

	public bool chatEnabled
	{
		get
		{
			return _chatEnabled;
		}
		set
		{
			_chatEnabled = value;
			Broadcast("CHAT_CHANGE");
		}
	}

	public int chatChannel
	{
		get
		{
			return _chatChannel;
		}
		set
		{
			_chatChannel = value;
			Broadcast("CHAT_CHANGE");
		}
	}

	public List<ChatPlayerData> chatIgnores
	{
		get
		{
			return _chatIgnores;
		}
		set
		{
			_chatIgnores = value;
			Broadcast("CHAT_CHANGE");
		}
	}

	public bool friendRequestsEnabled
	{
		get
		{
			return _friendRequestsEnabled;
		}
		set
		{
			_friendRequestsEnabled = value;
		}
	}

	public bool duelRequestsEnabled
	{
		get
		{
			return _duelRequestsEnabled;
		}
		set
		{
			_duelRequestsEnabled = value;
		}
	}

	public bool showHelm
	{
		get
		{
			return _showHelm;
		}
		set
		{
			_showHelm = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public bool showMount
	{
		get
		{
			return _showMount;
		}
		set
		{
			_showMount = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public bool showBody
	{
		get
		{
			return _showBody;
		}
		set
		{
			_showBody = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public bool showAccessory
	{
		get
		{
			return _showAccessory;
		}
		set
		{
			_showAccessory = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public List<ItemRef> lockedItems
	{
		get
		{
			return _lockedItems;
		}
		set
		{
			_lockedItems = value;
			Broadcast("LOCKED_ITEMS_CHANGE");
		}
	}

	public List<ItemData> dungeonLootItems
	{
		get
		{
			Dungeon dungeon = GameData.instance.PROJECT.dungeon;
			if (dungeon != null)
			{
				_dungeonLootItems = GameData.instance.SAVE_STATE.GetDungeonLoot(dungeon.type, dungeon.dungeonRef.id);
			}
			return _dungeonLootItems;
		}
	}

	public long updateMilliseconds
	{
		get
		{
			return _updateMilliseconds;
		}
		set
		{
			_updateMilliseconds = value;
			setUpdateTimer();
		}
	}

	public Inventory inventory
	{
		get
		{
			return _inventory;
		}
		set
		{
			_inventory = value;
		}
	}

	public Tutorial tutorial
	{
		get
		{
			return _tutorial;
		}
		set
		{
			_tutorial = value;
		}
	}

	public Equipment equipment
	{
		get
		{
			return _equipment;
		}
		set
		{
			_equipment = value;
		}
	}

	public Runes runes
	{
		get
		{
			return _runes;
		}
		set
		{
			_runes = value;
			Broadcast("RUNES_CHANGE");
		}
	}

	public Enchants enchants
	{
		get
		{
			return _enchants;
		}
		set
		{
			_enchants = value;
			Broadcast("ENCHANTS_CHANGE");
		}
	}

	public Augments augments
	{
		get
		{
			return _augments;
		}
		set
		{
			_augments = value;
			Broadcast("AUGMENTS_CHANGE");
		}
	}

	public Mounts mounts
	{
		get
		{
			return _mounts;
		}
		set
		{
			_mounts = value;
			Broadcast("MOUNTS_CHANGE");
		}
	}

	public Armory armory
	{
		get
		{
			return _armory;
		}
		set
		{
			_armory = value;
		}
	}

	public Zones zones
	{
		get
		{
			return _zones;
		}
		set
		{
			_zones = value;
		}
	}

	public Teams teams
	{
		get
		{
			return _teams;
		}
		set
		{
			_teams = value;
		}
	}

	public DailyQuests dailyQuests
	{
		get
		{
			return _dailyQuests;
		}
		set
		{
			_dailyQuests = value;
			Broadcast("DAILY_QUEST_CHANGE");
		}
	}

	public CharacterAchievements characterAchievements
	{
		get
		{
			return _characterAchievements;
		}
		set
		{
			_characterAchievements = value;
		}
	}

	public FamiliarStable familiarStable
	{
		get
		{
			return _familiarStable;
		}
		set
		{
			_familiarStable = value;
			Broadcast("FAMILIAR_STABLE_CHANGE");
		}
	}

	public List<int> platforms
	{
		get
		{
			return _platforms;
		}
		set
		{
			_platforms = value;
		}
	}

	public List<TeammateData> selectedTeammates
	{
		get
		{
			return _selectedTeammates;
		}
		set
		{
			_selectedTeammates = value;
		}
	}

	public List<int> eventsWon
	{
		get
		{
			return _eventsWon;
		}
		set
		{
			_eventsWon = value;
		}
	}

	public List<FriendData> friends
	{
		get
		{
			return _friends;
		}
		set
		{
			_friends = value;
			Broadcast("FRIEND_CHANGE");
		}
	}

	public List<RequestData> requests
	{
		get
		{
			return _requests;
		}
		set
		{
			_requests = value;
			Broadcast("REQUEST_CHANGE");
		}
	}

	public List<ConsumableModifierData> consumableModifiers
	{
		get
		{
			return _consumableModifiers;
		}
		set
		{
			_consumableModifiers = value;
			Broadcast("CONSUMABLE_MODIFIER_CHANGE");
		}
	}

	public CharacterGuildData guildData
	{
		get
		{
			return _guildData;
		}
		set
		{
			_guildData = value;
			Broadcast("GUILD_CHANGE");
		}
	}

	public string customconsum
	{
		get
		{
			return _customconsum;
		}
		set
		{
			_customconsum = value;
			Broadcast("APPEARANCE_CHANGE");
		}
	}

	public int pvpEventLootID
	{
		get
		{
			return _pvpEventLootID;
		}
		set
		{
			_pvpEventLootID = value;
		}
	}

	public int riftEventLootID
	{
		get
		{
			return _riftEventLootID;
		}
		set
		{
			_riftEventLootID = value;
		}
	}

	public int gauntletEventLootID
	{
		get
		{
			return _gauntletEventLootID;
		}
		set
		{
			_gauntletEventLootID = value;
		}
	}

	public int gvgEventLootID
	{
		get
		{
			return _gvgEventLootID;
		}
		set
		{
			_gvgEventLootID = value;
		}
	}

	public int invasionEventLootID
	{
		get
		{
			return _invasionEventLootID;
		}
		set
		{
			_invasionEventLootID = value;
		}
	}

	public int fishingEventLootID
	{
		get
		{
			return _fishingEventLootID;
		}
		set
		{
			_fishingEventLootID = value;
		}
	}

	public int gveEventLootID
	{
		get
		{
			return _gveEventLootID;
		}
		set
		{
			_gveEventLootID = value;
		}
	}

	public BHAnalytics analytics
	{
		get
		{
			if (_analytics == null)
			{
				_analytics = new BHAnalytics(id);
			}
			return _analytics;
		}
		set
		{
			_analytics = value;
		}
	}

	public CharacterExtraInfo extraInfo
	{
		get
		{
			return _extraInfo;
		}
		set
		{
			_extraInfo = value;
		}
	}

	public List<BoosterRef> activeBoosters => _activeBoosters;

	public List<BoosterRef> passiveBoosters => _passiveBoosters;

	public bool? lastBrawlPrivateCheckbox
	{
		get
		{
			return _lastBrawlPrivateCheckbox;
		}
		set
		{
			_lastBrawlPrivateCheckbox = value;
		}
	}

	public IMXG0Data imxG0Data
	{
		get
		{
			return _imxG0Data;
		}
		set
		{
			_imxG0Data = value;
		}
	}

	public NFTState nftState => (NFTState)_nftState;

	public string nftToken => _nftToken;

	public Character(int id, int playerID)
	{
		_id = id;
		_playerID = playerID;
	}

	public int getItemQty(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return 0;
		}
		if (itemRef.itemType == 3)
		{
			return itemRef.id switch
			{
				1 => gold, 
				2 => credits, 
				3 => 0, 
				4 => energy, 
				5 => tickets, 
				12 => changename, 
				6 => points, 
				8 => shards, 
				13 => extraInfo.seals, 
				10 => badges, 
				_ => 0, 
			};
		}
		return _inventory.getItem(itemRef.id, itemRef.itemType)?.qty ?? 0;
	}

	public void removeItems(List<ItemData> items)
	{
		if (items == null || items.Count <= 0)
		{
			return;
		}
		foreach (ItemData item in items)
		{
			if (item != null)
			{
				removeItem(item, dispatch: false);
			}
		}
		Broadcast("INVENTORY_CHANGE");
	}

	public void removeItem(ItemData itemData, bool dispatch = true)
	{
		if (itemData == null)
		{
			return;
		}
		switch (itemData.itemRef.itemType)
		{
		case 3:
			switch (itemData.itemRef.id)
			{
			case 1:
				gold -= itemData.qty;
				break;
			case 2:
				credits -= itemData.qty;
				break;
			case 3:
				exp -= itemData.qty;
				break;
			case 4:
				energy -= itemData.qty;
				break;
			case 5:
				tickets -= itemData.qty;
				break;
			case 12:
				changename -= itemData.qty;
				break;
			case 6:
				points -= itemData.qty;
				break;
			case 8:
				shards -= itemData.qty;
				break;
			case 13:
				extraInfo.seals -= itemData.qty;
				break;
			case 10:
				badges -= itemData.qty;
				break;
			}
			break;
		default:
		{
			ItemData item = inventory.getItem(itemData.itemRef.id, itemData.itemRef.itemType);
			if (item != null)
			{
				item.qty -= itemData.qty;
			}
			break;
		}
		case 10:
			break;
		}
		if (dispatch)
		{
			Broadcast("INVENTORY_CHANGE");
		}
	}

	public void updateInventoryItems(List<ItemData> items)
	{
		if (items == null || items.Count <= 0)
		{
			return;
		}
		foreach (ItemData item2 in items)
		{
			if (item2 != null)
			{
				ItemData item = inventory.getItem(item2.itemRef.id, item2.itemRef.itemType, int.MinValue);
				if (item != null)
				{
					item.qty = item2.qty;
				}
				else
				{
					inventory.insertItem(item2.itemRef, item2.qty);
				}
			}
		}
		Broadcast("INVENTORY_CHANGE");
	}

	public void addItems(List<ItemData> items, bool isDungeonLoot = false)
	{
		if (items == null || items.Count <= 0)
		{
			return;
		}
		foreach (ItemData item in items)
		{
			if (item != null)
			{
				addItem(item, dispatch: false, isDungeonLoot);
			}
		}
		Broadcast("INVENTORY_CHANGE");
	}

	public void addItem(ItemData itemData, bool dispatch = true, bool isDungeonLoot = false)
	{
		switch (itemData.itemRef.itemType)
		{
		case 3:
			switch (itemData.itemRef.id)
			{
			case 1:
				gold += itemData.qty;
				break;
			case 2:
				credits += itemData.qty;
				break;
			case 3:
				exp += itemData.qty;
				break;
			case 4:
				energy += itemData.qty;
				break;
			case 5:
				tickets += itemData.qty;
				break;
			case 12:
				changename += itemData.qty;
				break;
			case 6:
				points += itemData.qty;
				break;
			case 8:
				shards += itemData.qty;
				break;
			case 13:
				extraInfo.seals += itemData.qty;
				break;
			case 10:
				badges += itemData.qty;
				break;
			case 9:
				tokens += itemData.qty;
				break;
			}
			break;
		default:
		{
			ItemData item = inventory.getItem(itemData.itemRef.id, itemData.itemRef.itemType, int.MinValue);
			if (item != null)
			{
				item.qty += itemData.qty;
			}
			else
			{
				inventory.insertItem(itemData.itemRef, itemData.qty);
			}
			break;
		}
		case 10:
			break;
		}
		if (GameData.instance.PROJECT.dungeon != null && isDungeonLoot)
		{
			addDungeonLoot(itemData, GameData.instance.PROJECT.dungeon.type, GameData.instance.PROJECT.dungeon.dungeonRef.id);
		}
		if (dispatch)
		{
			Broadcast("INVENTORY_CHANGE");
		}
	}

	public void addDungeonLoot(ItemData itemData, int dungeonType, int dungeonId)
	{
		if (_dungeonLootItems == null || _dungeonLootItems.Count == 0)
		{
			_dungeonLootItems = GameData.instance.SAVE_STATE.GetDungeonLoot(dungeonType, dungeonId);
		}
		ItemData itemData2 = _dungeonLootItems.Find((ItemData i) => i.itemRef == itemData.itemRef);
		if (itemData2 != null)
		{
			itemData2.qty += itemData.qty;
		}
		else
		{
			_dungeonLootItems.Add(itemData);
		}
		GameData.instance.SAVE_STATE.SetDungeonLoot(_dungeonLootItems, dungeonType, dungeonId);
	}

	public void clearDungeonLootItems()
	{
		if (_dungeonLootItems != null)
		{
			_dungeonLootItems.Clear();
		}
		GameData.instance.SAVE_STATE.SetDungeonLoot(_dungeonLootItems, -1, -1);
	}

	public bool hasPlatform(int platform)
	{
		foreach (int platform2 in _platforms)
		{
			if (platform2 == platform)
			{
				return true;
			}
		}
		return false;
	}

	public void addPlatform(int platform)
	{
		if (!hasPlatform(platform))
		{
			_platforms.Add(platform);
		}
	}

	public List<FriendData> getFriendsOnline()
	{
		List<FriendData> list = new List<FriendData>();
		foreach (FriendData friend in _friends)
		{
			if (friend.online)
			{
				list.Add(friend);
			}
		}
		return list;
	}

	public FriendData getFriendData(int charID, int teamType = -1, bool duplicateData = true)
	{
		foreach (FriendData friend in _friends)
		{
			if (friend.characterData.charID != charID)
			{
				continue;
			}
			if (!duplicateData)
			{
				if (teamType == 6)
				{
					return new FriendData(friend.characterData.Duplicate(), friend.online);
				}
				return friend;
			}
			return new FriendData(friend.characterData.Duplicate(), friend.online);
		}
		return null;
	}

	public void addFriendData(FriendData friendData)
	{
		if (getFriendData(friendData.characterData.charID) == null)
		{
			_friends.Add(friendData);
			Broadcast("FRIEND_CHANGE");
		}
	}

	public void removeFriendData(int charID)
	{
		FriendData friendData = _friends.Find((FriendData x) => x.characterData.charID == charID);
		if (friendData == null)
		{
			return;
		}
		int playerID = friendData.characterData.playerID;
		List<FriendData> list = _friends.FindAll((FriendData x) => x.characterData.playerID == playerID);
		int count = _friends.Count;
		foreach (FriendData item in list)
		{
			_friends.Remove(item);
		}
		if (count > _friends.Count)
		{
			Broadcast("FRIEND_CHANGE");
		}
	}

	public RequestData getRequestData(int charID)
	{
		foreach (RequestData request in _requests)
		{
			if (request.characterData.charID == charID)
			{
				return request;
			}
		}
		return null;
	}

	public void addRequestData(RequestData requestData)
	{
		if (getRequestData(requestData.characterData.charID) == null)
		{
			_requests.Add(requestData);
			Broadcast("REQUEST_CHANGE");
		}
	}

	public void removeRequestData(int charID)
	{
		for (int i = 0; i < _requests.Count; i++)
		{
			if (_requests[i].characterData.charID == charID)
			{
				_requests.RemoveAt(i);
				Broadcast("REQUEST_CHANGE");
				break;
			}
		}
	}

	public void SetGuildInvite(List<GuildInfo> list)
	{
		_guildInvites = list;
	}

	public void RemoveGuildInvite(int guildID)
	{
		for (int i = 0; i < _guildInvites.Count; i++)
		{
			if (_guildInvites[i].id == guildID)
			{
				_guildInvites.RemoveAt(i);
				GameData.instance.PROJECT.character.Broadcast("GUILD_INVITE_CHANGE");
				break;
			}
		}
	}

	public bool GuildInviteExists(int guildID)
	{
		foreach (GuildInfo guildInvite in _guildInvites)
		{
			if (guildID == guildInvite.id)
			{
				return true;
			}
		}
		return false;
	}

	public UserData getContact(int charID, int teamType = -1)
	{
		FriendData friendData = getFriendData(charID, teamType);
		if (friendData != null)
		{
			return friendData;
		}
		if (_guildData != null)
		{
			GuildMemberData member = _guildData.getMember(charID, teamType);
			if (member != null)
			{
				return member;
			}
		}
		return null;
	}

	public List<TeammateData> getTeammates(TeamRules teamRules, bool forceCalculate = false)
	{
		List<TeammateData> list = new List<TeammateData>();
		if (teamRules.allowFriends)
		{
			foreach (FriendData friend in _friends)
			{
				if (friend.characterData.charID != GameData.instance.PROJECT.character.id)
				{
					list.Add(new TeammateData(friend.characterData.charID, 1, -1L, forceCalculate));
				}
			}
		}
		if (teamRules.allowGuildmates && _guildData != null)
		{
			foreach (GuildMemberData member in _guildData.members)
			{
				if (!TeammateData.listHasTeammate(list, member.characterData.charID, 1) && member.characterData.charID != GameData.instance.PROJECT.character.id)
				{
					list.Add(new TeammateData(member.characterData.charID, 1, -1L));
				}
			}
		}
		if (teamRules.allowFamiliars)
		{
			foreach (ItemData item in _inventory.GetItemsByType(6))
			{
				for (int i = 0; i < item.qty; i++)
				{
					list.Add(new TeammateData(item.itemRef.id, 2, -1L));
				}
			}
		}
		if (teamRules.familiarsAdded != null)
		{
			foreach (FamiliarRef item2 in teamRules.familiarsAdded)
			{
				list.Add(new TeammateData(item2.id, 2, -1L));
			}
		}
		return Util.SortVector(list, new string[1] { "total" }, Util.ARRAY_DESCENDING);
	}

	public float getGameModifierValueTotal(int type)
	{
		return GameModifier.getTypeTotal(getModifiers(), type);
	}

	public bool getPlayerOnline(int charID)
	{
		FriendData friendData = getFriendData(charID);
		if (friendData != null)
		{
			return friendData.online;
		}
		if (_guildData != null)
		{
			GuildMemberData member = _guildData.getMember(charID);
			if (member != null)
			{
				return member.online;
			}
		}
		return false;
	}

	public void setPlayerOnline(int charID, bool online)
	{
		FriendData friendData = getFriendData(charID, -1, duplicateData: false);
		if (friendData != null)
		{
			friendData.online = online;
			Broadcast("FRIEND_CHANGE");
		}
		if (_guildData != null)
		{
			GuildMemberData member = _guildData.getMember(charID, -1, duplicateData: false);
			if (member != null)
			{
				member.online = online;
				Broadcast("GUILD_MEMBER_CHANGE");
			}
		}
	}

	public Conversation addConversation(Conversation conversation)
	{
		_conversations.Add(conversation);
		return conversation;
	}

	public Conversation getConversation(int charID)
	{
		foreach (Conversation conversation in _conversations)
		{
			if (conversation.charID == charID)
			{
				return conversation;
			}
		}
		return null;
	}

	public ZoneRef getZoneRef()
	{
		ZoneRef zoneRef = ZoneBook.Lookup(_zoneID);
		if (zoneRef != null)
		{
			return zoneRef;
		}
		return ZoneBook.GetStarterZone();
	}

	public List<AbilityRef> getAbilities()
	{
		Dictionary<int, EquipmentRef> equipmentSlots = _equipment.equipmentSlots;
		Dictionary<int, RuneRef> runeSlots = _runes.runeSlots;
		List<AbilityRef> list = new List<AbilityRef>();
		foreach (KeyValuePair<int, EquipmentRef> item in equipmentSlots)
		{
			if (item.Value == null || item.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability in item.Value.abilities)
			{
				list.Add(ability);
			}
		}
		if (list.Count <= 0)
		{
			foreach (AbilityRef item2 in VariableBook.abilitiesDefault)
			{
				list.Add(item2);
			}
		}
		foreach (KeyValuePair<int, RuneRef> item3 in runeSlots)
		{
			if (item3.Value.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability2 in item3.Value.abilities)
			{
				list.Add(ability2);
			}
		}
		foreach (EquipmentSetBonusRef equippedSetBonuse in _equipment.getEquippedSetBonuses())
		{
			if (equippedSetBonuse == null || equippedSetBonuse.abilities == null)
			{
				continue;
			}
			foreach (AbilityRef ability3 in equippedSetBonuse.abilities)
			{
				list.Add(ability3);
			}
		}
		MountData mountEquipped = _mounts.getMountEquipped();
		if (mountEquipped != null && mountEquipped.mountRef.abilities != null)
		{
			foreach (AbilityRef ability4 in mountEquipped.mountRef.abilities)
			{
				list.Add(ability4);
			}
		}
		List<AbilityRef> equippedModifierConditionWithAbilities = _equipment.getEquippedModifierConditionWithAbilities();
		if (equippedModifierConditionWithAbilities != null && equippedModifierConditionWithAbilities.Count > 0)
		{
			foreach (AbilityRef item4 in equippedModifierConditionWithAbilities)
			{
				list.Add(item4);
			}
			return list;
		}
		return list;
	}

	public CharacterData toCharacterData(bool duplicateMounts = false)
	{
		Mounts mounts = _mounts;
		if (duplicateMounts)
		{
			long equipped = 0L;
			if (_mounts.getMountEquipped() != null)
			{
				equipped = _mounts.getMountEquipped().uid;
			}
			mounts = new Mounts(equipped, _mounts.cosmetic, _mounts.mounts);
		}
		Equipment equipment = _equipment;
		Runes runes = _runes;
		Enchants enchants = _enchants;
		bool flag = false;
		if (armory.battleArmorySelected)
		{
			equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(armory.currentArmoryEquipmentSlot);
			runes = new Runes(armory.currentArmoryEquipmentSlot.runes.runeSlots);
			enchants = new Enchants(armory.currentArmoryEquipmentSlot.enchants.slots, armory.currentArmoryEquipmentSlot.enchants.enchants);
			MountData mountData = null;
			MountRef mountRef = null;
			mounts.setCosmetic(null);
			for (int i = 0; i < mounts.mounts.Count; i++)
			{
				MountData mountData2 = mounts.mounts[i];
				if (mountData2.uid == armory.currentArmoryEquipmentSlot.mount)
				{
					mountData = mountData2;
					flag = true;
				}
				if (mountData2.mountRef.id == armory.currentArmoryEquipmentSlot.mountCosmetic)
				{
					mountRef = mountData2.mountRef;
				}
			}
			if (mountRef == null)
			{
				for (int j = 0; j <= MountBook.size; j++)
				{
					MountRef mountRef2 = MountBook.Lookup(j);
					if (!(mountRef2 == null) && mountRef2.cosmetic && mountRef2.id == armory.currentArmoryEquipmentSlot.mountCosmetic)
					{
						mountRef = mountRef2;
						break;
					}
				}
			}
			if (mountData != null)
			{
				mounts.setEquipped(mountData, doDispatch: false);
			}
			if (mountRef != null)
			{
				mounts.setCosmetic(mountRef);
			}
		}
		CharacterData characterData = ((imxG0Data == null) ? new CharacterData(new CharacterPuppetInfoDefault(CharacterPuppet.ParseGenderFromString(_gender), _hairID, _hairColorID, _skinColorID, 1f, 1f, equipment, mounts, _showHelm, _showMount, _showBody, _showAccessory), _id, _playerID, _name, _herotag, _nameHasChanged, _customconsum, _level, _power, _stamina, _agility, _zoneCompleted, runes, enchants, (_guildData != null) ? _guildData.toGuildInfo() : null, null, hasVipgor: false) : new CharacterData(new CharacterPuppetInfoIMXG0(imxG0Data.puppet, imxG0Data.card, imxG0Data.rarity, imxG0Data.name, CharacterPuppet.ParseGenderFromString(_gender), 1f, 1f, equipment, mounts, _showHelm, _showMount, _showBody, _showAccessory), _id, _playerID, _name, _herotag, _nameHasChanged, _customconsum, _level, _power, _stamina, _agility, _zoneCompleted, runes, enchants, (_guildData != null) ? _guildData.toGuildInfo() : null, null, hasVipgor: false, _nftToken, _nftState));
		characterData.setArmory(_armory);
		if (armory.battleArmorySelected)
		{
			characterData.showMount = flag;
		}
		return characterData;
	}

	public CharacterDisplay toCharacterDisplay(float scale = 3f, bool displayMount = false, bool enableLoading = true)
	{
		CharacterPuppetInfo characterPuppetInfo = ((imxG0Data == null) ? ((CharacterPuppetInfo)new CharacterPuppetInfoDefault(CharacterPuppet.ParseGenderFromString(_gender), _hairID, _hairColorID, _skinColorID, scale, 1f, _equipment, _mounts, _showHelm, displayMount && _showMount, _showBody, _showAccessory, null, enableLoading)) : ((CharacterPuppetInfo)new CharacterPuppetInfoIMXG0(imxG0Data.puppet, imxG0Data.card, imxG0Data.rarity, imxG0Data.name, CharacterPuppet.ParseGenderFromString(_gender), scale, 1f, _equipment, _mounts, _showHelm, displayMount && _showMount, _showBody, _showAccessory, null, enableLoading)));
		CharacterDisplay characterDisplay = GameData.instance.windowGenerator.GetCharacterDisplay(characterPuppetInfo);
		characterDisplay.SetCharacterDisplay(characterPuppetInfo);
		return characterDisplay;
	}

	public int getTotalStats()
	{
		return getTotalPower() + getTotalStamina() + getTotalAgility();
	}

	public int getTotalPower()
	{
		return getTotalStat(0);
	}

	public int getTotalStamina()
	{
		return getTotalStat(1);
	}

	public int getTotalAgility()
	{
		return getTotalStat(2);
	}

	public int getTotalStatsForEquipment(Equipment e)
	{
		return getTotalPowerForEquipment(e) + getTotalStaminaForEquipment(e) + getTotalAgilityForEquipment(e);
	}

	public int getTotalPowerForEquipment(Equipment e)
	{
		return getTotalStatForEquipment(0, e);
	}

	public int getTotalStaminaForEquipment(Equipment e)
	{
		return getTotalStatForEquipment(1, e);
	}

	public int getTotalAgilityForEquipment(Equipment e)
	{
		return getTotalStatForEquipment(2, e);
	}

	public int getTotalStat(int stat)
	{
		int num = 0;
		switch (stat)
		{
		case 0:
			num += _power + VariableBook.characterBase.power;
			break;
		case 1:
			num += _stamina + VariableBook.characterBase.stamina;
			break;
		case 2:
			num += _agility + VariableBook.characterBase.agility;
			break;
		}
		num += _equipment.getStatTotal(stat);
		num += _enchants.getStatTotal(stat);
		MountData mountEquipped = _mounts.getMountEquipped();
		if (mountEquipped != null)
		{
			num += mountEquipped.getTotalStat(stat, tier);
		}
		return num;
	}

	public int getTotalStatForEquipment(int stat, Equipment equipment)
	{
		int num = 0;
		switch (stat)
		{
		case 0:
			num += _power + VariableBook.characterBase.power;
			break;
		case 1:
			num += _stamina + VariableBook.characterBase.stamina;
			break;
		case 2:
			num += _agility + VariableBook.characterBase.agility;
			break;
		}
		num += equipment.getStatTotal(stat);
		num += _enchants.getStatTotal(stat);
		MountData mountEquipped = _mounts.getMountEquipped();
		if (mountEquipped != null)
		{
			num += mountEquipped.getTotalStat(stat, tier);
		}
		return num;
	}

	public int getArmoryTotalStats(ArmoryEquipment armoryEquip)
	{
		return getArmoryTotalPower(armoryEquip) + getArmoryTotalStamina(armoryEquip) + getArmoryTotalAgility(armoryEquip);
	}

	public int getArmoryTotalPower(ArmoryEquipment armoryEquip)
	{
		return getArmoryTotalStat(0, armoryEquip);
	}

	public int getArmoryTotalStamina(ArmoryEquipment armoryEquip)
	{
		return getArmoryTotalStat(1, armoryEquip);
	}

	public int getArmoryTotalAgility(ArmoryEquipment armoryEquip)
	{
		return getArmoryTotalStat(2, armoryEquip);
	}

	public int getArmoryTotalStat(int stat, ArmoryEquipment armoryEquip)
	{
		int num = 0;
		switch (stat)
		{
		case 0:
			num += _power + VariableBook.characterBase.power;
			break;
		case 1:
			num += _stamina + VariableBook.characterBase.stamina;
			break;
		case 2:
			num += _agility + VariableBook.characterBase.agility;
			break;
		}
		num += armoryEquip.GetStatTotal(stat);
		num += armoryEquip.enchants.getStatTotal(stat);
		MountData mountData = null;
		for (int i = 0; i < _mounts.mounts.Count; i++)
		{
			if (armoryEquip.mount == _mounts.mounts[i].uid)
			{
				mountData = _mounts.mounts[i];
				break;
			}
		}
		if (mountData != null)
		{
			num += mountData.getTotalStat(stat, tier);
		}
		return num;
	}

	private void StartEnergyTimer()
	{
		ClearEnergyTimer();
		if (_energy < energyMax)
		{
			_energySeconds = Mathf.RoundToInt((float)(-energyMilliseconds) / 1000f);
			if (_energySeconds <= 0)
			{
				OnEnergyTimerComplete();
			}
			else
			{
				_energyTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _energySeconds, OnEnergyTimerComplete, OnEnergyTimer);
			}
		}
	}

	private void OnEnergyTimer()
	{
		_energySeconds--;
		Broadcast("ENERGY_SECONDS_CHANGE");
	}

	private void OnEnergyTimerComplete()
	{
		int energyGain = VariableBook.energyGain;
		int num = _energy + energyGain;
		if (num > energyMax)
		{
			num = energyMax;
		}
		energy = num;
		energyMilliseconds -= energyCooldown;
		StartEnergyTimer();
		Broadcast("ENERGY_CHANGE");
	}

	private void ClearEnergyTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _energyTimer);
	}

	private void StartTicketsTimer()
	{
		ClearTicketsTimer();
		if (_tickets < ticketsMax)
		{
			_ticketsSeconds = Mathf.RoundToInt((float)(-ticketsMilliseconds) / 1000f);
			if (_ticketsSeconds <= 0)
			{
				OnTicketsTimerComplete();
			}
			else
			{
				_ticketsTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _ticketsSeconds, null, OnTicketsTimer);
			}
		}
	}

	private void OnTicketsTimer()
	{
		_ticketsSeconds--;
		if (_ticketsSeconds <= 0)
		{
			OnTicketsTimerComplete();
		}
		else
		{
			Broadcast("TICKETS_SECONDS_CHANGE");
		}
	}

	private void OnTicketsTimerComplete()
	{
		int ticketsGain = VariableBook.ticketsGain;
		int num = _tickets + ticketsGain;
		if (num > ticketsMax)
		{
			num = ticketsMax;
		}
		tickets = num;
		ticketsMilliseconds -= ticketsCooldown;
		StartTicketsTimer();
		Broadcast("TICKETS_CHANGE");
	}

	private void ClearTicketsTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _ticketsTimer);
	}

	private void StartShardsTimer()
	{
		ClearShardsTimer();
		if (_shards < shardsMax)
		{
			_shardsSeconds = Mathf.RoundToInt((float)(-shardsMilliseconds) / 1000f);
			if (_shardsSeconds <= 0)
			{
				OnShardsTimerComplete();
			}
			else
			{
				_shardsTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _shardsSeconds, null, OnShardsTimer);
			}
		}
	}

	private void OnShardsTimer()
	{
		_shardsSeconds--;
		if (_shardsSeconds <= 0)
		{
			OnShardsTimerComplete();
		}
		else
		{
			Broadcast("SHARDS_SECONDS_CHANGE");
		}
	}

	private void OnShardsTimerComplete()
	{
		int shardsGain = VariableBook.shardsGain;
		int num = _shards + shardsGain;
		if (num > shardsMax)
		{
			num = shardsMax;
		}
		shards = num;
		shardsMilliseconds -= shardsCooldown;
		StartShardsTimer();
		Broadcast("SHARDS_CHANGE");
	}

	private void ClearShardsTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _shardsTimer);
	}

	private void StartTokensTimer()
	{
		ClearTokensTimer();
		if (_tokens < tokensMax)
		{
			_tokensSeconds = Mathf.RoundToInt((float)(-tokensMilliseconds) / 1000f);
			if (_tokensSeconds <= 0)
			{
				OnTokensTimerComplete();
			}
			else
			{
				_tokensTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _tokensSeconds, null, OnTokensTimer);
			}
		}
	}

	private void OnTokensTimer()
	{
		_tokensSeconds--;
		if (_tokensSeconds <= 0)
		{
			OnTokensTimerComplete();
		}
		else
		{
			Broadcast("TOKENS_SECONDS_CHANGE");
		}
	}

	private void OnTokensTimerComplete()
	{
		int tokensGain = VariableBook.tokensGain;
		int num = _tokens + tokensGain;
		if (num > tokensMax)
		{
			num = tokensMax;
		}
		tokens = num;
		tokensMilliseconds -= tokensCooldown;
		StartTokensTimer();
		Broadcast("TOKENS_CHANGE");
	}

	private void ClearTokensTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _tokensTimer);
	}

	private void OnBadgesTimer()
	{
		_badgesSeconds--;
		if (_badgesSeconds <= 0)
		{
			OnBadgesTimerComplete();
		}
		else
		{
			Broadcast("BADGES_SECONDS_CHANGE");
		}
	}

	private void OnBadgesTimerComplete()
	{
		int badgesGain = VariableBook.badgesGain;
		int num = _badges + badgesGain;
		if (num > badgesMax)
		{
			num = badgesMax;
		}
		badges = num;
		badgesMilliseconds -= badgesCooldown;
		StartBadgesTimer();
		Broadcast("BADGES_CHANGE");
	}

	private void StartBadgesTimer()
	{
		ClearBadgesTimer();
		if (_badges < badgesMax)
		{
			_badgesSeconds = Mathf.RoundToInt((float)(-badgesMilliseconds) / 1000f);
			if (_badgesSeconds <= 0)
			{
				OnBadgesTimerComplete();
			}
			else
			{
				_badgesTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _badgesSeconds, null, OnBadgesTimer);
			}
		}
	}

	private void ClearBadgesTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _badgesTimer);
	}

	private void startShopRotationTimer()
	{
		_shopRotationSeconds = Mathf.RoundToInt((float)(-shopRotationMilliseconds) / 1000f);
		if (_shopRotationSeconds <= 0)
		{
			onShopRotationTimerComplete();
		}
		else
		{
			_shopRotationTimer = GameData.instance.main.coroutineTimer?.AddTimer(null, 1000f, _shopRotationSeconds, onShopRotationTimerComplete, onShopRotationTimer);
		}
	}

	private void onShopRotationTimer()
	{
		_shopRotationSeconds--;
		Broadcast("SHOP_ROTATION_SECONDS_CHANGE");
		if (_shopRotationSeconds == 0)
		{
			onShopRotationTimerComplete();
		}
	}

	private void onShopRotationTimerComplete()
	{
		doUpdateShopRotation();
	}

	private void doUpdateShopRotation()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), onUpdateShopRotation);
		CharacterDALC.instance.doUpdateShopRotation();
	}

	private void onUpdateShopRotation(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		shopRotationID = sfsob.GetInt("cha41");
		shopRotationMilliseconds = sfsob.GetLong("cha42");
	}

	private void clearAdTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _adTimer);
	}

	private void startAdTimer()
	{
		clearAdTimer();
		if (adMilliseconds >= 0)
		{
			onAdTimerComplete();
		}
		else
		{
			GameData.instance.main.coroutineTimer?.AddTimer(null, VariableBook.adRefreshMilliseconds, onAdTimerComplete);
		}
	}

	private void onAdTimerComplete()
	{
		Broadcast("AD_READY");
	}

	public void SendAdReadyMessage()
	{
		GameData.instance.main.AddBreadcrumb("SendAdReadyMessage - Ad Ready");
		Broadcast("AD_READY");
	}

	public ChatPlayerData getChatIgnore(int charID)
	{
		foreach (ChatPlayerData chatIgnore in _chatIgnores)
		{
			if (chatIgnore.charID == charID)
			{
				return chatIgnore;
			}
		}
		return null;
	}

	public void addChatIgnore(ChatPlayerData chatData)
	{
		if (getChatIgnore(chatData.charID) == null)
		{
			_chatIgnores.Add(chatData);
			Broadcast("CHAT_CHANGE");
		}
	}

	public void removeChatIgnore(int charID)
	{
		for (int i = 0; i < _chatIgnores.Count; i++)
		{
			if (_chatIgnores[i].charID == charID)
			{
				_chatIgnores.RemoveAt(i);
				Broadcast("CHAT_CHANGE");
				break;
			}
		}
	}

	public List<ConsumableModifierData> getConsumableModifiersWithMatchingModifiers(List<GameModifier> modifiers)
	{
		List<ConsumableModifierData> list = new List<ConsumableModifierData>();
		foreach (ConsumableModifierData consumableModifier in _consumableModifiers)
		{
			if (VariableBook.adgorRef.IsAdgorConsumable(consumableModifier.consumableRef.id))
			{
				continue;
			}
			foreach (GameModifier modifier in consumableModifier.consumableRef.modifiers)
			{
				foreach (GameModifier modifier2 in modifiers)
				{
					if (modifier.type == modifier2.type && !ConsumableModifierData.listContainsData(list, consumableModifier))
					{
						list.Add(consumableModifier);
					}
				}
			}
		}
		return list;
	}

	public EquipmentRef GetUpgradeEquipment()
	{
		List<ItemData> itemsByType = _inventory.GetItemsByType(1);
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = _equipment.getEquipmentSlot(i);
			foreach (ItemData item in itemsByType)
			{
				int itemType = item.itemRef.itemType;
				if ((itemType != 1 && itemType != 16) || item.rarity == 8)
				{
					continue;
				}
				EquipmentRef equipmentRef = item.itemRef as EquipmentRef;
				if (Equipment.getAvailableSlot(equipmentRef.equipmentType) == i)
				{
					if (equipmentSlot == null)
					{
						return equipmentRef;
					}
					if (equipmentRef.total > equipmentSlot.total)
					{
						return equipmentRef;
					}
				}
			}
		}
		return null;
	}

	public int getFamiliarCount()
	{
		int num = 0;
		foreach (FamiliarRef completeFamiliar in FamiliarBook.GetCompleteFamiliarList())
		{
			if (!(completeFamiliar == null) && completeFamiliar.obtainable && getItemQty(completeFamiliar) > 0)
			{
				num++;
			}
		}
		return num;
	}

	public List<TeammateData> getAutoAssignedTeam(TeamRules teamRules, int type)
	{
		List<TeammateData> teammates = getTeammates(teamRules, forceCalculate: true);
		teammates = (from teammate in teammates
			orderby teammate.totalCalculated descending, teammate.staminaCalculated
			select teammate).ToList();
		TeammateData item = new TeammateData(id, 1, -1L);
		teammates.Insert(0, item);
		List<TeammateData> source = teammates.FindAll((TeammateData teammate) => teammate.type == 1);
		List<TeammateData> collection = teammates.FindAll((TeammateData teammate) => teammate.type != 1);
		source = (from x in source
			group x by ((CharacterData)x.data).playerID into y
			select y.First()).ToList();
		teammates = new List<TeammateData>();
		teammates.AddRange(source);
		teammates.AddRange(collection);
		if (teammates.Count > teamRules.slots)
		{
			teammates.RemoveRange(teamRules.slots, teammates.Count - teamRules.slots);
		}
		bool flag = false;
		foreach (TeammateData item2 in teammates)
		{
			if (item2.id == id && item2.type == 1)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			teammates[teammates.Count - 1] = new TeammateData(id, 1, -1L);
		}
		return teammates;
	}

	public List<GameModifier> getArmoryModifiers(ArmoryEquipment armoryEquip)
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (GameModifier item in VariableBook.modifiersBase)
		{
			list.Add(item);
		}
		foreach (GameModifier modifier in VariableBook.characterBase.modifiers)
		{
			list.Add(modifier);
		}
		foreach (GameModifier modifier2 in armoryEquip.GetModifiers())
		{
			list.Add(modifier2);
		}
		list = addArmoryRuneModifiers(list, armoryEquip);
		list = addArmoryMountModifiers(list, armoryEquip);
		list = addArmoryEnchantModifiers(list, armoryEquip);
		if (GameModifier.listHasType(list, 68))
		{
			list = addRuneModifiers(list, all: false);
		}
		if (GameModifier.listHasType(list, 70))
		{
			list = addMountModifiers(list);
		}
		if (GameModifier.listHasType(list, 69))
		{
			list = addEnchantModifiers(list);
		}
		foreach (ConsumableModifierData consumableModifier in _consumableModifiers)
		{
			if (consumableModifier == null || !consumableModifier.isActive())
			{
				continue;
			}
			foreach (GameModifier modifier3 in consumableModifier.consumableRef.modifiers)
			{
				list.Add(modifier3);
			}
		}
		DailyBonusRef currentBonusRef = DailyBonusBook.GetCurrentBonusRef();
		if (currentBonusRef != null)
		{
			foreach (GameModifier modifier4 in currentBonusRef.modifiers)
			{
				list.Add(modifier4);
			}
		}
		if (_guildData != null)
		{
			foreach (GameModifier modifier5 in _guildData.perks.getModifiers())
			{
				list.Add(modifier5);
			}
			return list;
		}
		return list;
	}

	public List<GameModifier> getModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		list.AddRange(GameModifierHelper.GetGameModifierBase());
		list.AddRange(GameModifierHelper.GetGameModifierCharacterBase());
		list.AddRange(_equipment.getModifiers());
		list = addRuneModifiers(list);
		list = addMountModifiers(list);
		list = addEnchantModifiers(list);
		if (GameModifier.listHasType(list, 68))
		{
			list = addRuneModifiers(list, all: false);
		}
		if (GameModifier.listHasType(list, 70))
		{
			list = addMountModifiers(list);
		}
		if (GameModifier.listHasType(list, 69))
		{
			list = addEnchantModifiers(list);
		}
		foreach (ConsumableModifierData consumableModifier in _consumableModifiers)
		{
			if (consumableModifier != null && consumableModifier.isActive())
			{
				list.AddRange(consumableModifier.consumableRef.modifiers);
			}
		}
		DailyBonusRef currentBonusRef = DailyBonusBook.GetCurrentBonusRef();
		if (currentBonusRef != null)
		{
			list.AddRange(currentBonusRef.modifiers);
		}
		if (_guildData != null)
		{
			List<GameModifier> modifiers = _guildData.perks.getModifiers();
			if (modifiers != null)
			{
				list.AddRange(modifiers);
			}
		}
		return list;
	}

	private List<GameModifier> addArmoryMountModifiers(List<GameModifier> modifiers, ArmoryEquipment armoryEquip)
	{
		List<GameModifier> list = new List<GameModifier>();
		MountData mountData = null;
		for (int i = 0; i < _mounts.mounts.Count; i++)
		{
			if (armoryEquip.mount == _mounts.mounts[i].uid)
			{
				mountData = _mounts.mounts[i];
				break;
			}
		}
		if (mountData != null)
		{
			mountData.getGameModifiers();
			foreach (GameModifier item in list)
			{
				list.Add(item);
			}
		}
		foreach (GameModifier item2 in list)
		{
			modifiers.Add(item2);
		}
		return modifiers;
	}

	private List<GameModifier> addMountModifiers(List<GameModifier> modifiers)
	{
		foreach (GameModifier gameModifier in _mounts.getGameModifiers())
		{
			modifiers.Add(gameModifier);
		}
		return modifiers;
	}

	private List<GameModifier> addArmoryEnchantModifiers(List<GameModifier> modifiers, ArmoryEquipment armoryEquip)
	{
		foreach (GameModifier gameModifier in armoryEquip.enchants.getGameModifiers())
		{
			modifiers.Add(gameModifier);
		}
		return modifiers;
	}

	private List<GameModifier> addEnchantModifiers(List<GameModifier> modifiers)
	{
		foreach (GameModifier gameModifier in _enchants.getGameModifiers())
		{
			modifiers.Add(gameModifier);
		}
		return modifiers;
	}

	private List<GameModifier> addArmoryRuneModifiers(List<GameModifier> modifiers, ArmoryEquipment armoryEquip, bool all = true)
	{
		foreach (KeyValuePair<int, RuneRef> runeSlot in armoryEquip.runes.runeSlots)
		{
			if (runeSlot.Value == null || (!all && runeSlot.Value.runeType != 1))
			{
				continue;
			}
			foreach (GameModifier modifier in runeSlot.Value.modifiers)
			{
				modifiers.Add(modifier);
			}
		}
		return modifiers;
	}

	private List<GameModifier> addRuneModifiers(List<GameModifier> modifiers, bool all = true)
	{
		foreach (KeyValuePair<int, RuneRef> runeSlot in _runes.runeSlots)
		{
			if (runeSlot.Value == null || runeSlot.Value.modifiers == null || (!all && runeSlot.Value.runeType != 1))
			{
				continue;
			}
			foreach (GameModifier modifier in runeSlot.Value.modifiers)
			{
				modifiers.Add(modifier);
			}
		}
		return modifiers;
	}

	public List<ItemData> getAvailableRunes(int slot)
	{
		List<ItemData> list = new List<ItemData>();
		int slotType = Runes.getSlotType(slot);
		foreach (ItemData item in _inventory.GetItemsByType(9, slotType))
		{
			RuneRef runeRef = item.itemRef as RuneRef;
			if (!_runes.hasSlotMemory(runeRef, slot))
			{
				list.Add(new ItemData(runeRef, item.qty));
			}
		}
		return list;
	}

	public List<ItemData> getAvailableArmoryRunes(int slot)
	{
		List<ItemData> list = new List<ItemData>();
		int slotType = Runes.getSlotType(slot);
		foreach (ItemData item in _inventory.GetItemsByType(9, slotType))
		{
			RuneRef runeRef = item.itemRef as RuneRef;
			if (!_runes.hasSlotMemory(runeRef, slot) && !armory.currentArmoryEquipmentSlot.runes.isRuneEquipped(runeRef))
			{
				list.Add(new ItemData(runeRef, item.qty));
			}
		}
		return list;
	}

	public CharacterStats getStatBalance(int power, int stamina, int agility)
	{
		int totalStats = getTotalStats();
		int num = power + stamina + agility;
		float num2 = (float)totalStats / (float)num;
		power = Mathf.RoundToInt((float)power * num2);
		stamina = Mathf.RoundToInt((float)stamina * num2);
		agility = Mathf.RoundToInt((float)agility * num2);
		if (power < 1)
		{
			power = 1;
		}
		if (stamina < 1)
		{
			stamina = 1;
		}
		if (agility < 1)
		{
			agility = 1;
		}
		return new CharacterStats(power, stamina, agility);
	}

	public void checkZoneCompleted()
	{
		int highestCompletedZoneID = _zones.getHighestCompletedZoneID();
		if (_zoneCompleted != highestCompletedZoneID)
		{
			zoneCompleted = highestCompletedZoneID;
		}
	}

	public void checkTimerChanges(SFSObject sfsob)
	{
		if (sfsob != null)
		{
			bool flag = false;
			bool flag2 = false;
			if (sfsob.ContainsKey("cha27"))
			{
				energy = sfsob.GetInt("cha27");
				flag = true;
			}
			if (sfsob.ContainsKey("cha28"))
			{
				energyMilliseconds = sfsob.GetLong("cha28");
				energyCooldown = sfsob.GetLong("cha97");
				flag = true;
			}
			if (sfsob.ContainsKey("cha29"))
			{
				tickets = sfsob.GetInt("cha29");
				flag2 = true;
			}
			if (sfsob.ContainsKey("cha30"))
			{
				D.Log(string.Format("checkTimerChanges -> {0}", sfsob.GetLong("cha30")));
				ticketsMilliseconds = sfsob.GetLong("cha30");
				ticketsCooldown = sfsob.GetLong("cha98");
				flag2 = true;
			}
			if (sfsob.ContainsKey(ServerConstants.CHARACTER_CHANGENAME))
			{
				changename = sfsob.GetInt(ServerConstants.CHARACTER_CHANGENAME);
			}
			if (sfsob.ContainsKey("cha67"))
			{
				shards = sfsob.GetInt("cha67");
			}
			if (sfsob.ContainsKey("cha68"))
			{
				shardsMilliseconds = sfsob.GetLong("cha68");
				shardsCooldown = sfsob.GetLong("cha101");
			}
			if (sfsob.ContainsKey("cha122"))
			{
				extraInfo.seals = sfsob.GetInt("cha122");
			}
			if (sfsob.ContainsKey("cha123"))
			{
				extraInfo.sealsMilliseconds = sfsob.GetLong("cha123");
				extraInfo.sealsCooldown = sfsob.GetLong("cha124");
			}
			if (sfsob.ContainsKey("cha71"))
			{
				tokens = sfsob.GetInt("cha71");
			}
			if (sfsob.ContainsKey("cha72"))
			{
				tokensMilliseconds = sfsob.GetLong("cha72");
				tokensCooldown = sfsob.GetLong("cha99");
			}
			if (sfsob.ContainsKey("cha83"))
			{
				badges = sfsob.GetInt("cha83");
			}
			if (sfsob.ContainsKey("cha84"))
			{
				badgesMilliseconds = sfsob.GetLong("cha84");
				badgesCooldown = sfsob.GetLong("cha100");
			}
			if (flag)
			{
				updateEnergyRefillNotification();
			}
			if (flag2)
			{
				updateTicketsRefillNotification();
			}
		}
	}

	public void checkCurrencyChanges(SFSObject sfsob, bool update = false)
	{
		if (sfsob == null)
		{
			return;
		}
		bool flag = false;
		if (sfsob.ContainsKey("cha9"))
		{
			int @int = sfsob.GetInt("cha9");
			if (GameData.instance.PROJECT.character.gold != @int)
			{
				GameData.instance.PROJECT.character.gold = @int;
				flag = true;
			}
		}
		if (sfsob.ContainsKey("cha10"))
		{
			int int2 = sfsob.GetInt("cha10");
			if (GameData.instance.PROJECT.character.credits != int2)
			{
				GameData.instance.PROJECT.character.credits = int2;
				flag = true;
			}
		}
		if (flag && update)
		{
			KongregateAnalytics.updateCommonFields();
		}
	}

	public void updateAchievements()
	{
		if (AppInfo.allowAchievements)
		{
			switch (AppInfo.platform)
			{
			case 1:
			case 2:
				updateMobileAchievements();
				break;
			case 7:
				updateSteamAchievements();
				break;
			}
		}
	}

	private void updateMobileAchievements()
	{
		_ = AppInfo.allowAchievements;
	}

	private void UpdateiOSAchievements()
	{
		foreach (AchievementRef achievement in AchievementBook.GetAchievements())
		{
			if (achievement == null || !achievement.getCompleted())
			{
				continue;
			}
			Social.ReportProgress(achievement.achievementID, 100.0, delegate(bool success)
			{
				if (!success)
				{
					D.Log("all", $"[Achievement::UpdateProgress] failed for platform {AppInfo.platform} - ID: {achievement.achievementID}", forceLoggly: true);
				}
			});
		}
	}

	private void onAndroidLoadAchievements(IAchievement[] achievements)
	{
		if (achievements == null || achievements.Length == 0)
		{
			return;
		}
		foreach (IAchievement achievement in achievements)
		{
			if (achievement == null)
			{
				continue;
			}
			AchievementRef achievementRef = AchievementBook.LookupAchievementID(achievement.id);
			if (achievementRef == null || !achievementRef.getCompleted())
			{
				continue;
			}
			Social.ReportProgress(achievement.id, 100.0, delegate(bool success)
			{
				if (!success)
				{
					D.Log("all", $"[Achievement::UpdateProgress] failed for platform {AppInfo.platform} - ID: {achievement.id}", forceLoggly: true);
				}
			});
		}
	}

	public void updateSteamAchievements()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		for (int i = 0; i < AchievementBook.size; i++)
		{
			AchievementRef achievementRef = AchievementBook.Lookup(i);
			if (achievementRef != null && achievementRef.getCompleted())
			{
				SteamUserStats.SetAchievement(achievementRef.achievementID);
			}
		}
		int nData = GameData.instance.PROJECT.character.level;
		int ownedCount = FamiliarBook.GetOwnedCount();
		int highestCompletedZoneID = GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID();
		int totalStars = GameData.instance.PROJECT.character.zones.getTotalStars();
		SteamUserStats.SetStat("level", nData);
		SteamUserStats.SetStat("familiar", ownedCount);
		SteamUserStats.SetStat("zone", highestCompletedZoneID);
		SteamUserStats.SetStat("stars", totalStars);
		SteamUserStats.StoreStats();
	}

	public void updateNotifications()
	{
		updateEnergyRefillNotification();
		updateTicketsRefillNotification();
		updateDailyRewardNotification();
		updateDaySevenNotification();
		updateDayThirtyNotification();
	}

	public void updateEnergyRefillNotification()
	{
		checkNotificationTime(1, getSecondsUntilEnergyRefill(), Language.GetString("notification_energy_full_name"), Language.GetString("notification_energy_full_desc"));
	}

	public void updateTicketsRefillNotification()
	{
		checkNotificationTime(2, (int)getSecondsUntilTicketsRefill(), Language.GetString("notification_tickets_full_name"), Language.GetString("notification_tickets_full_desc"));
	}

	public void updateDailyRewardNotification()
	{
		checkNotificationTime(3, (int)ServerExtension.instance.getSecondsTillDayEnds() + 1800, Language.GetString("notification_daily_reward_name"), Language.GetString("notification_daily_reward_desc"));
	}

	public void updateDaySevenNotification()
	{
		checkNotificationTime(4, 604800, Language.GetString("notification_day_seven_name"), Language.GetString("notification_day_seven_desc"));
	}

	public void updateDayThirtyNotification()
	{
		checkNotificationTime(5, 2592000, Language.GetString("notification_day_thirty_name"), Language.GetString("notification_day_thirty_desc"));
	}

	private void checkNotificationTime(int notificationID, int seconds, string name, string desc)
	{
		if (seconds <= 0)
		{
			AppInfo.doCancelLocalNotification(notificationID);
		}
		else
		{
			AppInfo.doScheduleLocalNotification(notificationID, seconds, name, desc);
		}
	}

	private int getSecondsUntilEnergyRefill()
	{
		int num = energyMax - energy;
		if (num <= 0)
		{
			return 0;
		}
		int num2 = energySeconds;
		int num3 = (int)((num - 1) * (energyCooldown / 1000));
		return num2 + num3;
	}

	private float getSecondsUntilTicketsRefill()
	{
		int num = ticketsMax - tickets;
		if (num <= 0)
		{
			return 0f;
		}
		float num2 = ticketsSeconds;
		float num3 = (float)(num - 1) * (VariableBook.ticketsCooldown / 1000f);
		return num2 + num3;
	}

	public int getGuildMemberLimit()
	{
		int num = VariableBook.guildMemberLimit;
		if (_guildData != null)
		{
			num += Mathf.RoundToInt(GameModifier.getTypeTotal(getModifiers(), 42));
		}
		return num;
	}

	public bool getForceCharacterCustomize()
	{
		if (_name != null && _name.Length <= 0)
		{
			return true;
		}
		if (getItemQty(ServiceBook.GetFirstServiceByType(5)) > 0)
		{
			return true;
		}
		return false;
	}

	public List<ItemData> getFishingRods()
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in _inventory.GetItemsByType(1, 1))
		{
			if (item.qty > 0 && (item.itemRef as EquipmentRef).hasSubtype(VariableBook.fishingEquipmentSubtype.id))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public EquipmentRef getFishingRod()
	{
		EquipmentRef equipmentRef = EquipmentBook.Lookup(GameData.instance.SAVE_STATE.GetFishingRod(GameData.instance.PROJECT.character.id));
		if (equipmentRef != null && GameData.instance.PROJECT.character.getItemQty(equipmentRef) > 0 && equipmentRef.hasSubtype(VariableBook.fishingEquipmentSubtype.id))
		{
			return equipmentRef;
		}
		List<ItemData> itemsByType = _inventory.GetItemsByType(1, 1);
		List<object> list = new List<object>();
		foreach (ItemData item in itemsByType)
		{
			list.Add(item);
		}
		foreach (ItemData item2 in Util.SortVector(list, new string[1] { "rarity" }, Util.ARRAY_DESCENDING))
		{
			if (item2.qty > 0)
			{
				EquipmentRef equipmentRef2 = item2.itemRef as EquipmentRef;
				if (equipmentRef2.hasSubtype(VariableBook.fishingEquipmentSubtype.id))
				{
					return equipmentRef2;
				}
			}
		}
		return null;
	}

	public BobberRef getFishingBobber()
	{
		BobberRef bobberRef = BobberBook.Lookup(GameData.instance.SAVE_STATE.GetFishingBobber(GameData.instance.PROJECT.character.id));
		if (bobberRef != null && GameData.instance.PROJECT.character.getItemQty(bobberRef) > 0)
		{
			return bobberRef;
		}
		List<ItemData> itemsByType = _inventory.GetItemsByType(14);
		List<object> list = new List<object>();
		foreach (ItemData item in itemsByType)
		{
			list.Add(item);
		}
		foreach (ItemData item2 in Util.SortVector(list, new string[1] { "rarity" }, Util.ARRAY_DESCENDING))
		{
			if (item2.qty > 0)
			{
				return item2.itemRef as BobberRef;
			}
		}
		return null;
	}

	public BaitRef getFishingBait()
	{
		BaitRef baitRef = BaitBook.Lookup(GameData.instance.SAVE_STATE.GetFishingBait(GameData.instance.PROJECT.character.id));
		if (baitRef != null && GameData.instance.PROJECT.character.getItemQty(baitRef) > 0)
		{
			return baitRef;
		}
		List<ItemData> itemsByType = _inventory.GetItemsByType(13);
		List<object> list = new List<object>();
		foreach (ItemData item in itemsByType)
		{
			list.Add(item);
		}
		foreach (ItemData item2 in Util.SortVector(list, new string[1] { "rarity" }, Util.ARRAY_ASCENDING))
		{
			if (item2.qty > 0)
			{
				return item2.itemRef as BaitRef;
			}
		}
		return null;
	}

	private EquipmentRef getCosmeticInventorySlotById(ArmoryRef aRef)
	{
		return aRef;
	}

	private EquipmentRef getInventorySlotById(int idx)
	{
		for (int i = 0; i < inventory.items.Count; i++)
		{
			if (inventory.items[i].itemRef.id == idx && inventory.items[i].itemRef is EquipmentRef)
			{
				return inventory.items[i].itemRef as EquipmentRef;
			}
		}
		return null;
	}

	private void equipCurrentArmoryEquipmentIfPosible(int armorySlot, int equipSlot)
	{
		ArmoryEquipment currentArmoryEquipmentSlot = armory.currentArmoryEquipmentSlot;
		ArmoryRef armoryRef = currentArmoryEquipmentSlot.GetArmoryEquipmentSlotByArmoryType(armorySlot);
		if (armoryRef == null)
		{
			armoryRef = currentArmoryEquipmentSlot.GetArmoryEquipmentSlot(equipSlot);
		}
		if (armoryRef != null)
		{
			EquipmentRef inventorySlotById = getInventorySlotById(armoryRef.id);
			equipment.setEquipmentSlot(inventorySlotById, equipSlot, doBroadcast: false);
			if (!(inventorySlotById != null))
			{
				return;
			}
			ArmoryRef armoryRef2 = currentArmoryEquipmentSlot.GetCosmeticSlotByArmoryType(armorySlot);
			if (armoryRef2 == null)
			{
				armoryRef2 = currentArmoryEquipmentSlot.GetCosmeticSlot(equipSlot);
			}
			if (armoryRef2 != null)
			{
				if (equipSlot == 7 || equipSlot == 6)
				{
					equipment.setCosmeticSlot(getCosmeticInventorySlotById(armoryRef2), equipSlot, doBroadcast: false);
				}
				else
				{
					equipment.setCosmeticSlot(getInventorySlotById(armoryRef2.id), equipSlot, doBroadcast: false);
				}
			}
			else
			{
				equipment.setCosmeticSlot(null, equipSlot, doBroadcast: false);
			}
		}
		else
		{
			equipment.setEquipmentSlot(null, equipSlot, doBroadcast: false);
		}
	}

	public void equipCurrentArmorySlot()
	{
		ArmoryEquipment currentArmoryEquipmentSlot = armory.currentArmoryEquipmentSlot;
		GameData.instance.PROJECT.character.equipment.BroadcastBefore();
		equipCurrentArmoryEquipmentIfPosible(7, 6);
		equipCurrentArmoryEquipmentIfPosible(4, 3);
		equipCurrentArmoryEquipmentIfPosible(3, 2);
		equipCurrentArmoryEquipmentIfPosible(1, 0);
		equipCurrentArmoryEquipmentIfPosible(5, 4);
		equipCurrentArmoryEquipmentIfPosible(2, 1);
		equipCurrentArmoryEquipmentIfPosible(6, 5);
		equipCurrentArmoryEquipmentIfPosible(8, 7);
		MountRef cosmeticMount = mounts.getCosmeticMount((int)currentArmoryEquipmentSlot.mountCosmetic);
		mounts.setEquipped(mounts.getMount(currentArmoryEquipmentSlot.mount));
		mounts.setCosmetic(cosmeticMount);
		if (currentArmoryEquipmentSlot.runes != null)
		{
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(0) != null && runes.getRuneSlot(0) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(0), 0);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(1) != null && runes.getRuneSlot(1) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(1), 1);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(2) != null && runes.getRuneSlot(2) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(2), 2);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(3) != null && runes.getRuneSlot(3) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(3), 3);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(4) != null && runes.getRuneSlot(4) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(4), 4);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(5) != null && runes.getRuneSlot(5) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(5), 5);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(6) != null && runes.getRuneSlot(6) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(6), 6);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(8) != null && runes.getRuneSlot(8) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(8), 8);
			}
			if (currentArmoryEquipmentSlot.runes.getRuneSlot(7) != null && runes.getRuneSlot(7) != null)
			{
				runes.setRuneSlot(currentArmoryEquipmentSlot.runes.getRuneSlot(7), 7);
			}
		}
		if (currentArmoryEquipmentSlot.enchants != null)
		{
			List<long> list = new List<long>();
			for (int i = 0; i < currentArmoryEquipmentSlot.enchants.slots.Count; i++)
			{
				long item = currentArmoryEquipmentSlot.enchants.slots[i];
				list.Add(item);
			}
			enchants.setEnchantSlots(list);
		}
		if (currentArmoryEquipmentSlot.enchants != null && currentArmoryEquipmentSlot.runes != null)
		{
			CharacterDALC.instance.doCharacterEquipmentGlobalSave();
			Broadcast("ARMORY_TO_EQUIPMENT");
		}
		GameData.instance.PROJECT.character.equipment.Broadcast();
	}

	private void changeRuneSlotForArmoryRune(RuneRef runeRef, int runeSlot)
	{
		if (runes.getRuneSlot(runeSlot) != null)
		{
			CharacterDALC.instance.doRuneChange(runeRef, runeSlot);
		}
	}

	public static Character fromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return null;
		}
		if (!sfsob.ContainsKey("cha1"))
		{
			return null;
		}
		int @int = sfsob.GetInt("cha1");
		int int2 = sfsob.GetInt("pla3");
		Character character = new Character(@int, int2);
		character.name = sfsob.GetUtfString("cha2");
		character.gender = sfsob.GetUtfString("cha12");
		character.hairID = sfsob.GetInt("cha20");
		character.hairColorID = sfsob.GetInt("cha21");
		character.skinColorID = sfsob.GetInt("cha22");
		character.herotag = sfsob.GetUtfString("cha109");
		character.nameHasChanged = sfsob.GetBool(ServerConstants.CHARACTER_NAMEHASCHANGED);
		character.level = sfsob.GetInt("cha4");
		character.exp = sfsob.GetLong("cha5");
		character.gold = sfsob.GetInt("cha9");
		character.credits = sfsob.GetInt("cha10");
		character.points = sfsob.GetInt("cha19");
		character.power = sfsob.GetInt("cha6");
		character.stamina = sfsob.GetInt("cha7");
		character.agility = sfsob.GetInt("cha8");
		character.zoneID = sfsob.GetInt("cha24");
		character.zoneCompleted = sfsob.GetInt("cha94");
		character.dailyID = sfsob.GetInt("cha25");
		character.dailyDate = Util.GetDateFromString(sfsob.GetUtfString("cha26"));
		character.dailyFishingDate = Util.GetDateFromString(sfsob.GetUtfString("cha96"));
		character.shopRotationID = sfsob.GetInt("cha41");
		character.shopRotationMilliseconds = sfsob.GetLong("cha42");
		character.adMilliseconds = sfsob.GetLong("cha65");
		character.adItem = ItemData.fromSFSObject(sfsob.GetSFSObject("cha66"));
		if (sfsob.ContainsKey("cha75"))
		{
			character.nbpDate = Util.GetDateFromString(sfsob.GetUtfString("cha75"));
		}
		else
		{
			character.nbpDate = null;
		}
		character.admin = sfsob.GetBool("cha15");
		character.moderator = sfsob.GetBool("cha34");
		character.autoPilot = sfsob.GetBool("cha32");
		character.chatEnabled = sfsob.GetBool("cha35");
		character.chatChannel = sfsob.GetInt("cha36");
		character.friendRequestsEnabled = sfsob.GetBool("cha40");
		character.duelRequestsEnabled = sfsob.GetBool("cha82");
		character.showHelm = sfsob.GetBool("cha48");
		character.showMount = sfsob.GetBool("cha93");
		character.showBody = sfsob.GetBool("cha133");
		character.showAccessory = sfsob.GetBool("cha134");
		character.lockedItems = ItemRef.listFromSFSObject(sfsob, "cha95");
		character.updateMilliseconds = sfsob.GetLong("cha91");
		character.armory = Armory.FromSFSObject(sfsob);
		character.equipment = Equipment.fromSFSObject(sfsob);
		character.runes = Runes.fromSFSObject(sfsob);
		character.enchants = Enchants.fromSFSObject(sfsob);
		character.augments = Augments.fromSFSObject(sfsob);
		character.mounts = Mounts.fromSFSObject(sfsob);
		character.tutorial = Tutorial.fromSFSObject(sfsob);
		character.inventory = Inventory.fromSFSObject(sfsob);
		character.zones = Zones.fromSFSObject(sfsob);
		character.dailyQuests = DailyQuests.fromSFSObject(sfsob);
		character.characterAchievements = CharacterAchievements.fromSFSObject(sfsob);
		character.familiarStable = FamiliarStable.fromSFSObject(sfsob);
		character.platforms = Util.arrayToIntegerVector(sfsob.GetIntArray("cha74"));
		character.friends = FriendData.listFromSFSObject(sfsob);
		character.requests = RequestData.listFromSFSObject(sfsob);
		character.chatIgnores = ChatPlayerData.listFromSFSObject(sfsob);
		character.consumableModifiers = ConsumableModifierData.listFromSFSObject(sfsob);
		character.guildData = CharacterGuildData.fromSFSObject(sfsob);
		character.pvpEventLootID = sfsob.GetInt("pvp1");
		character.riftEventLootID = sfsob.GetInt("rif1");
		character.gauntletEventLootID = sfsob.GetInt("gau1");
		character.gvgEventLootID = sfsob.GetInt("gvg1");
		character.invasionEventLootID = sfsob.GetInt("inv1");
		character.fishingEventLootID = sfsob.GetInt("fise1");
		character.gveEventLootID = sfsob.GetInt("gve1");
		character.teams = Teams.fromSFSObject(sfsob);
		character.energy = sfsob.GetInt("cha27");
		character.energyMilliseconds = sfsob.GetLong("cha28");
		character.energyCooldown = sfsob.GetLong("cha97");
		character.tickets = sfsob.GetInt("cha29");
		character.ticketsMilliseconds = sfsob.GetLong("cha30");
		character.ticketsCooldown = sfsob.GetLong("cha98");
		character.changenameCooldown = (sfsob.ContainsKey(ServerConstants.CHARACTER_CHANGENAME_COOLDOWN) ? sfsob.GetLong(ServerConstants.CHARACTER_CHANGENAME_COOLDOWN) : 0);
		character.shards = sfsob.GetInt("cha67");
		character.shardsMilliseconds = sfsob.GetLong("cha68");
		character.shardsCooldown = sfsob.GetLong("cha101");
		character.tokens = sfsob.GetInt("cha71");
		character.tokensMilliseconds = sfsob.GetLong("cha72");
		character.tokensCooldown = sfsob.GetLong("cha99");
		character.badges = sfsob.GetInt("cha83");
		character.badgesMilliseconds = sfsob.GetLong("cha84");
		character.badgesCooldown = sfsob.GetLong("cha100");
		character.eventsWon = Util.arrayToIntegerVector(sfsob.GetIntArray("cha108"));
		character.extraInfo = CharacterExtraInfo.FromSFSObject(sfsob, character);
		character.extraInfo.seals = sfsob.GetInt("cha122");
		character.extraInfo.sealsCooldown = sfsob.GetLong("cha124");
		character.extraInfo.sealsMilliseconds = sfsob.GetLong("cha123");
		character.GetCurrentBoosters();
		string text = (sfsob.ContainsKey("nft1") ? sfsob.GetUtfString("nft1") : null);
		if (!string.IsNullOrEmpty(text))
		{
			character.imxG0Data = JsonUtility.FromJson<IMXG0Data>(text);
		}
		character._nftToken = (sfsob.ContainsKey("nft2") ? sfsob.GetUtfString("nft2") : null);
		character._nftState = (sfsob.ContainsKey("nft3") ? sfsob.GetInt("nft3") : 0);
		return character;
	}

	public static int getTier(int zoneCompleted)
	{
		return zoneCompleted + 1;
	}

	public static string getGenderImageURL(string gender)
	{
		return "";
	}

	public static long getLevelExp(int level)
	{
		if (level <= 1)
		{
			return 0L;
		}
		long num = (long)Mathf.Round(Mathf.Pow((float)level / 50f, 2f) * 800000f);
		float num2 = 0.15f + 0.01f * (float)level;
		return (long)Util.roundToNearest(10f, Mathf.Round((float)num * num2));
	}

	public static int getExpLevel(long exp)
	{
		int i;
		for (i = 1; i < int.MaxValue; i++)
		{
			long levelExp = getLevelExp(i);
			if (exp < levelExp)
			{
				return i - 1;
			}
		}
		return i;
	}

	public static void testCharacterExp(int intervals = 1)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 1; i <= 100; i++)
		{
			long levelExp = getLevelExp(i);
			long num4 = levelExp - num;
			num = (int)levelExp;
			num2 = (int)num4;
			num3++;
			if ((num3 >= intervals || i == 2) && i != 2)
			{
				num3 = 0;
			}
		}
	}

	public bool getItemLocked(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		foreach (ItemRef lockedItem in _lockedItems)
		{
			if (!(lockedItem == null) && lockedItem == itemRef)
			{
				return true;
			}
		}
		return false;
	}

	private void clearUpdateTimer()
	{
		if (_updateTimer != null)
		{
			throw new Exception("Error --> CONTROL");
		}
	}

	private void setUpdateTimer()
	{
		clearUpdateTimer();
		_ = _updateMilliseconds;
		_ = 0;
	}

	private void onUpdateTimer()
	{
		CharacterDALC.instance.doUpdate();
	}

	public void RerunDungeon(Dungeon dungeon)
	{
		_rerunDungeon = dungeon;
		OnTimerCompletePlayerExitTimer();
	}

	private void OnTimerCompletePlayerExitTimer()
	{
		D.Log("all", "_rerunDungeon.type " + _rerunDungeon.type);
		switch (_rerunDungeon.type)
		{
		case 1:
		{
			ZoneNodeDifficultyRef dungeonZoneNodeDifficultyRef = ZoneBook.GetDungeonZoneNodeDifficultyRef(_rerunDungeon.dungeonRef);
			GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnEnterZoneNode);
			D.Log("all", "Character Enter Zone");
			List<TeammateData> list2 = GameData.instance.PROJECT.character.selectedTeammates;
			for (int j = 0; j < list2.Count; j++)
			{
				if (list2[j].id == GameData.instance.PROJECT.character.id)
				{
					list2[j] = new TeammateData(list2[j].id, 1, -1L);
					break;
				}
			}
			GameDALC.instance.doEnterZoneNode(dungeonZoneNodeDifficultyRef, list2);
			return;
		}
		case 2:
		{
			RaidDifficultyRef raidDifficultyRef = null;
			foreach (RaidRef allRaid in RaidBook.GetAllRaids())
			{
				if (allRaid == null)
				{
					continue;
				}
				foreach (RaidDifficultyRef difficulty in allRaid.difficulties)
				{
					if (difficulty != null && _rerunDungeon.dungeonRef.link == difficulty.dungeonRef.link)
					{
						raidDifficultyRef = difficulty;
						break;
					}
				}
				if (raidDifficultyRef != null)
				{
					break;
				}
			}
			if (raidDifficultyRef == null)
			{
				break;
			}
			GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnEnterRaid);
			D.Log("all", "Character Enter RAID");
			List<TeammateData> list = GameData.instance.PROJECT.character.selectedTeammates;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].id == GameData.instance.PROJECT.character.id)
				{
					list[i] = new TeammateData(list[i].id, 1, -1L);
					break;
				}
			}
			GameDALC.instance.doEnterRaid(raidDifficultyRef, list);
			return;
		}
		}
		GameData.instance.main.HideLoading();
	}

	private void OnEnterZoneNode(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnEnterZoneNode);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.DUNGEONS_PLAYED);
		}
	}

	private void OnEnterRaid(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnEnterRaid);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.DUNGEONS_PLAYED);
		}
	}

	public bool hasRuneOrHad()
	{
		if (inventory.GetItemsByType(9, -1, checkQty: false).Count > 0 || runes.hasAnySlotMemory || runes.hasAnySlotEquipped)
		{
			return true;
		}
		foreach (ArmoryEquipment armoryEquipmentSlot in armory.armoryEquipmentSlots)
		{
			if (armoryEquipmentSlot.runes != null && (armoryEquipmentSlot.runes.hasAnySlotMemory || armoryEquipmentSlot.runes.hasAnySlotEquipped))
			{
				return true;
			}
		}
		return false;
	}

	public bool hasEnchantOrHad()
	{
		if (GameData.instance.PROJECT.character.inventory.GetItemsByType(11, -1, checkQty: false).Count > 0 || enchants.hasAnyEnchantEquipped || enchants.hasAnyEnchant)
		{
			return true;
		}
		foreach (ArmoryEquipment armoryEquipmentSlot in armory.armoryEquipmentSlots)
		{
			if (armoryEquipmentSlot.enchants != null && (armoryEquipmentSlot.enchants.hasAnyEnchantEquipped || armoryEquipmentSlot.enchants.hasAnyEnchant))
			{
				return true;
			}
		}
		return false;
	}

	public bool hasAugmentOrHad()
	{
		if (GameData.instance.PROJECT.character.inventory.GetItemsByType(15, -1, checkQty: false).Count > 0 || augments.augments.Count > 0)
		{
			return true;
		}
		return false;
	}

	public bool hasAccessoryOrHad()
	{
		return GameData.instance.PROJECT.character.inventory.GetItemsByType(1, 7, checkQty: false).Count > 0;
	}

	public void ClearTimers()
	{
		clearAdTimer();
		ClearBadgesTimer();
		ClearEnergyTimer();
		ClearTicketsTimer();
		ClearShardsTimer();
		ClearTokensTimer();
		extraInfo.Clear();
	}

	public static int getStatType(string type)
	{
		return STAT_TYPES[type.ToLowerInvariant()];
	}

	public static string getStatName(int type)
	{
		return Language.GetString(STAT_NAMES[type]);
	}

	internal int getItemQty(object itemRef)
	{
		throw new NotImplementedException();
	}

	public void TrackSetStats(string state)
	{
		string jsonMap = JsonConvert.SerializeObject(new KongregateAnalyticsSchema.CharacterInventory
		{
			state = state,
			update_type = "full",
			items_full_inventory = inventory.statItems(this)
		});
		AppInfo.doKongregateAnalyticsEvent("inventory", jsonMap);
	}

	public void saveShowNameplate(bool v)
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(66), onShowNameplate);
		CharacterDALC.instance.doShowNameplate(v);
	}

	private void onShowNameplate(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(66), onShowNameplate);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.Log(string.Format("Character::onShowNamePlate {0}", sfsob.GetInt("err0")));
			return;
		}
		string utfString = sfsob.GetUtfString("char125");
		_extraInfo.setJsonData(2, utfString);
	}

	public void GetCurrentBoosters()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(70), OnGetCurrentBoosters);
		CharacterDALC.instance.getCurrentBoosters();
	}

	private void OnGetCurrentBoosters(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(70), OnGetCurrentBoosters);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.Log(string.Format("OnGetCurrentBoosters {0}", sfsob.GetInt("err0")));
			return;
		}
		List<BoosterRef> list = BoosterRef.listFromSFSObjectActives(sfsob);
		_activeBoosters.Clear();
		_activeBoosters = list;
		foreach (BoosterRef booster in _activeBoosters)
		{
			if (booster.endDate.HasValue && !_boostersCoroutines.ContainsKey(booster.id))
			{
				TimeSpan timeSpan = booster.endDate.Value - ServerExtension.instance.GetDate();
				Coroutine value = GameData.instance.main.coroutineTimer?.AddTimer(null, (float)timeSpan.TotalMilliseconds, CoroutineTimer.TYPE.MILLISECONDS, 1, delegate
				{
					BoosterCoroutine(booster);
				});
				_boostersCoroutines.Add(booster.id, value);
			}
		}
		List<BoosterRef> list2 = BoosterRef.listFromSFSObjectPassives(sfsob);
		_passiveBoosters.Clear();
		_passiveBoosters = list2;
		BOOSTER_CHANGED.Invoke(null);
	}

	private void BoosterCoroutine(BoosterRef boosterRef)
	{
		_boostersCoroutines.Remove(boosterRef.id);
		GetCurrentBoosters();
		BOOSTER_CHANGED.Invoke(boosterRef);
	}

	public Coroutine GetTimerForBoosterRef(BoosterRef boosterRef)
	{
		foreach (KeyValuePair<int, Coroutine> boostersCoroutine in _boostersCoroutines)
		{
			if (boostersCoroutine.Key == boosterRef.id)
			{
				return boostersCoroutine.Value;
			}
		}
		return null;
	}

	public void FreezeNFT()
	{
		_nftState = 3;
	}
}
