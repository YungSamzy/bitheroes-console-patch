using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.leaderboardlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.leaderboard;

public class LeaderboardWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI rankListTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI valueTxt;

	public Image filterDropdown;

	public Image loadingIcon;

	public LeaderboardList leaderboardList;

	private LeaderboardRef currentLeaderboardRef;

	private Transform dropdownWindow;

	private TrophyHandler trophyHandler;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_leaderboards");
		rankListTxt.text = Language.GetString("ui_rank");
		nameListTxt.text = Language.GetString("ui_name");
		trophyHandler = base.gameObject.AddComponent<TrophyHandler>();
		leaderboardList.trophyHandler = trophyHandler;
		for (int i = 0; i < LeaderboardBook.size; i++)
		{
			LeaderboardRef leaderboardRef = LeaderboardBook.Lookup(i);
			if (leaderboardRef != null)
			{
				currentLeaderboardRef = leaderboardRef;
				break;
			}
		}
		filterDropdown.GetComponentInChildren<TextMeshProUGUI>().text = currentLeaderboardRef.name;
		LeaderboardDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnGetList);
		DoGetList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void DoGetList()
	{
		LeaderboardRef leaderboardRef = currentLeaderboardRef;
		if (leaderboardRef == null)
		{
			loadingIcon.gameObject.SetActive(value: false);
			return;
		}
		valueTxt.text = leaderboardRef.value;
		loadingIcon.gameObject.SetActive(value: true);
		leaderboardList.ClearList();
		LeaderboardDALC.instance.doGetList(leaderboardRef.id);
	}

	private void OnGetList(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		LeaderboardRef leaderboardRef = currentLeaderboardRef;
		if (leaderboardRef != null)
		{
			int @int = sfsob.GetInt("lea0");
			if (leaderboardRef.id == @int)
			{
				List<LeaderboardData> list = LeaderboardData.listFromSFSObject(sfsob);
				loadingIcon.gameObject.SetActive(value: false);
				CreateList(list);
			}
		}
	}

	private void CreateList(List<LeaderboardData> list)
	{
		foreach (LeaderboardData item in list)
		{
			leaderboardList.Data.InsertOneAtEnd(new LeaderboardItem
			{
				data = item
			});
		}
	}

	public void OnFilterDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_view"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.ClearList();
		componentInChildren.StartList(base.gameObject, currentLeaderboardRef.id, OnFilterDropdownChange);
		for (int i = 0; i < LeaderboardBook.size; i++)
		{
			LeaderboardRef leaderboardRef = LeaderboardBook.Lookup(i);
			if (leaderboardRef != null)
			{
				componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
				{
					id = i,
					title = leaderboardRef.name,
					btnHelp = false,
					data = leaderboardRef
				});
			}
		}
	}

	private void OnFilterDropdownChange(MyDropdownItemModel model)
	{
		Debug.Log($"OnFilterDropdownChange {model.data}");
		currentLeaderboardRef = model.data as LeaderboardRef;
		Debug.Log($"OnFilterDropdownChange {currentLeaderboardRef.id}");
		filterDropdown.GetComponentInChildren<TextMeshProUGUI>().text = currentLeaderboardRef.name;
		DoGetList();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public override void DoDestroy()
	{
		LeaderboardDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnGetList);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		filterDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		filterDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
