using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class VariableBookData : BaseBook
{
	public class Resolution : BaseBookItem
	{
		[XmlAttribute("width")]
		public int width { get; set; }

		[XmlAttribute("height")]
		public int height { get; set; }
	}

	public class GameResolutions : BaseBookItem
	{
		[XmlElement("resolution")]
		public List<Resolution> lstResolution { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }

		[XmlAttribute("reductionID")]
		public string reductionID { get; set; }

		[XmlAttribute("reductionType")]
		public string reductionType { get; set; }

		[XmlAttribute("minQty")]
		public string minQty { get; set; }

		[XmlAttribute("maxQty")]
		public string maxQty { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("dialog")]
		public string dialog { get; set; }

		[XmlAttribute("startDate")]
		public string startDate { get; set; }

		[XmlAttribute("endDate")]
		public string endDate { get; set; }

		[XmlAttribute("rewardType")]
		public string rewardType { get; set; }
	}

	public class Purchase : BaseBookItem
	{
		[XmlElement("item")]
		public Item item { get; set; }
	}

	public class Reduction : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Compensate : BaseBookItem
	{
		[XmlElement("purchase")]
		public List<Purchase> lstPurchase { get; set; }

		[XmlElement("reduction")]
		public Reduction reduction { get; set; }
	}

	public class AdItems : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class FishingShopItem : BaseBookItem
	{
	}

	public class PvpDuelTeamRules : BaseBookItem
	{
		[XmlAttribute("slots")]
		public int slots { get; set; }

		[XmlAttribute("allowFriends")]
		public string allowFriends { get; set; }

		[XmlAttribute("allowGuildmates")]
		public string allowGuildmates { get; set; }
	}

	public class PvpDuelBattleRules : BaseBookItem
	{
		[XmlAttribute("allowSwitch")]
		public string allowSwitch { get; set; }
	}

	public class Channel : BaseBookItem
	{
	}

	public class WorldChatChannels : BaseBookItem
	{
		[XmlElement("channel")]
		public List<Channel> lstChannel { get; set; }
	}

	public class Modifiers : BaseBookItem
	{
		[XmlElement(ElementName = "modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	public class ModifiersBase
	{
		[XmlElement("modifiers")]
		public Modifiers modifiers { get; set; }
	}

	[XmlRoot(ElementName = "equipment")]
	public class Equipment
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	public class CharacterBase : BaseBookItem
	{
		[XmlElement("equipment")]
		public List<Equipment> lstEquipment { get; set; }

		[XmlElement("item")]
		public Item item { get; set; }

		[XmlAttribute("power")]
		public string power { get; set; }

		[XmlAttribute(AttributeName = "stamina")]
		public string stamina { get; set; }

		[XmlAttribute(AttributeName = "agility")]
		public string agility { get; set; }

		[XmlElement("modifiers")]
		public Modifiers modifiers { get; set; }
	}

	public class Requirement : BaseBookItem
	{
		[XmlAttribute("levelReq")]
		public int levelReq { get; set; }

		[XmlAttribute("zoneCompleteReq")]
		public int zoneCompleteReq { get; set; }

		[XmlAttribute("zoneCompleteNodeReq")]
		public int zoneCompleteNodeReq { get; set; }

		[XmlAttribute("zoneUnlockReq")]
		public int zoneUnlockReq { get; set; }

		[XmlAttribute("zoneUnlockNodeReq")]
		public int zoneUnlockNodeReq { get; set; }

		[XmlAttribute("itemReqType")]
		public string itemReqType { get; set; }

		[XmlAttribute("itemReqQty")]
		public int itemReqQty { get; set; }

		[XmlAttribute("dialogLocked")]
		public string dialogLocked { get; set; }

		[XmlAttribute("hide")]
		public bool hide { get; set; }

		[XmlAttribute("message")]
		public string message { get; set; }
	}

	public class Requirements : BaseBookItem
	{
		[XmlElement(ElementName = "requirement")]
		public List<Requirement> lstRequirement { get; set; }
	}

	public class TutorialShopItem : BaseBookItem
	{
	}

	public class ModeratorItems : BaseBookItem
	{
		[XmlElement("item")]
		public Item item { get; set; }
	}

	public class LoginItems : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Rewards : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Adgor : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Variables
	{
		[XmlElement("gameURL")]
		public string gameURL { get; set; }

		[XmlElement("gameForumsURL")]
		public string gameForumsURL { get; set; }

		[XmlElement("gameResolutions")]
		public GameResolutions gameResolutions { get; set; }

		[XmlElement("compensateIDs")]
		public string compensateIDs { get; set; }

		[XmlElement("characterBaseForceConsumableIds")]
		public string characterBaseForceConsumableIds { get; set; }

		[XmlElement("compensateType")]
		public string compensateType { get; set; }

		[XmlElement("compensate")]
		public Compensate compensate { get; set; }

		[XmlElement("friendsMax")]
		public int friendsMax { get; set; }

		[XmlElement("requestsMax")]
		public int requestsMax { get; set; }

		[XmlElement("recommendMax")]
		public int recommendMax { get; set; }

		[XmlElement("healthMult")]
		public float healthMult { get; set; }

		[XmlElement("shieldMult")]
		public float shieldMult { get; set; }

		[XmlElement("adMillisecondsMin")]
		public long adMillisecondsMin { get; set; }

		[XmlElement("adMillisecondsMax")]
		public long adMillisecondsMax { get; set; }

		[XmlElement("adItems")]
		public AdItems adItems { get; set; }

		[XmlElement("adRefreshMilliseconds")]
		public long adRefreshMilliseconds { get; set; }

		[XmlElement("battleMilliseconds")]
		public long battleMilliseconds { get; set; }

		[XmlElement("battleMeterMax")]
		public int battleMeterMax { get; set; }

		[XmlElement("battleDamageGainMax")]
		public float BattleDamageGainMax { get; set; }

		[XmlElement("battleSpeedMultiplier")]
		public string battleSpeedMultiplier { get; set; }

		[XmlElement("battleDefaultBounds")]
		public string battleDefaultBounds { get; set; }

		[XmlElement("battleDeathAsset")]
		public string battleDeathAsset { get; set; }

		[XmlElement("energyMax")]
		public int energyMax { get; set; }

		[XmlElement("energyGain")]
		public int energyGain { get; set; }

		[XmlElement("energyIncrease")]
		public int energyIncrease { get; set; }

		[XmlElement("energyCooldown")]
		public long energyCooldown { get; set; }

		[XmlElement("ticketsMax")]
		public int ticketsMax { get; set; }

		[XmlElement("ticketsGain")]
		public int ticketsGain { get; set; }

		[XmlElement("ticketsCooldown")]
		public long ticketsCooldown { get; set; }

		[XmlElement("shardsMax")]
		public int shardsMax { get; set; }

		[XmlElement("shardsGain")]
		public int shardsGain { get; set; }

		[XmlElement("shardsCooldown")]
		public long shardsCooldown { get; set; }

		[XmlElement("sealsMax")]
		public int sealsMax { get; set; }

		[XmlElement("sealsGain")]
		public int sealsGain { get; set; }

		[XmlElement("sealsCooldown")]
		public long sealsCooldown { get; set; }

		[XmlElement("tokensMax")]
		public int tokensMax { get; set; }

		[XmlElement("tokensGain")]
		public int tokensGain { get; set; }

		[XmlElement("tokensCooldown")]
		public long tokensCooldown { get; set; }

		[XmlElement("badgesMax")]
		public int badgesMax { get; set; }

		[XmlElement("badgesGain")]
		public int badgesGain { get; set; }

		[XmlElement("badgesCooldown")]
		public long badgesCooldown { get; set; }

		[XmlElement("newsScrollDelay")]
		public long newsScrollDelay { get; set; }

		[XmlElement("newsScrollDuration")]
		public float newsScrollDuration { get; set; }

		[XmlElement("shopPromoScrollDelay")]
		public long shopPromoScrollDelay { get; set; }

		[XmlElement("shopPromoScrollDuration")]
		public float shopPromoScrollDuration { get; set; }

		[XmlElement("shopRotationCooldown")]
		public long shopRotationCooldown { get; set; }

		[XmlElement("shopRotationVisible")]
		public string shopRotationVisible { get; set; }

		[XmlElement("shopQuantities")]
		public string shopQuantities { get; set; }

		[XmlElement("fishingEquipmentSubtype")]
		public string fishingEquipmentSubtype { get; set; }

		[XmlElement("fishingDistanceMin")]
		public int fishingDistanceMin { get; set; }

		[XmlElement("fishingDistanceMax")]
		public int fishingDistanceMax { get; set; }

		[XmlElement("fishingShopItem")]
		public FishingShopItem fishingShopItem { get; set; }

		[XmlElement("fishingDailyDialog")]
		public string fishingDailyDialog { get; set; }

		[XmlElement("fishingShopDialog")]
		public string fishingShopDialog { get; set; }

		[XmlElement("eventSalesShopDialog")]
		public string eventSalesShopDialog { get; set; }

		[XmlElement("raidVisible")]
		public string raidVisible { get; set; }

		[XmlElement("brawlVisible")]
		public string brawlVisible { get; set; }

		[XmlElement("dungeonCharacterAdLimit")]
		public int dungeonCharacterAdLimit { get; set; }

		[XmlElement("dungeonConsumableLimit")]
		public int dungeonConsumableLimit { get; set; }

		[XmlElement("dungeonReconnectMilliseconds")]
		public long dungeonReconnectMilliseconds { get; set; }

		[XmlElement("pvpEventPointsBase")]
		public int pvpEventPointsBase { get; set; }

		[XmlElement("pvpEventPointsMin")]
		public int pvpEventPointsMin { get; set; }

		[XmlElement("pvpEventPointsMax")]
		public int PvpEventPointsMax { get; set; }

		[XmlElement("pvpEventPointsStatMult")]
		public float pvpEventPointsStatMult { get; set; }

		[XmlElement("pvpEventPointsLoseMult")]
		public float pvpEventPointsLoseMult { get; set; }

		[XmlElement("pvpEventPointsTargetMult")]
		public float pvpEventPointsTargetMult { get; set; }

		[XmlElement("pvpEventPointsTargetStatLimit")]
		public int pvpEventPointsTargetStatLimit { get; set; }

		[XmlElement("pvpEventPointsTargetStatLimitMult")]
		public float pvpEventPointsTargetStatLimitMult { get; set; }

		[XmlElement("pvpEventTargets")]
		public int pvpEventTargets { get; set; }

		[XmlElement("pvpEventHistoryLimit")]
		public int pvpEventHistoryLimit { get; set; }

		[XmlElement("pvpEventHistoryReplay")]
		public string pvpEventHistoryReplay { get; set; }

		[XmlElement("pvpDuelBattleBG")]
		public string pvpDuelBattleBG { get; set; }

		[XmlElement("pvpDuelBattleMusic")]
		public string pvpDuelBattleMusic { get; set; }

		[XmlElement("pvpDuelTurnSeconds")]
		public float pvpDuelTurnSeconds { get; set; }

		[XmlElement("pvpDuelTeamRules")]
		public PvpDuelTeamRules pvpDuelTeamRules { get; set; }

		[XmlElement("pvpDuelBattleRules")]
		public PvpDuelBattleRules pvpDuelBattleRules { get; set; }

		[XmlElement("gvgEventPointsBase")]
		public int gvgEventPointsBase { get; set; }

		[XmlElement("gvgEventPointsMin")]
		public int gvgEventPointsMin { get; set; }

		[XmlElement("gvgEventPointsMax")]
		public int gvgEventPointsMax { get; set; }

		[XmlElement("gvgEventPointsStatMult")]
		public float gvgEventPointsStatMult { get; set; }

		[XmlElement("gvgEventPointsTargetStatLimit")]
		public int gvgEventPointsTargetStatLimit { get; set; }

		[XmlElement("gvgEventPointsTargetStatLimitMult")]
		public float gvgEventPointsTargetStatLimitMult { get; set; }

		[XmlElement("gvgEventTargets")]
		public int gvgEventTargets { get; set; }

		[XmlElement("gvgEventSearchLimit")]
		public int gvgEventSearchLimit { get; set; }

		[XmlElement("gvgEventLootRolls")]
		public int gvgEventLootRolls { get; set; }

		[XmlElement("gveEventDifficultyIntervals")]
		public int gveEventDifficultyIntervals { get; set; }

		[XmlElement("invasionEventWaveOffset")]
		public int invasionEventWaveOffset { get; set; }

		[XmlElement("invasionEventLootEntities")]
		public int invasionEventLootEntities { get; set; }

		[XmlElement("guildNameLength")]
		public int guildNameLength { get; set; }

		[XmlElement("guildInitialsLength")]
		public int guildInitialsLength { get; set; }

		[XmlElement(ElementName = "guildMemberLimit")]
		public int guildMemberLimit { get; set; }

		[XmlElement(ElementName = "guildApplicationListLimit")]
		public int guildApplicationListLimit { get; set; }

		[XmlElement("guildApplicantListLimit")]
		public int guildApplicantListLimit { get; set; }

		[XmlElement("guildChatMessageLimit")]
		public int guildChatMessageLimit { get; set; }

		[XmlElement("guildChatInputLength")]
		public int guildChatInputLength { get; set; }

		[XmlElement("guildMessageLimit")]
		public int guildMessageLimit { get; set; }

		[XmlElement("guildPointsDefault")]
		public int guildPointsDefault { get; set; }

		[XmlElement("guildHonorMult")]
		public float guildHonorMult { get; set; }

		[XmlElement("guildMutinyOfficerMilliseconds")]
		public long guildMutinyOfficerMilliseconds { get; set; }

		[XmlElement("guildMutinyMemberMilliseconds")]
		public long guildMutinyMemberMilliseconds { get; set; }

		[XmlElement("guildMutinyRecruitMilliseconds")]
		public long guildMutinyRecruitMilliseconds { get; set; }

		[XmlElement("dailyQuestGain")]
		public int dailyQuestGain { get; set; }

		[XmlElement("dailyQuestLimit")]
		public int dailyQuestLimit { get; set; }

		[XmlElement("dailyQuestHistoryCount")]
		public int dailyQuestHistoryCount { get; set; }

		[XmlElement("dailyFishingLoot")]
		public string dailyFishingLoot { get; set; }

		[XmlElement("dailyFishingStartLoot")]
		public string dailyFishingStartLoot { get; set; }

		[XmlElement("worldChatMessageLimit")]
		public int worldChatMessageLimit { get; set; }

		[XmlElement("worldChatInputLength")]
		public int worldChatInputLength { get; set; }

		[XmlElement("worldChatChannels")]
		public WorldChatChannels worldChatChannels { get; set; }

		[XmlElement("characterNameLength")]
		public int characterNameLength { get; set; }

		[XmlElement("characterPointIncrease")]
		public string characterPointIncrease { get; set; }

		[XmlElement("fusionFamiliarsRequired")]
		public int fusionFamiliarsRequired { get; set; }

		[XmlElement("fusionAugmentIncrease")]
		public int fusionAugmentIncrease { get; set; }

		[XmlElement("familiarStableMaxQty")]
		public int familiarStableMaxQty { get; set; }

		[XmlElement("familiarStableBonus")]
		public float familiarStableBonus { get; set; }

		[XmlElement("enchantMax")]
		public int enchantMax { get; set; }

		[XmlElement("mountMax")]
		public int mountMax { get; set; }

		[XmlElement("augmentMax")]
		public int augmentMax { get; set; }

		[XmlElement("chatEnabled")]
		public bool chatEnabled { get; set; }

		[XmlElement("chatMessageDelay")]
		public int chatMessageDelay { get; set; }

		[XmlElement("chatMuteSeconds")]
		public string chatMuteSeconds { get; set; }

		[XmlElement("chatMuteReasons")]
		public string chatMuteReasons { get; set; }

		[XmlElement("chatMuteSecondsModeratorLimit")]
		public int chatMuteSecondsModeratorLimit { get; set; }

		[XmlElement("chatTextColorError")]
		public string chatTextColorError { get; set; }

		[XmlElement("chatTextColorSelf")]
		public string chatTextColorSelf { get; set; }

		[XmlElement("chatTextColorOthers")]
		public string chatTextColorOthers { get; set; }

		[XmlElement("chatTextColorAdmin")]
		public string chatTextColorAdmin { get; set; }

		[XmlElement("chatTextColorModerator")]
		public string chatTextColorModerator { get; set; }

		[XmlElement("chatTextColorGlobalMessage")]
		public string chatTextColorGlobalMessage { get; set; }

		[XmlElement("abilitiesDefault")]
		public string abilitiesDefault { get; set; }

		[XmlElement("serverIdleMilliseconds")]
		public long serverIdleMilliseconds { get; set; }

		[XmlElement("serverIdleDisconnectMilliseconds")]
		public long serverIdleDisconnectMilliseconds { get; set; }

		[XmlElement("serverIdleMillisecondsUnity")]
		public long serverIdleMillisecondsUnity { get; set; }

		[XmlElement("serverIdleDisconnectMillisecondsUnity")]
		public long serverIdleDisconnectMillisecondsUnity { get; set; }

		[XmlElement("modifiersBase")]
		public ModifiersBase modifiersBase { get; set; }

		[XmlElement("characterBase")]
		public CharacterBase characterBase { get; set; }

		[XmlElement("requirements")]
		public Requirements requirements { get; set; }

		[XmlElement("tutorialShopItem")]
		public TutorialShopItem tutorialShopItem { get; set; }

		[XmlElement("tutorialDialogIntro")]
		public string tutorialDialogIntro { get; set; }

		[XmlElement("tutorialDialogBrawl")]
		public string tutorialDialogBrawl { get; set; }

		[XmlElement("tutorialDialogRaid")]
		public string tutorialDialogRaid { get; set; }

		[XmlElement("tutorialDialogFusion")]
		public string tutorialDialogFusion { get; set; }

		[XmlElement("tutorialDialogRifts")]
		public string tutorialDialogRifts { get; set; }

		[XmlElement("tutorialDialogGauntlet")]
		public string tutorialDialogGauntlet { get; set; }

		[XmlElement("tutorialDialogFamiliarStable")]
		public string tutorialDialogFamiliarStable { get; set; }

		[XmlElement("tutorialDialogEnchants")]
		public string tutorialDialogEnchants { get; set; }

		[XmlElement("tutorialDialogGvE")]
		public string tutorialDialogGvE { get; set; }

		[XmlElement("tutorialDialogGvG")]
		public string tutorialDialogGvG { get; set; }

		[XmlElement("tutorialDialogInvasion")]
		public string tutorialDialogInvasion { get; set; }

		[XmlElement("tutorialDialogFishing")]
		public string tutorialDialogFishing { get; set; }

		[XmlElement("tutorialDialogCraft")]
		public string tutorialDialogCraft { get; set; }

		[XmlElement("tutorialDialogUpgrade")]
		public string tutorialDialogUpgrade { get; set; }

		[XmlElement("nbpDialog")]
		public string nbpDialog { get; set; }

		[XmlElement("nbpMilliseconds")]
		public long nbpMilliseconds { get; set; }

		[XmlElement("platformLinkLimit")]
		public int PlatformLinkLimit { get; set; }

		[XmlElement("moderatorItems")]
		public ModeratorItems ModeratorItems { get; set; }

		[XmlElement("loginItems")]
		public LoginItems loginItems { get; set; }

		[XmlElement("rewards")]
		public Rewards Rewards { get; set; }

		[XmlElement("adgor")]
		public Adgor adgor { get; set; }

		[XmlElement("boosters")]
		public Boosters boosters { get; set; }

		[XmlElement("tutorials")]
		public Tutorials tutorials { get; set; }

		[XmlElement("tutorialBootyId")]
		public string tutorialBootyId { get; set; }

		[XmlElement("tutorialAugmentsId")]
		public string tutorialAugmentsId { get; set; }

		[XmlElement("tutorialRunesId")]
		public string tutorialRunesId { get; set; }

		[XmlElement("excludedBoostWindow")]
		public ExcludedBoostWindow excludedBoostWindow { get; set; }

		[XmlElement("revUEnabled")]
		public bool revUEnabled { get; set; }
	}

	public class ExcludedBoostWindow : BaseBookItem
	{
		[XmlElement("consumable")]
		public List<BaseBookItem> lstConsumable { get; set; }
	}

	public class Boosters : BaseBookItem
	{
		[XmlElement("payment")]
		public List<Payment> lstPayment { get; set; }
	}

	public class Payment
	{
		[XmlAttribute("id")]
		public string id { get; set; }
	}

	public class Tutorials : BaseBookItem
	{
		[XmlElement("tutorial")]
		public List<Tutorial> lstTutorial { get; set; }
	}

	public class Tutorial
	{
		[XmlAttribute("type")]
		public string type { get; set; }

		[XmlAttribute("minZone")]
		public string minZone { get; set; }

		[XmlAttribute("minNode")]
		public string minNode { get; set; }

		[XmlAttribute("maxZone")]
		public string maxZone { get; set; }

		[XmlAttribute("maxNode")]
		public string maxNode { get; set; }

		[XmlAttribute("requiredItemType")]
		public string requiredItemType { get; set; }

		[XmlAttribute("requiredItemSubtype")]
		public string requiredItemSubtype { get; set; }

		[XmlAttribute("requiredItemQty")]
		public string requiredItemQty { get; set; }
	}

	public Variables variables { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
