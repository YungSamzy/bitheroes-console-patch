using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.variable;

public class VariableBook
{
	private static List<AbilityRef> _abilitiesDefault;

	private static List<ChatChannel> _chatChannels;

	private static List<Vector2> _resolutions;

	private static List<GameRequirement> _gameRequirements;

	private static ItemRef _fishingShopItem;

	private static List<ItemRewardRef> _rewards;

	private static CharacterBase _characterBase;

	private static List<GameModifier> _modifiersBase;

	private static AdgorRef _adgorRef;

	private static List<int> _characterBaseForceConsumableIds;

	private static int _friendsMax;

	private static int _battleMeterMax;

	private static float _battleSpeedMultiplier;

	private static float _battleMilliseconds;

	private static float _shieldMult;

	private static string _tutorialDialogEnchants;

	private static int _enchantMax;

	private static int _mountMax;

	private static int _characterNameLength;

	private static string[] _shopQuantities;

	private static int _dailyQuestLimit;

	private static int _worldChatMessageLimit;

	private static int _worldChatInputLength;

	private static int _guildChatMessageLimit;

	private static string _tutorialDialogBrawl;

	private static string _tutorialDialogRaid;

	private static string _tutorialDialogGvE;

	private static string _tutorialDialogRifts;

	private static string _tutorialDialogGauntlet;

	private static string _tutorialDialogFishing;

	private static string _tutorialDialogGvG;

	private static string _tutorialDialogFusion;

	private static string _tutorialDialogCraft;

	private static string _tutorialDialogUpgrade;

	private static string _tutorialDialogInvasion;

	private static string _tutorialDialogFamiliarStable;

	private static int _guildNameLength;

	private static int _guildInitialsLength;

	private static int _guildChatInputLength;

	private static int _familiarStableMaxQty;

	private static bool _brawlVisible;

	private static bool _raidVisible;

	private static int _augmentMax;

	private static int _gveEventDifficultyIntervals;

	private static string _tutorialDialogIntro;

	private static List<int> _chatMuteSeconds;

	private static int _chatMuteSecondsModeratorLimit;

	private static List<string> _chatMuteReasons;

	private static int _energyMax;

	private static int _energyIncrease;

	private static EquipmentSubtypeRef _fishingEquipmentSubtype;

	private static int _fishingDistanceMin;

	private static int _fishingDistanceMax;

	private static string _fishingShopDialog;

	private static string _fishingDailyDialog;

	private static string _eventSalesShopDialog;

	private static string _gameForumsURL;

	private static string _nbpDialog;

	private static bool _shopRotationVisible;

	private static long _shopPromoScrollDelay;

	private static MusicRef _pvpDuelBattleMusic;

	private static string _pvpDuelBattleBGURL;

	private static string _battleDeathAsset;

	private static long _adRefreshMilliseconds;

	private static List<string> _paymentsIDNBPZone;

	private static long _nbpMilliseconds;

	private static ItemRef _tutorialShopItem;

	private static int _sealsGain;

	private static int _sealsMax;

	private static long _sealsCooldown;

	private static long _serverIdleMilliseconds;

	private static long _serverIdleDisconnectMilliseconds;

	private static int _guildMemberLimit;

	private static int _ticketsGain;

	private static int _fusionAugmentIncrease;

	private static int _energyGain;

	private static int _shardsGain;

	private static int _tokensGain;

	private static int _badgesGain;

	private static float _ticketsCooldown;

	private static float _ticketsMax;

	private static float _shardsMax;

	private static float _tokensMax;

	private static float _badgesMax;

	private static long _guildMutinyOfficerMilliseconds;

	private static long _guildMutinyMemberMilliseconds;

	private static long _guildMutinyRecruitMilliseconds;

	private static float _familiarStableBonus;

	private static int _dungeonConsumableLimit;

	private static int _fusionFamiliarsRequired;

	private static float _pvpDuelTurnSeconds;

	private static string _chatTextColorError;

	private static string _chatTextColorAdmin;

	private static string _chatTextColorSelf;

	private static string _chatTextColorModerator;

	private static string _chatTextColorOthers;

	private static string _chatTextColorGlobalMessage;

	private static int _dailyQuestGain;

	private static int _guildMessageLimit;

	private static string _characterPointIncrease;

	private static bool _revUEnabled;

	private static List<TutorialRef> _tutorials;

	private static int _tutorialBootyId;

	private static int _tutorialAugmentsId;

