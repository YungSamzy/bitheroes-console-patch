using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.ad;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.login;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.heroselector;
using com.ultrabit.bitheroes.ui.utility;
using Kongregate;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using SimpleJSON;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.application;

public class AppInfo
{
	private class AdQualitySdkInitLogger : ISAdQualityInitCallback
	{
		public void adQualitySdkInitSuccess()
		{
		}

		public void adQualitySdkInitFailed(ISAdQualityInitError adQualitySdkInitError, string errorMessage)
		{
			D.LogError("unity: adQualitySdkInitFailed " + adQualitySdkInitError.ToString() + " message: " + errorMessage);
		}
	}

	public const int PLATFORM_LOCAL = 0;

	public const int PLATFORM_ANDROID = 1;

	public const int PLATFORM_IOS = 2;

	public const int PLATFORM_STANDALONE = 3;

	public const int PLATFORM_KONGREGATE = 4;

	public const int PLATFORM_FACEBOOK = 5;

	public const int PLATFORM_GOOGLE_PLAY = 6;

	public const int PLATFORM_STEAM = 7;

	public const int PLATFORM_KARTRIDGE = 8;

	public const int PLATFORM_DESKTOP = 9;

	public const int PLATFORM_COUNT = 10;

	private static Dictionary<string, int> PLATFORM_TYPES = new Dictionary<string, int>
	{
		{ "local", 0 },
		{ "android", 1 },
		{ "ios", 2 },
		{ "standalone", 3 },
		{ "kongregate", 4 },
		{ "facebook", 5 },
		{ "googleplay", 6 },
		{ "steam", 7 },
		{ "kartridge", 8 },
		{ "desktop", 9 }
	};

	public const int BROWSER_NONE = 0;

	public const int BROWSER_UNKNOWN = 1;

	public const int BROWSER_CHROME = 2;

	public const int BROWSER_FIREFOX = 3;

	public const int BROWSER_SAFARI = 4;

	public const string SECRET_HEX = "k5iw3la0";

	public const string DEFAULT_LANGUAGE = "en";

	public static bool TESTING = false;

	public const float TESTING_SPEED = 3f;

	private static int _platform = -1;

	private static string _version = "";

	private static string _userID = "";

	private static string _userToken = "";

	private static string _userEmail = "";

	private static string _userName = "";

	private static int _playerID = -1;

	private static AppData _appData;

	private const string GUEST_NAME = "guest";

	public const string ANDROID_APP_URL = "https://play.google.com/store/apps/details?id=com.kongregate.mobile.bitheroes.google";

	public const string IOS_APP_URL = "https://itunes.apple.com/us/app/bit-heroes/id1176312930?ls=1&mt=8";

	private static List<int> NOTIFICATION_IDENTIFIER = new List<int>();

	public const int NOTIFICATION_ENERGY_ID = 1;

	public const int NOTIFICATION_TICKETS_ID = 2;

	public const int NOTIFICATION_DAILY_REWARD = 3;

	public const int NOTIFICATION_DAY_SEVEN = 4;

	public const int NOTIFICATION_DAY_THIRTY = 5;

	private static bool _kongAvailable = false;

	private static bool _isKongLogged = false;

	private static bool _adsLoaded = false;

	private static bool _offerwallAvailable = false;

	private static bool _offerwallShown = false;

	private static bool _adCompleted = false;

	private static float _adTime = 0f;

	public static AdEventDispatcher adEventDispatcher = new AdEventDispatcher();

	public static int AD_REWARD_DELAYED_STEPS = 10;

	public static int AD_REWARD_DELAYED_CHECK = 100;

	private static Coroutine _adRewardTimer;

	private static string PreviousKongIDLogged = null;

	private static float realtimeSinceStartup;

	private static DialogWindow dialogWindow;

	public static int platform => _platform;

	public static string version => _version;

	public static string userID
	{
		get
		{
			return _userID ?? string.Empty;
		}
		set
		{
			_userID = value;
		}
	}

	public static string userToken
	{
		get
		{
			return _userToken;
		}
		set
		{
			_userToken = value;
		}
	}

	public static string userEmail => _userEmail;

	public static string userName => _userName;

