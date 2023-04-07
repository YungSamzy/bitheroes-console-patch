using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.shoppromolist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.shop;

public class ShopWindow : WindowsMain
{
	public static int TAB_FEATURED = 0;

	public static int TAB_GEAR = 1;

	public static int TAB_BOOSTS = 2;

	public static int TAB_OTHER = 3;

	private const int CHECKS_DELAY = 2;

	private const int DEBUG_DATE_DROPDOWN_LENGTH = 14;

	[SerializeField]
	private Button eulaBtn;

	public Image multiplierDropdown;

	public Image debugDateDropdown;

	public Image backPanel;

	public RectTransform placeholderTabs;

	public Transform shopTabPrefab;

	public Transform shopPanelPrefab;

	private int _defaultTab;

	private int _currentTab;

	private ItemData _itemCheck;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private Transform window;

	private MyDropdownItemModel _modelMultiplierDropdown;

	private List<MyDropdownItemModel> _dropdownMultiplierItems;

	private int _selectedQty;

	private MyDropdownItemModel _modelDebugDateDropdown;

	private List<MyDropdownItemModel> _dropdownDebugDateItems;

	private int _selectedDebugDate;

	private List<ShopTab> _tabs;

	private List<ShopPanel> _panels = new List<ShopPanel>();

	private Coroutine _timer;

	public DateTime? debugDate
	{
		get
		{
			if (_dropdownDebugDateItems == null || _dropdownDebugDateItems.Count <= _selectedDebugDate)
			{
				return null;
			}
			return (DateTime?)_dropdownDebugDateItems[_selectedDebugDate].data;
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(int defaultTab = -1)
	{
		if (defaultTab == -1)
		{
			defaultTab = TAB_FEATURED;
		}
		_defaultTab = defaultTab;
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		EulaButton();
		CreateTabs();
		SetTab(_defaultTab);
		debugDateDropdown.gameObject.SetActive(GameData.instance.PROJECT.character.admin);
		CreateDropdowns();
		CreateTimer();
		SCROLL_IN_START.AddListener(OnScrollInStart);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		GameData.instance.PROJECT.PauseDungeon();
		ListenForBack(OnClose);
		GameData.instance.windowGenerator.ShowCurrencies(show: true);
		CreateWindow();
		CheckTutorial(scrolling: true);
		CheckAsianFont(overrideResized: true);
	}

	private void EulaButton()
	{
		eulaBtn.gameObject.SetActive(GameData.instance.PROJECT.character.toCharacterData().isIMXG0);
		eulaBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("eula");
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		foreach (ShopPanel panel in _panels)
		{
			if (panel == null)
			{
				break;
			}
			ShopPromoList componentInChildren = panel.GetComponentInChildren<ShopPromoList>();
			if (!(componentInChildren != null))
			{
				continue;
			}
			foreach (com.ultrabit.bitheroes.ui.lists.shoppromolist.MyListItemViewsHolder visibleItem in componentInChildren._VisibleItems)
			{
				componentInChildren.RefreshOne(visibleItem);
			}
			if (componentInChildren.GetComponentInParent<ShopPromoTile>() != null)
			{
				componentInChildren.GetComponentInParent<ShopPromoTile>().RefreshMask();
			}
		}
	}

	public void OnEula()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("eula"), Language.GetString("eula_notice"), base.gameObject);
	}

