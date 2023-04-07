using System;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using UnityEngine;

namespace com.ultrabit.bitheroes.extensions;

public class ServerExtension : BaseExtension
{
	public const int GAME_DALC = 0;

	public const int CHARACTER_DALC = 1;

	public const int ADMIN_DALC = 2;

	public const int FRIEND_DALC = 3;

	public const int USER_DALC = 4;

	public const int PVP_DALC = 5;

	public const int LEADERBOARD_DALC = 6;

	public const int MERCHANT_DALC = 7;

	public const int CHAT_DALC = 8;

	public const int GUILD_DALC = 9;

	public const int BATTLE_DALC = 10;

	public const int RIFT_DALC = 11;

	public const int GAUNTLET_DALC = 12;

	public const int GVG_DALC = 13;

	public const int INVASION_DALC = 14;

	public const int BRAWL_DALC = 15;

	public const int FISHING_DALC = 16;

	public const int GVE_DALC = 17;

	public const int PLAYER_VOTING_DALC = 18;

	public const int EVENT_SALES_DALC = 19;

	public const int PLAYER_DALC = 21;

	private double _DAILY_TIME;

	private float _DAILY_THRESHOLD = 600000f;

	private float _TIMEOUT_THRESHOLD = 30000f;

	private Coroutine _DAILY_TIMER;

	private Coroutine _TIMEOUT_TIMER;

	private Coroutine _PREVENT_IDLE_TIMER;

	private Coroutine _IDLE_TIMER;

	private Coroutine _IDLE_DISCONNECT_TIMER;

	private float _NOTIFY_THRESHOLD = 2000f;

	private Coroutine _IDLE_NOTIFY_TIMER;

	private static ServerExtension _instance;

	private float _time;

	private DateTime _date;

	private string _serverInstanceID;

	private EventDispatcher eventDispatcher;

	public float startIdleTimerTime;

