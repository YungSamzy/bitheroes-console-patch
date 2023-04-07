using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.lists.nodesinglemodifierlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneNodeSingleWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nodeTxt;

	public TextMeshProUGUI bonusesTxt;

	public TextMeshProUGUI energyTxt;

	public Button enterBtn;

	public Button chestBtn;

	private NodeSingleModifierList nodeSingleModifierList;

	private ZoneNodeRef _nodeRef;

	private ZoneNodeDifficultyRef _difficultyRef;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ZoneNodeRef nodeRef)
	{
		_nodeRef = nodeRef;
		_difficultyRef = _nodeRef.getFirstDifficultyRef();
		nodeSingleModifierList = GetComponentInChildren<NodeSingleModifierList>();
		topperTxt.text = Language.GetString("ui_dungeon");
		nodeTxt.text = Util.ParseString(nodeRef.name);
		bonusesTxt.text = Language.GetString("ui_bonuses");
		energyTxt.text = Util.NumberFormat(nodeRef.getFirstDifficultyRef().energy);
		enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enter");
		nodeSingleModifierList.InitList();
		foreach (GameModifier modifier in nodeRef.getFirstDifficultyRef().modifiers)
		{
			nodeSingleModifierList.Data.InsertOneAtEnd(new NodeSingleModifierItem
			{
				desc = modifier.GetTileDesc()
			});
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!GameData.instance.tutorialManager.hasPopup)
		{
			_ = base.gameObject.activeSelf;
		}
	}

	public void OnEnterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_difficultyRef.teamRules.slots <= 1)
		{
			GameData.instance.PROJECT.CheckTutorialChanges();
			List<TeammateData> list = new List<TeammateData>();
			list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			DoEnterZoneNode(list);
		}
		else
		{
			DoTeamSelect();
		}
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(1, _nodeRef.zoneID, _nodeRef.nodeID, "", 0);
	}

	private void DoTeamSelect()
	{
		GameData.instance.windowGenerator.NewTeamWindow(1, _difficultyRef.teamRules, OnTeamSelect, base.gameObject);
	}

	private void OnTeamSelect(TeamData teamData)
	{
		DoEnterZoneNode(teamData.teammates);
	}

	private void DoEnterZoneNode(List<TeammateData> teammates)
	{
		GameData.instance.main.ShowLoading();
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnEnterZoneNode);
		GameDALC.instance.doEnterZoneNode(_difficultyRef, teammates);
	}

	private void OnEnterZoneNode(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnEnterZoneNode);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.DUNGEONS_PLAYED);
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		enterBtn.interactable = true;
		chestBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		enterBtn.interactable = false;
		chestBtn.interactable = false;
	}
}
