using System.Collections.Generic;
using System.Linq;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.MultiplePrefabsFriendList;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.friend;

public class FriendWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI friendsTxt;

	public Button inviteBtn;

	public Button findBtn;

	public MultiplePrefabsFriendList friendList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private new int _sortingLayer;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_friends");
		inviteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invite");
		findBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_suggest");
		UpdateText();
		friendList.StartList();
		OnCreateList();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		GameData.instance.PROJECT.PauseDungeon();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && GameData.instance.PROJECT.character.tutorial.GetState(127) && !GameData.instance.PROJECT.character.tutorial.GetState(128))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(128);
			GameData.instance.tutorialManager.ShowTutorialForButton(findBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(128), 4, findBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
		}
	}

	public override void UpdateSortingLayers(int layer)
	{
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

	private void OnCreateList()
	{
		double virtualAbstractNormalizedScrollPosition = friendList.GetVirtualAbstractNormalizedScrollPosition();
		friendList.ClearList();
		friendList.Data.InsertItems(0, CreateModel());
		friendList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	public void OnInviteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFriendInviteWindow();
	}

	public void OnFindBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFriendRecommendWindow(base.gameObject);
	}

	private void UpdateText()
	{
		int num = (from x in GameData.instance.PROJECT.character.friends
			group x by x.characterData.playerID).Count();
		friendsTxt.text = Util.NumberFormat(num) + "/" + Util.NumberFormat(VariableBook.friendsMax);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (inviteBtn != null)
		{
			inviteBtn.interactable = true;
		}
		if (findBtn != null)
		{
			findBtn.interactable = true;
		}
		GameData.instance.PROJECT.character.AddListener("FRIEND_CHANGE", OnFriendChange);
		GameData.instance.PROJECT.character.AddListener("REQUEST_CHANGE", OnRequestChange);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (inviteBtn != null)
		{
			inviteBtn.interactable = false;
		}
		if (findBtn != null)
		{
			findBtn.interactable = false;
		}
		GameData.instance.PROJECT.character.RemoveListener("FRIEND_CHANGE", OnFriendChange);
		GameData.instance.PROJECT.character.RemoveListener("REQUEST_CHANGE", OnRequestChange);
	}

	private void OnFriendChange()
	{
		UpdateText();
		OnCreateList();
	}

	private void OnRequestChange()
	{
		UpdateText();
		OnCreateList();
	}

	public void DestroySomething(GameObject toDestroy)
	{
		Object.Destroy(toDestroy);
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	private List<BaseModel> CreateModel()
	{
		List<BaseModel> list = new List<BaseModel>();
		if (GameData.instance.PROJECT.character.requests.Count > 0)
		{
			list.Add(new FriendRequestViewTileModel
			{
				FriendRequestCount = GameData.instance.PROJECT.character.requests.Count.ToString()
			});
		}
		List<FriendData> list2 = Util.SortVector(GameData.instance.PROJECT.character.friends, new string[2] { "offline", "loginMilliseconds" }, Util.ARRAY_ASCENDING);
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(new FriendTileModel
			{
				name = list2[i].characterData.nameplateWithGuild,
				level = Language.GetString("ui_current_lvl", new string[1] { Util.NumberFormat(list2[i].characterData.level) }),
				login = Language.GetString(list2[i].getLoginText()),
				online = list2[i].online,
				id = list2[i].characterData.charID,
				characterData = list2[i].characterData
			});
		}
		return list;
	}
}
