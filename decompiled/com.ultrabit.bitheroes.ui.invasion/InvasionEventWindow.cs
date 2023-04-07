using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.invasion;

public class InvasionEventWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI costNameValueTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public TextMeshProUGUI rankNameTxt;

	public TextMeshProUGUI communityPointsNameTxt;

	public TextMeshProUGUI communityPointsValueTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI rankValueTxt;

	public TextMeshProUGUI typeNameTxt;

	public TextMeshProUGUI typeValueTxt;

	public TextMeshProUGUI altNameTxt;

	public Image tierBar;

	public Button playBtn;

	public Button lootBtn;

	public Button leaderboardBtn;

	public Button rewardsBtn;

	public Button helpBtn;

	public Button refreshBtn;

	public Button chestBtn;

	public Button segmentedHelpBtn;

	public Image badgesBtn;

	public Image costDropdown;

	public Image difficultyDropdown;

	public CurrencyBarFill currencyBarFill;

	public Transform invasionEventTierRefPrefab;

	private const string BLANK = "-";

	private InvasionEventRef _eventRef;

	private Transform window;

	private TimeBarColor timeBar;

	private List<MyDropdownItemModel> difficultyData = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedBonus;

	private List<MyDropdownItemModel> _bonuses = new List<MyDropdownItemModel>();

	private int rank;

	private int points;

	private int highest;

	private int difficulty;

	private long communityPoints;

	private int _zone = -1;

	private int selectedDifficulty;

	private float _tierBarWidth;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private List<InvasionEventTierTile> _tierTiles = new List<InvasionEventTierTile>();

	private InvasionEventCharacterData _eventCharacterData;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = EventRef.getEventTypeName(5);
		costNameTxt.text = Language.GetString("ui_cost");
		difficultyNameTxt.text = Language.GetString("ui_wave");
		rankNameTxt.text = Language.GetString("ui_rank");
		communityPointsNameTxt.text = Language.GetString("ui_community_points");
		communityPointsValueTxt.text = "";
		rankValueTxt.text = "-";
		typeValueTxt.text = "-";
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		lootBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
		leaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		rewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		segmentedHelpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		lootBtn.gameObject.SetActive(value: false);
		segmentedHelpBtn.gameObject.SetActive(value: false);
		timeBar = GetComponentInChildren<TimeBarColor>();
		_tierBarWidth = tierBar.rectTransform.sizeDelta.x;
		tierBar.rectTransform.sizeDelta = new Vector2(0f, tierBar.rectTransform.sizeDelta.y);
		currencyBarFill.Init();
		SetStats(-1, -1, -1, -1, 0L);
		SetLootID();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
		ListenForBack(OnClose);
		ListenForForward(OnPlayBtn);
		CreateWindow();
	}

	private void UpdateDifficultyDropdown()
	{
		selectedDifficulty = GameData.instance.SAVE_STATE.GetInvasionEventDifficulty(GameData.instance.PROJECT.character.id);
		if (points <= 0)
		{
			selectedDifficulty = difficulty - 2;
			if (selectedDifficulty <= 0)
			{
				selectedDifficulty = 1;
			}
		}
		if (selectedDifficulty <= 0)
		{
			selectedDifficulty = difficulty;
		}
		difficultyData.Clear();
		MyDropdownItemModel myDropdownItemModel = null;
		if (difficulty > 0 && difficulty < selectedDifficulty)
		{
			selectedDifficulty = difficulty;
			GameData.instance.SAVE_STATE.SetInvasionEventDifficulty(GameData.instance.PROJECT.character.id, selectedDifficulty);
		}
		for (int num = difficulty; num > 0; num--)
		{
			int num2 = num;
			string title = Util.NumberFormat(num2);
			MyDropdownItemModel myDropdownItemModel2 = new MyDropdownItemModel
			{
				id = num2,
				title = title,
				data = num2
			};
			difficultyData.Add(myDropdownItemModel2);
			if (num2 == selectedDifficulty)
			{
				myDropdownItemModel = myDropdownItemModel2;
			}
		}
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = ((myDropdownItemModel != null) ? myDropdownItemModel.title : "-");
		if (_eventRef == null || _checkingStats || difficulty <= 0)
		{
			UIUtil.LockImage(difficultyDropdown, locked: true);
		}
		else
		{
			UIUtil.LockImage(difficultyDropdown, locked: false);
		}
	}

	public void OnDifficultyDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, selectedDifficulty, OnDifficultyDropdownClicked);
		componentInChildren.Data.InsertItems(0, difficultyData);
	}

	public void OnDifficultyDropdownClicked(MyDropdownItemModel model)
	{
		selectedDifficulty = model.id;
		GameData.instance.SAVE_STATE.SetInvasionEventDifficulty(GameData.instance.PROJECT.character.id, model.id);
		difficultyDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void UpdateBonusDropdown()
	{
		_bonuses.Clear();
		int invasionEventBonus = GameData.instance.SAVE_STATE.GetInvasionEventBonus(GameData.instance.PROJECT.character.id);
		if (_eventRef != null)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.badges)
			};
			_bonuses.Add(myDropdownItemModel);
			_selectedBonus = myDropdownItemModel;
			for (int i = 0; i < InvasionEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = InvasionEventBook.LookupBonus(i);
				if (currencyBonusRef != null)
				{
					int badges = _eventRef.getBadges(currencyBonusRef);
					MyDropdownItemModel myDropdownItemModel2 = new MyDropdownItemModel
					{
						id = i,
						title = Util.NumberFormat(badges),
						data = currencyBonusRef,
						btnHelp = true
					};
					_bonuses.Add(myDropdownItemModel2);
					if (currencyBonusRef.id == invasionEventBonus)
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
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(5)));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetInvasionEventBonus(), OnCostDropdownChange);
		componentInChildren.Data.InsertItems(0, _bonuses);
	}

	public void OnCostDropdownChange(MyDropdownItemModel selected)
	{
		int bonus = ((selected.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetInvasionEventBonus(GameData.instance.PROJECT.character.id, bonus);
		_selectedBonus = selected;
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
			InvasionDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
			InvasionDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		InvasionDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
		InvasionDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("Invasion Event Reward", null, list, sfsob, "Invasion Event", 2);
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
		if (lootBtn == null || lootBtn.gameObject == null)
		{
			D.Log("all", $"InvasionEventWindow :: SetLootID() :: Null reference: lootBtn = {lootBtn}, lootBtn.gameObject = {lootBtn?.gameObject}", forceLoggly: true);
			return;
		}
		if (playBtn == null || playBtn.gameObject == null)
		{
			D.Log("all", $"InvasionEventWindow :: SetLootID() :: Null reference: playBtn = {playBtn}, PlayBtn.gameObject = {playBtn?.gameObject}", forceLoggly: true);
			return;
		}
		lootBtn.gameObject.SetActive(_lootEventID >= 0);
		playBtn.gameObject.SetActive(!lootBtn.gameObject.activeSelf);
	}

	private void DoEventStats()
	{
		if (!_checkingStats)
		{
			SetStats(-1, -1, -1, -1, 0L);
			RestartRefreshTimer();
			if (_eventRef != null)
			{
				_checkingStats = true;
				Util.SetButton(playBtn, enabled: false);
				InvasionDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
				InvasionDALC.instance.doEventStats(_eventRef.id);
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
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		InvasionEventCharacterData invasionEventCharacterData = InvasionEventCharacterData.fromSFSObject(sfsob);
		if (this != null && base.gameObject != null)
		{
			SetStats(invasionEventCharacterData.rank, invasionEventCharacterData.points, invasionEventCharacterData.highest, invasionEventCharacterData.difficulty, invasionEventCharacterData.communityPoints);
			_zone = invasionEventCharacterData.zone;
		}
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
		if (difficultyDropdown == null || _selectedBonus == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_event_difficulty"));
			return false;
		}
		return true;
	}

	private void DoTeamSelect()
	{
		if (DoAllowEnter())
		{
			GameData.instance.windowGenerator.NewTeamWindow(7, _eventRef.teamRules, OnTeamSelect, base.gameObject);
		}
	}

	private void OnTeamSelect(TeamData teamData)
	{
		DoEventEnter(teamData.teammates);
	}

	private void DoEventEnter(List<TeammateData> teammates)
	{
		if (DoAllowEnter())
		{
			GameData.instance.main.ShowLoading();
			InvasionDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
			InvasionDALC.instance.doEventEnter(selectedDifficulty, GetSelectedBonusID(), teammates);
		}
	}

	private void OnEventEnter(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		InvasionDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void UpdateEvent()
	{
		_eventRef = InvasionEventBook.GetCurrentEventRef();
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
			InvasionEventRef nextEventRef = InvasionEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
			SetStats(-1, -1, -1, -1, 0L);
		}
		else
		{
			_eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
			timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
			timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
			timeBar.COMPLETE.AddListener(OnEventTimerComplete);
			if (_eventRef.hasSegmentedRewards)
			{
				segmentedHelpBtn.gameObject.SetActive(value: true);
			}
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
		typeNameTxt.text = ((_eventRef == null) ? InvasionEventRef.getInvasionTypeName(0) : _eventRef.getCurrentTypeName());
		if (altNameTxt != null)
		{
			altNameTxt.text = ((_eventRef == null) ? InvasionEventRef.getInvasionTypeName(1) : _eventRef.getAltTypeName());
		}
		UpdateBonusDropdown();
	}

	private void OnEventTimerComplete()
	{
		SetStats(-1, -1, -1, -1, 0L);
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int rank = -1, int points = -1, int highest = -1, int difficulty = -1, long communityPoints = 0L)
	{
		this.rank = rank;
		this.points = points;
		this.highest = highest;
		this.difficulty = difficulty;
		this.communityPoints = communityPoints;
		int num = ((_eventRef != null) ? _eventRef.GetCurrentValue(highest, points) : highest);
		if (_eventRef != null)
		{
			_eventRef.GetAltValue(highest, points);
		}
		rankValueTxt.text = ((rank > 0) ? Util.NumberFormat(rank, abbreviate: false) : "-");
		typeValueTxt.text = ((rank > 0) ? Util.NumberFormat(num, abbreviate: false) : "-");
		typeNameTxt.text = ((_eventRef == null) ? EventRef.getEventTypeName(0) : _eventRef.GetCurrentRankTypeName());
		communityPointsValueTxt.text = ((communityPoints > 0) ? Util.NumberFormat(communityPoints, abbreviate: false) : "-");
		UpdateDifficultyDropdown();
		UpdateTierBar(communityPoints);
	}

	private void UpdateTierBar(long communityPoints)
	{
		ClearTierTiles();
		if (_eventRef == null)
		{
			return;
		}
		if (!_eventRef.hasSegmentedRewards)
		{
			InvasionEventTierRef lastTierRef = _eventRef.GetLastTierRef();
			if (lastTierRef != null)
			{
				long num = (long)lastTierRef.points;
				tierBar.GetComponent<RegularBarFill>().UpdateBar(communityPoints, num);
				CreateTierTiles(num, communityPoints);
			}
		}
		else
		{
			InvasionEventLevelRef lastLevelRef = _eventRef.GetLastLevelRef();
			if (lastLevelRef != null)
			{
				long num2 = lastLevelRef.points;
				tierBar.GetComponent<RegularBarFill>().UpdateBar(communityPoints, num2);
				CreateTierTiles(num2, communityPoints);
			}
		}
	}

	private void ClearTierTiles()
	{
		foreach (InvasionEventTierTile tierTile in _tierTiles)
		{
			if (tierTile != null)
			{
				Object.Destroy(tierTile.gameObject);
			}
		}
		_tierTiles.Clear();
	}

	private void CreateTierTiles(long max, long communityPoints)
	{
		if (_eventRef == null)
		{
			ClearTierTiles();
		}
		else
		{
			if (_tierTiles.Count > 0)
			{
				return;
			}
			if (!_eventRef.hasSegmentedRewards)
			{
				InvasionEventTierRef tierRef = _eventRef.GetTierRef(communityPoints);
				{
					foreach (InvasionEventTierRef tierReward in _eventRef.tierRewards)
					{
						Transform obj = Object.Instantiate(invasionEventTierRefPrefab);
						obj.SetParent(panel.transform, worldPositionStays: false);
						obj.SetSiblingIndex(tierBar.transform.GetSiblingIndex() + 1);
						float num = tierReward.points / (float)max;
						InvasionEventTierTile component = obj.GetComponent<InvasionEventTierTile>();
						component.LoadDetails(_eventRef, tierReward, null, tierRef.id == tierReward.id);
						component.GetComponent<RectTransform>().anchoredPosition = new Vector2(tierBar.rectTransform.anchoredPosition.x + _tierBarWidth * num, tierBar.rectTransform.anchoredPosition.y - 5f);
						_tierTiles.Add(component);
					}
					return;
				}
			}
			InvasionEventLevelRef levelRef = _eventRef.GetLevelRef(communityPoints);
			foreach (InvasionEventLevelRef level in _eventRef.levels)
			{
				if (level.id != 1)
				{
					Transform obj2 = Object.Instantiate(invasionEventTierRefPrefab);
					obj2.SetParent(panel.transform, worldPositionStays: false);
					obj2.SetSiblingIndex(tierBar.transform.GetSiblingIndex() + 1);
					float num2 = (float)level.points / (float)max;
					InvasionEventTierTile component2 = obj2.GetComponent<InvasionEventTierTile>();
					component2.LoadDetails(_eventRef, null, level, levelRef.id == level.id);
					component2.GetComponent<RectTransform>().anchoredPosition = new Vector2(tierBar.rectTransform.anchoredPosition.x + _tierBarWidth * num2, tierBar.rectTransform.anchoredPosition.y - 5f);
					_tierTiles.Add(component2);
				}
			}
		}
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_eventRef.teamRules.slots <= 1)
		{
			List<TeammateData> list = new List<TeammateData>();
			list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			DoEventEnter(list);
		}
		else
		{
			DoTeamSelect();
		}
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

	public void OnLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(5, 0, allowSegmented: true, base.gameObject);
	}

	public void OnRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(5, 2), rank, points, _zone, -1, alternate: false, communityPoints, base.gameObject);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_eventRef != null && _eventRef.hasSegmentedRewards)
		{
			GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(5), Util.parseMultiLine(Language.GetString("invasion_segmented_help_desc")), base.gameObject);
		}
		else
		{
			GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(5), Util.parseMultiLine(Language.GetString("invasion_help_desc")), base.gameObject);
		}
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(8, _eventRef.id, 0, "", GameData.instance.SAVE_STATE.GetInvasionEventDifficulty(GameData.instance.PROJECT.character.id));
	}

	public void OnBadgesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(11);
	}

	public void OnSegmentedBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewInvasionGameModifierListWindow(_eventRef.levels, communityPoints, null);
	}

	private int GetBadgeCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonusRef = InvasionEventBook.LookupBonus(GetSelectedBonusID());
		return _eventRef.getBadges(bonusRef);
	}

	public override void DoDestroy()
	{
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		playBtn.interactable = true;
		lootBtn.interactable = true;
		leaderboardBtn.interactable = true;
		rewardsBtn.interactable = true;
		helpBtn.interactable = true;
		refreshBtn.interactable = true;
		chestBtn.interactable = true;
		segmentedHelpBtn.interactable = true;
		badgesBtn.GetComponent<EventTrigger>().enabled = true;
		costDropdown.GetComponent<EventTrigger>().enabled = true;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		playBtn.interactable = false;
		lootBtn.interactable = false;
		leaderboardBtn.interactable = false;
		rewardsBtn.interactable = false;
		helpBtn.interactable = false;
		refreshBtn.interactable = false;
		chestBtn.interactable = false;
		segmentedHelpBtn.interactable = true;
		badgesBtn.GetComponent<EventTrigger>().enabled = false;
		costDropdown.GetComponent<EventTrigger>().enabled = false;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
