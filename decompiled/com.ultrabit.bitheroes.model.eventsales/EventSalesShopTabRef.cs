using System.Collections.Generic;

namespace com.ultrabit.bitheroes.model.eventsales;

public class EventSalesShopTabRef
{
	private int _id;

	private string _name;

	private List<EventSalesShopItemRef> _items = new List<EventSalesShopItemRef>();

	public int id => _id;

	public string name => _name;

	public List<EventSalesShopItemRef> items => _items;

	public EventSalesShopTabRef(int id, string name, List<EventSalesShopItemRef> items)
	{
		_id = id;
		_name = name;
		_items = items;
	}

	public EventSalesShopItemRef getItem(int id)
	{
		foreach (EventSalesShopItemRef item in _items)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}
}
