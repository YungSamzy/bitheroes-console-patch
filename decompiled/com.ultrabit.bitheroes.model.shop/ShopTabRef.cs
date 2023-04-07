using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.shop;

[DebuggerDisplay("{name} (ShopTabRef)")]
public class ShopTabRef : IEquatable<ShopTabRef>, IComparable<ShopTabRef>
{
	private List<ItemRef> _items;

	private ShopBookData.Tab _tabData;

	private int _id;

	public int id => _id;

	public string name => _tabData.name;

	public List<ItemRef> items => _items;

	public ShopTabRef(int id, ShopBookData.Tab tabData)
	{
		_tabData = tabData;
		_id = id;
		_items = new List<ItemRef>();
		foreach (ShopBookData.Item item in tabData.lstItem)
		{
			_items.Add(ItemBook.Lookup(item.id, item.type));
		}
	}

	public ItemRef getTutorialShopItem()
	{
		foreach (ItemRef item in _items)
		{
			if (item == VariableBook.tutorialShopItem)
			{
				return item;
			}
		}
		return null;
	}

	public bool Equals(ShopTabRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(ShopTabRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
