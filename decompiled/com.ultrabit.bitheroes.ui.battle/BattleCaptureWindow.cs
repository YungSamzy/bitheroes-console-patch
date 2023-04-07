using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.chat;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleCaptureWindow : WindowsMain
{
	public TextMeshProUGUI chanceCostTxt;

	public TextMeshProUGUI chanceSuccessTxt;

	public Button chanceBtn;

	public Image chanceGoldIcon;

	public Image chanceCreditsIcon;

	public TextMeshProUGUI guaranteeCostTxt;

	public TextMeshProUGUI guaranteeSuccessTxt;

	public Button guaranteeBtn;

	public Image guaranteeGoldIcon;

	public Image guaranteeCreditsIcon;

	public TextMeshProUGUI captureText;

	public TextMeshProUGUI ownedText;

	public Button viewBtn;

	public Image shine;

	public RectTransform placeholderPosition;

	public Camera overUICameraPrefab;

	private BattleEntity _entity;

	private bool _confirm = true;

	private Camera _entitiesCamera;

	private bool autoGold;

	private bool autoGem;

	private bool goldBtnPressed;

	private bool gemBtnPressed;

	private DialogWindow dialogPurchase;

	private CanvasGroup canvasGroup;

	public BattleEntity entity => _entity;

	private void UpdateParentToOverUI()
	{
		base.gameObject.transform.SetParent(_entitiesCamera.transform.GetChild(0));
		RectTransform component = base.transform.GetComponent<RectTransform>();
		component.sizeDelta = Vector2.zero;
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.position = Vector3.zero;
		component.localPosition = Vector3.zero;
		component.anchoredPosition = Vector3.zero;
		component.localScale = Vector3.one;
		panel.GetComponent<Canvas>().sortingLayerName = "OverUI";
	}

	public override void Start()
	{
		base.Start();
	}

	private void Update()
	{
	}

	public void LoadDetails(BattleEntity entity)
	{
		_entity = entity;
		autoGold = false;
		autoGem = false;
		canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		_entity.SetBars(vis: false);
		_entity.classParent = typeof(BattleCaptureWindow);
		GameData.instance.windowGenerator.CHANGE.AddListener(OnWindowsChange);
		_entitiesCamera = Object.Instantiate(overUICameraPrefab);
		_entitiesCamera.orthographicSize = GameData.instance.main.mainCamera.orthographicSize;
		_entitiesCamera.rect = new Rect(0f, 0f, 1f, 1f);
		OnWindowsChange();
		_entitiesCamera.transform.position = GameData.instance.main.mainCamera.transform.position;
		if (_entity.asset != null)
		{
			SortingGroup component = _entity.asset.GetComponent<SortingGroup>();
			if (_entity.asset.activeInHierarchy && component != null && component.enabled)
			{
				component.sortingLayerID = SortingLayer.NameToID("OverUI");
				component.sortingOrder += base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
			}
		}
		SortingGroup component2 = _entity.overlay.GetComponent<SortingGroup>();
		if (_entity.overlay.gameObject.activeInHierarchy && component2 != null && component2.enabled)
		{
			component2.sortingLayerID = SortingLayer.NameToID("OverUI");
			component2.sortingOrder += base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
		}
		Util.ChangeLayer(entity.transform, "OVERUI");
		captureText.text = Language.GetString("battle_capture_context", new string[1] { entity.captureFamiliarRef.coloredName });
		chanceSuccessTxt.text = Language.GetString("battle_capture_chance", new string[1] { entity.captureFamiliarRef.rarityRef.getCaptureSuccessString() });
		guaranteeSuccessTxt.text = Language.GetString("battle_capture_chance", new string[1] { Util.NumberFormat(100f) }, color: true);
		int num = GameData.instance.PROJECT.character.getItemQty(_entity.captureFamiliarRef) + GameData.instance.PROJECT.character.familiarStable.getFamiliarQty(_entity.captureFamiliarRef);
		ownedText.text = Language.GetString("battle_capture_owned", new string[2]
		{
			Util.colorString(Util.NumberFormat(num), (num > 0) ? "#00FF00" : "#FF0000"),
			ItemRef.GetItemName(6)
		});
		chanceBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_persuade");
		guaranteeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_bribe");
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		float delay = 0.1f / entity.battle.GetSpeed();
		Vector2 vector = new Vector2(0f, 0f);
		new Vector3(placeholderPosition.transform.position.x, placeholderPosition.transform.position.y, GameData.instance.main.mainCamera.transform.position.z);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameData.instance.main.mainCamera.ScreenToWorldPoint(placeholderPosition.transform.position), Vector2.zero);
		if ((bool)raycastHit2D)
		{
			vector.x = raycastHit2D.point.x;
			vector.y = raycastHit2D.point.y;
		}
		float movementDuration = entity.battle.GetMovementDuration(entity.x, 0f, entity.y, -90f, 0.5f);
		AddBG(0.6f);
		canvasGroup.alpha = 0f;
		StartCoroutine(StartAnimation(delay, movementDuration));
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, delay, CoroutineTimer.TYPE.SECONDS, delegate
		{
			entity.PlayAnimation("walk");
		});
		List<object> list = new List<object>();
		list.Add(entity);
		com.ultrabit.bitheroes.model.utility.Tween.StartLocalMovement(entity.gameObject, 0f, -90f, movementDuration, delay, OnEntityStop, list);
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		UpdateCost();
		SetMouse(enabled: false);
		ListenForBack(DoDecline);
		ListenForForward(DoCaptureChance);
		GameData.instance.windowGenerator.ShowCurrencies(show: true);
		CreateWindow(closeWord: true, Language.GetString("ui_decline"), scroll: false);
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("persuade_tubbo");
		if (GameData.instance.PROJECT.character.autoPilot && (!GetPersuadeAvailable() || (GameData.instance.PROJECT.character.tutorial.GetState(18) && GameData.instance.PROJECT.character.tutorial.GetState(19) && (tutorialRef == null || !tutorialRef.areConditionsMet || GameData.instance.PROJECT.character.tutorial.GetState(108)))))
		{
			CheckAutoPersuade();
		}
		if (GameData.instance.windowGenerator.GetDialogByClass(typeof(ConversationWindow)) != null)
		{
			OnWindowsChange();
		}
	}

	private IEnumerator StartAnimation(float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		DoAlpha(1f, duration);
	}

	public void DoAlpha(float finalAlpha, float duration)
	{
		float currentAlpha = canvasGroup.alpha;
		DOTween.To(() => currentAlpha, delegate(float x)
		{
			currentAlpha = x;
		}, finalAlpha, duration).SetEase(Ease.Linear).OnUpdate(delegate
		{
			canvasGroup.alpha = currentAlpha;
		})
			.OnComplete(delegate
			{
				if (finalAlpha == 0f)
				{
					DoDestroy();
				}
			});
	}

	private void OnWindowsChange()
	{
		if (!(GameData.instance.windowGenerator.GetLastDialog() != null))
		{
			return;
		}
		if ((bool)GameData.instance.windowGenerator.GetLastDialog().GetComponent<BattleCaptureWindow>())
		{
			if (GameData.instance.windowGenerator.chatVisible || GameData.instance.windowGenerator.GetDialogByClass(typeof(ConversationWindow)) != null)
			{
				if (_entitiesCamera != null)
				{
					_entitiesCamera.depth = GameData.instance.main.uiCamera.depth - 1f;
				}
			}
			else if (_entitiesCamera != null)
			{
				_entitiesCamera.depth = GameData.instance.main.uiCamera.depth + 1f;
			}
		}
		else if (_entitiesCamera != null)
		{
			_entitiesCamera.depth = GameData.instance.main.uiCamera.depth - 1f;
		}
	}

	public void OnChanceBtn()
	{
		goldBtnPressed = true;
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCaptureChance();
	}

	public void OnGuaranteeBtn()
	{
		gemBtnPressed = true;
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCaptureGuarantee();
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowFamiliar(_entity.captureFamiliarRef.id);
	}

	public override void OnClose()
	{
		DoDecline();
	}

	private void OnEntityStop(List<object> objs)
	{
		(objs[0] as BattleEntity).PlayAnimation("idle");
		SetMouse(enabled: true);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(18) && GetPersuadeAvailable())
		{
			_confirm = false;
			GameData.instance.PROJECT.character.tutorial.SetState(18);
			GameData.instance.tutorialManager.ShowTutorialForButton(chanceBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(18), 4, chanceBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			HideBrideAndChance();
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(19) && GetPersuadeAvailable())
		{
			_confirm = false;
			GameData.instance.PROJECT.character.tutorial.SetState(19);
			GameData.instance.tutorialManager.ShowTutorialForButton(chanceBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(19), 4, chanceBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			HideBrideAndChance();
			return;
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("persuade_tubbo");
		if (tutorialRef != null && tutorialRef.areConditionsMet && !GameData.instance.PROJECT.character.tutorial.GetState(108) && GetPersuadeAvailable())
		{
			_confirm = false;
			GameData.instance.PROJECT.character.tutorial.SetState(108);
			GameData.instance.tutorialManager.ShowTutorialForButton(chanceBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(108), 4, chanceBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			HideBrideAndChance();
		}
	}

	private bool GetPersuadeAvailable()
	{
		ServiceRef captureChanceServiceRef = _entity.captureFamiliarRef.rarityRef.captureChanceServiceRef;
		int num = ((captureChanceServiceRef.costCreditsRaw <= 0) ? 1 : 2);
		int num2 = ((num == 2) ? captureChanceServiceRef.costCredits : captureChanceServiceRef.costGold);
		return ((num == 2) ? GameData.instance.PROJECT.character.credits : GameData.instance.PROJECT.character.gold) >= num2;
	}

	private void OnShopRotationChange()
	{
		UpdateCost();
	}

	private void SetMouse(bool enabled)
	{
		if (enabled)
		{
			Enable();
		}
		else
		{
			Disable();
		}
	}

	private void UpdateCost()
	{
		ServiceRef captureGuaranteeServiceRef = _entity.captureFamiliarRef.rarityRef.captureGuaranteeServiceRef;
		ShopSaleRef itemSaleRef = ShopBook.GetItemSaleRef(captureGuaranteeServiceRef, GameData.instance.PROJECT.character.shopRotationID);
		int num = ((captureGuaranteeServiceRef.costCreditsRaw <= 0) ? 1 : 2);
		int num2 = ((num == 2) ? captureGuaranteeServiceRef.costCredits : captureGuaranteeServiceRef.costGold);
		guaranteeGoldIcon.gameObject.SetActive(num == 1);
		guaranteeCreditsIcon.gameObject.SetActive(num == 2);
		string text = Util.NumberFormat(num2);
		if (itemSaleRef != null)
		{
			text = Util.ParseString("^" + text + "^");
		}
		guaranteeCostTxt.text = text;
		ServiceRef captureChanceServiceRef = _entity.captureFamiliarRef.rarityRef.captureChanceServiceRef;
		ShopSaleRef itemSaleRef2 = ShopBook.GetItemSaleRef(captureChanceServiceRef, GameData.instance.PROJECT.character.shopRotationID);
		int num3 = ((captureChanceServiceRef.costCreditsRaw <= 0) ? 1 : 2);
		int num4 = ((num3 == 2) ? captureChanceServiceRef.costCredits : captureChanceServiceRef.costGold);
		chanceGoldIcon.gameObject.SetActive(num3 == 1);
		chanceCreditsIcon.gameObject.SetActive(num3 == 2);
		string text2 = Util.NumberFormat(num4);
		if (itemSaleRef2 != null)
		{
			text2 = Util.ParseString("^" + text2 + "^");
		}
		chanceCostTxt.text = text2;
	}

	private void DoCaptureChance()
	{
		DoCapturePurchase(_entity.captureFamiliarRef.rarityRef.captureChanceServiceRef, guarantee: false);
	}

	private void DoCaptureGuarantee()
	{
		DoCapturePurchase(_entity.captureFamiliarRef.rarityRef.captureGuaranteeServiceRef, guarantee: true);
	}

	private void DoCapturePurchase(ServiceRef serviceRef, bool guarantee)
	{
		if (_confirm)
		{
			string coloredName = _entity.captureFamiliarRef.coloredName;
			int currencyID = ((!guarantee) ? 1 : 2);
			CurrencyRef currencyRef = CurrencyBook.Lookup(currencyID);
			int currencyCost = serviceRef.getCost(currencyID);
			string @string = Language.GetString(guarantee ? "purchase_bribe_confirm" : "purchase_persuade_confirm", new string[3]
			{
				coloredName,
				Util.NumberFormat(currencyCost),
				currencyRef.name
			});
			dialogPurchase = TransactionManager.instance.ConfirmItemPurchase(serviceRef, "Battle", 1, @string, delegate
			{
				OnCapturePurchaseConfirm(serviceRef, currencyID, currencyCost);
			});
			if (autoGold)
			{
				GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 500f, onTimerCompleteCapturePurchaseConfirm);
			}
			if (autoGem)
			{
				GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 500f, onTimerCompleteCapturePurchaseGemsConfirm);
			}
		}
		else
		{
			ServiceRef captureChanceServiceRef = _entity.captureFamiliarRef.rarityRef.captureChanceServiceRef;
			int num = ((captureChanceServiceRef.costCreditsRaw <= 0) ? 1 : 2);
			int currencyCost2 = ((num == 2) ? captureChanceServiceRef.costCredits : captureChanceServiceRef.costGold);
			DoCaptureAccept(captureChanceServiceRef, num, currencyCost2);
		}
	}

	private void onTimerCompleteCapturePurchaseGemsConfirm()
	{
		dialogPurchase.OnClose();
		ServiceRef captureGuaranteeServiceRef = _entity.captureFamiliarRef.rarityRef.captureGuaranteeServiceRef;
		int num = 2;
		CurrencyBook.Lookup(num);
		int cost = captureGuaranteeServiceRef.getCost(num);
		DoCaptureAccept(captureGuaranteeServiceRef, num, cost);
	}

	private void onTimerCompleteCapturePurchaseConfirm()
	{
		dialogPurchase.OnClose();
		ServiceRef captureChanceServiceRef = _entity.captureFamiliarRef.rarityRef.captureChanceServiceRef;
		int num = 1;
		CurrencyBook.Lookup(num);
		int cost = captureChanceServiceRef.getCost(num);
		DoCaptureAccept(captureChanceServiceRef, num, cost);
	}

	private void OnCapturePurchaseConfirm(ServiceRef serviceRef, int currencyID, int currencyCost)
	{
		DoCaptureAccept(serviceRef, currencyID, currencyCost);
	}

	private void DoCaptureAccept(ServiceRef serviceRef, int currencyID, int currencyCost)
	{
		Disable();
		GameData.instance.main.ShowLoading();
		BattleDALC.instance.doCaptureAccept(_entity, serviceRef, currencyID, currencyCost);
		BattleDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnCaptureAccept);
	}

	private void OnCaptureAccept(BaseEvent e)
	{
		Enable();
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		BattleDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnCaptureAccept);
		SFSObject sfsob = obj.sfsob;
		KongregateAnalytics.checkEconomyTransaction("Battle Capture", null, null, sfsob, "Battle", 1, currencyUpdate: false);
		GameData.instance.PROJECT.character.checkCurrencyChanges(sfsob, update: true);
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.PROJECT.CheckTutorialChanges();
		GameData.instance.audioManager.PlaySoundLink("purchase");
		DoDestroy();
	}

	private void DoDecline()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("battle_capture_decline", new string[1] { ItemRef.GetItemName(6) }), null, null, delegate
		{
			GameData.instance.audioManager.PlaySoundLink("familiardecline");
			BattleDALC.instance.doCaptureDecline(_entity);
			_entity.Remove();
			DoDestroy();
		});
	}

	private void HideBrideAndChance()
	{
		Util.SetButton(guaranteeBtn, enabled: false);
		chanceSuccessTxt.gameObject.SetActive(value: false);
		guaranteeSuccessTxt.gameObject.SetActive(value: false);
	}

	public override void DoDestroy()
	{
		_entity.classParent = null;
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_ID_CHANGE", OnShopRotationChange);
		if (_entity.asset != null)
		{
			SortingGroup component = _entity.asset.GetComponent<SortingGroup>();
			if (_entity.asset.activeInHierarchy && component != null && component.enabled)
			{
				component.sortingLayerID = SortingLayer.NameToID("Default");
				component.sortingOrder -= base.layer * 1000 + GameData.instance.windowGenerator.dialogCount * 100;
			}
		}
		SortingGroup component2 = _entity.overlay.GetComponent<SortingGroup>();
		if (_entity.overlay.isActiveAndEnabled && component2 != null && component2.enabled)
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
		Util.ChangeLayer(entity.transform, "Default");
		Object.Destroy(_entitiesCamera.gameObject);
		GameData.instance.windowGenerator.CHANGE.RemoveListener(OnWindowsChange);
		GameData.instance.windowGenerator.ShowCurrencies(show: false);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		chanceBtn.interactable = true;
		guaranteeBtn.interactable = true;
		viewBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		chanceBtn.interactable = false;
		guaranteeBtn.interactable = false;
		viewBtn.interactable = false;
	}

	private void CheckAutoPersuade()
	{
		int num = 500;
		if (GameData.instance.SAVE_STATE.autopersuadeFamiliarsGold)
		{
			ServiceRef captureChanceServiceRef = _entity.captureFamiliarRef.rarityRef.captureChanceServiceRef;
			if (GameData.instance.PROJECT.character.gold >= captureChanceServiceRef.costGold && GameData.instance.SAVE_STATE.GetAutopersuadeGoldFamiliarRarity(GameData.instance.PROJECT.character.id, _entity.captureFamiliarRef.rarityRef, GameData.instance.SAVE_STATE.GetAutopersuadeFamiliarsGoldRarities(GameData.instance.PROJECT.character.id)))
			{
				GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, num, onTimerCompleteAutoPersuadeTimer);
				autoGold = true;
				return;
			}
		}
		if (GameData.instance.SAVE_STATE.autopersuadeFamiliarsGems)
		{
			ServiceRef captureGuaranteeServiceRef = _entity.captureFamiliarRef.rarityRef.captureGuaranteeServiceRef;
			if (GameData.instance.PROJECT.character.credits >= captureGuaranteeServiceRef.costCredits && GameData.instance.SAVE_STATE.GetAutopersuadeGemsFamiliarRarity(GameData.instance.PROJECT.character.id, _entity.captureFamiliarRef.rarityRef, GameData.instance.SAVE_STATE.GetAutopersuadeFamiliarsGemsRarities(GameData.instance.PROJECT.character.id)))
			{
				GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, num, onTimerCompleteAutoPersuadeGemsTimer);
				autoGem = true;
			}
		}
	}

	private void onTimerCompleteAutoPersuadeGemsTimer()
	{
		bool guarantee = true;
		if (autoGold)
		{
			guarantee = false;
		}
		if (!goldBtnPressed && !gemBtnPressed)
		{
			DoCapturePurchase(_entity.captureFamiliarRef.rarityRef.captureGuaranteeServiceRef, guarantee);
		}
	}

	private void onTimerCompleteAutoPersuadeTimer()
	{
		bool guarantee = true;
		if (autoGold)
		{
			guarantee = false;
		}
		if (!goldBtnPressed && !gemBtnPressed)
		{
			DoCapturePurchase(_entity.captureFamiliarRef.rarityRef.captureChanceServiceRef, guarantee);
		}
	}
}
