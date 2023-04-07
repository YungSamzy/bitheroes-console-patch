using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.brawlsearchlist;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlWindow : WindowsMain
{
	public const int TILES_PER_ROW = 5;

	public TextMeshProUGUI topperTxt;

	public Button helpBtn;

	public Button refreshBtn;

	public Button filterBtn;

	public Button createBtn;

	public BrawlSearchList brawlSearchList;

	public Image loadingIcon;

	public CurrencyBarFill currencyBarFill;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private IEnumerator _refreshTimer;

	private int seconds = 3;

	private new int _sortingLayer;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_brawl");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		filterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_filter");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_summon");
		GameData.instance.PROJECT.character.lastBrawlPrivateCheckbox = null;
		brawlSearchList.InitList(this);
		DoSearch();
		currencyBarFill.Init();
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		if (brawlSearchList.Data != null)
		{
			topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
			topMask.frontSortingOrder = 1 + _sortingLayer + brawlSearchList.GetItemsCount() * 5;
			topMask.backSortingLayerID = SortingLayer.NameToID("UI");
			topMask.backSortingOrder = _sortingLayer;
			bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
			bottomMask.frontSortingOrder = 1 + _sortingLayer + brawlSearchList.GetItemsCount() * 5;
			bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
			bottomMask.backSortingOrder = _sortingLayer;
			brawlSearchList.Refresh();
		}
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("ui_brawl"), Language.GetString("brawl_help_desc"), base.gameObject);
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoSearch();
	}

	public void OnFilterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewBrawlFilterWindow(base.gameObject);
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewBrawlCreateWindow(base.gameObject);
	}

	private void RestartRefreshTimer()
	{
		if (_refreshTimer != null)
		{
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		seconds = 3;
		_refreshTimer = OnRefreshTimer();
		StartCoroutine(_refreshTimer);
		Util.SetButton(refreshBtn, enabled: false);
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
	}

	private IEnumerator OnRefreshTimer()
	{
		yield return new WaitForSeconds(1f);
		seconds--;
		if (seconds <= 0)
		{
			Util.SetButton(refreshBtn);
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = "";
			StopCoroutine(_refreshTimer);
			_refreshTimer = null;
		}
		else
		{
			refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(seconds);
			_refreshTimer = OnRefreshTimer();
			StartCoroutine(_refreshTimer);
		}
	}

	public void DoSearch()
	{
		loadingIcon.gameObject.SetActive(value: true);
		brawlSearchList.ClearList();
		RestartRefreshTimer();
		BrawlDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnSearch);
		BrawlDALC.instance.doSearch(GameData.instance.SAVE_STATE.GetBrawlFilter(GameData.instance.PROJECT.character.id), GameData.instance.SAVE_STATE.GetBrawlTierFilter(GameData.instance.PROJECT.character.id), GameData.instance.SAVE_STATE.GetBrawlDifficultyFilter(GameData.instance.PROJECT.character.id));
	}

	private void OnSearch(BaseEvent e)
	{
		if (loadingIcon != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		DALCEvent obj = e as DALCEvent;
		BrawlDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnSearch);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<BrawlRoom> rooms = BrawlRoom.listFromSFSObject(sfsob);
		CreateList(rooms);
	}

	public void CreateList(List<BrawlRoom> rooms)
	{
		if (!brawlSearchList.IsInitialized)
		{
			StartCoroutine(WaitAndCreate(rooms, 0.1f));
			return;
		}
		brawlSearchList.ClearList();
		List<BrawlRoom> list = Util.SortVector(rooms, new string[3] { "tierID", "difficultyID", "index" }, Util.ARRAY_DESCENDING);
		for (int i = 0; i < list.Count; i++)
		{
			brawlSearchList.Data.InsertOneAtEnd(new BrawlSearchItem
			{
				brawlRoom = list[i]
			});
		}
		UpdateSortingLayers(_sortingLayer);
	}

	private IEnumerator WaitAndCreate(List<BrawlRoom> rooms, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		CreateList(rooms);
	}

	public void OnXealsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowServiceType(15);
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
		if (helpBtn != null && helpBtn.gameObject != null)
		{
			helpBtn.interactable = state;
		}
		if (refreshBtn != null && refreshBtn.gameObject != null)
		{
			refreshBtn.interactable = state;
		}
		if (filterBtn != null && filterBtn.gameObject != null)
		{
			filterBtn.interactable = state;
		}
		if (createBtn != null && createBtn.gameObject != null)
		{
			createBtn.interactable = state;
		}
	}
}