	public static ServerExtension instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ServerExtension();
			}
			return _instance;
		}
	}

	public SmartFox smartfox { get; private set; }

	public string serverInstanceID => _serverInstanceID;

	public void Init()
	{
		eventDispatcher = new EventDispatcher();
		CreateSmartFox();
		GameDALC.instance.Init(0);
		CharacterDALC.instance.Init(1);
		AdminDALC.instance.Init(2);
		FriendDALC.instance.Init(3);
		UserDALC.instance.Init(4);
		PlayerDALC.instance.Init(21);
		PvPDALC.instance.Init(5);
		LeaderboardDALC.instance.Init(6);
		MerchantDALC.instance.Init(7);
		ChatDALC.instance.Init(8);
		GuildDALC.instance.Init(9);
		BattleDALC.instance.Init(10);
		RiftDALC.instance.Init(11);
		GauntletDALC.instance.Init(12);
		GvGDALC.instance.Init(13);
		InvasionDALC.instance.Init(14);
		BrawlDALC.instance.Init(15);
		FishingDALC.instance.Init(16);
		GvEDALC.instance.Init(17);
		PlayerVotingDALC.instance.Init(18);
		EventSalesDALC.instance.Init(19);
	}

	public void ReconnectToSmartFox()
	{
		ClearSmartFox();
		CreateSmartFox();
	}

	private void CreateSmartFox()
	{
		ClearSmartFox();
		smartfox = new SmartFox();
		smartfox.ThreadSafeMode = true;
		smartfox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		smartfox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
	}

	public void ClearSmartFox()
	{
		if (smartfox != null)
		{
			smartfox.Disconnect();
			smartfox.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
			smartfox.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
			smartfox.RemoveAllEventListeners();
			smartfox = null;
		}
	}

	public void setServerInstanceID(string serverID)
	{
		_serverInstanceID = serverID;
	}

	public void setDate(DateTime date)
	{
		_date = date;
		_time = Time.realtimeSinceStartup;
		startDailyTimer();
	}

	private void startDailyTimer()
	{
		_DAILY_TIME = GetMillisecondsTillDayEnds();
		stopDailyTimer();
		_DAILY_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, _DAILY_THRESHOLD, CoroutineTimer.TYPE.MILLISECONDS, 0, onDailyTimer);
	}

	private void stopDailyTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _DAILY_TIMER);
	}

	private void onDailyTimer()
	{
		double num = GetMillisecondsTillDayEnds();
		if (num > _DAILY_TIME && GameData.instance.PROJECT != null)
		{
			GameData.instance.PROJECT.doDayChange();
		}
		_DAILY_TIME = num;
	}

	public void startTimeoutTimer()
	{
		stopTimeoutTimer();
		_TIMEOUT_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, _TIMEOUT_THRESHOLD, 1, onTimeoutTimer);
	}

	public void stopTimeoutTimer()
	{
		if (GameData.instance.main != null && GameData.instance.main.coroutineTimer != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _TIMEOUT_TIMER);
		}
	}

	private void onTimeoutTimer()
	{
		Disconnect();
	}

	public void startPreventIdleTimer(bool doStartIdleTimer)
	{
		stopPreventIdleTimer();
		_PREVENT_IDLE_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, 120f, CoroutineTimer.TYPE.SECONDS, 1, OnPreventIdleTimerComplete);
		if (doStartIdleTimer)
		{
			startIdleTimer();
		}
	}

	private void stopPreventIdleTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _PREVENT_IDLE_TIMER);
	}

	private void OnPreventIdleTimerComplete()
	{
		GameDALC.instance.doIdleResponse();
		startPreventIdleTimer(doStartIdleTimer: false);
	}

	public void startIdleTimer()
	{
		stopIdleNotifyTimer();
		stopIdleTimer();
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			float delay = VariableBook.serverIdleMilliseconds;
			_IDLE_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, delay, 1, onIdleTimer);
			startIdleTimerTime = Time.realtimeSinceStartup;
			startIdleNotifyTimer();
		}
	}

	public void stopIdleTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _IDLE_TIMER);
	}

	private void onIdleTimer()
	{
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			if (GameData.instance.PROJECT.character.admin || GameData.instance.PROJECT.character.moderator || GameData.instance.windowGenerator.HasDialogByClass(typeof(BattleCaptureWindow)))
			{
				GameDALC.instance.doIdleResponse();
				return;
			}
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("server_idle_message"), Language.GetString("ui_yes"), onIdleConfirm);
			startIdleDisconnectTimer();
		}
	}

	private void onIdleConfirm()
	{
		GameDALC.instance.doIdleResponse();
		stopIdleDisconnectTimer();
	}

	public void startIdleDisconnectTimer()
	{
		stopIdleDisconnectTimer();
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			_IDLE_DISCONNECT_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, VariableBook.serverIdleDisconnectMilliseconds, 1, onIdleDisconnectTimer);
		}
	}

	public void stopIdleDisconnectTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _IDLE_DISCONNECT_TIMER);
	}

	private void onIdleDisconnectTimer()
	{
		Disconnect();
	}

	private void stopIdleNotifyTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _IDLE_NOTIFY_TIMER);
	}

	private void startIdleNotifyTimer()
	{
		_IDLE_NOTIFY_TIMER = GameData.instance.main.coroutineTimer.AddTimer(null, _NOTIFY_THRESHOLD, 1, onIdleNotifyTimer);
		if (GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null && GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID() <= 0)
		{
			MenuInterfaceQuestTile menuInterfaceQuestTile = GameData.instance.PROJECT.instance.instanceInterface.GetButton(typeof(MenuInterfaceQuestTile)) as MenuInterfaceQuestTile;
			if (menuInterfaceQuestTile != null && menuInterfaceQuestTile.available)
			{
				menuInterfaceQuestTile.SetNotify(active: false);
			}
		}
	}

	private void onIdleNotifyTimer()
	{
		if (GameData.instance.PROJECT == null || GameData.instance.PROJECT.character == null)
		{
			return;
		}
		stopIdleNotifyTimer();
		if (GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null && GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID() <= 0)
		{
			MenuInterfaceQuestTile menuInterfaceQuestTile = GameData.instance.PROJECT.instance.instanceInterface.GetButton(typeof(MenuInterfaceQuestTile)) as MenuInterfaceQuestTile;
			if (menuInterfaceQuestTile != null && menuInterfaceQuestTile.available)
			{
				menuInterfaceQuestTile.SetNotify(active: true);
			}
		}
	}

	public void Update()
	{
		if (smartfox != null)
		{
			smartfox.ProcessEvents();
		}
	}

	public void Connect(ConfigData config)
	{
		D.Log($"Connecting with Configuration: {config.Host} - {config.Port} - {config.Zone}");
		smartfox.Connect(config);
	}

	public void ChangeServerInstance(string instanceId)
	{
		Disconnect(null, null, relog: true, instanceId);
	}

	public void Disconnect(string error = null, string closeText = null, bool relog = false, string serverInstanceID = null, int duelCharID = 0, int brawlIndex = 0)
	{
		D.Log("Calling Disconnect in ServerExtension - " + error);
		stopPreventIdleTimer();
		stopIdleDisconnectTimer();
		stopDailyTimer();
		stopIdleTimer();
		stopTimeoutTimer();
		GameData.instance.main.OnConnectionClosed(null, error, relog, serverInstanceID, duelCharID, brawlIndex);
		ClearSmartFox();
	}

	public void GuestLogin()
	{
		startPreventIdleTimer(doStartIdleTimer: true);
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("use8", AppInfo.GetLanguage());
		smartfox.Send(new LoginRequest("", "", ServerZones.SERVER, sFSObject));
	}

	private void OnExtensionResponse(BaseEvent e)
	{
		SFSObject sFSObject = e.Params["params"] as SFSObject;
		if (!((string)e.Params["cmd"] != "ServerExtension") && sFSObject.ContainsKey("dal0"))
		{
			stopTimeoutTimer();
			switch (sFSObject.GetInt("dal0"))
			{
			case 0:
				GameDALC.instance.parse(sFSObject);
				break;
			case 1:
				CharacterDALC.instance.parse(sFSObject);
				break;
			case 2:
				AdminDALC.instance.parse(sFSObject);
				break;
			case 3:
				FriendDALC.instance.parse(sFSObject);
				break;
			case 4:
				UserDALC.instance.parse(sFSObject);
				break;
			case 21:
				PlayerDALC.instance.parse(sFSObject);
				break;
			case 5:
				PvPDALC.instance.parse(sFSObject);
				break;
			case 6:
				LeaderboardDALC.instance.parse(sFSObject);
				break;
			case 7:
				MerchantDALC.instance.parse(sFSObject);
				break;
			case 8:
				ChatDALC.instance.parse(sFSObject);
				break;
			case 9:
				GuildDALC.instance.parse(sFSObject);
				break;
			case 10:
				BattleDALC.instance.parse(sFSObject);
				break;
			case 11:
				RiftDALC.instance.parse(sFSObject);
				break;
			case 12:
				GauntletDALC.instance.parse(sFSObject);
				break;
			case 13:
				GvGDALC.instance.parse(sFSObject);
				break;
			case 14:
				InvasionDALC.instance.parse(sFSObject);
				break;
			case 15:
				BrawlDALC.instance.parse(sFSObject);
				break;
			case 16:
				FishingDALC.instance.parse(sFSObject);
				break;
			case 17:
				GvEDALC.instance.parse(sFSObject);
				break;
			case 18:
				PlayerVotingDALC.instance.parse(sFSObject);
				break;
			case 19:
				EventSalesDALC.instance.parse(sFSObject);
				break;
			case 20:
				break;
			}
		}
	}

	private void OnConnectionLost(BaseEvent e)
	{
		D.Log("Connection Lost");
		DispatchEvent(new CustomSFSXEvent(CustomSFSXEvent.CLOSE));
	}

	public void Send(SFSObject sfsob, Room room = null, string extension = null, bool idleTimer = true)
	{
		if (extension == null)
		{
			extension = "ServerExtension";
		}
		if (idleTimer)
		{
			bool doStartIdleTimer = sfsob.ContainsKey("act0") && sfsob.GetInt("act0") != 12;
			startPreventIdleTimer(doStartIdleTimer);
		}
		smartfox.Send(new ExtensionRequest(extension, sfsob, room));
	}

	public string GenerateHash(string str)
	{
		str += "k5iw3la0";
		return Util.MD5(str);
	}

	public long GetMillisecondsTillDayEnds()
	{
		DateTime date = GetDate();
		return (long)(date.Date.AddDays(1.0).Subtract(date).TotalSeconds * 1000.0);
	}

	public DateTime GetDate()
	{
		_ = _date;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		_date = _date.AddSeconds(realtimeSinceStartup - _time);
		_time = realtimeSinceStartup;
		return _date;
	}

	public void LoadConfig(string path)
	{
		smartfox.LoadConfig(path);
	}

	public Room GetRoom(int roomID)
	{
		return smartfox.GetRoomById(roomID);
	}

	public void AddEventListener(string type, EventListenerDelegate eventListenerDelegate)
	{
		eventDispatcher.AddEventListener(type, eventListenerDelegate);
	}

	public void RemoveEventListener(string type, EventListenerDelegate eventListenerDelegate)
	{
		eventDispatcher.RemoveEventListener(type, eventListenerDelegate);
	}

	public void DispatchEvent(BaseEvent evt)
	{
		eventDispatcher.DispatchEvent(evt);
	}

	public double getSecondsTillDayEnds()
	{
		return GetMillisecondsTillDayEnds() / 1000;
	}
}
