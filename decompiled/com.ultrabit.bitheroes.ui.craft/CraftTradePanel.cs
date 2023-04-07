using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.tradelist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftTradePanel : MonoBehaviour
{
	public GameObject craftListView;

	public GameObject craftListScroll;

	public TradeList tradeList;

	public Button filterBtn;

	public TMP_InputField searchTxt;

	public TextMeshProUGUI descTxt;

	private ItemTradeFilterWindow _panel;

	private CraftWindow _craftWindow;

	private bool _updatePending;

	private bool _hasFilteredRecipes;

	public static List<CraftTradeRef> availableCraftRecipes
	{
		get
		{
			List<CraftTradeRef> list = new List<CraftTradeRef>();
			for (int i = 0; i < CraftBook.sizeTrades; i++)
			{
				CraftTradeRef craftTradeRef = CraftBook.LookupTrade(i);
				if (craftTradeRef == null || craftTradeRef.isOld || craftTradeRef.crafter != "NONE" || (craftTradeRef.gameRequirement != null && !craftTradeRef.gameRequirement.RequirementsMet()))
				{
					continue;
				}
				bool flag = true;
				foreach (ItemRef craftingRequiredItem in craftTradeRef.craftingRequiredItems)
				{
					if (!GameData.instance.PROJECT.character.inventory.hasOwnedItem(craftingRequiredItem))
					{
						flag = false;
					}
				}
				if (flag)
				{
					list.Add(craftTradeRef);
				}
			}
			return list;
		}
	}

	public bool updatePending
	{
		get
		{
			return _updatePending;
		}
		set
		{
			_updatePending = value;
		}
	}

	public void LoadDetails(CraftWindow craftWindow)
	{
		_craftWindow = craftWindow;
		filterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_filter");
		descTxt.text = Language.GetString("ui_no_trade_recipes");
		tradeList.InitList(_craftWindow);
		CreateList();
	}

	private void CreateList()
	{
		double virtualAbstractNormalizedScrollPosition = tradeList.GetVirtualAbstractNormalizedScrollPosition();
		tradeList.ClearList();
		int tradeFilter = GameData.instance.SAVE_STATE.GetTradeFilter(GameData.instance.PROJECT.character.id);
		AdvancedFilterSettings tradeAdvancedFilter = GameData.instance.SAVE_STATE.GetTradeAdvancedFilter(GameData.instance.PROJECT.character.id);
		string text = searchTxt.text;
		Dictionary<int, bool> seenRecipes = GameData.instance.SAVE_STATE.GetSeenRecipes(GameData.instance.PROJECT.character.id);
		_hasFilteredRecipes = false;
		foreach (CraftTradeRef availableCraftRecipe in availableCraftRecipes)
		{
			int num = availableCraftRecipe.tradeRef.resultItem.itemRef.itemType;
			if (num == 4)
			{
				ConsumableRef consumableRef = availableCraftRecipe.tradeRef.resultItem.itemRef as ConsumableRef;
				if (consumableRef.consumableItemType > 0)
				{
					num = consumableRef.consumableItemType;
				}
			}
			int id = availableCraftRecipe.tradeRef.resultItem.itemRef.rarityRef.id;
			if (GameData.instance.SAVE_STATE.GetIsTradeFiltered(GameData.instance.PROJECT.character.id, num, id, tradeFilter, tradeAdvancedFilter, ItemTradeFilterWindow.TRADE_FILTERS))
			{
				_hasFilteredRecipes = true;
				continue;
			}
			if (text.Length > 0 && !Language.GetString(availableCraftRecipe.tradeRef.resultItem.itemRef.name).ToLower().Contains(searchTxt.text.ToLower()))
			{
				_hasFilteredRecipes = true;
				continue;
			}
			tradeList.Data.InsertOneAtEnd(new TradeItem
			{
				tradeRef = availableCraftRecipe.tradeRef,
				sourceRef = availableCraftRecipe,
				seen = seenRecipes.ContainsKey(availableCraftRecipe.id)
			});
		}
		descTxt.gameObject.SetActive(tradeList.Data.Count == 0);
		if (descTxt.gameObject.activeSelf)
		{
			if (_hasFilteredRecipes)
			{
				descTxt.SetText(Language.GetString("ui_no_filtered_recipes"));
			}
			else
			{
				descTxt.SetText(Language.GetString("ui_no_trade_recipes"));
			}
		}
		if (!descTxt.gameObject.activeSelf)
		{
			tradeList.Data.InsertOneAtEnd(new TradeItem
			{
				unlocks = true,
				unlocksText = new string[2]
				{
					Language.GetString("ui_trade_unlocks_1"),
					Language.GetString("ui_trade_unlocks_2")
				}
			});
		}
		tradeList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	public void DoUpdate()
	{
		CreateList();
	}

	public void OnFilterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoFilterWindow();
	}

	private void DoFilterWindow()
	{
		_panel = GameData.instance.windowGenerator.NewItemTradeFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterWindow);
	}

	private void OnFilterWindow()
	{
		_panel.OnEventClose.RemoveAllListeners();
		DoUpdate();
	}

	public void CheckTutorialAugment()
	{
		MyListItemViewsHolder myListItemViewsHolder = FindTutorialTradeAndScroll(15, VariableBook.tutorialAugmentsId);
		if (myListItemViewsHolder != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(70);
			GameData.instance.PROJECT.CheckTutorialChanges();
			if (tradeList.Data[myListItemViewsHolder.ItemIndex].tradeRef.requirementsMet())
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(myListItemViewsHolder.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(70), 3, myListItemViewsHolder.craftBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(myListItemViewsHolder.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(70), 3, myListItemViewsHolder.craftBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.character.tutorial.SetState(71);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void CheckTutorialRune()
	{
		MyListItemViewsHolder myListItemViewsHolder = FindTutorialTradeAndScroll(4, VariableBook.tutorialRunesId);
		if (myListItemViewsHolder != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(87);
			GameData.instance.PROJECT.CheckTutorialChanges();
			if (tradeList.Data[myListItemViewsHolder.ItemIndex].tradeRef.requirementsMet())
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(myListItemViewsHolder.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(87), 3, myListItemViewsHolder.craftBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(myListItemViewsHolder.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(70), 3, myListItemViewsHolder.craftBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.character.tutorial.SetState(88);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private MyListItemViewsHolder FindTutorialTradeAndScroll(int itemType, int itemID)
	{
		for (int i = 0; i < tradeList.Data.Count; i++)
		{
			if (tradeList.Data[i].tradeRef != null && tradeList.Data[i].tradeRef.resultItem.type == itemType && tradeList.Data[i].tradeRef.resultItem.id == itemID)
			{
				tradeList.ScrollTo(i);
				return tradeList.GetItemViewsHolderIfVisible(i);
			}
		}
		return null;
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		DoUpdate();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		craftListView.gameObject.SetActive(value: true);
		craftListScroll.gameObject.SetActive(value: true);
		tradeList.Refresh();
	}

	public void Hide()
	{
		GameData.instance.SAVE_STATE.SetSeenRecipes(GameData.instance.PROJECT.character.id, tradeList.recipesSeen);
		_craftWindow.UpdateCraftNotificationTile();
		base.gameObject.SetActive(value: false);
		craftListView.gameObject.SetActive(value: false);
		craftListScroll.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		filterBtn.interactable = true;
	}

	public void DoDisable()
	{
		filterBtn.interactable = false;
	}
}
