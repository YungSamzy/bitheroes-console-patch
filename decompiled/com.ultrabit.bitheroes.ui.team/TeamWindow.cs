using System.Collections.Generic;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.MultiplePrefabsTeamList;
using com.ultrabit.bitheroes.ui.lists.teamlist;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.team;

public class TeamWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI statsTxt;

	public Button autoBtn;

	public Button clearBtn;

	public Button acceptBtn;

	public Button statsBG;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private int _type;

	private TeamRules _teamRules;

	private TeamList teamList;

	private MultiplePrefabsTeamList multipleTeamList;

	private int _selectingSlot;

	private bool _groups;

	private bool _showArmoryButton;

	private bool _clicked;

	private UnityAction<TeamData> onTeamSelectedComplete;

	private TeamData teamData;

	public TeamRules teamRules => _teamRules;

	public override void Start()
	{
		base.Start();
		Disable();
		_clicked = false;
	}

	public void LoadDetails(int type, TeamRules teamRules, UnityAction<TeamData> onTeamSelectedComplete, bool showArmoryButton = true)
	{
		_type = type;
		_teamRules = teamRules;
		_showArmoryButton = showArmoryButton;
		this.onTeamSelectedComplete = onTeamSelectedComplete;
		topperTxt.text = Language.GetString("ui_team");
		nameListTxt.text = Language.GetString("ui_name");
		autoBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_auto");
		acceptBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
		clearBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_clear");
		_groups = _teamRules.size < _teamRules.slots;
		if (!_groups)
		{
			Object.Destroy(GetComponentInChildren<MultiplePrefabsTeamList>().gameObject);
			(teamList = GetComponentInChildren<TeamList>()).enabled = true;
			teamList.InitList(OnRemoveButtonClicked, OnAddButtonClicked, this, _showArmoryButton);
		}
		else
		{
			Object.Destroy(GetComponentInChildren<TeamList>().gameObject);
			(multipleTeamList = GetComponentInChildren<MultiplePrefabsTeamList>()).enabled = true;
			multipleTeamList.InitList(_teamRules.slots / _teamRules.size, _teamRules.slots);
		}
		UpdateTiles(GetTeamMates(allowArmory: false));
		if (!GameData.instance.PROJECT.character.teams.hasTeam(type, teamRules) && GameData.instance.PROJECT.character.tutorial.GetState(93))
		{
			AutoAssignTiles();
		}
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		ListenForForward(OnAcceptBtn);
		forceAnimation = true;
		CreateWindow();
	}

	public override void OnClose()
	{
		base.OnClose();
		GameData.instance.PROJECT.character.armory.battleArmorySelected = false;
	}

	public override void UpdateSortingLayers(int slayer)
	{
		base.UpdateSortingLayers(slayer);
		if (!_groups)
		{
			topMask.frontSortingOrder = 1 + base.sortingLayer + 98;
			bottomMask.frontSortingOrder = 1 + base.sortingLayer + 98;
			foreach (MyViewsHolder visibleItem in teamList._VisibleItems)
			{
				teamList.UpdateListItem(visibleItem);
			}
		}
		else
		{
			topMask.frontSortingOrder = 1 + base.sortingLayer + 98;
			bottomMask.frontSortingOrder = 1 + base.sortingLayer + 98;
			multipleTeamList.Refresh();
		}
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = slayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = slayer;
	}

	private void OnScrollInComplete(object e)
	{
		CheckTutorial();
	}

	public void CheckTutorial(object e = null)
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("teammate_add");
		if (tutorialRef != null && tutorialRef.areConditionsMet && !GameData.instance.PROJECT.character.tutorial.GetState(93) && _teamRules.allowFamiliars && !_groups)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(93);
			foreach (MyViewsHolder visibleItem in teamList._VisibleItems)
			{
				if (visibleItem.addBtn.gameObject.activeSelf)
				{
					GameData.instance.tutorialManager.ShowTutorialForButton(visibleItem.addBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(93), 4, visibleItem.addBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
					GameData.instance.PROJECT.CheckTutorialChanges();
					return;
				}
			}
		}
		TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("fusion_add");
		if (tutorialRef2 != null && tutorialRef2.areConditionsMet && GameData.instance.PROJECT.character.inventory.getItem(VariableBook.tutorialBootyId, 6) != null && !GameData.instance.PROJECT.character.tutorial.GetState(121) && _teamRules.allowFamiliars && !_groups)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(121);
			foreach (MyViewsHolder visibleItem2 in teamList._VisibleItems)
			{
				if (visibleItem2.addBtn.gameObject.activeSelf)
				{
					GameData.instance.tutorialManager.ShowTutorialForButton(visibleItem2.addBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(121), 4, visibleItem2.addBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
					GameData.instance.PROJECT.CheckTutorialChanges();
					return;
				}
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(20) && _teamRules.allowFamiliars)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(20);
			GameData.instance.tutorialManager.ShowTutorialForButton(acceptBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(20), 4, acceptBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, CheckTutorial, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void OnRemoveButtonClicked()
	{
		UpdateStats();
	}

	private void OnAddButtonClicked(int slot)
	{
		List<TeammateData> teammates = GameData.instance.PROJECT.character.getTeammates(_teamRules);
		List<TeammateData> tileTeammates = GetTileTeammates();
		_selectingSlot = slot;
		GameData.instance.windowGenerator.NewTeamSelectWindow(teammates, tileTeammates, _teamRules, OnTeammateSelected, base.gameObject);
	}

	private void OnTeammateSelected(TeammateData teammateSelected)
	{
		if (!_groups)
		{
			teamList.Data[_selectingSlot].teammateData = teammateSelected;
			_selectingSlot = -1;
			UpdateTiles(teamList.GetTeammateDataList());
		}
		else if (multipleTeamList.Data[_selectingSlot].GetType() == typeof(TeamListItemModel))
		{
			(multipleTeamList.Data[_selectingSlot] as TeamListItemModel).teammateData = teammateSelected;
			_selectingSlot = -1;
			UpdateTiles(multipleTeamList.GetTeammateDataList());
		}
	}

	public List<TeammateData> GetTileTeammates()
	{
		List<TeammateData> list = new List<TeammateData>();
		if (!_groups)
		{
			for (int i = 0; i < teamList.Data.Count; i++)
			{
				list.Add(teamList.Data[i].teammateData);
			}
		}
		else
		{
			for (int j = 0; j < multipleTeamList.Data.Count; j++)
			{
				if (multipleTeamList.Data[j].GetType() == typeof(TeamListItemModel))
				{
					list.Add((multipleTeamList.Data[j] as TeamListItemModel).teammateData);
				}
			}
		}
		return list;
	}

	private void UpdateTiles(List<TeammateData> myTeam)
	{
		UpdateTilesCoroutine(myTeam);
	}

	private void UpdateTilesCoroutine(List<TeammateData> myTeam)
	{
		if (_groups)
		{
			_ = _teamRules.size;
		}
		double virtualAbstractNormalizedScrollPosition;
		if (!_groups)
		{
			virtualAbstractNormalizedScrollPosition = teamList.GetVirtualAbstractNormalizedScrollPosition();
			teamList.ClearList();
		}
		else
		{
			virtualAbstractNormalizedScrollPosition = multipleTeamList.GetVirtualAbstractNormalizedScrollPosition();
			multipleTeamList.ClearList();
		}
		List<TeamListItemModel> list = new List<TeamListItemModel>();
		List<BaseModel> list2 = new List<BaseModel>();
		int num = int.MinValue;
		int num2 = int.MinValue;
		int num3 = int.MinValue;
		int num4 = int.MinValue;
		int num5 = int.MaxValue;
		int num6 = int.MaxValue;
		int num7 = int.MaxValue;
		int num8 = int.MaxValue;
		for (int i = 0; i < _teamRules.slots; i++)
		{
			TeammateData teammateData = null;
			if (myTeam.Count > i)
			{
				teammateData = myTeam[i];
			}
			if (teammateData != null)
			{
				teammateData.armoryID = -1L;
				teammateData.SetBattleType(_type);
				int num9 = 0;
				int power = GetPower(teammateData);
				int stamina = GetStamina(teammateData);
				int agility = GetAgility(teammateData);
				if (num9 > num)
				{
					num = num9;
				}
				if (num9 < num5)
				{
					num5 = num9;
				}
				if (power > num2)
				{
					num2 = power;
				}
				if (power < num6)
				{
					num6 = power;
				}
				if (stamina > num3)
				{
					num3 = stamina;
				}
				if (stamina < num7)
				{
					num7 = stamina;
				}
				if (agility > num4)
				{
					num4 = agility;
				}
				if (agility < num8)
				{
					num8 = agility;
				}
			}
			if (!_groups)
			{
				list.Add(new TeamListItemModel
				{
					slot = i,
					teammateData = teammateData,
					teamWindow = this
				});
			}
			else
			{
				list2.Add(new TeamListItemModel
				{
					slot = i,
					teammateData = teammateData,
					onAddButtonClicked = OnAddButtonClicked,
					onRemoveButtonClicked = OnRemoveButtonClicked,
					teamWindow = this
				});
			}
		}
		if (!_groups)
		{
			teamList.SetReferenceStats(num, num5, num2, num6, num3, num7, num4, num8);
			teamList.Data.InsertItems(0, list);
		}
		else
		{
			TeamTileVH.SetReferenceStats(num, num5, num2, num6, num3, num7, num4, num8);
			multipleTeamList.Data.InsertItems(0, list2);
			for (int j = 0; j < _teamRules.slots / _teamRules.size; j++)
			{
				GroupTileModel groupTileModel = new GroupTileModel();
				groupTileModel.titleGroup = Language.GetString("ui_group_count", new string[1] { (j + 1).ToString() });
				BaseModel model = groupTileModel;
				multipleTeamList.Data.InsertOne(j * (_teamRules.size + 1), model);
			}
		}
		bool looped;
		if (!_groups)
		{
			teamList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out looped);
		}
		else
		{
			multipleTeamList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out looped);
		}
		UpdateStats();
		UpdateSortingLayers(base.sortingLayer);
	}

	private void AutoAssignTiles()
	{
		UpdateTiles(GameData.instance.PROJECT.character.getAutoAssignedTeam(_teamRules, _type));
	}

	private void ClearTiles()
	{
		List<TeammateData> list = new List<TeammateData>();
		list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
		UpdateTiles(list);
	}

	private void UpdateStats()
	{
		int num = 0;
		if (!_groups)
		{
			for (int i = 0; i < teamList.Data.Count; i++)
			{
				if (teamList.Data[i].teammateData != null)
				{
					num += GetTotal(teamList.Data[i].teammateData);
				}
			}
		}
		else
		{
			for (int j = 0; j < multipleTeamList.Data.Count; j++)
			{
				if (multipleTeamList.Data[j].GetType() == typeof(TeamListItemModel) && (multipleTeamList.Data[j] as TeamListItemModel).teammateData != null)
				{
					num += GetTotal((multipleTeamList.Data[j] as TeamListItemModel).teammateData);
				}
			}
		}
		statsTxt.text = Util.NumberFormat(num, abbreviate: false);
	}

	public List<TeammateData> GetTeamMates(bool allowArmory = true)
	{
		List<TeammateData> teammates = GameData.instance.PROJECT.character.teams.getTeammates(_type, _teamRules);
		List<TeammateData> list = new List<TeammateData>();
		if (teammates == null || teammates.Count <= 0)
		{
			list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			return list;
		}
		Character character = GameData.instance.PROJECT.character;
		for (int i = 0; i < teammates.Count; i++)
		{
			TeammateData teammateData = teammates[i];
			if (teammateData == null || teammateData.data == null)
			{
				continue;
			}
			switch (teammateData.type)
			{
			case 1:
				if (teammateData.id != character.id)
				{
					bool flag = character.getFriendData(teammateData.id) != null;
					bool flag2 = character.guildData != null && character.guildData.getMember(teammateData.id) != null;
					if ((!(_teamRules.allowGuildmates && flag2) && !(_teamRules.allowFriends && flag)) || character.getContact(teammateData.id) == null)
					{
						continue;
					}
					if (!allowArmory)
					{
						teammateData.armoryID = -1L;
					}
				}
				break;
			case 2:
			{
				FamiliarRef familiarRef = FamiliarBook.Lookup(teammateData.id);
				int teammateCount = GetTeammateCount(teammateData, list);
				if (_teamRules.hasFamiliarAdded(familiarRef))
				{
					if (teammateCount > 1)
					{
						continue;
					}
					break;
				}
				if (!_teamRules.allowFamiliars)
				{
					continue;
				}
				int itemQty = character.getItemQty(familiarRef);
				if (teammateCount >= itemQty)
				{
					continue;
				}
				break;
			}
			}
			list.Add(teammateData);
		}
		return list;
	}

	public int GetTeammateCount(TeammateData teammateData, List<TeammateData> myTeam)
	{
		int num = 0;
		if (!_groups)
		{
			for (int i = 0; i < myTeam.Count; i++)
			{
				if (myTeam[i] != null && myTeam[i].id == teammateData.id && myTeam[i].type == teammateData.type)
				{
					num++;
				}
			}
		}
		else
		{
			for (int j = 0; j < multipleTeamList.Data.Count; j++)
			{
				if (multipleTeamList.Data[j].GetType() == typeof(TeamListItemModel) && (multipleTeamList.Data[j] as TeamListItemModel).teammateData != null && (multipleTeamList.Data[j] as TeamListItemModel).teammateData.id == teammateData.id && (multipleTeamList.Data[j] as TeamListItemModel).teammateData.type == teammateData.type)
				{
					num++;
				}
			}
		}
		return num;
	}

	public void OnStatsBG()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_stats"), Language.GetString("ui_total_stats"));
	}

	public void OnAutoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AutoAssignTiles();
	}

	public void OnClearBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ClearTiles();
	}

	public void OnAcceptBtn()
	{
		if (_clicked)
		{
			return;
		}
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int emptyTiles = GetEmptyTiles();
		if (_groups && emptyTiles > 0)
		{
			bool flag = false;
			for (int i = 0; i < multipleTeamList.Data.Count; i++)
			{
				if (!(multipleTeamList.Data[i].GetType() == typeof(TeamListItemModel)))
				{
					continue;
				}
				bool flag2 = (multipleTeamList.Data[i] as TeamListItemModel).teammateData == null;
				if (flag2)
				{
					flag = true;
				}
				else if (flag && !flag2)
				{
					GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_team"), Language.GetString("ui_team_reorder_confirm"), null, null, delegate
					{
						orderTiles();
					});
					return;
				}
			}
		}
		if (emptyTiles > 0)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_team"), Language.GetString("ui_team_not_full_confirm"), null, null, delegate
			{
				SaveSlots();
			});
		}
		else
		{
			SaveSlots();
		}
	}

	private void SaveSlots()
	{
		bool flag = false;
		if (!_groups)
		{
			teamData = GameData.instance.PROJECT.character.teams.setTeam(_type, _teamRules, teamList.GetTeammateDataList());
		}
		else
		{
			teamData = GameData.instance.PROJECT.character.teams.setTeam(_type, _teamRules, multipleTeamList.GetTeammateDataList());
		}
		CharacterDALC.instance.doSaveTeam(teamData);
		List<TeammateData> teammates = teamData.teammates;
		if (GameData.instance.PROJECT.character.armory.battleArmorySelected && GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot != null && GameData.instance.PROJECT.character.armory.armoryEquipmentSlots.Count > 0 && GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.unlocked)
		{
			GameData.instance.PROJECT.character.rerunArmoryID = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.id;
			GameData.instance.PROJECT.character.equipCurrentArmorySlot();
			GameData.instance.PROJECT.character.armory.battleArmorySelected = false;
			flag = true;
		}
		else
		{
			for (int i = 0; i < teammates.Count; i++)
			{
				if (teammates[i] != null && teammates[i].id == GameData.instance.PROJECT.character.id && teammates[i].armoryID > 0)
				{
					GameData.instance.PROJECT.character.rerunArmoryID = teammates[i].armoryID;
					GameData.instance.PROJECT.character.armory.SetCurrentArmoryEquipmentSlotByID(teammates[i].armoryID);
					GameData.instance.PROJECT.character.equipCurrentArmorySlot();
					flag = true;
				}
			}
		}
		GameData.instance.PROJECT.character.lastEventType = teamData.type;
		_clicked = true;
		if (flag)
		{
			GameData.instance.main.ShowLoading();
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(57), onCharacterEquipmentGlobalSave);
		}
		else
		{
			onTeamSelectedComplete(teamData);
			Invoke("RestoreClicked", 1f);
		}
	}

	private void onCharacterEquipmentGlobalSave(BaseEvent e)
	{
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(57), onCharacterEquipmentGlobalSave);
		onTeamSelectedComplete(teamData);
		RestoreClicked();
	}

	private void RestoreClicked()
	{
		_clicked = false;
	}

	private void orderTiles()
	{
		for (int i = 0; i < multipleTeamList.Data.Count; i++)
		{
			if (!(multipleTeamList.Data[i].GetType() == typeof(TeamListItemModel)) || (multipleTeamList.Data[i] as TeamListItemModel).teammateData != null)
			{
				continue;
			}
			for (int j = i; j < multipleTeamList.Data.Count; j++)
			{
				if (multipleTeamList.Data[j].GetType() == typeof(TeamListItemModel) && (multipleTeamList.Data[j] as TeamListItemModel).teammateData != null)
				{
					multipleTeamList.Data.Swap(i, j);
					break;
				}
			}
		}
	}

	private int GetEmptyTiles()
	{
		int num = 0;
		if (!_groups)
		{
			for (int i = 0; i < teamList.Data.Count; i++)
			{
				if (teamList.Data[i].teammateData == null)
				{
					num++;
				}
			}
		}
		else
		{
			for (int j = 0; j < multipleTeamList.Data.Count; j++)
			{
				if (multipleTeamList.Data[j].GetType() == typeof(TeamListItemModel) && (multipleTeamList.Data[j] as TeamListItemModel).teammateData == null)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetPower(TeammateData data)
	{
		return GetStats(data).power;
	}

	public int GetStamina(TeammateData data)
	{
		return GetStats(data).stamina;
	}

	public int GetAgility(TeammateData data)
	{
		return GetStats(data).agility;
	}

	public int GetTotal(TeammateData data)
	{
		return GetPower(data) + GetStamina(data) + GetAgility(data);
	}

	private CharacterStats GetStats(TeammateData data)
	{
		switch (data.type)
		{
		case 1:
			if (_teamRules.statBalance)
			{
				return GameData.instance.PROJECT.character.getStatBalance(data.power, data.stamina, data.agility);
			}
			if (data.id == GameData.instance.PROJECT.character.id && data.armoryID < 1)
			{
				return new CharacterStats(GameData.instance.PROJECT.character.getTotalPower(), GameData.instance.PROJECT.character.getTotalStamina(), GameData.instance.PROJECT.character.getTotalAgility());
			}
			break;
		case 2:
		{
			FamiliarRef familiarRef = FamiliarBook.Lookup(data.id);
			float familiarMult = _teamRules.getFamiliarMult(familiarRef);
			float num = (float)data.power * familiarMult;
			float num2 = (float)data.stamina * familiarMult;
			float num3 = (float)data.agility * familiarMult;
			return new CharacterStats((int)num, (int)num2, (int)num3);
		}
		}
		return new CharacterStats(data.power, data.stamina, data.agility);
	}

	public override void DoDestroy()
	{
		CancelInvoke("RestoreClicked");
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public void DestroyAnything(GameObject toDestroy)
	{
		Object.Destroy(toDestroy);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (autoBtn != null && autoBtn.gameObject != null)
		{
			autoBtn.interactable = state;
		}
		if (clearBtn != null && clearBtn.gameObject != null)
		{
			clearBtn.interactable = state;
		}
		if (acceptBtn != null && acceptBtn.gameObject != null)
		{
			acceptBtn.interactable = state;
		}
		if (statsBG != null && statsBG.gameObject != null)
		{
			statsBG.interactable = state;
		}
	}
}
