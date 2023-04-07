using System.Collections.Generic;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.lists.eventsalesshoplist;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.eventsales;

public class EventSalesShopPanel : MonoBehaviour
{
	public GameObject fishingShopListView;

	public GameObject fishingShopListScroll;

	public EventSalesShopList eventShopList;

	private EventSalesShopTabRef _tabRef;

	private EventSalesShopWindow _eventShopWindow;

	private EventSalesShopEventRef _eventRef;

	public EventSalesShopTabRef tabRef => _tabRef;

	public EventSalesShopWindow fishingShopWindow => _eventShopWindow;

	public void LoadDetails(EventSalesShopWindow eventShopWindow, EventSalesShopEventRef eventRef, EventSalesShopTabRef tabRef = null)
	{
		_eventShopWindow = eventShopWindow;
		_tabRef = tabRef;
		_eventRef = eventRef;
		eventShopList.InitList();
	}

	public void CheckTutorial()
	{
	}

	public void DoUpdate(List<ItemData> purchasesCountList = null)
	{
		double virtualAbstractNormalizedScrollPosition = eventShopList.GetVirtualAbstractNormalizedScrollPosition();
		eventShopList.ClearList();
		foreach (EventSalesShopItemRef eventItemRef in _tabRef.items)
		{
			if (purchasesCountList != null && purchasesCountList.Exists((ItemData a) => a.itemRef.Equals(eventItemRef.itemData.itemRef)) && (eventItemRef.purchaseLimit > 0 || eventItemRef.itemData.itemRef.unique))
			{
				ItemData itemData = purchasesCountList.Find((ItemData a) => a.itemRef.Equals(eventItemRef.itemData.itemRef));
				if (itemData != null)
				{
					eventItemRef.purchaseRemainingQty = eventItemRef.purchaseLimit - itemData.qty;
				}
			}
			eventShopList.Data.InsertOneAtEnd(new EventSalesShopItem
			{
				eventSalesShopPanel = this,
				eventSalesItemRef = eventItemRef,
				eventRef = _eventRef
			});
		}
		eventShopList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	public void DoRefreshQty()
	{
		eventShopList.Refresh();
	}

	public void DoShow()
	{
		fishingShopListView.SetActive(value: true);
		fishingShopListScroll.SetActive(value: true);
	}

	public void DoHide()
	{
		fishingShopListView.SetActive(value: false);
		fishingShopListScroll.SetActive(value: false);
	}
}
