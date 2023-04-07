using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.eventsalesshoplist;

public class EventSalesShopList : GridAdapter<GridParams, EventSalesItemViewsHolder>
{
	public SimpleDataHelper<EventSalesShopItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventSalesShopItem>(this);
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

	protected override void UpdateCellViewsHolder(EventSalesItemViewsHolder newOrRecycled)
	{
		EventSalesShopItem eventSalesShopItem = Data[newOrRecycled.ItemIndex];
		int qty = GetQty(eventSalesShopItem);
		int num = eventSalesShopItem.eventSalesItemRef.purchaseLimit;
		int num2 = eventSalesShopItem.eventSalesItemRef.purchaseRemainingQty;
		if (!eventSalesShopItem.eventSalesItemRef.itemData.itemRef.unique)
		{
			num2 = ((eventSalesShopItem.eventSalesItemRef.purchaseRemainingQty < 0) ? num : eventSalesShopItem.eventSalesItemRef.purchaseRemainingQty);
		}
		else
		{
			num = 1;
			if (num2 == -1)
			{
				num2 = 1;
				if (GameData.instance.PROJECT.character.inventory.hasOwnedItem(eventSalesShopItem.eventSalesItemRef.itemData.itemRef))
				{
					num2 = 0;
				}
			}
		}
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new EventSalesShopItemRefModelData(eventSalesShopItem.eventRef, eventSalesShopItem.eventSalesItemRef, qty), locked: false, 0, tintRarity: true, newOrRecycled.UIItemIconColor);
		itemIcon.SetItemActionType(eventSalesShopItem.eventSalesItemRef.itemData.itemRef.hasContents() ? 7 : 4);
		itemIcon.setQty(eventSalesShopItem.eventSalesItemRef.itemData.qty * qty);
		newOrRecycled.UIName.text = eventSalesShopItem.eventSalesItemRef.itemData.itemRef.coloredName;
		bool flag = num > 0 || eventSalesShopItem.eventSalesItemRef.itemData.itemRef.unique;
		bool flag2 = false;
		newOrRecycled.UIRibbon.gameObject.SetActive(flag);
		if (flag)
		{
			newOrRecycled.UIRibbonTxt.text = $"{num - num2}/{num}";
			flag2 = num2 == 0;
			newOrRecycled.views.parent.GetComponent<ItemIcon>().DisableItem(flag2);
		}
		newOrRecycled.UICost.text = ((!flag2) ? Util.NumberFormat(eventSalesShopItem.eventSalesItemRef.cost * qty) : (eventSalesShopItem.eventSalesItemRef.itemData.itemRef.unique ? Language.GetString("ui_owned") : Language.GetString("shop_sold_out")));
		newOrRecycled.UIMaterialImage.overrideSprite = eventSalesShopItem.eventRef.GetMaterialRef().GetSpriteIcon();
	}

	private int GetQty(EventSalesShopItem model)
	{
		if (model.eventSalesItemRef.itemData.itemRef.allowQty && model.eventSalesShopPanel != null && model.eventSalesShopPanel.fishingShopWindow != null)
		{
			return model.eventSalesShopPanel.fishingShopWindow.GetQty();
		}
		return 1;
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(EventSalesItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<EventSalesShopItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<EventSalesShopItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		EventSalesShopItem[] newItems = new EventSalesShopItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		EventSalesShopItem[] newItems = new EventSalesShopItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(EventSalesShopItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
