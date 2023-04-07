using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.eventtargetlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.events;

public class EventTargetWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI winListTxt;

	public TextMeshProUGUI loseListTxt;

	public Button statsBtn;

	public Button fleeBtn;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private EventRef _eventRef;

	private List<EventTargetData> _targets;

	private EventTargetList eventTargetList;

	private EventTargetData eventTargetData;

	private bool _opponentSelected;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(EventRef eventRef, List<EventTargetData> targets)
	{
		_eventRef = eventRef;
		_targets = targets;
		topperTxt.text = Language.GetString("ui_select_opponent");
		nameListTxt.text = Language.GetString("ui_name");
		winListTxt.text = Language.GetString("ui_win");
		loseListTxt.text = Language.GetString("ui_lose");
		fleeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_flee");
		statsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_stats");
		eventTargetList = GetComponentInChildren<EventTargetList>();
		eventTargetList.InitList(OnSelectedOpponent, this);
		SortTiles();
		SCROLL_IN_START.AddListener(ResetOpponentSelected);
		ListenForBack(OnFleeBtn);
		forceAnimation = true;
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		UpdateTiles();
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + _sortingLayer + eventTargetList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + _sortingLayer + eventTargetList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _sortingLayer;
	}

	private void UpdateTiles()
	{
		eventTargetList.ClearList();
		for (int i = 0; i < _targets.Count; i++)
		{
			eventTargetList.Data.InsertOneAtEnd(new EventTargetItem
			{
				eventTargetData = _targets[i]
			});
		}
	}

	private void OnSelectedOpponent(EventTargetData eventTargetData)
	{
		if (!_opponentSelected)
		{
			_opponentSelected = true;
			this.eventTargetData = eventTargetData;
			if (_eventRef.teamRules.slots <= 1)
			{
				List<TeammateData> list = new List<TeammateData>();
				list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
				DoEventBattle(list);
			}
			else
			{
				DoTeamSelect();
			}
		}
	}

	private void DoTeamSelect()
	{
		bool showArmoryButton = true;
		if (_eventRef.eventType == 1)
		{
			showArmoryButton = false;
		}
		GameData.instance.windowGenerator.NewTeamWindow(_eventRef.getTeamType(), _eventRef.teamRules, OnTeamSelected, base.gameObject, -1, showArmoryButton);
	}

	private void OnTeamSelected(TeamData teamData)
	{
		DoEventBattle(teamData.teammates);
	}

	private void DoEventBattle(List<TeammateData> teammates)
	{
		switch (_eventRef.eventType)
		{
		case 1:
			DoPvPEventBattle(teammates);
			break;
		case 4:
			DoGvGEventBattle(teammates);
			break;
		}
	}

	private void DoPvPEventBattle(List<TeammateData> teammates)
	{
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_BATTLE), OnPvPEventBattle);
		PvPDALC.instance.doEventBattle(eventTargetData.characterData.charID, teammates);
	}

	private void OnPvPEventBattle(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_BATTLE), OnPvPEventBattle);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.PVP_BATTLES_PLAYED);
		}
	}

	private void DoGvGEventBattle(List<TeammateData> teammates)
	{
		GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnGvGEventBattle);
		GvGDALC.instance.doEventBattle(eventTargetData.characterData.charID, teammates);
	}

	private void OnGvGEventBattle(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnGvGEventBattle);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public void Sort()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		switch (_eventRef.eventType)
		{
		case 1:
			GameData.instance.SAVE_STATE.SetPvPEventSort(GameData.instance.PROJECT.character.id, !GameData.instance.SAVE_STATE.GetPvPEventSort(GameData.instance.PROJECT.character.id));
			break;
		case 4:
			GameData.instance.SAVE_STATE.SetGvGEventSort(GameData.instance.PROJECT.character.id, !GameData.instance.SAVE_STATE.GetGvGEventSort(GameData.instance.PROJECT.character.id));
			break;
		}
		SortTiles();
	}

	private void SortTiles()
	{
		bool flag = true;
		switch (_eventRef.eventType)
		{
		case 1:
			flag = GameData.instance.SAVE_STATE.GetPvPEventSort(GameData.instance.PROJECT.character.id);
			break;
		case 4:
			flag = GameData.instance.SAVE_STATE.GetGvGEventSort(GameData.instance.PROJECT.character.id);
			break;
		}
		_targets = Util.SortVector(_targets, new string[1] { "stats" }, flag ? Util.ARRAY_DESCENDING : Util.ARRAY_ASCENDING);
		UpdateTiles();
	}

	public void OnFleeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_flee"), Language.GetString("ui_flee_confirm", new string[1] { CurrencyRef.GetCurrencyName(_eventRef.getCurrencyID()) }), null, null, delegate
		{
			OnEventFleeConfirm();
		}, null, base.gameObject);
	}

	private void OnEventFleeConfirm()
	{
		switch (_eventRef.eventType)
		{
		case 1:
			DoPvPEventFlee();
			break;
		case 4:
			DoGvGEventFlee();
			break;
		}
	}

	private void DoPvPEventFlee()
	{
		PvPDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_FLEE), OnPvPEventFlee);
		PvPDALC.instance.doEventFlee();
	}

	private void OnPvPEventFlee(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		PvPDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(PvPDALC.EVENT_FLEE), OnPvPEventFlee);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			base.OnClose();
		}
	}

	private void DoGvGEventFlee()
	{
		GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnGvGEventFlee);
		GvGDALC.instance.doEventFlee();
	}

	private void OnGvGEventFlee(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnGvGEventFlee);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			base.OnClose();
		}
	}

	public CharacterData GetCharacterData(int charID)
	{
		foreach (EventTargetData target in _targets)
		{
			if (target.characterData.charID == charID)
			{
				return target.characterData;
			}
		}
		return null;
	}

	private void ResetOpponentSelected(object e)
	{
		_opponentSelected = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_START.RemoveListener(ResetOpponentSelected);
		base.DoDestroy();
	}
}
