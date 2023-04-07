using System.Collections.Generic;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.friendrequestlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.friend;

public class FriendRequestWindow : WindowsMain
{
	public const int SORT_LOGIN = 0;

	public const int SORT_LEVEL = 1;

	public const int SORT_STATS = 2;

	public const int SORT_POWER = 3;

	public const int SORT_STAMINA = 4;

	public const int SORT_AGILITY = 5;

	public const int SORT_NAME = 6;

	private static List<string> SORT_NAMES = new List<string>();

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI lvlListTxt;

	public Button declineAllBtn;

	public Image sortDropdown;

	public FriendRequestList friendList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	public TextMeshProUGUI txtDropdown;

	private int selectedRarityOption;

	private Transform window;

	private new int _sortingLayer;

	private List<MyDropdownItemModel> dropdownModel = new List<MyDropdownItemModel>();

	private MyDropdownItemModel selectedFilterOption;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		SORT_NAMES.Clear();
		SORT_NAMES.Add("ui_login");
		SORT_NAMES.Add("ui_level");
		SORT_NAMES.Add("ui_stats");
		SORT_NAMES.Add("stat_power");
		SORT_NAMES.Add("stat_stamina");
		SORT_NAMES.Add("stat_agility");
		SORT_NAMES.Add("ui_name");
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_friend_requests");
		nameListTxt.text = Language.GetString("ui_name");
		lvlListTxt.text = Language.GetString("ui_lvl");
		declineAllBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline_all");
		selectedRarityOption = GameData.instance.SAVE_STATE.GetFriendRequestSort();
		OnCreateList();
		CreateDropdownModel();
		UpdateDropdownText();
		ListenForBack(OnClose);
		CreateWindow();
		UpdateSortingLayers(_sortingLayer);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + _sortingLayer + ((friendList.Data != null) ? friendList.Data.Count : friendList.Content.childCount);
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + _sortingLayer + ((friendList.Data != null) ? friendList.Data.Count : friendList.Content.childCount);
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _sortingLayer;
	}

	public void OnSortDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_items"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, selectedRarityOption, OnFilterOptionSelected);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItems(0, dropdownModel);
	}

	public void OnFilterOptionSelected(MyDropdownItemModel model)
	{
		selectedFilterOption = model;
		if (selectedFilterOption.data != null)
		{
			selectedRarityOption = (int)selectedFilterOption.data;
			GameData.instance.SAVE_STATE.SetFriendRequestSort(selectedRarityOption);
		}
		UpdateDropdownText();
		OnCreateList();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnDeclineAllBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.DoDenyAllRequests();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		declineAllBtn.interactable = true;
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
		GameData.instance.PROJECT.character.AddListener("FRIEND_CHANGE", OnFriendChange);
		GameData.instance.PROJECT.character.AddListener("REQUEST_CHANGE", OnFriendChange);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		declineAllBtn.interactable = false;
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
		GameData.instance.PROJECT.character.RemoveListener("FRIEND_CHANGE", OnFriendChange);
		GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", OnFriendChange);
	}

	private void OnFriendChange()
	{
		OnCreateList();
		if (GameData.instance.PROJECT.character.requests.Count <= 0)
		{
			OnClose();
		}
	}

	private void CreateDropdownModel()
	{
		dropdownModel.Clear();
		for (int i = 0; i < SORT_NAMES.Count; i++)
		{
			dropdownModel.Add(new MyDropdownItemModel
			{
				id = i,
				title = Language.GetString(SORT_NAMES[i]),
				btnHelp = false,
				data = i
			});
		}
		int selected = selectedRarityOption;
		selectedFilterOption = dropdownModel.Find((MyDropdownItemModel item) => item.id.Equals(selected));
	}

	private void UpdateDropdownText()
	{
		txtDropdown.text = selectedFilterOption.title;
	}

	private void OnCreateList()
	{
		friendList.StartList();
		friendList.ClearList();
		friendList.Data.InsertItems(0, CreateModel());
	}

	private List<FriendTileModel> CreateModel()
	{
		List<FriendTileModel> list = new List<FriendTileModel>();
		List<RequestData> list2 = Util.SortVector(GameData.instance.PROJECT.character.requests, sortTiles(), Util.ARRAY_DESCENDING);
		for (int i = 0; i < list2.Count; i++)
		{
			RequestData requestData = GameData.instance.PROJECT.character.requests[i];
			string text = ((requestData.characterData.guildInfo == null) ? Util.ParseName(requestData.characterData.name) : Util.ParseName(requestData.characterData.name, requestData.characterData.guildInfo.initials));
			list.Add(new FriendTileModel
			{
				requestData = requestData,
				name = text,
				level = Language.GetString("ui_current_lvl", new string[1] { Util.NumberFormat(requestData.characterData.level) }),
				login = Language.GetString(requestData.characterData.getLoginText()),
				online = requestData.online,
				characterData = requestData.characterData,
				stats = requestData.characterData.getTotalStats(),
				power = requestData.characterData.getTotalStat(0),
				stamina = requestData.characterData.getTotalStat(1),
				agility = requestData.characterData.getTotalStat(2),
				levelVal = requestData.characterData.level,
				nameVal = requestData.characterData.name,
				loginMilliseconds = requestData.characterData.loginMilliseconds
			});
		}
		int options = Util.ARRAY_DESCENDING;
		if (selectedRarityOption == 0 || selectedRarityOption == 6)
		{
			options = Util.ARRAY_ASCENDING;
		}
		return Util.SortVector(list, sortTiles(), options);
	}

	private string[] sortTiles()
	{
		string[] result = new string[3] { "loginMilliseconds", "stats", "levelVal" };
		switch (selectedRarityOption)
		{
		case 0:
			result = new string[3] { "loginMilliseconds", "stats", "levelVal" };
			break;
		case 1:
			result = new string[1] { "levelVal" };
			break;
		case 2:
			result = new string[1] { "stats" };
			break;
		case 3:
			result = new string[1] { "power" };
			break;
		case 4:
			result = new string[1] { "stamina" };
			break;
		case 5:
			result = new string[1] { "agility" };
			break;
		case 6:
			result = new string[1] { "nameVal" };
			break;
		}
		return result;
	}

	public void DestroySomething(GameObject toDestroy)
	{
		Object.Destroy(toDestroy);
	}
}
