using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{name} (EventRewardRef)")]
public class EventRewardRef : IEquatable<EventRewardRef>, IComparable<EventRewardRef>
{
	private int _min;

	private int _max;

	private List<ItemData> _items;

	private string name => $"{min} to {max}";

	public int min => _min;

	public int max => _max;

	public List<ItemData> items => _items;

	public EventRewardRef(BaseEventBookData.Reward rewardData)
	{
		_min = ((rewardData.min != null) ? int.Parse(rewardData.min) : int.MinValue);
		_max = ((rewardData.max != null) ? int.Parse(rewardData.max) : int.MaxValue);
		_items = new List<ItemData>();
		foreach (BaseEventBookData.Item item in rewardData.lstItem)
		{
			_items.Add(new ItemData(ItemBook.Lookup(item.id, ItemRef.getItemType(item.type)), item.qty));
		}
	}

	public EventRewardRef(int min, int max, List<ItemData> items)
	{
		_min = min;
		_max = max;
		_items = items;
	}

	public void AddItems(List<ItemData> items)
	{
		_items.AddRange(items);
	}

	public bool Equals(EventRewardRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (min.Equals(other.min) && max.Equals(other.max))
		{
			return items.SequenceEqual(items);
		}
		return false;
	}

	public int CompareTo(EventRewardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = min.CompareTo(other.min);
		if (num == 0)
		{
			return max.CompareTo(other.max);
		}
		return num;
	}
}
