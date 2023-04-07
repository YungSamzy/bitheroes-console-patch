using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;

namespace com.ultrabit.bitheroes.parsing.model.utility;

public class ErrorCode
{
	public const int NONE = 0;

	public const int LOGIN_ERROR = 2;

	public const int ALREADY_LOGGED_OUT = 8;

	public const int NO_MAINHAND = 19;

	public const int NOT_ENOUGH_GOLD = 20;

	public const int NOT_ENOUGH_CREDITS = 21;

	public const int NOT_ENOUGH_ENERGY = 22;

	public const int NOT_ENOUGH_TICKETS = 23;

	public const int MOBILE_VERSION = 24;

	public const int SWF_VERSION = 25;

	public const int EVENT_NOT_ACTIVE = 27;

	public const int DUNGEON_CONSUMABLE_LIMIT = 31;

	public const int CHAT_MUTED = 37;

	public const int PLAYER_OFFLINE = 39;

	public const int NOT_ENOUGH_SHARDS = 44;

	public const int NOT_IN_A_GUILD = 53;

	public const int INVALID_GUILD_PERMISSIONS = 58;

	public const int NOT_ENOUGH_HONOR = 64;

	public const int BATTLE_CONSUMABLE_LIMIT = 68;

	public const int BATTLE_CONSUMABLE_DISABLED = 69;

	public const int NO_CONSUMABLE_TARGETS = 78;

	public const int FUSION_FAMILIARS_REQUIRED = 95;

	public const int NOT_ENOUGH_TOKENS = 96;

	public const int HAS_RUNE_SLOT_MEMORY = 97;

	public const int MAX_GUILD_EVENT_PLAYERS = 101;

	public const int DUEL_TARGET_ANOTHER_SERVER = 102;

	public const int NOT_ENOUGH_BADGES = 104;

	public const int ENTER_GUILD_EVENT_CONFIRM = 108;

	public const int ALREADY_IN_DUNGEON = 109;

	public const int ALREADY_IN_BATTLE = 110;

	public const int FAMILIAR_STABLE_MAX = 111;

	public const int MAX_ENCHANTS = 112;

	public const int ALREADY_PURCHASED = 116;

	public const int MAX_MOUNTS = 117;

	public const int NOT_ENOUGH_SEASHELLS = 118;

	public const int MAX_AUGMENTS = 119;

	public const int NOT_ENOUGH_CHANGENAME = 122;

	public const int CHANGENAME_COOLDOWN = 123;

	public const int NOT_ENOUGH_SEALS = 127;

	public const int CHARACTER_DOESNT_EXIST = 128;

	public const int NOT_ENOUGH_EVENT_SALES_CURRENCY = 129;

	public const int ROOM_NOT_FOUND = 130;

	private static string getLink(int code)
	{
		return "error_code_" + code + "_desc";
	}

	public static string getErrorMessage(int errorCode)
	{
		string link = getLink(errorCode);
		switch (errorCode)
		{
		case 19:
			return Language.GetString(link, new string[1] { EquipmentRef.getEquipmentTypeName(1) });
		case 20:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(1) });
		case 21:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(2) });
		case 22:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(4) });
		case 23:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(5) });
		case 31:
			return Language.GetString(link, new string[2]
			{
				Util.NumberFormat(VariableBook.dungeonConsumableLimit),
				ItemRef.GetItemName(4)
			});
		case 44:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(8) });
		case 127:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(13) });
		case 64:
			return Language.GetString(link, new string[1] { CurrencyRef.GetCurrencyName(7) });
		case 68:
		case 69:
		case 78:
			return Language.GetString(link, new string[1] { ItemRef.GetItemNamePlural(4) });
		case 95:
			return Language.GetString(link, new string[2]
			{
				VariableBook.fusionFamiliarsRequired.ToString(),
				ItemRef.GetItemNamePlural(6)
			});
		case 96:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(9) });
		case 97:
			return Language.GetString(link, new string[1] { ItemRef.GetItemName(9) });
		case 101:
			return Language.GetString(link, new string[1] { GameData.instance.PROJECT.character.getGuildMemberLimit().ToString() });
		case 104:
			return Language.GetString("error_not_enough_currency", new string[1] { CurrencyRef.GetCurrencyName(10) });
		case 111:
			return Language.GetString(link, new string[3]
			{
				Util.NumberFormat(VariableBook.familiarStableMaxQty),
				ItemRef.GetItemName(6),
				Language.GetString("ui_stable")
			});
		case 112:
			return Language.GetString(link, new string[2]
			{
				Util.NumberFormat(VariableBook.enchantMax),
				ItemRef.GetItemNamePlural(11)
			});
		case 117:
			return Language.GetString(link, new string[2]
			{
				Util.NumberFormat(VariableBook.mountMax),
				ItemRef.GetItemNamePlural(8)
			});
		case 118:
			return Language.GetString(link, new string[1] { VariableBook.fishingShopItem.coloredName });
		case 119:
			return Language.GetString(link, new string[2]
			{
				Util.NumberFormat(VariableBook.augmentMax),
				ItemRef.GetItemNamePlural(15)
			});
		case 123:
			return Language.GetString(link, new string[1] { Util.TimeFormat((int)(GameData.instance.PROJECT.character.changenameCooldown / 1000), isLong: true) }, color: true);
		default:
			return Language.GetString(link);
		}
	}
}
