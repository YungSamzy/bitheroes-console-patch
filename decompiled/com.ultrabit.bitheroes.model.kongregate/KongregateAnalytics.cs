using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.service;
using com.ultrabit.bitheroes.ui.shop;
using Newtonsoft.Json;
using Sfs2X.Entities.Data;
using SimpleJSON;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.kongregate;

public class KongregateAnalytics
{
	public const int CPE_EVENT_KONG_ENTER_WINTERMARSH_ID = 12;

	public const int CPE_EVENT_KONG_FINISH_ZONE1_FLAG5_ID = 5;

	public const int CPE_EVENT_KONG_FINISH_ASHVALE_ID = 51;

	public const string ECONOMY_TYPE_BATTLE_CAPTURE = "Battle Capture";

	public const string ECONOMY_TYPE_BATTLE_COMPLETE = "Battle Complete";

	public const string ECONOMY_TYPE_BATTLE_COMPLETE_GAUNTLET = "Battle Complete (Gauntlet)";

	public const string ECONOMY_TYPE_BATTLE_COMPLETE_PVP = "Battle Complete (PvP)";

	public const string ECONOMY_TYPE_DUNGEON_LOOT = "Dungeon Loot";

	public const string ECONOMY_TYPE_DUNGEON_LOOT_ZONE = "Dungeon Loot (Zone)";

	public const string ECONOMY_TYPE_DUNGEON_LOOT_RAID = "Dungeon Loot (Raid)";

	public const string ECONOMY_TYPE_DUNGEON_LOOT_RIFT = "Dungeon Loot (Trials)";

	public const string ECONOMY_TYPE_ZONE_NODE_REWARD = "Zone Node Reward";

	public const string ECONOMY_TYPE_PVP_EVENT_REWARD = "PvP Event Reward";

	public const string ECONOMY_TYPE_RIFT_EVENT_REWARD = "Trials Event Reward";

	public const string ECONOMY_TYPE_GAUNTLET_EVENT_REWARD = "Gauntlet Event Reward";

	public const string ECONOMY_TYPE_GVG_EVENT_REWARD = "GvG Event Reward";

	public const string ECONOMY_TYPE_INVASION_EVENT_REWARD = "Invasion Event Reward";

	public const string ECONOMY_TYPE_FISHING_EVENT_REWARD = "Fishing Event Reward";

	public const string ECONOMY_TYPE_GVE_EVENT_REWARD = "GvE Event Reward";

	public const string ECONOMY_TYPE_DAILY_LOGIN_REWARD = "Daily Login Reward";

	public const string ECONOMY_TYPE_DAILY_QUEST_REWARD = "Bounty Reward";

	public const string ECONOMY_TYPE_PURCHASE_ITEM = "Purchase Item";

	public const string ECONOMY_TYPE_USE_CONSUMABLE = "Use Consumable";

	public const string ECONOMY_TYPE_INCENTIVIZED_AD = "Incentivized Ad";

	public const string ECONOMY_TYPE_GUILD_CREATE = "Guild Create";

	public const string ECONOMY_TYPE_CRAFT_EXCHANGE = "Craft Exchange";

	public const string ECONOMY_TYPE_CRAFT_UPGRADE = "Craft Upgrade";

	public const string ECONOMY_TYPE_CRAFT_REFORGE = "Craft Reforge";

	public const string ECONOMY_TYPE_CRAFT_TRADE = "Craft Trade";

	public const string ECONOMY_TYPE_FUSION = "Fusion";

	public const string ECONOMY_TYPE_RUNE = "Rune";

	public const string ECONOMY_TYPE_IAP = "IAP";

	public const string ECONOMY_CONTEXT_GAME = "Game";

	public const string ECONOMY_CONTEXT_ZONE = "Zone";

	public const string ECONOMY_CONTEXT_BATTLE = "Battle";

	public const string ECONOMY_CONTEXT_DUNGEON = "Dungeon";

	public const string ECONOMY_CONTEXT_INSTANCE = "Town";

