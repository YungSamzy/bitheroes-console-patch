using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.parsing.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.team;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gvg;

public class GvGEventWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI costNameValueTxt;

	public TextMeshProUGUI characterNameTxt;

	public TextMeshProUGUI characterRankNameTxt;

	public TextMeshProUGUI characterRankValueTxt;

	public TextMeshProUGUI characterPointsNameTxt;

	public TextMeshProUGUI characterPointsValueTxt;

	public TextMeshProUGUI guildNameTxt;

	public TextMeshProUGUI guildRankNameTxt;

	public TextMeshProUGUI guildRankValueTxt;

	public TextMeshProUGUI guildPointsNameTxt;

	public TextMeshProUGUI guildPointsValueTxt;

	public Button playBtn;

	public Button lootBtn;

	public Button characterLeaderboardBtn;

	public Button characterRewardsBtn;

	public Button guildLeaderboardBtn;

	public Button guildRewardsBtn;

	public Button teamBtn;

	public Button helpBtn;

	public Button refreshBtn;

	public Button consumablesBtn;

	public Image guildBtn;

	public Image badgesBtn;

	public Image costDropdown;

	public CurrencyBarFill currencyFillBar;

	private List<MyDropdownItemModel> _bonuses = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedBonus;

	private bool bonusAllowed = true;

	private GvGEventRef _eventRef;

	private TimeBarColor timeBar;

	private Transform window;

	private const string BLANK = "-";

	private TrophyHandler trophyHandler;

	private bool showingCurrencyTime;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private int _characterRank;

	private int _characterPoints;

	private int _guildRank;

	private int _guildPoints;

	private int _zone;

	private bool isAllowedToEnter
	{
		get
		{
			if (_eventRef == null)
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
	}

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = EventRef.getEventTypeName(4);
		eventTxt.text = Language.GetString("event_end", new string[1] { EventRef.getEventTypeName(4) });
		costNameTxt.text = Language.GetString("ui_cost");
		characterNameTxt.text = Language.GetString("ui_player");
		characterRankNameTxt.text = Language.GetString("ui_rank");
		characterPointsNameTxt.text = Language.GetString("ui_points");
		guildNameTxt.text = Language.GetString("ui_guild");
		guildRankNameTxt.text = Language.GetString("ui_rank");
		guildPointsNameTxt.text = Language.GetString("ui_points");
		characterRankValueTxt.text = "-";
		characterPointsValueTxt.text = "-";
		guildRankValueTxt.text = "-";
		guildPointsValueTxt.text = "-";
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		lootBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
		characterLeaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		characterRewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		guildLeaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		guildRewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		teamBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_team");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		lootBtn.gameObject.SetActive(value: false);
		timeBar = GetComponentInChildren<TimeBarColor>();
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		currencyFillBar.Init();
		SetStats();
		SetLootID();
		UpdateEvent();
		UpdateButtons();
		DoEventLootCheck();
		DoEventStats();
		ListenForBack(OnClose);
		ListenForForward(OnPlayBtn);
		CreateWindow();
	}

	private void OnInventoryChange()
	{
		UpdateButtons();
	}

	private void UpdateBonusDropdown()
	{
		_bonuses.Clear();
		int gvGEventBonus = GameData.instance.SAVE_STATE.GetGvGEventBonus(GameData.instance.PROJECT.character.id);
		if (_eventRef != null)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.badges)
			};
			_bonuses.Add(myDropdownItemModel);
			_selectedBonus = myDropdownItemModel;
			for (int i = 0; i < GvGEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = GvGEventBook.LookupBonus(i);
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
					if (currencyBonusRef.id == gvGEventBonus)
					{
						_selectedBonus = myDropdownItemModel2;
					}
				}
			}
		}
		costNameValueTxt.text = ((_selectedBonus != null) ? _selectedBonus.title : "");
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
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(10)));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetGvGEventBonus(), OnCostDropdownClicked);
		componentInChildren.Data.InsertItemsAtStart(_bonuses);
	}

	public void OnCostDropdownClicked(MyDropdownItemModel model)
	{
		_selectedBonus = model;
		int bonus = ((model.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetGvGEventBonus(GameData.instance.PROJECT.character.id, bonus);
		costNameValueTxt.text = _selectedBonus.title;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
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
			GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnEventLootCheck);
			GvGDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnEventLootItems);
		GvGDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("GvG Event Reward", null, list, sfsob, "GvG Event", 2);
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
		if (lootBtn != null && lootBtn.gameObject != null)
		{
			lootBtn.gameObject.SetActive(_lootEventID >= 0);
		}
		if (playBtn != null && playBtn.gameObject != null)
		{
			playBtn.gameObject.SetActive(!lootBtn.gameObject.activeSelf);
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
				Util.SetButton(playBtn, enabled: false);
				GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnEventStats);
				GvGDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		_checkingStats = false;
		if (_eventRef != null)
		{
			Util.SetButton(playBtn);
		}
		DALCEvent obj = baseEvent as DALCEvent;
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnEventStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GvGEventCharacterData gvGEventCharacterData = GvGEventCharacterData.fromSFSObject(sfsob);
		SetStats(gvGEventCharacterData.rank, gvGEventCharacterData.points, gvGEventCharacterData.guildRank, gvGEventCharacterData.guildPoints);
		_zone = gvGEventCharacterData.zone;
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
		if (!base.disabled && isAllowedToEnter)
		{
			Disable();
			GameData.instance.main.ShowLoading();
			GvGDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
			GvGDALC.instance.doEventEnter(GetSelectedBonusID(), confirm);
		}
	}

	private void OnEventEnter(BaseEvent e)
	{
		Enable();
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		GvGDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
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
				GameData.instance.windowGenerator.ShowErrorCode(@int);
			}
		}
		else
		{
			List<EventTargetData> targets = EventTargetData.listFromSFSObject(sfsob);
			GameData.instance.windowGenerator.NewEventTargetWindow(_eventRef, targets, base.gameObject);
		}
	}

	private void UpdateEvent()
	{
		_eventRef = GvGEventBook.GetCurrentEventRef();
		bool flag = _eventRef != null;
		Util.SetButton(playBtn, flag);
		Util.SetButton(teamBtn, flag);
		Util.SetButton(characterLeaderboardBtn, flag);
		Util.SetButton(characterRewardsBtn, flag);
		Util.SetButton(guildLeaderboardBtn, flag);
		Util.SetButton(guildRewardsBtn, flag);
		string @string = Language.GetString("event_blank");
		if (flag)
		{
			_eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
			timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
			timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
			timeBar.COMPLETE.AddListener(OnEventTimerComplete);
		}
		else
		{
			GvGEventRef nextEventRef = GvGEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
			SetStats();
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
		UpdateBonusDropdown();
	}

	private void OnEventTimerComplete()
	{
		SetStats();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int characterRank = -1, int characterPoints = -1, int guildRank = -1, int guildPoints = -1)
	{
		_characterRank = characterRank;
		_characterPoints = characterPoints;
		_guildRank = guildRank;
		_guildPoints = guildPoints;
		characterRankValueTxt.text = ((characterRank > 0) ? Util.NumberFormat(characterRank, abbreviate: false) : "-");
		characterPointsValueTxt.text = ((characterRank > 0) ? Util.NumberFormat(characterPoints, abbreviate: false) : "-");
		guildRankValueTxt.text = ((guildRank > 0) ? Util.NumberFormat(guildRank, abbreviate: false) : "-");
		guildPointsValueTxt.text = ((guildRank > 0) ? Util.NumberFormat(guildPoints, abbreviate: false) : "-");
		UpdateBonusDropdown();
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventEnter();
	}

	public void OnLootBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEventLootItems();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	public void OnCharacterLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(4, 0, allowSegmented: true, base.gameObject);
	}

	public void OnCharacterRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(4, 2), _characterRank, _characterPoints, _zone, -1, alternate: false, 0L, base.gameObject);
	}

	public void OnGuildLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(4, 1, allowSegmented: false, base.gameObject);
	}

	public void OnGuildRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(4, 2), _guildRank, _guildPoints, -1, -1, alternate: true, 0L, base.gameObject);
	}

	public void OnGuildBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(4, 2, allowSegmented: false, base.gameObject);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(10), compare: false, added: true);
	}

	public void OnTeamBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTeamWindow(6, _eventRef.teamRules, delegate
		{
			CloseTeamWindow();
		}, base.gameObject);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(4), Util.parseMultiLine(Language.GetString("gvg_help_desc")), base.gameObject);
	}

	public void OnBadgesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(11);
	}

	private void CloseTeamWindow()
	{
		(GameData.instance.windowGenerator.GetDialogByClass(typeof(TeamWindow)) as TeamWindow).OnClose();
	}

	private int GetBadgeCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = GvGEventBook.LookupBonus(GameData.instance.SAVE_STATE.GetGvGEventBonus());
		return _eventRef.GetCurrencyCost(bonus);
	}

	private void UpdateButtons()
	{
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(10);
		consumablesBtn.gameObject.SetActive(consumablesByCurrencyID.Count > 0);
	}

	public override void DoDestroy()
	{
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtons(enabled: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtons(enabled: false);
	}

	private void SetButtons(bool enabled)
	{
		lootBtn.interactable = enabled;
		characterLeaderboardBtn.interactable = enabled;
		characterRewardsBtn.interactable = enabled;
		guildLeaderboardBtn.interactable = enabled;
		guildRewardsBtn.interactable = enabled;
		teamBtn.interactable = enabled;
		helpBtn.interactable = enabled;
		refreshBtn.interactable = enabled;
		consumablesBtn.interactable = enabled;
		playBtn.interactable = enabled;
		if (guildBtn.TryGetComponent<EventTrigger>(out var component))
		{
			component.enabled = enabled;
		}
		if (badgesBtn.TryGetComponent<EventTrigger>(out var component2))
		{
			component2.enabled = enabled;
		}
	}
}
