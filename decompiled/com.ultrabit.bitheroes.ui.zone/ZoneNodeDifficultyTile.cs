using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.lists.zonebonuslist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneNodeDifficultyTile : MonoBehaviour
{
	private Button selectBtn;

	public Transform[] buttonPrefabs = new Transform[4];

	public Image energyIcon;

	public TextMeshProUGUI energyTxt;

	public TextMeshProUGUI bonusesTxt;

	public Image bg;

	public Image arrow;

	public Image placeholderStar;

	public Transform placeholderSelect;

	private ZoneBonusList zoneBonusList;

	private ZoneNodeDifficultyRef _difficultyRef;

	private ZoneNodeDifficultyWindow _zoneNodeDifficultyWindow;

	private StarHandler _starHandler;

	private AsianLanguageFontManager asianLangManager;

	public void LoadDetails(ZoneNodeDifficultyRef difficultyRef, ZoneNodeDifficultyWindow zoneNodeDifficultyWindow)
	{
		_difficultyRef = difficultyRef;
		_zoneNodeDifficultyWindow = zoneNodeDifficultyWindow;
		Transform transform = Object.Instantiate(buttonPrefabs[difficultyRef.difficultyRef.id]);
		transform.SetParent(placeholderSelect, worldPositionStays: false);
		transform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
		transform.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
		transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		selectBtn = transform.GetComponent<Button>();
		energyTxt.text = Util.NumberFormat(difficultyRef.energy);
		bonusesTxt.text = Language.GetString("ui_bonuses");
		_starHandler = placeholderStar.GetComponent<StarHandler>();
		ZoneData zoneData = GameData.instance.PROJECT.character.zones.getZoneData(difficultyRef.zoneID, difficultyRef.difficultyRef.id);
		if (zoneData != null)
		{
			_starHandler.ReplaceStar(placeholderStar, zoneData.getNodeCompleteCount(difficultyRef.nodeID) > 0);
		}
		else
		{
			_starHandler.ReplaceStar(placeholderStar, fill: false);
		}
		selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = difficultyRef.difficultyRef.name;
		if (difficultyRef == difficultyRef.getNodeRef().getLastDifficultyRef())
		{
			arrow.gameObject.SetActive(value: false);
		}
		if (!GameData.instance.PROJECT.character.zones.nodeDifficultyIsUnlocked(difficultyRef))
		{
			Util.SetButton(selectBtn, enabled: false);
		}
		else
		{
			selectBtn.onClick.AddListener(OnSelectBtn);
		}
		zoneBonusList = GetComponentInChildren<ZoneBonusList>();
		zoneBonusList.InitList();
		if (difficultyRef.modifiers != null)
		{
			foreach (GameModifier modifier in difficultyRef.modifiers)
			{
				zoneBonusList.Data.InsertOneAtEnd(new ZoneBonusItem
				{
					desc = modifier.GetTileDesc()
				});
			}
		}
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	public void OnSelectBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_difficultyRef.teamRules.slots <= 1)
		{
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
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(1, _difficultyRef.zoneID, _difficultyRef.nodeID, "", _difficultyRef.difficultyRef.id);
	}

	private void DoTeamSelect()
	{
		GameData.instance.windowGenerator.NewTeamWindow(1, _difficultyRef.teamRules, OnTeamSelect, _zoneNodeDifficultyWindow.gameObject);
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
	}

	public void OnDestroy()
	{
		if (selectBtn != null)
		{
			selectBtn.onClick.RemoveListener(OnSelectBtn);
		}
	}

	public void DoEnable()
	{
		if (selectBtn != null)
		{
			selectBtn.interactable = true;
		}
	}

	public void DoDisable()
	{
		if (selectBtn != null)
		{
			selectBtn.interactable = false;
		}
	}
}