	public const string ECONOMY_CONTEXT_RAID = "Raid";

	public const string ECONOMY_CONTEXT_PVP_EVENT = "PvP Event";

	public const string ECONOMY_CONTEXT_RIFT_EVENT = "Trials Event";

	public const string ECONOMY_CONTEXT_GAUNTLET_EVENT = "Gauntlet Event";

	public const string ECONOMY_CONTEXT_GVG_EVENT = "GvG Event";

	public const string ECONOMY_CONTEXT_INVASION_EVENT = "Invasion Event";

	public const string ECONOMY_CONTEXT_FISHING_EVENT = "Fishing Event";

	public const string ECONOMY_CONTEXT_GVE_EVENT = "GvE Event";

	public const string ECONOMY_CONTEXT_DAILY_LOGIN = "Daily Login";

	public const string ECONOMY_CONTEXT_DAILY_QUEST = "Bounty";

	public const string ECONOMY_CONTEXT_SHOP = "Shop";

	public const string ECONOMY_CONTEXT_CRAFT = "Craft";

	public const string ECONOMY_CONTEXT_TUTORIAL = "Tutorial";

	public const string ECONOMY_CONTEXT_SERVICE = "Service";

	public const string ECONOMY_CONTEXT_GUILD = "Guild";

	public const string ECONOMY_CONTEXT_EVENT_SHOP = "Event Shop";

	public const string ECONOMY_CONTEXT_FISHING_SHOP = "Fishing Shop";

	public const string AD_CONTEXT_ADGOR = "AdGor";

	public const string AD_CONTEXT_FREESTUFF = "FreeStuff";

	public const string AD_CONTEXT_DUNGEON = "Dungeon";

	public const string AD_TYPE_REWARDED_VIDEO = "Rewarded Video";

	public const string AD_TYPE_OFFERWALL = "Offerwall";

	public const int CURRENCY_TYPE_BOUGHT = 0;

	public const int CURRENCY_TYPE_SPENT = 1;

	public const int CURRENCY_TYPE_EARNED = 2;

	public const string CPE_EVENT_KONG_ENTER_WINTERMARSH = "kong_play_wintermarsh";

	public const string CPE_EVENT_KONG_FINISH_ZONE1_FLAG5 = "kong_finish_zone1_flag5";

	public const string CPE_EVENT_KONG_FINISH_ASHVALE = "kong_finish_ashvale";

	public const string CPE_EVENT_KONG_JOIN_GUILD = "kong_join_guild";

	public const string CPE_EVENT_KONG_PLAY_RAID = "kong_play_raid";

	public static Dictionary<string, string> DB_OFFERWALL_NETWORK_ANALYTICS_MATCH = new Dictionary<string, string>
	{
		{ "default", "Offerwall" },
		{ "RevU", "Offerwall Revu" },
		{ "ironSource", "Offerwall" }
	};

	public static string getBattleEconomyType(Battle battle)
	{
		return battle.type switch
		{
			6 => "Battle Complete (Gauntlet)", 
			2 => "Battle Complete (PvP)", 
			_ => "Battle Complete", 
		};
	}

	public static string getBattleEconomyContext(Battle battle)
	{
		return battle.type switch
		{
			6 => "Gauntlet Event", 
			2 => "PvP Event", 
			_ => "Battle", 
		};
	}

	public static string getOfferwallMatch(string dbKey)
	{
		if (!DB_OFFERWALL_NETWORK_ANALYTICS_MATCH.ContainsKey(dbKey))
		{
			return DB_OFFERWALL_NETWORK_ANALYTICS_MATCH["default"];
		}
		return DB_OFFERWALL_NETWORK_ANALYTICS_MATCH[dbKey];
	}

	public static string getDungeonEconomyType(Dungeon dungeon)
	{
		return dungeon.type switch
		{
			1 => "Dungeon Loot (Zone)", 
			2 => "Dungeon Loot (Raid)", 
			3 => "Dungeon Loot (Trials)", 
			_ => "Dungeon Loot", 
		};
	}

