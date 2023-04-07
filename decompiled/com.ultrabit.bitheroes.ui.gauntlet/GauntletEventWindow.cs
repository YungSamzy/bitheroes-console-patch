using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rift;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gauntlet;

public class GauntletEventWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI costNameValueTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public TextMeshProUGUI difficultyNameValueTxt;

	public TextMeshProUGUI rankNameTxt;

	public TextMeshProUGUI rankValueTxt;

	public TextMeshProUGUI typeNameTxt;

	public TextMeshProUGUI typeValueTxt;

	public TextMeshProUGUI altNameTxt;

	public TextMeshProUGUI altValueTxt;

	public TextMeshProUGUI tokensTxts;

	public CurrencyBarFill currencyBarFill;

	public Image trophyImage;

	public Button playBtn;

	public Button lootBtn;

	public Button leaderboardBtn;

	public Button rewardsBtn;

	public Button helpBtn;

	public Button refreshBtn;

	public Button consumablesBtn;

	public Button chestBtn;

	public Image costDropdown;

	public Image difficultyDropdown;

	public Image tokensBtn;

	private GauntletEventRef _eventRef;

	private Transform window;

	private Transform difficultyCategory;

	private TimeBarColor timeBar;

	private int rank;

	private int points;

	private int highest;

	private int _difficulty;

	private const string BLANK = "-";

	private List<MyDropdownItemModel> difficultyData;

	private int _difficultySelected;

	private bool difficultyAllowed = true;

	private List<MyDropdownItemModel> bonusData;

	private MyDropdownItemModel selectedBonus;

	private bool bonusAllowed = true;

	private TrophyHandler trophyHandler;

	private bool _checkingLoot;

	private bool _checkingStats;

	private int _lootEventID;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private int _zone;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = EventRef.getEventTypeName(3);
		TextMeshProUGUI textMeshProUGUI = eventTxt;
		string text = (eventTxt.text = Language.GetString("event_end", new string[1] { EventRef.getEventTypeName(3) }));
		textMeshProUGUI.text = text;
		costNameTxt.text = Language.GetString("ui_cost");
		difficultyNameTxt.text = Language.GetString("ui_difficulty");
		rankNameTxt.text = Language.GetString("ui_rank");
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		lootBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_loot");
		leaderboardBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ranks");
		rewardsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_rewards");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		timeBar = GetComponentInChildren<TimeBarColor>();
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		currencyBarFill.Init();
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

	private void UpdateDifficultyDropdown()
	{
		difficultyData = new List<MyDropdownItemModel>();
		int num = GameData.instance.SAVE_STATE.GetGauntletEventDifficulty(GameData.instance.PROJECT.character.id);
		if (points <= 0 || num <= 0)
		{
			int difficulty = _difficulty;
			int highestMultFlagUnlocked = GameData.instance.PROJECT.character.zones.getHighestMultFlagUnlocked();
			difficulty = ((highestMultFlagUnlocked <= num) ? num : highestMultFlagUnlocked);
			num = difficulty - 2;
			if (num <= 0)
			{
				num = 1;
			}
		}
		_difficultySelected = num;
		string text = "";
		List<Vector2> list = new List<Vector2>();
		bool flag = true;
		int num2 = _difficulty;
		int tier = GameData.instance.PROJECT.character.tier;
		for (int num3 = _difficulty; num3 > 0; num3--)
		{
			int num4 = num3;
			GauntletEventTierRef difficultyTier = GauntletEventBook.GetDifficultyTier(num4);
			int tierName = difficultyTier.tierName;
			string text2 = "";
			if (tier >= tierName)
			{
				if (difficultyTier != null && difficultyTier.rarityRef != null)
				{
					text2 = difficultyTier.rarityRef.name;
				}
				if (num3 == _difficulty || flag)
				{
					text = text2;
					flag = false;
				}
				if (text != text2)
				{
					text = text2;
					list.Add(new Vector2(num4 + 1, num2));
					num2 = num4;
				}
			}
		}
		list.Add(new Vector2(1f, num2));
		int num5 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Vector2 vector = list[i];
			string text3 = Util.NumberFormat(vector.x) + " - " + Util.NumberFormat(vector.y);
			GauntletEventTierRef difficultyTier2 = GauntletEventBook.GetDifficultyTier((int)vector.x);
			if (difficultyTier2 != null)
			{
				text3 = difficultyTier2.rarityRef.ConvertString(text3);
			}
			string @string = Language.GetString("ui_tier_count", new string[1] { difficultyTier2.name });
			MyDropdownItemModel item = new MyDropdownItemModel
			{
				id = i,
				title = text3,
				btnHelp = false,
				desc = @string,
				data = vector
			};
			difficultyData.Add(item);
			if ((float)num5 < vector.y)
			{
				num5 = (int)vector.y;
			}
		}
		if (num5 > 0 && num5 < _difficultySelected)
		{
			_difficultySelected = num5;
			GameData.instance.SAVE_STATE.SetGauntletEventDifficulty(GameData.instance.PROJECT.character.id, _difficultySelected);
		}
		setDifficultyDropDownSelectedLabelWithColor(_difficultySelected);
		if (_eventRef == null || _checkingStats || _difficulty <= 0)
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
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, -1, OnCategoryDifficultyDropdownClicked);
		componentInChildren.Data.InsertItems(0, difficultyData);
	}

	public void OnCategoryDifficultyDropdownClicked(MyDropdownItemModel model)
	{
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
		Vector2 vector = (Vector2)model.data;
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		RarityRef rarityRef = null;
		GauntletEventTierRef difficultyTier = GauntletEventBook.GetDifficultyTier((int)vector.x);
		if (difficultyTier != null)
		{
			rarityRef = difficultyTier.rarityRef;
		}
		List<MyDropdownItemModel> list = new List<MyDropdownItemModel>();
		int num = 1;
		int num2 = int.Parse(difficultyTier.name);
		if (num2 > 12)
		{
			num = 10;
		}
		else if (num2 > 10)
		{
			num = 5;
		}
		int num3 = (int)vector.y;
		while ((float)num3 > vector.x - 1f)
		{
			string text = Util.NumberFormat(num3);
			if (rarityRef != null)
			{
				text = rarityRef.ConvertString(text);
			}
			list.Add(new MyDropdownItemModel
			{
				id = num3,
				title = text
			});
			num3 -= num;
		}
		difficultyCategory = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_difficulty"));
		DropdownList componentInChildren = difficultyCategory.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, 0, OnDifficultyDropdownClicked);
		componentInChildren.Data.InsertItems(0, list);
		setDifficultyDropDownSelectedLabelWithColor(_difficultySelected);
	}

	public void OnDifficultyDropdownClicked(MyDropdownItemModel model)
	{
		_difficultySelected = model.id;
		GameData.instance.SAVE_STATE.SetGauntletEventDifficulty(GameData.instance.PROJECT.character.id, _difficultySelected);
		setDifficultyDropDownSelectedLabelWithColor(_difficultySelected);
		if (difficultyCategory != null)
		{
			difficultyCategory.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void setDifficultyDropDownSelectedLabelWithColor(int val)
	{
		string text = GauntletEventBook.getDifficultyTierLimitedByCharTier(val).rarityRef.ConvertString(Util.NumberFormat(val));
		difficultyNameValueTxt.text = text;
	}

	private void UpdateBonusDropdown()
	{
		int gauntletEventBonus = GameData.instance.SAVE_STATE.GetGauntletEventBonus(GameData.instance.PROJECT.character.id);
		bonusData = new List<MyDropdownItemModel>();
		if (_eventRef != null)
		{
			MyDropdownItemModel item = (selectedBonus = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.tokens)
			});
			bonusData.Add(item);
			for (int i = 0; i < GauntletEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = GauntletEventBook.LookupBonus(i);
				if (currencyBonusRef != null)
				{
					int currencyCost = _eventRef.GetCurrencyCost(currencyBonusRef);
					MyDropdownItemModel item2 = new MyDropdownItemModel
					{
						id = i,
						title = Util.NumberFormat(currencyCost),
						data = currencyBonusRef,
						btnHelp = true
					};
					bonusData.Add(item2);
					if (currencyBonusRef.id == gauntletEventBonus)
					{
						selectedBonus = item2;
					}
				}
			}
		}
		costNameValueTxt.text = ((selectedBonus != null) ? selectedBonus.title : "");
		if (_eventRef == null)
		{
			UIUtil.LockImage(costDropdown, locked: true);
			bonusAllowed = false;
		}
		else
		{
			UIUtil.LockImage(costDropdown, locked: false);
		}
	}

	public void OnCostDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString(CurrencyRef.GetCurrencyName(9)));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, selectedBonus.id, OnCostDropdownClicked);
		componentInChildren.Data.InsertItemsAtEnd(bonusData);
	}

	public void OnCostDropdownClicked(MyDropdownItemModel model)
	{
		selectedBonus = model;
		costNameValueTxt.text = selectedBonus.title;
		int bonus = ((model.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetGauntletEventBonus(GameData.instance.PROJECT.character.id, bonus);
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private int GetSelectedBonusID()
	{
		if (!(selectedBonus.data is CurrencyBonusRef currencyBonusRef))
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
			GauntletDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
			GauntletDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GauntletDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
		GauntletDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("Gauntlet Event Reward", null, list, sfsob, "Gauntlet Event", 2);
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
				GauntletDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
				GauntletDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		_checkingStats = false;
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnEventStats);
		if (_eventRef != null)
		{
			Util.SetButton(playBtn);
		}
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GauntletEventCharacterData gauntletEventCharacterData = GauntletEventCharacterData.fromSFSObject(sfsob);
		SetStats(gauntletEventCharacterData.rank, gauntletEventCharacterData.points, gauntletEventCharacterData.highest, gauntletEventCharacterData.difficulty);
		_zone = gauntletEventCharacterData.zone;
	}

	private bool DoAllowEnter()
	{
		if (_eventRef == null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(27);
			return false;
		}
		if (GameData.instance.PROJECT.character.tokens < GetTokenCost())
		{
			GameData.instance.windowGenerator.ShowErrorCode(96);
			return false;
		}
		if (difficultyDropdown == null || _difficultySelected <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_event_difficulty"));
			return false;
		}
		return true;
	}

	private void DoTeamSelect()
	{
		if (DoAllowEnter())
		{
			GameData.instance.windowGenerator.NewTeamWindow(5, _eventRef.teamRules, OnTeamSelect, base.gameObject);
		}
	}

	private void OnTeamSelect(TeamData teamData)
	{
		DoEventEnter(teamData.teammates);
	}

	private void DoEventEnter(List<TeammateData> teammates)
	{
		if (DoAllowEnter())
		{
			GameData.instance.main.ShowLoading();
			GauntletDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
			GauntletDALC.instance.doEventEnter(_difficultySelected, GameData.instance.SAVE_STATE.GetGauntletEventBonus(), teammates);
		}
	}

	private void OnEventEnter(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GauntletDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(0), OnEventEnter);
		SFSObject sfsob = obj.sfsob;
		GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void UpdateEvent()
	{
		_eventRef = GauntletEventBook.GetCurrentEventRef();
		if (_eventRef == null)
		{
			Util.SetButton(playBtn, enabled: false);
			Util.SetButton(chestBtn, enabled: false);
		}
		else
		{
			Util.SetButton(playBtn);
			Util.SetButton(chestBtn);
		}
		string @string = Language.GetString("event_blank");
		if (_eventRef == null)
		{
			EventRef nextEventRef = GauntletEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
			SetStats();
		}
		else
		{
			_eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
			timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
			timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
			timeBar.COMPLETE.AddListener(OnEventTimerComplete);
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
		typeNameTxt.text = ((_eventRef == null) ? EventRef.GetEventRankTypeName(0, 3) : _eventRef.GetCurrentRankTypeName());
		altNameTxt.text = ((_eventRef == null) ? EventRef.GetEventRankTypeName(1, 3) : _eventRef.GetAltTypeName());
		UpdateBonusDropdown();
	}

	private void OnEventTimerComplete()
	{
		SetStats();
		UpdateEvent();
		DoEventLootCheck();
		DoEventStats();
	}

	private void SetStats(int rank = -1, int points = -1, int highest = -1, int difficulty = -1)
	{
		this.rank = rank;
		this.points = points;
		this.highest = highest;
		_difficulty = difficulty;
		int num = ((_eventRef != null) ? _eventRef.GetCurrentValue(highest, points) : highest);
		int num2 = ((_eventRef != null) ? _eventRef.GetAltValue(highest, points) : points);
		if (rankValueTxt != null)
		{
			rankValueTxt.text = ((rank > 0) ? Util.NumberFormat(rank, abbreviate: false) : "-");
		}
		if (altValueTxt != null)
		{
			altValueTxt.text = ((rank > 0) ? Util.NumberFormat(num2, abbreviate: false) : "-");
		}
		if (typeValueTxt != null)
		{
			typeValueTxt.text = ((rank > 0) ? Util.NumberFormat(num, abbreviate: false) : "-");
		}
		if (trophyHandler != null)
		{
			trophyHandler.ReplaceTrophy(trophyImage.gameObject, rank);
		}
		UpdateDifficultyDropdown();
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_eventRef.teamRules.slots <= 1)
		{
			List<TeammateData> list = new List<TeammateData>();
			list.Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			DoEventEnter(list);
		}
		else
		{
			DoTeamSelect();
		}
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

	public void OnLeaderboardBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(3, 0, base.gameObject);
	}

	public void OnRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(3, 2), rank, points, _zone, -1, alternate: false, 0L, base.gameObject);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(3), Util.parseMultiLine(Language.GetString("gauntlet_help_desc")), base.gameObject);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(9), compare: false, added: true);
	}

	public void OnTokensBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(10);
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(6, _eventRef.id, 0, "", _difficultySelected);
	}

	private int GetTokenCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = GauntletEventBook.LookupBonus(GetSelectedBonusID());
		return _eventRef.GetCurrencyCost(bonus);
	}

	private void UpdateButtons()
	{
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(9);
		consumablesBtn.gameObject.SetActive(consumablesByCurrencyID.Count > 0);
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		lootBtn.interactable = true;
		leaderboardBtn.interactable = true;
		rewardsBtn.interactable = true;
		helpBtn.interactable = true;
		refreshBtn.interactable = true;
		chestBtn.interactable = true;
		if (bonusAllowed)
		{
			costDropdown.GetComponent<EventTrigger>().enabled = true;
		}
		if (difficultyAllowed)
		{
			difficultyDropdown.GetComponent<EventTrigger>().enabled = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		lootBtn.interactable = false;
		leaderboardBtn.interactable = false;
		rewardsBtn.interactable = false;
		helpBtn.interactable = false;
		refreshBtn.interactable = false;
		chestBtn.interactable = false;
		costDropdown.GetComponent<EventTrigger>().enabled = false;
		difficultyDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