	public static int playerID
	{
		get
		{
			return _playerID;
		}
		set
		{
			_playerID = value;
		}
	}

	public static AppData appData => _appData;

	public static bool allowRatings
	{
		get
		{
			int num = _platform;
			if ((uint)(num - 1) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	public static bool allowNBPZ => PaymentBook.GetAllPaymentsByTypeAndZone(3, GameData.instance.PROJECT.character.zoneCompleted).Count > 0;

	public static bool allowKeycodes
	{
		get
		{
			if (platform == 0)
			{
				return true;
			}
			if (!IsDesktop())
			{
				return IsWeb();
			}
			return true;
		}
	}

	public static bool live => SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("live").AsBool;

	public static string deviceID => PlatformLoginHandler.instance.GetPlatformLogin()?.GetPlatformSpecificUserID();

	public static string deviceToken => PlatformLoginHandler.instance.GetPlatformLogin()?.GetUserToken();

	public static string anonID => GameData.instance.SAVE_STATE.uid;

	public static bool kongButtonAvailable
	{
		get
		{
			if (!IsWeb())
			{
				return kongApiAvailable;
			}
			return false;
		}
	}

	public static bool kongApiAvailable
	{
		get
		{
			bool flag = KongregateAPI.GetAPI() != null;
			if (!flag)
			{
				D.LogError($"KongregateInitFlow kongApiAvailable  - NotNull: {flag} && Available: {_kongAvailable}");
			}
			if (flag)
			{
				return _kongAvailable;
			}
			return false;
		}
	}

	public static KongregateAPI kongApi => KongregateAPI.GetAPI();

	public static string kongID
	{
		get
		{
			if (kongApi != null && kongApi.Services != null && kongApi.Services.GetUserId() > 0)
			{
				return kongApi.Services.GetUserId().ToString();
			}
			return null;
		}
	}

	public static string kongUsername
	{
		get
		{
			if (kongApi != null && kongApi.Services != null)
			{
				string username = kongApi.Services.GetUsername();
				if (username != null && username.Length > 0 && !username.ToLowerInvariant().Equals("guest"))
				{
					return username;
				}
			}
			return null;
		}
	}

	public static string kongToken => kongApi.Services.GetGameAuthToken();

	public static bool useSSL => SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("useSSL").AsBool;

	public static bool offerwallAvailable => _offerwallAvailable;

	public static bool adsAvailable
	{
		get
		{
			if (!_adsLoaded)
			{
				return false;
			}
			bool num = IronSource.Agent.isRewardedVideoAvailable();
			if (!num)
			{
				GameData.instance.main.ReCheckAdsAvailability();
			}
			return num;
		}
	}

	public static bool allowLogout
	{
		get
		{
			switch (_platform)
			{
			case 1:
			case 2:
			case 4:
			case 5:
			case 7:
			case 8:
				return false;
			default:
				return true;
			}
		}
	}

	public static bool allowAds
	{
		get
		{
			int num = _platform;
			if ((uint)(num - 1) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	public static bool allowAchievements => SteamManager.Initialized;

	public static bool allowAchievementShowable => false;

	public static bool allowAppNotifications => false;

	public static bool resizable => IsDesktop();

	public static bool allowNBP => PaymentBook.GetFirstPaymentByType(3) != null;

	public static int GetClientPlatform()
	{
		switch (platform)
		{
		case 2:
			return 2;
		case 1:
		case 6:
			return 1;
		case 7:
			return 4;
		case 4:
			return 3;
		case 8:
			return 5;
		default:
			return 6;
		}
	}

	public static void Init()
	{
		if (_platform < 0)
		{
			_platform = getCurrentPlatform();
		}
		if (version.Length <= 0)
		{
			_version = Application.version;
		}
	}

	public static void InitLibraries()
	{
		switch (_platform)
		{
		case 1:
		case 2:
		case 4:
			DoLoadKongregateAPI();
			GameData.instance.main.CreatePurchaseBehaviourCallback();
			RegisterForNotifications();
			break;
		case 7:
			GameData.instance.main.EnableSteamManager(enable: true);
			KongregateAPI.Settings.SteamAdapterClass = typeof(SteamworksAdapter);
			KongregateAPI.Settings.BundleID = "com.kongregate.steam.bitheroes";
			KongregateAPI.Settings.AppVersion = _version;
			DoLoadKongregateAPI();
			break;
		default:
			DoLoadKongregateAPI();
			break;
		}
	}

	private static void RegisterForNotifications()
	{
	}

	public static void InitIAP()
	{
		switch (_platform)
		{
		case 1:
		case 2:
			if (GameData.instance.transactionIAPMobile == null)
			{
				TransactionIAPMobile transactionIAPMobile = UnityEngine.Object.FindObjectOfType<TransactionIAPMobile>();
				if (transactionIAPMobile == null)
				{
					transactionIAPMobile = new GameObject("TransactionIAPMobile").AddComponent<TransactionIAPMobile>();
				}
				GameData.instance.transactionIAPMobile = transactionIAPMobile;
				GameData.instance.transactionIAPMobile.InitializePurchasing(3);
			}
			break;
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
			break;
		}
	}

	private static int getCurrentPlatform()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.WebGLPlayer:
			return 4;
		case RuntimePlatform.Android:
			return 1;
		case RuntimePlatform.IPhonePlayer:
			return 2;
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsPlayer:
			return 7;
		default:
			return 0;
		}
	}

	public static string getVersionFromXml(XmlDocument xml)
	{
		foreach (XmlElement item in xml.FirstChild.SelectNodes("version"))
		{
			string attribute = item.GetAttribute("version");
			foreach (int item2 in getPlatformIDsFromString(item.GetAttribute("platforms")))
			{
				if (item2 == platform)
				{
					return attribute;
				}
			}
		}
		return "";
	}

	public static void setAppData(AppData data)
	{
		_appData = data;
	}

	public static string getHashToken(string userID, string userName)
	{
		return ServerExtension.instance.GenerateHash(userID + userName);
	}

	public static string GetLanguage()
	{
		string language = GameData.instance.SAVE_STATE.language;
		if (language != null && language.Length > 0)
		{
			return language;
		}
		string text = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToString();
		if (text != null && text.Length > 0)
		{
			return Util.getStringBreak(text);
		}
		return "en";
	}

	public static string GetUserName()
	{
		if (_userName != null && _userName.Length > 0)
		{
			return _userName;
		}
		return "";
	}

	public static string GetPossibleCharacterName()
	{
		string text = GetUserName();
		if (text == null || text.Length <= 0)
		{
			return "";
		}
		text = Util.getStrippedNameString(text);
		if (text.Length > VariableBook.characterNameLength)
		{
			text = text.Substring(0, VariableBook.characterNameLength);
		}
		return text;
	}

	public static string GetSystem()
	{
		string text = null;
		text = ((!IsMobile()) ? SystemInfo.operatingSystem : SystemInfo.deviceModel);
		if (text == null || text.Length <= 0)
		{
			text = "UNAVAILABLE";
		}
		return text;
	}

	public static bool getResizable()
	{
		_ = platform;
		return IsDesktop();
	}

	public static List<int> getPlatformIDsFromString(string str)
	{
		string[] stringArrayFromStringProperty = Util.GetStringArrayFromStringProperty(str);
		List<int> list = new List<int>();
		string[] array = stringArrayFromStringProperty;
		for (int i = 0; i < array.Length; i++)
		{
			int platformID = getPlatformID(array[i]);
			list.Add(platformID);
		}
		return list;
	}

	public static int getPlatformID(string key)
	{
		int value = 0;
		PLATFORM_TYPES.TryGetValue(key.ToLower(), out value);
		return value;
	}

	public static string getPlatformKey(int plat)
	{
		foreach (KeyValuePair<string, int> pLATFORM_TYPE in PLATFORM_TYPES)
		{
			string key = pLATFORM_TYPE.Key;
			if (pLATFORM_TYPE.Value == plat)
			{
				return key;
			}
		}
		return "";
	}

	public static bool IsMobile()
	{
		int num = _platform;
		if ((uint)(num - 1) <= 1u)
		{
			return true;
		}
		return false;
	}

	public static bool CheckInternet()
	{
		if (!IsWeb())
		{
			return platform != 0;
		}
		return false;
	}

	public static bool IsWeb()
	{
		int num = _platform;
		if ((uint)(num - 3) <= 2u)
		{
			return true;
		}
		return false;
	}

	public static bool IsDesktop()
	{
		int num = platform;
		if (num == 0 || (uint)(num - 7) <= 2u)
		{
			return true;
		}
		return false;
	}

	public static bool IsAuthLogin()
	{
		switch (platform)
		{
		case 1:
		case 2:
		case 4:
		case 5:
		case 7:
		case 8:
			return true;
		default:
			return false;
		}
	}

	public static void doCancelAllLocalNotification()
	{
	}

	public static void doCancelLocalNotification(int notificationID)
	{
		if (allowAppNotifications)
		{
			_ = GameData.instance.SAVE_STATE.appNotificationsDisabled;
		}
	}

	public static void doScheduleLocalNotification(int notificationID, int seconds, string name, string desc)
	{
		if (allowAppNotifications && !GameData.instance.SAVE_STATE.appNotificationsDisabled)
		{
			doCancelLocalNotification(notificationID);
		}
	}

	public static void setUserName(string name)
	{
		_userName = name;
	}

	public static string GetPlatformName(int type)
	{
		return Language.GetString("platform_" + type + "_name");
	}

	public static int GetPlatformType(string type)
	{
		return PLATFORM_TYPES[type.ToLower()];
	}

	public static string GetServersURL()
	{
		return SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigAsUrlCacheBuster("serverURL");
	}

	public static string GetVersionURL()
	{
		return SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("mobileVersionURL");
	}

	public static string getAndroidValidateURL()
	{
		return SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("androidValidateURL");
	}

	public static string getiOSValidateURL()
	{
		return SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("iosValidateURL");
	}

	public static void DoLoadKongregateAPI()
	{
		GameData.instance.main.AddBreadcrumb("DoLoadKongregateAPI");
		if (!(GameData.instance.main == null) && !(GameData.instance.main.kongregateManager == null) && !(GameData.instance.main.kongregateManager.gameObject == null))
		{
			GameData.instance.main.kongregateManager.ConfigJSON = new TextAsset(SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("kongregate").ToString());
			KongregateInitBase.eventReady += KongregateManager_eventReady;
			KongregateInitBase.eventGameAuthChanged -= KongregateManager_onKongregateEventBundle;
			KongregateInitBase.eventGameAuthChanged += KongregateManager_onKongregateEventBundle;
			KongregateInitBase.eventUserChanged -= KongregateManager_onKongregateEventBundle;
			KongregateInitBase.eventUserChanged += KongregateManager_onKongregateEventBundle;
			KongregateInitBase.onKongregateEventBundle -= KongregateManager_onKongregateEventBundle;
			KongregateInitBase.onKongregateEventBundle += KongregateManager_onKongregateEventBundle;
			KongregateInitBase.eventLogout -= KongregateManager_onKongregateEventBundle;
			KongregateInitBase.eventLogout += KongregateManager_onKongregateEventBundle;
			D.Log("--------- STEAM EVENTS ADDED -------------");
			GameData.instance.main.AddBreadcrumb("Activating KongregateInit");
			GameData.instance.main.kongregateManager.gameObject.SetActive(value: true);
			if (kongApi.Analytics != null)
			{
				kongApi.Analytics.SetCommonPropsCallback(KongregateAnalytics.CommonPropsCallback);
				kongApi.Analytics.UpdateCommonProps();
			}
		}
	}

	private static void KongregateManager_onKongregateEventBundle()
	{
		float num = realtimeSinceStartup - Time.realtimeSinceStartup;
		float num2 = 1.5f;
		if (num < 0f - num2)
		{
			realtimeSinceStartup = Time.realtimeSinceStartup;
			OnAuthChanged();
		}
	}

	public static void AddLoggingKeys(ref Dictionary<string, string> fields)
	{
		fields.Add("UserName", userName);
		fields.Add("UserEmail", userEmail);
		fields.Add("KongregateUserId", kongApiAvailable ? kongID : "N/A");
		fields.Add("KongregateUserName", kongApiAvailable ? kongUsername : "N/A");
		fields.Add("CharacterID", GameData.instance.SAVE_STATE.characterID.ToString());
		fields.Add("CharacterName", GameData.instance.SAVE_STATE.characterName);
		fields.Add("CharacterHerotag", GameData.instance.SAVE_STATE.herotag);
		fields.Add("CharacterHeroType", GameData.instance.SAVE_STATE.heroType);
	}

	private static void OnUserChange()
	{
		GameData.instance.main.AddBreadcrumb($"OnUserChange {kongApi.Services.GetUserId()}");
	}

	private static void KongregateManager_eventGameAuthChanged()
	{
		GameData.instance.main.AddBreadcrumb($"KongregateManager_eventGameAuthChanged {kongApi.Services.GetUserId()}");
	}

	private static void KongregateManager_eventReady()
	{
		GameData.instance.main.AddBreadcrumb("KongregateManager_eventReady");
		if (!_kongAvailable)
		{
			_kongAvailable = true;
			kongApi.Stats.Submit("initialize", 1L);
			GameData.instance.main.AddBreadcrumb("initialize stats");
		}
	}

	public static bool DoKongregateLoginCheck()
	{
		if (kongApiAvailable && kongApi.Services.GetUserId() > 0 && kongApi.Services.GetGameAuthToken() != null && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.instance != null && !GameData.instance.PROJECT.character.hasPlatform(4))
		{
			HeroSelectWindow.OpenWindow();
			_isKongLogged = false;
			return true;
		}
		if (GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.instance == null)
		{
			_isKongLogged = false;
			return false;
		}
		if (GameData.instance.PROJECT.character.toCharacterData().isIMXG0 && !IsKongLogged())
		{
			LogoutNotification();
			return true;
		}
		PreviousKongIDLogged = kongApi.Services.GetUserId().ToString();
		return false;
	}

	public static string GetIronSourceAppKey()
	{
		return _platform switch
		{
			1 => SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("IronSource")["Android"], 
			2 => SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("IronSource")["IOS"], 
			_ => null, 
		};
	}

	public static string GetIronSourceADQualityKey()
	{
		return SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("IronSource")["ADQualityKey"];
	}

	public static void InitAds()
	{
		if (_adsLoaded || !IsMobile())
		{
			return;
		}
		try
		{
			IronSourceConfig.Instance.setClientSideCallbacks(status: true);
			IronSourceAdQuality.Initialize(GetIronSourceADQualityKey(), new ISAdQualityConfig
			{
				AdQualityInitCallback = new AdQualitySdkInitLogger()
			});
			string advertiserId = IronSource.Agent.getAdvertiserId();
			D.Log("unity-script: IronSource.Agent.getAdvertiserId : " + advertiserId);
			IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
			IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
			IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
			IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
			IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
			IronSourceEvents.onOfferwallOpenedEvent += OfferwalOpenedEvent;
			IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
			IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
			IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;
			D.Log("unity-script: IronSource.Agent.init, with adid: " + GameData.instance.SAVE_STATE.adid);
			IronSource.Agent.setUserId(GameData.instance.SAVE_STATE.adid);
			IronSource.Agent.init(GetIronSourceAppKey(), IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.OFFERWALL);
			D.Log("unity-script: IronSource.Agent.validateIntegration");
			IronSource.Agent.validateIntegration();
			D.Log("unity-script: unity version " + IronSource.unityVersion());
			CheckIronSourceAvailable();
			_offerwallAvailable = true;
			_adsLoaded = true;
		}
		catch (Exception e)
		{
			D.LogException("ADFlow", e);
		}
	}

	private static void OfferwallAvailableEvent(bool obj)
	{
		GameData.instance.main.AddBreadcrumb("OfferwallAvailableEvent");
		CheckIronSourceAvailable();
	}

	private static void OfferwallClosedEvent()
	{
		GameData.instance.main.AddBreadcrumb("OfferwallClosedEvent");
		DoCheckOfferwallCredits();
	}

	private static void OfferwallShowFailedEvent(IronSourceError obj)
	{
		GameData.instance.main.AddBreadcrumb("OfferwallShowFailedEvent");
		CheckIronSourceAvailable();
	}

	private static void OfferwalOpenedEvent()
	{
		GameData.instance.main.AddBreadcrumb("OfferwalOpenedEvent");
		CheckIronSourceAvailable();
	}

	private static void RewardedVideoAvailabilityChangedEvent(bool obj)
	{
		GameData.instance.main.AddBreadcrumb("RewardedVideoAvailabilityChangedEvent");
		CheckIronSourceAvailable();
	}

	private static void DispatchAdCompleted()
	{
		GameData.instance.main.AddBreadcrumb("DispatchAdCompleted");
		adEventDispatcher.Invoke(new AdEvent("AD_COMPLETE", Time.realtimeSinceStartup - _adTime));
		_adCompleted = false;
	}

	private static void RewardedVideoAdClosedEvent()
	{
		if (_adCompleted)
		{
			DispatchAdCompleted();
			return;
		}
		if (_adRewardTimer != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _adRewardTimer);
		}
		_adRewardTimer = GameData.instance.main.coroutineTimer.AddTimer(null, AD_REWARD_DELAYED_CHECK, CoroutineTimer.TYPE.MILLISECONDS, AD_REWARD_DELAYED_STEPS, delegate
		{
			if (_adCompleted)
			{
				DispatchAdCompleted();
			}
			else
			{
				GameData.instance.main.AddBreadcrumb("RewardedVideoAdClosedEvent ABANDONED");
				adEventDispatcher.Invoke(new AdEvent("AD_ABANDONED", Time.time - _adTime));
			}
		}, delegate
		{
			if (_adCompleted)
			{
				GameData.instance.main.coroutineTimer.StopTimer(ref _adRewardTimer);
				DispatchAdCompleted();
			}
		});
	}

	private static void RewardedVideoAdRewardedEvent(IronSourcePlacement obj)
	{
		_adCompleted = true;
		GameData.instance.main.AddBreadcrumb("RewardedVideoAdRewardedEvent " + obj.getPlacementName());
		adEventDispatcher.Invoke(new AdEvent("AD_REWARDED", Time.realtimeSinceStartup - _adTime));
		CheckIronSourceAvailable();
	}

	private static void RewardedVideoAdShowFailedEvent(IronSourceError obj)
	{
		GameData.instance.main.AddBreadcrumb($"RewardedVideoAdShowFailedEvent {obj.getErrorCode()}");
		CheckIronSourceAvailable();
	}

	private static void RewardedVideoAdOpenedEvent()
	{
		GameData.instance.main.AddBreadcrumb("RewardedVideoAdOpenedEvent");
		CheckIronSourceAvailable();
	}

	public static void ShowAd(string placement)
	{
		KongregateAnalytics.trackAdStart(placement, "Rewarded Video");
		GameData.instance.main.AddBreadcrumb("ADFlow ShowAd " + placement);
		int num = platform;
		if ((uint)(num - 1) <= 1u)
		{
			ShowIronSourceAd(placement);
		}
	}

	private static void ShowIronSourceAd(string placement)
	{
		_adTime = Time.realtimeSinceStartup;
		IronSource.Agent.showRewardedVideo(placement);
	}

	public static void CheckIronSourceAvailable()
	{
		bool flag = adsAvailable;
		GameData.instance.main.AddBreadcrumb($"CheckIronSourceAvailable {flag}");
		if (flag && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.SendAdReadyMessage();
		}
	}

	public static void ShowOfferwall(string link)
	{
		_offerwallShown = true;
		if (GameData.instance.PROJECT.instance != null)
		{
			GameData.instance.PROJECT.ClearOfferwallChecked();
		}
		switch (link)
		{
		case "offerwall_ironsource":
			ShowIronSourceAdOfferwall();
			KongregateAnalytics.trackAdStart(KongregateAnalytics.getOfferwallMatch("ironSource"), "Offerwall");
			break;
		case "offerwall_revu":
			KongregateAnalytics.trackAdStart(KongregateAnalytics.getOfferwallMatch("RevU"), "Offerwall");
			ShowRevUAdOfferwall();
			break;
		default:
			throw new NotImplementedException("AppInfo.ShowOfferWall(" + link + ") :: Offerwall link not implemented.");
		}
	}

	private static void ShowIronSourceAdOfferwall()
	{
		if (_platform != 1 && _platform != 2)
		{
			throw new NotImplementedException($"AppInfo.ShowIronSourceAdOfferwall() :: Offerwall platform {_platform} not implemented.");
		}
		GameData.instance.main.AddBreadcrumb("ShowIronSourceAdOfferwall");
		IronSource.Agent.showOfferwall();
	}

	private static void ShowRevUAdOfferwall()
	{
		string forRevUOfferwallURL = GetForRevUOfferwallURL(log: true);
		if (forRevUOfferwallURL != null)
		{
			GameData.instance.main.AddBreadcrumb("ShowRevUAdOfferwall");
			Application.OpenURL(forRevUOfferwallURL + GameData.instance.SAVE_STATE.uid);
		}
	}

	public static string GetForRevUOfferwallURL(bool log = false)
	{
		JSONNode revUNode = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("RevU");
		if (revUNode == null)
		{
			Log("AppInfo.ShowRevUAdOfferwall() :: Environment does not have a RevU Node configured.");
			return null;
		}
		string text;
		switch (platform)
		{
		case 1:
			text = GetPlatformNodeValue("Android");
			break;
		case 2:
			text = GetPlatformNodeValue("IOS");
			break;
		default:
			Log($"AppInfo.ShowRevUAdOfferwall() :: Platform with ID {platform} not implemented.");
			return null;
		}
		if (text == null || text.Length <= 0)
		{
			Log("AppInfo.ShowRevUAdOfferwall() :: URL link null or empty.");
			return null;
		}
		return text;
		string GetPlatformNodeValue(string platformName)
		{
			string text2 = revUNode[platformName];
			if (text2 == null)
			{
				Log("AppInfo.ShowRevUAdOfferwall() :: Platform " + platformName + " not found.");
			}
			return text2;
		}
		void Log(string message)
		{
			if (log)
			{
				D.LogError(message);
			}
			else
			{
				D.LogWarning(message);
			}
		}
	}

	public static bool DoCheckOfferwallCredits(bool force = false)
	{
		if (GameData.instance.PROJECT.character == null)
		{
			return false;
		}
		if (!_offerwallShown && !force)
		{
			return false;
		}
		GameData.instance.main.AddBreadcrumb("DoCheckOfferwallCredits");
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(34), OnCheckOfferwallCredits);
		CharacterDALC.instance.doCheckOfferwall();
		return true;
	}

	private static void OnCheckOfferwallCredits(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(34), OnCheckOfferwallCredits);
		if (GameData.instance.PROJECT.character == null)
		{
			return;
		}
		SFSObject sfsob = dALCEvent.sfsob;
		if (!sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("off2");
			ISFSArray sFSArray = sfsob.GetSFSArray("off7");
			GameData.instance.main.AddBreadcrumb($"OnCheckOfferwallCredits - Credits: {@int}");
			if (@int > 0)
			{
				_offerwallShown = false;
				GameData.instance.windowGenerator.ShowCreditsPurchased(@int, "", Language.GetString("ui_offerwall"), sendAnalytics: false);
				List<ItemData> list = new List<ItemData>();
				list.Add(new ItemData(CurrencyBook.Lookup(2), @int));
				for (int i = 0; i < sFSArray.Size(); i++)
				{
					ISFSObject sFSObject = sFSArray.GetSFSObject(i);
					string utfString = sFSObject.GetUtfString("network");
					int int2 = sFSObject.GetInt("credits");
					List<ItemData> list2 = new List<ItemData>();
					list2.Add(new ItemData(CurrencyBook.Lookup(2), int2));
					KongregateAnalytics.trackEconomyTransaction(KongregateAnalytics.getOfferwallMatch(utfString), ItemData.parseSummary(null, list2), int2, 0);
					KongregateAnalytics.trackAdEnd(list, KongregateAnalytics.getOfferwallMatch(utfString), "Offerwall");
					D.Log("rosty", $"Offerwall: track: end: {utfString} => {int2}");
				}
			}
			GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob);
		}
		else
		{
			D.LogError("all", string.Format("ADFlow OnCheckOfferwallCredits - Error: {0}", sfsob.GetInt("err0")));
		}
	}

