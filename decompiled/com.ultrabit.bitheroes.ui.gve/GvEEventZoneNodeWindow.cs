using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.parsing.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.gvenoderewardslist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gve;

public class GvEEventZoneNodeWindow : WindowsMain
{
	public static int RECENT_NODE = -1;

	private const string BLANK = "-";

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public TextMeshProUGUI rewardsTxt;

	public TextMeshProUGUI progressNameTxt;

	public TimeBarColor progressBar;

	public Button refreshBtn;

	public Button consumablesBtn;

	public Button enterBtn;

	public Button chestBtn;

	public Image costDropdown;

	public TextMeshProUGUI costTxt;

	public Image difficultyDropdown;

	public TextMeshProUGUI difficultyTxt;

	public Image badgesBtn;

	public CurrencyBarFill currencyBarFill;

	public GvENodeRewardsList gvENodeRewardsList;

	private GvEEventRef _eventRef;

	private GvEZoneNodeRef _nodeRef;

	private bool _checkingData;

	private int _nodePoints;

	private int _highest;

	private int _difficulty;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private Transform dropdownWindow;

	private bool difficultyEnabled;

	private bool costEnabled;

	private MyDropdownItemModel _selectedDifficulty;

	private List<MyDropdownItemModel> _difficulties = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedBonus;

	private List<MyDropdownItemModel> _bonuses = new List<MyDropdownItemModel>();

	private TeamData _selectedTeam;

	public override void Start()
	{
		base.Start();
		progressBar.enabled = false;
	}

	public void LoadDetails(GvEEventRef eventRef, GvEZoneNodeRef nodeRef)
	{
		Disable();
		_eventRef = eventRef;
		_nodeRef = nodeRef;
		topperTxt.text = _nodeRef.name;
		costNameTxt.text = Language.GetString("ui_cost");
		difficultyNameTxt.text = Language.GetString("ui_difficulty");
		rewardsTxt.text = Language.GetString("ui_guild_rewards");
		progressNameTxt.text = Language.GetString("ui_progress");
		enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enter");
		gvENodeRewardsList.InitList();
		currencyBarFill.Init();
		CreateItems();
		UpdateBonusDropdown();
		DoEventZoneNodeData();
		ListenForBack(OnClose);
		ListenForForward(OnEnterBtn);
		CreateWindow();
	}