	private void OnScrollInStart(object e)
	{
		SCROLL_IN_START.RemoveListener(OnScrollInStart);
		CheckTutorial(scrolling: true);
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void CheckTutorial(bool scrolling = false)
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || GameData.instance.PROJECT.character.tutorial.GetState(29))
		{
			return;
		}
		int tutorialItemTab = GetTutorialItemTab();
		if (tutorialItemTab >= 0)
		{
			_defaultTab = tutorialItemTab;
			SetTab(_defaultTab);
		}
		if (!scrolling)
		{
			ShopPanel shopPanel = GetPanel(_currentTab);
			if ((bool)shopPanel)
			{
				shopPanel.CheckTutorial();
			}
		}
	}

	private int GetTutorialItemTab()
	{
		for (int i = 0; i < ShopBook.tabsSize; i++)
		{
			ShopTabRef shopTabRef = ShopBook.LookupTab(i);
			if (shopTabRef != null && shopTabRef.getTutorialShopItem() != null)
			{
				return i + 1;
			}
		}
		return -1;
	}

	private void OnShopRotationChange()
	{
		DoUpdate();
	}

	private void CreateTabs()
	{
		if (_tabs == null)
		{
			_tabs = new List<ShopTab>();
			AddTab(Language.GetString("shop_tab_primary_name"));
			for (int i = 0; i < ShopBook.tabsSize; i++)
			{
				ShopTabRef shopTabRef = ShopBook.LookupTab(i);
				AddTab(shopTabRef.name, shopTabRef);
			}
		}
	}

	private void AddTab(string name, ShopTabRef tabRef = null)
	{
		Transform tab = UnityEngine.Object.Instantiate(shopTabPrefab);
		tab.SetParent(placeholderTabs, worldPositionStays: false);
		tab.GetComponent<ShopTab>().LoadDetails(_tabs.Count, name, tabRef);
		tab.GetComponent<ShopTab>().tabBtn.onClick.AddListener(delegate
		{
			OnTabClick(tab.GetComponent<ShopTab>());
		});
		_tabs.Add(tab.GetComponent<ShopTab>());
	}

	private void CreateDropdowns()
	{
		_dropdownMultiplierItems = new List<MyDropdownItemModel>();
		string[] shopQuantities = VariableBook.shopQuantities;
		for (int i = 0; i < shopQuantities.Length; i++)
		{
			int.TryParse(shopQuantities[i], out var result);
			_dropdownMultiplierItems.Add(new MyDropdownItemModel
			{
				id = result,
				title = Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(result) }),
				data = result
			});
		}
		if (_dropdownMultiplierItems.Count > 0)
		{
			_selectedQty = _dropdownMultiplierItems[0].id;
			multiplierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _dropdownMultiplierItems[0].title;
		}
		_dropdownDebugDateItems = new List<MyDropdownItemModel>();
		for (int j = 0; j < 14; j++)
		{
			DateTime dateTime = ServerExtension.instance.GetDate().AddDays(j);
			_dropdownDebugDateItems.Add(new MyDropdownItemModel
			{
				id = j,
				title = dateTime.ToShortDateString(),
				data = dateTime
			});
		}
		if (_dropdownDebugDateItems.Count > 0)
		{
			_selectedDebugDate = _dropdownDebugDateItems[0].id;
			debugDateDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _dropdownDebugDateItems[0].title;
		}
	}

	public void OnMultiplierDropdown()
	{
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_quantity"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedQty, OnMultiplierDropdownClick);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtStart(_dropdownMultiplierItems);
	}

	public void OnMultiplierDropdownClick(MyDropdownItemModel model)
	{
		_modelMultiplierDropdown = model;
		_selectedQty = (_modelMultiplierDropdown.data as int?).Value;
		multiplierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		DoUpdate();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnDebugDateDropdown()
	{
		window = GameData.instance.windowGenerator.NewDropdownWindow("Debug Date");
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedDebugDate, OnDebugDateDropdownClick);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtStart(_dropdownDebugDateItems);
	}

	public void OnDebugDateDropdownClick(MyDropdownItemModel model)
	{
		_modelDebugDateDropdown = model;
		_selectedDebugDate = _modelDebugDateDropdown.id;
		debugDateDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		OnTimer();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void ClearTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
	}

	private void CreateTimer()
	{
		ClearTimer();
		long saleChangeMilliseconds = ShopBook.GetSaleChangeMilliseconds();
		if (saleChangeMilliseconds > 0)
		{
			_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, saleChangeMilliseconds, OnTimer);
		}
	}

	private void OnTimer()
	{
		UpdateShopChange();
		DoUpdate();
		CreateTimer();
	}

	private void UpdateShopChange()
	{
		ShopPanel primaryPanel = GetPrimaryPanel();
		if (primaryPanel != null)
		{
			primaryPanel.CreateTiles();
		}
	}

	private ShopPanel GetPrimaryPanel()
	{
		foreach (ShopPanel panel in _panels)
		{
			if (panel.tabRef == null)
			{
				return panel;
			}
		}
		return null;
	}

	public int GetQty()
	{
		if (_dropdownMultiplierItems == null)
		{
			return 1;
		}
		return _selectedQty;
	}

	private ShopPanel GetPanel(int id)
	{
		if (id < 0 || id >= _panels.Count)
		{
			return null;
		}
		return _panels[id];
	}

	private ShopTab GetTab(int id)
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
		ShopPanel shopPanel = GetPanel(_currentTab);
		if (shopPanel != null)
		{
			shopPanel.DoHide();
		}
		_currentTab = id;
		ShopTab tab = GetTab(id);
		if (tab == null)
		{
			return;
		}
		ShopPanel component = GetPanel(_currentTab);
		if (component == null)
		{
			Transform obj = UnityEngine.Object.Instantiate(shopPanelPrefab);
			obj.SetParent(panel.transform, worldPositionStays: false);
			obj.GetComponent<ShopPanel>().DoShow();
			obj.GetComponent<ShopPanel>().LoadDetails(this, tab.tabRef);
			component = obj.GetComponent<ShopPanel>();
			while (_panels.Count <= _currentTab)
			{
				_panels.Add(null);
			}
			_panels[_currentTab] = component;
		}
		if ((bool)component)
		{
			component.DoShow();
			component.DoUpdate();
		}
	}

	private void OnTabClick(ShopTab tab)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(tab.index);
	}

	public void DoInventoryCheck(ItemRef itemRef)
	{
		_itemCheck = new ItemData(itemRef, GameData.instance.PROJECT.character.getItemQty(itemRef));
		Disable();
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		CharacterDALC.instance.doInventoryCheck();
	}

	private void OnInventoryCheck(BaseEvent baseEvent)
	{
		Enable();
		DALCEvent obj = baseEvent as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnInventoryCheck);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			List<ItemData> list = ItemData.listFromSFSObject(sfsob);
			if (list != null && list.Count > 0)
			{
				GameData.instance.PROJECT.character.updateInventoryItems(list);
			}
			if (GameData.instance.PROJECT.character.getItemQty(_itemCheck.itemRef) > _itemCheck.qty)
			{
				if (_itemCheck.itemRef.itemType == 4)
				{
					ConsumableRef consumableRef = _itemCheck.itemRef as ConsumableRef;
					ConsumableManager.instance.SetupConsumable(consumableRef, 1);
					ConsumableManager.instance.DoUseConsumable(1);
				}
			}
			else
			{
				Disable();
				GameData.instance.main.ShowLoading();
				StartCoroutine(Delay(2, _itemCheck.itemRef));
			}
		}
		_itemCheck = null;
	}

	private IEnumerator Delay(int seconds, ItemRef itemRef)
	{
		yield return new WaitForSeconds(seconds);
		DoInventoryCheck(itemRef);
	}

	public void DoUpdate()
	{
		foreach (ShopPanel panel in _panels)
		{
			if (!(panel == null))
			{
				panel.DoUpdate();
			}
		}
	}

	public override void DoDestroy()
	{
		ClearTimer();
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		SCROLL_IN_START.RemoveListener(OnScrollInStart);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowCurrencies(show: false);
		}
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (_tabs != null)
		{
			foreach (ShopTab tab in _tabs)
			{
				tab.tabBtn.interactable = true;
			}
		}
		multiplierDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (_tabs != null)
		{
			foreach (ShopTab tab in _tabs)
			{
				tab.tabBtn.interactable = false;
			}
		}
		multiplierDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