	public static void doKongregateAnalyticsEvent(string eventName, string jsonMap = null)
	{
		if (kongApi == null || kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			kongApi.Analytics.AddEvent(eventName, jsonMap);
		}
		catch (Exception e)
		{
			D.LogException("AnalyticsFLow", e);
		}
	}

	public static void doKongregateAnalyticsEvent(string eventName, Dictionary<string, object> evt)
	{
		if (kongApi == null || kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			kongApi.Analytics.AddEvent(eventName, evt);
		}
		catch (Exception ex)
		{
			D.LogError("all", "AnalyticsFlow doKongregateAnalyticsEvent " + ex.Message);
		}
	}

	public static void doKongregateCommonFields(Dictionary<string, object> jsonMap)
	{
		if (kongApi == null || kongApi.Analytics == null)
		{
			return;
		}
		try
		{
			kongApi.Analytics.SetCommonProperties(jsonMap);
			kongApi.Analytics.UpdateCommonProps();
		}
		catch (Exception ex)
		{
			D.LogError("all", "AnalyticsFlow doKongregateAnalyticsEvent " + ex.Message);
		}
	}

	public static float GetLeftOffset()
	{
		if (!getIPhoneX())
		{
			return 0f;
		}
		if (!getOrientationFlipped())
		{
			return getOrientationOffset();
		}
		return 0f;
	}

