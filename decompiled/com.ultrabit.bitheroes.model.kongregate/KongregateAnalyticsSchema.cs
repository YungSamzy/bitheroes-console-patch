using System;
using System.Collections.Generic;

namespace com.ultrabit.bitheroes.model.kongregate;

public class KongregateAnalyticsSchema
{
	[Serializable]
	public class PlayStarts
	{
		public string play_id;

		public string mode;

		public int enemyCharID;

		public int defender_id;

		public int mastery_number;

		public string map_name;

		public int mission_number;
	}

	[Serializable]
	public class PlayEnds
	{
		public string play_id;

		public string mode;

		public string end_type;

		public int hard_currency_change;

		public int soft_currency_change;

		public int pve_energy_used;

		public int pvp_energy_used;

		public bool with_friend;

		public int TS_friend_max;

		public int rival_total_stats;

		public int event_id;

		public bool autoplay;

		public int defender_id;

		public int mastery_number;

		public string map_name;

		public int mission_number;
	}

	[Serializable]
	public class AdStart
	{
		public bool is_optional;

		public string ad_type;

		public string context_of_offer;
	}

	[Serializable]
	public class AdEnd
	{
		public string reward;

		public string ad_type;

		public string context_of_offer;
	}

	[Serializable]
	public class EconomyTransaction
	{
		public string type;

		public string resources_summary;

		public int hard_currency_change;

		public int soft_currency_change;

		public string context_of_offer;

		public int item_id;

		public int item_type;
	}

	[Serializable]
	public class TutorialStepEnd
	{
		public string tutorial_type;

		public string description;

		public int step_number;

		public bool is_final;
	}

	[Serializable]
	public class PaymentFields
	{
		public int hard_currency_change;

		public int soft_currency_change;

		public string type;

		public float discount_percent;

		public string context_of_offer;
	}

	[Serializable]
	public class CommonFields
	{
		public int soft_currency_balance;

		public int soft_currency_bought;

		public int soft_currency_spent;

		public int soft_currency_earned;

		public int hard_currency_balance;

		public int hard_currency_bought;

		public int hard_currency_spent;

		public int hard_currency_earned;

		public int num_pve_played;

		public int num_pvp_played;

		public int num_pve_won;

		public int num_pvp_won;

		public int current_pve_energy;

		public int current_pvp_energy;

		public int max_pve_energy;

		public int max_pvp_energy;

		public int player_total_stats;

		public int player_power;

		public int player_stamina;

		public int player_agility;

		public string game_username;

		public int player_level;

		public int guild_id;

		public int server_player_id;

		public DateTime server_event_time_gmt;

		public int player_state;

		public string client_build_platform;
	}

	[Serializable]
	public class RuneStat
	{
		public string item_name;

		public int rarity;

		public int type;
	}

	[Serializable]
	public class CharacterInventory
	{
		public string state;

		public string update_type;

		public List<Dictionary<string, object>> items_full_inventory;
	}

	[Serializable]
	public class SettingsStat
	{
		public string name;

		public object value;
	}

	[Serializable]
	public class ItemStat
	{
		public string item_name;

		public int rarity;

		public int type;
	}

	[Serializable]
	public class StatEntityPlayer
	{
		public string player_name;

		public int player_total_stats;
	}

	[Serializable]
	public class NavigationActions
	{
		public string origin;

		public string nav_element_name;

		public string sub_nav_element_name;
	}

	[Serializable]
	public class LoadOutStats
	{
		public int mode;

		public string loadout_context;

		public int player_total_stats;

		public List<LoadOutStatsFriend> friends;

		public List<ItemStat> runes;

		public List<ItemStat> gear;

		public List<Dictionary<string, object>> boosts;

		public List<Dictionary<string, object>> familiars;

		public List<SettingsStat> settings;

		public LoadOutStats()
		{
			friends = new List<LoadOutStatsFriend>();
			runes = new List<ItemStat>();
			gear = new List<ItemStat>();
			boosts = new List<Dictionary<string, object>>();
			familiars = new List<Dictionary<string, object>>();
			settings = new List<SettingsStat>();
		}
	}

	public class LoadOutStatsFriend
	{
		public int index;

		public int stats;
	}

	public const string ANALYTICS_LOADING_ENDS = "loading_ends";

	public const string ANALYTICS_LOADING_ENDS_TIME = "load_time_ms";

	public const string ANALYTICS_LOADING_ENDS_TYPE = "loading_type";

	public const string ANALYTICS_LOAD_INIT_COMPLETE = "init_complete";

	public const string ANALYTICS_LOAD_SERVER_CONNECT = "server_connect";

	public const string ANALYTICS_LOAD_LOGGED_IN = "logged_in";

	public const string ANALYTICS_LOAD_BOOKS_DOWNLOADED = "books_downloaded";

	public const string ANALYTICS_LOAD_BOOKS_PARSED = "books_parsed";

	public const string ANALYTICS_LOAD_DLC_DOWNLOADED = "dlc_downloaded";

	public const string ANALYTICS_LOAD_LOGIN_END = "login_end";

	public const string ANALYTICS_LOAD_TOWN_ENTER = "town_enter";
}
