using System;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.data;

public class EventSalesShopItemRefModelData : BaseModelData
{
	private EventSalesShopEventRef _eventRef;

	private EventSalesShopItemRef _eventSalesShopItemRef;

	private int _qty;

	public EventSalesShopItemRef eventSalesShopItemRef => _eventSalesShopItemRef;

	public override ItemRef itemRef => eventSalesShopItemRef.itemData.itemRef;

	public EventSalesShopEventRef eventRef => _eventRef;

	public override int power => 0;

	public override int stamina => 0;

	public override int agility => 0;

	public override object data => null;

	public override int qty
	{
		get
		{
			return _qty;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override int type => eventSalesShopItemRef.itemData.itemRef.itemType;

	public EventSalesShopItemRefModelData(EventSalesShopEventRef eventRef, EventSalesShopItemRef eventSalesShopItemRef, int qty)
	{
		_eventRef = eventRef;
		_eventSalesShopItemRef = eventSalesShopItemRef;
		_qty = qty;
	}
}
