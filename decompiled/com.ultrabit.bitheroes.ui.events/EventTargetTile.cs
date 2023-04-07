using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.events;

public class EventTargetTile : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI statsTxt;

	public TextMeshProUGUI fightBtnTxt;

	public TextMeshProUGUI pointsWinTxt;

	public TextMeshProUGUI pointsLoseTxt;

	private EventRef _eventRef;

	private EventTargetData _targetData;

	private void Start()
	{
		fightBtnTxt.text = Language.GetString("ui_fight");
	}

	public void TEST_PvPEvent()
	{
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_ENTER), TEST_onEventEnter);
		PvPDALC.instance.doEventEnter(1);
	}

	private void TEST_onEventEnter(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_ENTER), TEST_onEventEnter);
		enable();
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			Debug.Log("err0");
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		List<EventTargetData> list = EventTargetData.listFromSFSObject(sfsob);
		Debug.LogWarning("DEBUG DATA -- hardcoded pvp event");
		_eventRef = PvPEventBook.GetCurrentEventRef();
		CharacterData characterData = list[0].characterData;
		_targetData = new EventTargetData(characterData, 10, 10);
		List<TeammateData> list2 = new List<TeammateData>();
		list2.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
		doEventBattle(list2);
	}

	public void setTarget(EventTargetData tgt)
	{
		_targetData = tgt;
	}

	public void disable(bool changeMouse = true)
	{
		base.gameObject.SetActive(value: false);
	}

	public void enable(bool changeMouse = true)
	{
		base.gameObject.SetActive(value: true);
	}

	public void Fight()
	{
		TEST_PvPEvent();
	}

	private void doTeamSelect()
	{
	}

	private void doEventBattle(List<TeammateData> teammates)
	{
		int eventType = _eventRef.eventType;
		if (eventType != 1)
		{
			_ = 4;
		}
		else
		{
			doPvPEventBattle(teammates);
		}
	}

	private void doPvPEventBattle(List<TeammateData> teammates)
	{
		disable();
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_BATTLE), onPvPEventBattle);
		PvPDALC.instance.doEventBattle(_targetData.characterData.charID, teammates);
	}

	private void onPvPEventBattle(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_BATTLE), onPvPEventBattle);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			Debug.Log("err0");
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}
}