	private static int _tutorialRunesId;

	private static List<ConsumableRef> _excludedBoostWindow;

	public static int friendsMax => _friendsMax;

	public static int battleMeterMax => _battleMeterMax;

	public static ItemRef fishingShopItem => _fishingShopItem;

	public static float battleSpeedMultiplier => _battleSpeedMultiplier;

	public static float battleMilliseconds => _battleMilliseconds;

	public static float shieldMult => _shieldMult;

	public static string tutorialDialogEnchants => _tutorialDialogEnchants;

	public static int enchantMax => _enchantMax;

	public static int mountMax => _mountMax;

	public static int characterNameLength => _characterNameLength;

	public static string[] shopQuantities => _shopQuantities;

	public static int dailyQuestLimit => _dailyQuestLimit;

	public static int worldChatMessageLimit => _worldChatMessageLimit;

	public static int worldChatInputLength => _worldChatInputLength;

	public static int guildChatMessageLimit => _guildChatMessageLimit;

	public static string tutorialDialogBrawl => _tutorialDialogBrawl;

	public static string tutorialDialogRaid => _tutorialDialogRaid;

	public static string tutorialDialogGvE => _tutorialDialogGvE;

	public static string tutorialDialogRifts => _tutorialDialogRifts;

	public static string tutorialDialogGauntlet => _tutorialDialogGauntlet;

	public static string tutorialDialogFishing => _tutorialDialogFishing;

	public static string tutorialDialogGvG => _tutorialDialogGvG;

	public static string tutorialDialogFusion => _tutorialDialogFusion;

	public static string tutorialDialogCraft => _tutorialDialogCraft;

	public static string tutorialDialogUpgrade => _tutorialDialogUpgrade;

	public static string tutorialDialogInvasion => _tutorialDialogInvasion;

	public static string tutorialDialogFamiliarStable => _tutorialDialogFamiliarStable;

	public static int guildNameLength => _guildNameLength;

	public static int guildInitialsLength => _guildInitialsLength;

	public static int guildChatInputLength => _guildChatInputLength;

	public static int familiarStableMaxQty => _familiarStableMaxQty;

	public static bool brawlVisible => _brawlVisible;

	public static bool raidVisible => _raidVisible;

	public static int augmentMax => _augmentMax;

	public static List<ChatChannel> worldChatChannels => _chatChannels;

	public static int gveEventDifficultyIntervals => _gveEventDifficultyIntervals;

	public static List<AbilityRef> abilitiesDefault => _abilitiesDefault;

	public static string tutorialDialogIntro => _tutorialDialogIntro;

	public static List<int> chatMuteSeconds => _chatMuteSeconds;

	public static string chatTextColorError => _chatTextColorError;

	public static string characterPointIncrease => _characterPointIncrease;

	public static int energyGain => _energyGain;

	public static int shardsGain => _shardsGain;

	public static int tokensGain => _tokensGain;

	public static int badgesGain => _badgesGain;

	public static float ticketsCooldown => _ticketsCooldown;

	public static float ticketsMax => _ticketsMax;

	public static float shardsMax => _shardsMax;

	public static float tokensMax => _tokensMax;

	public static float badgesMax => _badgesMax;

	public static long guildMutinyOfficerMilliseconds => _guildMutinyOfficerMilliseconds;

	public static long guildMutinyMemberMilliseconds => _guildMutinyMemberMilliseconds;

	public static long guildMutinyRecruitMilliseconds => _guildMutinyRecruitMilliseconds;

	public static float familiarStableBonus => _familiarStableBonus;

	public static int dungeonConsumableLimit => _dungeonConsumableLimit;

	public static int fusionFamiliarsRequired => _fusionFamiliarsRequired;

	public static float pvpDuelTurnSeconds => _pvpDuelTurnSeconds;

	public static string chatTextColorAdmin => _chatTextColorAdmin;

	public static string chatTextColorSelf => _chatTextColorSelf;

	public static string chatTextColorModerator => _chatTextColorModerator;

	public static string chatTextColorOthers => _chatTextColorOthers;

	public static string chatTextColorGlobalMessage => _chatTextColorGlobalMessage;

	public static int dailyQuestGain => _dailyQuestGain;

	public static int guildMessageLimit => _guildMessageLimit;

	public static int ticketsGain => _ticketsGain;

	public static int fusionAugmentIncrease => _fusionAugmentIncrease;

