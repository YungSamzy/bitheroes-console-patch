using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.admin;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminCharacterWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button refreshBtn;

	public Button kickBtn;

	public Button banBtn;

	public Button unbanBtn;

	public Button renameBtn;

	public Button profileBtn;

	public Button inventoryBtn;

	public Button paymentsBtn;

	public Button platformsBtn;

	public Button purchasesBtn;

	public Button itemsBtn;

	public Button familiarStableBtn;

	public Button enchantsBtn;

	public Button mountsBtn;

	public Button offerwallBtn;

	public Button tradesBtn;

	public Transform infoListContent;

	public CharacterInfoTile characterInfoItemPrefab;

	private AdminCharacterData _characterData;

	private List<CharacterInfoTile> _infoTiles = new List<CharacterInfoTile>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(AdminCharacterData characterData)
	{
		kickBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_kick");
		banBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ban");
		unbanBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Unban";
		renameBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_name");
		profileBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Profile";
		inventoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Inventory";
		paymentsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Payments";
		platformsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Platforms";
		purchasesBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Purchases";
		itemsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_items");
		familiarStableBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_stable");
		enchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(11);
		mountsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(8);
		offerwallBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Offers";
		tradesBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Trades";
		SetCharacterData(characterData);
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: false, stayUp: true);
	}

	private void SetCharacterData(AdminCharacterData characterData)
	{
		_characterData = characterData;
		DoUpdate();
	}

	public void DoUpdate()
	{
		topperTxt.text = _characterData.name;
		int offset = 100;
		bool outline = false;
		bool abbreviate = false;
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData("Player", outline, offset);
		characterInfoData.addValue("ID", _characterData.id);
		characterInfoData.addValue("Name", _characterData.name);
		characterInfoData.addValue("Hero Tag", _characterData.herotag);
		characterInfoData.addValue("Platform", AppInfo.GetPlatformName(_characterData.platform));
		characterInfoData.addValue("IP Address", _characterData.ipAddress);
		characterInfoData.addValue("System", _characterData.system);
		characterInfoData.addValue("Language", _characterData.language);
		characterInfoData.addValue("Level", Util.NumberFormat(_characterData.level, abbreviate));
		characterInfoData.addValue("Exp", Util.NumberFormat(_characterData.exp, abbreviate));
		characterInfoData.addValue("Gold", Util.NumberFormat(_characterData.gold, abbreviate));
		characterInfoData.addValue("Gems", Util.NumberFormat(_characterData.credits, abbreviate));
		characterInfoData.addValue("Gems Purchased", Util.NumberFormat(_characterData.creditsPurchased, abbreviate));
		characterInfoData.addValue("Gems Spent", Util.NumberFormat(_characterData.creditsSpent, abbreviate));
		characterInfoData.addValue("Dollars Spent", Util.NumberFormat(_characterData.dollarsSpent, abbreviate));
		characterInfoData.addValue("Points", Util.NumberFormat(_characterData.points, abbreviate));
		characterInfoData.addValue("Energy", Util.NumberFormat(_characterData.energy, abbreviate));
		characterInfoData.addValue("Tickets", Util.NumberFormat(_characterData.tickets, abbreviate));
		characterInfoData.addValue("Shards", Util.NumberFormat(_characterData.shards, abbreviate));
		characterInfoData.addValue("Xeals", Util.NumberFormat(_characterData.seals, abbreviate));
		characterInfoData.addValue("Tokens", Util.NumberFormat(_characterData.tokens, abbreviate));
		characterInfoData.addValue("Badges", Util.NumberFormat(_characterData.badges, abbreviate));
		characterInfoData.addValue("Banned", _characterData.banned ? "true" : "false");
		characterInfoData.addValue("Create Date", Util.dateFormat(Util.localizeDate(_characterData.createDate)));
		characterInfoData.addValue("Login Date", Util.dateFormat(Util.localizeDate(_characterData.loginDate)));
		list.Add(characterInfoData);
		if (_characterData.guildInfo != null)
		{
			CharacterInfoData characterInfoData2 = new CharacterInfoData("Guild", outline, offset);
			characterInfoData2.addValue("ID", _characterData.guildInfo.id);
			characterInfoData2.addValue("Name", _characterData.guildInfo.name);
			characterInfoData2.addValue("Initials", _characterData.guildInfo.initials);
			list.Add(characterInfoData2);
		}
		if (_characterData.platforms.Count > 0)
		{
			CharacterInfoData characterInfoData3 = new CharacterInfoData("Platforms", outline, offset);
			foreach (CharacterPlatformData platform in _characterData.platforms)
			{
				if (platform.active)
				{
					characterInfoData3.addValue(AppInfo.GetPlatformName(platform.platform), platform.userID);
				}
			}
			if (characterInfoData3.info.Count > 0)
			{
				list.Add(characterInfoData3);
			}
		}
		banBtn.gameObject.SetActive(!_characterData.banned);
		unbanBtn.gameObject.SetActive(!banBtn.gameObject.activeSelf);
		CreateInfoTiles(list);
		StartCoroutine(WaitToFixText());
	}

	private void CreateInfoTiles(List<CharacterInfoData> info)
	{
		ClearInfoTiles();
		for (int i = 0; i < info.Count; i++)
		{
			CharacterInfoData statData = info[i];
			CharacterInfoTile characterInfoTile = Object.Instantiate(characterInfoItemPrefab, infoListContent);
			characterInfoTile.LoadDetails(statData);
			_infoTiles.Add(characterInfoTile);
		}
	}

	private void ClearInfoTiles()
	{
		foreach (CharacterInfoTile infoTile in _infoTiles)
		{
			Object.Destroy(infoTile.gameObject);
		}
		_infoTiles.Clear();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForSeconds(0.1f);
		infoListContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
		ForceScrollDown();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoRefresh();
	}

	public void OnKickBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoKickPlayerName(_characterData.name + "#" + _characterData.herotag);
	}

	public void OnBanBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoBanPlayerNameConfirm(_characterData.name + "#" + _characterData.herotag);
	}

	public void OnUnbanBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUnbanPlayerNameConfirm(_characterData.name + "#" + _characterData.herotag);
	}

	public void OnRenameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminRenameWindow(_characterData.name, 0, _characterData.herotag, base.gameObject);
	}

	public void OnProfileBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(_characterData.id, base.layer);
	}

	public void OnInventoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterInventory();
	}

	public void OnPaymentsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterPayments();
	}

	public void OnPlatformsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminCharacterPlatformsWindow(_characterData.id, _characterData.platforms, base.gameObject);
	}

	public void OnPurchasesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminCharacterPurchasesWindow(_characterData.id, base.gameObject);
	}

	public void OnItemsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminCharacterItemsWindow(_characterData.id, base.gameObject);
	}

	public void OnFamiliarStableBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFamiliarStableWindow();
	}

	public void OnEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantsWindow(_characterData.enchants);
	}

	public void OnMountBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewMountSelectWindow(_characterData.mounts);
	}

	public void OnOfferwallBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterOfferwalls();
	}

	private void DoKickPlayerName(string playerName)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnKickPlayerName);
		AdminDALC.instance.doKickPlayerName(playerName);
	}

	private void OnKickPlayerName(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnKickPlayerName);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void DoBanPlayerNameConfirm(string playerName)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow("Ban", "Are you sure you want to ban " + playerName + "?", null, null, delegate
		{
			OnBanPlayerNameConfirm(playerName);
		});
	}

	private void OnBanPlayerNameConfirm(string playerName)
	{
		DoBanPlayerName(playerName);
	}

	private void DoBanPlayerName(string playerName)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnBanPlayerName);
		AdminDALC.instance.doBanPlayerName(playerName);
	}

	private void OnBanPlayerName(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnBanPlayerName);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoRefresh();
		}
	}

	private void DoUnbanPlayerNameConfirm(string playerName)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow("Unban", "Are you sure you want to unban " + playerName + "?", null, null, delegate
		{
			OnUnbanPlayerNameConfirm(playerName);
		});
	}

	private void OnUnbanPlayerNameConfirm(string playerName)
	{
		DoUnbanPlayerName(playerName);
	}

	private void DoUnbanPlayerName(string playerName)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(29), OnUnbanPlayerName);
		AdminDALC.instance.doUnbanPlayerName(playerName);
	}

	private void OnUnbanPlayerName(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(29), OnUnbanPlayerName);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoRefresh();
		}
	}

	public void DoRefresh()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(15), OnRefresh);
		AdminDALC.instance.doCharacterSearch(_characterData.id);
	}

	private void OnRefresh(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(15), OnRefresh);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminCharacterData characterData = AdminCharacterData.fromSFSObject(sfsob);
		SetCharacterData(characterData);
	}

	private void DoCharacterInventory()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(16), OnCharacterInventory);
		AdminDALC.instance.doCharacterInventory(_characterData.id);
	}

	private void OnCharacterInventory(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(16), OnCharacterInventory);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		Inventory inventory = Inventory.fromSFSObject(sfsob);
		GameData.instance.windowGenerator.NewItemSearchWindow(inventory.items, adminWindow: true, null, showQty: true, closeOnSelect: true, null, showLock: false, tooltipSuggested: false, base.gameObject);
	}

	private void DoCharacterPayments()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(17), OnCharacterPayments);
		AdminDALC.instance.doCharacterPayments(_characterData.id, int.MaxValue);
	}

	private void OnCharacterPayments(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(17), OnCharacterPayments);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<CharacterPaymentData> list = CharacterPaymentData.listFromSFSObject(sfsob);
		int num = list.Count;
		int offset = 100;
		bool outline = false;
		List<CharacterInfoData> list2 = new List<CharacterInfoData>();
		foreach (CharacterPaymentData item in list)
		{
			CharacterInfoData characterInfoData = new CharacterInfoData("Payment " + Util.NumberFormat(num), outline, offset);
			characterInfoData.addValue("ID", Util.NumberFormat(item.id));
			characterInfoData.addValue("Date", Util.dateFormat(Util.localizeDate(item.createDate)));
			if (item.userID.Length > 0)
			{
				characterInfoData.addValue("User ID", item.userID);
			}
			characterInfoData.addValue("Platform", AppInfo.GetPlatformName(item.platform));
			characterInfoData.addValue("Gems", Util.NumberFormat(item.credits));
			characterInfoData.addValue("Dollars", Util.NumberFormat(item.dollars));
			characterInfoData.addValue("Payment ID", item.paymentID);
			characterInfoData.addValue("Order ID", item.orderID);
			list2.Add(characterInfoData);
			num--;
		}
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list2, "Payments", base.gameObject);
	}

	private void DoCharacterOfferwalls()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(28), OnCharacterOfferwalls);
		AdminDALC.instance.doCharacterOfferwalls(_characterData.id);
	}

	private void OnCharacterOfferwalls(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(28), OnCharacterOfferwalls);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<CharacterOfferwallData> list = CharacterOfferwallData.listFromSFSObject(sfsob);
		int num = list.Count;
		int offset = 100;
		bool outline = false;
		List<CharacterInfoData> list2 = new List<CharacterInfoData>();
		foreach (CharacterOfferwallData item in list)
		{
			CharacterInfoData characterInfoData = new CharacterInfoData("Offerwall " + Util.NumberFormat(num), outline, offset);
			characterInfoData.addValue("ID", Util.NumberFormat(item.id));
			characterInfoData.addValue("Date", Util.dateFormat(Util.localizeDate(item.createDate)));
			characterInfoData.addValue("Gems", Util.NumberFormat(item.credits));
			characterInfoData.addValue("Network", item.network);
			characterInfoData.addValue("Event ID", item.eventID);
			characterInfoData.addValue("Looted", item.looted ? "Yes" : "No");
			list2.Add(characterInfoData);
			num--;
		}
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list2, "Offerwalls", base.gameObject);
	}

	public void OnTradesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoItemSelect();
	}

	private void DoItemSelect()
	{
		List<ItemRef> allItems = ItemBook.GetAllItems();
		List<ItemData> list = new List<ItemData>();
		foreach (ItemRef item in allItems)
		{
			int itemType = item.itemType;
			if (itemType != 5 && itemType != 16)
			{
				list.Add(new ItemData(item, 1));
			}
		}
		GameData.instance.windowGenerator.NewItemSearchWindow(list, adminWindow: false, null, showQty: false, closeOnSelect: true, null, showLock: false, tooltipSuggested: false, base.gameObject).SELECT.AddListener(OnItemSelect);
	}

	private void OnItemSelect(object e)
	{
		ItemRef selectedItem = (e as ItemSearchWindow).selectedItem;
		if (selectedItem.itemType == 4)
		{
			if ((selectedItem as ConsumableRef).consumableType == 4)
			{
				GameData.instance.windowGenerator.NewItemContentsWindow(selectedItem, 1, null, null, purchaseable: false);
			}
			else
			{
				DoInfoExchange(selectedItem);
			}
		}
		else
		{
			DoInfoExchange(selectedItem);
		}
	}

	private void DoInfoExchange(ItemRef itemRef)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(27), OnInfoExchange);
		AdminDALC.instance.doInfoExchange(itemRef);
	}

	private void OnInfoExchange(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(27), OnInfoExchange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> items = ItemData.listFromSFSObject(sfsob);
		GameData.instance.windowGenerator.ShowItems(items, compare: false, added: false, Language.GetString("ui_exchange"));
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
		kickBtn.interactable = true;
		banBtn.interactable = true;
		unbanBtn.interactable = true;
		renameBtn.interactable = true;
		profileBtn.interactable = true;
		inventoryBtn.interactable = true;
		paymentsBtn.interactable = true;
		platformsBtn.interactable = true;
		purchasesBtn.interactable = true;
		itemsBtn.interactable = true;
		familiarStableBtn.interactable = true;
		enchantsBtn.interactable = true;
		mountsBtn.interactable = true;
		offerwallBtn.interactable = true;
		tradesBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
		kickBtn.interactable = false;
		banBtn.interactable = false;
		unbanBtn.interactable = false;
		renameBtn.interactable = false;
		profileBtn.interactable = false;
		inventoryBtn.interactable = false;
		paymentsBtn.interactable = false;
		platformsBtn.interactable = false;
		purchasesBtn.interactable = false;
		itemsBtn.interactable = false;
		familiarStableBtn.interactable = false;
		enchantsBtn.interactable = false;
		mountsBtn.interactable = false;
		offerwallBtn.interactable = false;
		tradesBtn.interactable = false;
	}
}
