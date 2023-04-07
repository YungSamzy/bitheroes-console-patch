using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.payment.credits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.service;

public class ServiceWindow : WindowsMain
{
	public Button creditsBtn;

	public Button goldBtn;

	public Button energyBtn;

	public Button ticketsBtn;

	public Button otherBtn;

	public Transform servicePanel;

	public Transform paymentCreditsPanel;

	public Transform paymentCreditsPanelAndroid;

	public Transform paymentCreditsPanelFacebook;

	public Transform paymentCreditsPanelIOS;

	public Transform paymentCreditsPanelKartridge;

	public Transform paymentCreditsPanelKongregate;

	public Transform paymentCreditsPanelSteam;

	public Transform paymentCreditsPanelLocal;

	public const int TAB_CREDITS = 0;

	public const int TAB_GOLD = 1;

	public const int TAB_ENERGY = 2;

	public const int TAB_TICKETS = 3;

	public const int TAB_OTHER = 4;

	private static Dictionary<string, int> TABS = new Dictionary<string, int>
	{
		["credits"] = 0,
		["gold"] = 1,
		["energy"] = 2,
		["tickets"] = 3,
		["other"] = 4
	};

	private List<Button> _tabs;

	private List<GameObject> _panels = new List<GameObject>();

	private int _currentTab = -1;

	public override void Start()
	{
		if (!VariableBook.GameRequirementMet(1))
		{
			ticketsBtn.gameObject.SetActive(value: false);
			(otherBtn.transform as RectTransform).anchoredPosition = (ticketsBtn.transform as RectTransform).anchoredPosition;
		}
		base.Start();
		Disable();
	}

	public void LoadDetails(int tab = 0, ServiceRef highlightedRef = null)
	{
		SetTabButtons();
		SetTab(tab, highlightedRef);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnCreditsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnGoldBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	public void OnEnergyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(2);
	}

	public void OnTicketsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(3);
	}

	public void OnOtherBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(4);
	}

	public void SetTabButtons()
	{
		if (_tabs == null)
		{
			_tabs = new List<Button>();
			SetTabButton(creditsBtn, CurrencyRef.GetCurrencyName(2), 0);
			SetTabButton(goldBtn, CurrencyRef.GetCurrencyName(1), 1);
			SetTabButton(energyBtn, CurrencyRef.GetCurrencyName(4), 2);
			SetTabButton(ticketsBtn, CurrencyRef.GetCurrencyName(5), 3);
			SetTabButton(otherBtn, Language.GetString("ui_other"), 4);
		}
	}

	private void SetTabButton(Button button, string text, int tab)
	{
		button.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(text);
		while (_tabs.Count <= tab)
		{
			_tabs.Add(null);
		}
		_tabs[tab] = button;
	}

	private Button GetTabButton(int tab)
	{
		if (tab < 0 || tab >= _tabs.Count)
		{
			return null;
		}
		return _tabs[tab];
	}

	private GameObject GetPanel(int tab)
	{
		if (tab < 0 || tab >= _panels.Count)
		{
			return null;
		}
		return _panels[tab];
	}

	public void SetTab(int tab, ServiceRef highlightedRef = null)
	{
		for (int i = 0; i < _tabs.Count; i++)
		{
			Util.SetTab(_tabs[i], i == tab);
		}
		GameObject gameObject = GetPanel(_currentTab);
		if (gameObject != null)
		{
			gameObject.BroadcastMessage("HideServicePanel");
		}
		_currentTab = tab;
		GameObject gameObject2 = GetPanel(_currentTab);
		if (!gameObject2)
		{
			switch (_currentTab)
			{
			case 0:
				switch (AppInfo.platform)
				{
				case 0:
					gameObject2 = Object.Instantiate(paymentCreditsPanelLocal).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelLocal>().LoadDetails();
					break;
				case 1:
					gameObject2 = Object.Instantiate(paymentCreditsPanelAndroid).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelAndroid>().LoadDetails();
					break;
				case 2:
					gameObject2 = Object.Instantiate(paymentCreditsPanelIOS).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelIOS>().LoadDetails();
					break;
				case 4:
					gameObject2 = Object.Instantiate(paymentCreditsPanelKongregate).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelKongregate>().LoadDetails();
					break;
				case 5:
					gameObject2 = Object.Instantiate(paymentCreditsPanelFacebook).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelFacebook>().LoadDetails();
					break;
				case 7:
					gameObject2 = Object.Instantiate(paymentCreditsPanelSteam).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelSteam>().LoadDetails();
					break;
				case 8:
					gameObject2 = Object.Instantiate(paymentCreditsPanelKartridge).gameObject;
					gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
					gameObject2.GetComponent<PaymentCreditsPanelKartridge>().LoadDetails();
					break;
				}
				break;
			case 1:
				gameObject2 = Object.Instantiate(servicePanel).gameObject;
				gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
				gameObject2.GetComponent<ServicePanel>().LoadDetails(ServiceBook.GetServicesByType(1), highlightedRef);
				break;
			case 2:
				gameObject2 = Object.Instantiate(servicePanel).gameObject;
				gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
				gameObject2.GetComponent<ServicePanel>().LoadDetails(ServiceBook.GetServicesByType(2), highlightedRef);
				break;
			case 3:
				gameObject2 = Object.Instantiate(servicePanel).gameObject;
				gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
				gameObject2.GetComponent<ServicePanel>().LoadDetails(ServiceBook.GetServicesByType(3), highlightedRef);
				break;
			case 4:
				gameObject2 = Object.Instantiate(servicePanel).gameObject;
				gameObject2.transform.SetParent(panel.transform, worldPositionStays: false);
				gameObject2.GetComponent<ServicePanel>().LoadDetails(ServiceBook.GetOtherServices(), highlightedRef);
				break;
			}
			if ((bool)gameObject2)
			{
				while (_panels.Count <= _currentTab)
				{
					_panels.Add(null);
				}
				_panels[_currentTab] = gameObject2;
			}
		}
		if ((bool)gameObject2)
		{
			gameObject2.BroadcastMessage("ShowServicePanel");
		}
	}

	public static int GetTab(string type)
	{
		return TABS[type.ToLowerInvariant()];
	}

	public override void DoDestroy()
	{
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowCurrencies(show: false);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (_tabs == null)
		{
			return;
		}
		for (int i = 0; i < _tabs.Count; i++)
		{
			if (_tabs[i] != null)
			{
				_tabs[i].interactable = true;
			}
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (_tabs == null)
		{
			return;
		}
		for (int i = 0; i < _tabs.Count; i++)
		{
			if (_tabs[i] != null)
			{
				_tabs[i].interactable = false;
			}
		}
	}
}