	public static int guildMemberLimit => _guildMemberLimit;

	public static int chatMuteSecondsModeratorLimit => _chatMuteSecondsModeratorLimit;

	public static List<string> chatMuteReasons => _chatMuteReasons;

	public static int energyMax => _energyMax;

	public static int energyIncrease => _energyIncrease;

	public static List<int> CharacterBaseForceConsumableIds => _characterBaseForceConsumableIds;

	public static EquipmentSubtypeRef fishingEquipmentSubtype => _fishingEquipmentSubtype;

	public static int fishingDistanceMin => _fishingDistanceMin;

	public static int fishingDistanceMax => _fishingDistanceMax;

	public static string fishingShopDialog => _fishingShopDialog;

	public static string fishingDailyDialog => _fishingDailyDialog;

	public static string eventSalesShopDialog => _eventSalesShopDialog;

	public static string gameForumsURL => _gameForumsURL;

	public static string nbpDialog => _nbpDialog;

	public static bool shopRotationVisible => _shopRotationVisible;

	public static long shopPromoScrollDelay => _shopPromoScrollDelay;

	public static MusicRef pvpDuelBattleMusic => _pvpDuelBattleMusic;

	public static string pvpDuelBattleBGURL => _pvpDuelBattleBGURL;

	public static CharacterBase characterBase => _characterBase;

	public static List<GameModifier> modifiersBase => _modifiersBase;

	public static AdgorRef adgorRef => _adgorRef;

	public static long adRefreshMilliseconds => _adRefreshMilliseconds;

	public static List<string> paymentsIDNBPZone => _paymentsIDNBPZone;

	public static long nbpMilliseconds => _nbpMilliseconds;

	public static ItemRef tutorialShopItem => _tutorialShopItem;

	public static int sealsGain => _sealsGain;

	public static int sealsMax => _sealsMax;

	public static long sealsCooldown => _sealsCooldown;

	public static long serverIdleMilliseconds => _serverIdleMilliseconds;

	public static long serverIdleDisconnectMilliseconds => _serverIdleDisconnectMilliseconds;

	public static List<TutorialRef> tutorials => _tutorials;

	public static int tutorialBootyId => _tutorialBootyId;

	public static int tutorialAugmentsId => _tutorialAugmentsId;

	public static int tutorialRunesId => _tutorialRunesId;

	public static List<ConsumableRef> excludedBoostWindow => _excludedBoostWindow;

