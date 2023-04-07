using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.admin;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.server;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI messageTitleTxt;

	public TextMeshProUGUI searchTitleTxt;

	public TextMeshProUGUI serverTitleTxt;

	public TextMeshProUGUI serverSelectedText;

	public TextMeshProUGUI charactersTxt;

	public Button toolsBtn;

	public Button messageGlobalBtn;

	public Button messageInstanceBtn;

	public Button playerMeBtn;

	public Button playerIDBtn;

	public Button playerNameBtn;

	public Button playerKongIDBtn;

	public Button playerOrderIDBtn;

	public Button guildMineBtn;

	public Button guildIDBtn;

	public Button guildNameBtn;

	public Button guildInitialsBtn;

	public Button wikiBtn;

	public Button itemsBtn;

	public Button zonesBtn;

	public TMP_InputField messageTxt;

	public TMP_InputField searchNameTxt;

	public Image serverDropdown;

	private Transform window;

	private string selectedServerInstance;

	private List<MyDropdownItemModel> difficultyData;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_admin");
		messageTitleTxt.text = Language.GetString("ui_message");
		searchTitleTxt.text = Language.GetString("ui_search");
		serverTitleTxt.text = "Server Instance";
		toolsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Tools";
		messageGlobalBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Global";
		messageInstanceBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Instance";
		playerMeBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Me";
		playerIDBtn.GetComponentInChildren<TextMeshProUGUI>().text = "ID";
		playerNameBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Name";
		playerKongIDBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Kong ID";
		playerOrderIDBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Order ID";
		guildMineBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Mine";
		guildIDBtn.GetComponentInChildren<TextMeshProUGUI>().text = "ID";
		guildNameBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Name";
		guildInitialsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Initials";
		wikiBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Wiki";
		itemsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Trades";
		zonesBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Zones";
		messageTxt.text = "";
		searchNameTxt.text = "";
		charactersTxt.text = "";
		messageTxt.onSubmit.AddListener(delegate
		{
			SendGlobalMessage();
		});
		CreateServerDropdown();
		DoUpdateInfo();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateServerDropdown()
	{
		difficultyData = new List<MyDropdownItemModel>();
		D.Log("AdminWindow::CreateServerDropdown " + ServerExtension.instance.serverInstanceID);
		selectedServerInstance = ServerExtension.instance.serverInstanceID;
		ServerRef sELECTED_SERVER = GameData.instance.main.SELECTED_SERVER;
		if (sELECTED_SERVER != null)
		{
			foreach (ServerInstanceRef instance in sELECTED_SERVER.instances)
			{
				MyDropdownItemModel item = new MyDropdownItemModel
				{
					id = int.Parse(instance.id),
					title = instance.id,
					btnHelp = false
				};
				difficultyData.Add(item);
			}
		}
		serverSelectedText.text = selectedServerInstance;
	}

	public void OnServerDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_server"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, int.Parse(selectedServerInstance), OnServerChanged);
		componentInChildren.Data.InsertItems(0, difficultyData);
	}

	public void OnServerChanged(MyDropdownItemModel model)
	{
		ServerExtension.instance.ChangeServerInstance(model.id.ToString());
	}

	private void DoUpdateInfo()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnUpdateInfo);
		AdminDALC.instance.doInfoGeneral();
	}

	private void OnUpdateInfo(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnUpdateInfo);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		sfsob.GetInt("serv5");
		int @int = sfsob.GetInt("serv6");
		charactersTxt.text = @int.ToString();
	}

	public void OnMessageGlobalBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SendGlobalMessage();
	}

	public void OnMessageInstanceBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SendGlobalMessage(instance: true);
	}

	public void OnToolsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminServerWindow(base.gameObject);
	}

	public void OnCheckInfoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUpdateInfo();
	}

	public void OnPlayerMeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		searchNameTxt.SetTextWithoutNotify(GameData.instance.PROJECT.character.id.ToString());
		OnPlayerIDBtn();
	}

	public void OnPlayerIDBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int.TryParse(searchNameTxt.text, out var result);
		DoCharacterSearch(result);
	}

	public void OnPlayerNameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterSearch(0, searchNameTxt.text);
	}

	public void OnPlayerKongIDBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterSearch(0, null, 4, searchNameTxt.text);
	}

	public void OnPlayerOrderIDBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCharacterSearch(0, null, -1, null, searchNameTxt.text);
	}

	public void OnGuildMineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.guildData == null)
		{
			GameData.instance.windowGenerator.ShowDialogMessage("Error", "You are not part of any guild.");
			return;
		}
		searchNameTxt.SetTextWithoutNotify(GameData.instance.PROJECT.character.guildData.id.ToString());
		OnGuildIDBtn();
	}

	public void OnGuildIDBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int.TryParse(searchNameTxt.text, out var result);
		DoGuildSearch(result);
	}

	public void OnGuildNameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoGuildSearch(0, searchNameTxt.text);
	}

	public void OnGuildInitialsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoGuildSearch(0, null, searchNameTxt.text);
	}

	public void OnWikiBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL("http://bit-heroes.wikia.com/wiki/Bit_Heroes_Wiki");
	}

	public void OnItemsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoItemSelect();
	}

	public void OnZonesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoZoneInfo();
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

	private void DoZoneInfo()
	{
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		for (int i = 0; i <= ZoneBook.size; i++)
		{
			ZoneRef zoneRef = ZoneBook.Lookup(i);
			if (zoneRef == null)
			{
				continue;
			}
			foreach (ZoneNodeRef node in zoneRef.nodes)
			{
				if (node == null)
				{
					continue;
				}
				foreach (ZoneNodeDifficultyRef difficulty in node.difficulties)
				{
					if (difficulty == null || difficulty.rewards == null || difficulty.rewards.Count <= 0)
					{
						continue;
					}
					CharacterInfoData characterInfoData = new CharacterInfoData("Z" + zoneRef.id + " (" + zoneRef.name + ") : N" + node.id + " (" + node.name + ")", outline: true, 90);
					foreach (ItemData reward in difficulty.rewards)
					{
						characterInfoData.addValue(reward.itemRef.name, reward.qty);
					}
					list.Add(characterInfoData);
				}
			}
		}
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list, Language.GetString("ui_zones"));
	}

	private void DoCharacterSearch(int charID, string name = null, int platform = -1, string userID = null, string orderID = null)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(15), OnCharacterSearch);
		AdminDALC.instance.doCharacterSearch(charID, name, platform, userID, orderID);
	}

	private void OnCharacterSearch(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(15), OnCharacterSearch);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey(ServerConstants.TOTAL_CHARACTERS_NAMES))
		{
			ISFSArray sFSArray = sfsob.GetSFSArray(ServerConstants.CHARACTERS_DATA);
			List<CharacterHeroTagData> list = new List<CharacterHeroTagData>();
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				list.Add(CharacterHeroTagData.FromSFSObject(sFSObject));
			}
			GameData.instance.windowGenerator.NewCharactersSearchListWindow(list, 0, showSelect: true, OnSelectCharactersSearchListWindow, base.gameObject);
		}
		else
		{
			AdminCharacterData characterData = AdminCharacterData.fromSFSObject(sfsob);
			GameData.instance.windowGenerator.NewAdminCharacterWindow(characterData, base.gameObject);
		}
	}

	private void OnSelectCharactersSearchListWindow(string name)
	{
		searchNameTxt.text = name;
	}

	private void DoGuildSearch(int guildID, string name = null, string initials = null)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(23), OnGuildSearch);
		AdminDALC.instance.doGuildSearch(guildID, name, initials);
	}

	private void OnGuildSearch(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(23), OnGuildSearch);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminGuildData guildData = AdminGuildData.fromSFSObject(sfsob);
		GameData.instance.windowGenerator.NewAdminGuildWindow(guildData, base.gameObject);
	}

	private void SendGlobalMessage(bool instance = false)
	{
		if (messageTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError("Enter a message");
		}
		else
		{
			AdminDALC.instance.doGlobalMessage(messageTxt.text, instance);
		}
	}

	public override void DoDestroy()
	{
		messageTxt.onSubmit.RemoveListener(delegate
		{
			SendGlobalMessage();
		});
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		toolsBtn.interactable = true;
		messageGlobalBtn.interactable = true;
		messageInstanceBtn.interactable = true;
		playerIDBtn.interactable = true;
		playerNameBtn.interactable = true;
		playerKongIDBtn.interactable = true;
		playerOrderIDBtn.interactable = true;
		guildIDBtn.interactable = true;
		guildNameBtn.interactable = true;
		guildInitialsBtn.interactable = true;
		wikiBtn.interactable = true;
		itemsBtn.interactable = true;
		zonesBtn.interactable = true;
		messageTxt.interactable = true;
		searchNameTxt.interactable = true;
		serverDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		toolsBtn.interactable = false;
		messageGlobalBtn.interactable = false;
		messageInstanceBtn.interactable = false;
		playerIDBtn.interactable = false;
		playerNameBtn.interactable = false;
		playerKongIDBtn.interactable = false;
		playerOrderIDBtn.interactable = false;
		guildIDBtn.interactable = false;
		guildNameBtn.interactable = false;
		guildInitialsBtn.interactable = false;
		wikiBtn.interactable = false;
		itemsBtn.interactable = false;
		zonesBtn.interactable = false;
		messageTxt.interactable = false;
		searchNameTxt.interactable = false;
		serverDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
