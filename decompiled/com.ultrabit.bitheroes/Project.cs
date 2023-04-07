using System;
using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.events.dialog;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.character.achievements;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.server;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.augment;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.enchant;
using com.ultrabit.bitheroes.ui.eventsales;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.fishing;
using com.ultrabit.bitheroes.ui.friend;
using com.ultrabit.bitheroes.ui.fusion;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.gauntlet;
using com.ultrabit.bitheroes.ui.guild;
using com.ultrabit.bitheroes.ui.gve;
using com.ultrabit.bitheroes.ui.gvg;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.instance.fishing;
using com.ultrabit.bitheroes.ui.invasion;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.pvp;
using com.ultrabit.bitheroes.ui.raid;
using com.ultrabit.bitheroes.ui.rift;
using com.ultrabit.bitheroes.ui.shop;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.zone;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes;

public class Project
{
	private Character _character;

	private MenuInterface _menuInterface;

	private GameNotification _notification;

	private Dungeon _dungeon;

	private Battle _battle;

	private Instance _instance;

	private ZoneWindow _zoneWindow;

	private ServerInstanceRef _reconnectRef;

	private Coroutine _duelTimer;

	private bool _guildHallEditMode;

	private bool _fishingMode;

	private bool _brawlRejoin;

	private CharacterData _playerData;

	private DialogWindow notificationNewBoostersDialog;

	private uint _armoryMinLevelRequired = 25u;

	private GuildInviteWindow currentGuildInviteWindow;

	private FriendInviteWindow currentFriendInviteWindow;

	private bool _autopilot;

	private bool _gameIsPaused;

	private int _lastZone = -1;

	private bool _offerwallChecked;

	private TransitionScreen _currentTransitionScreen;

	private UnityAction _closeNotificationByFriendRequest;

	private CharacterData _lastArmoryCharacterData;

	private bool flag;

	public HashSet<string> NFTTokenFreezeCache { get; private set; } = new HashSet<string>();


	private string _randomAdviceLink
	{
		get
		{
			List<DialogBook.AdviceLink> list = new List<DialogBook.AdviceLink>();
			foreach (DialogBook.AdviceLink item in Enum.GetValues(typeof(DialogBook.AdviceLink)).Cast<DialogBook.AdviceLink>().ToList())
			{
				switch (item)
				{
				case DialogBook.AdviceLink.advice_6:
					if (!VariableBook.GameRequirementMet(2))
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_7:
					if (!VariableBook.GameRequirementMet(8) || GameData.instance.PROJECT.character.guildData != null)
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_8:
					if (!VariableBook.GameRequirementMet(2))
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_9:
				case DialogBook.AdviceLink.advice_10:
				case DialogBook.AdviceLink.advice_11:
				{
					TutorialRef tutorialRef3 = VariableBook.LookUpTutorial("runes_shop");
					TutorialRef tutorialRef4 = VariableBook.LookUpTutorial("runes_craft");
					if (!tutorialRef3.areItemConditionsMet && !tutorialRef3.isMinFlagConditionMet && !tutorialRef4.areItemConditionsMet && !tutorialRef4.isMinFlagConditionMet)
					{
						continue;
					}
					break;
				}
				case DialogBook.AdviceLink.advice_12:
				case DialogBook.AdviceLink.advice_13:
				{
					TutorialRef tutorialRef5 = VariableBook.LookUpTutorial("enchants_identify");
					if (!tutorialRef5.areItemConditionsMet && !tutorialRef5.isMinFlagConditionMet)
					{
						continue;
					}
					break;
				}
				case DialogBook.AdviceLink.advice_14:
					if (!VariableBook.GameRequirementMet(13))
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_15:
					if (!VariableBook.GameRequirementMet(31))
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_16:
				case DialogBook.AdviceLink.advice_17:
				{
					TutorialRef tutorialRef = VariableBook.LookUpTutorial("augment_craft");
					TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("augment_shop");
					if (!tutorialRef.areItemConditionsMet && !tutorialRef.isMinFlagConditionMet && !tutorialRef2.areItemConditionsMet && !tutorialRef2.isMinFlagConditionMet)
					{
						continue;
					}
					break;
				}
				case DialogBook.AdviceLink.advice_2:
					if (!GameData.instance.PROJECT.character.equipment.hasAnySlotEmpty)
					{
						continue;
					}
					break;
				case DialogBook.AdviceLink.advice_1:
				case DialogBook.AdviceLink.advice_3:
				case DialogBook.AdviceLink.advice_4:
				case DialogBook.AdviceLink.advice_5:
					break;
				default:
					continue;
				}
				list.Add(item);
			}
			return list[UnityEngine.Random.Range(0, list.Count)].ToString();
		}
	}

	public Character character => _character;

	public Dungeon dungeon => _dungeon;

	public Battle battle => _battle;

	public Instance instance => _instance;

	public GameNotification notification => _notification;

	public MenuInterface menuInterface => _menuInterface;

	public bool guildHallEditMode => _guildHallEditMode;

	public CharacterData playerData
	{
		get
		{
			return _playerData;
		}
		set
		{
			_playerData = value;
		}
	}

	public uint armoryMinLevelRequired => _armoryMinLevelRequired;

	public bool gameIsPaused => _gameIsPaused;

	public int lastZone
	{
		get
		{
			return _lastZone;
		}
		set
		{
			_lastZone = value;
		}
	}

	public Project()
	{
		createListeners();
	}

	public void NotificationNewBoosters(int num = 1)
	{
		if (notificationNewBoostersDialog == null)
		{
			notificationNewBoostersDialog = GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_congratulations"), Language.GetString("ui_new_booster"));
			notificationNewBoostersDialog.DESTROYED.AddListener(DialogNotBoosterClosed);
		}
	}

	private void DialogNotBoosterClosed(object e)
	{
		notificationNewBoostersDialog.DESTROYED.RemoveListener(DialogNotBoosterClosed);
		notificationNewBoostersDialog = null;
	}

	public void doDayChange()
	{
		if (_character != null)
		{
			CharacterDALC.instance.doDailyQuestCheck();
			_character.updateDailyRewardNotification();
		}
	}

	public void SetCharacter(Character character)
	{
		if (character == null)
		{
			D.Log("CHARACTER NULL");
			return;
		}
		if (_character != null)
		{
			_character.RemoveListener("GUILD_CHANGE", OnCharacterGuildChange);
		}
		if (_notification == null)
		{
			_notification = GameData.instance.windowGenerator.AddNotificationObject();
		}
		_character = character;
		AppInfo.playerID = _character.playerID;
		ServerExtension.instance.stopIdleTimer();
		ServerExtension.instance.stopIdleDisconnectTimer();
		_character.AddListener("GUILD_CHANGE", OnCharacterGuildChange);
	}

	public void ContinueSetCharacter()
	{
		GameDALC.instance.doPlayersOnline(update: true);
		ServerExtension.instance.startPreventIdleTimer(doStartIdleTimer: true);
		KongregateAnalytics.updateCommonFields();
		Instance.ResetForceOfferwall();
		Instance.ResetAutoTravelType();
		if (_character == null)
		{
			D.Log("_character NULL");
			return;
		}
		_character.updateAchievements();
		_character.updateNotifications();
		_character.TrackSetStats("set_character");
		UpdateFriendCharIDs();
	}

	private void UpdateFriendCharIDs()
	{
		if (GameData.instance.friendCharIDsToUpdate == null)
		{
			return;
		}
		foreach (int item in GameData.instance.friendCharIDsToUpdate)
		{
			OnPlayerLoginSuccess(item);
			D.Log($"Project.cs :: Friend {item} login state updated.");
		}
		GameData.instance.friendCharIDsToUpdate = null;
	}

	public bool checkPreRegistration()
	{
		return false;
	}

	private bool doAndroidPreRegistration()
	{
		return false;
	}

	private void createAndroidServiceListeners()
	{
	}

	private void removeAndroidServiceListeners()
	{
	}

	private void createAndroidInventoryListeners()
	{
	}

	private void removeAndroidInventoryListeners()
	{
	}

	private void onAndroidServiceReady()
	{
	}

	private void onAndroidServiceNotSupported()
	{
	}

	private void doAndroidInventory()
	{
	}

	private void onAndroidInventoryLoaded()
	{
	}

	private void onAndroidLoadInventoryFailed()
	{
	}

	private void confirmAndroidPreRegistrationItem()
	{
	}

	private void onAndroidPreRegistrationItemConfirm(DialogEvent e)
	{
	}

	private void executeAndroidPreRegistrationItem()
	{
	}

	private void onAndroidValidateComplete(Event e)
	{
	}

	private void onAndroidValidateError()
	{
	}

	internal void doInventoryCheck()
	{
	}

	private void onInventoryCheck(DALCEvent e)
	{
	}