	public static float GetRightOffset()
	{
		if (!getIPhoneX())
		{
			return 0f;
		}
		if (getOrientationFlipped())
		{
			return 0f - getOrientationOffset();
		}
		return 0f;
	}

	private static float getOrientationOffset()
	{
		return 35f;
	}

	private static bool getIPhoneX()
	{
		if (_platform != 2)
		{
			return false;
		}
		int item = Math.Max(Screen.width, Screen.height);
		return new List<int> { 2436, 1792, 2688, 2340, 2532, 2778 }.Contains(item);
	}

	private static bool getOrientationFlipped()
	{
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
		{
			return true;
		}
		return false;
	}

	public static List<int> getPlatformTypes(string v)
	{
		List<int> list = new List<int>();
		if (v == null || v.Length <= 0)
		{
			return list;
		}
		string[] array = v.Split(',');
		foreach (string type in array)
		{
			list.Add(GetPlatformType(type));
		}
		return list;
	}

	public static void OnAuthChanged()
	{
		if (GameData.instance.PROJECT.character == null)
		{
			HeroSelectWindow.RefreshIfWindowExists();
			return;
		}
		GameData.instance.PROJECT.character.toCharacterData();
		if (PreviousKongIDLogged != null && PreviousKongIDLogged != kongApi.Services.GetUserId().ToString())
		{
			if (PreviousKongIDLogged == "0")
			{
				GameData.instance.PROJECT.character.addPlatform(4);
				OpenLinkedNotification();
			}
			else
			{
				LogoutNotification();
			}
			PreviousKongIDLogged = kongApi.Services.GetUserId().ToString();
			OnUserChange();
			KongregateManager_eventGameAuthChanged();
		}
	}

	public static void LogoutNotification()
	{
		if (!dialogWindow)
		{
			dialogWindow = GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("notification_login_reconnect"), Language.GetString("ui_reconnect"), delegate
			{
				GameData.instance.main.Logout(relog: true, reloadXMLfiles: true);
				dialogWindow = null;
			});
		}
	}

	public static void OpenLinkedNotification()
	{
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("notification_linked"), Language.GetString("notification_linked_successfully"), Language.GetString("ui_got_it"), delegate
		{
			HeroSelectWindow.OpenWindow();
		});
	}

	public static bool IsKongLogged()
	{
		bool result = !string.IsNullOrEmpty(kongID);
		if (!kongApiAvailable || kongApi.Services.GetUserId() <= 0 || kongApi.Services.GetGameAuthToken() == null)
		{
			result = false;
		}
		return result;
	}

	public static bool CheckIfNFTLogged()
	{
		if (GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.toCharacterData().isIMXG0 && !IsKongLogged())
		{
			LogoutNotification();
			return true;
		}
		return false;
	}
}
