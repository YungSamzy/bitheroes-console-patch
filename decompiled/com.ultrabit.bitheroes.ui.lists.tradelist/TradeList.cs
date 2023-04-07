using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.craft;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.tradelist;

public class TradeList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Transform itemCraftTilePrefab;

	public Transform unlocksCraftTilePrefab;

	public bool tutorial;

	private Dictionary<int, bool> _schematicsSeen;

	private Dictionary<int, bool> _recipesSeen;

	private TradeItem _currentModel;

	private CraftWindow _craftWindow;

	public SimpleDataHelper<TradeItem> Data { get; private set; }

	public Dictionary<int, bool> schematicsSeen => _schematicsSeen;

	public Dictionary<int, bool> recipesSeen => _recipesSeen;

	protected override void Start()
	{
		InitList();
	}

	public void InitList(CraftWindow craftWindow = null)
	{
		if (Data == null)
		{
			_craftWindow = craftWindow;
			Data = new SimpleDataHelper<TradeItem>(this);
			_schematicsSeen = GameData.instance.SAVE_STATE.GetSeenSchematics(GameData.instance.PROJECT.character.id);
			_recipesSeen = GameData.instance.SAVE_STATE.GetSeenRecipes(GameData.instance.PROJECT.character.id);
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		TradeItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.backgroundImage.enabled = !model.unlocks;
		newOrRecycled.craftBtn.gameObject.SetActive(!model.unlocks);
		newOrRecycled.resultIcon.gameObject.SetActive(!model.unlocks);
		newOrRecycled.unlocksText1.gameObject.SetActive(model.unlocks);
		newOrRecycled.unlocksText2.gameObject.SetActive(model.unlocks);
		if (model.unlocks)
		{
			newOrRecycled.unlocksText1.text = ((model.unlocksText != null && model.unlocksText.Length >= 1) ? model.unlocksText[0] : string.Empty);
			newOrRecycled.unlocksText2.text = ((model.unlocksText != null && model.unlocksText.Length >= 2) ? model.unlocksText[1] : string.Empty);
			return;
		}
		newOrRecycled.craftBtn.GetComponentInChildren<TextMeshProUGUI>().text = ((model.sourceRef is FusionRef) ? Language.GetString("ui_fuse") : Language.GetString("ui_craft"));
		newOrRecycled.craftBtn.onClick.RemoveAllListeners();
		newOrRecycled.craftBtn.onClick.AddListener(delegate
		{
			OnCraftBtn(model);
		});
		ItemIcon itemIcon = newOrRecycled.resultIcon.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.resultIcon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.tradeRef.resultItem.itemRef, model.tradeRef.resultItem.qty), locked: true, model.tradeRef.resultItem.qty, tintRarity: true, newOrRecycled.resultIconBg);
		itemIcon.SetItemActionType(0);
		if (model.tradeRef.resultItem.itemRef.itemType == 6 || (model.tradeRef.resultItem.itemRef.isViewable() && model.tradeRef.resultItem.itemRef.hasContents()))
		{
			itemIcon.SetItemActionType(7);
		}
		foreach (ItemData requiredItem in model.tradeRef.requiredItems)
		{
			if (requiredItem != null)
			{
				Transform obj = Object.Instantiate(itemCraftTilePrefab);
				obj.SetParent(newOrRecycled.requiredPlaceholder, worldPositionStays: false);
				obj.GetComponent<ItemCraftTile>().LoadDetails(requiredItem, isLocked(model));
			}
		}
		bool flag = isLocked(model);
		bool flag2 = model.tradeRef.resultItem.itemRef.isHidden();
		bool flag3 = !isLocked(model) && model.tradeRef.requirementsMet();
		if (isLocked(model))
		{
			newOrRecycled.backgroundImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
			Util.SetImageAlpha(newOrRecycled.backgroundImage, 0.9f);
		}
		else
		{
			newOrRecycled.backgroundImage.color = (flag3 ? Color.white : new Color(0.7f, 0.7f, 0.7f));
			Util.SetImageAlpha(newOrRecycled.backgroundImage, 1f);
		}
		bool flag4 = false;
		if (model.sourceRef != null && model.sourceRef is FusionRef)
		{
			flag4 = (model.sourceRef as FusionRef).craftLocked;
		}
		if (flag3 && !flag4)
		{
			Util.SetButton(newOrRecycled.craftBtn);
		}
		else
		{
			Util.SetButton(newOrRecycled.craftBtn, enabled: false);
		}
		itemIcon.SetHidden(flag2 || flag, flag, flag);
		itemIcon.SetLongNotify(notify: false);
		if (model.sourceRef is FusionRef)
		{
			if (!(model.sourceRef as FusionRef).isLocked() && !model.seen && !model.notifyShown)
			{
				itemIcon.SetLongNotify(GameData.instance.SAVE_STATE.notificationsFusions && !GameData.instance.SAVE_STATE.notificationsDisabled);
				model.notifyShown = true;
				if (!_schematicsSeen.ContainsKey(itemIcon.itemRef.id))
				{
					_schematicsSeen.Add(itemIcon.itemRef.id, value: true);
				}
				else
				{
					_schematicsSeen[itemIcon.itemRef.id] = true;
				}
			}
		}
		else if (_craftWindow != null && _craftWindow.currentTab == 1 && model.sourceRef is CraftTradeRef && !model.seen && !model.notifyShown)
		{
			itemIcon.SetLongNotify(GameData.instance.SAVE_STATE.notificationsCraft && !GameData.instance.SAVE_STATE.notificationsDisabled);
			model.notifyShown = true;
			if (!_recipesSeen.ContainsKey(model.sourceRef.id))
			{
				_recipesSeen.Add(model.sourceRef.id, value: true);
			}
			else
			{
				_recipesSeen[model.sourceRef.id] = true;
			}
		}
	}

	private bool isLocked(TradeItem model)
	{
		if (model.sourceRef is FusionRef)
		{
			return (model.sourceRef as FusionRef).isLocked();
		}
		return false;
	}

	private void OnCraftBtn(TradeItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		string text = "";
		List<string> list = new List<string>();
		if (model.sourceRef is FusionRef)
		{
			foreach (ItemData requiredItem in model.tradeRef.requiredItems)
			{
				if (requiredItem.itemRef.itemType == 6)
				{
					list.Add(requiredItem.itemRef.coloredName);
				}
			}
			if (!tutorial)
			{
				text = ((list.Count > 0) ? Language.GetString("ui_fuse_confirm", new string[1] { Util.FormatStrings(list) }) : Language.GetString("ui_fuse_confirm_no_familiars"));
				GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), text, null, null, delegate
				{
					OnCraftConfirm(model);
				}, null, null, GetComponentInParent<WindowsMain>().layer);
			}
			else
			{
				OnCraftConfirm(model);
			}
		}
		else
		{
			text = Language.GetString("ui_trade_confirm");
			GameData.instance.windowGenerator.NewItemTradeWindow(model);
		}
		tutorial = false;
	}

	private void OnCraftConfirm(TradeItem model)
	{
		if (model.sourceRef is CraftTradeRef)
		{
			CraftTradeRef craftTradeRef = model.sourceRef as CraftTradeRef;
			DoItemTrade(model, craftTradeRef);
		}
		else if (model.sourceRef is FusionRef)
		{
			FusionRef fusionRef = model.sourceRef as FusionRef;
			DoItemFusion(model, fusionRef);
		}
	}

	private void DoItemTrade(TradeItem model, CraftTradeRef craftTradeRef)
	{
		GameData.instance.main.ShowLoading();
		_currentModel = model;
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_TRADE), OnItemTrade);
		MerchantDALC.instance.doItemTrade(craftTradeRef.id);
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
			ItemData itemData = ItemData.fromSFSObject(sfsob);
			GameData.instance.PROJECT.character.addItem(itemData);
			GameData.instance.PROJECT.character.removeItems(_currentModel.tradeRef.requiredItems);
			ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
			if ((bool)itemListWindow)
			{
				itemListWindow.RemoveItems(_currentModel.tradeRef.requiredItems);
			}
			List<ItemData> list = new List<ItemData>();
			list.Add(itemData);
			GameData.instance.PROJECT.character.checkTimerChanges(sfsob);
			KongregateAnalytics.checkEconomyTransaction("Craft Trade", _currentModel.tradeRef.requiredItems, list, sfsob, "Craft", 2);
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.GOLD_SPENT, ItemData.getItemRefQuantity(_currentModel.tradeRef.requiredItems, CurrencyBook.Lookup(1)));
			GameData.instance.PROJECT.character.analytics.adjustValue(BHAnalytics.CREDITS_SPENT, ItemData.getItemRefQuantity(_currentModel.tradeRef.requiredItems, CurrencyBook.Lookup(2)));
			GameData.instance.windowGenerator.ShowItem(itemData, compare: true, added: true, forceNonEquipment: false, null, GetComponentInParent<WindowsMain>().layer);
		}
		_currentModel = null;
	}

	private void DoItemFusion(TradeItem model, FusionRef fusionRef)
	{
		GameData.instance.main.ShowLoading();
		_currentModel = model;
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_FUSION), OnFusion);
		MerchantDALC.instance.doItemFusion(fusionRef.id);
	}

	private void OnFusion(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_FUSION), OnFusion);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ItemData itemData = ItemData.fromSFSObject(sfsob);
			GameData.instance.PROJECT.character.addItem(itemData);
			GameData.instance.PROJECT.character.removeItems(_currentModel.tradeRef.requiredItems);
			List<ItemData> list = new List<ItemData>();
			list.Add(itemData);
			KongregateAnalytics.checkEconomyTransaction("Fusion", _currentModel.tradeRef.requiredItems, list, sfsob, "Craft");
			FusionRef fusionRef = _currentModel.sourceRef as FusionRef;
			if (GameData.instance.SAVE_STATE.animations && GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 0, GameData.instance.SAVE_STATE.GetAnimationsTypes(GameData.instance.PROJECT.character.id)))
			{
				GameData.instance.windowGenerator.NewFusionResultWindow(fusionRef, _currentModel.parentWindow as GameObject);
			}
			else
			{
				GameData.instance.windowGenerator.ShowItem(itemData, compare: true, added: true, forceNonEquipment: false, null, GetComponentInParent<WindowsMain>().layer);
			}
		}
		_currentModel = null;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		for (int i = 0; i < inRecycleBinOrVisible.requiredPlaceholder.childCount; i++)
		{
			Object.Destroy(inRecycleBinOrVisible.requiredPlaceholder.GetChild(i).gameObject);
		}
		inRecycleBinOrVisible.craftBtn.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<TradeItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<TradeItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		TradeItem[] newItems = new TradeItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		TradeItem[] newItems = new TradeItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(TradeItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
