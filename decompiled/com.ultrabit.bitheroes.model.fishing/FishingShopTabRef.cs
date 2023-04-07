using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.fishing;

[DebuggerDisplay("{name} (FishingShopTabRef)")]
public class FishingShopTabRef : IEquatable<FishingShopTabRef>, IComparable<FishingShopTabRef>
{
	private int _id;

	private string _name;

	private List<FishingShopItemRef> _items = new List<FishingShopItemRef>();

	public int id => _id;

	public string name => _name;

	public List<FishingShopItemRef> items => _items;

	public FishingShopTabRef(int id, string name, List<FishingShopBookData.Item> items, int index)
	{
		_id = id;
		_name = name;
		foreach (FishingShopBookData.Item item2 in items)
		{
			ItemRef itemRef = ItemBook.Lookup(item2.id, item2.type);
			if (itemRef != null)
			{
				ItemData itemData = new ItemData(itemRef, item2.qty);
				FishingShopItemRef item = new FishingShopItemRef(index, itemData, item2.cost);
				_items.Add(item);
				index++;
			}
		}
	}

	public FishingShopItemRef getItem(int id)
	{
		foreach (FishingShopItemRef item in _items)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public bool Equals(FishingShopTabRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(FishingShopTabRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
