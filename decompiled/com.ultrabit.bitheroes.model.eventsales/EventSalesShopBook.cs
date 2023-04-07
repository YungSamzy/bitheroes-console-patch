using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.material;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.eventsales;

public class EventSalesShopBook
{
	private static List<EventSalesShopEventRef> _events;

	public static int tabsSize => GetCurrentEvent()?.tabs.Count ?? 0;

	public static int itemsSize => GetCurrentEvent()?.GetItems().Count ?? 0;

	public static bool HasEventActive => GetCurrentEvent() != null;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		if (XMLBook.instance.eventSalesShopBook == null)
		{
			yield break;
		}
		_events = new List<EventSalesShopEventRef>();
		int num = 0;
		foreach (EventSalesShopBookData.Event lstEvent in XMLBook.instance.eventSalesShopBook.lstEvents)
		{
			List<EventSalesShopTabRef> list = new List<EventSalesShopTabRef>();
			int id = 0;
			foreach (EventSalesShopBookData.Tab item3 in lstEvent.tabs.lstTab)
			{
				List<EventSalesShopItemRef> list2 = new List<EventSalesShopItemRef>();
				foreach (EventSalesShopBookData.Item item4 in item3.lstItem)
				{
					ItemRef itemRef = ItemBook.Lookup(item4.id, item4.type);
					if (itemRef != null)
					{
						ItemData itemData = new ItemData(itemRef, item4.qty);
						EventSalesShopItemRef item = new EventSalesShopItemRef(num, itemData, item4.cost, item4.purchaseLimit);
						list2.Add(item);
						num++;
					}
				}
				list.Add(new EventSalesShopTabRef(id, item3.name, list2));
			}
			EventSalesShopEventRef item2 = new EventSalesShopEventRef(lstEvent.id, lstEvent.hudSprite, lstEvent.hudLabel, lstEvent.name, lstEvent.topper, lstEvent.background, MaterialBook.Lookup(lstEvent.material), new DateRef(lstEvent.startDate, lstEvent.endDate), list);
			_events.Add(item2);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static EventSalesShopEventRef GetCurrentEvent()
	{
		if (_events == null)
		{
			return null;
		}
		foreach (EventSalesShopEventRef @event in _events)
		{
			if (@event.IsActive())
			{
				return @event;
			}
		}
		return null;
	}

	public static bool hasItem(ItemRef itemRef)
	{
		EventSalesShopEventRef currentEvent = GetCurrentEvent();
		if (currentEvent != null)
		{
			foreach (EventSalesShopItemRef item in currentEvent.GetItems())
			{
				if (item.itemData.itemRef.Equals(itemRef))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static EventSalesShopTabRef LookupTab(int id)
	{
		EventSalesShopEventRef currentEvent = GetCurrentEvent();
		if (currentEvent != null)
		{
			if (id >= 0 && id < currentEvent.tabs.Count)
			{
				return currentEvent.tabs[id];
			}
			return null;
		}
		return null;
	}

	public static EventSalesShopItemRef LookupItem(int id)
	{
		EventSalesShopEventRef currentEvent = GetCurrentEvent();
		if (currentEvent != null)
		{
			foreach (EventSalesShopItemRef item in currentEvent.GetItems())
			{
				if (item.id.Equals(id))
				{
					return item;
				}
			}
		}
		return null;
	}
}
