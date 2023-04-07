using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.ranklist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.events;

public class EventLeaderboardWindow : WindowsMain
{
	private const string BLANK = "-";

	public Image eventDropdown;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI rankCurrentTxt;

	public TextMeshProUGUI rankCurrentValueTxt;

	public TextMeshProUGUI rankListTxt;

	public TextMeshProUGUI emptyTxt;

	public TextMeshProUGUI eventNameTxt;

	public TextMeshProUGUI pointsCurrentTxt;

	public TextMeshProUGUI pointsListTxt;

	public TextMeshProUGUI pointsCurrentvalueTxt;

	public GameObject trophyImage;

	private TrophyHandler trophyHandler;

	public Image loadingIcon;

	private Transform window;

	private List<EventRef> events;

	private EventRef currentEvent;

	private int eventType;

	private int leaderboardType;

	private bool allowSegmented;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int eventType, int leaderboardType = 0, bool allowSegmented = true)
	{
		this.eventType = eventType;
		this.leaderboardType = leaderboardType;
		this.allowSegmented = allowSegmented;
		string eventTypeNameShort = EventRef.getEventTypeNameShort(eventType);
		topperTxt.text = Language.GetString("event_ranks", new string[1] { eventTypeNameShort });
		nameListTxt.text = Language.GetString("ui_name");
		rankCurrentTxt.text = Language.GetString("ui_rank");
		rankListTxt.text = Language.GetString("ui_rank");
		emptyTxt.text = Language.GetString("ui_leaderboard_empty");
		pointsCurrentTxt.text = ((eventType != 6) ? Language.GetString("ui_points") : Language.GetString("ui_weight"));
		pointsListTxt.text = Language.GetString("ui_points");
		TextMeshProUGUI componentInChildren = closeBtn.GetComponentInChildren<TextMeshProUGUI>();
		if (componentInChildren != null)
		{
			componentInChildren.text = Language.GetString("ui_x");
		}
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
		InitializeData();
	}

	private void OnDestroy()
	{
		LeaderboardDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnGetList);
	}

	private void InitializeData()
	{
		events = EventBook.GetSortedEvents(eventType, 2);
		if (events.Count > 0)
		{
			ReloadList(events[0].id);
		}
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		LeaderboardDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnGetList);
	}

	private EventRef GetCurrentEvent(int id)
	{
		return events.Find((EventRef item) => item.id.Equals(id));
	}

	public void OnGetList(BaseEvent baseEvent)
	{
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (loadingIcon != null && loadingIcon.gameObject != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		if (sfsob.ContainsKey("err0"))
		{
			return;
		}
		EventLeaderboardData eventLeaderboardData = EventLeaderboardData.fromSFSObject(sfsob);
		if (eventLeaderboardData == null)
		{
			return;
		}
		EventRef eventRefByID = EventBook.GetEventRefByID(EventBook.GetEvents(eventLeaderboardData.eventData.type), eventLeaderboardData.eventID);
		if (eventLeaderboardData.eventData.type == 6)
		{
			eventRefByID = FishingEventBook.GetEventRefByID(eventLeaderboardData.eventID);
		}
		if (eventRefByID == null)
		{
			return;
		}
		int points = ((eventLeaderboardData.eventData.type != 6) ? eventLeaderboardData.eventData.points : ((FishingEventCharacterData)eventLeaderboardData.eventData).highest);
		int rank = eventLeaderboardData.eventData.rank;
		if (eventLeaderboardData.eventData is GvGEventCharacterData)
		{
			GvGEventCharacterData gvGEventCharacterData = eventLeaderboardData.eventData as GvGEventCharacterData;
			if (leaderboardType == 1)
			{
				rank = gvGEventCharacterData.guildRank;
				points = gvGEventCharacterData.guildPoints;
			}
		}
		if (eventLeaderboardData.eventData is GvEEventCharacterData)
		{
			GvEEventCharacterData gvEEventCharacterData = eventLeaderboardData.eventData as GvEEventCharacterData;
			if (leaderboardType == 1)
			{
				rank = gvEEventCharacterData.guildRank;
				points = gvEEventCharacterData.guildPoints;
			}
		}
		List<LeaderboardData> leaderboardList = eventLeaderboardData.leaderboardList;
		RankList componentInChildren = GetComponentInChildren<RankList>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.InitList();
		componentInChildren.trophyHandler = trophyHandler;
		if (componentInChildren.Data.Count > 0)
		{
			componentInChildren.Data.RemoveItems(0, componentInChildren.Data.Count);
		}
		foreach (LeaderboardData item in leaderboardList)
		{
			string position = Util.NumberFormat(item.rank, abbreviate: false);
			string points2 = Util.NumberFormat((float)item.value / GetDivider(), abbreviate: false);
			componentInChildren.Data.InsertOneAtEnd(new LeaderboardItem
			{
				id = item.id,
				name = item.parsedName,
				points = points2,
				position = position,
				data = item
			});
		}
		SetStats(rank, points, eventRefByID.divider);
		emptyTxt.gameObject.SetActive(componentInChildren.Data.Count == 0);
	}

	private void SetStats(int rank, int points, float divider = 1f)
	{
		pointsCurrentvalueTxt.text = ((points > 0) ? Util.NumberFormat((float)points / divider) : "-");
		if (rank > 0)
		{
			rankCurrentValueTxt.text = Util.NumberFormat(rank);
			rank--;
			trophyHandler.ReplaceTrophy(trophyImage, rank);
		}
		else
		{
			rankCurrentValueTxt.text = "-";
		}
	}

	private float GetDivider()
	{
		return currentEvent.divider;
	}

	public void OnDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_events"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentEvent.id);
		foreach (EventRef @event in events)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = @event.id,
				title = @event.name,
				btnHelp = false
			});
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		eventDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		eventDropdown.GetComponent<EventTrigger>().enabled = false;
	}

	public void ReloadList(int id)
	{
		loadingIcon.gameObject.SetActive(value: true);
		currentEvent = GetCurrentEvent(id);
		eventNameTxt.text = Language.GetString(currentEvent.name);
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
		LeaderboardDALC.instance.doGetEvent(eventType, currentEvent.id, leaderboardType, allowSegmented);
	}
}
