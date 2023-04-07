using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.guildapplicantslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildApplicantsWindow : WindowsMain
{
	public const int SORT_LOGIN = 0;

	public const int SORT_LEVEL = 1;

	public const int SORT_STATS = 2;

	public const int SORT_POWER = 3;

	public const int SORT_STAMINA = 4;

	public const int SORT_AGILITY = 5;

	public const int SORT_NAME = 6;

	private static Dictionary<int, string> SORT_NAMES = new Dictionary<int, string>
	{
		[0] = "ui_login",
		[1] = "ui_level",
		[2] = "ui_stats",
		[3] = "stat_power",
		[4] = "stat_stamina",
		[5] = "stat_agility",
		[6] = "ui_name"
	};

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI lvlListTxt;

	public Button declineAllBtn;

	public Button refreshBtn;

	public Image sortDropdown;

	public Image loadingIcon;

	public GuildApplicantsList guildApplicantsList;

	private Transform dropdownWindow;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	public TextMeshProUGUI emptyTxt;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_applications");
		nameListTxt.text = Language.GetString("ui_name");
		lvlListTxt.text = Language.GetString("ui_lvl");
		declineAllBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline_all");
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		emptyTxt.text = Language.GetString("ui_guild_applicants_empty");
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_RANK_CHANGE", OnChange);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		guildApplicantsList.InitList();
		sortDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(SORT_NAMES[GameData.instance.SAVE_STATE.GetGuildApplicantSort(GameData.instance.PROJECT.character.id)]);
		DoApplicationsLoad();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnChange()
	{
		if (!GameData.instance.PROJECT.character.guildData.hasPermission(2))
		{
			base.OnClose();
		}
	}

	public override void UpdateSortingLayers(int layer)
	{
		D.Log("nacho", "UpdateSortingLayers");
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + _sortingLayer + 98;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + _sortingLayer + 98;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _sortingLayer;
	}

	public void DoApplicationsLoad()
	{
		guildApplicantsList.ClearList();
		Util.SetButton(declineAllBtn, enabled: false);
		Util.SetButton(refreshBtn, enabled: false);
		loadingIcon.gameObject.SetActive(value: true);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(17), OnApplicationsLoad);
		GuildDALC.instance.doApplicationsLoad();
	}

	private void OnApplicationsLoad(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Util.SetButton(declineAllBtn);
		Util.SetButton(refreshBtn);
		loadingIcon.gameObject.SetActive(value: false);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(17), OnApplicationsLoad);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("use6");
		List<UserData> list = new List<UserData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(UserData.fromSFSObject(sFSObject));
		}
		CreateList(list);
		UpdateEmptyText();
	}

	private void UpdateEmptyText()
	{
		emptyTxt.gameObject.SetActive(guildApplicantsList.Data.Count <= 0);
	}

	private void CreateList(List<UserData> users)
	{
		List<GuildApplicantsSortTile> list = new List<GuildApplicantsSortTile>();
		string[] names = new string[0];
		for (int i = 0; i < users.Count; i++)
		{
			list.Add(new GuildApplicantsSortTile(users[i]));
		}
		switch (GameData.instance.SAVE_STATE.GetGuildApplicantSort(GameData.instance.PROJECT.character.id))
		{
		case 0:
			names = new string[3] { "loginMilliseconds", "stats", "level" };
			break;
		case 1:
			names = new string[2] { "level", "stats" };
			break;
		case 2:
			names = new string[2] { "stats", "level" };
			break;
		case 3:
			names = new string[3] { "power", "stats", "level" };
			break;
		case 4:
			names = new string[3] { "stamina", "stats", "level" };
			break;
		case 5:
			names = new string[3] { "agility", "stats", "level" };
			break;
		case 6:
			names = new string[1] { "name" };
			break;
		}
		List<GuildApplicantsSortTile> list2 = Util.SortVector(list, names, Util.ARRAY_DESCENDING);
		for (int j = 0; j < list2.Count; j++)
		{
			guildApplicantsList.Data.InsertOneAtEnd(new GuildApplicantItem
			{
				userData = list2[j].userData
			});
		}
	}

	public void DoApplicationDeclineAll()
	{
		if (guildApplicantsList.Data.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_guild_applications"));
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_decline_guild_applications_confirm"), null, null, delegate
		{
			OnApplicationDeclineAllYes();
		});
	}

	private void OnApplicationDeclineAllYes()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(16), OnApplicationDeclineAll);
		GuildDALC.instance.doApplicationDeclineAll();
	}

	private void OnApplicationDeclineAll(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(16), OnApplicationDeclineAll);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		guildApplicantsList.ClearList();
		base.OnClose();
	}

	public void OnSortDropdownBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetGuildApplicantSort(GameData.instance.PROJECT.character.id), OnSortDropdownChange);
		for (int i = 0; i < SORT_NAMES.Count; i++)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = i,
				title = Language.GetString(SORT_NAMES[i]),
				btnHelp = false,
				data = null
			});
		}
	}

	private void OnSortDropdownChange(MyDropdownItemModel model)
	{
		GameData.instance.SAVE_STATE.SetGuildApplicantSort(GameData.instance.PROJECT.character.id, model?.id ?? 0);
		sortDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(SORT_NAMES[GameData.instance.SAVE_STATE.GetGuildApplicantSort(GameData.instance.PROJECT.character.id)]);
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
		DoApplicationsLoad();
	}

	public void OnDeclineAllBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoApplicationDeclineAll();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoApplicationsLoad();
	}

	public override void DoDestroy()
	{
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_RANK_CHANGE", OnChange);
			GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		declineAllBtn.interactable = true;
		refreshBtn.interactable = true;
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		declineAllBtn.interactable = false;
		refreshBtn.interactable = false;
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
