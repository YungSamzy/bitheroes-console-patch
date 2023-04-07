using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.zone;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.xml;

public class XMLBook
{
	public const string ASIAN_LANGUAGE_BOOK = "AsianLanguagesBook.xml";

	public const string LANGUAGE_BOOK = "LanguageBook.xml";

	public const string MUSIC_BOOK = "MusicBook.xml";

	public const string SOUND_BOOK = "SoundBook.xml";

	public const string BATTLE_BOOK = "BattleBook.xml";

	public const string FILTER_BOOK = "FilterBook.xml";

	public const string RARITY_BOOK = "RarityBook.xml";

	public const string PROBABILITY_BOOK = "ProbabilityBook.xml";

	public const string ABILITY_BOOK = "AbilityBook.xml";

	public const string EQUIPMENT_BOOK = "EquipmentBook.xml";

	public const string RUNE_BOOK = "RuneBook.xml";

	public const string MATERIAL_BOOK = "MaterialBook.xml";

	public const string FISH_BOOK = "FishBook.xml";

	public const string BAIT_BOOK = "BaitBook.xml";

	public const string BOBBER_BOOK = "BobberBook.xml";

	public const string CURRENCY_BOOK = "CurrencyBook.xml";

	public const string CONSUMABLE_BOOK = "ConsumableBook.xml";

	public const string SERVICE_BOOK = "ServiceBook.xml";

	public const string AUGMENT_BOOK = "AugmentBook.xml";

	public const string FAMILIAR_BOOK = "FamiliarBook.xml";

	public const string ENCHANT_BOOK = "EnchantBook.xml";

	public const string MOUNT_BOOK = "MountBook.xml";

	public const string FUSION_BOOK = "FusionBook.xml";

	public const string GUILD_BOOK = "GuildBook.xml";

	public const string GUILD_SHOP_BOOK = "GuildShopBook.xml";

	public const string GUILD_HALL_BOOK = "GuildHallBook.xml";

	public const string VARIABLE_BOOK = "VariableBook.xml";

	public const string AD_BOOK = "AdBook.xml";

	public const string FISHING_BOOK = "FishingBook.xml";

	public const string DAILY_REWARDS_BOOK = "DailyRewardBook.xml";

	public const string DAILY_BONUSES_BOOK = "DailyBonusBook.xml";

	public const string DAILY_QUEST_BOOK = "DailyQuestBook.xml";

	public const string CHARACTER_ACHIEVEMENT_BOOK = "CharacterAchievementBook.xml";

	public const string CRAFT_BOOK = "CraftBook.xml";

	public const string NPC_BOOK = "NPCBook.xml";

	public const string ENCOUNTER_BOOK = "EncounterBook.xml";

	public const string DUNGEON_BOOK = "DungeonBook.xml";

	public const string ZONE_BOOK = "ZoneBook.xml";

	public const string SHOP_BOOK = "ShopBook.xml";

	public const string FISHING_SHOP_BOOK = "FishingShopBook.xml";

	public const string PVP_EVENT_BOOK = "PvPEventBook.xml";

	public const string RIFT_EVENT_BOOK = "RiftEventBook.xml";

	public const string GAUNTLET_EVENT_BOOK = "GauntletEventBook.xml";

	public const string GVG_EVENT_BOOK = "GvGEventBook.xml";

	public const string GVE_EVENT_BOOK = "GvEEventBook.xml";

	public const string INVASION_EVENT_BOOK = "InvasionEventBook.xml";

	public const string FISHING_EVENT_BOOK = "FishingEventBook.xml";

	public const string RAID_BOOK = "RaidBook.xml";

	public const string BRAWL_BOOK = "BrawlBook.xml";

	public const string INSTANCE_BOOK = "InstanceBook.xml";

	public const string PAYMENT_BOOK = "PaymentBook.xml";

	public const string LEADERBOARD_BOOK = "LeaderboardBook.xml";