	public static string getDungeonEconomyContext(Dungeon dungeon)
	{
		return dungeon.type switch
		{
			1 => "Zone", 
			2 => "Raid", 
			3 => "Trials Event", 
			_ => "Dungeon", 
		};
	}

	public static void trackPlayStarts(string playID, string mode, int enemyCharID = -1, int difficulty = -1, ZoneNodeDifficultyRef nodeDifficultyRef = null)
	{
		KongregateAnalyticsSchema.PlayStarts playStarts = new KongregateAnalyticsSchema.PlayStarts();
		playStarts.play_id = playID;
		playStarts.mode = mode;
		if (enemyCharID >= 0)
		{
			playStarts.defender_id = enemyCharID;
		}
		if (difficulty >= 0)
		{
			playStarts.mastery_number = difficulty;
		}
		if (nodeDifficultyRef != null)
		{
			playStarts.map_name = Util.GetRawString(nodeDifficultyRef.getNodeRef().statName);
			playStarts.mission_number = nodeDifficultyRef.getNodeRef().missionID;
		}
		string jsonMap = JsonConvert.SerializeObject(playStarts);
		AppInfo.doKongregateAnalyticsEvent("play_starts", jsonMap);
	}

	public static void trackPlayEnds(string playID, string mode, string endType, int creditsGained, int goldGained, int pveEnergyChange, int pvpEnergyChange, int friendStats, int enemyStats, int enemyCharID, int difficulty, ZoneNodeDifficultyRef nodeDifficultyRef, int eventID)
	{
		KongregateAnalyticsSchema.PlayEnds playEnds = new KongregateAnalyticsSchema.PlayEnds();
		playEnds.play_id = playID;
		playEnds.mode = mode;
		playEnds.end_type = endType;
		playEnds.hard_currency_change = creditsGained;
		playEnds.soft_currency_change = goldGained;
		playEnds.pve_energy_used = pveEnergyChange;
		playEnds.pvp_energy_used = pvpEnergyChange;
		playEnds.with_friend = friendStats > 0;
		playEnds.TS_friend_max = friendStats;
		playEnds.rival_total_stats = enemyStats;
		playEnds.event_id = eventID;
		playEnds.autoplay = GameData.instance.PROJECT.character.autoPilot;
		playEnds.rival_total_stats = enemyStats;
		if (enemyCharID >= 0)
		{
			playEnds.defender_id = enemyCharID;
		}
		if (difficulty >= 0)
		{
			playEnds.mastery_number = difficulty;
		}
		int num = 0;
		if (nodeDifficultyRef != null)
		{
			playEnds.map_name = Util.GetRawString(nodeDifficultyRef.getNodeRef().statName);
			playEnds.mission_number = nodeDifficultyRef.getNodeRef().missionID;
			num = playEnds.mission_number;
		}
		string jsonMap = JsonConvert.SerializeObject(playEnds);
		AppInfo.doKongregateAnalyticsEvent("play_ends", jsonMap);
		switch (num)
		{
		case 12:
			TrackCPEEvent("kong_play_wintermarsh");
			break;
		case 5:
			TrackCPEEvent("kong_finish_zone1_flag5");
			break;
		case 51:
			TrackCPEEvent("kong_finish_ashvale");
			break;
		}
	}

	public static void trackAdStart(string context, string type)
	{
		string jsonMap = JsonConvert.SerializeObject(new KongregateAnalyticsSchema.AdStart
		{
			is_optional = true,
			ad_type = type,
			context_of_offer = context
		});
		AppInfo.doKongregateAnalyticsEvent("ad_start", jsonMap);
	}

	public static void trackAdEnd(List<ItemData> items, string context, string type)
	{
		string jsonMap = JsonConvert.SerializeObject(new KongregateAnalyticsSchema.AdEnd
		{
			reward = ItemData.parseSummary(null, items),
			ad_type = type,
			context_of_offer = context
		});
		AppInfo.doKongregateAnalyticsEvent("ad_end", jsonMap);
	}

