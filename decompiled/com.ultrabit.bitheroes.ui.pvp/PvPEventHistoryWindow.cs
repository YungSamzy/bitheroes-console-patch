using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.historylist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.pvp;

public class PvPEventHistoryWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI timeListTxt;

	public TextMeshProUGUI opponentListTxt;

	public TextMeshProUGUI pointsListTxt;

	public TextMeshProUGUI eventNameTxt;

	public TextMeshProUGUI emptyTxt;

	private Transform window;

	public Image eventDropdown;

	public Image loadingIcon;

	private List<EventRef> events;

	private EventRef currentEvent;

	public override void Start()
	{
		InitializeData();
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_history");
		timeListTxt.text = Language.GetString("ui_time_since");
		opponentListTxt.text = Language.GetString("ui_opponent");
		pointsListTxt.text = Language.GetString("ui_points");
		emptyTxt.text = Language.GetString("ui_leaderboard_empty");
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnDestroy()
	{
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_HISTORY), OnGetList);
	}

	private void InitializeData()
	{
		events = EventBook.GetSortedEvents(1, 2);
		if (events.Count > 0)
		{
			ReloadList(events[0].id);
		}
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_HISTORY), OnGetList);
	}

	private EventRef GetCurrentEvent(int id)
	{
		return events.Find((EventRef item) => item.id.Equals(id));
	}

	public void OnGetList(BaseEvent baseEvent)
	{
		loadingIcon.gameObject.SetActive(value: false);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			return;
		}
		sfsob.GetInt("eve0");
		List<EventHistoryData> list = EventHistoryData.listFromSFSObject(sfsob);
		HistoryList componentInChildren = GetComponentInChildren<HistoryList>();
		if (componentInChildren.Data.Count > 0)
		{
			componentInChildren.Data.RemoveItems(0, componentInChildren.Data.Count);
		}
		foreach (EventHistoryData item in list)
		{
			componentInChildren.Data.InsertOneAtEnd(new HistoryListModel
			{
				id = item.charID,
				time = item.parsedCreatedDate,
				name = item.parsedName,
				point = item.parsedPoints
			});
		}
		emptyTxt.gameObject.SetActive(componentInChildren.Data.Count == 0);
	}

	public void OnEventDropdown()
	{
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
		PvPDALC.instance.doEventHistory(id);
	}
}
