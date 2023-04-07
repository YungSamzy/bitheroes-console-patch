using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.daily;

[DebuggerDisplay("{day} (DailyRewardRef)")]
public class DailyRewardRef : IEquatable<DailyRewardRef>, IComparable<DailyRewardRef>
{
	private int _day;

	private List<ItemData> _items;

	public int day => _day;

	public List<ItemData> items => _items;

	public DailyRewardRef(int day, DailyRewardBookData.Items items)
	{
		_day = day;
		_items = new List<ItemData>();
		foreach (DailyRewardBookData.Item item in items.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_items.Add(new ItemData(itemRef, item.qty));
			}
		}
	}

	public bool Equals(DailyRewardRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (day.Equals(other.day))
		{
			return items.SequenceEqual(other.items);
		}
		return false;
	}

	public int CompareTo(DailyRewardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return day.CompareTo(other.day);
	}
}