	public static int getGoldChange(SFSObject sfsob)
	{
		int num = ((sfsob != null && sfsob.ContainsKey("cha128")) ? sfsob.GetInt("cha128") : GameData.instance.PROJECT.character.gold);
		return ((sfsob != null && sfsob.ContainsKey("cha9")) ? sfsob.GetInt("cha9") : num) - num;
	}

	public static int getCreditsChange(SFSObject sfsob)
	{
		int num = ((sfsob != null && sfsob.ContainsKey("cha129")) ? sfsob.GetInt("cha129") : GameData.instance.PROJECT.character.credits);
		return ((sfsob != null && sfsob.ContainsKey("cha10")) ? sfsob.GetInt("cha10") : num) - num;
	}

	public static void checkEconomyTransaction(string type, List<ItemData> itemsRemoved, List<ItemData> itemsAdded, SFSObject sfsob, string context = null, int currencyType = -1, bool currencyUpdate = true, ItemRef originItem = null, bool checkCurrencyChange = true)
	{
		int num = 0;
		int num2 = 0;
		if (checkCurrencyChange)
		{
			CurrencyRef itemRef = CurrencyBook.Lookup(1);
			num = getGoldChange(sfsob);
			int itemRefQuantity = ItemData.getItemRefQuantity(itemsRemoved, itemRef);
			int itemRefQuantity2 = ItemData.getItemRefQuantity(itemsAdded, itemRef);
			CurrencyRef itemRef2 = CurrencyBook.Lookup(2);
			num2 = getCreditsChange(sfsob);
			int itemRefQuantity3 = ItemData.getItemRefQuantity(itemsRemoved, itemRef2);
			int itemRefQuantity4 = ItemData.getItemRefQuantity(itemsAdded, itemRef2);
			if (num == 0)
			{
				if (itemRefQuantity2 != 0)
				{
					num = itemRefQuantity2;
				}
				else if (itemRefQuantity != 0)
				{
					num = -itemRefQuantity;
				}
			}
			if (num2 == 0)
			{
				if (itemRefQuantity4 != 0)
				{
					num2 = itemRefQuantity4;
				}
				else if (itemRefQuantity3 != 0)
				{
					num2 = -itemRefQuantity3;
				}
			}
			if (num == 0 && num2 == 0)
			{
				return;
			}
		}
		string text = "";
		if (originItem != null)
		{
			text = originItem.name + ": ";
		}
		trackEconomyTransaction(type, text + ItemData.parseSummary(itemsRemoved, itemsAdded), num2, num, context, currencyType, currencyUpdate, originItem);
	}

