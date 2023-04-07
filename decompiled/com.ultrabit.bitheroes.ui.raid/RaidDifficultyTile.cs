using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.ui.lists.raidbonuslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.raid;

public class RaidDifficultyTile : MonoBehaviour
{
	public RaidBonusList raidBonusList;

	public TextMeshProUGUI txtShards;

	public TextMeshProUGUI txtDifficultyButtonLabel;

	public TextMeshProUGUI txtBonusesLabel;

	private RaidDifficultyRef _difficultyRef;

	private RaidRef _raidRef;

	private RaidDifficultyWindow _raidDifficultyWindow;

	public void SetDifficultyRef(RaidRef raidRef, RaidDifficultyRef raidDifficultyRef, RaidDifficultyWindow raidDifficultyWindow)
	{
		_difficultyRef = raidDifficultyRef;
		_raidRef = raidRef;
		_raidDifficultyWindow = raidDifficultyWindow;
		txtShards.text = raidDifficultyRef.shards.ToString();
		txtDifficultyButtonLabel.text = Language.GetString(raidDifficultyRef.difficultyRef.name).ToUpperInvariant();
		txtBonusesLabel.text = Language.GetString("ui_bonuses");
		List<GameModifier> modifiers = raidDifficultyRef.modifiers;
		if (modifiers.Count <= 0)
		{
			return;
		}
		raidBonusList.InitList();
		if (raidBonusList.Data.Count > 0)
		{
			raidBonusList.Data.RemoveItems(0, raidBonusList.Data.Count);
		}
		foreach (GameModifier item in modifiers)
		{
			raidBonusList.Data.InsertOneAtEnd(new RaidBonusItem
			{
				bonus = item.GetTileDesc()
			});
		}
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(4, _raidRef.id, 0, "", _difficultyRef.difficultyRef.id);
	}

	public void OnClickPlay()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_difficultyRef.teamRules.slots <= 1)
		{
			List<TeammateData> list = new List<TeammateData>();
			list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			DoEnterRaid(list);
		}
		else
		{
			DoTeamSelect();
		}
	}

	private void DoTeamSelect()
	{
		GameData.instance.windowGenerator.NewTeamWindow(3, _difficultyRef.teamRules, OnTeamSelect, _raidDifficultyWindow.gameObject);
	}

	private void OnTeamSelect(TeamData teamData)
	{
		DoEnterRaid(teamData.teammates);
	}

	private void DoEnterRaid(List<TeammateData> teammates)
	{
		GameData.instance.main.ShowLoading();
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnEnterRaid);
		GameDALC.instance.doEnterRaid(_difficultyRef, teammates);
	}

	private void OnEnterRaid(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnEnterRaid);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			KongregateAnalytics.TrackCPEEvent("kong_play_raid");
			GameData.instance.PROJECT.character.analytics.incrementValue(BHAnalytics.DUNGEONS_PLAYED);
		}
	}
}
