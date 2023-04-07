using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.brawlinvitelist;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlRoomInviteFriendsPanel : MonoBehaviour
{
	public TextMeshProUGUI lvlTxt;

	public Image powerIcon;

	public Image staminaIcon;

	public Image agilityIcon;

	public BrawlInviteList brawlInviteList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private BrawlRoomInviteWindow _inviteWindow;

	private BrawlRoom _room;

	private bool _refreshPending;

	public bool refreshPending
	{
		get
		{
			return _refreshPending;
		}
		set
		{
			_refreshPending = value;
		}
	}

	public void LoadDetails(BrawlRoomInviteWindow inviteWindow, BrawlRoom room)
	{
		_inviteWindow = inviteWindow;
		_room = room;
		GameData.instance.PROJECT.character.AddListener("FRIEND_CHANGE", OnFriendChange);
		lvlTxt.text = Language.GetString("ui_lvl");
		brawlInviteList.InitList(inviteWindow);
		CreateList();
	}

	public void UpdateLayers()
	{
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = _inviteWindow.sortingLayer + brawlInviteList.Content.childCount + 15;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _inviteWindow.sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = _inviteWindow.sortingLayer + brawlInviteList.Content.childCount + 15;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _inviteWindow.sortingLayer;
		foreach (MyListItemViewsHolder visibleItem in brawlInviteList._VisibleItems)
		{
			brawlInviteList.UpdateListItem(visibleItem);
		}
	}

	private void OnFriendChange()
	{
		if (_inviteWindow.currentTab == 0)
		{
			CreateList();
		}
		else
		{
			_refreshPending = true;
		}
	}

	public void CreateList()
	{
		brawlInviteList.ClearList();
		List<UserData> list = new List<UserData>();
		foreach (FriendData friend in GameData.instance.PROJECT.character.friends)
		{
			if (friend.online && !_room.hasPlayer(friend.characterData.charID))
			{
				list.Add(friend);
			}
		}
		if (list.Count <= 0)
		{
			Hide();
			return;
		}
		List<UserData> list2 = Util.SortVector(list, new string[2] { "stats", "level" }, Util.ARRAY_DESCENDING);
		for (int i = 0; i < list2.Count; i++)
		{
			brawlInviteList.Data.InsertOneAtEnd(new BrawlInviteItem
			{
				characterData = list2[i].characterData,
				online = list2[i].online,
				selectText = Language.GetString("ui_invite"),
				inviteWindow = _inviteWindow
			});
		}
		Show();
	}

	private void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("FRIEND_CHANGE", OnFriendChange);
	}

	public void Show()
	{
		if (brawlInviteList.Data.Count <= 0)
		{
			Hide(fromShow: true);
			return;
		}
		brawlInviteList.gameObject.SetActive(value: true);
		lvlTxt.gameObject.SetActive(value: true);
		powerIcon.gameObject.SetActive(value: true);
		staminaIcon.gameObject.SetActive(value: true);
		agilityIcon.gameObject.SetActive(value: true);
	}

	public void Hide(bool fromShow = false)
	{
		if (fromShow)
		{
			brawlInviteList.gameObject.SetActive(value: true);
			return;
		}
		brawlInviteList.gameObject.SetActive(value: false);
		lvlTxt.gameObject.SetActive(value: false);
		powerIcon.gameObject.SetActive(value: false);
		staminaIcon.gameObject.SetActive(value: false);
		agilityIcon.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
	}

	public void DoDisable()
	{
	}
}
