using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using com.ultrabit.bitheroes;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.achievement;
using com.ultrabit.bitheroes.model.ad;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.bait;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.boober;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.character.achievements;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.encounter;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.fish;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.model.login;
using com.ultrabit.bitheroes.model.material;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.news;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.payment.utility;
using com.ultrabit.bitheroes.model.playervoting;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.segmented;
using com.ultrabit.bitheroes.model.server;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.wallet;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.parsing.model.utility;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.audio;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.chat;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.kongregate;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.zone;
using DG.Tweening;
using Kongregate;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : ServerExtensionBehavior
{
	public static readonly Rect DEFAULT_BOUNDS = new Rect(0f, 0f, 800f, 480f);

	public static readonly Vector2 DEFAULT_CENTER = new Vector2(DEFAULT_BOUNDS.width / 2f, DEFAULT_BOUNDS.height / 2f);

	public LoadingWindow loadingWindow;

	public Transform windowsGenerator;

	public Transform instanceInterface;

	public Transform menuInterface;

	public Transform tutorialManager;

	public GameObject currencyCreditsTile;

	public GameObject currencyGoldTile;

	public GameObject currencyEnergyTile;

	public GameObject currencyTicketsTile;

	private static GameContainer _CONTAINER;

	private static Rect _BOUNDS;

	private Project _PROJECT;

	private bool _ACTIVE = true;

	private static float _STAGE_SCALE = 1f;

	private static float _SCREEN_SCALE = 1f;

	private static float _BORDER_SCALE = 1f;

	private static float _WIDTH_SCALE = 1f;

	private static float _HEIGHT_SCALE = 1f;

	private static Vector2 _CENTER;

	private static int _FPS_TARGET = 30;

	private GameDispatcher _DISPATCHER;

	private static GameBackground _BACKGROUND;

	private static KongregateLogoTween _KONGREGATE_LOGO;

	private static GameLogo2022 _GAME_LOGO;

	private Camera _mainCamera;

	private Camera _uiCamera;

	private Camera _entitiesCamera;

	public Text version;

	public int SELECTED_INSTANCE;

	public ServerRef SELECTED_SERVER;

	private static bool _SERVER_SELECTED = false;

	public GameObject steamManager;

	public KongregateInit kongregateManager;

	public AssetLoader assetLoader;

	public LogManager logManager;

	public CoroutineTimer coroutineTimer;

	[HideInInspector]
	public PaymentUtilityBehaviour paymentUtilityBehaviour;

	public CrashlyticsManager crashlyticsManager;

	public int processedXML;

	public CurrencyBarFill[] currencies;

	public EventSystem eventSystem;

	private SFSObject xmlSFSObject;

	private SFSObject _characterLoginSFSObject;

	private PlatformLogin platformLogin;

	private Coroutine _cancelLogin;

	private bool firstTimeInitSharedObjectsObtained = true;

	private Coroutine _adReCheckTimer;

	private Action _onXMLProcessCompleted;

	public Image background;

	public bool ACTIVE => _ACTIVE;

	public static GameContainer CONTAINER => _CONTAINER;

	public static Rect BOUNDS => _BOUNDS;

	public static float SCREEN_SCALE => _SCREEN_SCALE;

	public static float BORDER_SCALE => _BORDER_SCALE;

	public GameDispatcher DISPATCHER => _DISPATCHER;

	public GameBackground BACKGROUND => _BACKGROUND;

	public static float STAGE_SCALE => _STAGE_SCALE;

	public static int FPS_TARGET => _FPS_TARGET;

	public Camera mainCamera => _mainCamera;

	public Camera uiCamera => _uiCamera;

	public Camera entitiesCamera
	{
		get
		{
			return _entitiesCamera;
		}
		set
		{
			_entitiesCamera = value;
		}
	}

	private void Awake()
	{
	}

	private IEnumerator Start()
	{
		Screen.sleepTimeout = -1;
		TouchScreenKeyboard.hideInput = true;
		Benchmark.Start("init_complete");
		SingletonMonoBehaviour<EnvironmentManager>.instance.Initialize();
		if (!SingletonMonoBehaviour<EnvironmentManager>.instance.initialized)
		{
			D.LogError("No Environment exiting");
			yield break;
		}
		AddBreadcrumb("EnvironmentObtained");
		crashlyticsManager = SingletonMonoBehaviour<CrashlyticsManager>.instance;
		crashlyticsManager.Init();
		logManager = SingletonMonoBehaviour<LogManager>.instance;
		logManager.Initialize();
		version.text = UnityCloudBuildManifest.GetInfoText() ?? "";
		D.ShowOnly(SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("debug")["dev"]);
		yield return new WaitForEndOfFrame();
		GameData.instance.Init();
		GameData.instance.eventSystem = eventSystem;
		GameData.instance.main = this;
		yield return StartCoroutine(AsyncLoadScenes());
		AppInfo.Init();
		_mainCamera = Camera.main;
		_uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		UnityEngine.Object.Instantiate(windowsGenerator);
		UnityEngine.Object.Instantiate(tutorialManager);
		AudioManager audioManager = _mainCamera.gameObject.AddComponent<AudioManager>();
		audioManager.LoadDetails(GameData.instance.main.assetLoader.GetAsset<AudioMixer>("ui/audio/MainAudioMixer"));
		GameData.instance.audioManager = audioManager;
		_CONTAINER = new GameContainer();
		GameData.instance.battleTextPool = new GameObject();
		GameData.instance.battleTextPool.name = "BattleTextPool";
		GameData.instance.battleTextPool.transform.SetParent(_mainCamera.transform);
		_DISPATCHER = base.gameObject.AddComponent<GameDispatcher>();
		Util.asianLangManager = new AsianLanguageManager(AppInfo.GetLanguage());
		GameData.instance.trophyComponent = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/TrophyComponent")).GetComponent<TrophyComponent>();
		GameData.instance.starComponent = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/StarComponent")).GetComponent<StarComponent>();
		GameData.instance.dotsComponent = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/DotsComponent")).GetComponent<DotsComponent>();
		GameData.instance.augmentsComponent = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/AugmentsComponent")).GetComponent<AugmentsComponent>();
		GameData.instance.rankComponent = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/RankComponent")).GetComponent<RankComponent>();
		yield return new WaitForEndOfFrame();
		Application.targetFrameRate = 60;
		_mainCamera.orthographicSize = DEFAULT_BOUNDS.height / 2f;
		_mainCamera.backgroundColor = Color.black;
		if (GameData.instance.RELOAD_SERVER_XML_FILES)
		{
			string text = GameData.instance.main.assetLoader.GetAsset<TextAsset>("lang/ClientLanguage").text;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(text);
			Language.Init(xmlDocument);
		}
		ServerExtension.instance.Init();
		ServerExtension.instance.AddEventListener(CustomSFSXEvent.CLOSE, OnConnectionClosedEvent);
		AddBreadcrumb("ServerExtension:InitComplete");
		GameData.instance.main = this;
		GameData.instance.windowGenerator.SetCurrencyTiles(currencyCreditsTile, currencyGoldTile, currencyEnergyTile, currencyTicketsTile);
		GameData.instance.windowGenerator.ShowCurrencies(show: false);
		background.gameObject.SetActive(value: false);
		UpdateBounds();
		if (GameData.instance.DISCONNECTED)
		{
			GameData.instance.DISCONNECTED = false;
			ShowBackground();
			if (GameData.instance.RELOG)
			{
				ContinueInitialization();
				yield break;
			}
			GameData.instance.DISCONNECTED = false;
			string description = Language.GetString("ui_disconnected_error");
			if (GameData.instance.DISCONNECT_ERROR_MESSAGE != null)
			{
				description = GameData.instance.DISCONNECT_ERROR_MESSAGE;
			}
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), description, Language.GetString("ui_reconnect"), delegate
			{
				AddBreadcrumb($"{GetType()}::OnClickReconnect");
				ContinueInitialization();
			});
		}
		else
		{
			ContinueInitialization();
		}
	}

	private void ContinueInitialization()
	{
		if (AppInfo.CheckInternet())
		{
			StartCoroutine(DoConnectInternet());
		}
		else
		{
			StartCoroutine(InitLibraries());
		}
	}

	private IEnumerator InitLibraries()
	{
		AddBreadcrumb("InitLibraries");
		AppInfo.InitLibraries();
		PlatformLoginHandler.instance.Init();
		yield return new WaitForEndOfFrame();
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("init_complete", stop: true),
			["loading_type"] = "init_complete"
		});
		yield return null;
		if (GameData.instance.SAVE_STATE.logosDisabled || !GameData.instance.DISPLAY_LOGOS)
		{
			ShowBackground();
			DoResizeTimer();
		}
		else
		{
			HideLoading();
			ShowKongregateLogo();
		}
	}

	public void HideLoading()
	{
		loadingWindow.SetText("");
		ShowLoading(visibility: false);
	}

	public void ShowLoading(string text = null)
	{
		if (text != null)
		{
			UpdateLoading(text);
		}
		else
		{
			ShowLoading(visibility: true);
		}
	}

	public void UpdateLoading(string desc = null)
	{
		if (desc == null)
		{
			desc = "";
		}
		if (!(loadingWindow == null))
		{
			if (!loadingWindow.visible)
			{
				ShowLoading();
			}
			loadingWindow.SetText(desc);
		}
	}

	private void ShowLoading(bool visibility)
	{
		loadingWindow.ShowLoading(visibility);
		if (visibility)
		{
			CONTAINER.AddToLayer(loadingWindow.gameObject, 13);
			loadingWindow.transform.SetAsLastSibling();
		}
		else
		{
			loadingWindow.transform.SetAsFirstSibling();
		}
	}

	private void DoConnectSmartFox(ConfigData config)
	{
		AddBreadcrumb("DoConnectSmartFox");
		if (!ServerExtension.instance.smartfox.IsConnected)
		{
			UpdateLoading(Language.GetString("ui_connecting_to_server"));
			ServerExtension.instance.smartfox.AddEventListener(SFSEvent.CONNECTION, OnConnectionSuccesful);
			ServerExtension.instance.Connect(config);
		}
		else
		{
			DoSmartFoxServerLogin();
		}
	}

	private void DoSmartFoxServerLogin()
	{
		ServerExtension.instance.smartfox.AddEventListener(SFSEvent.LOGIN, OnSmartFoxServerLogin);
		ServerExtension.instance.smartfox.AddEventListener(SFSEvent.LOGIN_ERROR, OnSmartFoxServerLoginError);
		UpdateLoading(Language.GetString("ui_logging_in"));
		ServerExtension.instance.GuestLogin();
	}

	private void OnSmartFoxServerLogin(BaseEvent evt)
	{
		Benchmark.Start("logged_in");
		AddBreadcrumb("GuestLoginComplete");
		ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.LOGIN, OnSmartFoxServerLogin);
		ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnSmartFoxServerLoginError);
		SFSObject sFSObject = evt.Params["data"] as SFSObject;
		string utfString = sFSObject.GetUtfString("serv1");
		string utfString2 = sFSObject.GetUtfString("serv0");
		if (sFSObject.ContainsKey("serv11"))
		{
			string[] utfStringArray = sFSObject.GetUtfStringArray("serv11");
			if (utfStringArray != null && utfStringArray.Length != 0)
			{
				XMLDLCConfig.OverrideWhiteList(utfStringArray);
			}
		}
		DateTime dateFromString = Util.GetDateFromString(utfString2);
		ServerExtension.instance.setServerInstanceID(utfString);
		ServerExtension.instance.setDate(dateFromString);
		if (!GameData.instance.RELOAD_SERVER_XML_FILES)
		{
			GameData.instance.RELOAD_SERVER_XML_FILES = true;
			ProcessLogin();
			return;
		}
		if (sFSObject.ContainsKey("xml0"))
		{
			xmlSFSObject = sFSObject;
		}
		InitLoginProcess();
	}

	private IEnumerator AsyncLoadScenes()
	{
		string[] array = new string[7] { "Dungeon", "Battle", "Fishing", "town", "guild", "armory", "player_armory" };
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			SceneManager.LoadSceneAsync(array2[i], LoadSceneMode.Additive);
			yield return new WaitForEndOfFrame();
		}
	}

	private void InitLoginProcess()
	{
		if (AppInfo.IsMobile() && !GameData.instance.SAVE_STATE.sharedObjectsObtained)
		{
			GameData.instance.SAVE_STATE.sharedObjectsObtained = true;
			try
			{
				Util.GetSharedObjects();
				DoPlatformLogin();
				return;
			}
			catch (Exception ex)
			{
				firstTimeInitSharedObjectsObtained = false;
				if (crashlyticsManager != null)
				{
					crashlyticsManager.LogException(ex);
				}
				D.LogException("Failed to Get SharedObjects Info", ex);
				HideLoading();
				GameData.instance.windowGenerator.NewConfirmMessageWindow("Ops", "We were not able to get your original game settings, Game will start with a fresh character.", "OK", delegate
				{
					ShowLoading();
					DoPlatformLogin();
				});
				return;
			}
		}
		DoPlatformLogin();
	}

	private void OnSmartFoxServerLoginError(BaseEvent evt)
	{
		ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.LOGIN, OnSmartFoxServerLogin);
		ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnSmartFoxServerLoginError);
		D.LogError("all", "[ServerConnectivityIssue] SmartFox Guest Login Failed");
		OnConnectionClosedEvent(null);
	}

	private void OnConnectionSuccesful(BaseEvent evt)
	{
		ServerExtension.instance.smartfox.RemoveEventListener(SFSEvent.CONNECTION, OnConnectionSuccesful);
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("server_connect", stop: true),
			["loading_type"] = "server_connect"
		});
		if ((bool)evt.Params["success"])
		{
			D.Log("On Connection Succeed");
			DoSmartFoxServerLogin();
			return;
		}
		if (_SERVER_SELECTED)
		{
			GameData.instance.SAVE_STATE.serverID = -1;
		}
		HideLoading();
		D.LogError("all", "[ServerConnectivityIssue] Connection to Smartfox Failed");
		ServerExtension.instance.Disconnect(Language.GetString("ui_connecting_to_server"));
	}

	private void OnConnectionClosedEvent(BaseEvent evt)
	{
		D.Log("OnConnectionClosedEvent from Main");
		OnConnectionClosed(evt);
	}

	public void OnConnectionClosed(BaseEvent evt, string error = null, bool relog = false, string serverInstanceID = null, int duelCharID = 0, int brawlIndex = 0)
	{
		if (!GameData.instance.DISCONNECTED)
		{
			ServerExtension.instance.RemoveEventListener(CustomSFSXEvent.CLOSE, OnConnectionClosedEvent);
			if (error == null)
			{
				error = Language.GetString("ui_disconnected_error");
			}
			D.LogError("all", $"{GetType()} OnConnectionClosed - Error: {error} - relog: {relog} - serverInstance: {serverInstanceID} - duelCharID: {duelCharID} - brawlIndex: {brawlIndex}");
			SingletonMonoBehaviour<SpriteManager>.instance.Restore();
			SingletonMonoBehaviour<PrefabManager>.instance.Restore();
			SingletonMonoBehaviour<AssetBundleManager>.instance.Restore();
			SingletonMonoBehaviour<AssetManager>.instance.Restore();
			GameData.instance.DISCONNECTED = true;
			GameData.instance.SERVER_INSTANCE_ID = serverInstanceID;
			GameData.instance.DUEL_CHAR_ID = duelCharID;
			GameData.instance.BRAWL_INDEX = brawlIndex;
			GameData.instance.RELOG = relog;
			GameData.instance.DISCONNECT_ERROR_MESSAGE = error;
			com.ultrabit.bitheroes.model.utility.Tween.ClearData();
			UnityEngine.Object.Destroy(_DISPATCHER);
			GameData.instance.Clear();
			XMLBook.instance.Clear();
			Benchmark.Clear();
			ReloadScene();
		}
	}

	private void StartInitDLC()
	{
		AddBreadcrumb("InitDLC");
		InitDLC();
	}

	private void InitXML()
	{
		D.Log("david", $"{GetType()} - InitXML Start");
		if (!(this == null) && base.isActiveAndEnabled)
		{
			UpdateLoading(Language.GetString("ui_getting_server_data"));
			Benchmark.Start("books_parsed");
			XMLBook.instance.OnProcessXMLComplete.AddListener(StartSliceInitBooks);
			XMLBook.instance.OnProcessXMLError.AddListener(OnProcessXMLErrorHandler);
			StartCoroutine(XMLBook.instance.LoadFromSFSObject(xmlSFSObject));
		}
	}

	private void OnProcessXMLErrorHandler()
	{
		HideLoading();
		D.LogError("XMLDLC Flow unable to load languages/XML from DLC [client ver: " + Application.version + "]");
		XMLBook.instance.OnProcessXMLError.RemoveListener(OnProcessXMLErrorHandler);
		string @string = Language.GetString("ui_dlc_download_error");
		ServerExtension.instance.Disconnect(@string);
	}

	private void StartSliceInitBooks()
	{
		AddBreadcrumb("StartSliceBooks");
		XMLBook.instance.OnProcessXMLComplete.RemoveListener(StartSliceInitBooks);
		StartCoroutine("SliceInitBooks");
	}

	public void UpdateSliceProgress(float progress)
	{
		UpdateLoading(Language.GetString("ui_loading_server_data") + " " + Math.Round(progress, 0) + "%");
	}

	private IEnumerator SliceInitBooks()
	{
		AddBreadcrumb("UpdateSliceProgress");
		yield return StartCoroutine(LanguageBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(MusicBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(SoundBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(BattleBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FilterBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(RarityBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(ProbabilityBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(AbilityBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(EquipmentBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(RuneBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(MaterialBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FishBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(BaitBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(BobberBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(CurrencyBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(ConsumableBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(ServiceBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(AugmentBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FamiliarBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(EnchantBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(MountBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FusionBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GuildBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GuildShopBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GuildHallBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(VariableBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(AdBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FishingBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(DailyRewardBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(DailyBonusBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(DailyQuestBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(CharacterAchievementBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(CraftBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(NPCBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(EncounterBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(DungeonBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(ZoneBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(ShopBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FishingShopBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(RewardBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(EventRewardBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(PvPEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(RiftEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GauntletEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GvGEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(GvEEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(InvasionEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(FishingEventBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(RaidBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(BrawlBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(InstanceBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(PaymentBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(LeaderboardBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(NewsBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(DialogBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(AchievementBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(PlayerVotingBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(BoosterBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(WalletBook.Init(UpdateSliceProgress));
		yield return StartCoroutine(EventSalesShopBook.Init(UpdateSliceProgress));
		AddBreadcrumb("InitXML:StartSliceBooks:Complete");
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("books_parsed", stop: true),
			["loading_type"] = "books_parsed"
		});
		StartCoroutine(SliceBooksFinalize());
	}

	private IEnumerator SliceBooksFinalize()
	{
		try
		{
			AddBreadcrumb("SliceBooksFinalize-Start");
			FamiliarBook.preloadFamiliarAssets();
			AddBreadcrumb("SliceBooksFinalize-InitIAP");
			AppInfo.InitIAP();
			XMLBook.instance.Clear();
			xmlSFSObject = null;
			Benchmark.Start("dlc_downloaded");
			_onXMLProcessCompleted?.Invoke();
			AddBreadcrumb("InitXML:SliceBooksFinalize:InitAds");
			AppInfo.InitAds();
		}
		catch (Exception ex)
		{
			D.LogError("all", "InitXML::SliceBooksFinalize " + ex.Message);
		}
		yield return null;
	}

	public void CreatePurchaseBehaviourCallback()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "PurchaseHelperBehaviour";
		paymentUtilityBehaviour = gameObject.AddComponent<PaymentUtilityBehaviour>();
	}

	private void InitDLC()
	{
		GameManager instance = SingletonMonoBehaviour<GameManager>.instance;
		instance.onDownloadComplete = (Action)Delegate.Combine(instance.onDownloadComplete, new Action(InitXML));
		GameManager instance2 = SingletonMonoBehaviour<GameManager>.instance;
		instance2.onProgressUpdated = (Action<float>)Delegate.Combine(instance2.onProgressUpdated, new Action<float>(OnDLCProgressUpdated));
		SingletonMonoBehaviour<GameManager>.instance.Init();
	}

	private void OnDLCProgressUpdated(float value)
	{
		UpdateLoading(Language.GetString("ui_getting_assets", new string[1] { Math.Round(value * 100f, 0).ToString() }));
	}

	public void ProcessLogin()
	{
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("dlc_downloaded", stop: true),
			["loading_type"] = "dlc_downloaded"
		});
		AddBreadcrumb("FinishLoginFlow");
		UpdateLoading(Language.GetString("ui_logging_entering_town"));
		GameManager instance = SingletonMonoBehaviour<GameManager>.instance;
		instance.onDownloadComplete = (Action)Delegate.Remove(instance.onDownloadComplete, new Action(InitXML));
		GameManager instance2 = SingletonMonoBehaviour<GameManager>.instance;
		instance2.onProgressUpdated = (Action<float>)Delegate.Remove(instance2.onProgressUpdated, new Action<float>(OnDLCProgressUpdated));
		StartCoroutine(EndLoginProcessAndActiveTown());
	}

	private IEnumerator EndLoginProcessAndActiveTown()
	{
		Benchmark.Start("town_enter");
		AddBreadcrumb("Parsing Character");
		Character character = Character.fromSFSObject(_characterLoginSFSObject);
		yield return null;
		GameData.instance.PROJECT.SetCharacter(character);
		if (GameData.instance.SAVE_STATE.characterID == 0 && character != null && character.tutorial.GetState(0))
		{
			character.tutorial.SetState(130);
		}
		if (!firstTimeInitSharedObjectsObtained)
		{
			logManager.SendLog($"Character {character.id} was unable to get original shared objects", "", LogType.Error);
		}
		crashlyticsManager.SetKeyValue("UserID", AppInfo.userID);
		crashlyticsManager.SetKeyValue("CharID", (character != null) ? character.id.ToString() : "-1");
		if (character != null)
		{
			crashlyticsManager.SetKeyValue("CharName", character.name ?? "");
		}
		yield return null;
		HideLoading();
		_PROJECT = GameData.instance.PROJECT;
		AppInfo.userID = _characterLoginSFSObject.GetUtfString("use3");
		AppInfo.playerID = (_characterLoginSFSObject.ContainsKey("pla3") ? _characterLoginSFSObject.GetInt("pla3") : (-1));
		bool flag = _characterLoginSFSObject.ContainsKey("pla13") && _characterLoginSFSObject.GetBool("pla13");
		if (GameData.instance.PROJECT.IsNFTInFrozenCache(character))
		{
			character.FreezeNFT();
		}
		if (AppInfo.playerID == -1 || flag || character == null || character.nftState == Character.NFTState.bitverseHeroFrozen)
		{
			GameData.instance.windowGenerator.NewHeroSelectWindow(null, showCloseBtn: false, forceRelog: true);
		}
		else
		{
			ContinueToTown();
		}
		_characterLoginSFSObject = null;
	}

	public void ContinueToTown()
	{
		GameData.instance.PROJECT.ContinueSetCharacter();
		AddBreadcrumb("EnteringTown");
		GoToTown();
	}

	private void OnCharacterChecked()
	{
		ContinueToTown();
	}

	private void DoCancelLogin()
	{
		if (platformLogin != null)
		{
			AddBreadcrumb("Specific Platform Login Canceled due to Timeout");
			platformLogin.CancelLogin();
		}
	}

	private void DoPlatformLogin()
	{
		AddBreadcrumb("DoPlatformLogin");
		platformLogin = PlatformLoginHandler.instance.GetPlatformLogin();
		if (platformLogin != null)
		{
			_cancelLogin = coroutineTimer.AddTimer(base.gameObject, 30f, CoroutineTimer.TYPE.SECONDS, DoCancelLogin);
			platformLogin.Login(delegate
			{
				D.Log("onLoginCompleted");
				AddBreadcrumb("Specific Platform Login Complete");
				coroutineTimer.StopTimer(ref _cancelLogin);
				DoLoginAttempt();
			}, delegate(float delay)
			{
				D.Log("onLoginFailed");
				AddBreadcrumb("Specific Platform Login Failed");
				coroutineTimer.StopTimer(ref _cancelLogin);
				StartCoroutine(OnGettingLoginInfoFailed(delay));
			});
		}
		else
		{
			HideLoading();
			GameData.instance.logInManager.SetMain = this;
			GameData.instance.logInManager.CreateLogin();
		}
	}

	private IEnumerator OnGettingLoginInfoFailed(float delay)
	{
		yield return new WaitForSeconds(delay);
		HideLoading();
		if (!AppInfo.live)
		{
			GameData.instance.logInManager.SetMain = this;
			GameData.instance.logInManager.CreateLogin();
		}
		else
		{
			string desc = (string.IsNullOrEmpty(platformLogin.errorMessage) ? Language.GetString("ui_logging_in_error") : platformLogin.errorMessage);
			GameData.instance.windowGenerator.ShowError(desc);
			platformLogin.errorMessage = null;
		}
	}

	private void DoLoginAttempt()
	{
		AppInfo.setUserName(platformLogin.GetUserName());
		AppInfo.userID = platformLogin.GetUserID();
		GameData.instance.main.UpdateLoading(Language.GetString("ui_logging_in"));
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnLoginPlatform);
		PlayerDALC.instance.doLoginPlatform();
	}

	private void OnLoginPlatform(BaseEvent e)
	{
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnLoginPlatform);
		SFSObject sfsob = (e as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("err0");
			D.LogError("all", $"[ServerConnectivityIssue] OnPlatformLoginError - Code: {@int}");
			ServerExtension.instance.Disconnect(ErrorCode.getErrorMessage(@int));
		}
		else
		{
			OnRemoteLoginSuccess(sfsob);
		}
	}

	private void OnRemoteLoginSuccess(SFSObject sfsObject)
	{
		AddBreadcrumb("OnRemoteLoginSuccess");
		AddBreadcrumb("Login Success - Character Obtained");
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("logged_in", stop: true),
			["loading_type"] = "logged_in"
		});
		_characterLoginSFSObject = sfsObject;
		_onXMLProcessCompleted = ProcessLogin;
		DoLoadXMLs();
	}

	private void DoLoadXMLs()
	{
		Benchmark.Start("books_downloaded");
		AddBreadcrumb("DoLoadXMLs:Start");
		UpdateLoading(Language.GetString("ui_loading_server_data"));
		UserDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnLoadXMLs);
		UserDALC.instance.doLoadXMLs();
	}

	private void OnLoadXMLs(BaseEvent e)
	{
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("books_downloaded", stop: true),
			["loading_type"] = "books_downloaded"
		});
		DALCEvent obj = e as DALCEvent;
		UserDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnLoadXMLs);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			AddBreadcrumb(string.Format("LoadXMLError-{0}", sfsob.GetInt("err0")));
			D.LogError("all", "InitFlow - Unable to download game XML data");
		}
		else
		{
			_BACKGROUND.StartFamiliarsParade();
			xmlSFSObject = sfsob;
			StartInitDLC();
		}
	}

	public void ReCheckAdsAvailability()
	{
		if (_adReCheckTimer != null)
		{
			GameData.instance.main.AddBreadcrumb("CheckAdsAvailability");
			_adReCheckTimer = coroutineTimer.AddTimer(base.gameObject, 10f, CoroutineTimer.TYPE.SECONDS, OnTimerCheckAdsAvailability);
		}
	}

	private void OnTimerCheckAdsAvailability()
	{
		GameData.instance.main.AddBreadcrumb("CheckAdsAvailability Timer");
		_adReCheckTimer = null;
		AppInfo.CheckIronSourceAvailable();
	}

	public void DoLoginEmail(string email, string password)
	{
		GameData.instance.SAVE_STATE.email = email;
		GameData.instance.SAVE_STATE.password = password;
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnLoginEmail);
		PlayerDALC.instance.doLoginEmail(email, ServerExtension.instance.GenerateHash(password));
	}

	private void OnLoginEmail(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnLoginEmail);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			GameData.instance.logInManager.OnLoginError();
			GameData.instance.SAVE_STATE.password = "";
		}
		else
		{
			GameData.instance.logInManager.CloseLogin(toNextScene: true);
			OnRemoteLoginSuccess(sfsob);
		}
	}

	public void DoCreateEmailUser(string email, string password)
	{
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnCreateEmailUser);
		PlayerDALC.doCreateEmail(email, password);
	}

	public void OnCreateEmailUser(BaseEvent baseEvent)
	{
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnCreateEmailUser);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			PersistentPlayerData sAVE_STATE = GameData.instance.SAVE_STATE;
			string email = (GameData.instance.SAVE_STATE.password = "");
			sAVE_STATE.email = email;
		}
		else
		{
			string email2 = GameData.instance.SAVE_STATE.email;
			AppInfo.setUserName(email2.Substring(0, email2.IndexOf('@')));
			GameData.instance.logInManager.CloseUserCreation();
			OnRemoteLoginSuccess(sfsob);
		}
	}

	public void GoToTown()
	{
		if (background != null && background.gameObject != null)
		{
			background.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				UnityEngine.Object.Destroy(version);
				UnityEngine.Object.Destroy(background);
			});
		}
		AppInfo.doKongregateAnalyticsEvent("loading_ends", new Dictionary<string, object>
		{
			["load_time_ms"] = Benchmark.Elapsed("town_enter", stop: true),
			["loading_type"] = "town_enter"
		});
		Invoke("CheckArmory", 5f);
	}

	private void CheckArmory()
	{
		GameData.instance.PROJECT.character.armory.CheckArmoryEquipmentSlotState(GameData.instance.PROJECT.character.inventory);
	}

	public void DestroyBackground()
	{
		if (background != null)
		{
			UnityEngine.Object.Destroy(background);
		}
	}

	public void InitCurrencies()
	{
		if (currencies != null)
		{
			CurrencyBarFill[] array = currencies;
			foreach (CurrencyBarFill currencyBarFill in array)
			{
				if (currencyBarFill != null)
				{
					currencyBarFill.Init();
				}
			}
		}
		GameData.instance.windowGenerator.ShowCurrencies(show: true);
	}

	public void AddInstanceInterface()
	{
		Transform obj = UnityEngine.Object.Instantiate(instanceInterface);
		obj.SetParent(GameData.instance.windowGenerator.canvas.transform, worldPositionStays: false);
		obj.GetComponent<InstanceInterface>().LoadDetails(null);
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene("Main");
	}

	public Scene GetFirstInactiveScene()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetActiveScene().buildIndex != SceneManager.GetSceneAt(i).buildIndex)
			{
				return SceneManager.GetSceneAt(i);
			}
		}
		return SceneManager.GetSceneByName("Login");
	}

	private void UpdateBounds()
	{
		float num = Mathf.Max(Screen.width, Screen.height);
		float num2 = Mathf.Min(Screen.width, Screen.height);
		float num3 = num / DEFAULT_BOUNDS.width;
		float num4 = num2 / DEFAULT_BOUNDS.height;
		float num5 = 1f;
		float num6 = 1f;
		if (num3 > num4)
		{
			num5 = num3 / num4;
			_STAGE_SCALE = num4;
			_SCREEN_SCALE = num5;
		}
		else if (num4 > num3)
		{
			num6 = num4 / num3;
			_STAGE_SCALE = num3;
			_SCREEN_SCALE = num6;
		}
		else
		{
			num5 = num3 / num4;
			num6 = num4 / num3;
			_STAGE_SCALE = num3;
			_SCREEN_SCALE = num5;
		}
		_BOUNDS = new Rect(0f, 0f, DEFAULT_BOUNDS.width * num5, DEFAULT_BOUNDS.height * num6);
		_CENTER = new Vector2(_BOUNDS.width / 2f, _BOUNDS.height / 2f);
		_WIDTH_SCALE = BOUNDS.width / DEFAULT_BOUNDS.width;
		_HEIGHT_SCALE = BOUNDS.height / DEFAULT_BOUNDS.height;
		_BORDER_SCALE = ((_WIDTH_SCALE > _HEIGHT_SCALE) ? _HEIGHT_SCALE : _WIDTH_SCALE);
	}

	public bool DoBack()
	{
		object screenFocus = GetScreenFocus();
		if (screenFocus == null)
		{
			return false;
		}
		try
		{
			bool result = false;
			if (screenFocus is WindowsMain)
			{
				result = (screenFocus as WindowsMain).DoBack();
			}
			if (screenFocus is Battle)
			{
				result = (screenFocus as Battle).DoBack();
			}
			if (screenFocus is Dungeon)
			{
				result = (screenFocus as Dungeon).DoBack();
			}
			if (screenFocus is Instance)
			{
				result = (screenFocus as Instance).DoBack();
			}
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private bool DoForward()
	{
		object screenFocus = GetScreenFocus();
		if (screenFocus == null)
		{
			return false;
		}
		try
		{
			bool result = false;
			if (screenFocus is WindowsMain)
			{
				result = (screenFocus as WindowsMain).DoForward();
			}
			if (screenFocus is DialogWindow)
			{
				result = (screenFocus as DialogWindow).DoForward();
			}
			if (screenFocus is Battle)
			{
				result = (screenFocus as Battle).DoForward();
			}
			if (screenFocus is Dungeon)
			{
				result = (screenFocus as Dungeon).DoForward();
			}
			if (screenFocus is Instance)
			{
				result = (screenFocus as Instance).DoForward();
			}
			return result;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static object GetScreenFocus()
	{
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(TransitionScreen)) || GameData.instance.tutorialManager.hasPopup)
		{
			return null;
		}
		if (GameData.instance.windowGenerator.hasPopup)
		{
			WindowsMain lastDialog = GameData.instance.windowGenerator.GetLastDialog();
			if (lastDialog != null && lastDialog.GetComponent<DungeonUI>() == null && lastDialog.GetComponent<BattleUI>() == null)
			{
				return lastDialog;
			}
		}
		if (GameData.instance.windowGenerator.chatVisible)
		{
			return GameData.instance.windowGenerator.GetDialogByClass(typeof(ChatWindow)) as ChatWindow;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			return GameData.instance.PROJECT.battle;
		}
		if (GameData.instance.PROJECT.dungeon != null)
		{
			return GameData.instance.PROJECT.dungeon;
		}
		if (GameData.instance.PROJECT.instance != null)
		{
			return GameData.instance.PROJECT.instance;
		}
		return null;
	}

	public override void Update()
	{
		base.Update();
		bool flag = GameData.instance != null && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.admin;
		if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Backspace)) && (AppInfo.allowKeycodes || AppInfo.IsMobile()) && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
		{
			DoBack();
		}
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && AppInfo.allowKeycodes && (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
		{
			DoForward();
		}
		if (Input.GetKeyUp(KeyCode.F6) && (flag || !AppInfo.live))
		{
			GameData.instance.SAVE_STATE.logosDisabled = !GameData.instance.SAVE_STATE.logosDisabled;
			if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.notification != null)
			{
				string data = "Game Logos " + ((!GameData.instance.SAVE_STATE.logosDisabled) ? "Enabled" : "Disabled");
				GameData.instance.PROJECT.notification.AddToQueue(0, "", 0, data);
			}
		}
		if (Input.GetKeyUp(KeyCode.F7) && (flag || !AppInfo.live))
		{
			ToggleTesting();
		}
		if (Input.GetKeyUp(KeyCode.F8) && (flag || !AppInfo.live))
		{
			if (GameData.instance.windowGenerator.HasDialogByClass(typeof(ZoneWindow)))
			{
				GameData.instance.windowGenerator.GetDialogByClass(typeof(ZoneWindow));
			}
			if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.dungeon != null)
			{
				GameData.instance.PROJECT.dungeon.ReloadAssets();
			}
			if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.instance != null)
			{
				GameData.instance.PROJECT.instance.reloadAssets();
			}
		}
		Input.GetKeyUp(KeyCode.F9);
		if (Input.GetKeyUp(KeyCode.F10) && GameData.instance.PROJECT != null && GameData.instance.PROJECT.instance != null && flag)
		{
			GameData.instance.PROJECT.instance.PrintData();
		}
		if (Input.GetKeyUp(KeyCode.F11))
		{
			GameData.instance.windowGenerator.ShowAdminTools();
		}
		if (!Input.GetKeyUp(KeyCode.F1) || AppInfo.live)
		{
			if (Input.GetKeyUp(KeyCode.F2))
			{
				_ = AppInfo.live;
			}
			if (Input.GetKeyUp(KeyCode.F3) && !AppInfo.live)
			{
				DoTestDialog();
			}
			if (Input.GetKeyUp(KeyCode.F12) && !AppInfo.live && flag)
			{
				GameData.instance.SAVE_STATE.ClearDialogsSeen(GameData.instance.PROJECT.character.id);
			}
		}
	}

	public static void ToggleTesting()
	{
		AppInfo.TESTING = !AppInfo.TESTING;
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.notification != null)
		{
			string data = "Testing " + (AppInfo.TESTING ? "Enabled" : "Disabled");
			GameData.instance.PROJECT.notification.AddToQueue(0, "", 0, data);
		}
	}

	private void DoTestDialog()
	{
		GameData.instance.windowGenerator.NewDialogPopup(DialogBook.Lookup("z2_n8_boss"));
	}

	public void ShowBackground()
	{
		if (!(_BACKGROUND != null))
		{
			_BACKGROUND = GameData.instance.windowGenerator.AddGameBackground();
			UpdateBackground();
		}
	}

	public void HideBackground()
	{
		if (!(_BACKGROUND == null))
		{
			_BACKGROUND.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(_BACKGROUND.gameObject);
			_BACKGROUND = null;
		}
	}

	private void UpdateBackground()
	{
		if (!(_BACKGROUND == null))
		{
			_BACKGROUND.transform.SetParent(GameData.instance.windowGenerator.canvas.transform);
			((RectTransform)_BACKGROUND.transform).anchoredPosition = Vector2.zero;
			((RectTransform)_BACKGROUND.transform).sizeDelta = Vector2.zero;
			((RectTransform)_BACKGROUND.transform).localScale = Vector3.one;
		}
	}

	public void ShowKongregateLogo()
	{
		if (!(_KONGREGATE_LOGO != null))
		{
			_KONGREGATE_LOGO = GameData.instance.windowGenerator.AddKongregateLogo();
			_KONGREGATE_LOGO.COMPLETE.AddListener(OnKongregateLogoComplete);
			UpdateLogos();
		}
	}

	public void HideKongregateLogo()
	{
		if (!(_KONGREGATE_LOGO == null))
		{
			_KONGREGATE_LOGO.COMPLETE.RemoveListener(OnKongregateLogoComplete);
			UnityEngine.Object.Destroy(_KONGREGATE_LOGO.gameObject);
			_KONGREGATE_LOGO = null;
		}
	}

	private void OnKongregateLogoComplete()
	{
		HideKongregateLogo();
		ShowBackground();
		ShowGameLogo();
	}

	public void ShowGameLogo()
	{
		if (!(_GAME_LOGO != null))
		{
			_GAME_LOGO = GameData.instance.windowGenerator.AddGameLogo2022();
			_GAME_LOGO.COMPLETE.AddListener(OnGameLogoComplete);
			UpdateLogos();
		}
	}

	public void HideGameLogo()
	{
		if (!(_GAME_LOGO == null))
		{
			_GAME_LOGO.COMPLETE.RemoveListener(OnGameLogoComplete);
			UnityEngine.Object.Destroy(_GAME_LOGO.gameObject);
			_GAME_LOGO = null;
		}
	}

	private void OnGameLogoComplete()
	{
		HideGameLogo();
		DoResizeTimer();
	}

	private void DoResizeTimer()
	{
		GameData.instance.DISPLAY_LOGOS = false;
		DoLoadServers();
	}

	private IEnumerator DoConnectInternet()
	{
		float pingInterval = 1f;
		int iterations = 5;
		ShowLoading(Language.GetString("ui_checking_connection"));
		bool online = false;
		while (iterations > 0)
		{
			UnityWebRequest internetRequest = new UnityWebRequest("https://www.google.com/");
			yield return internetRequest.SendWebRequest();
			online = !internetRequest.isHttpError && !internetRequest.isNetworkError;
			if (online)
			{
				break;
			}
			iterations--;
			yield return new WaitForSeconds(pingInterval);
		}
		if (!online)
		{
			HideLoading();
			ServerExtension.instance.Disconnect(Language.GetString("ui_checking_connection_error"));
			yield return null;
		}
		else if (AppInfo.IsMobile())
		{
			StartCoroutine(DoCheckMobileVersion());
		}
		else
		{
			StartCoroutine(InitLibraries());
		}
	}

	private IEnumerator DoCheckMobileVersion()
	{
		UpdateLoading(Language.GetString("ui_checking_app_version"));
		yield return null;
		UnityWebRequest www = UnityWebRequest.Get(AppInfo.GetVersionURL());
		yield return www.SendWebRequest();
		HideLoading();
		if (www.isNetworkError || www.isHttpError)
		{
			ServerExtension.instance.Disconnect(Language.GetString("ui_checking_app_version_error"));
			yield return null;
			yield break;
		}
		string text = www.downloadHandler.text;
		D.Log(text);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNodeList childNodes = xmlDocument.SelectSingleNode("data").ChildNodes;
		bool versionMatch = false;
		int count = childNodes.Count;
		for (int i = 0; i < count; i++)
		{
			XmlAttributeCollection attributes = childNodes.Item(i).Attributes;
			string value = attributes.GetNamedItem("id").Value;
			foreach (int platformType in AppInfo.getPlatformTypes(attributes.GetNamedItem("platforms").Value))
			{
				if (platformType == AppInfo.platform && value == AppInfo.version)
				{
					StartCoroutine(InitLibraries());
					versionMatch = true;
					yield break;
				}
			}
		}
		yield return null;
		if (!versionMatch)
		{
			AddBreadcrumb("Force Update Required");
			ShowBackground();
			GameData.instance.windowGenerator.ShowErrorCode(24);
		}
	}

	private void DoLoadServers()
	{
		AddBreadcrumb("DoLoadServers");
		ShowLoading(Language.GetString("ui_getting_server_data"));
		StartCoroutine(GetServerInformation());
	}

	private IEnumerator GetServerInformation()
	{
		Benchmark.Start("server_connect");
		string serversURL = AppInfo.GetServersURL();
		if (serversURL == null || serversURL.Equals(""))
		{
			DoLoadSmartfox(null);
			yield return null;
			yield break;
		}
		UnityWebRequest www = UnityWebRequest.Get(AppInfo.GetServersURL());
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			HideLoading();
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_checking_swf_version_maintenance", new string[1] { Language.GetString("game_name") }));
			yield return null;
			yield break;
		}
		string text = www.downloadHandler.text;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XMLBook.instance.GetObjectFromXMLDocument(ref XMLBook.instance.serverBookData, xmlDocument);
		ServerBook.Init();
		if (ServerBook.size() <= 0)
		{
			HideLoading();
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_checking_swf_version_maintenance", new string[1] { Language.GetString("game_name") }));
			yield return null;
		}
		else
		{
			DoLoadSmartfox(ServerBook.GetFirstServer());
		}
	}

	private IEnumerator ConnectWithRemoteConfig(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_checking_swf_version_maintenance"));
			yield return null;
			yield break;
		}
		string text = www.downloadHandler.text;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		XmlNode firstChild = xmlDocument.FirstChild;
		string innerText = firstChild.SelectSingleNode("ip").InnerText;
		string innerText2 = firstChild.SelectSingleNode("port").InnerText;
		string innerText3 = firstChild.SelectSingleNode("zone").InnerText;
		if (firstChild.SelectSingleNode("httpPort") != null)
		{
			_ = firstChild.SelectSingleNode("httpPort").InnerText;
		}
		ConfigData configData = new ConfigData();
		configData.Host = innerText;
		configData.Port = int.Parse(innerText2);
		configData.Zone = innerText3;
		DoConnectSmartFox(configData);
	}

	private void DoLoadSmartfox(ServerRef serverRef, bool selected = false)
	{
		_SERVER_SELECTED = selected;
		SELECTED_SERVER = serverRef;
		if (serverRef == null)
		{
			ConfigData configData = null;
			if (!AppInfo.live)
			{
				configData = new ConfigData();
				configData.Host = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("debug")["smartfox"]["host"];
				configData.Port = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("debug")["smartfox"]["port"].AsInt;
				configData.Zone = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("debug")["smartfox"]["zone"];
			}
			DoConnectSmartFox(configData);
			return;
		}
		string url = serverRef.url;
		if (GameData.instance.SERVER_INSTANCE_ID != null)
		{
			ServerInstanceRef instance = serverRef.GetInstance(GameData.instance.SERVER_INSTANCE_ID);
			if (instance != null)
			{
				url = instance.url;
			}
			GameData.instance.SERVER_INSTANCE_ID = null;
		}
		GameData.instance.SAVE_STATE.serverID = serverRef.id;
		StartCoroutine(ConnectWithRemoteConfig(url));
	}

	private void UpdateLogos()
	{
		if (_GAME_LOGO != null)
		{
			_GAME_LOGO.transform.position = new Vector3(_uiCamera.transform.position.x, _uiCamera.transform.position.y, _uiCamera.transform.position.z + _uiCamera.nearClipPlane);
			float x = GameData.instance.windowGenerator.canvas.transform.localScale.x;
			Util.ChangeLayer(_GAME_LOGO.transform, "UI");
			_GAME_LOGO.transform.localScale = new Vector3(x * 2f, x * 2f, 1f);
		}
	}

	public void Logout(bool relog, bool reloadXMLfiles)
	{
		ShowLoading(Language.GetString("ui_logging_out"));
		GameData.instance.RELOG = relog;
		GameData.instance.RELOAD_SERVER_XML_FILES = reloadXMLfiles;
		if (!GameData.instance.RELOG)
		{
			GameData.instance.SAVE_STATE.password = "";
			GameData.instance.SAVE_STATE.characterID = 0;
			GameData.instance.SAVE_STATE.playerID = -1;
		}
		UserDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnLogout);
		UserDALC.instance.doLogout();
	}

	public void OnLogout(BaseEvent baseEvent)
	{
		UserDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnLogout);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("err0");
			if (@int == 8)
			{
				ReloadScene();
			}
			else
			{
				GameData.instance.windowGenerator.ShowErrorCode(@int);
			}
		}
		else
		{
			ReloadScene();
		}
	}

	public void EnableSteamManager(bool enable)
	{
		steamManager.SetActive(enable);
	}

	public static void ClearDuelCharID()
	{
		GameData.instance.DUEL_CHAR_ID = 0;
	}

	public static void ClearBrawlIndex()
	{
		GameData.instance.BRAWL_INDEX = 0;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (GameData.instance != null && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			ServerExtension.instance.startPreventIdleTimer(doStartIdleTimer: true);
			GameDALC.instance.doIdleResponse();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (GameData.instance != null && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			ServerExtension.instance.startPreventIdleTimer(doStartIdleTimer: true);
			GameDALC.instance.doIdleResponse();
		}
	}

	public void AddBreadcrumb(string step)
	{
		if (logManager != null)
		{
			logManager.AddBreadcrumb($"{GetType()}::{step}");
		}
		if (crashlyticsManager != null)
		{
			crashlyticsManager.SetKeyValue("InitFlowStep", $"{GetType()}::{step}");
		}
	}

	protected override void OnDestroy()
	{
		CancelInvoke("CheckArmory");
		base.OnDestroy();
	}
}
