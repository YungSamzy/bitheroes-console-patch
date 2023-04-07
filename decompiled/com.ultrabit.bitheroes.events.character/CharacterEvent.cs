using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.character;

public class CharacterEvent : CustomSFSXEvent
{
	public const string FRIEND_CHANGE = "FRIEND_CHANGE";

	public const string REQUEST_CHANGE = "REQUEST_CHANGE";

	public const string NAME_CHANGE = "NAME_CHANGE";

	public const string LEVEL_CHANGE = "LEVEL_CHANGE";

	public const string EXP_CHANGE = "EXP_CHANGE";

	public const string CURRENCY_CHANGE = "CURRENCY_CHANGE";

	public const string POINTS_CHANGE = "POINTS_CHANGE";

	public const string STATS_CHANGE = "STATS_CHANGE";

	public const string APPEARANCE_CHANGE = "APPEARANCE_CHANGE";

	public const string INVENTORY_CHANGE = "INVENTORY_CHANGE";

	public const string ENERGY_CHANGE = "ENERGY_CHANGE";

	public const string ENERGY_MILLISECONDS_CHANGE = "ENERGY_MILLISECONDS_CHANGE";

	public const string ENERGY_SECONDS_CHANGE = "ENERGY_SECONDS_CHANGE";

	public const string TICKETS_CHANGE = "TICKETS_CHANGE";

	public const string TICKETS_MILLISECONDS_CHANGE = "TICKETS_MILLISECONDS_CHANGE";

	public const string TICKETS_SECONDS_CHANGE = "TICKETS_SECONDS_CHANGE";

	public const string CHANGENAME_CHANGE = "CHANGENAME_CHANGE";

	public const string SHARDS_CHANGE = "SHARDS_CHANGE";

	public const string SHARDS_MILLISECONDS_CHANGE = "SHARDS_MILLISECONDS_CHANGE";

	public const string SHARDS_SECONDS_CHANGE = "SHARDS_SECONDS_CHANGE";

	public const string TOKENS_CHANGE = "TOKENS_CHANGE";

	public const string TOKENS_MILLISECONDS_CHANGE = "TOKENS_MILLISECONDS_CHANGE";

	public const string TOKENS_SECONDS_CHANGE = "TOKENS_SECONDS_CHANGE";

	public const string BADGES_CHANGE = "BADGES_CHANGE";

	public const string BADGES_MILLISECONDS_CHANGE = "BADGES_MILLISECONDS_CHANGE";

	public const string BADGES_SECONDS_CHANGE = "BADGES_SECONDS_CHANGE";

	public const string AUTO_PILOT_CHANGE = "AUTO_PILOT_CHANGE";

	public const string CONSUMABLE_MODIFIER_CHANGE = "CONSUMABLE_MODIFIER_CHANGE";

	public const string CHAT_CHANGE = "CHAT_CHANGE";

	public const string SHOP_ROTATION_ID_CHANGE = "SHOP_ROTATION_ID_CHANGE";

	public const string SHOP_ROTATION_MILLISECONDS_CHANGE = "SHOP_ROTATION_MILLISECONDS_CHANGE";

	public const string SHOP_ROTATION_SECONDS_CHANGE = "SHOP_ROTATION_SECONDS_CHANGE";

	public const string GUILD_CHANGE = "GUILD_CHANGE";

	public const string GUILD_INVITE_CHANGE = "GUILD_INVITE_CHANGE";

	public const string GUILD_MEMBER_CHANGE = "GUILD_MEMBER_CHANGE";

	public const string GUILD_RANK_CHANGE = "GUILD_RANK_CHANGE";

	public const string GUILD_PERMISSIONS_CHANGE = "GUILD_PERMISSIONS_CHANGE";

	public const string GUILD_PERKS_CHANGE = "GUILD_PERKS_CHANGE";

	public const string FAMILIAR_STABLE_CHANGE = "FAMILIAR_STABLE_CHANGE";

	public const string DAILY_QUEST_CHANGE = "DAILY_QUEST_CHANGE";

	public const string AD_MILLISECONDS_CHANGE = "AD_MILLISECONDS_CHANGE";

	public const string AD_READY = "AD_READY";

	public const string RUNES_CHANGE = "RUNES_CHANGE";

	public const string NBP_DATE_CHANGE = "NBP_DATE_CHANGE";

	public const string ENCHANTS_CHANGE = "ENCHANTS_CHANGE";

	public const string AUGMENTS_CHANGE = "AUGMENTS_CHANGE";

	public const string MOUNTS_CHANGE = "MOUNTS_CHANGE";

	public const string USER_TOKEN_UPDATE = "USER_TOKEN_UPDATE";

	public const string LOCKED_ITEMS_CHANGE = "LOCKED_ITEMS_CHANGE";

	public const string DAILY_FISHING_DATE_CHANGE = "DAILY_FISHING_DATE_CHANGE";

	public const string SEALS_CHANGE = "SEALS_CHANGE";

	public const string SEALS_MILLISECONDS_CHANGE = "SEALS_MILLISECONDS_CHANGE";

	public const string SEALS_SECONDS_CHANGE = "SEALS_SECONDS_CHANGE";

	public const string ARMORY_TO_EQUIPMENT = "ARMORY_TO_EQUIPMENT";

	public const string BOOSTERS_CHANGED = "BOOSTERS_CHANGED";

	private string _eventType;

	public CharacterEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new CharacterEvent(eventType, _dispatcher);
	}
}