	public const string NEWS_BOOK = "NewsBook.xml";

	public const string DIALOG_BOOK = "DialogBook.xml";

	public const string ACHIEVEMENT_BOOK = "AchievementBook.xml";

	public const string REWARD_BOOK = "RewardBook.xml";

	public const string EVENT_REWARD_BOOK = "EventRewardBook.xml";

	public const string FORBIDDEN_CHARACTER_NAME = "ForbiddenCharacterName.xml";

	public const string SERVER_BOOK = "ServerBook.xml";

	public const string BOOSTER_BOOK = "BoosterBook.xml";

	public const string WALLET_BOOK = "WalletBook.xml";

	public const string EVENT_SALES_SHOP_BOOK = "EventSalesShopBook.xml";

	private static XMLBook _instance;

	public AsianLanguageBookData asianLanguageBook;

	public LanguageBookData languageBook;

	public MusicBookData musicBook;

	public SoundBookData soundBook;

	public BattleBookData battleBook;

	public FilterBookData filterBook;

	public RarityBookData rarityBook;

	public ProbabilityBookData probabilityBook;

	public AbilityBookData abilityBook;

	public EquipmentBookData equipmentBookData;

	public RuneBookData runeBookData;

	public MaterialBookData materialBook;

	public FishingBookData fishingBook;

	public EventSalesShopBookData eventSalesShopBook;

	public BaitBookData baitBook;

	public BobberBookData bobberBook;

	public CurrencyBookData currencyBook;

	public ConsumableBookData consumableBook;

	public ServiceBookData serviceBook;

	public AugmentBookData augmentBookData;

	public FamiliarBookData familiarBook;

	public EnchantBookData enchantBookData;

	public MountBookData mountBook;

	public FusionBookData fusionBook;

	public GuildBookData guildBook;

	public GuildShopBookData guildShopBook;

	public GuildHallBookData guildHallBook;

	public VariableBookData variableBook;

	public AdBookData adBook;

	public FishBookData fishBook;

	public DailyRewardBookData dailyRewardBook;

	public DailyBonusBookData dailyBonusBook;

	public DailyQuestBookData dailyQuestBook;

	public CharacterAchievementBookData characterAchievementBook;

	public CraftBookData craftBook;

	public NPCBookData NPCBook;

	public EncounterBookData encounterBook;

	public DungeonBookData dungeonBook;

	public ZoneBookData zoneBook;

	public ShopBookData shopBook;

	public FishingShopBookData fishingShopBook;

	public BaseEventBookData PVPEventBook;

	public BaseEventBookData riftEventBook;

	public BaseEventBookData gauntletEventBook;

	public BaseEventBookData GVEEventBook;

	public BaseEventBookData GVGEventBook;

	public BaseEventBookData invasionEventBook;

	public BaseEventBookData fishingEventBook;

	public RaidBookData raidBook;

	public BrawlBookData brawlBook;

	public InstanceBookData instanceBook;

	public PaymentBookData paymentBook;

	public LeaderboardBookData leaderboardBook;

	public NewsBookData newsBook;

	public DialogBookData dialogBook;

	public AchievementBookData achievementBook;

	public RewardBookData rewardBook;

	public EventRewardBookData eventRewardBook;

	public ForbiddenCharacterName forbiddenCharacterName;

	public ServerBookData serverBookData;

	public BoosterBookData boosterBook;

	public WalletBookData walletBookData;

	public UnityEvent OnProcessXMLComplete = new UnityEvent();

	public UnityEvent OnProcessXMLError = new UnityEvent();

	private int _booksTotal;

	private int _steps;

	private float _initializationProgressIncrease;

	public int processingBeforeYield = 30;

	private int currentProcessingCount;

