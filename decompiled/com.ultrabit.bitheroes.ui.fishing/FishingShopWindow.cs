using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingShopWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI seashellTxt;

	public Image multiplierDropdown;

	public RectTransform placeholderTabs;

	private List<FishingShopTab> _tabs;

	private List<FishingShopPanel> _panels = new List<FishingShopPanel>();

	private int _defaultTab;

	private int _currentTab;

	public Transform fishingShopTabPrefab;

	public Transform fishingShopPanelPrefab;

	private Transform window;

	private int? currentQtyId;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_shop");
		CreateTabs();
		SetTab(_defaultTab);
		UpdateText();
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
		ListenForBack(OnClose);
		CreateWindow();
		CheckTutorial(scrolling: true);
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
		FishingShopPanel fishingShopPanel = GetPanel(_currentTab);
		if ((bool)fishingShopPanel)
		{
			fishingShopPanel.DoRefreshQty();
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
			_tabs = new List<FishingShopTab>();
			for (int i = 0; i < FishingShopBook.tabsSize; i++)
			{
				FishingShopTabRef fishingShopTabRef = FishingShopBook.LookupTab(i);
				AddTab(fishingShopTabRef.name, fishingShopTabRef);
			}
		}
	}

	private void AddTab(string name, FishingShopTabRef tabRef = null)
	{
		Transform tab = Object.Instantiate(fishingShopTabPrefab);
		tab.SetParent(placeholderTabs, worldPositionStays: false);
		tab.GetComponent<FishingShopTab>().LoadDetails(_tabs.Count, name, tabRef);
		tab.GetComponent<FishingShopTab>().tabBtn.onClick.AddListener(delegate
		{
			OnTabClick(tab.GetComponent<FishingShopTab>());
		});
		_tabs.Add(tab.GetComponent<FishingShopTab>());
	}

	private FishingShopPanel GetPanel(int id)
	{
		if (id < 0 || id >= _panels.Count)
		{
			return null;
		}
		return _panels[id];
	}

	private FishingShopTab GetTab(int id)
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
		FishingShopPanel fishingShopPanel = GetPanel(_currentTab);
		if ((bool)fishingShopPanel)
		{
			fishingShopPanel.DoHide();
		}
		_currentTab = id;
		FishingShopTab tab = GetTab(id);
		if (!tab)
		{
			return;
		}
		FishingShopPanel component = GetPanel(_currentTab);
		if (component == null)
		{
			Transform obj = Object.Instantiate(fishingShopPanelPrefab);
			obj.SetParent(panel.transform, worldPositionStays: false);
			obj.GetComponent<FishingShopPanel>().DoShow();
			obj.GetComponent<FishingShopPanel>().LoadDetails(this, tab.tabRef);
			component = obj.GetComponent<FishingShopPanel>();
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

	private void OnTabClick(FishingShopTab tab)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(tab.index);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void DoUpdate()
	{
		foreach (FishingShopPanel panel in _panels)
		{
			if ((bool)panel)
			{
				panel.DoUpdate();
			}
		}
		UpdateText();
	}

	private void UpdateText()
	{
		seashellTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.getItemQty(VariableBook.fishingShopItem), abbreviate: false);
	}

	private void OnCharacterInventoryChange()
	{
		DoUpdate();
	}

	public override void DoDestroy()
	{
		foreach (FishingShopTab tab in _tabs)
		{
			if (tab != null)
			{
				tab.tabBtn.onClick.RemoveListener(delegate
				{
					OnTabClick(tab.GetComponent<FishingShopTab>());
				});
			}
		}
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
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
