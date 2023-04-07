using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.material;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.character;

public class Tutorial : MonoBehaviour
{
	public const int ZONE_BUTTON_DIALOG = 0;

	public const int ZONE_BUTTON = 1;

	public const int ZONE_BUTTON_CONTINUE = 2;

	public const int ZONE_NODE = 3;

	public const int ZONE_NODE_ENTER = 4;

	public const int ZONE_NODE_FAMILIAR = 5;

	public const int ZONE_NODE_PLAYERS = 6;

	public const int DUNGEON_NODE = 7;

	public const int BATTLE_ABILITY_BASIC = 8;

	public const int BATTLE_ABILITY_ADVANCED = 9;

	public const int BATTLE_ABILITY_CONSERVE = 10;

	public const int BATTLE_CONSUMABLES = 11;

	public const int ITEM_EQUIP_ICON = 12;

	public const int ITEM_EQUIP_BUTTON = 13;

	public const int LEVEL_UP = 14;

	public const int STAT_POWER = 15;

	public const int STAT_STAMINA = 16;

	public const int STAT_AGILITY = 17;

	public const int FAMILIAR_CAPTURE_FIRST = 18;

	public const int FAMILIAR_CAPTURE_SECOND = 19;

	public const int TEAM_SELECT_BUTTON = 20;

	public const int PVP_BUTTON = 21;

	public const int PVP_PLAY_BUTTON = 22;

	public const int AUTO_PILOT_BUTTON = 23;

	public const int CRAFT_BUTTON = 24;

	public const int CRAFT_UPGRADE_ICON = 25;

	public const int CRAFT_UPGRADE_BUTTON = 26;

	public const int CRAFT_EXCHANGE_TAB = 27;

	public const int SHOP_BUTTON = 28;

	public const int SHOP_ITEM_TILE = 29;

	public const int ITEM_PURCHASE_BUTTON = 30;

	public const int SHOP_ITEM_CONFIRM = 31;

	public const int GUILD_BUTTON = 32;

	public const int ITEM_UPGRADE_BUTTON = 33;

	public const int EXTRA_STATS_ICON = 34;

	public const int EXTRA_STATS_BUTTON = 35;

	public const int DAILY_REWARDS = 36;

	public const int DAILY_REWARDS_BUTTON = 37;

	public const int NEWS = 38;

	public const int CONSUMABLE_USE_ICON = 39;

	public const int PET_EQUIP_ICON = 40;

	public const int INVENTORY_UPGRADE_TILE = 41;

	public const int INVENTORY_UPGRADE_ICON = 42;

	public const int ZONE_COMPLETE = 43;

	public const int BATTLE_TARGET_SELECT = 44;

	public const int ITEM_ABILITY_UPGRADE_ICON = 45;

	public const int ZONE_NODE_DEFEAT = 46;

	public const int ABILITY_DESCRIPTION_MOBILE = 47;

	public const int DAILY_QUEST_BUTTON = 48;

	public const int DAILY_QUEST_LOOT = 49;

	public const int MOUNT_MATERIAL = 50;

	public const int MOUNT_SUMMON = 51;

	public const int MOUNT_EQUIP = 52;

	public const int MOUNT_SUMMON_BUTTON = 53;

	public const int FISHING_START = 54;

	public const int FISHING_CASTING = 55;

	public const int FISHING_CATCHING = 56;

	public const int FISHING_BUTTON = 57;

	public const int FISHING_BAIT = 58;

	public const int FISHING_TRAVEL = 59;

	public const int AUGMENT_IDENTIFY = 60;

	public const int AUGMENT_EQUIP = 61;

	public const int BOOSTER = 62;

	public const int RUNE_BUTTON = 63;

	public const int ENCHANT_BUTTON = 64;

	public const int SCHEMATICS_FAMILIAR_BUTTON = 65;

	public const int SCHEMATICS_FUSION_BUTTON = 66;

	public const int SCHEMATICS_FUSE_BUTTON = 67;

	public const int AUGMENTS_FAMILIAR_BUTTON = 68;

	public const int AUGMENTS_CRAFT_BUTTON = 69;

	public const int AUGMENTS_CRAFT_TRADE = 70;

	public const int AUGMENTS_TRADE = 71;

	public const int AUGMENTS_AUGMENTS_BUTTON = 72;

	public const int AUGMENTS_IDENTIFY_BUTTON = 73;

	public const int AUGMENTS_IDENTIFY_TILE = 74;

	public const int AUGMENTS_EQUIP_BUTTON = 75;

	public const int PET_CHARACTER_TILE_BUTTON = 76;