	public static XMLBook instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new XMLBook();
			}
			return _instance;
		}
	}

	public void Clear()
	{
		if (asianLanguageBook != null)
		{
			asianLanguageBook.Clear();
		}
		asianLanguageBook = null;
		if (languageBook != null)
		{
			languageBook.Clear();
		}
		languageBook = null;
		if (musicBook != null)
		{
			musicBook.Clear();
		}
		musicBook = null;
		if (soundBook != null)
		{
			soundBook.Clear();
		}
		soundBook = null;
		if (battleBook != null)
		{
			battleBook.Clear();
		}
		battleBook = null;
		if (filterBook != null)
		{
			filterBook.Clear();
		}
		filterBook = null;
		if (rarityBook != null)
		{
			rarityBook.Clear();
		}
		rarityBook = null;
		if (probabilityBook != null)
		{
			probabilityBook.Clear();
		}
		probabilityBook = null;
		if (abilityBook != null)
		{
			abilityBook.Clear();
		}
		abilityBook = null;
		if (equipmentBookData != null)
		{
			equipmentBookData.Clear();
		}
		equipmentBookData = null;
		if (runeBookData != null)
		{
			runeBookData.Clear();
		}
		runeBookData = null;
		if (materialBook != null)
		{
			materialBook.Clear();
		}
		materialBook = null;
		if (fishingBook != null)
		{
			fishingBook.Clear();
		}
		fishingBook = null;
		if (baitBook != null)
		{
			baitBook.Clear();
		}
		baitBook = null;
		if (bobberBook != null)
		{
			bobberBook.Clear();
		}
		bobberBook = null;
		if (currencyBook != null)
		{
			currencyBook.Clear();
		}
		currencyBook = null;
		if (consumableBook != null)
		{
			consumableBook.Clear();
		}
		consumableBook = null;
		if (serviceBook != null)
		{
			serviceBook.Clear();
		}
		serviceBook = null;
		if (augmentBookData != null)
		{
			augmentBookData.Clear();
		}
		augmentBookData = null;
		if (familiarBook != null)
		{
			familiarBook.Clear();
		}
		familiarBook = null;
		enchantBookData = null;
		mountBook = null;
		fusionBook = null;
		guildBook = null;
		guildShopBook = null;
		guildHallBook = null;
		variableBook = null;
		adBook = null;
		fishBook = null;
		dailyRewardBook = null;
		dailyBonusBook = null;
		dailyQuestBook = null;
		characterAchievementBook = null;
		craftBook = null;
		NPCBook = null;
		encounterBook = null;
		dungeonBook = null;
		zoneBook = null;
		shopBook = null;
		fishingShopBook = null;
		if (PVPEventBook != null)
		{
			PVPEventBook.Clear();
		}
		PVPEventBook = null;
		if (riftEventBook != null)
		{
			riftEventBook.Clear();
		}
		riftEventBook = null;
		if (gauntletEventBook != null)
		{
			gauntletEventBook.Clear();
		}
		gauntletEventBook = null;
		if (GVEEventBook != null)
		{
			GVEEventBook.Clear();
		}
		GVEEventBook = null;
		if (GVEEventBook != null)
		{
			GVGEventBook.Clear();
		}
		GVGEventBook = null;
		if (invasionEventBook != null)
		{
			invasionEventBook.Clear();
		}
		invasionEventBook = null;
		if (fishingEventBook != null)
		{
			fishingEventBook.Clear();
		}
		fishingEventBook = null;
		raidBook = null;
		brawlBook = null;
		instanceBook = null;
		paymentBook = null;
		leaderboardBook = null;
		newsBook = null;
		dialogBook = null;
		achievementBook = null;
		rewardBook = null;
		eventRewardBook = null;
		forbiddenCharacterName = null;
		boosterBook = null;
		serverBookData = null;
		walletBookData = null;
		eventSalesShopBook = null;
		_instance = null;
	}

	public IEnumerator LoadFromSFSObject(SFSObject sfsObject)
	{
		Dictionary<string, XmlDocument> unmatched = new Dictionary<string, XmlDocument>();
		ISFSArray sFSArray = sfsObject.GetSFSArray("xml0");
		bool assetBundlesEnabled = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("EnableAssetBundles").AsBool;
		_booksTotal = 0;
		_steps = 0;
		currentProcessingCount = 0;
		_initializationProgressIncrease = 0f;
		foreach (ISFSObject item in sFSArray)
		{
			string name = item.GetUtfString("xml1");
			if (XMLDLCConfig.ContainsKey(name) || XMLDLCConfig.ContainsKey("zones/" + name))
			{
				D.Log("rosty", "Skipping: " + name);
				continue;
			}
			D.Log("rosty", "Adding: " + name);
			string input;
			if (AppInfo.IsWeb())
			{
				item.GetUtfString("xml2");
				input = Util.Base64Decode(item.GetUtfString("xml2"));
			}
			else
			{
				ByteArray byteArray = item.GetByteArray("xml2");
				input = Util.UncompressString(byteArray);
			}
			yield return null;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(input);
			if (!InstantiateBook(name, xmlDocument))
			{
				unmatched.Add(name, xmlDocument);
			}
			else
			{
				_booksTotal++;
			}
		}
		bool errorLoadingXML = false;
		string[] whitelist = XMLDLCConfig.GetWhitelist();
		foreach (string path in whitelist)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string fileName = Path.GetFileName(path);
			TextAsset textAsset = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset<TextAsset>(fileName);
			if (!assetBundlesEnabled && textAsset == null)
			{
				textAsset = (TextAsset)Resources.Load("xml-resources/" + fileNameWithoutExtension);
			}
			if (textAsset == null)
			{
				Debug.LogError("Error loading " + fileNameWithoutExtension);
				errorLoadingXML = true;
				break;
			}
			string text = textAsset.text;
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.LoadXml(text);
			if (!InstantiateBook(fileName, xmlDocument2))
			{
				unmatched.Add(fileName, xmlDocument2);
			}
			else
			{
				_booksTotal++;
			}
			yield return null;
		}
		if (errorLoadingXML)
		{
			OnProcessXMLError.Invoke();
			yield break;
		}
		string languageData = ((!SingletonMonoBehaviour<EnvironmentManager>.instance.loadDLCFromBundledResources || !Application.isEditor) ? Language.LoadLangFromDLC(assetBundlesEnabled) : Language.LoadLangFromBundledResources());
		if (languageData == null)
		{
			OnProcessXMLError.Invoke();
			yield break;
		}
		yield return null;
		XmlDocument xmlDocument3 = new XmlDocument();
		xmlDocument3.LoadXml(languageData);
		Language.Init(xmlDocument3);
		_booksTotal++;
		List<ZoneXMLData> lstZoneData = new List<ZoneXMLData>();
		D.Log("rosty", $"ZoneBook files count {zoneBook.zoneFiles.lstFiles.Count}");
		for (int i = 0; i < zoneBook.zoneFiles.lstFiles.Count; i++)
		{
			ZoneBookData.ZoneFile zoneFile = zoneBook.zoneFiles.lstFiles[i];
			string xml = zoneFile.xml;
			D.Log("rosty", "ZONE FILE: " + xml);
			D.Log("rosty", $"ZONE             : {unmatched[xml]}");
			ZoneXMLData type = new ZoneXMLData();
			if (unmatched.ContainsKey(xml))
			{
				GetObjectFromXMLDocument(ref type, unmatched[xml]);
				type.zone.id = zoneFile.id;
				lstZoneData.Add(type);
			}
			yield return null;
		}
		zoneBook.dictZones = lstZoneData;
		PlayerPrefs.Save();
		OnProcessXMLComplete?.Invoke();
	}

	private bool InstantiateBook(string name, XmlDocument doc)
	{
		switch (name)
		{
		case "AsianLanguagesBook.xml":
			GetObjectFromXMLDocument(ref asianLanguageBook, doc);
			return true;
		case "LanguageBook.xml":
			GetObjectFromXMLDocument(ref languageBook, doc);
			return true;
		case "MusicBook.xml":
			GetObjectFromXMLDocument(ref musicBook, doc);
			return true;
		case "SoundBook.xml":
			GetObjectFromXMLDocument(ref soundBook, doc);
			return true;
		case "BattleBook.xml":
			GetObjectFromXMLDocument(ref battleBook, doc);
			return true;
		case "FilterBook.xml":
			GetObjectFromXMLDocument(ref filterBook, doc);
			return true;
		case "RarityBook.xml":
			GetObjectFromXMLDocument(ref rarityBook, doc);
			return true;
		case "ProbabilityBook.xml":
			GetObjectFromXMLDocument(ref probabilityBook, doc);
			return true;
		case "AbilityBook.xml":
			GetObjectFromXMLDocument(ref abilityBook, doc);
			return true;
		case "EquipmentBook.xml":
			GetObjectFromXMLDocument(ref equipmentBookData, doc);
			return true;
		case "RuneBook.xml":
			GetObjectFromXMLDocument(ref runeBookData, doc);
			return true;
		case "MaterialBook.xml":
			GetObjectFromXMLDocument(ref materialBook, doc);
			return true;
		case "FishingBook.xml":
			GetObjectFromXMLDocument(ref fishingBook, doc);
			return true;
		case "BaitBook.xml":
			GetObjectFromXMLDocument(ref baitBook, doc);
			return true;
		case "BobberBook.xml":
			GetObjectFromXMLDocument(ref bobberBook, doc);
			return true;
		case "CurrencyBook.xml":
			GetObjectFromXMLDocument(ref currencyBook, doc);
			return true;
		case "ConsumableBook.xml":
			GetObjectFromXMLDocument(ref consumableBook, doc);
			return true;
		case "ServiceBook.xml":
			GetObjectFromXMLDocument(ref serviceBook, doc);
			return true;
		case "AugmentBook.xml":
			GetObjectFromXMLDocument(ref augmentBookData, doc);
			return true;
		case "FamiliarBook.xml":
			GetObjectFromXMLDocument(ref familiarBook, doc);
			return true;
		case "EnchantBook.xml":
			GetObjectFromXMLDocument(ref enchantBookData, doc);
			return true;
		case "MountBook.xml":
			GetObjectFromXMLDocument(ref mountBook, doc);
			return true;
		case "FusionBook.xml":
			GetObjectFromXMLDocument(ref fusionBook, doc);
			return true;
		case "GuildBook.xml":
			GetObjectFromXMLDocument(ref guildBook, doc);
			return true;
		case "GuildShopBook.xml":
			GetObjectFromXMLDocument(ref guildShopBook, doc);
			return true;
		case "GuildHallBook.xml":
			GetObjectFromXMLDocument(ref guildHallBook, doc);
			return true;
		case "VariableBook.xml":
			GetObjectFromXMLDocument(ref variableBook, doc);
			return true;
		case "AdBook.xml":
			GetObjectFromXMLDocument(ref adBook, doc);
			return true;
		case "FishBook.xml":
			GetObjectFromXMLDocument(ref fishBook, doc);
			return true;
		case "DailyRewardBook.xml":
			GetObjectFromXMLDocument(ref dailyRewardBook, doc);
			return true;
		case "DailyBonusBook.xml":
			GetObjectFromXMLDocument(ref dailyBonusBook, doc);
			return true;
		case "DailyQuestBook.xml":
			GetObjectFromXMLDocument(ref dailyQuestBook, doc);
			return true;
		case "CharacterAchievementBook.xml":
			GetObjectFromXMLDocument(ref characterAchievementBook, doc);
			return true;
		case "CraftBook.xml":
			GetObjectFromXMLDocument(ref craftBook, doc);
			return true;
		case "NPCBook.xml":
			GetObjectFromXMLDocument(ref NPCBook, doc);
			return true;
		case "EncounterBook.xml":
			GetObjectFromXMLDocument(ref encounterBook, doc);
			return true;
		case "DungeonBook.xml":
			GetObjectFromXMLDocument(ref dungeonBook, doc);
			return true;
		case "ZoneBook.xml":
			GetObjectFromXMLDocument(ref zoneBook, doc);
			return true;
		case "ShopBook.xml":
			GetObjectFromXMLDocument(ref shopBook, doc);
			return true;
		case "FishingShopBook.xml":
			GetObjectFromXMLDocument(ref fishingShopBook, doc);
			return true;
		case "PvPEventBook.xml":
			GetObjectFromXMLDocument(ref PVPEventBook, doc);
			return true;
		case "RiftEventBook.xml":
			GetObjectFromXMLDocument(ref riftEventBook, doc);
			return true;
		case "GauntletEventBook.xml":
			GetObjectFromXMLDocument(ref gauntletEventBook, doc);
			return true;
		case "InvasionEventBook.xml":
			GetObjectFromXMLDocument(ref invasionEventBook, doc);
			return true;
		case "FishingEventBook.xml":
			GetObjectFromXMLDocument(ref fishingEventBook, doc);
			return true;
		case "GvEEventBook.xml":
			GetObjectFromXMLDocument(ref GVEEventBook, doc);
			return true;
		case "GvGEventBook.xml":
			GetObjectFromXMLDocument(ref GVGEventBook, doc);
			return true;
		case "RaidBook.xml":
			GetObjectFromXMLDocument(ref raidBook, doc);
			return true;
		case "BrawlBook.xml":
			GetObjectFromXMLDocument(ref brawlBook, doc);
			return true;
		case "InstanceBook.xml":
			GetObjectFromXMLDocument(ref instanceBook, doc);
			return true;
		case "PaymentBook.xml":
			GetObjectFromXMLDocument(ref paymentBook, doc);
			return true;
		case "LeaderboardBook.xml":
			GetObjectFromXMLDocument(ref leaderboardBook, doc);
			return true;
		case "NewsBook.xml":
			GetObjectFromXMLDocument(ref newsBook, doc);
			return true;
		case "DialogBook.xml":
			GetObjectFromXMLDocument(ref dialogBook, doc);
			return true;
		case "AchievementBook.xml":
			GetObjectFromXMLDocument(ref achievementBook, doc);
			return true;
		case "RewardBook.xml":
			GetObjectFromXMLDocument(ref rewardBook, doc);
			return true;
		case "EventRewardBook.xml":
			GetObjectFromXMLDocument(ref eventRewardBook, doc);
			return true;
		case "ForbiddenCharacterName.xml":
			GetObjectFromXMLDocument(ref forbiddenCharacterName, doc);
			return true;
		case "BoosterBook.xml":
			GetObjectFromXMLDocument(ref boosterBook, doc);
			return true;
		case "WalletBook.xml":
			GetObjectFromXMLDocument(ref walletBookData, doc);
			return true;
		case "EventSalesShopBook.xml":
			GetObjectFromXMLDocument(ref eventSalesShopBook, doc);
			return true;
		default:
			return false;
		}
	}

	public void GetObjectFromXMLDocument<T>(ref T type, XmlDocument doc)
	{
		type = default(T);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		type = (T)xmlSerializer.Deserialize(new XmlNodeReader(doc));
	}

	public float UpdateProgress()
	{
		_steps++;
		if (_initializationProgressIncrease == 0f)
		{
			_initializationProgressIncrease = 100f / (float)_booksTotal;
		}
		if (_steps > _booksTotal)
		{
			_steps = _booksTotal;
		}
		return _initializationProgressIncrease * (float)_steps;
	}

	public bool UpdateProcessingCount()
	{
		currentProcessingCount++;
		if (currentProcessingCount % processingBeforeYield == 0)
		{
			currentProcessingCount = 0;
			return true;
		}
		return false;
	}
}
