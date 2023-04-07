using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rift;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rift;

public class RiftEventWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI costNameTxt;

	public TextMeshProUGUI difficultyNameTxt;

	public TextMeshProUGUI rankNameTxt;

	public TextMeshProUGUI rankValueTxt;

	public TextMeshProUGUI typeNameTxt;

	public TextMeshProUGUI typeValueTxt;

	public TextMeshProUGUI altNameTxt;

	public TextMeshProUGUI altValueTxt;

	public TextMeshProUGUI tokensTxts;

	public CurrencyBarFill currencyBarFill;

	public TextMeshProUGUI difficultyTxt;

	public TextMeshProUGUI costNameValueTxt;

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

	private RiftEventRef _eventRef;

	private Transform window;

	private Transform difficultyCategory;

	private TimeBarColor timeBar;

	private int rank;

	private int points;

	private int highest;

	private int _difficulty;

	private MyDropdownItemModel selectedDifficulty;

	private List<MyDropdownItemModel> _bonuses;

	private MyDropdownItemModel _selectedBonus;

	private bool bonusAllowed = true;

	private const string BLANK = "-";

	private List<MyDropdownItemModel> difficultyData;

	private int _difficultySelected;

	private TrophyHandler trophyHandler;

	public Image trophyImage;

	private bool _checkingStats;

	private bool _checkingLoot;

	private int _lootEventID;

	private IEnumerator _refreshTimer;

	private int seconds = 10;

	private int _zone;

	private int eventDifficulty;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = EventRef.getEventTypeName(2);
		TextMeshProUGUI textMeshProUGUI = eventTxt;
		string text = (eventTxt.text = Language.GetString("event_end", new string[1] { EventRef.getEventTypeName(2) }));
		textMeshProUGUI.text = text;
		costNameTxt.text = Language.GetString("ui_cost");
		difficultyNameTxt.text = Language.GetString("ui_difficulty");
		rankNameTxt.text = Language.GetString("ui_rank");
		rankValueTxt.text = "-";
		typeValueTxt.text = "-";
		altValueTxt.text = "-";
		tokensTxts.text = GameData.instance.SAVE_STATE.GetRiftEventBonus().ToString();
		eventDifficulty = GameData.instance.SAVE_STATE.GetRiftEventDifficulty();
		difficultyTxt.text = eventDifficulty.ToString();
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
		int num = GameData.instance.SAVE_STATE.GetRiftEventDifficulty(GameData.instance.PROJECT.character.id);
		if (points <= 0 || num <= 0)
		{
			int num2 = _difficulty;
			int highestMultFlagUnlocked = GameData.instance.PROJECT.character.zones.getHighestMultFlagUnlocked();
			if (highestMultFlagUnlocked > num)
			{
				num2 = highestMultFlagUnlocked;
			}
			else if (num < _difficulty)
			{
				num2 = num;
			}
			num = num2 - 2;
			if (num <= 0)
			{
				num = 1;
			}
		}
		_difficultySelected = num;
		string text = "";
		List<Vector2> list = new List<Vector2>();
		bool flag = true;
		int num3 = _difficulty;
		int tier = GameData.instance.PROJECT.character.tier;
		for (int num4 = _difficulty; num4 > 0; num4--)
		{
			int num5 = num4;
			RiftEventTierRef difficultyTier = RiftEventBook.GetDifficultyTier(num5);
			int tierName = difficultyTier.tierName;
			string text2 = "";
			if (tier >= tierName)
			{
				if (difficultyTier != null && difficultyTier.rarityRef != null)
				{
					text2 = difficultyTier.rarityRef.name;
				}
				if (num4 == _difficulty || flag)
				{
					text = text2;
					flag = false;
				}
				if (text != text2)
				{
					text = text2;
					list.Add(new Vector2(num5 + 1, num3));
					num3 = num5;
				}
			}
		}
		list.Add(new Vector2(1f, num3));
		int num6 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Vector2 vector = list[i];
			string text3 = Util.NumberFormat(vector.x) + " - " + Util.NumberFormat(vector.y);
			RiftEventTierRef difficultyTier2 = RiftEventBook.GetDifficultyTier((int)vector.x);
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
			if ((float)num6 < vector.y)
			{
				num6 = (int)vector.y;
			}
		}
		if (num6 > 0 && num6 < _difficultySelected)
		{
			_difficultySelected = num6;
			GameData.instance.SAVE_STATE.SetRiftEventDifficulty(GameData.instance.PROJECT.character.id, _difficultySelected);
			eventDifficulty = _difficultySelected;
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
		RiftEventTierRef difficultyTier = RiftEventBook.GetDifficultyTier((int)vector.x);
		if (difficultyTier != null)
		{
			rarityRef = difficultyTier.rarityRef;
		}
		List<MyDropdownItemModel> list = new List<MyDropdownItemModel>();
		int num = 1;
		D.Log($"difficultyTierRef.name {difficultyTier.name} is difficultyTierRef null == {difficultyTier == null}");
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
	}

	public void OnDifficultyDropdownClicked(MyDropdownItemModel model)
	{
		if (difficultyCategory != null)
		{
			difficultyCategory.GetComponent<DropdownWindow>().OnClose();
		}
		_difficultySelected = model.id;
		GameData.instance.SAVE_STATE.SetRiftEventDifficulty(GameData.instance.PROJECT.character.id, _difficultySelected);
		setDifficultyDropDownSelectedLabelWithColor(_difficultySelected);
		eventDifficulty = _difficultySelected;
	}

	private void setDifficultyDropDownSelectedLabelWithColor(int val)
	{
		string text = RiftEventBook.getDifficultyTierLimitedByCharTier(val).rarityRef.ConvertString(Util.NumberFormat(val));
		difficultyTxt.text = text;
		eventDifficulty = val;
	}

	public void OnDifficultySelected(MyDropdownItemModel model)
	{
		selectedDifficulty = model;
		difficultyTxt.text = model.id.ToString();
		GameData.instance.SAVE_STATE.SetRiftEventDifficulty(model.id);
		eventDifficulty = model.id;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void UpdateBonusDropdown()
	{
		int riftEventBonus = GameData.instance.SAVE_STATE.GetRiftEventBonus(GameData.instance.PROJECT.character.id);
		_bonuses = new List<MyDropdownItemModel>();
		if (_eventRef != null)
		{
			MyDropdownItemModel item = (_selectedBonus = new MyDropdownItemModel
			{
				id = -1,
				title = Util.NumberFormat(_eventRef.tokens)
			});
			_bonuses.Add(item);
			for (int i = 0; i < RiftEventBook.sizeBonuses; i++)
			{
				CurrencyBonusRef currencyBonusRef = RiftEventBook.LookupBonus(i);
				if (currencyBonusRef != null)
				{
					int currencyCost = _eventRef.GetCurrencyCost(currencyBonusRef);
					MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
					{
						id = i,
						title = Util.NumberFormat(currencyCost),
						data = currencyBonusRef,
						btnHelp = true
					};
					_bonuses.Add(myDropdownItemModel);
					if (currencyBonusRef.id == riftEventBonus)
					{
						_selectedBonus = myDropdownItemModel;
					}
				}
			}
		}
		costNameValueTxt.text = ((_selectedBonus != null) ? _selectedBonus.title : "");
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
		componentInChildren.StartList(base.gameObject, _selectedBonus.id, OnCostDropdownClicked);
		componentInChildren.Data.InsertItemsAtEnd(_bonuses);
	}

	public void OnCostDropdownClicked(MyDropdownItemModel model)
	{
		_selectedBonus = model;
		costNameValueTxt.text = _selectedBonus.title;
		int bonus = ((model.data is CurrencyBonusRef currencyBonusRef) ? currencyBonusRef.id : (-1));
		GameData.instance.SAVE_STATE.SetRiftEventBonus(GameData.instance.PROJECT.character.id, bonus);
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
			RiftDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_CHECK), OnEventLootCheck);
			RiftDALC.instance.doEventLootCheck();
		}
	}

	private void OnEventLootCheck(BaseEvent e)
	{
		_checkingLoot = false;
		DALCEvent obj = e as DALCEvent;
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_CHECK), OnEventLootCheck);
		int @int = obj.sfsob.GetInt("eve0");
		SetLootID(@int);
	}

	private void DoEventLootItems()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		RiftDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_ITEMS), OnEventLootItems);
		RiftDALC.instance.doEventLootItems(_lootEventID);
	}

	private void OnEventLootItems(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_LOOT_ITEMS), OnEventLootItems);
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
			KongregateAnalytics.checkEconomyTransaction("Trials Event Reward", null, list, sfsob, "Trials Event", 2);
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
		if (lootBtn != null)
		{
			lootBtn.gameObject.SetActive(_lootEventID >= 0);
		}
		if (playBtn != null)
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
				RiftDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_STATS), OnEventStats);
				RiftDALC.instance.doEventStats(_eventRef.id);
			}
		}
	}

	private void OnEventStats(BaseEvent baseEvent)
	{
		_checkingStats = false;
		DALCEvent obj = baseEvent as DALCEvent;
		if (_eventRef != null)
		{
			Util.SetButton(playBtn);
		}
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_STATS), OnEventStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		RiftEventCharacterData riftEventCharacterData = RiftEventCharacterData.fromSFSObject(sfsob);
		SetStats(riftEventCharacterData.rank, riftEventCharacterData.points, riftEventCharacterData.highest, riftEventCharacterData.difficulty);
		_zone = riftEventCharacterData.zone;
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
		if (difficultyDropdown == null || _selectedBonus == null)
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
			GameData.instance.windowGenerator.NewTeamWindow(4, _eventRef.teamRules, OnTeamSelect, base.gameObject);
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
			RiftDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_ENTER), OnEventEnter);
			RiftDALC.instance.doEventEnter(_difficultySelected, GameData.instance.SAVE_STATE.GetRiftEventBonus(), teammates);
		}
	}

	private void OnEventEnter(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		RiftDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(RiftDALC.EVENT_ENTER), OnEventEnter);
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
		_eventRef = RiftEventBook.GetCurrentEventRef();
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
			EventRef nextEventRef = RiftEventBook.GetNextEventRef();
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
		typeNameTxt.text = ((_eventRef == null) ? EventRef.GetEventRankTypeName(0, 2) : _eventRef.GetCurrentRankTypeName());
		altNameTxt.text = ((_eventRef == null) ? EventRef.GetEventRankTypeName(1, 2) : _eventRef.GetAltTypeName());
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
		GameData.instance.windowGenerator.NewEventLeaderboardWindow(2, 0, base.gameObject);
	}

	public void OnRewardsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEventRewardsWindow(EventBook.GetSortedEvents(2, 2), rank, points, _zone, -1, alternate: false, 0L, base.gameObject);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(EventRef.getEventTypeName(2), Util.parseMultiLine(Language.GetString("rift_help_desc")), base.gameObject);
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(9), compare: false, added: true);
	}

	public void OnChestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowPossibleDungeonLoot(5, _eventRef.id, 0, "", _difficultySelected);
	}

	public void OnTokensBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(10);
	}

	private int GetTokenCost()
	{
		if (_eventRef == null)
		{
			return 0;
		}
		CurrencyBonusRef bonus = RiftEventBook.LookupBonus(GetSelectedBonusID());
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
		leaderboardBtn.interactable = true;
		rewardsBtn.interactable = true;
		helpBtn.interactable = true;
		refreshBtn.interactable = true;
		chestBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		leaderboardBtn.interactable = false;
		rewardsBtn.interactable = false;
		helpBtn.interactable = false;
		refreshBtn.interactable = false;
		chestBtn.interactable = false;
	}
}