	private void DoEventZoneNodeData()
	{
		if (!_checkingData)
		{
			Util.SetButton(enterBtn, enabled: false);
			SetStats();
			RestartRefreshTimer();
			_checkingData = true;
			GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnEventZoneNodeData);
			GvEDALC.instance.doEventZoneNodeData(_eventRef.id, _nodeRef.nodeID);
		}
	}

	private void OnEventZoneNodeData(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		_checkingData = false;
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnEventZoneNodeData);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GvEEventZoneData gvEEventZoneData = GvEEventZoneData.fromSFSObject(sfsob);
		int @int = sfsob.GetInt("eve10");
		int int2 = sfsob.GetInt("eve11");
		GvEEventZoneWindow gvEEventZoneWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(GvEEventZoneWindow)) as GvEEventZoneWindow;
		if ((bool)gvEEventZoneWindow)
		{
			gvEEventZoneWindow.SetZoneData(gvEEventZoneData);
		}
		SetStats(gvEEventZoneData.getNodePoints(_nodeRef.nodeID), @int, int2);
		Util.SetButton(enterBtn);
	}

	private void SetStats(int nodePoints = -1, int highest = -1, int difficulty = -1)
	{
		_nodePoints = nodePoints;
		_highest = highest;
		_difficulty = difficulty;
		UpdateProgressBar();
		UpdateDifficultyDropdown();
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

	private void CreateItems()
	{
		if (gvENodeRewardsList.Data.Count > 0 || _nodeRef.rewards == null || _nodeRef.rewards.Count <= 0)
		{
			return;
		}
		foreach (ItemData reward in _nodeRef.rewards)
		{
			if (reward != null)
			{
				gvENodeRewardsList.Data.InsertOneAtEnd(new GvENodeRewardsItem
				{
					itemData = reward
				});
			}
		}
	}

	private void UpdateProgressBar()
	{
		float num = _nodePoints;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > (float)_nodeRef.points)
		{
			num = _nodeRef.points;
		}
		progressBar.SetMaxValueSeconds(_nodeRef.points);
		progressBar.SetCurrentValueSeconds(num);
		progressBar.ForceStart(invokeSeconds: false);
		progressBar.ShowColorWithoutTimer();
		progressBar.text = Util.NumberFormat(num) + " / " + Util.NumberFormat(_nodeRef.points);
	}

	private void UpdateDifficultyDropdown()
	{
		int num = GameData.instance.SAVE_STATE.GetGvEEventDifficulty(GameData.instance.PROJECT.character.id);
		if (num <= 0)
		{
			num = _difficulty;
		}
		for (int num2 = _difficulty; num2 > 0; num2--)
		{
			if (Util.multiple(num2, VariableBook.gveEventDifficultyIntervals))
			{
				int num3 = num2;
				string title = Util.NumberFormat(num3);
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
				{
					id = num2,
					title = title,
					data = num3
				};
				_difficulties.Add(myDropdownItemModel);
				if (num3 == num)
				{
					_selectedDifficulty = myDropdownItemModel;
				}
			}
		}
		difficultyTxt.text = ((_selectedDifficulty != null) ? _selectedDifficulty.title : "-");
		if (_checkingData || _difficulty <= 0)
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
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetGvEEventDifficulty(GameData.instance.PROJECT.character.id), OnDifficultySelected);
		componentInChildren.Data.InsertItemsAtEnd(_difficulties);
	}

	public void OnDifficultySelected(MyDropdownItemModel model)
	{
		difficultyTxt.text = model.title;
		GameData.instance.SAVE_STATE.SetGvEEventDifficulty((model.data as int?).Value);
		_selectedDifficulty = model;
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void UpdateBonusDropdown()
	{
		int gvEEventBonus = GameData.instance.SAVE_STATE.GetGvEEventBonus(GameData.instance.PROJECT.character.id);
		if (_eventRef != null)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.badges)
			};
			_bonuses.Add(myDropdownItemModel);
			_selectedBonus = myDropdownItemModel;
			for (int i = 0; i < GvEEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = GvEEventBook.LookupBonus(i);
				if (currencyBonusRef != null)
				{
					int currencyCost = _eventRef.GetCurrencyCost(currencyBonusRef);
					MyDropdownItemModel myDropdownItemModel2 = new MyDropdownItemModel
					{
						id = i,
						title = Util.NumberFormat(currencyCost),
						data = currencyBonusRef,
						btnHelp = true
					};
					_bonuses.Add(myDropdownItemModel2);
					if (currencyBonusRef.id == gvEEventBonus)
					{
						_selectedBonus = myDropdownItemModel2;
					}
				}
			}
		}
		costTxt.text = ((_selectedBonus != null) ? _selectedBonus.title : "");
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
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(10)));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetGvEEventBonus(), OnCostDropdownChange);
		componentInChildren.Data.InsertItemsAtStart(_bonuses);
	}

	public void OnCostDropdownChange(MyDropdownItemModel model)
	{
		int bonus = ((model.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetGvEEventBonus(GameData.instance.PROJECT.character.id, bonus);
		_selectedBonus = model;
		costTxt.text = _selectedBonus.title;
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
		GvEEventWindow gvEEventWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(GvEEventWindow)) as GvEEventWindow;
		if ((bool)gvEEventWindow)
		{
			gvEEventWindow.UpdateBonusDropdown();
		}
	}

	private bool DoAllowEnter()
	{
		GvEEventRef currentEventRef = GvEEventBook.GetCurrentEventRef();
		if (currentEventRef == null || currentEventRef.id != _eventRef.id || currentEventRef.eventType != _eventRef.eventType)
		{
			GameData.instance.windowGenerator.ShowErrorCode(27);
			return false;
		}
		if (GameData.instance.PROJECT.character.badges < GetBadgeCost())
		{
			GameData.instance.windowGenerator.ShowErrorCode(104);
			return false;
		}
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return false;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return false;
		}
		return true;
	}

	private void DoTeamSelect()
	{
		if (DoAllowEnter())
		{
			_selectedTeam = null;
			GameData.instance.windowGenerator.NewTeamWindow(8, _eventRef.teamRules, OnTeamSelect, base.gameObject);
		}
	}

	private void OnTeamSelect(TeamData teamData)
	{
		_selectedTeam = teamData;
		DoEventEnter();
	}

	private void DoEventEnterConfirm(string message)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), message, null, null, delegate
		{
			DoEventEnter(confirm: true);
		});
	}

	private void DoEventEnter(bool confirm = false)
	{
		if (!DoAllowEnter())
		{
			return;
		}
		if (_selectedDifficulty == null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(0);
			return;
		}
		int value = (_selectedDifficulty.data as int?).Value;
		if (_selectedTeam == null)
		{
			_selectedTeam = GameData.instance.PROJECT.character.teams.getTeam(8, _eventRef.teamRules);
		}
		if (_selectedTeam == null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(0);
			return;
		}
		GameData.instance.main.ShowLoading();
		GvEDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
		GvEDALC.instance.doEventEnter(_nodeRef.nodeID, value, GetSelectedBonusID(), _selectedTeam.teammates, confirm);
	}

	private void OnEventEnter(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GvEDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("err0");
			if (@int == 108)
			{
				DoEventEnterConfirm(ErrorCode.getErrorMessage(@int));
			}
			else
			{
				GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			}
		}
		else
		{
			RECENT_NODE = _nodeRef.nodeID;
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

	private int GetBadgeCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = GvEEventBook.LookupBonus(GetSelectedBonusID());
		return _eventRef.GetCurrencyCost(bonus);
	}

	public void OnBadgesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(11);
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventZoneNodeData();
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(10), compare: false, added: true);
	}

	public void OnEnterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoTeamSelect();
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(11, _nodeRef.zoneID, _nodeRef.nodeID, "", 0);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		enterBtn.interactable = true;
		refreshBtn.interactable = true;
		consumablesBtn.interactable = true;
		chestBtn.interactable = true;
		costDropdown.GetComponent<EventTrigger>().enabled = true;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = true;
		badgesBtn.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		enterBtn.interactable = false;
		refreshBtn.interactable = false;
		consumablesBtn.interactable = false;
		chestBtn.interactable = false;
		costDropdown.GetComponent<EventTrigger>().enabled = false;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = false;
		badgesBtn.GetComponent<EventTrigger>().enabled = false;
	}
}
