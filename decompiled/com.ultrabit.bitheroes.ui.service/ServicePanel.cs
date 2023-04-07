using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.events.character;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.lists.servicelist;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.service;

public class ServicePanel : MonoBehaviour
{
	public GameObject serviceListView;

	public GameObject serviceListScroll;

	public Button consumablesBtn;

	public ServiceList serviceList;

	private List<ServiceRef> services;

	private bool visible;

	public void LoadDetails(List<ServiceRef> services, ServiceRef highlightedRef = null)
	{
		HideServicePanel();
		this.services = services;
		CreateTiles(highlightedRef);
	}

	private void CreateTiles(ServiceRef highlightedRef = null)
	{
		serviceList.InitList(OnItemClick);
		serviceList.ClearList();
		foreach (ServiceRef service in services)
		{
			int num = 0;
			int num2 = 0;
			if (service.costCredits > 0)
			{
				num = service.costCredits;
				num2 = 2;
			}
			else
			{
				num = service.costGold;
				num2 = 1;
			}
			bool highlighted = ((highlightedRef != null && highlightedRef.id == service.id) ? true : false);
			Sprite spriteIcon = service.GetSpriteIcon();
			string text = ((service.desc != null) ? Language.GetString(service.desc) : "");
			serviceList.Data.InsertOneAtEnd(new ServiceListItemModel
			{
				id = service.id,
				name = service.coloredName,
				description = Util.ParseString(text),
				cost = num,
				currencyID = num2,
				highlighted = highlighted,
				icon = spriteIcon
			});
		}
	}

	private void OnItemClick(int id)
	{
		ServiceRef s = null;
		foreach (ServiceRef service in services)
		{
			if (service.id.Equals(id))
			{
				s = service;
				break;
			}
		}
		if (s != null)
		{
			string @string = Language.GetString("purchase_confirm", new string[3]
			{
				s.coloredName,
				Util.NumberFormat(s.GetServiceCost()),
				Language.GetString(s.GetPurchaseCurrencyRef().name)
			});
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), @string, Language.GetString("ui_yes"), Language.GetString("ui_no"), delegate
			{
				StartCoroutine(DoPurchase(s));
			});
		}
	}

	private IEnumerator DoPurchase(ServiceRef service)
	{
		yield return new WaitForSeconds(0.5f);
		TransactionManager.instance.DoItemPurchase(service, service.GetPurchaseCurrencyRef().id, service.GetServiceCost());
	}

	public void OnConsumablesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowItems(GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(GetServiceCurrency()), compare: false, added: true);
	}

	private void OnInventoryChange(CharacterEvent e)
	{
		if (visible)
		{
			UpdateButtons();
		}
	}

	private void UpdateButtons()
	{
		int serviceCurrency = GetServiceCurrency();
		List<ItemData> consumablesByCurrencyID = GameData.instance.PROJECT.character.inventory.getConsumablesByCurrencyID(serviceCurrency);
		consumablesBtn.gameObject.SetActive(serviceCurrency > 0 && consumablesByCurrencyID.Count > 0);
	}

	private int GetServiceCurrency()
	{
		if (services != null)
		{
			foreach (ServiceRef service in services)
			{
				int currencyID = service.getCurrencyID();
				if (currencyID > 0)
				{
					return currencyID;
				}
			}
			return -1;
		}
		return -1;
	}

	public void ShowServicePanel()
	{
		visible = true;
		serviceListView.SetActive(value: true);
		serviceListScroll.SetActive(value: true);
		UpdateButtons();
		Debug.Log("ShowServicePanel");
	}

	public void HideServicePanel()
	{
		visible = false;
		serviceListView.SetActive(value: false);
		serviceListScroll.SetActive(value: false);
		consumablesBtn.gameObject.SetActive(value: false);
		Debug.Log("HideServicePanel");
	}

	public void DoEnable()
	{
		consumablesBtn.interactable = true;
	}

	public void DoDisable()
	{
		consumablesBtn.interactable = false;
	}
}
