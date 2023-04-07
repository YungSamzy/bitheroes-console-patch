using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.eventsales;

public class EventSalesShopWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI eventTxt;

	public TextMeshProUGUI materialTxt;

	public Image materialIcon;

	public Image multiplierDropdown;

	public Image background;

	public Image topper;

	public RectTransform placeholderTabs;

	private EventSalesShopEventRef _eventRef;

	private TimeBarColor _timeBar;

	private List<EventSalesShopTab> _tabs;

	private List<EventSalesShopPanel> _panels = new List<EventSalesShopPanel>();

	private int _defaultTab;

	private int _currentTab;

	public EventSalesShopTab eventSalesShopTabPrefab;

	public Transform eventSalesShopPanelPrefab;

	private Transform window;

	private int? currentQtyId;

	private List<ItemData> _remainingPurchases;

	private string _origin = "";

	public override void Start()
	{
		base.Start();
		Disable();
		_eventRef = EventSalesShopBook.GetCurrentEvent();
		if (_eventRef == null)
		{
			OnClose();
			return;
		}
		DoEventRemainingPurchases(_eventRef);
		topperTxt.text = Language.GetString(_eventRef.GetName());
		eventTxt.text = Language.GetString("event_end", new string[1] { Language.GetString(_eventRef.GetName()) });
		materialIcon.overrideSprite = _eventRef.GetMaterialRef().GetSpriteIcon();
		if (!string.IsNullOrEmpty(_eventRef.GetTopper()))
		{
			Sprite spriteAsset = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.CUSTOM_UI, _eventRef.GetTopper());
			if (spriteAsset != null)
			{
				topper.overrideSprite = spriteAsset;
			}
		}
		if (!string.IsNullOrEmpty(_eventRef.GetBackground()))
		{
			Sprite spriteAsset2 = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.CUSTOM_UI, _eventRef.GetBackground());
			if (spriteAsset2 != null)
			{
				background.overrideSprite = spriteAsset2;
			}
		}
		_timeBar = GetComponentInChildren<TimeBarColor>();
		UpdateEvent();
		CreateTabs();
		SetTab(_defaultTab);
		HandleTabVisualization();
		UpdateText();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
		ListenForBack(OnClose);
		CreateWindow();
		CheckTutorial(scrolling: true);
	}

	public void LoadDetails(string origin)
	{
		_origin = origin;
	}

	public void DoEventRemainingPurchases(EventSalesShopEventRef eventRef)
	{
		GameData.instance.main.UpdateLoading();
		EventSalesDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnEventRemainingPurchases);
		EventSalesDALC.instance.doEventRemainingPurchases(eventRef);
	}

	public void DoRefreshEventRemainingPurchases()
	{
		DoEventRemainingPurchases(_eventRef);
	}

	private void OnEventRemainingPurchases(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = baseEvent as DALCEvent;
		EventSalesDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnEventRemainingPurchases);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		_remainingPurchases = ItemData.listFromSFSObject(sfsob.GetSFSObject("pur11"));
		DoUpdate(_remainingPurchases);
	}

	public void OnDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_quantity"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentQtyId.HasValue ? currentQtyId.Value : 0, OnQtySelected);
		componentInChildren.ClearList();
		for (int i = 0; i < VariableBook.shopQuantities.Length; i++)
		{
			int num = int.Parse(VariableBook.shopQuantities[i]);
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = i,
				title = Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(num) }),
				locked = false
			});
		}
	}

	public void OnQtySelected(MyDropdownItemModel model)
	{
		currentQtyId = model.id;
		multiplierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(int.Parse(VariableBook.shopQuantities[currentQtyId.Value])) });
		window.GetComponent<DropdownWindow>().OnClose();
		EventSalesShopPanel eventSalesShopPanel = GetPanel(_currentTab);
		if ((bool)eventSalesShopPanel)
		{
			eventSalesShopPanel.DoRefreshQty();
		}
	}

	public int GetQty()
	{
		if (!currentQtyId.HasValue)
		{
			return 1;
		}
		return int.Parse(VariableBook.shopQuantities[currentQtyId.Value]);
	}

	public void CheckTutorial(bool scrolling = false)
	{
	}

	private void CreateTabs()
	{
		if (_tabs == null)
		{
			_tabs = new List<EventSalesShopTab>();
			for (int i = 0; i < EventSalesShopBook.tabsSize; i++)
			{
				EventSalesShopTabRef eventSalesShopTabRef = EventSalesShopBook.LookupTab(i);
				AddTab(eventSalesShopTabRef.name, eventSalesShopTabRef);
			}
		}
	}

	private void AddTab(string name, EventSalesShopTabRef tabRef = null)
	{
		EventSalesShopTab tab = Object.Instantiate(eventSalesShopTabPrefab);
		tab.transform.SetParent(placeholderTabs, worldPositionStays: false);
		tab.LoadDetails(_tabs.Count, name, tabRef);
		tab.tabBtn.onClick.AddListener(delegate
		{
			OnTabClick(tab);
		});
		_tabs.Add(tab);
	}

	private EventSalesShopPanel GetPanel(int id)
	{
		if (id < 0 || id >= _panels.Count)
		{
			return null;
		}
		return _panels[id];
	}

	private EventSalesShopTab GetTab(int id)
	{
		if (id < 0 || id >= _tabs.Count)
		{
			return null;
		}
		return _tabs[id];
	}

	public void SetTab(int id = 0)
	{
		for (int i = 0; i < _tabs.Count; i++)
		{
			Util.SetTab(_tabs[i].tabBtn, i == id);
		}
		EventSalesShopPanel eventSalesShopPanel = GetPanel(_currentTab);
		if ((bool)eventSalesShopPanel)
		{
			eventSalesShopPanel.DoHide();
		}
		_currentTab = id;
		EventSalesShopTab tab = GetTab(id);
		if (!tab)
		{
			return;
		}
		EventSalesShopPanel component = GetPanel(_currentTab);
		if (component == null)
		{
			Transform obj = Object.Instantiate(eventSalesShopPanelPrefab);
			obj.SetParent(panel.transform, worldPositionStays: false);
			obj.GetComponent<EventSalesShopPanel>().DoShow();
			obj.GetComponent<EventSalesShopPanel>().LoadDetails(this, _eventRef, tab.tabRef);
			component = obj.GetComponent<EventSalesShopPanel>();
			while (_panels.Count <= _currentTab)
			{
				_panels.Add(null);
			}
			_panels[_currentTab] = component;
		}
		if ((bool)component)
		{
			component.DoShow();
			component.DoUpdate(_remainingPurchases);
		}
	}

	private void HandleTabVisualization()
	{
		if (_tabs.Count == 1)
		{
			_tabs[0].gameObject.SetActive(value: false);
		}
	}

	private void OnTabClick(EventSalesShopTab tab)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(tab.index);
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString(_eventRef.GetName()), Language.GetString("event_sales_shop_help_desc"));
		string navElementName = _eventRef.GetName() + "-" + _eventRef.GetDateRef().startDate.Year;
		KongregateAnalytics.trackNavigationAction(new NavigationData(_origin, navElementName, "Info-Screen"));
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void DoUpdate(List<ItemData> remainingPurchases = null)
	{
		foreach (EventSalesShopPanel panel in _panels)
		{
			if ((bool)panel)
			{
				panel.DoUpdate(remainingPurchases);
			}
		}
		UpdateText();
	}

	private void UpdateText()
	{
		materialTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.getItemQty(_eventRef.GetMaterialRef()), abbreviate: false);
	}

	private void OnCharacterInventoryChange()
	{
		DoUpdate();
		DoRefreshEventRemainingPurchases();
	}

	private void UpdateEvent()
	{
		string @string = Language.GetString("event_blank");
		_eventRef = EventSalesShopBook.GetCurrentEvent();
		if (_eventRef != null)
		{
			DateRef dateRef = _eventRef.GetDateRef();
			dateRef.getMillisecondsUntilEnd();
			_timeBar.SetMaxValueMilliseconds(dateRef.getMillisecondsDuration());
			_timeBar.SetCurrentValueMilliseconds(dateRef.getMillisecondsUntilEnd());
			_timeBar.COMPLETE.AddListener(OnEventTimerComplete);
			@string = Language.GetString("event_end", new string[1] { Language.GetString(_eventRef.GetName()) });
		}
		eventTxt.text = @string;
		eventTxt.gameObject.SetActive(@string.Length > 0);
	}

	private void OnEventTimerComplete()
	{
		UpdateEvent();
	}

	public override void DoDestroy()
	{
		foreach (EventSalesShopTab tab in _tabs)
		{
			if (tab != null)
			{
				tab.tabBtn.onClick.RemoveListener(delegate
				{
					OnTabClick(tab.GetComponent<EventSalesShopTab>());
				});
			}
		}
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
		_timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		multiplierDropdown.GetComponent<EventTrigger>().enabled = true;
		CheckTutorial(scrolling: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		multiplierDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