	public static bool revUEnabled => _revUEnabled;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_abilitiesDefault = new List<AbilityRef>();
		_chatChannels = new List<ChatChannel>();
		_resolutions = new List<Vector2>();
		_gameRequirements = new List<GameRequirement>();
		_rewards = new List<ItemRewardRef>();
		_modifiersBase = new List<GameModifier>();
		_characterBaseForceConsumableIds = new List<int>();
		_abilitiesDefault = AbilityBook.LookupAbilities(XMLBook.instance.variableBook.variables.abilitiesDefault);
		foreach (VariableBookData.Channel item in XMLBook.instance.variableBook.variables.worldChatChannels.lstChannel)
		{
			_chatChannels.Add(new ChatChannel(item.id, item.name));
		}
		foreach (VariableBookData.Resolution item2 in XMLBook.instance.variableBook.variables.gameResolutions.lstResolution)
		{
			_resolutions.Add(new Vector2(item2.width, item2.height));
		}
		foreach (VariableBookData.Requirement item3 in XMLBook.instance.variableBook.variables.requirements.lstRequirement)
		{
			_gameRequirements.Add(new GameRequirement(item3));
		}
		VariableBookData.FishingShopItem fishingShopItem = XMLBook.instance.variableBook.variables.fishingShopItem;
		_fishingShopItem = ItemBook.Lookup(fishingShopItem.id, fishingShopItem.type);
		int num = 0;
		foreach (VariableBookData.Item item4 in XMLBook.instance.variableBook.variables.Rewards.lstItem)
		{
			_rewards.Add(new ItemRewardRef(num, item4));
			num++;
		}
		int power = ((XMLBook.instance.variableBook.variables.characterBase.power == null) ? 1 : int.Parse(XMLBook.instance.variableBook.variables.characterBase.power));
		int stamina = ((XMLBook.instance.variableBook.variables.characterBase.stamina == null) ? 1 : int.Parse(XMLBook.instance.variableBook.variables.characterBase.stamina));
		int agility = ((XMLBook.instance.variableBook.variables.characterBase.agility == null) ? 1 : int.Parse(XMLBook.instance.variableBook.variables.characterBase.agility));
		List<GameModifier> list = new List<GameModifier>();
		if (XMLBook.instance.variableBook.variables.characterBase.modifiers != null && XMLBook.instance.variableBook.variables.characterBase.modifiers.lstModifier != null && XMLBook.instance.variableBook.variables.characterBase.modifiers.lstModifier.Count > 0)
		{
			foreach (GameModifierData item5 in XMLBook.instance.variableBook.variables.characterBase.modifiers.lstModifier)
			{
				list.Add(new GameModifier(item5));
			}
		}
		string[] array = XMLBook.instance.variableBook.variables.characterBaseForceConsumableIds.Split(',');
		foreach (string s in array)
		{
			_characterBaseForceConsumableIds.Add(int.Parse(s));
		}
		_characterBase = new CharacterBase(power, stamina, agility, list);
		foreach (GameModifierData item6 in XMLBook.instance.variableBook.variables.modifiersBase.modifiers.lstModifier)
		{
			_modifiersBase.Add(new GameModifier(item6));
		}
		_adgorRef = new AdgorRef(XMLBook.instance.variableBook.variables.adgor.lstItem);
		_friendsMax = XMLBook.instance.variableBook.variables.friendsMax;
		_battleMeterMax = XMLBook.instance.variableBook.variables.battleMeterMax;
		_battleSpeedMultiplier = Util.GetFloatFromStringProperty(XMLBook.instance.variableBook.variables.battleSpeedMultiplier);
		_battleMilliseconds = XMLBook.instance.variableBook.variables.battleMilliseconds;
		_shieldMult = XMLBook.instance.variableBook.variables.shieldMult;
		_tutorialDialogEnchants = XMLBook.instance.variableBook.variables.tutorialDialogEnchants;
		_enchantMax = XMLBook.instance.variableBook.variables.enchantMax;
		_mountMax = XMLBook.instance.variableBook.variables.mountMax;
		_characterNameLength = XMLBook.instance.variableBook.variables.characterNameLength;
		_shopQuantities = XMLBook.instance.variableBook.variables.shopQuantities.Split(',');
		_dailyQuestLimit = XMLBook.instance.variableBook.variables.dailyQuestLimit;
		_worldChatMessageLimit = XMLBook.instance.variableBook.variables.worldChatMessageLimit;
		_worldChatInputLength = XMLBook.instance.variableBook.variables.worldChatInputLength;
		_guildChatMessageLimit = XMLBook.instance.variableBook.variables.guildChatMessageLimit;
		_tutorialDialogBrawl = XMLBook.instance.variableBook.variables.tutorialDialogBrawl;
		_tutorialDialogRaid = XMLBook.instance.variableBook.variables.tutorialDialogRaid;
		_tutorialDialogGvE = XMLBook.instance.variableBook.variables.tutorialDialogGvE;
		_tutorialDialogRifts = XMLBook.instance.variableBook.variables.tutorialDialogRifts;
		_tutorialDialogGauntlet = XMLBook.instance.variableBook.variables.tutorialDialogGauntlet;
		_tutorialDialogFishing = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogFishing);
		_dailyQuestLimit = XMLBook.instance.variableBook.variables.dailyQuestLimit;
		_worldChatMessageLimit = XMLBook.instance.variableBook.variables.worldChatMessageLimit;
		_worldChatInputLength = XMLBook.instance.variableBook.variables.worldChatInputLength;
		_guildChatMessageLimit = XMLBook.instance.variableBook.variables.guildChatMessageLimit;
		_tutorialDialogBrawl = XMLBook.instance.variableBook.variables.tutorialDialogBrawl;
		_tutorialDialogRaid = XMLBook.instance.variableBook.variables.tutorialDialogRaid;
		_tutorialDialogGvE = XMLBook.instance.variableBook.variables.tutorialDialogGvE;
		_tutorialDialogRifts = XMLBook.instance.variableBook.variables.tutorialDialogRifts;
		_tutorialDialogGauntlet = XMLBook.instance.variableBook.variables.tutorialDialogGauntlet;
		_tutorialDialogFishing = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogFishing);
		_tutorialDialogGvG = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogGvG);
		_tutorialDialogFusion = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogFusion);
		_tutorialDialogCraft = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogCraft);
		_tutorialDialogUpgrade = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogUpgrade);
		_tutorialDialogInvasion = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogInvasion);
		_tutorialDialogFamiliarStable = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogFamiliarStable);
		_guildNameLength = XMLBook.instance.variableBook.variables.guildNameLength;
		_guildInitialsLength = XMLBook.instance.variableBook.variables.guildInitialsLength;
		_guildChatInputLength = XMLBook.instance.variableBook.variables.guildChatInputLength;
		_familiarStableMaxQty = XMLBook.instance.variableBook.variables.familiarStableMaxQty;
		_brawlVisible = Util.GetBoolFromStringProperty(XMLBook.instance.variableBook.variables.brawlVisible);
		_raidVisible = Util.GetBoolFromStringProperty(XMLBook.instance.variableBook.variables.raidVisible);
		_augmentMax = XMLBook.instance.variableBook.variables.augmentMax;
		_tutorialDialogGvG = Util.GetStringFromStringProperty(XMLBook.instance.variableBook.variables.tutorialDialogGvG);
		_guildNameLength = XMLBook.instance.variableBook.variables.guildNameLength;
		_guildInitialsLength = XMLBook.instance.variableBook.variables.guildInitialsLength;
		_guildChatInputLength = XMLBook.instance.variableBook.variables.guildChatInputLength;
		_familiarStableMaxQty = XMLBook.instance.variableBook.variables.familiarStableMaxQty;
		_brawlVisible = Util.GetBoolFromStringProperty(XMLBook.instance.variableBook.variables.brawlVisible);
		_raidVisible = Util.GetBoolFromStringProperty(XMLBook.instance.variableBook.variables.raidVisible);
		_augmentMax = XMLBook.instance.variableBook.variables.augmentMax;
		_gveEventDifficultyIntervals = XMLBook.instance.variableBook.variables.gveEventDifficultyIntervals;
		_tutorialDialogIntro = XMLBook.instance.variableBook.variables.tutorialDialogIntro;
		_chatMuteSeconds = Util.GetIntListFromStringProperty(XMLBook.instance.variableBook.variables.chatMuteSeconds);
		_chatMuteSecondsModeratorLimit = XMLBook.instance.variableBook.variables.chatMuteSecondsModeratorLimit;
		_chatMuteReasons = Util.GetStringListFromStringProperty(XMLBook.instance.variableBook.variables.chatMuteReasons, ',', language: true);
		_energyMax = XMLBook.instance.variableBook.variables.energyMax;
		_energyIncrease = XMLBook.instance.variableBook.variables.energyIncrease;
		_fishingEquipmentSubtype = EquipmentBook.LookupSubtypeLink(XMLBook.instance.variableBook.variables.fishingEquipmentSubtype);
		_fishingDistanceMin = XMLBook.instance.variableBook.variables.fishingDistanceMin;
		_fishingDistanceMax = XMLBook.instance.variableBook.variables.fishingDistanceMax;
		_fishingShopDialog = XMLBook.instance.variableBook.variables.fishingShopDialog;
		_eventSalesShopDialog = XMLBook.instance.variableBook.variables.eventSalesShopDialog;
		_fishingDailyDialog = XMLBook.instance.variableBook.variables.fishingDailyDialog;
		_gameForumsURL = XMLBook.instance.variableBook.variables.gameForumsURL;
		_nbpDialog = XMLBook.instance.variableBook.variables.nbpDialog;
		_shopRotationVisible = Util.parseBoolean(XMLBook.instance.variableBook.variables.shopRotationVisible);
		_shopPromoScrollDelay = XMLBook.instance.variableBook.variables.shopPromoScrollDelay;
		_pvpDuelBattleMusic = MusicBook.Lookup((XMLBook.instance.variableBook.variables.pvpDuelBattleMusic != null) ? XMLBook.instance.variableBook.variables.pvpDuelBattleMusic : "battle");
		_pvpDuelBattleBGURL = ((XMLBook.instance.variableBook.variables.pvpDuelBattleBG != null) ? XMLBook.instance.variableBook.variables.pvpDuelBattleBG : null);
		_battleDeathAsset = XMLBook.instance.variableBook.variables.battleDeathAsset;
		_adRefreshMilliseconds = XMLBook.instance.variableBook.variables.adRefreshMilliseconds;
		_revUEnabled = XMLBook.instance.variableBook.variables.revUEnabled;
		List<string> list2 = new List<string>();
		if (XMLBook.instance.variableBook.variables.boosters != null)
		{
			foreach (VariableBookData.Payment item7 in XMLBook.instance.variableBook.variables.boosters.lstPayment)
			{
				list2.Add(item7.id);
			}
		}
		_paymentsIDNBPZone = list2;
		_nbpMilliseconds = XMLBook.instance.variableBook.variables.nbpMilliseconds;
		_tutorialShopItem = ItemBook.Lookup(XMLBook.instance.variableBook.variables.tutorialShopItem.id, ItemRef.getItemType(XMLBook.instance.variableBook.variables.tutorialShopItem.type));
		_sealsGain = XMLBook.instance.variableBook.variables.sealsGain;
		_sealsMax = XMLBook.instance.variableBook.variables.sealsMax;
		_sealsCooldown = XMLBook.instance.variableBook.variables.sealsCooldown;
		_serverIdleMilliseconds = XMLBook.instance.variableBook.variables.serverIdleMilliseconds;
		_serverIdleDisconnectMilliseconds = XMLBook.instance.variableBook.variables.serverIdleDisconnectMilliseconds;
		_guildMemberLimit = XMLBook.instance.variableBook.variables.guildMemberLimit;
		_fusionAugmentIncrease = XMLBook.instance.variableBook.variables.fusionAugmentIncrease;
		_familiarStableMaxQty = XMLBook.instance.variableBook.variables.familiarStableMaxQty;
		_energyGain = XMLBook.instance.variableBook.variables.energyGain;
		_ticketsGain = XMLBook.instance.variableBook.variables.ticketsGain;
		_shardsGain = XMLBook.instance.variableBook.variables.shardsGain;
		_tokensGain = XMLBook.instance.variableBook.variables.tokensGain;
		_badgesGain = XMLBook.instance.variableBook.variables.badgesGain;
		_ticketsCooldown = XMLBook.instance.variableBook.variables.ticketsCooldown;
		_guildMemberLimit = XMLBook.instance.variableBook.variables.guildMemberLimit;
		_ticketsMax = XMLBook.instance.variableBook.variables.ticketsMax;
		_shardsMax = XMLBook.instance.variableBook.variables.shardsMax;
		_tokensMax = XMLBook.instance.variableBook.variables.tokensMax;
		_badgesMax = XMLBook.instance.variableBook.variables.badgesMax;
		_nbpMilliseconds = XMLBook.instance.variableBook.variables.nbpMilliseconds;
		_guildChatMessageLimit = XMLBook.instance.variableBook.variables.guildChatMessageLimit;
		_guildMutinyOfficerMilliseconds = XMLBook.instance.variableBook.variables.guildMutinyOfficerMilliseconds;
		_guildMutinyMemberMilliseconds = XMLBook.instance.variableBook.variables.guildMutinyMemberMilliseconds;
		_guildMutinyRecruitMilliseconds = XMLBook.instance.variableBook.variables.guildMutinyRecruitMilliseconds;
		_energyIncrease = XMLBook.instance.variableBook.variables.energyIncrease;
		_familiarStableBonus = XMLBook.instance.variableBook.variables.familiarStableBonus;
		_dungeonConsumableLimit = XMLBook.instance.variableBook.variables.dungeonConsumableLimit;
		_fusionFamiliarsRequired = XMLBook.instance.variableBook.variables.fusionFamiliarsRequired;
		_familiarStableMaxQty = XMLBook.instance.variableBook.variables.familiarStableMaxQty;
		_enchantMax = XMLBook.instance.variableBook.variables.enchantMax;
		_mountMax = XMLBook.instance.variableBook.variables.mountMax;
		_augmentMax = XMLBook.instance.variableBook.variables.augmentMax;
		_characterNameLength = XMLBook.instance.variableBook.variables.characterNameLength;
		_guildNameLength = XMLBook.instance.variableBook.variables.guildNameLength;
		_guildInitialsLength = XMLBook.instance.variableBook.variables.guildInitialsLength;
		_pvpDuelTurnSeconds = XMLBook.instance.variableBook.variables.pvpDuelTurnSeconds;
		_worldChatMessageLimit = XMLBook.instance.variableBook.variables.worldChatMessageLimit;
		_worldChatInputLength = XMLBook.instance.variableBook.variables.worldChatInputLength;
		_chatTextColorError = XMLBook.instance.variableBook.variables.chatTextColorError;
		_chatTextColorAdmin = XMLBook.instance.variableBook.variables.chatTextColorAdmin;
		_chatTextColorSelf = XMLBook.instance.variableBook.variables.chatTextColorSelf;
		_chatTextColorModerator = XMLBook.instance.variableBook.variables.chatTextColorModerator;
		_chatTextColorOthers = XMLBook.instance.variableBook.variables.chatTextColorOthers;
		_chatTextColorGlobalMessage = XMLBook.instance.variableBook.variables.chatTextColorGlobalMessage;
		_dailyQuestGain = XMLBook.instance.variableBook.variables.dailyQuestGain;
		_guildMessageLimit = XMLBook.instance.variableBook.variables.guildMessageLimit;
		_characterPointIncrease = XMLBook.instance.variableBook.variables.characterPointIncrease;
		_tutorials = new List<TutorialRef>();
		foreach (VariableBookData.Tutorial item8 in XMLBook.instance.variableBook.variables.tutorials.lstTutorial)
		{
			_tutorials.Add(new TutorialRef(item8));
		}
		_tutorialBootyId = ((XMLBook.instance.variableBook.variables.tutorialBootyId != null) ? int.Parse(XMLBook.instance.variableBook.variables.tutorialBootyId) : (-1));
		_tutorialAugmentsId = ((XMLBook.instance.variableBook.variables.tutorialAugmentsId != null) ? int.Parse(XMLBook.instance.variableBook.variables.tutorialAugmentsId) : (-1));
		_tutorialRunesId = ((XMLBook.instance.variableBook.variables.tutorialRunesId != null) ? int.Parse(XMLBook.instance.variableBook.variables.tutorialRunesId) : (-1));
		if (XMLBook.instance.variableBook.variables.excludedBoostWindow != null && XMLBook.instance.variableBook.variables.excludedBoostWindow.lstConsumable != null)
		{
			_excludedBoostWindow = new List<ConsumableRef>();
			foreach (BaseBookItem item9 in XMLBook.instance.variableBook.variables.excludedBoostWindow.lstConsumable)
			{
				_excludedBoostWindow.Add(ConsumableBook.Lookup(item9.id));
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GameRequirement GetGameRequirement(int type)
	{
		foreach (GameRequirement gameRequirement in _gameRequirements)
		{
			if (gameRequirement.type == type)
			{
				return gameRequirement;
			}
		}
		return null;
	}

	public static bool GameRequirementMet(int type, bool def = true)
	{
		return GetGameRequirement(type)?.RequirementsMet() ?? def;
	}

	public static List<Vector2> GetGameResolutions()
	{
		return _resolutions;
	}

	public static int GetChatMuteSecondsIndex(int seconds)
	{
		for (int i = 0; i < chatMuteSeconds.Count; i++)
		{
			if (chatMuteSeconds[i] == seconds)
			{
				return i;
			}
		}
		return 0;
	}

	public static string GetChatMuteReason(int index)
	{
		if (index < 0 || index >= chatMuteReasons.Count)
		{
			return "";
		}
		return chatMuteReasons[index];
	}

	public static ItemRewardRef GetItemReward(string link)
	{
		foreach (ItemRewardRef reward in _rewards)
		{
			if (reward.link.ToLowerInvariant() == link.ToLowerInvariant())
			{
				return reward;
			}
		}
		return null;
	}

	public static List<ItemRewardRef> GetItemRewards(string theString)
	{
		List<ItemRewardRef> list = new List<ItemRewardRef>();
		if (theString == null || theString.Length <= 0)
		{
			return list;
		}
		List<string> list2 = new List<string>(theString.Split(','));
		foreach (ItemRewardRef reward in _rewards)
		{
			if (list2.Contains(reward.link))
			{
				list.Add(reward);
			}
		}
		return list;
	}

	public static List<ItemRewardRef> GetItemRewardsByType(int type)
	{
		List<ItemRewardRef> list = new List<ItemRewardRef>();
		foreach (ItemRewardRef reward in _rewards)
		{
			if (reward.type == type)
			{
				list.Add(reward);
			}
		}
		return list;
	}

	public static string GetBattleDeathAsset()
	{
		return _battleDeathAsset;
	}

	public static TutorialRef LookUpTutorial(string type)
	{
		foreach (TutorialRef tutorial in _tutorials)
		{
			if (tutorial.type.ToLower().Equals(type.ToLower()))
			{
				return tutorial;
			}
		}
		return null;
	}
}