	public const int PET_PLACEHOLDER_BUTTON = 77;

	public const int PET_EQUIP_BUTTON = 78;

	public const int PET_UPGRADE_BUTTON = 79;

	public const int ACCESSORY_CHARACTER_TILE_BUTTON = 80;

	public const int ACCESSORY_PLACEHOLDER_BUTTON = 81;

	public const int ACCESSORY_EQUIP_BUTTON = 82;

	public const int ACCESSORY_UPGRADE_BUTTON = 83;

	public const int MOUNT_CHARACTER_TILE_BUTTON = 84;

	public const int MOUNT_PLACEHOLDER_BUTTON = 85;

	public const int RUNES_CRAFT_BUTTON = 86;

	public const int RUNES_CRAFT_TRADE = 87;

	public const int RUNES_TRADE = 88;

	public const int RUNES_CHARACTER_TILE_BUTTON = 89;

	public const int RUNES_RUNES_BUTTON = 90;

	public const int RUNES_RUNE_TILE = 91;

	public const int RUNES_EQUIP = 92;

	public const int TEAM_ADD_BUTTON = 93;

	public const int ENCHANTS_CHARACTER_TILE_BUTTON = 94;

	public const int ENCHANTS_ENCHANTS_BUTTON = 95;

	public const int ENCHANTS_IDENTIFY_BUTTON = 96;

	public const int ENCHANTS_IDENTIFY_TILE = 97;

	public const int ENCHANTS_EQUIP = 98;

	public const int SCHEMATICS_DIALOG = 99;

	public const int CRAFT_DIALOG = 100;

	public const int ADGOR_TILE_BUTTON = 101;

	public const int ADGOR_MOBILE_WATCH_BUTTON = 102;

	public const int CRAFT_UPGRADE_CHARACTER_TILE_BUTTON = 103;

	public const int CRAFT_UPGRADE_ITEM_TILE_BUTTON = 104;

	public const int CRAFT_UPGRADE_UPGRADE_BUTTON = 105;

	public const int CRAFT_UPGRADE_UPGRADE_UPGRADE_BUTTON = 106;

	public const int CRAFT_UPGRADE_DIALOG = 107;

	public const int FAMILIAR_CAPTURE_THIRD = 108;

	public const int SCHEMATICS_USE_ICON = 109;

	public const int CRAFT_UPGRADE_ITEM_VIEW_BUTTON = 110;

	public const int ENCHANTS_ITEM_IDENTIFY_BUTTON = 111;

	public const int MOUNT_SUMMON_TOOLTIP_BUTTON = 112;

	public const int RUNES_SELECT_BUTTON = 113;

	public const int AUGMENTS_IDENTIFY_TOOLTIP_BUTTON = 114;

	public const int CRAFT_EXCHANGE_ITEMICON = 115;

	public const int CRAFT_EXCHANGE_ITEMICON_MOBILE = 116;

	public const int CRAFT_EXCHANGE_BUTTON = 117;

	public const int PET_TOOLTIP_EQUIP_BUTTON = 118;

	public const int ACCESSORY_TOOLTIP_EQUIP_BUTTON = 119;

	public const int CONSUMABLE_USE_ICON_TOOLTIP = 120;

	public const int TEAM_ADD_BUTTON_FUSION = 121;

	public const int REFORGE_CRAFT_BUTTON = 122;

	public const int REFORGE_TAB = 123;

	public const int REFORGE_ICON = 124;

	public const int REFORGE_ICON_TOOLTIP = 125;

	public const int REFORGE_BUTTON = 126;

	public const int FRIENDS_BUTTON = 127;

	public const int FRIENDS_SUGGEST_BUTTON = 128;

	public const int FRIENDS_ADD_BUTTON = 129;

	public const int ACCOUNT_REACTIVATION = 130;

	public const int ACHIEVEMENTS_BUTTON = 131;

	public const int ACHIEVEMENT_CLAIM = 132;

	public const int TOTAL_STEPS = 133;

	public static Transform tutorialBgPrefab;

	private static Dictionary<int, TutorialData> DATA;

	private static int STEP;

	private static RectTransform PLACEHOLDER;

	private static GameObject CONTAINER;

	private static GameObject OBJECT;

	private static Messenger DISPATCHER;

	private static string EVENT;

	private static Action<object> FUNC;

	private static string MOUSE;

	private static GameObject TRIGGER;

	private static GameObject POPUP;

	private static TutorialPopup TUTORIAL;

	private Dictionary<int, bool> _states = new Dictionary<int, bool>();

