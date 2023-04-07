using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.pvpteamlist;
using com.ultrabit.bitheroes.ui.team;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.pvp;

public class PvPEventWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	private PvPEventRef _eventRef;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI costNameValueTxt;

	public TextMeshProUGUI rankNameTxt;

	public TextMeshProUGUI rankTxt;

	public TextMeshProUGUI pointsNameTxt;

	public TextMeshProUGUI pointsTxt;

	public GameObject trophyImage;

	public Button playBtn;

	public Button lootBtn;

	public Button historyBtn;

	public Button leaderboardBtn;

	public Button rewardsBtn;

	public Button teamBtn;

	public Button consumablesBtn;

	public Button refreshBtn;

	public Image ticketsDropdown;

	public PvPTeamList pvpTeamList;

	public SpriteMask[] spriteMasks;

	private TimeBarColor timeBar;

	private Transform window;

	private const string BLANK = "-";

	private TrophyHandler trophyHandler;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private MyDropdownItemModel _selectedBonus;

	private List<MyDropdownItemModel> _bonuses = new List<MyDropdownItemModel>();

	private Transform teamWindow;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private int _rank;

	private int _points;

	private int _zone;

	private bool isAllowedToEnter
	{
		get
		{
			if (_eventRef == null)
			{
				GameData.instance.windowGenerator.ShowErrorCode(27);
				return false;
			}
			if (GameData.instance.PROJECT.character.tickets < GetTicketCost())
			{
				GameData.instance.windowGenerator.ShowErrorCode(23);
				return false;
			}
			return true;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = EventRef.getEventTypeName(1);
		costNameTxt.text = Language.GetString("ui_cost");
		rankNameTxt.text = Language.GetString("ui_rank");
		pointsNameTxt.text = Language.GetString("ui_points");
		rankTxt.text = "-";
		pointsTxt.text = "-";
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		lootBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
		historyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_history");
		leaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		rewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		teamBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_team");
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		timeBar = GetComponentInChildren<TimeBarColor>();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		GameData.instance.PROJECT.character.teams.AddListener(CustomSFSXEvent.CHANGE, OnTeamChange);
		pvpTeamList.InitList(OnItemClick, this);
		CreateTiles();
		SetStats();
		SetLootID();
		UpdateEvent();
		UpdateButtons();
		DoEventLootCheck();
		DoEventStats();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		ListenForForward(OnPlayBtn);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		if (pvpTeamList.Data != null)
		{
			pvpTeamList.Refresh();
			int num = base.sortingLayer + pvpTeamList.Data.Count;
			SpriteMask[] array = spriteMasks;
			foreach (SpriteMask obj in array)
			{
				obj.frontSortingLayerID = SortingLayer.NameToID("UI");
				obj.frontSortingOrder = num + 1 + pvpTeamList.Data.Count;
				obj.backSortingLayerID = SortingLayer.NameToID("UI");
				obj.backSortingOrder = base.sortingLayer;
			}
		}
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(22) && PvPEventBook.GetCurrentEventRef() != null && GameData.instance.PROJECT.character.tickets > 0)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(22);
			GameData.instance.tutorialManager.ShowTutorialForButton(playBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(22), 4, playBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void UpdateDropdowns()
	{
		_bonuses.Clear();
		int pvPEventBonus = GameData.instance.SAVE_STATE.GetPvPEventBonus(GameData.instance.PROJECT.character.id);
		if (_eventRef != null)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.tickets)
			};
			_bonuses.Add(myDropdownItemModel);
			_selectedBonus = myDropdownItemModel;
			for (int i = 0; i < PvPEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = PvPEventBook.LookupBonus(i);
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
					if (currencyBonusRef.id == pvPEventBonus)
					{
						_selectedBonus = myDropdownItemModel2;
					}
				}
			}
		}
		costNameValueTxt.text = ((_selectedBonus != null) ? _selectedBonus.title : "");
		if (_eventRef == null)
		{
			UIUtil.LockImage(ticketsDropdown, locked: true);
		}
		else
		{
			UIUtil.LockImage(ticketsDropdown, locked: false);
		}
	}

	public void OnDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(5)));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetPvPEventBonus(GameData.instance.PROJECT.character.id), OnCostDropdownChange);
		componentInChildren.Data.InsertItemsAtEnd(_bonuses);
	}

	public void OnCostDropdownChange(MyDropdownItemModel selected)
	{
		int difficulty = ((selected.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		_selectedBonus = selected;
		costNameValueTxt.text = _selectedBonus.title;
		GameData.instance.SAVE_STATE.SetPvPEventBonus(GameData.instance.PROJECT.character.id, difficulty);
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

	private void ClearTiles()
	{
		pvpTeamList.ClearList();
	}

	private void CreateTiles()
	{
		ClearTiles();
		if (_eventRef == null)
		{
			return;
		}
		TeamData team = GameData.instance.PROJECT.character.teams.getTeam(2, _eventRef.teamRules);
		if (team == null)
		{
			GameData.instance.PROJECT.character.teams.setTeam(2, _eventRef.teamRules.copy(), GameData.instance.PROJECT.character.getAutoAssignedTeam(_eventRef.teamRules, 2));
			return;
		}
		foreach (TeammateData teammate in team.teammates)
		{
			object data = teammate?.data;
			pvpTeamList.Data.InsertOneAtEnd(new PvPTeamItem
			{
				data = data
			});
		}
	}

	private void OnItemClick(PvPTeamItem model)
	{
		CharacterData characterData = model.data as CharacterData;
		GameData.instance.windowGenerator.ShowPlayer(characterData.charID);
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

	public void DoEventLootCheck()
	{
		if (!_checkingLoot)
		{
			_checkingLoot = true;
			PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_CHECK), OnEventLootCheck);
			PvPDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_CHECK), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_ITEMS), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("PvP Event Reward", null, list, sfsob, "PvP Event", 2);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
		else
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_event_loot"));
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
				PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_STATS), OnEventStats);
				PvPDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		_checkingStats = false;
		DALCEvent obj = baseEvent as DALCEvent;
		if (this != null && base.gameObject != null && _eventRef != null)
		{
			Util.SetButton(playBtn);
		}
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_STATS), OnEventStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		EventCharacterData eventCharacterData = EventCharacterData.fromSFSObject(sfsob);
		SetStats(eventCharacterData.rank, eventCharacterData.points);
		_zone = eventCharacterData.zone;
	}

	private void DoEventEnter()
	{
		if (!base.disabled && isAllowedToEnter)
		{
			Disable();
			GameData.instance.main.ShowLoading();
			PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_ENTER), OnEventEnter);
			PvPDALC.instance.doEventEnter(GetSelectedBonusID());
		}
	}

	private void OnEventEnter(BaseEvent e)
	{
		Enable();
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_ENTER), OnEventEnter);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<EventTargetData> targets = EventTargetData.listFromSFSObject(sfsob);
		GameData.instance.windowGenerator.NewEventTargetWindow(_eventRef, targets, base.gameObject);
	}

	private void UpdateEvent()
	{
		_eventRef = PvPEventBook.GetCurrentEventRef();
		bool flag = _eventRef != null;
		Util.SetButton(playBtn, flag);
		bool flag2 = flag && _eventRef.teamRules.slots > 1;
		Util.SetButton(teamBtn, flag2);
		string @string = Language.GetString("event_blank");
		if (flag)
		{
			_eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
			timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
			timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
			timeBar.COMPLETE.AddListener(OnEventTimerComplete);
		}
		else
		{
			PvPEventRef nextEventRef = PvPEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
			SetStats();
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
		CreateTiles();
		UpdateDropdowns();
	}

	private void OnEventTimerComplete()
	{
		SetStats();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int rank = -1, int points = -1)
	{
		_rank = rank;
		_points = points;
		rankTxt.text = ((rank > 0) ? Util.NumberFormat(rank, abbreviate: false) : "-");
		pointsTxt.text = ((rank > 0) ? Util.NumberFormat(points, abbreviate: false) : "-");
		trophyHandler.ReplaceTrophy(trophyImage, rank);
		UpdateDropdowns();
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventEnter();
	}

	public void OnLootBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Disable();
		GameData.instance.main.ShowLoading();
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_LOOT_ITEMS), OnEventLootItems);
		PvPDALC.instance.doEventLootItems(_lootEventID);
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	public void OnLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(1, 0, base.gameObject);
	}

	public void OnHistoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPvPEventHistoryWindow(base.gameObject);
	}

	public void OnRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(1, 2), _rank, _points, _zone, -1, alternate: false, 0L, base.gameObject);
	}

	public void OnTeamBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTeamWindow(2, _eventRef.teamRules, delegate
		{
			CloseTeamWindow();
		}, base.gameObject, -1, showArmoryButton: false);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(5), compare: false, added: true);
	}

	private void CloseTeamWindow()
	{
		(GameData.instance.windowGenerator.GetDialogByClass(typeof(TeamWindow)) as TeamWindow).OnClose();
	}

	private void OnInventoryChange()
	{
		UpdateButtons();
	}

	private void OnTeamChange()
	{
		CreateTiles();
	}

	private int GetTicketCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = PvPEventBook.LookupBonus(GetSelectedBonusID());
		return _eventRef.GetCurrencyCost(bonus);
	}

	private void UpdateButtons()
	{
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(5);
		consumablesBtn.gameObject.SetActive(consumablesByCurrencyID.Count > 0);
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		GameData.instance.PROJECT.character.teams.RemoveListener(CustomSFSXEvent.CHANGE, OnTeamChange);
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtons(enabled: true);
		pvpTeamList.Refresh();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtons(enabled: false);
	}

	private void SetButtons(bool enabled)
	{
		lootBtn.interactable = enabled;
		historyBtn.interactable = enabled;
		leaderboardBtn.interactable = enabled;
		rewardsBtn.interactable = enabled;
		teamBtn.interactable = enabled;
		consumablesBtn.interactable = enabled;
		refreshBtn.interactable = enabled;
		playBtn.interactable = enabled;
		if (ticketsDropdown.TryGetComponent<EventTrigger>(out var component))
		{
			component.enabled = enabled;
		}
	}
}
