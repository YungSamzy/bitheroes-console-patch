using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.friendrecommendlist;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.friend;

public class FriendRecommendWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI lvlTxt;

	public Image loadingIcon;

	public Button refreshBtn;

	public FriendRecommendList friendRecommendList;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private new int _sortingLayer;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = Language.GetString("ui_players");
		lvlTxt.text = Language.GetString("ui_lvl");
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		friendRecommendList.InitList(this);
		DoRefreshList();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		friendRecommendList.Refresh();
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 200 + sortingLayer + friendRecommendList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 200 + sortingLayer + friendRecommendList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _sortingLayer;
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || !GameData.instance.PROJECT.character.tutorial.GetState(128) || GameData.instance.PROJECT.character.tutorial.GetState(129))
		{
			return;
		}
		if (friendRecommendList.Data.Count > 0)
		{
			MyListItemViewsHolder itemViewsHolderIfVisible = friendRecommendList.GetItemViewsHolderIfVisible(0);
			if (itemViewsHolderIfVisible != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(129);
				if (itemViewsHolderIfVisible.UIOnlineSelect.gameObject.activeSelf)
				{
					GameData.instance.tutorialManager.ShowTutorialForButton(itemViewsHolderIfVisible.UIOnlineSelect.gameObject, new TutorialPopUpSettings(Tutorial.GetText(129), 4, itemViewsHolderIfVisible.UIOnlineSelect.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false);
				}
				else
				{
					GameData.instance.tutorialManager.ShowTutorialForButton(itemViewsHolderIfVisible.UIOfflineSelect.gameObject, new TutorialPopUpSettings(Tutorial.GetText(129), 4, itemViewsHolderIfVisible.UIOfflineSelect.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false);
				}
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
		else
		{
			Invoke("CheckTutorial", 0.5f);
		}
	}

	public void DoRefreshList()
	{
		friendRecommendList.ClearList();
		Util.SetButton(refreshBtn, enabled: false);
		loadingIcon.gameObject.SetActive(value: true);
		FriendDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnRefreshList);
		FriendDALC.instance.doFriendRecommend();
	}

	private void OnRefreshList(BaseEvent e)
	{
		Util.SetButton(refreshBtn);
		if (loadingIcon != null && loadingIcon.gameObject != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		DALCEvent obj = e as DALCEvent;
		FriendDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnRefreshList);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("cha13");
		List<RecommendData> list = new List<RecommendData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(RecommendData.fromSFSObject(sFSObject));
		}
		foreach (RecommendData item in list)
		{
			friendRecommendList.Data.InsertOneAtEnd(new FriendRecommendItem
			{
				characterData = item.characterData,
				online = item.online,
				selectText = Language.GetString("ui_add")
			});
		}
		UpdateSortingLayers(_sortingLayer);
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoRefreshList();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
	}
}
