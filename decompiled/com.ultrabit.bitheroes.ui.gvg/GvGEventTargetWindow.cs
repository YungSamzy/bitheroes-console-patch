using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.ui.lists.gvgeventtargetlist;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gvg;

public class GvGEventTargetWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI pointsListTxt;

	public Button statsBtn;

	public Button fleeBtn;

	private GvGEventRef _eventRef;

	private List<EventTargetData> targets;

	private GvGEventTargetList eventTargetList;

	private EventTargetData eventTargetData;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private bool _opponentSelected;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GvGEventRef eventRef, List<EventTargetData> targets)
	{
		_eventRef = eventRef;
		this.targets = targets;
		topperTxt.text = Language.GetString("ui_select_opponent");
		nameListTxt.text = Language.GetString("ui_name");
		pointsListTxt.text = Language.GetString("ui_points");
		fleeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_flee");
		statsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_stats");
		ListenForBack(OnFleeBtn);
		SCROLL_IN_START.AddListener(ResetOpponentSelected);
		eventTargetList = GetComponentInChildren<GvGEventTargetList>();
		eventTargetList.InitList(OnSelectedOpponent, this);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = 1 + base.sortingLayer + eventTargetList.Content.childCount;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = base.sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = 1 + base.sortingLayer + eventTargetList.Content.childCount;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = base.sortingLayer;
	}

	private void OnSelectedOpponent(EventTargetData eventTargetData)
	{
		if (!_opponentSelected)
		{
			_opponentSelected = true;
			this.eventTargetData = eventTargetData;
			if (_eventRef.teamRules.slots <= 1)
			{
				new List<TeammateData>().Add(new TeammateData(GameData.instance.PROJECT.character.id, 1, -1L));
			}
		}
	}

	public void OnFleeBtn()
	{
		GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_YES_NO, Language.GetString("ui_flee"), Language.GetString("ui_flee_confirm", new string[1] { CurrencyRef.GetCurrencyName(_eventRef.getCurrencyID()) }), base.gameObject);
	}

	public void DialogYesReciever()
	{
		base.OnClose();
	}

	private void ResetOpponentSelected(object e)
	{
		_opponentSelected = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_START.RemoveListener(ResetOpponentSelected);
		base.DoDestroy();
	}
}