	private void OnCharacterGuildChange()
	{
		if (_character.guildData != null)
		{
			GameData.instance.PROJECT.notification.AddToQueue(_character.id, _character.name, 11, _character.guildData.name);
		}
		else
		{
			GameData.instance.PROJECT.notification.AddToQueue(_character.id, _character.name, 12);
			if (_instance != null && _instance.instanceRef.type == 2)
			{
				DoEnterInstance(InstanceBook.GetFirstInstanceByType(1));
			}
		}
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildWindow)) != null || GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildlessWindow)) != null || GameData.instance.windowGenerator.GetDialogByClass(typeof(GvGEventWindow)) != null)
		{
			GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
			if (_character.guildData != null)
			{
				GameData.instance.windowGenerator.NewGuildWindow();
			}
			else
			{
				GameData.instance.windowGenerator.NewGuildlessWindow();
			}
		}
		DoUpdateTimers();
		GameData.instance.windowGenerator.UpdateChatWindow();
	}

	public bool IsArmoryLoaded(CharacterData characterData)
	{
		return characterData.Equals(_lastArmoryCharacterData);
	}

	public void CleanLastArmoryCharacterLoaded()
	{
		_lastArmoryCharacterData = null;
	}

	public void DoEnterInstance(InstanceRef instanceRef, bool transition = true, CharacterData characterData = null)
	{
		if (instanceRef != null)
		{
			if (characterData != null)
			{
				_lastArmoryCharacterData = characterData;
			}
			GameData.instance.main.ShowLoading();
			GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstanceResponse);
			GameDALC.instance.doEnterInstance(instanceRef, transition, characterData);
		}
	}

	private void OnEnterInstanceResponse(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstanceResponse);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError(string.Format("OnEnterInstanceResponse Error {0}", sfsob.GetInt("err0")));
		}
	}

	private void OnReceiveRequest(BaseEvent e)
	{
		if (_character != null)
		{
			SFSObject sfsob = (e as DALCEvent).sfsob;
			RequestData requestData = RequestData.fromSFSObject(sfsob);
			_character.addRequestData(requestData);
			GameData.instance.PROJECT.notification.AddToQueue(requestData.characterData.charID, requestData.characterData.name, 2);
			_closeNotificationByFriendRequest = delegate
			{
				CloseNotificationIfFriendRequestNoLongerExists(requestData);
			};
			GameData.instance.PROJECT.character.AddListener("REQUEST_CHANGE", _closeNotificationByFriendRequest);
			GameData.instance.PROJECT.notification.ON_NOTIFICATION_CLOSE.AddListener(delegate
			{
				GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", _closeNotificationByFriendRequest);
			});
		}
	}

	private void CloseNotificationIfFriendRequestNoLongerExists(RequestData requestData)
	{
		if (GameData.instance.PROJECT.character.getRequestData(requestData.characterData.charID) == null)
		{
			GameData.instance.PROJECT.notification.ForceNotificationEnd();
			GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", _closeNotificationByFriendRequest);
		}
	}

	private void OnFriendAdded(BaseEvent e)
	{
		if (_character != null)
		{
			FriendData friendData = FriendData.fromSFSObject((e as DALCEvent).sfsob);
			_character.addFriendData(friendData);
			GameData.instance.PROJECT.notification.AddToQueue(friendData.characterData.charID, friendData.characterData.parsedName, 3);
			_character.updateAchievements();
			GameDALC.instance.RefreshMyBounties();
		}
	}

	private void OnFriendRemoved(BaseEvent e)
	{
		if (_character != null)
		{
			int @int = (e as DALCEvent).sfsob.GetInt("cha1");
			_character.removeFriendData(@int);
		}
	}

	private void OnPlayerUpdate(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		if (_character == null)
		{
			return;
		}
		CharacterData characterData = CharacterData.fromSFSObject(dALCEvent.sfsob);
		FriendData friendData = _character.getFriendData(characterData.charID, -1, duplicateData: false);
		if (friendData != null)
		{
			friendData.characterData = characterData;
		}
		if (_character.guildData != null)
		{
			GuildMemberData member = _character.guildData.getMember(characterData.charID, -1, duplicateData: false);
			if (member != null)
			{
				member.characterData = characterData;
				_character.Broadcast("GUILD_MEMBER_CHANGE");
			}
		}
	}

	private void OnPlayersOnline(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		if (_character != null)
		{
			int[] intArray = dALCEvent.sfsob.GetIntArray("cha45");
			foreach (int charID in intArray)
			{
				_character.setPlayerOnline(charID, online: true);
			}
		}
	}

	private void OnReconnectDungeon(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		string text = (sfsob.ContainsKey("serv7") ? sfsob.GetUtfString("serv7") : null);
		if (text == null)
		{
			return;
		}
		ServerRef serverRef = ServerBook.Lookup(GameData.instance.SAVE_STATE.serverID);
		if (serverRef != null)
		{
			ServerInstanceRef serverInstanceRef = serverRef.GetInstance(text);
			if (serverInstanceRef != null)
			{
				_reconnectRef = serverInstanceRef;
				checkTutorial();
			}
		}
	}

	private void DoReconnectDungeonConfirm(ServerInstanceRef serverInstanceRef)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(GameData.instance.windowGenerator.GetErrorName(), Language.GetString("dungeon_reconnect_confirm"), null, null, delegate
		{
			OnReconnectDungeonYes(serverInstanceRef);
		}, delegate
		{
			OnReconnectDungeonNo();
		});
	}

	private void OnReconnectDungeonYes(ServerInstanceRef serverInstanceRef)
	{
		GameData.instance.SAVE_STATE.isComingFromDungeon = true;
		ServerExtension.instance.Disconnect(null, null, relog: true, serverInstanceRef.id);
	}

	private void OnReconnectDungeonNo()
	{
		GameDALC.instance.doReconnectDungeon(cancel: true);
	}

	private void OnPlayerLogin(BaseEvent e)
	{
		int @int = (e as DALCEvent).sfsob.GetInt("cha1");
		if (_character == null)
		{
			GameData.instance.friendCharIDsToUpdate?.Add(@int);
			D.Log($"Project.cs :: Tried to update log state to friend {@int} during login, added to late update list.");
		}
		else
		{
			OnPlayerLoginSuccess(@int);
		}
	}

	private void OnPlayerLoginSuccess(int charID)
	{
		bool playerOnline = _character.getPlayerOnline(charID);
		_character.setPlayerOnline(charID, online: true);
		FriendData friendData = _character.getFriendData(charID);
		GuildMemberData guildMemberData = ((_character.guildData != null) ? _character.guildData.getMember(charID) : null);
		if ((friendData != null || guildMemberData != null) && !playerOnline)
		{
			CharacterData characterData = ((friendData != null) ? friendData.characterData : guildMemberData.characterData);
			GameData.instance.PROJECT.notification.AddToQueue(charID, characterData.name, 1);
		}
	}

	private void OnPlayerLogout(BaseEvent e)
	{
		if (e is DALCEvent dALCEvent && dALCEvent.sfsob != null)
		{
			int @int = dALCEvent.sfsob.GetInt("cha1");
			if (_character != null)
			{
				_character.setPlayerOnline(@int, online: false);
			}
		}
	}

	private void OnDailyQuestsUpdate(BaseEvent e)
	{
		D.Log("OnDailyQuestsUpdate");
		DailyQuests dailyQuests = DailyQuests.fromSFSObject((e as DALCEvent).sfsob);
		List<DailyQuestData> list = new List<DailyQuestData>();
		List<DailyQuestData> list2 = new List<DailyQuestData>();
		int num = 0;
		foreach (DailyQuestData quest in dailyQuests.quests)
		{
			if (quest.completed)
			{
				num++;
			}
			DailyQuestData questDataFromID = _character.dailyQuests.getQuestDataFromID(quest.questRef.id);
			if (questDataFromID == null)
			{
				list2.Add(quest);
			}
			else if (!questDataFromID.completed && quest.completed)
			{
				list.Add(questDataFromID);
			}
		}
		foreach (DailyQuestData item in list)
		{
			_notification.AddToQueue(_character.id, item.questRef.coloredName, 13, item);
		}
		if (list2.Count > 0)
		{
			_notification.AddToQueue(_character.id, null, 14);
		}
		if (num > 0 || list.Count > 0 || list2.Count > 0)
		{
			dailyQuests.setUpdated(v: true);
		}
		_character.dailyQuests = dailyQuests;
	}

	private void OnSaveTeam(BaseEvent e)
	{
		TeamData teamData = TeamData.fromSFSObject((e as DALCEvent).sfsob);
		_character.teams.setTeam(teamData.type, teamData.teamRules, teamData.teammates);
	}

	private void OnAdjustItems(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		List<ItemData> list = (sfsob.ContainsKey("ite4") ? ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4")) : null);
		List<ItemData> list2 = (sfsob.ContainsKey("ite5") ? ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5")) : null);
		if (list != null && list.Count > 0)
		{
			_character.addItems(list);
			foreach (ItemData item in list)
			{
				_notification.AddToQueue(_character.id, _character.name, 17, item);
			}
		}
		if (list2 == null || list2.Count <= 0)
		{
			return;
		}
		_character.removeItems(list2);
		foreach (ItemData item2 in list2)
		{
			_notification.AddToQueue(_character.id, _character.name, 18, item2);
		}
	}

	private void OnGuildChatMessage(BaseEvent e)
	{
		if (e != null)
		{
			ChatData chatData = ChatData.fromSFSObject((e as DALCEvent).sfsob);
			if (chatData != null && _character != null && _character.guildData != null && _character.guildData.messages != null)
			{
				_character.guildData.addMessage(chatData);
			}
		}
	}

	private void OnGuildPlayerAdded(BaseEvent e)
	{
		if (_character.guildData != null)
		{
			GuildMemberData guildMemberData = GuildMemberData.fromSFSObject((e as DALCEvent).sfsob);
			if (guildMemberData.characterData.charID != _character.id)
			{
				_character.guildData.addMember(guildMemberData);
				_character.Broadcast("GUILD_MEMBER_CHANGE");
				_notification.AddToQueue(guildMemberData.characterData.charID, guildMemberData.characterData.name, 9);
			}
		}
	}

	private void OnGuildPlayerRemoved(BaseEvent e)
	{
		if (_character.guildData == null)
		{
			return;
		}
		int @int = (e as DALCEvent).sfsob.GetInt("cha1");
		if (@int != _character.id)
		{
			GuildMemberData member = _character.guildData.getMember(@int);
			if (member != null)
			{
				string name = member.characterData.name;
				_character.guildData.removeMember(@int);
				_character.Broadcast("GUILD_MEMBER_CHANGE");
				_notification.AddToQueue(@int, name, 10);
			}
		}
	}

	public void ClearOfferwallChecked()
	{
		_offerwallChecked = false;
	}

	private void OnGuildChange(BaseEvent e)
	{
		CharacterGuildData characterGuildData = CharacterGuildData.fromSFSObject((e as DALCEvent).sfsob);
		_character.guildData = characterGuildData;
		if (characterGuildData != null)
		{
			_character.updateAchievements();
			GameDALC.instance.doPlayersOnline();
			KongregateAnalytics.TrackCPEEvent("kong_join_guild");
		}
	}

	private void OnGuildRankUpdate(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		int @int = sfsob.GetInt("cha1");
		int int2 = sfsob.GetInt("gui1");
		List<bool> booleanVectorFromArray = Util.getBooleanVectorFromArray(sfsob.GetBoolArray("gui4"));
		_character.guildData.UpdateMemberDataRank(@int, int2);
		if (@int == _character.id)
		{
			_character.guildData.setRank(int2);
			_character.guildData.setPermissions(booleanVectorFromArray);
			_character.Broadcast("GUILD_PERMISSIONS_CHANGE");
		}
		_character.Broadcast("GUILD_MEMBER_CHANGE");
	}

	private void OnGuildPermissionsUpdate(BaseEvent e)
	{
		List<bool> booleanVectorFromArray = Util.getBooleanVectorFromArray((e as DALCEvent).sfsob.GetBoolArray("gui4"));
		_character.guildData.setPermissions(booleanVectorFromArray);
		if (_instance != null)
		{
			_instance.instanceInterface.UpdateUIButtons();
		}
		if (_guildHallEditMode && !_character.guildData.hasPermission(6))
		{
			ToggleGuildHallEditMode();
		}
		_character.Broadcast("GUILD_PERMISSIONS_CHANGE");
	}

	private void OnGuildPerksUpdate(BaseEvent e)
	{
		GuildPerks perks = GuildPerks.fromSFSObject((e as DALCEvent).sfsob);
		if (_character.guildData != null)
		{
			_character.guildData.setPerks(perks);
		}
		_character.Broadcast("GUILD_PERKS_CHANGE");
		DoUpdateTimers();
	}

	private void OnPrivateMessage(BaseEvent e)
	{
		if (this == null || e == null)
		{
			return;
		}
		SFSObject sFSObject = (e as DALCEvent)?.sfsob;
		if (sFSObject == null)
		{
			return;
		}
		ChatData chatData = ChatData.fromSFSObject(sFSObject);
		if (chatData == null)
		{
			return;
		}
		int @int = sFSObject.GetInt("chat4");
		int int2 = sFSObject.GetInt("chat5");
		Conversation conversation;
		if (@int == _character.id)
		{
			conversation = GameData.instance.windowGenerator.ShowConversation(int2);
		}
		else
		{
			if (!chatData.admin && !chatData.moderator && _character.getChatIgnore(chatData.charID) != null)
			{
				return;
			}
			conversation = GameData.instance.windowGenerator.ShowConversation(chatData.charID, chatData.name);
			GameData.instance.audioManager.PlaySoundLink("messageprivate");
		}
		conversation.addMessage(chatData);
	}

	private void OnDuelTarget(BaseEvent e)
	{
		CharacterData characterData = CharacterData.fromSFSObject((e as DALCEvent).sfsob);
		if (characterData != null)
		{
			_notification.AddToQueue(characterData.charID, characterData.name, 15, characterData);
		}
		else
		{
			_notification.RemoveNotificationType(15);
		}
	}

	private void OnDuelSource(BaseEvent e)
	{
		CharacterData characterData = CharacterData.fromSFSObject((e as DALCEvent).sfsob);
		if (characterData != null)
		{
			_notification.AddToQueue(characterData.charID, characterData.name, 16, characterData);
			if ((bool)GameData.instance.PROJECT.dungeon || (bool)GameData.instance.PROJECT.battle || GameData.instance.windowGenerator.HasDialogByClass(typeof(BrawlRoomWindow)) || !VariableBook.GameRequirementMet(1))
			{
				DoDuelDecline(characterData.charID, load: false);
			}
		}
		else
		{
			_notification.RemoveNotificationType(16);
		}
	}

	private void OnHeroFrozen(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		string text = (sfsob.ContainsKey("nft2") ? sfsob.GetUtfString("nft2") : "");
		if (!NFTTokenFreezeCache.Contains(text))
		{
			NFTTokenFreezeCache.Add(text);
		}
		if (character != null && !string.IsNullOrEmpty(character.nftToken) && !string.IsNullOrEmpty(text) && character.nftToken == text)
		{
			character.FreezeNFT();
			ServerExtension.instance.Disconnect(Language.GetString("ui_frozen_alert"), Language.GetString("ui_confirm"));
		}
	}

	public bool IsNFTInFrozenCache(Character character)
	{
		if (character != null && !string.IsNullOrEmpty(character.nftToken))
		{
			return IsNFTInFrozenCache(character.nftToken);
		}
		NFTTokenFreezeCache.Clear();
		return false;
	}

	public bool IsNFTInFrozenCache(string nftToken)
	{
		bool result = NFTTokenFreezeCache.Contains(nftToken);
		NFTTokenFreezeCache.Clear();
		return result;
	}

	public void DoUpdateTimers()
	{
		if (_character != null)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(26), OnUpdateTimers);
			CharacterDALC.instance.doUpdateTimers();
		}
	}

	private void OnUpdateTimers(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(26), OnUpdateTimers);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			_character.checkTimerChanges(sfsob);
		}
	}

	public bool CheckCharacter()
	{
		if (_dungeon != null || _battle != null)
		{
			return false;
		}
		if (GameData.instance.tutorialManager != null && GameData.instance.tutorialManager.hasPopup)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(CharacterCustomizeWindow)))
		{
			return false;
		}
		if (_character.getForceCharacterCustomize())
		{
			GameData.instance.windowGenerator.NewCharacterCustomizeWindow().SELECT.AddListener(OnCharacterChecked);
			return true;
		}
		return false;
	}

	public bool checkTutorial()
	{
		if (GameData.instance.tutorialManager.hasPopup || _character == null || GameData.instance.windowGenerator.GetDialogCountWithout(typeof(DungeonUI)) > 0)
		{
			return false;
		}
		GameData.instance.windowGenerator.UpdateRestrictions();
		if (CheckCharacter())
		{
			return true;
		}
		if (_reconnectRef != null)
		{
			DoReconnectDungeonConfirm(_reconnectRef);
			_reconnectRef = null;
			return true;
		}
		if (GameData.instance.DUEL_CHAR_ID > 0)
		{
			GameData.instance.windowGenerator.ShowPlayer(GameData.instance.DUEL_CHAR_ID);
			DoDuelSend(GameData.instance.DUEL_CHAR_ID);
			Main.ClearDuelCharID();
			return true;
		}
		if (GameData.instance.BRAWL_INDEX > 0)
		{
			ShowBrawlWindow();
			DoBrawlJoin(GameData.instance.BRAWL_INDEX, invited: true);
			Main.ClearBrawlIndex();
			return true;
		}
		if (_menuInterface != null && _menuInterface.CheckTutorial())
		{
			return true;
		}
		if (_instance != null && _instance.CheckTutorial())
		{
			return true;
		}
		if (_character.pvpEventLootID >= 0)
		{
			DoPvPEventLootItems();
			return true;
		}
		if (_character.riftEventLootID >= 0)
		{
			DoRiftEventLootItems();
			return true;
		}
		if (_character.gauntletEventLootID >= 0)
		{
			DoGauntletEventLootItems();
			return true;
		}
		if (_character.gvgEventLootID >= 0)
		{
			DoGvGEventLootItems();
			return true;
		}
		if (_character.invasionEventLootID >= 0)
		{
			DoInvasionEventLootItems();
			return true;
		}
		if (_character.fishingEventLootID >= 0)
		{
			DoFishingEventLootItems();
			return true;
		}
		if (_character.gveEventLootID >= 0)
		{
			DoGvEEventLootItems();
			return true;
		}
		return false;
	}

	private void DoPvPEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_ITEMS), OnPvPEventLootItems);
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_ITEMS), OnPvPEventLootItems);
		PvPDALC.instance.doEventLootItems(_character.pvpEventLootID);
	}

	private void OnPvPEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_ITEMS), OnPvPEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.pvpEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.pvpEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("PvP Event Reward", null, list, sfsob, "PvP Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(1)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoRiftEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_ITEMS), OnRiftEventLootItems);
		RiftDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_ITEMS), OnRiftEventLootItems);
		RiftDALC.instance.doEventLootItems(_character.riftEventLootID);
	}

	private void OnRiftEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_ITEMS), OnRiftEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.riftEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.riftEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("Trials Event Reward", null, list, sfsob, "Trials Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(2)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoGauntletEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnGauntletEventLootItems);
		GauntletDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnGauntletEventLootItems);
		GauntletDALC.instance.doEventLootItems(_character.gauntletEventLootID);
	}

	private void OnGauntletEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnGauntletEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.gauntletEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.gauntletEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("Gauntlet Event Reward", null, list, sfsob, "Gauntlet Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(3)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoGvGEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnGvGEventLootItems);
		GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnGvGEventLootItems);
		GvGDALC.instance.doEventLootItems(_character.gvgEventLootID);
	}

	private void OnGvGEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnGvGEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.gvgEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.gvgEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("GvG Event Reward", null, list, sfsob, "GvG Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(4)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoInvasionEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnInvasionEventLootItems);
		InvasionDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnInvasionEventLootItems);
		InvasionDALC.instance.doEventLootItems(_character.invasionEventLootID);
	}

	private void OnInvasionEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnInvasionEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.invasionEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.invasionEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("Invasion Event Reward", null, list, sfsob, "Invasion Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(5)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoFishingEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnFishingEventLootItems);
		FishingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnFishingEventLootItems);
		FishingDALC.instance.doEventLootItems(_character.fishingEventLootID);
	}

	private void OnFishingEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnFishingEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.fishingEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.fishingEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("Fishing Event Reward", null, list, sfsob, "Fishing Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(6)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void DoGvEEventLootItems()
	{
		GameData.instance.main.ShowLoading();
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnGvEEventLootItems);
		GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnGvEEventLootItems);
		GvEDALC.instance.doEventLootItems(_character.gveEventLootID);
	}

	private void OnGvEEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnGvEEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			_character.gveEventLootID = -1;
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.gveEventLootID = @int;
		if (list.Count > 0)
		{
			_character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("GvE Event Reward", null, list, sfsob, "GvE Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true, EventRef.getEventTypeNameShort(7)).DESTROYED.AddListener(OnEventLootClosed);
		}
		else
		{
			checkTutorial();
		}
	}

	private void OnEventLootClosed(object e)
	{
		checkTutorial();
	}

	public void ValidateTutorialState(int state)
	{
		if (!_character.tutorial.GetState(state))
		{
			_character.tutorial.SetState(state);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void CheckTutorialChanges()
	{
		if (_character.tutorial.changed)
		{
			CharacterDALC.instance.doSaveTutorial(_character.tutorial.states);
			_character.tutorial.changed = false;
		}
	}

	public void DebugForceTutorialChanges()
	{
		CharacterDALC.instance.doSaveTutorial(_character.tutorial.states);
	}

	private void clearMenuInterface()
	{
		if (_menuInterface != null)
		{
			_menuInterface.Clear();
			_menuInterface = null;
		}
	}

	private void clearBattle()
	{
		if (_battle != null)
		{
			_battle.RemoveEventListener(CustomSFSXEvent.COMPLETE, onCompleteBattle);
			_battle.Clear();
			_battle = null;
		}
	}

	private void clearDungeon()
	{
		if (_dungeon != null)
		{
			character.clearDungeonLootItems();
			_dungeon.COMPLETE.RemoveListener(OnCompleteDungeon);
			_dungeon.DoDestroy();
			_dungeon = null;
		}
	}

	private void ClearInstance(bool exit = true, bool clear = true, bool remove = true)
	{
		if (_instance != null)
		{
			_instance.ShowInstance(enabled: false);
			if (exit)
			{
				_instance.extension.DoPlayerExit();
			}
			if (clear)
			{
				_instance = null;
			}
		}
	}

	private void clearNotification()
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void AddMenuInterface()
	{
		clearMenuInterface();
		_menuInterface = GameData.instance.windowGenerator.AddMenuInterface();
		Main.CONTAINER.AddToLayer(_menuInterface.gameObject, 1);
		GameData.instance.main.InitCurrencies();
	}

	private void addInstance()
	{
		if (!(_instance == null))
		{
			_instance.ShowInstance(enabled: true);
		}
	}

	private void hideMenuContainer()
	{
		_menuInterface.gameObject.SetActive(value: false);
	}

	private void showMenuContainer()
	{
		if (_menuInterface != null)
		{
			_menuInterface.gameObject.SetActive(value: true);
			_menuInterface.UpdateRestrictions();
		}
		GameData.instance.windowGenerator.UpdateRestrictions();
	}

	private void OnCharacterChecked()
	{
		checkTutorial();
	}

	private void OnDialogChange()
	{
		checkTutorial();
	}

	private void onGameUpdate(DALCEvent e)
	{
		ServerExtension.instance.Disconnect(Language.GetString("ui_game_updated"), Language.GetString("ui_connect"));
	}

	private void onNotification(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		int int2 = sfsob.GetInt("not0");
		string utfString2 = sfsob.GetUtfString("not1");
		if (_notification != null)
		{
			_notification.AddToQueue(@int, utfString, int2, utfString2);
		}
	}

	private void onEnterBattle(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		GameData.instance.main.HideLoading();
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.Log("err0");
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		clearBattle();
		_battle = Battle.fromSFSObject(sfsob);
		_battle.AddEventListener(CustomSFSXEvent.COMPLETE, onCompleteBattle);
		_battle.gameObject.SetActive(value: false);
		_currentTransitionScreen = GameData.instance.windowGenerator.NewBattleTransitionScreen("Battle", null, onEnterBattleTransitionComplete, onEnterBattleTransitionToggle);
		GameData.instance.audioManager.PlayMusic(_battle.music);
	}

	private void onEnterBattleTransitionComplete()
	{
		_battle.DoStart();
	}

	private void onEnterBattleTransitionToggle()
	{
		if ((bool)_dungeon)
		{
			_dungeon.ShowDungeon(enabled: false);
		}
		else
		{
			ClearInstance(exit: false, clear: false);
			GameData.instance.windowGenerator.ShowCurrencies(show: false);
			GameData.instance.windowGenerator.ShowBattleUI();
		}
		_battle.gameObject.SetActive(value: true);
		_battle.MoveToScene();
		hideMenuContainer();
		GameData.instance.main.HideBackground();
	}

	private void onCompleteBattle(BaseEvent e)
	{
		int type = _battle.type;
		_currentTransitionScreen = GameData.instance.windowGenerator.NewTransitionScreen(null, "Battle", onCompleteBattleTransitionComplete, delegate
		{
			onCompleteBattleTransitionToggle(type);
		}, unloadFirst: false);
	}

	private void onCompleteBattleTransitionToggle(int type)
	{
		if (_dungeon != null)
		{
			GameData.instance.audioManager.PlayMusic(_dungeon.dungeonRef.music, loop: true, tween: true, _dungeon.musicPosition);
			_dungeon.ShowDungeon(enabled: true);
			clearBattle();
			OnAdviceDialogWindowClosed(null, type);
		}
		else
		{
			addInstance();
			GameData.instance.audioManager.PlayMusic(_instance.instanceRef.musicRef);
			GameData.instance.windowGenerator.ShowCurrencies(show: true);
			GameData.instance.windowGenerator.HideBattleUI();
			bool defeated = _battle.defeated;
			clearBattle();
			if (defeated && GameData.instance.SAVE_STATE.defeatAdvices)
			{
				GameData.instance.windowGenerator.NewDialogPopup(DialogBook.Lookup(_randomAdviceLink)).DESTROYED.AddListener(delegate(object z)
				{
					OnAdviceDialogWindowClosed(z, type);
				});
			}
			else
			{
				OnAdviceDialogWindowClosed(null, type);
			}
		}
		showMenuContainer();
	}

	private void OnAdviceDialogWindowClosed(object e, int type)
	{
		if (e != null)
		{
			(e as WindowsMain).DESTROYED.RemoveListener(delegate(object z)
			{
				OnAdviceDialogWindowClosed(z, type);
			});
		}
		switch (type)
		{
		case 2:
			ShowPvPWindow();
			break;
		case 6:
			ShowGauntletWindow();
			break;
		case 7:
			ShowGvGWindow();
			break;
		case 8:
			ShowInvasionWindow();
			break;
		case 9:
			if (_brawlRejoin)
			{
				DoBrawlRejoin();
				SetBrawlRejoin(v: false);
			}
			else
			{
				ShowBrawlWindow();
			}
			break;
		case 3:
		case 4:
		case 5:
			break;
		}
	}

	private void onCompleteBattleTransitionComplete()
	{
		if (_dungeon != null)
		{
			bool flag = _dungeon.extension.QueueContains(9);
			DungeonPlayer dungeonPlayer = _dungeon.focus as DungeonPlayer;
			_dungeon.extension.SetPaused(pause: false);
			if (_dungeon.IsCleared())
			{
				_dungeon.extension.ShowCleared();
			}
			else if (_character.autoPilot)
			{
				_dungeon.CheckAutoPilot();
			}
			else if (!_character.autoPilot && flag && !dungeonPlayer.IsDead())
			{
				_dungeon.extension.ShowComplete();
			}
		}
	}

	private void onEnterDungeon(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		if (!GameData.instance.SAVE_STATE.isComingFromDungeon)
		{
			GameData.instance.PROJECT.character.clearDungeonLootItems();
		}
		GameData.instance.SAVE_STATE.isComingFromDungeon = true;
		if (GameData.instance.main != null)
		{
			GameData.instance.main.HideLoading();
		}
		SFSObject sfsob = dALCEvent.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			D.LogError(string.Format("Project::OnEnterDungeon::{0}", sfsob.GetInt("err0")));
			return;
		}
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		clearDungeon();
		Scene? scene = null;
		DungeonRef dungeonRef = Dungeon.DungeonRefFromSFSObject(sfsob);
		if (_instance != null)
		{
			scene = _instance.instanceScene;
		}
		_currentTransitionScreen = GameData.instance.windowGenerator.NewTransitionScreen("Dungeon", scene.HasValue ? scene.Value.name : null, onEnterDungeonTransitionComplete, delegate
		{
			onEnterDungeonTransitionToggle(sfsob);
		});
		GameData.instance.audioManager.PlayMusic(dungeonRef.music);
		GameData.instance.audioManager.PlaySoundLink("dungeonenter");
	}

	private void onEnterDungeonTransitionToggle(SFSObject sfsob)
	{
		_dungeon = Dungeon.FromSFSObject(sfsob);
		_dungeon.COMPLETE.AddListener(OnCompleteDungeon);
		CheckInitialLogin();
		ClearInstance(exit: false);
		_dungeon.AddedToStage(SceneManager.GetSceneByName("Dungeon"));
		GameData.instance.main.HideBackground();
		GameData.instance.windowGenerator.ShowCurrencies(show: false);
		GameData.instance.windowGenerator.ShowBattleUI();
	}

	private void onEnterDungeonTransitionComplete()
	{
		_dungeon.CheckTutorial();
		MenuInterfaceAutoPilotTile menuInterfaceAutoPilotTile = GameData.instance.windowGenerator.GetBattleUI(typeof(MenuInterfaceAutoPilotTile)) as MenuInterfaceAutoPilotTile;
		if (menuInterfaceAutoPilotTile != null)
		{
			menuInterfaceAutoPilotTile.CheckTutorial();
		}
	}

	private void OnCompleteDungeon(object e)
	{
		_dungeon.AvoidDestruction();
		_currentTransitionScreen = GameData.instance.windowGenerator.NewTransitionScreen(null, "Dungeon", OnCompleteDungeonTransitionComplete, OnCompleteDungeonTransitionToggle, unloadFirst: false);
	}

	private void OnCompleteDungeonTransitionToggle()
	{
		GameData.instance.windowGenerator.ShowCurrencies(show: true);
		GameData.instance.windowGenerator.HideBattleUI();
		if (_dungeon.extension.rewards != null)
		{
			ItemListWindow itemListWindow = GameData.instance.windowGenerator.NewItemListWindow(_dungeon.extension.rewards, compare: true, added: true, Language.GetString("ui_rewards"), large: false, forceNonEquipment: false, select: false, null, null, -1, Language.GetString("ui_collect"));
			itemListWindow.DESTROYED.AddListener(OnCompleteDungeonTransitionToggleCheck);
			if (_dungeon.extension.reRunDungeon)
			{
				GameData.instance.main.loadingWindow.transform.SetParent(itemListWindow.transform.parent);
				GameData.instance.main.loadingWindow.transform.SetAsFirstSibling();
				GameData.instance.main.loadingWindow.GetComponent<Canvas>().overrideSorting = false;
			}
		}
	}

	private void OnCompleteDungeonTransitionComplete()
	{
		if (_dungeon.extension.rewards == null)
		{
			OnCompleteDungeonTransitionToggleCheck();
		}
	}

	private void OnCompleteDungeonTransitionToggleCheck(object e = null)
	{
		if (_dungeon == null)
		{
			return;
		}
		int dungeonType = _dungeon.type;
		bool dungeonDefeat = _dungeon.extension.defeated;
		bool reRunDungeon = _dungeon.extension.reRunDungeon;
		D.Log("all", "OnCompleteDungeonTransitionToggleCheck dungeonRerun: " + reRunDungeon);
		GameData.instance.main.mainCamera.backgroundColor = Color.black;
		if (!dungeonDefeat)
		{
			GameData.instance.PROJECT.character.GetCurrentBoosters();
		}
		if (reRunDungeon)
		{
			Dungeon dungeon = _dungeon;
			_character.RerunDungeon(dungeon);
			clearDungeon();
			return;
		}
		clearDungeon();
		if (dungeonDefeat && GameData.instance.SAVE_STATE.defeatAdvices)
		{
			GameData.instance.windowGenerator.NewDialogPopup(DialogBook.Lookup(_randomAdviceLink)).DESTROYED.AddListener(delegate(object z)
			{
				OnZoneCompleteWindowClosed(z, dungeonType, dungeonDefeat);
			});
		}
		else if (dungeonType == 1 && _lastZone != -1 && _lastZone < GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID())
		{
			GameData.instance.windowGenerator.NewZoneCompletedWindow(GameData.instance.PROJECT.character.zones.getHighestCompletedZoneRef()).DESTROYED.AddListener(delegate(object z)
			{
				OnZoneCompleteWindowClosed(z, dungeonType, dungeonDefeat);
			});
		}
		else
		{
			ShowAfterZoneCompleted(dungeonType, dungeonDefeat);
		}
	}

	private void OnZoneCompleteWindowClosed(object e, int dungeonType, bool dungeonDefeat)
	{
		(e as WindowsMain).DESTROYED.RemoveListener(delegate(object z)
		{
			OnZoneCompleteWindowClosed(z, dungeonType, dungeonDefeat);
		});
		ShowAfterZoneCompleted(dungeonType, dungeonDefeat);
	}

	private void ShowAfterZoneCompleted(int dungeonType, bool dungeonDefeat)
	{
		bool flag = checkTutorial();
		bool flag2 = CheckCharacter();
		if (GameData.instance.lastNode != null && GameData.instance.PROJECT.character.zones.nodeIsCompleted(GameData.instance.lastNode))
		{
			GameData.instance.zoneWindowTween = true;
		}
		if (GameData.instance.tutorialManager != null && !GameData.instance.tutorialManager.hasPopup && !flag && !flag2)
		{
			switch (dungeonType)
			{
			case 1:
				ShowZoneWindow(dungeonDefeat);
				break;
			case 2:
				ShowRaidWindow();
				break;
			case 3:
				ShowRiftWindow();
				break;
			case 4:
				ShowGvEWindow(null, GvEEventZoneNodeWindow.RECENT_NODE);
				GvEEventZoneNodeWindow.RECENT_NODE = -1;
				break;
			}
		}
	}

	private void OnEnterInstance(BaseEvent e)
	{
		GameData.instance.SAVE_STATE.isComingFromDungeon = false;
		DALCEvent dALCEvent = e as DALCEvent;
		SFSObject sfsob = dALCEvent.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		InstanceRef instanceRefFromSFSObject = Instance.GetInstanceRefFromSFSObject(sfsob);
		GameData.instance.audioManager.PlayMusic(instanceRefFromSFSObject.musicRef);
		string typeName = instanceRefFromSFSObject.typeName;
		string currentSceneName = null;
		if (_instance != null)
		{
			currentSceneName = _instance.instanceRef.typeName;
		}
		_currentTransitionScreen = GameData.instance.windowGenerator.NewTransitionScreen(typeName, currentSceneName, OnEnterInstanceTransitionComplete, delegate
		{
			OnEnterInstanceTransitionToggle(sfsob);
		});
	}

	private void OnCharacterAchievementUpdate(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		GameData.instance.PROJECT.character.characterAchievements = CharacterAchievements.fromSFSObject(sfsob);
	}

	private void OnEnterInstanceTransitionToggle(SFSObject sfsob = null)
	{
		if (sfsob != null)
		{
			Instance instance = Instance.FromSFSObject(sfsob);
			CheckInitialLogin();
			_instance = instance;
			_instance.AddedToStage(SceneManager.GetSceneByName(instance.instanceRef.typeName));
		}
	}

	private void OnEnterInstanceTransitionComplete()
	{
	}

	private void CheckInitialLogin()
	{
		if (GameData.instance.main.BACKGROUND != null)
		{
			GameData.instance.main.HideBackground();
			GameData.instance.windowGenerator.ShowCurrencies(show: true);
			AddMenuInterface();
			GameData.instance.windowGenerator.ShowChatUI();
			CheckCharacter();
		}
	}

	public void DoSendRequestByID(int charID)
	{
		if (_character != null)
		{
			GameData.instance.main.ShowLoading();
			FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnSendRequest);
			FriendDALC.instance.doSendRequestByID(charID);
		}
	}

	public void SetCurrentFriendInviteWindow(FriendInviteWindow pwindow)
	{
		currentFriendInviteWindow = pwindow;
	}

	public void DoSendRequestByName(string playerName)
	{
		if (_character != null)
		{
			GameData.instance.main.ShowLoading();
			FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnSendRequest);
			FriendDALC.instance.doSendRequestByName(playerName);
		}
	}

	private void OnSendRequest(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnSendRequest);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey(ServerConstants.TOTAL_CHARACTERS_NAMES))
		{
			ISFSArray sFSArray = sfsob.GetSFSArray(ServerConstants.CHARACTERS_DATA);
			List<CharacterHeroTagData> list = new List<CharacterHeroTagData>();
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				list.Add(CharacterHeroTagData.FromSFSObject(sFSObject));
			}
			GameData.instance.windowGenerator.NewCharactersSearchListWindow(list, 0, showSelect: true, OnSelectFrindFromCharacterSearch);
		}
		else if (!GameData.instance.windowGenerator.HasDialogByClass(typeof(FriendRecommendWindow)))
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("ui_friend_request_sent"));
		}
	}

	private void OnSelectFrindFromCharacterSearch(string name)
	{
		currentFriendInviteWindow.playerNameTxt.text = name;
	}

	public void DoAcceptRequest(int charID)
	{
		if (_character != null)
		{
			GameData.instance.main.ShowLoading();
			FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnAcceptRequest);
			FriendDALC.instance.doAcceptRequest(charID);
		}
	}

	private void OnAcceptRequest(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnAcceptRequest);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		FriendData friendData = FriendData.fromSFSObject(sfsob);
		_character.removeRequestData(friendData.characterData.charID);
		_character.addFriendData(friendData);
		_character.updateAchievements();
		if (!sfsob.ContainsKey("cha141"))
		{
			return;
		}
		foreach (SFSObject item in sfsob.GetSFSArray("cha141"))
		{
			friendData = FriendData.fromSFSObject(item);
			_character.removeRequestData(friendData.characterData.charID);
			_character.addFriendData(friendData);
		}
	}

	public void DoDenyRequest(int charID)
	{
		if (_character != null)
		{
			GameData.instance.main.ShowLoading();
			FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnDenyRequest);
			FriendDALC.instance.doDenyRequest(charID);
		}
	}

	private void OnDenyRequest(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnDenyRequest);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("cha1");
		_character.removeRequestData(@int);
	}

	public void DoDenyAllRequests()
	{
		GameData.instance.main.ShowLoading();
		FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnDenyAllRequest);
		FriendDALC.instance.doDenyAllRequests();
	}

	private void OnDenyAllRequest(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnDenyAllRequest);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<RequestData> requests = RequestData.listFromSFSObject(sfsob);
		_character.requests = requests;
	}

	public void DoGuildInviteAccept(int guildID)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGuildInviteAccept);
		GuildDALC.instance.doInviteAccept(guildID);
	}

	private void OnGuildInviteAccept(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnGuildInviteAccept);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		CharacterGuildData characterGuildData = CharacterGuildData.fromSFSObject(sfsob);
		_character.guildData = characterGuildData;
		_character.updateAchievements();
		GameDALC.instance.doPlayersOnline();
		KongregateAnalytics.TrackCPEEvent("kong_join_guild");
		GameData.instance.PROJECT.character.RemoveGuildInvite(characterGuildData.id);
	}

	public void DoGuildInviteDecline(int guildID)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(10), OnGuildInviteDecline);
		GuildDALC.instance.doInviteDecline(guildID);
	}

	private void OnGuildInviteDecline(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(10), OnGuildInviteDecline);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("gui0");
		GuildInvitesWindow guildInvitesWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildInvitesWindow)) as GuildInvitesWindow;
		if (guildInvitesWindow != null)
		{
			guildInvitesWindow.RemoveGuild(@int);
		}
		GameData.instance.PROJECT.character.RemoveGuildInvite(@int);
	}

	public void DoGuildInviteByID(int charID)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnGuildInvite);
		GuildDALC.instance.doInviteSendByID(charID);
	}

	public void DoGuildInviteByName(string name)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnGuildInvite);
		GuildDALC.instance.doInviteSendByName(name);
	}

	public void SetCurrentGuildInviteWindow(GuildInviteWindow pWindow)
	{
		currentGuildInviteWindow = pWindow;
	}

	private void OnGuildInvite(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnGuildInvite);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey(ServerConstants.TOTAL_CHARACTERS_NAMES))
		{
			ISFSArray sFSArray = sfsob.GetSFSArray(ServerConstants.CHARACTERS_DATA);
			List<CharacterHeroTagData> list = new List<CharacterHeroTagData>();
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				list.Add(CharacterHeroTagData.FromSFSObject(sFSObject));
			}
			GameData.instance.windowGenerator.NewCharactersSearchListWindow(list, 0, showSelect: true, OnSelectCharacterSearchGuildWindow);
		}
		else
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("ui_guild_invite_success"));
		}
	}

	private void OnSelectCharacterSearchGuildWindow(string name)
	{
		if (currentGuildInviteWindow != null)
		{
			currentGuildInviteWindow.playerNameTxt.text = name;
		}
	}

	public void DoGuildApply(int guildID)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(13), OnGuildApply);
		GuildDALC.instance.doApplicationSend(guildID);
	}

	private void OnGuildApply(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(13), OnGuildApply);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		CharacterGuildData characterGuildData = CharacterGuildData.fromSFSObject(sfsob);
		if (characterGuildData != null)
		{
			_character.guildData = characterGuildData;
			_character.updateAchievements();
			GameDALC.instance.doPlayersOnline();
			KongregateAnalytics.TrackCPEEvent("kong_join_guild");
		}
		else if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildApplicationWindow)) == null)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("ui_guild_apply_success"));
		}
	}

	public void DoFriendRemoveConfirm(int charID)
	{
		FriendData friendData = _character.getFriendData(charID);
		if (friendData != null)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_friend_remove_confirm", new string[1] { friendData.characterData.parsedName }), null, null, delegate
			{
				DoFriendRemove(charID);
			});
		}
	}

	public void DoFriendRemove(int charID)
	{
		if (_character != null)
		{
			GameData.instance.main.ShowLoading();
			FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnFriendRemove);
			FriendDALC.instance.doFriendRemove(charID);
		}
	}

	private void OnFriendRemove(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnFriendRemove);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("cha1");
		_character.removeFriendData(@int);
	}

	public void DoDailyQuestLoot(DailyQuestData questData)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(14), OnDailyQuestLoot);
		CharacterDALC.instance.doDailyQuestLoot(questData);
	}

	private void OnDailyQuestLoot(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(14), OnDailyQuestLoot);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		DailyQuests dailyQuests = DailyQuests.fromSFSObject(sfsob);
		_character.addItems(list);
		_character.dailyQuests = dailyQuests;
		KongregateAnalytics.checkEconomyTransaction("Bounty Reward", null, list, sfsob, "Bounty", 2);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		_character.tutorial.SetState(48);
		_character.tutorial.SetState(49);
		CheckTutorialChanges();
	}

	public void DoCharacterAchievementLoot(CharacterAchievementData achievementData)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(72), OnCharacterAchievementLoot);
		CharacterDALC.instance.doCharacterAchievementLoot(achievementData);
	}

	private void OnCharacterAchievementLoot(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(72), OnCharacterAchievementLoot);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		CharacterAchievements characterAchievements = CharacterAchievements.fromSFSObject(sfsob);
		_character.addItems(items);
		_character.characterAchievements = characterAchievements;
		AchievementsWindow achievementsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AchievementsWindow)) as AchievementsWindow;
		if (achievementsWindow != null)
		{
			achievementsWindow.OnUpdateAchievements();
		}
		GameData.instance.windowGenerator.ShowItems(items, compare: true, added: true);
	}

	public void DoCharacterAchievementsMultipleLoot(List<CharacterAchievementData> achievementsData)
	{
		GameData.instance.main.ShowLoading();
		int[] array = new int[achievementsData.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = achievementsData[i].achievementRef.id;
		}
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(74), OnCharacterAchievementsMultipleLoot);
		CharacterDALC.instance.doCharacterAchievementsMultipleLoot(array);
	}

	private void OnCharacterAchievementsMultipleLoot(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(74), OnCharacterAchievementsMultipleLoot);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		CharacterAchievements characterAchievements = CharacterAchievements.fromSFSObject(sfsob);
		_character.addItems(items);
		_character.characterAchievements = characterAchievements;
		AchievementsWindow achievementsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AchievementsWindow)) as AchievementsWindow;
		if (achievementsWindow != null)
		{
			achievementsWindow.OnUpdateAchievements();
		}
		GameData.instance.windowGenerator.ShowItems(items, compare: true, added: true);
	}

	public void DoCharacterAchievementCheckLoot(CharacterAchievementData achievementData)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(73), OnCharacterAchievementCheckLoot);
		CharacterDALC.instance.doCharacterAchievementCheckLoot(achievementData);
	}

	private void OnCharacterAchievementCheckLoot(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(73), OnCharacterAchievementCheckLoot);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		GameData.instance.windowGenerator.ShowItems(items, compare: true, added: false, null, large: false, forceNonEquipment: true);
	}

	public void DoDuelSend(int charID, bool online = false)
	{
		GameData.instance.main.ShowLoading();
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_SEND), OnDuelSend);
		PvPDALC.instance.doDuelSend(charID, online);
	}

	private void OnDuelSend(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_SEND), OnDuelSend);
		SFSObject sfsob = obj.sfsob;
		int @int = sfsob.GetInt("cha1");
		if (sfsob.ContainsKey("err0"))
		{
			int int2 = sfsob.GetInt("err0");
			if (int2 == 102)
			{
				DoDuelServerChangeConfirm(@int);
			}
			else
			{
				GameData.instance.windowGenerator.ShowErrorCode(int2);
			}
		}
	}

	private void DoDuelServerChangeConfirm(int charID)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(GameData.instance.windowGenerator.GetErrorName(), Language.GetString("ui_server_confirm", new string[1] { Language.GetString("ui_duel") }), null, null, delegate
		{
			OnDuelServerChangeConfirm(charID);
		}, null, null, 10);
	}

	private void OnDuelServerChangeConfirm(int charID)
	{
		DoDuelGetServerInstance(charID);
	}

	private void DoDuelGetServerInstance(int charID)
	{
		StartDuelTimer();
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(25), OnDuelGetServerInstance);
		CharacterDALC.instance.doGetServerInstance(charID);
	}

	private void OnDuelGetServerInstance(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CancelDuelGetServerInstace();
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("serv1");
		if (utfString == ServerExtension.instance.serverInstanceID)
		{
			CharacterProfileWindow characterProfileWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(CharacterProfileWindow)) as CharacterProfileWindow;
			if (characterProfileWindow != null)
			{
				characterProfileWindow.DoGetOnline();
			}
			GameData.instance.windowGenerator.ShowErrorCode(39);
		}
		else
		{
			ServerExtension.instance.Disconnect(null, null, relog: true, utfString, @int);
		}
	}

	public void DoBrawlJoin(int index, bool invited = false)
	{
		if (!(GameData.instance.PROJECT.dungeon != null) && !(GameData.instance.PROJECT.battle != null) && GameData.instance.windowGenerator.GetDialogByClass(typeof(BrawlRoomWindow)) == null)
		{
			GameData.instance.main.ShowLoading();
			BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnBrawlJoin);
			BrawlDALC.instance.doJoin(index, invited);
		}
	}

	private void OnBrawlJoin(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnBrawlJoin);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		BrawlRoom brawlRoom = BrawlRoom.fromSFSObject(sfsob);
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		GameData.instance.windowGenerator.NewBrawlRoomWindow(brawlRoom);
	}

	public void DoBrawlRejoin()
	{
		if (!(GameData.instance.PROJECT.dungeon != null) && !GameData.instance.PROJECT.battle && !GameData.instance.windowGenerator.HasDialogByClass(typeof(BrawlRoomWindow)))
		{
			GameData.instance.main.ShowLoading();
			BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(14), OnBrawlRejoin);
			BrawlDALC.instance.doRejoin();
		}
	}

	private void OnBrawlRejoin(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(14), OnBrawlRejoin);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		BrawlRoom brawlRoom = BrawlRoom.fromSFSObject(sfsob);
		GameData.instance.windowGenerator.ClearAllWindows(null, removeChat: false);
		GameData.instance.windowGenerator.NewBrawlRoomWindow(brawlRoom);
	}

	private void CancelDuelGetServerInstace()
	{
		StopDuelTimer();
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(25), OnDuelGetServerInstance);
	}

	public void StartDuelTimer()
	{
		if (_duelTimer != null)
		{
			GameData.instance.main.coroutineTimer.RestartTimer(_duelTimer);
		}
		else
		{
			_duelTimer = GameData.instance.main.coroutineTimer.AddTimer(null, 10000f, OnDuelTimer);
		}
	}

	public void StopDuelTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _duelTimer);
	}

	private void OnDuelTimer()
	{
		CancelDuelGetServerInstace();
		CharacterProfileWindow characterProfileWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(CharacterProfileWindow)) as CharacterProfileWindow;
		if ((bool)characterProfileWindow)
		{
			characterProfileWindow.DoGetOnline();
		}
		GameData.instance.windowGenerator.ShowErrorCode(39);
	}

	public void DoDuelAccept(int charID)
	{
		GameData.instance.main.ShowLoading();
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_ACCEPT), OnDuelAccept);
		PvPDALC.instance.doDuelAccept(charID);
	}

	private void OnDuelAccept(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_ACCEPT), OnDuelAccept);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void DoDuelDecline(int charID, bool load = true)
	{
		if (load)
		{
			GameData.instance.main.ShowLoading();
		}
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_DECLINE), OnDuelDecline);
		PvPDALC.instance.doDuelDecline(charID);
	}

	private void OnDuelDecline(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_DECLINE), OnDuelDecline);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void DoDuelCancel(int charID)
	{
		GameData.instance.main.ShowLoading();
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_CANCEL), OnDuelCancel);
		PvPDALC.instance.doDuelCancel(charID);
	}

	private void OnDuelCancel(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_CANCEL), OnDuelCancel);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void doLogoutConfirm()
	{
		if (AppInfo.allowLogout)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_logout"), Language.GetString("ui_logout_confirm"), null, null, delegate
			{
				OnLogoutConfirm();
			});
		}
	}

	private void OnLogoutConfirm()
	{
		GameData.instance.main.Logout(relog: false, reloadXMLfiles: false);
	}

	public void SetBrawlRejoin(bool v)
	{
		_brawlRejoin = v;
	}

	public void ToggleGuildHallEditMode()
	{
		_guildHallEditMode = !_guildHallEditMode;
		_menuInterface.gameObject.SetActive(!_guildHallEditMode);
		if (_instance != null)
		{
			_instance.instanceInterface.gameObject.SetActive(!_guildHallEditMode);
			_instance.SetMouse(!_guildHallEditMode);
			_instance.SetGuildHallInterface(_guildHallEditMode);
		}
		if ((bool)_notification)
		{
			_notification.gameObject.SetActive(!_guildHallEditMode);
		}
		if (_guildHallEditMode)
		{
			if (_instance.instanceGuildHallInterface != null && _instance.instanceGuildHallInterface.editBar != null)
			{
				_instance.instanceGuildHallInterface.editBar.UpdateSelectedType();
			}
			GameData.instance.windowGenerator.ClearAllWindows();
			GameData.instance.windowGenerator.ShowCurrencies(show: false);
			GameData.instance.windowGenerator.SetChatUIVisibility(visible: false);
		}
		else
		{
			GameData.instance.windowGenerator.ShowCurrencies(show: true);
			GameData.instance.windowGenerator.SetChatUIVisibility(visible: true);
		}
	}

	public bool DoDailyFishingRewardCheck()
	{
		if (GameData.instance.tutorialManager != null && GameData.instance.tutorialManager.hasPopup)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.HasDialogByClass(typeof(DialogPopup)))
		{
			return false;
		}
		DateTime date = ServerExtension.instance.GetDate();
		DateTime dailyFishingDate = _character.dailyFishingDate;
		if (date.Year > dailyFishingDate.Year || date.Month > dailyFishingDate.Month || date.Day > dailyFishingDate.Day)
		{
			DoDailyFishingReward();
			return true;
		}
		return false;
	}

	public void DoDailyFishingReward(object parent = null)
	{
		DialogPopup dialogPopup = GameData.instance.windowGenerator.GetDialogByClass(typeof(DialogPopup)) as DialogPopup;
		if (!GameData.instance.tutorialManager.hasPopup && (!(dialogPopup != null) || dialogPopup.cleared) && !DoDialogCheck(VariableBook.fishingDailyDialog, delegate(object theParent)
		{
			DoDailyFishingReward(theParent);
		}, parent))
		{
			GameData.instance.main.ShowLoading();
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(43), OnDailyFishingReward);
			CharacterDALC.instance.doDailyFishingReward();
		}
	}

	public void OnDailyFishingReward(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(43), OnDailyFishingReward);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("cha96"));
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		_character.dailyFishingDate = dateFromString;
		_character.addItems(list);
		KongregateAnalytics.checkEconomyTransaction("Daily Login Reward", null, list, sfsob, "Daily Login", 2);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true).DESTROYED.AddListener(OnDailyFishingRewardClosed);
	}

	private void OnDailyFishingRewardClosed(object e)
	{
		(e as ItemListWindow).DESTROYED.RemoveListener(OnDailyFishingRewardClosed);
		FishingWindow fishingWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(FishingWindow)) as FishingWindow;
		if (fishingWindow != null)
		{
			fishingWindow.CheckTutorial();
		}
	}

	public void ToggleFishingMode()
	{
		_fishingMode = !_fishingMode;
		_menuInterface.gameObject.SetActive(!_fishingMode);
		if (_instance != null)
		{
			_instance.instanceInterface.gameObject.SetActive(!_fishingMode);
			_instance.SetMouse(!_fishingMode);
			_instance.SetMovement(!_fishingMode);
			_instance.SetFishingInterface(_fishingMode);
		}
		if (_notification != null)
		{
			_notification.gameObject.SetActive(!_fishingMode);
		}
		if (_fishingMode)
		{
			GameData.instance.windowGenerator.ClearAllWindows(typeof(InstanceFishingInterface));
			GameData.instance.windowGenerator.ShowCurrencies(show: false);
			GameData.instance.windowGenerator.SetChatUIVisibility(visible: false);
		}
		else
		{
			GameData.instance.windowGenerator.ShowCurrencies(show: true);
			GameData.instance.windowGenerator.SetChatUIVisibility(visible: true);
		}
	}

	public bool CheckGameRequirement(int type)
	{
		GameRequirement gameRequirement = VariableBook.GetGameRequirement(type);
		if (gameRequirement == null)
		{
			return true;
		}
		string requirementsText = gameRequirement.GetRequirementsText();
		if (requirementsText != null)
		{
			DialogRef dialogLocked = gameRequirement.GetDialogLocked();
			if (dialogLocked != null)
			{
				GameData.instance.windowGenerator.NewDialogPopup(dialogLocked);
			}
			else
			{
				GameData.instance.windowGenerator.ShowError(requirementsText);
			}
			return false;
		}
		return true;
	}

	public void clearZoneWindow()
	{
		throw new Exception("Error --> CONTROL.");
	}

	public void clearCharacter()
	{
		if (_character != null)
		{
			KongregateAnalytics.updateCommonFields();
		}
	}

	public void ShowZoneWindow(bool defeated = false)
	{
		if (_zoneWindow == null)
		{
			_zoneWindow = GameData.instance.windowGenerator.NewQuestWindow();
		}
		_zoneWindow.SetDefeated(defeated);
	}

	public void ShowPvPWindow()
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(PvPEventWindow)) == null)
		{
			GameData.instance.windowGenerator.NewPvPEventWindow(new int[2] { 0, 3 });
		}
	}

	public void ShowFusionWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(FusionWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogFusion, delegate(object theParent)
		{
			ShowFusionWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewFusionWindow(parent as GameObject);
		}
	}

	public void ShowMountWindow(object parent = null)
	{
	}

	public void ShowFishingWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(FishingWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogFishing, delegate(object theParent)
		{
			ShowFishingWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewFishingWindow(parent as GameObject);
		}
	}

	public void ShowFishingShop(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(FishingShopWindow)) == null && !DoDialogCheck(VariableBook.fishingShopDialog, delegate(object theParent)
		{
			ShowFishingShop(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewFishingShopWindow(parent as GameObject);
		}
	}

	public void ShowEventSalesShop(string origin, object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(EventSalesShopWindow)) == null && !DoDialogCheck(VariableBook.eventSalesShopDialog, delegate(object theParent)
		{
			ShowEventSalesShop(origin, theParent);
		}, parent))
		{
			if (EventSalesShopBook.HasEventActive)
			{
				GameData.instance.windowGenerator.NewEventSalesShopWindow(origin, parent as GameObject);
			}
			else
			{
				GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_OK, Language.GetString("error_name"), Language.GetString("ui_event_sales_shop_disabled"));
			}
		}
	}

	public void ShowFishingMode(object parent = null)
	{
		ToggleFishingMode();
	}

	public void ShowRiftWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(RiftEventWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogRifts, delegate(object theParent)
		{
			ShowRiftWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewRiftEventWindow(parent as GameObject);
		}
	}

	public void ShowGauntletWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GauntletEventWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogGauntlet, delegate(object theParent)
		{
			ShowGauntletWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewGauntletEventWindow(parent as GameObject);
		}
	}

	public void ShowGvGWindow(object parent = null)
	{
		if (_character.guildData != null)
		{
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GvGEventWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogGvG, delegate(object theParent)
			{
				ShowGvGWindow(theParent);
			}, parent))
			{
				GameData.instance.windowGenerator.NewGvGEventWindow(parent as GameObject);
			}
		}
		else
		{
			ShowGuildWindow();
		}
	}

	public void ShowGvEWindow(object parent = null, int nodeID = -1)
	{
		if (_character.guildData != null)
		{
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GvEEventWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogGvE, delegate(object theParent)
			{
				ShowGvEWindow(theParent);
			}, parent))
			{
				GameData.instance.windowGenerator.NewGvEEventWindow(nodeID);
			}
		}
		else
		{
			ShowGuildWindow();
		}
	}

	public void ShowInvasionWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(InvasionEventWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogInvasion, delegate(object theParent)
		{
			ShowInvasionWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewInvasionEventWindow(parent as GameObject);
		}
	}

	public void ShowGuildWindow(int tab = 0, bool scroll = true)
	{
		if (_character.guildData != null)
		{
			if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildWindow)) == null)
			{
				GameData.instance.windowGenerator.NewGuildWindow();
			}
		}
		else if (GameData.instance.windowGenerator.GetDialogByClass(typeof(GuildlessWindow)) == null)
		{
			GameData.instance.windowGenerator.NewGuildlessWindow();
		}
	}

	public void ShowBrawlWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(BrawlWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogBrawl, delegate(object theParent)
		{
			ShowBrawlWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewBrawlWindow(parent as GameObject);
		}
	}

	public void ShowRaidWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(RaidWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogRaid, delegate(object theParent)
		{
			ShowRaidWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewRaidWindow(parent as GameObject);
		}
	}

	public void ShowShopWindow()
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(ShopWindow)) == null)
		{
			_ = AppInfo.platform;
			GameData.instance.windowGenerator.NewShopWindow(new int[4] { 0, 1, 2, 3 }, ShopWindow.TAB_FEATURED);
		}
	}

	public void ShowFamiliarStableWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(FamiliarStableWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogFamiliarStable, delegate(object theParent)
		{
			ShowFamiliarStableWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewFamiliarStableWindow(parent as GameObject);
		}
	}

	public void ShowEnchantsWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(EnchantsWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogEnchants, delegate(object theParent)
		{
			ShowEnchantsWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewEnchantsWindow(_character.enchants, changeable: true, parent as GameObject);
		}
	}

	public void ShowAugmentsWindow(object parent = null)
	{
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentSelectWindow)) == null && !DoDialogCheck(VariableBook.tutorialDialogEnchants, delegate(object theParent)
		{
			ShowEnchantsWindow(theParent);
		}, parent))
		{
			GameData.instance.windowGenerator.NewAugmentSelectWindow(_character.augments, changeable: true, null, -1, parent as GameObject);
		}
	}

	private bool DoDialogCheck(string link, Action<object> func, object parent = null)
	{
		DialogRef dialogRef = DialogBook.Lookup(link);
		if (dialogRef != null && !dialogRef.seen)
		{
			List<object> list = new List<object>();
			list.Add(func);
			list.Add(parent);
			GameData.instance.windowGenerator.NewDialogPopup(dialogRef, list).CLEAR.AddListener(OnDialogCheck);
			return true;
		}
		return false;
	}

	private void OnDialogCheck(object e)
	{
		DialogPopup obj = e as DialogPopup;
		obj.CLEAR.RemoveListener(OnDialogCheck);
		List<object> obj2 = obj.data as List<object>;
		Action<object> action = obj2[0] as Action<object>;
		obj2.RemoveAt(0);
		object obj3 = obj2[0];
		obj2.RemoveAt(0);
		action(obj3);
	}

	private void createContainers()
	{
	}

	private void clearContainers()
	{
	}

	public void ShowPossibleDungeonLoot(int battleType, int zoneID, int nodeID, string link, int difficultyID)
	{
		D.Log("all", "ShowPossibleDungeonLoot");
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(65), OnGetPossibleDungeonLoot);
		CharacterDALC.instance.doGetPossibleBattleLoot(battleType, zoneID, nodeID, link, difficultyID);
	}

	private void OnGetPossibleDungeonLoot(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(65), OnGetPossibleDungeonLoot);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError("Project::onGetPossibleDungeonLoot Error err0");
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		GameData.instance.windowGenerator.ShowItems(items, compare: true, added: false, Language.GetString("ui_possible_items"), large: true, forceNonEquipment: true, Language.GetString("ui_possible_items_help"));
	}

	public void PauseDungeon()
	{
		if (!(_dungeon == null))
		{
			_gameIsPaused = false;
			_autopilot = GameData.instance.PROJECT.character.autoPilot;
			MenuInterfaceAutoPilotTile menuInterfaceAutoPilotTile = GameData.instance.windowGenerator.GetBattleUI(typeof(MenuInterfaceAutoPilotTile)) as MenuInterfaceAutoPilotTile;
			if (_autopilot)
			{
				menuInterfaceAutoPilotTile.ForceClick();
			}
			menuInterfaceAutoPilotTile.AddGrayscale();
			_gameIsPaused = true;
		}
	}

	public void ResumeDungeon()
	{
		_gameIsPaused = false;
		if (!(_dungeon == null))
		{
			MenuInterfaceAutoPilotTile menuInterfaceAutoPilotTile = GameData.instance.windowGenerator.GetBattleUI(typeof(MenuInterfaceAutoPilotTile)) as MenuInterfaceAutoPilotTile;
			if (_autopilot)
			{
				menuInterfaceAutoPilotTile.ForceClick();
			}
			menuInterfaceAutoPilotTile.ClearGrayscale();
		}
	}

	public void createListeners()
	{
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), onEnterBattle);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), onNotification);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), onEnterDungeon);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstance);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(16), OnCharacterAchievementUpdate);
		FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnReceiveRequest);
		FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnFriendAdded);
		FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnFriendRemoved);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(10), OnPlayerUpdate);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnPlayerLogin);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnPlayerLogout);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnPlayersOnline);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(14), OnReconnectDungeon);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnDailyQuestsUpdate);
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(12), OnSaveTeam);
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(23), OnAdjustItems);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnGuildChatMessage);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(20), OnGuildPlayerAdded);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(21), OnGuildPlayerRemoved);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(23), OnGuildChange);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(27), OnGuildRankUpdate);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(28), OnGuildPermissionsUpdate);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(37), OnGuildPerksUpdate);
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnPrivateMessage);
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_TARGET), OnDuelTarget);
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_SOURCE), OnDuelSource);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(17), OnHeroFrozen);
	}

	public void SetSpecialListeners()
	{
		GameData.instance.windowGenerator.CHANGE.AddListener(OnDialogChange);
	}

	public void clearListeners()
	{
		GameData.instance.windowGenerator.CHANGE.RemoveListener(OnDialogChange);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), onEnterBattle);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), onNotification);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), onEnterDungeon);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstance);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(16), OnCharacterAchievementUpdate);
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnReceiveRequest);
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnFriendAdded);
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnFriendRemoved);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(10), OnPlayerUpdate);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnPlayerLogin);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnPlayerLogout);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnPlayersOnline);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(14), OnReconnectDungeon);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnDailyQuestsUpdate);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(12), OnSaveTeam);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(23), OnAdjustItems);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnGuildChatMessage);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnGuildPlayerAdded);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(21), OnGuildPlayerRemoved);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(23), OnGuildChange);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(27), OnGuildRankUpdate);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(28), OnGuildPermissionsUpdate);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(37), OnGuildPerksUpdate);
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnPrivateMessage);
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_TARGET), OnDuelTarget);
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.DUEL_SOURCE), OnDuelSource);
	}

	public void clear()
	{
		if (_character != null)
		{
			_character.RemoveListener("GUILD_CHANGE", OnCharacterGuildChange);
		}
		clearListeners();
	}

	public void remove()
	{
		clear();
	}

	public void UpdateTransitionScreenProgress(float progress)
	{
		if (_currentTransitionScreen != null)
		{
			_currentTransitionScreen.IncreaseProgress(progress);
		}
	}
}
