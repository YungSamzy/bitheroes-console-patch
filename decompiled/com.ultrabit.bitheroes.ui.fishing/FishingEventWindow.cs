using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingEventWindow : WindowsMain
{
	public Button leaderboardBtn;

	public Button rewardsBtn;

	public Button refreshBtn;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI rankNameTxt;

	public TextMeshProUGUI rankValueTxt;

	public TextMeshProUGUI typeNameTxt;

	public TextMeshProUGUI typeValueTxt;

	public TextMeshProUGUI altNameTxt;

	public TextMeshProUGUI altValueTxt;

	private EventRef _eventRef;

	private TimeBarColor timeBar;

	private TrophyHandler trophyHandler;

	public Image trophyImage;

	private const string BLANK = "-";

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private int _rank;

	private int _points;

	private int _highest;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = EventRef.getEventTypeName(6);
		eventTxt.text = Language.GetString("event_end", new string[1] { EventRef.getEventTypeName(6) });
		rankNameTxt.text = Language.GetString("ui_rank");
		typeNameTxt.text = Language.GetString("ui_type");
		altNameTxt.text = Language.GetString("ui_highest");
		leaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		rewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		timeBar = GetComponentInChildren<TimeBarColor>();
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		SetStats();
		SetLootID();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
		ListenForBack(OnClose);
		CreateWindow();
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
			FishingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnEventLootCheck);
			FishingDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		_checkingLoot = false;
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		FishingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEventLootItems);
		FishingDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("Fishing Event Reward", null, list, sfsob, "Fishing Event", 2);
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
		if (_lootEventID >= 0)
		{
			DoEventLootItems();
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
				FishingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventStats);
				FishingDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		_checkingStats = false;
		DALCEvent obj = baseEvent as DALCEvent;
		FishingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		FishingEventCharacterData fishingEventCharacterData = FishingEventCharacterData.fromSFSObject(sfsob);
		SetStats(fishingEventCharacterData.rank, fishingEventCharacterData.points, fishingEventCharacterData.highest);
	}

	private void UpdateEvent()
	{
		_eventRef = FishingEventBook.GetCurrentEventRef();
		string @string = Language.GetString("event_blank");
		if (_eventRef == null)
		{
			FishingEventRef nextEventRef = FishingEventBook.GetNextEventRef();
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
		typeNameTxt.text = ((_eventRef == null) ? FishingEventRef.getFishingTypeName(0) : _eventRef.GetCurrentRankTypeName());
		altNameTxt.text = ((_eventRef == null) ? FishingEventRef.getFishingTypeName(1) : _eventRef.GetAltTypeName());
	}

	private void OnEventTimerComplete()
	{
		SetStats();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int rank = -1, int points = -1, int highest = -1, int difficulty = -1)
	{
		_rank = rank;
		_points = points;
		_highest = highest;
		int num = ((_eventRef != null) ? _eventRef.GetCurrentValue(highest, points) : highest);
		int num2 = ((_eventRef != null) ? _eventRef.GetAltValue(highest, points) : points);
		if (rankValueTxt != null)
		{
			rankValueTxt.text = ((rank > 0) ? Util.NumberFormat(rank, abbreviate: false) : "-");
		}
		if (altValueTxt != null)
		{
			altValueTxt.text = ((rank > 0) ? GetNumberString(num2) : "-");
		}
		if (typeValueTxt != null)
		{
			typeValueTxt.text = ((rank > 0) ? GetNumberString(num) : "-");
		}
		if (trophyHandler != null)
		{
			trophyHandler.ReplaceTrophy(trophyImage.gameObject, rank);
		}
	}

	private string GetNumberString(float number)
	{
		if (_eventRef == null)
		{
			return Util.NumberFormat(number, abbreviate: false);
		}
		return Util.NumberFormat(number / _eventRef.divider, abbreviate: false);
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
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(6, 0, base.gameObject);
	}

	public void OnRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(6, 2), _rank, _points, -1, -1, alternate: false, 0L, base.gameObject);
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
		leaderboardBtn.interactable = true;
		rewardsBtn.interactable = true;
		refreshBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		leaderboardBtn.interactable = false;
		rewardsBtn.interactable = false;
		refreshBtn.interactable = false;
	}
}