	public static void trackEconomyTransaction(string type, string summary, int creditsChange, int goldChange, string context = null, int currencyType = -1, bool currencyUpdate = true, ItemRef originItem = null)
	{
		if (context == null)
		{
			context = getCurrentEconomyContext();
		}
		bool flag = false;
		switch (currencyType)
		{
		case 0:
			if (creditsChange > 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_BOUGHT, creditsChange, update: false);
				flag = true;
			}
			if (goldChange > 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_BOUGHT, goldChange, update: false);
				flag = true;
			}
			break;
		case 1:
			if (creditsChange < 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_SPENT, -creditsChange, update: false);
				flag = true;
			}
			if (goldChange < 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_SPENT, -goldChange, update: false);
				flag = true;
			}
			break;
		case 2:
			if (creditsChange > 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_EARNED, creditsChange, update: false);
				flag = true;
			}
			if (goldChange > 0)
			{
				GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_EARNED, goldChange, update: false);
				flag = true;
			}
			break;
		}
		if (flag && currencyUpdate)
		{
			updateCommonFields();
		}
		KongregateAnalyticsSchema.EconomyTransaction economyTransaction = new KongregateAnalyticsSchema.EconomyTransaction();
		economyTransaction.type = type;
		economyTransaction.resources_summary = summary;
		economyTransaction.hard_currency_change = creditsChange;
		economyTransaction.soft_currency_change = goldChange;
		economyTransaction.context_of_offer = context;
		if (originItem != null)
		{
			economyTransaction.item_id = originItem.id;
			economyTransaction.item_type = originItem.itemType;
		}
		string text = JsonConvert.SerializeObject(economyTransaction);
		D.Log("all", "[Analytics] economy_transactions *** START *** ");
		D.Log("all", "[Analytics] " + text);
		D.Log("all", "[Analytics] economy_transactions *** END *** ");
		AppInfo.doKongregateAnalyticsEvent("economy_transactions", text);
	}

	public static void updateCommonFields()
	{
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			Character character = GameData.instance.PROJECT.character;
			GameData.instance.SAVE_STATE.characterID = character.id;
			GameData.instance.SAVE_STATE.characterGuildID = ((character.guildData != null) ? character.guildData.id : 0);
			GameData.instance.SAVE_STATE.characterGold = character.gold;
			GameData.instance.SAVE_STATE.characterCredits = character.credits;
			GameData.instance.SAVE_STATE.characterLevel = character.level;
			GameData.instance.SAVE_STATE.characterEnergy = character.energy;
			GameData.instance.SAVE_STATE.characterEnergyMax = character.energyMax;
			GameData.instance.SAVE_STATE.characterTickets = character.tickets;
			GameData.instance.SAVE_STATE.characterTicketsMax = character.ticketsMax;
			GameData.instance.SAVE_STATE.characterTotalStats = character.getTotalStats();
			GameData.instance.SAVE_STATE.characterTotalPower = character.getTotalPower();
			GameData.instance.SAVE_STATE.characterTotalStamina = character.getTotalStamina();
			GameData.instance.SAVE_STATE.characterTotalAgility = character.getTotalAgility();
			GameData.instance.SAVE_STATE.characterName = character.name;
			GameData.instance.SAVE_STATE.herotag = character.herotag;
			GameData.instance.SAVE_STATE.heroType = ((character.imxG0Data != null) ? character.imxG0Data.name : "Basic Hero");
			GameData.instance.SAVE_STATE.playerID = character.playerID;
		}
		AppInfo.doKongregateCommonFields(CommonPropsCallback());
	}

	public static void trackPaymentStart(string productID, string jsonMap)
	{
		if (AppInfo.kongApi == null || AppInfo.kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			AppInfo.kongApi.Analytics.StartPurchase(productID, 1, jsonMap);
		}
		catch (Exception ex)
		{
			D.LogError("all", "KongregateAnalytics::trackPaymentStart " + ex.Message);
		}
	}

	public static void trackPaymentReceiptFail(string jsonMap)
	{
		if (AppInfo.kongApi == null || AppInfo.kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			AppInfo.kongApi.Analytics.FinishPurchase("RECEIPT_FAIL", "", jsonMap, "");
		}
		catch (Exception ex)
		{
			D.LogError("all", "KongregateAnalytics::trackPaymentReceiptFail " + ex.Message);
		}
	}

	public static void trackPaymentFail(string jsonMap)
	{
		if (AppInfo.kongApi == null || AppInfo.kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			AppInfo.kongApi.Analytics.FinishPurchase("FAIL", "", jsonMap, "");
		}
		catch (Exception ex)
		{
			D.LogError("all", "KongregateAnalytics::trackPaymentFail " + ex.Message);
		}
	}

	public static void trackPaymentSuccess(string transactionID, string jsonMap, string dataSignature = "")
	{
		if (AppInfo.kongApi == null || AppInfo.kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			AppInfo.kongApi.Analytics.FinishPurchase("SUCCESS", transactionID, jsonMap, dataSignature);
		}
		catch (Exception ex)
		{
			D.LogError("all", "KongregateAnalytics::trackPaymentSuccess " + ex.Message);
		}
	}

	public static void trackTutorialStep(TutorialData data)
	{
		if (data != null)
		{
			string jsonMap = JsonConvert.SerializeObject(new KongregateAnalyticsSchema.TutorialStepEnd
			{
				tutorial_type = data.name,
				description = data.desc,
				step_number = data.step,
				is_final = data.finalized
			});
			AppInfo.doKongregateAnalyticsEvent("tutorial_step_ends", jsonMap);
		}
	}

	public static void trackNavigationAction(NavigationData navData)
	{
		string jsonMap = JsonConvert.SerializeObject(new KongregateAnalyticsSchema.NavigationActions
		{
			origin = navData.nav_element_origin,
			nav_element_name = navData.nav_element_name,
			sub_nav_element_name = navData.sub_nav_element_name
		});
		AppInfo.doKongregateAnalyticsEvent("nav_actions", jsonMap);
	}

	public static void trackCrossPromotion()
	{
	}

	public static string getPaymentGameFields(string type, int creditsChange = 0, int goldChange = 0, string context = null, float discount = 0f)
	{
		if (context == null)
		{
			context = getCurrentEconomyContext();
		}
		return JsonConvert.SerializeObject(new KongregateAnalyticsSchema.PaymentFields
		{
			hard_currency_change = creditsChange,
			soft_currency_change = goldChange,
			type = type,
			discount_percent = discount,
			context_of_offer = context
		});
	}

	private static string getCurrentEconomyContext()
	{
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(ServiceWindow)))
		{
			return "Service";
		}
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(ShopWindow)))
		{
			return "Shop";
		}
		return "Game";
	}

	private static string GetCPETokenForEvent(string eventName)
	{
		try
		{
			JSONNode configValue = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("cpe");
			if (configValue != null)
			{
				string text = configValue[eventName];
				if (!string.IsNullOrEmpty(text))
				{
					return "adjust." + text;
				}
			}
		}
		catch (Exception e)
		{
			D.LogException("CPEEvent", e);
		}
		return null;
	}

	public static void TrackCPEEvent(string eventName)
	{
		string cPETokenForEvent = GetCPETokenForEvent(eventName);
		if (!string.IsNullOrEmpty(cPETokenForEvent))
		{
			D.Log("Tracking CPE Event: " + eventName);
			AppInfo.doKongregateAnalyticsEvent(cPETokenForEvent);
		}
		else
		{
			D.Log("Tracking CPE Event: " + eventName + " - Token Not Found");
		}
	}

	public static void TrackSwitchHeroes(int oldHeroID, string oldHeroName, string oldHeroType, int newHeroID, string newHeroName, string newHeroType)
	{
		if (oldHeroID > 0 && !string.IsNullOrEmpty(oldHeroName) && !string.IsNullOrEmpty(oldHeroType) && newHeroID > 0 && !string.IsNullOrEmpty(newHeroName) && !string.IsNullOrEmpty(newHeroType))
		{
			string jsonMap = JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					"old_hero_id",
					$"{oldHeroID}|{oldHeroName}|{oldHeroType}"
				},
				{
					"new_hero_id",
					$"{newHeroID}|{newHeroName}|{newHeroType}"
				}
			});
			AppInfo.doKongregateAnalyticsEvent("hero_switches", jsonMap);
		}
	}

	public static string GetCurrentEconomyContextLocation()
	{
		if (GameData.instance.PROJECT == null)
		{
			return "Game";
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			return "Battle";
		}
		if (GameData.instance.PROJECT.dungeon != null)
		{
			return "Dungeon";
		}
		if (GameData.instance.PROJECT.instance != null)
		{
			return "Town";
		}
		return "Game";
	}

	public static Dictionary<string, object> CommonPropsCallback()
	{
		int num = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.playerID : GameData.instance.SAVE_STATE.playerID);
		int num2 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.id : GameData.instance.SAVE_STATE.characterID);
		int num3 = ((GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null) ? GameData.instance.SAVE_STATE.characterGuildID : ((GameData.instance.PROJECT.character.guildData != null) ? GameData.instance.PROJECT.character.guildData.id : 0));
		int num4 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.gold : GameData.instance.SAVE_STATE.characterGold);
		int num5 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.credits : GameData.instance.SAVE_STATE.characterCredits);
		int num6 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.level : GameData.instance.SAVE_STATE.characterLevel);
		int num7 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energy : GameData.instance.SAVE_STATE.characterEnergy);
		int num8 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.energyMax : GameData.instance.SAVE_STATE.characterEnergyMax);
		int num9 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.tickets : GameData.instance.SAVE_STATE.characterTickets);
		int num10 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.ticketsMax : GameData.instance.SAVE_STATE.characterTicketsMax);
		int num11 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.getTotalStats() : GameData.instance.SAVE_STATE.characterTotalStats);
		int num12 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.getTotalPower() : GameData.instance.SAVE_STATE.characterTotalPower);
		int num13 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.getTotalStamina() : GameData.instance.SAVE_STATE.characterTotalStamina);
		int num14 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.getTotalAgility() : GameData.instance.SAVE_STATE.characterTotalAgility);
		string text = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.name : GameData.instance.SAVE_STATE.characterName);
		string text2 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.herotag : GameData.instance.SAVE_STATE.herotag);
		string value = text + "#" + text2;
		string value2 = ((GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null) ? GameData.instance.SAVE_STATE.heroType : ((GameData.instance.PROJECT.character.imxG0Data != null) ? GameData.instance.PROJECT.character.imxG0Data.name : "Basic Hero"));
		int num15 = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.admin) ? 1 : 0);
		BHAnalytics bHAnalytics = ((GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null) ? GameData.instance.PROJECT.character.analytics : new BHAnalytics(num2));
		return new Dictionary<string, object>
		{
			{ "soft_currency_balance", num4 },
			{
				"soft_currency_bought",
				bHAnalytics.getValue(BHAnalytics.GOLD_BOUGHT)
			},
			{
				"soft_currency_spent",
				bHAnalytics.getValue(BHAnalytics.GOLD_SPENT)
			},
			{
				"soft_currency_earned",
				bHAnalytics.getValue(BHAnalytics.GOLD_EARNED)
			},
			{ "hard_currency_balance", num5 },
			{
				"hard_currency_bought",
				bHAnalytics.getValue(BHAnalytics.CREDITS_BOUGHT)
			},
			{
				"hard_currency_spent",
				bHAnalytics.getValue(BHAnalytics.CREDITS_SPENT)
			},
			{
				"hard_currency_earned",
				bHAnalytics.getValue(BHAnalytics.CREDITS_EARNED)
			},
			{
				"num_pve_played",
				bHAnalytics.getValue(BHAnalytics.DUNGEONS_PLAYED)
			},
			{
				"num_pvp_played",
				bHAnalytics.getValue(BHAnalytics.PVP_BATTLES_PLAYED)
			},
			{
				"num_pve_won",
				bHAnalytics.getValue(BHAnalytics.DUNGEONS_WON)
			},
			{
				"num_pvp_won",
				bHAnalytics.getValue(BHAnalytics.PVP_BATTLES_WON)
			},
			{ "current_pve_energy", num7 },
			{ "current_pvp_energy", num9 },
			{ "max_pve_energy", num8 },
			{ "max_pvp_energy", num10 },
			{ "player_total_stats", num11 },
			{ "player_power", num12 },
			{ "player_stamina", num13 },
			{ "player_agility", num14 },
			{ "game_username", text },
			{ "player_level", num6 },
			{ "guild_id", num3 },
			{ "hero_id", num2 },
			{ "hero_name", value },
			{ "hero_type", value2 },
			{ "server_player_id", num },
			{
				"server_event_time_gmt",
				ServerExtension.instance.GetDate()
			},
			{ "player_state", num15 },
			{ "client_build_platform", "unity" },
			{
				"application_version",
				Application.version.ToString()
			},
			{
				"client_build_target",
				GetClientBuildTarget()
			}
		};
	}

	private static string GetClientBuildTarget()
	{
		return AppInfo.platform switch
		{
			4 => "kongregate", 
			7 => "steam", 
			1 => "android", 
			2 => "ios", 
			8 => "kartridge", 
			_ => "local", 
		};
	}
}
