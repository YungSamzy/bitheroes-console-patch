using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.team;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gve;

public class GvEEventWindow : WindowsMain
{
	private const string BLANK = "-";

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI costNameValueTxt;

	public TextMeshProUGUI characterNameTxt;

	public TextMeshProUGUI characterRankNameTxt;

	public TextMeshProUGUI characterRankTxt;

	public TextMeshProUGUI characterPointsNameTxt;

	public TextMeshProUGUI characterPointsTxt;

	public TextMeshProUGUI guildNameTxt;

	public TextMeshProUGUI guildRankNameTxt;

	public TextMeshProUGUI guildRankTxt;

	public TextMeshProUGUI guildPointsNameTxt;

	public TextMeshProUGUI guildPointsTxt;

	public Button playBtn;

	public Button lootBtn;

	public Button characterLeaderboardBtn;

	public Button characterRewardsBtn;

	public Button guildLeaderboardBtn;

	public Button guildRewardsBtn;

	public Button teamBtn;

	public Button helpBtn;

	public Button refreshBtn;

	public Button consumablesBtn;

	public Image guildBtn;

	public Image badgesBtn;

	public Image costDropdown;

	public CurrencyBarFill currencyBarFill;

	private int _nodeID;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private GvEEventRef _eventRef;

	private TimeBarColor timeBar;

	private int characterRank;

	private int characterPoints;

	private int guildRank;

	private int guildPoints;

	private int _zone;

	private Transform window;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private bool costEnabled;

