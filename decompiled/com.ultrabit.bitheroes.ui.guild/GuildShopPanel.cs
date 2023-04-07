using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.guildshoplist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildShopPanel : MonoBehaviour
{
	private const string BLANK = "-";

	public TextMeshProUGUI honorTxt;

	public GuildShopList guildShopList;

	public GameObject guildShopListView;

	public GameObject guildShopListScroll;

	public Button honorBtn;

	public Image multiplierDropdown;

	public GameObject honorIcon;

	public GameObject textBox;

	public Image loadingIcon;

	private Transform window;

	private int? currentQtyId;

	public void LoadDetails()
	{
		honorBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		multiplierDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_number_multiplier", new string[1] { Util.NumberFormat(1f) });
		guildShopList.InitList();
		TransactionManager.instance.AddListener("PURCHASE_COMPLETE", OnPurchaseComplete);
	}

	private void OnPurchaseComplete()
	{
		DoLoadShop();
	}

	private void SetHonor(int honor = -1)
	{
		honorTxt.text = ((honor >= 0) ? Util.NumberFormat(honor) : "-");
	}

	private void DoLoadShop()
	{
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(22), OnLoadShop);
		GuildDALC.instance.doLoadShop();
	}

	private void OnLoadShop(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(22), OnLoadShop);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt("gui10");
		SetHonor(@int);
		GuildData guildData = GuildData.fromSFSObject(sfsob);
		if (loadingIcon != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		UpdateList(guildData);
	}

	private void UpdateList(GuildData guildData)
	{
		if (!guildShopList.IsInitialized)
		{
			StartCoroutine(WaitAndUpdate(guildData, 0.1f));
			return;
		}
		List<GuildShopItem> list = new List<GuildShopItem>();
		for (int i = 0; i < GuildShopBook.size; i++)
		{
			GuildShopRef shopRef = GuildShopBook.Lookup(i);
			list.Add(new GuildShopItem
			{
				shopRef = shopRef,
				guildData = guildData,
				guildShopPanel = this
			});
		}
		double virtualAbstractNormalizedScrollPosition = guildShopList.GetVirtualAbstractNormalizedScrollPosition();
		guildShopList.ClearList();
		guildShopList.Data.InsertItemsAtStart(list);
		guildShopList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	private IEnumerator WaitAndUpdate(GuildData guildData, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		UpdateList(guildData);
	}

	public void OnHonorBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(CurrencyRef.GetCurrencyName(7), Util.ParseString(CurrencyRef.GetCurrencyDesc(7)));
	}

	public void OnMultiplierDropdown()
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
		guildShopList.Refresh();
	}

	public int GetQty()
	{
		if (!currentQtyId.HasValue)
		{
			return 1;
		}
		return int.Parse(VariableBook.shopQuantities[currentQtyId.Value]);
	}

	public void Show()
	{
		honorTxt.gameObject.SetActive(value: true);
		guildShopListView.SetActive(value: true);
		guildShopListScroll.SetActive(value: true);
		honorBtn.gameObject.SetActive(value: true);
		multiplierDropdown.gameObject.SetActive(value: true);
		honorIcon.SetActive(value: true);
		textBox.SetActive(value: true);
		loadingIcon.gameObject.SetActive(value: true);
		DoEnable();
	}

	public void Hide()
	{
		honorTxt.gameObject.SetActive(value: false);
		guildShopListView.SetActive(value: false);
		guildShopListScroll.SetActive(value: false);
		honorBtn.gameObject.SetActive(value: false);
		multiplierDropdown.gameObject.SetActive(value: false);
		honorIcon.SetActive(value: false);
		textBox.SetActive(value: false);
		loadingIcon.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		honorBtn.interactable = true;
		multiplierDropdown.GetComponent<EventTrigger>().enabled = true;
		DoLoadShop();
		guildShopList.ClearList();
		SetHonor();
	}

	public void DoDisable()
	{
		honorBtn.interactable = false;
		multiplierDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
