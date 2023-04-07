using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.server;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlRoomInviteWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button guildmatesBtn;

	public Button friendsBtn;

	public Button otherBtn;

	public BrawlRoomInviteGuildmatesPanel brawlRoomInviteGuildmatesPanel;

	public BrawlRoomInviteFriendsPanel brawlRoomInviteFriendsPanel;

	public BrawlRoomInviteOtherPanel brawlRoomInviteOtherPanel;

	public const int TAB_FRIENDS = 0;

	public const int TAB_GUILDMATES = 1;

	public const int TAB_OTHER = 2;

	private static List<string> TAB_NAMES = new List<string>();

	private BrawlRoom _room;

	private int _currentTab = -1;

	public int currentTab => _currentTab;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(BrawlRoom room)
	{
		TAB_NAMES.Add("ui_friends");
		TAB_NAMES.Add("ui_guild");
		TAB_NAMES.Add("ui_other");
		_room = room;
		topperTxt.text = Language.GetString("ui_invite");
		guildmatesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_guild");
		friendsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_friends");
		otherBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_other");
		brawlRoomInviteFriendsPanel.LoadDetails(this, _room);
		brawlRoomInviteGuildmatesPanel.LoadDetails(this, _room);
		brawlRoomInviteOtherPanel.LoadDetails(this, _room);
		SetTab(0);
		_room.AddListener(CustomSFSXEvent.CHANGE, OnBrawlChange);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		brawlRoomInviteFriendsPanel.UpdateLayers();
		brawlRoomInviteGuildmatesPanel.UpdateLayers();
	}

	private void OnBrawlChange()
	{
		switch (_currentTab)
		{
		case 0:
			brawlRoomInviteFriendsPanel.CreateList();
			brawlRoomInviteGuildmatesPanel.refreshPending = true;
			break;
		case 1:
			brawlRoomInviteGuildmatesPanel.CreateList();
			brawlRoomInviteFriendsPanel.refreshPending = true;
			break;
		case 2:
			brawlRoomInviteFriendsPanel.refreshPending = true;
			brawlRoomInviteGuildmatesPanel.refreshPending = true;
			break;
		}
	}

	public void OnGuildmatesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	public void OnFriendsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnOtherBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(2);
	}

	private void SetTab(int tab)
	{
		switch (tab)
		{
		case 0:
			_currentTab = 0;
			AlphaTabs();
			Util.SetTab(friendsBtn);
			brawlRoomInviteGuildmatesPanel.Hide();
			brawlRoomInviteFriendsPanel.Show();
			brawlRoomInviteOtherPanel.Hide();
			if (brawlRoomInviteFriendsPanel.refreshPending)
			{
				brawlRoomInviteFriendsPanel.refreshPending = false;
				brawlRoomInviteFriendsPanel.CreateList();
			}
			break;
		case 1:
			_currentTab = 1;
			AlphaTabs();
			Util.SetTab(guildmatesBtn);
			brawlRoomInviteGuildmatesPanel.Show();
			brawlRoomInviteFriendsPanel.Hide();
			brawlRoomInviteOtherPanel.Hide();
			if (brawlRoomInviteGuildmatesPanel.refreshPending)
			{
				brawlRoomInviteGuildmatesPanel.refreshPending = false;
				brawlRoomInviteGuildmatesPanel.CreateList();
			}
			break;
		case 2:
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			_currentTab = 2;
			AlphaTabs();
			Util.SetTab(otherBtn);
			brawlRoomInviteGuildmatesPanel.Hide();
			brawlRoomInviteFriendsPanel.Hide();
			brawlRoomInviteOtherPanel.Show();
			break;
		}
	}

	public void DoInvite(int charID = 0, string name = null)
	{
		GameData.instance.main.ShowLoading();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(12), OnInvite);
		BrawlDALC.instance.doInvite(charID, name);
	}

	private void OnInvite(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(12), OnInvite);
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
			GameData.instance.windowGenerator.NewCharactersSearchListWindow(list, 0, showSelect: true, OnSelectNameFromWindow);
		}
	}

	private void OnSelectNameFromWindow(string name)
	{
		brawlRoomInviteOtherPanel.playerNameTxt.text = name;
	}

	private void AlphaTabs()
	{
		Util.SetTab(friendsBtn, enabled: false);
		Util.SetTab(guildmatesBtn, enabled: false);
		Util.SetTab(otherBtn, enabled: false);
	}

	public override void DoDestroy()
	{
		_room.RemoveListener(CustomSFSXEvent.CHANGE, OnBrawlChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (guildmatesBtn != null)
		{
			guildmatesBtn.interactable = state;
		}
		if (friendsBtn != null)
		{
			friendsBtn.interactable = state;
		}
		if (otherBtn != null)
		{
			otherBtn.interactable = state;
		}
		if (brawlRoomInviteFriendsPanel != null)
		{
			if (state)
			{
				brawlRoomInviteFriendsPanel.DoEnable();
			}
			else
			{
				brawlRoomInviteFriendsPanel.DoDisable();
			}
		}
		if (brawlRoomInviteGuildmatesPanel != null)
		{
			if (state)
			{
				brawlRoomInviteGuildmatesPanel.DoEnable();
			}
			else
			{
				brawlRoomInviteGuildmatesPanel.DoDisable();
			}
		}
		if (brawlRoomInviteOtherPanel != null)
		{
			if (state)
			{
				brawlRoomInviteOtherPanel.DoEnable();
			}
			else
			{
				brawlRoomInviteOtherPanel.DoDisable();
			}
		}
	}
}
