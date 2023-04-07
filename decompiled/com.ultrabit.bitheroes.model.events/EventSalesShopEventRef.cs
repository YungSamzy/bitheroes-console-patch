using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.material;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{name} (EventSalesShopEventRef)")]
public class EventSalesShopEventRef : IEquatable<EventSalesShopEventRef>, IComparable<EventSalesShopEventRef>
{
	private int _id;

	private string _hudSprite;

	private string _hudLabel;

	private string _name;

	private string _topper;

	private string _background;

	private MaterialRef _materialRef;

	private DateRef _dateRef;

	private List<EventSalesShopTabRef> _tabs;

	private List<EventSalesShopItemRef> _items;

	public List<EventSalesShopTabRef> tabs => _tabs;

	public int id => _id;

	public string hudSprite => _hudSprite;

	public string hudLabel => _hudLabel;

	public EventSalesShopEventRef(int id, string hudSprite, string hudLabel, string name, string topper, string background, MaterialRef materialRef, DateRef dateRef, List<EventSalesShopTabRef> tabs)
	{
		_id = id;
		_hudSprite = hudSprite;
		_hudLabel = hudLabel;
		_name = name;
		_topper = topper;
		_background = background;
		_materialRef = materialRef;
		_dateRef = dateRef;
		_tabs = tabs;
		_items = null;
	}

	public DateRef GetDateRef()
	{
		return _dateRef;
	}

	public bool IsActive()
	{
		return _dateRef.getActive();
	}

	public List<EventSalesShopItemRef> GetItems()
	{
		if (_items != null)
		{
			return _items;
		}
		_items = new List<EventSalesShopItemRef>();
		new List<EventSalesShopItemRef>();
		foreach (EventSalesShopTabRef tab in _tabs)
		{
			foreach (EventSalesShopItemRef item in tab.items)
			{
				_items.Add(item);
			}
		}
		return _items;
	}

	public bool Equals(EventSalesShopEventRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(EventSalesShopEventRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}

	public string GetName()
	{
		return _name;
	}

	public string GetBackground()
	{
		return _background;
	}

	public string GetTopper()
	{
		return _topper;
	}

	public MaterialRef GetMaterialRef()
	{
		return _materialRef;
	}
}