	private bool _changed;

	private bool _doUpdate;

	public Dictionary<int, bool> states => _states;

	public bool changed
	{
		get
		{
			return _changed;
		}
		set
		{
			_changed = value;
		}
	}

	public static void Init()
	{
		STEP = 0;
		DATA = new Dictionary<int, TutorialData>();
		string text = "Quest Intro";
		SetData(0, text, "Quest Dialog", finalized: false);
		SetData(1, text, "Selecting UI Quest Button", finalized: false);
		SetData(3, text, "Selecting node on Zone map", finalized: false);
		SetData(4, text, "Entering Zone map Node", finalized: true);
		string text2 = "Dungeon Intro";
		SetData(7, text2, "Dungeon movement instructions", finalized: true);
		string text3 = "Battle Intro";
		SetData(8, text3, "First ability", finalized: false);
		SetData(9, text3, "Second ability", finalized: true);
		string text4 = "Familiar First";
		SetData(18, text4, "First Familiar capture", finalized: true);
		string text5 = "Quest Team";
		SetData(5, text5, "Selecting node on Zone map with Familiar", finalized: false);
		SetData(20, text5, "Entering Zone map Node with team", finalized: true);
		string text6 = "Familiar Second";
		SetData(19, text6, "Second Familiar capture", finalized: true);
		string text7 = "Daily Rewards";
		SetData(37, text7, "Claiming Daily Reward", finalized: true);
		string text8 = "Daily Quests";
		SetData(49, text8, "Looting Daily Quest", finalized: true);
		string text9 = "Craft Intro";
		SetData(24, text9, "Selecting UI Craft Button", finalized: false);
		SetData(25, text9, "Selecting upgrade item", finalized: false);
		SetData(26, text9, "Upgrading item", finalized: true);
		string text10 = "Quest Social";
		SetData(6, text10, "Selecting node on Zone map with Friends", finalized: true);
		string text11 = "Shop Intro";
		SetData(28, text11, "Selecting UI Shop Button", finalized: false);
		SetData(29, text11, "Selecting purchase item", finalized: true);
		string text12 = "Guild Intro";
		SetData(32, text12, "Selecting UI Guild Button", finalized: true);
		string text13 = "PvP Intro";
		SetData(21, text13, "Selecting UI PvP Button", finalized: false);
		SetData(22, text13, "Entering PvP", finalized: true);
		SetData(130, text, "Account Reactivation", finalized: false);
	}

	public Tutorial(Dictionary<int, bool> states)
	{
		_states = states;
	}

	public bool GetState(int id)
	{
		if (_states.ContainsKey(id))
		{
			return _states[id];
		}
		return false;
	}

	public void SetState(int id, bool state = true)
	{
		if (_states.ContainsKey(id))
		{
			changed = _states[id] != state;
			_states[id] = state;
		}
		else
		{
			_states.Add(id, state);
			_changed = true;
		}
		if (changed && state)
		{
			KongregateAnalytics.trackTutorialStep(GetData(id));
		}
	}

	private static TutorialData GetData(int id)
	{
		if (!DATA.ContainsKey(id))
		{
			return null;
		}
		return DATA[id];
	}

	private static void SetData(int id, string name, string desc, bool finalized)
	{
		STEP++;
		TutorialData value = new TutorialData(id, STEP, name, desc, finalized);
		DATA[id] = value;
	}

