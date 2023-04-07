using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.eventrewardsranklist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.events;

public class EventRewardsWindow : WindowsMain
{
	public const int TAB_RANKS = 0;

	public const int TAB_POINTS = 1;

	public TextMeshProUGUI topperTxt;

	public Button ranksBtn;

	public Button pointsBtn;

	public Image dropdownEvents;

	public Image dropdownTiers;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	public EventRewardsRankList eventRewardsRankList;

	private List<EventRef> _events;

	private int _rank;

	private int _points;

	private int _zone;

	private bool _alternate;

	private long _communityPoints;

	private List<Button> _tabButtons;

	private int _currentTab = -1;

	private int _selectedDropdownZone = -1;

	private List<MyDropdownItemModel> _dropdownEventsObjs = new List<MyDropdownItemModel>();

	private List<MyDropdownItemModel> _dropdownTiersObjs = new List<MyDropdownItemModel>();

	private MyDropdownItemModel _selectedDropdownEvent;

	private MyDropdownItemModel _selectedDropdownTier;

	private bool dropdownIsZones;

	private Transform _dropdownEventsWindow;

	private Transform _dropdownTiersWindow;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<EventRef> events, int rank, int points, int zone = -1, int eventID = -1, bool alternate = false, long communityPoints = 0L)
	{
		_events = events;
		_rank = rank;
		_points = points;
		_zone = ((zone <= 0) ? (ZoneBook.GetLastZoneID() + 1) : zone);
		_selectedDropdownZone = _zone;
		_alternate = alternate;
		_communityPoints = communityPoints;
		topperTxt.text = Language.GetString("ui_rewards");
		eventRewardsRankList.InitList();
		CreateDropdownEvents(eventID);
		CreateDropdownTiers();
		SetTabButtons();
		SetTab(0);
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void SetTabButtons()
	{
		if (_tabButtons == null)
		{
			_tabButtons = new List<Button>();
			SetTabButton(ranksBtn, Language.GetString("ui_ranks"), 0);
			SetTabButton(pointsBtn, Language.GetString("ui_points"), 1);
		}
	}

	private void SetTabButton(Button button, string text, int tab)
	{
		button.GetComponentInChildren<TextMeshProUGUI>().text = text;
		while (_tabButtons.Count <= tab)
		{
			_tabButtons.Add(null);
		}
		_tabButtons[tab] = button;
		button.onClick.AddListener(delegate
		{
			OnTabButtonClicked(button);
		});
	}

	private void OnTabButtonClicked(Button button)
	{
		for (int i = 0; i < _tabButtons.Count; i++)
		{
			if (_tabButtons[i].GetInstanceID() == button.GetInstanceID())
			{
				SetTab(i);
				break;
			}
		}
	}

	private Button GetTabButton(int tab)
	{
		if (tab < 0 || tab >= _tabButtons.Count)
		{
			return null;
		}
		return _tabButtons[tab];
	}

	public void SetTab(int tab)
	{
		for (int i = 0; i < _tabButtons.Count; i++)
		{
			Util.SetTab(_tabButtons[i], i == tab);
		}
		_currentTab = tab;
		CreateTiles();
	}

	private void CreateDropdownEvents(int eventID)
	{
		_dropdownEventsObjs.Clear();
		_selectedDropdownEvent = null;
		dropdownEvents.GetComponentInChildren<TextMeshProUGUI>().text = "";
		foreach (EventRef @event in _events)
		{
			MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel
			{
				id = @event.id,
				title = @event.name,
				data = @event
			};
			_dropdownEventsObjs.Add(myDropdownItemModel);
			if (@event.id == eventID)
			{
				_selectedDropdownEvent = myDropdownItemModel;
			}
		}
		if (_selectedDropdownEvent == null && _dropdownEventsObjs.Count > 0)
		{
			_selectedDropdownEvent = _dropdownEventsObjs[0];
		}
		if (_selectedDropdownEvent != null)
		{
			dropdownEvents.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownEvent.title;
		}
	}

	private void CreateDropdownTiers()
	{
		_dropdownTiersObjs.Clear();
		_selectedDropdownTier = null;
		dropdownIsZones = false;
		dropdownTiers.GetComponentInChildren<TextMeshProUGUI>().text = "";
		EventRef eventRef = GetEventRef();
		if (eventRef == null)
		{
			Debug.Log("delete drop 2");
			return;
		}
		switch (eventRef.eventType)
		{
		case 5:
		{
			InvasionEventRef invasionEventRef = eventRef as InvasionEventRef;
			if (!invasionEventRef.hasSegmentedRewards)
			{
				InvasionEventTierRef tierRef = invasionEventRef.GetTierRef(_communityPoints);
				foreach (InvasionEventTierRef tierReward in invasionEventRef.tierRewards)
				{
					MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel();
					myDropdownItemModel.id = tierReward.id;
					myDropdownItemModel.title = Language.GetString("ui_tier_count", new string[1] { tierReward.id.ToString() });
					myDropdownItemModel.data = tierReward;
					MyDropdownItemModel myDropdownItemModel3 = myDropdownItemModel;
					_dropdownTiersObjs.Add(myDropdownItemModel3);
					if (tierRef == tierReward)
					{
						_selectedDropdownTier = myDropdownItemModel3;
					}
				}
				if (_selectedDropdownTier == null && _dropdownTiersObjs.Count > 0)
				{
					_selectedDropdownTier = _dropdownTiersObjs[0];
				}
				if (_selectedDropdownTier != null)
				{
					dropdownTiers.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownTier.title;
				}
				dropdownTiers.gameObject.SetActive(value: true);
			}
			else if (ZoneBook.GetLastZoneID() + 1 != _zone || GameData.instance.PROJECT.character.admin)
			{
				dropdownIsZones = true;
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel();
				myDropdownItemModel.id = -1;
				myDropdownItemModel.title = Language.GetString("ui_your_tier", new string[1] { _zone.ToString() });
				myDropdownItemModel.data = _zone;
				MyDropdownItemModel myDropdownItemModel4 = myDropdownItemModel;
				_dropdownTiersObjs.Add(myDropdownItemModel4);
				myDropdownItemModel = new MyDropdownItemModel();
				myDropdownItemModel.id = 0;
				myDropdownItemModel.title = Language.GetString("ui_max_tier", new string[1] { (ZoneBook.GetLastZoneID() + 1).ToString() });
				myDropdownItemModel.data = ZoneBook.GetLastZoneID() + 1;
				MyDropdownItemModel item3 = myDropdownItemModel;
				_dropdownTiersObjs.Add(item3);
				if (GameData.instance.PROJECT.character.admin)
				{
					for (int j = 1; j < ZoneBook.GetLastZoneID() + 2; j++)
					{
						myDropdownItemModel = new MyDropdownItemModel();
						myDropdownItemModel.id = j;
						myDropdownItemModel.title = Language.GetString("ui_tier_count", new string[1] { j.ToString() });
						myDropdownItemModel.data = j;
						MyDropdownItemModel item4 = myDropdownItemModel;
						_dropdownTiersObjs.Add(item4);
					}
				}
				_selectedDropdownTier = myDropdownItemModel4;
				if (_selectedDropdownTier != null)
				{
					dropdownTiers.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownTier.title;
				}
				dropdownTiers.gameObject.SetActive(value: true);
			}
			else
			{
				Debug.Log("hide drop");
				dropdownTiers.gameObject.SetActive(value: false);
			}
			break;
		}
		case 1:
		case 2:
		case 3:
		case 4:
		case 7:
			if (eventRef.hasSegmentedRewards && (ZoneBook.GetLastZoneID() + 1 != _zone || GameData.instance.PROJECT.character.admin))
			{
				dropdownIsZones = true;
				MyDropdownItemModel myDropdownItemModel = new MyDropdownItemModel();
				myDropdownItemModel.id = -1;
				myDropdownItemModel.title = Language.GetString("ui_your_tier", new string[1] { _zone.ToString() });
				myDropdownItemModel.data = _zone;
				MyDropdownItemModel myDropdownItemModel2 = myDropdownItemModel;
				_dropdownTiersObjs.Add(myDropdownItemModel2);
				myDropdownItemModel = new MyDropdownItemModel();
				myDropdownItemModel.id = 0;
				myDropdownItemModel.title = Language.GetString("ui_max_tier", new string[1] { (ZoneBook.GetLastZoneID() + 1).ToString() });
				myDropdownItemModel.data = ZoneBook.GetLastZoneID() + 1;
				MyDropdownItemModel item = myDropdownItemModel;
				_dropdownTiersObjs.Add(item);
				if (GameData.instance.PROJECT.character.admin)
				{
					for (int i = 1; i < ZoneBook.GetLastZoneID() + 2; i++)
					{
						myDropdownItemModel = new MyDropdownItemModel();
						myDropdownItemModel.id = i;
						myDropdownItemModel.title = Language.GetString("ui_tier_count", new string[1] { i.ToString() });
						myDropdownItemModel.data = i;
						MyDropdownItemModel item2 = myDropdownItemModel;
						_dropdownTiersObjs.Add(item2);
					}
				}
				_selectedDropdownTier = myDropdownItemModel2;
				if (_selectedDropdownTier != null)
				{
					dropdownTiers.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownTier.title;
				}
				dropdownTiers.gameObject.SetActive(value: true);
			}
			else
			{
				Debug.Log("hide drop");
				dropdownTiers.gameObject.SetActive(value: false);
			}
			break;
		default:
			Debug.Log("delete dropdown 1");
			dropdownTiers.gameObject.SetActive(value: false);
			break;
		}
	}

	public void OnDropdownEventClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_dropdownEventsWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_events"));
		DropdownList componentInChildren = _dropdownEventsWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedDropdownEvent.id, OnDropdownEventsChange);
		componentInChildren.Data.InsertItemsAtStart(_dropdownEventsObjs);
	}

	private void OnDropdownEventsChange(MyDropdownItemModel model)
	{
		_selectedDropdownEvent = model;
		dropdownEvents.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownEvent.title;
		if (_dropdownEventsWindow != null)
		{
			_dropdownEventsWindow.GetComponent<DropdownWindow>().OnClose();
		}
		CreateDropdownTiers();
		CreateTiles();
	}

	public void OnDropdownTiersClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_dropdownTiersWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_tier"));
		DropdownList componentInChildren = _dropdownTiersWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedDropdownTier.id, OnDropdownTiersChange);
		componentInChildren.Data.InsertItemsAtStart(_dropdownTiersObjs);
	}

	private void OnDropdownTiersChange(MyDropdownItemModel model)
	{
		_selectedDropdownTier = model;
		dropdownTiers.GetComponentInChildren<TextMeshProUGUI>().text = _selectedDropdownTier.title;
		if (dropdownIsZones)
		{
			_selectedDropdownZone = (_selectedDropdownTier.data as int?).Value;
		}
		if (_dropdownTiersWindow != null)
		{
			_dropdownTiersWindow.GetComponent<DropdownWindow>().OnClose();
		}
		CreateTiles();
	}

	private void ClearTiles()
	{
		eventRewardsRankList.ClearList();
	}

	private EventRewards GetRankRewards(int zone, EventRef eventRef)
	{
		if (_alternate)
		{
			switch (eventRef.eventType)
			{
			case 4:
				return eventRef.guildRankRewards;
			case 7:
				return eventRef.guildRankRewards;
			}
		}
		if (eventRef.eventType == 5 && !eventRef.hasSegmentedRewards)
		{
			return (_selectedDropdownTier.data as InvasionEventTierRef).rankRewards;
		}
		return eventRef.getRankRewards(zone);
	}

	private EventRewards GetPointRewards(int zone, EventRef eventRef)
	{
		if (_alternate)
		{
			switch (eventRef.eventType)
			{
			case 4:
				return eventRef.guildPointRewards;
			case 7:
				return eventRef.guildPointRewards;
			}
		}
		if (eventRef.eventType == 5 && !eventRef.hasSegmentedRewards)
		{
			return (_selectedDropdownTier.data as InvasionEventTierRef).pointRewards;
		}
		return eventRef.getPointRewards(zone);
	}

	private void CreateTiles()
	{
		ClearTiles();
		EventRef eventRef = GetEventRef();
		if (eventRef == null)
		{
			return;
		}
		EventRewards eventRewards = null;
		switch (_currentTab)
		{
		default:
			return;
		case 0:
			eventRewards = GetRankRewards(_selectedDropdownZone, eventRef);
			break;
		case 1:
			eventRewards = GetPointRewards(_selectedDropdownZone, eventRef);
			break;
		}
		bool currentEvent = eventRef.GetDateRef().getMillisecondsUntilStart() < 0 && eventRef.GetDateRef().getMillisecondsUntilEnd() > 0;
		if (eventRewards == null || eventRewards.rewards == null)
		{
			return;
		}
		foreach (EventRewardRef reward in eventRewards.rewards)
		{
			if (reward != null)
			{
				eventRewardsRankList.Data.InsertOneAtEnd(new EventRewardItemRank
				{
					eventRef = eventRef,
					rewardRef = reward,
					points = _points,
					rank = _rank,
					displayRank = (_currentTab == 0),
					currentEvent = currentEvent,
					currentZone = (_zone == _selectedDropdownZone)
				});
			}
		}
	}

	public EventRef GetEventRef()
	{
		if (_selectedDropdownEvent == null)
		{
			return null;
		}
		return _selectedDropdownEvent.data as EventRef;
	}

	public override void DoDestroy()
	{
		if (_tabButtons != null)
		{
			foreach (Button button in _tabButtons)
			{
				button.onClick.RemoveListener(delegate
				{
					OnTabButtonClicked(button);
				});
			}
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (_tabButtons != null)
		{
			foreach (Button tabButton in _tabButtons)
			{
				tabButton.enabled = true;
			}
		}
		dropdownEvents.GetComponent<EventTrigger>().enabled = true;
		dropdownTiers.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (_tabButtons != null)
		{
			foreach (Button tabButton in _tabButtons)
			{
				tabButton.enabled = true;
			}
		}
		dropdownEvents.GetComponent<EventTrigger>().enabled = false;
		dropdownTiers.GetComponent<EventTrigger>().enabled = false;
	}
}