	private List<MyDropdownItemModel> _bonuses = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedBonus;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int nodeID = -1)
	{
		_nodeID = nodeID;
		topperTxt.text = EventRef.getEventTypeName(7);
		eventTxt.text = Language.GetString("event_end", new string[1] { EventRef.getEventTypeName(7) });
		costNameTxt.text = Language.GetString("ui_cost");
		characterNameTxt.text = Language.GetString("ui_player");
		characterRankNameTxt.text = Language.GetString("ui_rank");
		characterPointsNameTxt.text = Language.GetString("ui_points");
		guildNameTxt.text = Language.GetString("ui_guild");
		guildRankNameTxt.text = Language.GetString("ui_rank");
		guildPointsNameTxt.text = Language.GetString("ui_points");
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		lootBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
		characterLeaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		characterRewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		guildLeaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		guildRewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		teamBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_team");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		lootBtn.gameObject.SetActive(value: false);
		timeBar = GetComponentInChildren<TimeBarColor>();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		currencyBarFill.Init();
		SetStats();
		SetLootID();
		UpdateEvent();
		UpdateButtons();
		CreateWindow(closeWord: false, "", scroll: true, stayUp: true);
		if (_nodeID >= 0 && _eventRef != null)
		{
			DoShow();
			StartCoroutine(WaitTillEndOfFrame());
		}
		else
		{
			DoShow();
		}
		DoEventLootCheck();
		DoEventStats();
		ListenForBack(OnClose);
		ListenForForward(OnPlayBtn);
	}

	private IEnumerator WaitTillEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		ShowZoneWindow(_nodeID);
	}

	private void OnInventoryChange()
	{
		UpdateButtons();
	}

	public void UpdateBonusDropdown()
	{
		_bonuses.Clear();
		int gvEEventBonus = GameData.instance.SAVE_STATE.GetGvEEventBonus(GameData.instance.PROJECT.character.id);
		if (_eventRef != null)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.badges)
			};
			_bonuses.Add(myDropdownItemModel);
			_selectedBonus = myDropdownItemModel;
			for (int i = 0; i < GvEEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = GvEEventBook.LookupBonus(i);
				if (currencyBonusRef != null)
				{
					int currencyCost = _eventRef.GetCurrencyCost(currencyBonusRef);
					MyDropdownItemModel myDropdownItemModel2 = new MyDropdownItemModel
					{
						id = i,
						title = Util.NumberFormat(currencyCost),
						data = currencyBonusRef,
						btnHelp = true
					};
					_bonuses.Add(myDropdownItemModel2);
					if (currencyBonusRef.id == gvEEventBonus)
					{
						_selectedBonus = myDropdownItemModel2;
					}
				}
			}
		}
		costNameValueTxt.text = ((_selectedBonus != null) ? _selectedBonus.title : "");
		if (_eventRef == null)
		{
			UIUtil.LockImage(costDropdown, locked: true);
		}
		else
		{
			UIUtil.LockImage(costDropdown, locked: false);
		}
	}

	public void OnCostDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(10)));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetGvEEventBonus(), OnCostDropdownClicked);
		componentInChildren.Data.InsertItemsAtStart(_bonuses);
	}

	public void OnCostDropdownClicked(MyDropdownItemModel model)
	{
		int bonus = ((model.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetGvEEventBonus(GameData.instance.PROJECT.character.id, bonus);
		_selectedBonus = model;
		costNameValueTxt.text = _selectedBonus.title;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private int GetSelectedBonusID()
	{
		if (!(_selectedBonus.data is CurrencyBonusRef currencyBonusRef))
		{
			return -1;
		}
		return currencyBonusRef.id;
	}

	private void RestartRefreshTimer()
	{
		if (_refreshTimer != null)
		{
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		seconds = 10;
		_refreshTimer = OnRefreshTimer();
		StartCoroutine(_refreshTimer);
		Util.SetButton(refreshBtn, enabled: false);
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
	}

	private IEnumerator OnRefreshTimer()
	{
		yield return new WaitForSeconds(1f);
		seconds--;
		if (seconds <= 0)
		{
			Util.SetButton(refreshBtn);
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		else
		{
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
			_refreshTimer = OnRefreshTimer();
			StartCoroutine(_refreshTimer);
		}
	}

	private void DoEventLootCheck()
	{
		if (!_checkingLoot)
		{
			_checkingLoot = true;
			GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
			GvEDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
		GvEDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("eve0");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		SetLootID(@int);
		if (list.Count > 0)
		{
			GameData.instance.PROJECT.character.addItems(list);
			KongregateAnalytics.checkEconomyTransaction("GvE Event Reward", null, list, sfsob, "GvE Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
		else
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_event_loot"));
		}
	}

	private void SetLootID(int eventID = -1)
	{
		_lootEventID = eventID;
		if (lootBtn != null && lootBtn.gameObject != null)
		{
			lootBtn.gameObject.SetActive(_lootEventID >= 0);
		}
		if (playBtn != null && playBtn.gameObject != null)
		{
			playBtn.gameObject.SetActive(!lootBtn.gameObject.activeSelf);
		}
	}

	private void DoEventStats()
	{
		if (!_checkingStats)
		{
			SetStats();
			RestartRefreshTimer();
			if (_eventRef != null)
			{
				_checkingStats = true;
				Util.SetButton(playBtn, enabled: false);
				GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
				GvEDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		_checkingStats = false;
		DALCEvent obj = baseEvent as DALCEvent;
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
		if (_eventRef != null)
		{
			Util.SetButton(playBtn);
		}
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GvEEventCharacterData gvEEventCharacterData = GvEEventCharacterData.fromSFSObject(sfsob);
		SetStats(gvEEventCharacterData.rank, gvEEventCharacterData.points, gvEEventCharacterData.guildRank, gvEEventCharacterData.guildPoints);
		_zone = gvEEventCharacterData.zone;
	}

	private bool DoAllowEnter()
	{
		if (_eventRef == null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(27);
			return false;
		}
		if (GameData.instance.PROJECT.character.badges < GetBadgeCost())
		{
			GameData.instance.windowGenerator.ShowErrorCode(104);
			return false;
		}
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return false;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return false;
		}
		return true;
	}

	private void DoEventEnter()
	{
		if (DoAllowEnter())
		{
			ShowZoneWindow();
		}
	}

	private void ShowZoneWindow(int nodeID = -1)
	{
		GameData.instance.windowGenerator.NewGvEEventZoneWindow(_eventRef, nodeID, base.gameObject);
	}

	private void UpdateEvent()
	{
		_eventRef = GvEEventBook.GetCurrentEventRef();
		if (_eventRef == null)
		{
			Util.SetButton(playBtn, enabled: false);
		}
		else
		{
			Util.SetButton(playBtn);
		}
		string @string = Language.GetString("event_blank");
		if (_eventRef == null)
		{
			GvEEventRef nextEventRef = GvEEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
			SetStats();
		}
		else
		{
			_eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
			timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
			timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
			timeBar.COMPLETE.AddListener(OnEventTimerComplete);
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
		UpdateBonusDropdown();
	}

	private void OnEventTimerComplete()
	{
		SetStats();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int characterRank = -1, int characterPoints = -1, int guildRank = -1, int guildPoints = -1)
	{
		this.characterRank = characterRank;
		this.characterPoints = characterPoints;
		this.guildRank = guildRank;
		this.guildPoints = guildPoints;
		characterRankTxt.text = ((this.characterRank > 0) ? Util.NumberFormat(this.characterRank, abbreviate: false) : "-");
		characterPointsTxt.text = ((this.characterRank > 0) ? Util.NumberFormat(this.characterPoints, abbreviate: false) : "-");
		guildRankTxt.text = ((this.guildRank > 0) ? Util.NumberFormat(this.guildRank, abbreviate: false) : "-");
		guildPointsTxt.text = ((this.guildRank > 0) ? Util.NumberFormat(this.guildPoints, abbreviate: false) : "-");
		UpdateBonusDropdown();
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventEnter();
	}

	public void OnLootBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventLootItems();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	public void OnCharacterLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(7, 0, allowSegmented: true, base.gameObject);
	}

	public void OnCharacterRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(7, 2), characterRank, characterPoints, _zone, -1, alternate: false, 0L, base.gameObject);
	}

	public void OnGuildLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(7, 1, allowSegmented: false, base.gameObject);
	}

	public void OnGuildRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(7, 2), guildRank, guildPoints, -1, -1, alternate: true, 0L, base.gameObject);
	}

	public void OnGuildBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(7, 2, allowSegmented: false, base.gameObject);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(10), compare: false, added: true);
	}

	public void OnTeamBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoTeamSelect();
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(7), Util.parseMultiLine(Language.GetString("gve_help_desc")), base.gameObject);
	}

	public void OnBadgesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(11);
	}

	private void DoTeamSelect()
	{
		GameData.instance.windowGenerator.NewTeamWindow(_eventRef.getTeamType(), _eventRef.teamRules, OnTeamSelect, base.gameObject);
	}

	private void OnTeamSelect(TeamData teamData)
	{
		(GameData.instance.windowGenerator.GetDialogByClass(typeof(TeamWindow)) as TeamWindow).OnClose();
		CharacterDALC.instance.doSaveTeam(GameData.instance.PROJECT.character.teams.getTeam(teamData.type, teamData.teamRules));
	}

	private int GetBadgeCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = GvEEventBook.LookupBonus(GetSelectedBonusID());
		return _eventRef.GetCurrencyCost(bonus);
	}

	private void UpdateButtons()
	{
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(10);
		consumablesBtn.gameObject.SetActive(consumablesByCurrencyID.Count > 0);
	}

	public override void DoDestroy()
	{
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (playBtn != null && playBtn.gameObject != null)
		{
			playBtn.interactable = true;
		}
		if (lootBtn != null && lootBtn.gameObject != null)
		{
			lootBtn.interactable = true;
		}
		if (characterLeaderboardBtn != null && characterLeaderboardBtn.gameObject != null)
		{
			characterLeaderboardBtn.interactable = true;
		}
		if (characterRewardsBtn != null && characterRewardsBtn.gameObject != null)
		{
			characterRewardsBtn.interactable = true;
		}
		if (guildLeaderboardBtn != null && guildLeaderboardBtn.gameObject != null)
		{
			guildLeaderboardBtn.interactable = true;
		}
		if (guildRewardsBtn != null && guildRewardsBtn.gameObject != null)
		{
			guildRewardsBtn.interactable = true;
		}
		if (teamBtn != null && teamBtn.gameObject != null)
		{
			teamBtn.interactable = true;
		}
		if (helpBtn != null && helpBtn.gameObject != null)
		{
			helpBtn.interactable = true;
		}
		if (refreshBtn != null && refreshBtn.gameObject != null)
		{
			refreshBtn.interactable = true;
		}
		if (consumablesBtn != null && consumablesBtn.gameObject != null)
		{
			consumablesBtn.interactable = true;
		}
		if (guildBtn != null && guildBtn.gameObject != null && guildBtn.GetComponent<EventTrigger>() != null)
		{
			guildBtn.GetComponent<EventTrigger>().enabled = true;
		}
		if (badgesBtn != null && badgesBtn.gameObject != null && badgesBtn.GetComponent<EventTrigger>() != null)
		{
			badgesBtn.GetComponent<EventTrigger>().enabled = true;
		}
		if (costDropdown != null && costDropdown.gameObject != null && costDropdown.GetComponent<EventTrigger>() != null)
		{
			costDropdown.GetComponent<EventTrigger>().enabled = true;
		}
	}
}
