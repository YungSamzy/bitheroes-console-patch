using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleEntitySelectWindow : WindowsMain
{
	public TextMeshProUGUI selectTxt;

	public Image selectBG;

	public Button cancelBtn;

	public Camera overUICameraPrefab;

	private List<BattleEntity> _entities;

	private bool _drag;

	private object _data;

	private bool _tutorial;

	private BattleEntity _selectedEntity;

	private List<Transform> _parents;

	private Camera _entitiesCamera;

	private Dictionary<int, int> entitiesPositions = new Dictionary<int, int>();

	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public UnityCustomEvent CLOSE = new UnityCustomEvent();

	public BattleEntity selectedEntity => _selectedEntity;

	public object data => _data;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(List<BattleEntity> entities, string text, bool drag = false, object data = null, bool tutorial = false)
	{
		Disable();
		_entities = entities;
		_drag = drag;
		_data = data;
		_tutorial = tutorial;
		selectTxt.text = text;
		StartCoroutine(FixText());
		GameData.instance.windowGenerator.CHANGE.AddListener(OnWindowsChange);
		_entitiesCamera = Object.Instantiate(overUICameraPrefab);
		_entitiesCamera.orthographicSize = GameData.instance.main.mainCamera.orthographicSize;
		_entitiesCamera.depth = GameData.instance.main.uiCamera.depth + 1f;
		_entitiesCamera.transform.position = GameData.instance.main.mainCamera.transform.position;
		GameData.instance.main.entitiesCamera = _entitiesCamera;
		int num = 1;
		_parents = new List<Transform>();
		foreach (BattleEntity entity in _entities)
		{
			_parents.Add(entity.transform.parent);
			entity.SetClickable(!drag, drag);
			if (!drag)
			{
				entity.SELECT.AddListener(OnEntitySelected);
			}
			entity.classParent = typeof(BattleEntitySelectWindow);
			if (entity.asset != null)
			{
				SortingGroup component = entity.asset.GetComponent<SortingGroup>();
				if (entity.asset.activeInHierarchy && component != null && component.enabled)
				{
					component.sortingLayerID = SortingLayer.NameToID("UI");
					component.sortingOrder += base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
					entity.highlightID = num;
					entitiesPositions.Add(entity.highlightID, component.sortingOrder);
					num++;
				}
			}
			SortingGroup component2 = entity.overlay.GetComponent<SortingGroup>();
			if (entity.overlay.gameObject.activeInHierarchy && component2 != null && component2.enabled)
			{
				component2.sortingLayerID = SortingLayer.NameToID("UI");
				component2.sortingOrder += base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
			}
			Util.ChangeLayer(entity.transform, "OVERUI");
		}
		cancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cancel");
		if (_tutorial)
		{
			CheckTutorial();
		}
		ListenForBack(DoDestroy);
		closeBtn.transform.position = GameData.instance.PROJECT.battle.battleUI.closeBtn.transform.position;
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	private IEnumerator FixText()
	{
		yield return new WaitForEndOfFrame();
		selectTxt.ForceMeshUpdate();
		selectBG.rectTransform.sizeDelta = new Vector2(selectTxt.rectTransform.sizeDelta.x + 33f, selectBG.rectTransform.sizeDelta.y);
	}

	public int GetPositionByID(int id)
	{
		return entitiesPositions[id];
	}

	public int GetHighlightPosition()
	{
		int num = 0;
		foreach (BattleEntity entity in _entities)
		{
			if (entity.asset != null)
			{
				SortingGroup component = entity.asset.GetComponent<SortingGroup>();
				if (entity.asset.activeInHierarchy && component != null && component.enabled && component.sortingOrder > num)
				{
					num = component.sortingOrder;
				}
			}
		}
		return num + 10;
	}

	private void OnWindowsChange()
	{
		if (!(GameData.instance.windowGenerator.GetLastDialog() != null))
		{
			return;
		}
		if ((bool)GameData.instance.windowGenerator.GetLastDialog().GetComponent<BattleEntitySelectWindow>())
		{
			if (_entitiesCamera != null)
			{
				_entitiesCamera.depth = GameData.instance.main.uiCamera.depth + 1f;
			}
		}
		else if (_entitiesCamera != null)
		{
			_entitiesCamera.depth = GameData.instance.main.uiCamera.depth - 1f;
		}
	}

	public void OnCancelBtn()
	{
		OnClose();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager.hasPopup || !base.gameObject.activeSelf || GameData.instance.PROJECT.character.tutorial.GetState(44) || _entities.Count <= 0)
		{
			return;
		}
		BattleEntity battleEntity = _entities[0];
		if (battleEntity != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(44);
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(battleEntity.gameObject, new TutorialPopUpSettings(Tutorial.GetText(44), 4, battleEntity.gameObject), null, stageTrigger: true, EventTriggerType.PointerDown, funcSameAsTargetFunc: false, delegate
			{
				GameData.instance.tutorialManager.ClearTutorial();
			}, shadow: false);
		}
	}

	public void DoSelectIndex(int index)
	{
		List<BattleEntity> list = Util.SortVector(_entities, new string[1] { "x" }, Util.ARRAY_ASCENDING);
		for (int i = 0; i < list.Count; i++)
		{
			BattleEntity entity = list[i];
			if (i == index)
			{
				DoEntitySelect(entity);
				break;
			}
		}
	}

	private void OnEntitySelected(object e)
	{
		BattleEntity entity = e as BattleEntity;
		DoEntitySelect(entity);
	}

	private void DoEntitySelect(BattleEntity entity)
	{
		_selectedEntity = entity;
		SELECT.Invoke(this);
	}

	public override void OnClose()
	{
		CLOSE.Invoke(this);
		Disable();
		DoDestroy();
	}

	public override void DoDestroy()
	{
		for (int i = 0; i < _entities.Count; i++)
		{
			BattleEntity battleEntity = _entities[i];
			_ = _parents[i];
			battleEntity.SetClickable(clickable: false, draggable: false);
			if (!_drag)
			{
				battleEntity.SELECT.RemoveListener(OnEntitySelected);
			}
			battleEntity.classParent = null;
			if (battleEntity.asset != null)
			{
				SortingGroup component = battleEntity.asset.GetComponent<SortingGroup>();
				if (component != null)
				{
					component.sortingLayerID = SortingLayer.NameToID("Default");
					component.sortingOrder -= base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
				}
			}
			SortingGroup component2 = battleEntity.overlay.GetComponent<SortingGroup>();
			if (battleEntity.overlay.gameObject.activeInHierarchy && component2 != null && component2.enabled)
			{
				if (GameData.instance.SAVE_STATE.battleBarOverlay)
				{
					component2.sortingLayerID = SortingLayer.NameToID("Overall");
				}
				else
				{
					component2.sortingLayerID = SortingLayer.NameToID("Default");
				}
				component2.sortingOrder -= base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
			}
			Util.ChangeLayer(battleEntity.transform, "Mouse");
		}
		Object.Destroy(_entitiesCamera.gameObject);
		if (GameData.instance.tutorialManager != null)
		{
			GameData.instance.tutorialManager.ClearTutorial();
		}
		GameData.instance.windowGenerator.CHANGE.RemoveListener(OnWindowsChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		cancelBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		cancelBtn.interactable = false;
	}
}