	public static string GetText(int id)
	{
		string link = "tutorial_" + id + "_desc";
		switch (id)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 7:
		case 13:
		case 20:
		case 22:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 30:
		case 33:
		case 34:
		case 35:
		case 42:
		case 43:
		case 57:
		case 58:
		case 59:
		case 62:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 72:
		case 73:
		case 74:
		case 75:
		case 78:
		case 82:
		case 84:
		case 85:
		case 86:
		case 87:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 101:
		case 102:
		case 103:
		case 104:
		case 106:
		case 109:
		case 110:
		case 111:
		case 112:
		case 113:
		case 114:
		case 115:
		case 116:
		case 117:
		case 120:
		case 121:
		case 122:
		case 123:
		case 124:
		case 125:
		case 126:
		case 127:
		case 128:
		case 129:
		case 131:
		case 132:
			return Language.GetString(link, new string[1] { GetCursorText() });
		case 5:
		case 6:
		case 11:
		case 21:
		case 32:
		case 37:
		case 39:
		case 41:
		case 46:
		case 71:
		case 88:
		case 105:
		case 108:
			return Language.GetString(link);
		case 8:
		case 44:
		case 47:
			return Language.GetString(link, new string[1] { Language.GetString("ability_name") });
		case 9:
			return Language.GetString(link, new string[2]
			{
				Language.GetString("ability_plural_name"),
				Language.GetString("ability_meter_short_name")
			});
		case 10:
			return Language.GetString(link, new string[3]
			{
				Language.GetString("ability_plural_name"),
				Language.GetString("ability_meter_short_name"),
				Language.GetString("ability_plural_name")
			});
		case 12:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				AppInfo.IsMobile() ? Language.GetString("ui_view").ToLowerInvariant() : Language.GetString("ui_equip").ToLowerInvariant()
			});
		case 14:
			return Language.GetString(link, new string[4]
			{
				VariableBook.energyIncrease.ToString(),
				CurrencyRef.GetCurrencyName(4),
				VariableBook.characterPointIncrease,
				GetCursorText()
			});
		case 15:
			return Language.GetString(link, new string[1] { Language.GetString("stat_power") }, color: true);
		case 16:
			return Language.GetString(link, new string[1] { Language.GetString("stat_stamina") }, color: true);
		case 17:
			return Language.GetString(link, new string[1] { Language.GetString("stat_agility") }, color: true);
		case 18:
			return Language.GetString(link, new string[2]
			{
				ItemRef.GetItemName(6),
				GetCursorText()
			});
		case 19:
			return Language.GetString(link, new string[1] { ItemRef.GetItemName(6) });
		case 23:
			return Language.GetString(link, new string[1] { GetCursorText(lowercase: true) });
		case 29:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				AppInfo.IsMobile() ? Language.GetString("ui_view").ToLowerInvariant() : Language.GetString("ui_buy").ToLowerInvariant()
			});
		case 40:
		case 76:
			return Language.GetString(link, new string[2]
			{
				EquipmentRef.getEquipmentTypeName(8),
				EquipmentRef.GetEquipmentTypeNamePlural(8)
			});
		case 45:
			return Language.GetString(link, new string[1] { Language.GetString("ability_plural_name") });
		case 48:
			return Language.GetString(link, new string[2]
			{
				Language.GetString("daily_quest_name"),
				GetCursorText()
			});
		case 49:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				Language.GetString("daily_quest_name")
			});
		case 50:
			return Language.GetString(link, new string[1] { ItemRef.GetItemNamePlural(8) });
		case 51:
			return Language.GetString(link, new string[1] { ItemRef.GetItemName(8) });
		case 52:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				ItemRef.GetItemName(8)
			});
		case 53:
			return Language.GetString(link, new string[3]
			{
				GetCursorText(),
				Language.GetString("ui_summon"),
				ItemRef.GetItemName(8)
			});
		case 54:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				Language.GetString("ui_start")
			});
		case 55:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				Language.GetString("ui_cast")
			});
		case 56:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				Language.GetString("ui_catch")
			});
		case 60:
			return Language.GetString(link, new string[1] { ItemRef.GetItemName(15) });
		case 61:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				ItemRef.GetItemName(15)
			});
		case 77:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				EquipmentRef.GetEquipmentTypeNamePlural(8)
			});
		case 79:
			return Language.GetString(link, new string[1] { Language.GetString(MaterialBook.Lookup(13).coloredName) });
		case 80:
			return Language.GetString(link, new string[2]
			{
				EquipmentRef.getEquipmentTypeName(7),
				EquipmentRef.GetEquipmentTypeNamePlural(7)
			});
		case 81:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				EquipmentRef.GetEquipmentTypeNamePlural(7)
			});
		case 83:
			return Language.GetString(link, new string[1] { Language.GetString(MaterialBook.Lookup(14).coloredName) });
		case 118:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				ItemRef.GetItemName(1, 8)
			});
		case 119:
			return Language.GetString(link, new string[2]
			{
				GetCursorText(),
				ItemRef.GetItemName(1, 7)
			});
		default:
			return Language.GetString("ui_question_mark");
		}
	}

	public static string GetCursorText(bool lowercase = false)
	{
		string text = (AppInfo.IsMobile() ? Language.GetString("ui_tap") : Language.GetString("ui_click"));
		if (lowercase)
		{
			text = text.ToLowerInvariant();
		}
		return text;
	}

	public static Tutorial fromSFSObject(ISFSObject sfsob)
	{
		bool[] boolArray = sfsob.GetBoolArray("cha11");
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		for (int i = 0; i < boolArray.Length; i++)
		{
			dictionary.Add(i, boolArray[i]);
		}
		return new Tutorial(dictionary);
	}
}
