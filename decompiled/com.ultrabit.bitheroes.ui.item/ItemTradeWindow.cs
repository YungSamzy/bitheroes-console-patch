using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.tradelist;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemTradeWindow : WindowsMain
{
	private const int REPEAT_DELAY_MAX = 150;

	private const int REPEAT_DELAY_MIN = 5;

	private const int REPEAT_DELAY_CHANGE = 5;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI itemNameTxt;

	public TextMeshProUGUI tradeTxt;

	public TextMeshProUGUI qtyTxt;

	public Button minBtn;

	public Button minusBtn;

	public Button plusBtn;

	public Button maxBtn;

	public Button tradeBtn;

	public GameObject mouseLock;

	public ItemIcon itemIcon;

	public RectTransform requiredPlaceholder;

	public Transform itemCraftTilePrefab;

	private TradeItem _tradeItem;

	private TradeItem _currentModel;

	private int _qtyMax = 1;

	private int _qtyMin = 1;

	private int _qty = 1;

	private ItemCraftTile[] requiredMaterials;

	private Coroutine _repeatTimer;

	private bool _repeatIncreased;

	private bool _repeatAdded;

	private int _repeatStat;

	private int _repeatDelay = 150;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private bool tutorial;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(TradeItem tradeItem)
	{
		_tradeItem = tradeItem;
		_qtyMax = CalcMaxAmount(tradeItem);
		topperTxt.text = Language.GetString("ui_craft");
		itemNameTxt.text = tradeItem.tradeRef.resultItem.itemRef.coloredName;
		tradeTxt.text = Language.GetString("ui_trade_desc");
		itemIcon.isThumbnail = true;
		itemIcon.SetItemData(tradeItem.tradeRef.resultItem, locked: false, tradeItem.tradeRef.resultItem.qty * _qty);
		int itemActionType = ((tradeItem.tradeRef.resultItem.itemRef.itemType == 6 || (tradeItem.tradeRef.resultItem.itemRef.isViewable() && tradeItem.tradeRef.resultItem.itemRef.hasContents())) ? 7 : 0);
		itemIcon.SetItemActionType(itemActionType);
		minBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMin);
		minusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		plusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		maxBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMax);
		tradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_craft");
		ListenForBack(OnClose);
		ListenForForward(OnTradeBtn);
		requiredMaterials = new ItemCraftTile[tradeItem.tradeRef.requiredItems.Count];
		int num = 0;
		foreach (ItemData requiredItem in tradeItem.tradeRef.requiredItems)
		{
			if (requiredItem != null)
			{
				Transform transform = Object.Instantiate(itemCraftTilePrefab);
				transform.SetParent(requiredPlaceholder, worldPositionStays: false);
				requiredMaterials[num] = transform.GetComponent<ItemCraftTile>();
				requiredMaterials[num].LoadDetails(requiredItem);
				num++;
			}
		}
		SetQty(1);
		mouseLock.transform.SetAsFirstSibling();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null))
		{
			if (GameData.instance.PROJECT.character.tutorial.GetState(70) && !GameData.instance.PROJECT.character.tutorial.GetState(71))
			{
				tutorial = true;
				GameData.instance.PROJECT.character.tutorial.SetState(71);
				GameData.instance.tutorialManager.ShowTutorialForButton(tradeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(71), 3, tradeBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
			if (GameData.instance.PROJECT.character.tutorial.GetState(87) && !GameData.instance.PROJECT.character.tutorial.GetState(88))
			{
				tutorial = true;
				GameData.instance.PROJECT.character.tutorial.SetState(88);
				GameData.instance.tutorialManager.ShowTutorialForButton(tradeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(88), 3, tradeBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
		}
	}

	private void DoUpdate()
	{
		itemIcon.SetItemData(_tradeItem.tradeRef.resultItem, locked: false, _tradeItem.tradeRef.resultItem.qty * _qty);
		int itemActionType = ((_tradeItem.tradeRef.resultItem.itemRef.itemType == 6 || (_tradeItem.tradeRef.resultItem.itemRef.isViewable() && _tradeItem.tradeRef.resultItem.itemRef.hasContents())) ? 7 : 0);
		itemIcon.SetItemActionType(itemActionType);
		int num = 0;
		foreach (ItemData requiredItem in _tradeItem.tradeRef.requiredItems)
		{
			if (requiredItem != null)
			{
				requiredMaterials[num].LoadDetails(requiredItem, locked: false, _qty);
				num++;
			}
		}
	}

	private void SetQty(int qty)
	{
		if (qty < _qtyMin)
		{
			qty = _qtyMin;
		}
		if (qty > _qtyMax)
		{
			qty = _qtyMax;
		}
		_qty = qty;
		qtyTxt.text = Util.NumberFormat(_qty);
		if (_qty <= _qtyMin)
		{
			Util.SetButton(minBtn, enabled: false);
			Util.SetButton(minusBtn, enabled: false);
		}
		else
		{
			Util.SetButton(minBtn);
			Util.SetButton(minusBtn);
		}
		if (_qty >= _qtyMax)
		{
			Util.SetButton(maxBtn, enabled: false);
			Util.SetButton(plusBtn, enabled: false);
		}
		else
		{
			Util.SetButton(maxBtn);
			Util.SetButton(plusBtn);
		}
		DoUpdate();
	}

	public void OnMinBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qtyMin);
	}

	public void OnMaxBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qtyMax);
	}

	public void OnPlusBtnDown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		mouseLock.transform.SetAsLastSibling();
		minBtn.transform.SetAsLastSibling();
		StartRepeatTimer(CheckIncreased(), added: true);
	}

	public void OnMinusBtnDown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		mouseLock.transform.SetAsLastSibling();
		minBtn.transform.SetAsLastSibling();
		StartRepeatTimer(CheckIncreased(), added: false);
	}

	public void OnPlusMinusBtnUp()
	{
		ClearRepeatTimer();
		mouseLock.transform.SetAsFirstSibling();
	}

	public bool CheckIncreased()
	{
		if (!Input.GetKeyDown(KeyCode.RightControl) && !Input.GetKey(KeyCode.RightControl) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.RightCommand) && !Input.GetKey(KeyCode.RightCommand) && !Input.GetKeyDown(KeyCode.LeftCommand))
		{
			return Input.GetKey(KeyCode.LeftCommand);
		}
		return true;
	}

	private void StartRepeatTimer(bool increased, bool added)
	{
		_repeatIncreased = increased;
		_repeatAdded = added;
		_repeatDelay = 150;
		DoRepeatStatChange();
		CreateRepeatTimer();
	}

	private void DoRepeatStatChange()
	{
		QtyChange(_repeatIncreased, _repeatAdded);
	}

	private void QtyChange(bool increased, bool added)
	{
		int allowedPoints = GetAllowedPoints(increased, added);
		allowedPoints *= (added ? 1 : (-1));
		SetQty(_qty + allowedPoints);
	}

	private int GetAllowedPoints(bool increased, bool added)
	{
		int num = 1;
		if (increased)
		{
			num = 10;
		}
		if (added)
		{
			if (_qtyMax < _qty + num)
			{
				num = _qtyMax - _qty;
			}
			else if (_qtyMin > _qty - num)
			{
				num = _qty;
			}
		}
		return num;
	}

	private void CreateRepeatTimer()
	{
		ClearRepeatTimer();
		_repeatTimer = StartCoroutine(OnRepeatTimer(_repeatDelay));
	}

	private void ClearRepeatTimer()
	{
		if (_repeatTimer != null)
		{
			StopCoroutine(_repeatTimer);
			_repeatTimer = null;
		}
	}

	private IEnumerator OnRepeatTimer(int delay)
	{
		yield return new WaitForSeconds((float)delay * 0.001f);
		_repeatDelay -= 5;
		if (_repeatDelay < 5)
		{
			_repeatDelay = 5;
		}
		StopCoroutine(_repeatTimer);
		DoRepeatStatChange();
		if (_repeatTimer != null)
		{
			CreateRepeatTimer();
		}
	}

	public void OnTradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (!tutorial)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_trade_confirm_count", new string[1] { Util.NumberFormat(_qty) }, color: true), null, null, delegate
			{
				DoItemTrade(_tradeItem, _tradeItem.sourceRef as CraftTradeRef, _qty);
			});
		}
		else
		{
			DoItemTrade(_tradeItem, _tradeItem.sourceRef as CraftTradeRef);
		}
	}

	private void DoItemTrade(TradeItem model, CraftTradeRef craftTradeRef, int qty = 1)
	{
		GameData.instance.main.ShowLoading();
		_currentModel = model;
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_TRADE), OnItemTrade);
		MerchantDALC.instance.doItemTrade(craftTradeRef.id, qty);
	}

	private void OnItemTrade(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_TRADE), OnItemTrade);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ItemData.fromSFSObject(sfsob);
			List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
			List<ItemData> list2 = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
			GameData.instance.PROJECT.character.addItems(list);
			GameData.instance.PROJECT.character.removeItems(list2);
			GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
			KongregateAnalytics.checkEconomyTransaction("Craft Trade", list2, list, sfsob, "Craft", 2);
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_SPENT, ItemData.getItemRefQuantity(_currentModel.tradeRef.requiredItems, CurrencyBook.Lookup(1)));
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_SPENT, ItemData.getItemRefQuantity(_currentModel.tradeRef.requiredItems, CurrencyBook.Lookup(2)));
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
			OnClose();
			if (tutorial)
			{
				GameData.instance.windowGenerator.ClearAllWindows();
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
		_currentModel = null;
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	private int CalcMaxAmount(TradeItem model)
	{
		int num = int.MaxValue;
		foreach (ItemData requiredItem in model.tradeRef.requiredItems)
		{
			if (requiredItem != null)
			{
				num = Mathf.Min(num, GameData.instance.PROJECT.character.getItemQty(requiredItem.itemRef) / requiredItem.qty);
			}
		}
		if (num == int.MaxValue)
		{
			return 0;
		}
		return num;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		tradeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		tradeBtn.interactable = true;
	}
}
