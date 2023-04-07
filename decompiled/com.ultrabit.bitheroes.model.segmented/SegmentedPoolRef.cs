using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.segmented;

[DebuggerDisplay("{link} (SegmentedPoolRef)")]
public class SegmentedPoolRef : IEquatable<SegmentedPoolRef>, IComparable<SegmentedPoolRef>
{
	private RewardBookData.Pool _poolData;

	private EventRewardBookData.Pool _poolEventData;

	private List<ItemRef> _items;

	public string link
	{
		get
		{
			if (_poolData == null)
			{
				return _poolEventData.link;
			}
			return _poolData.link;
		}
	}

	public List<ItemRef> items => _items;

	public SegmentedPoolRef(RewardBookData.Pool poolData)
	{
		_poolData = poolData;
		_items = new List<ItemRef>();
		foreach (RewardBookData.Item item in poolData.lstItem)
		{
			_items.Add(ItemBook.Lookup(item.id, ItemRef.getItemType(item.type)));
		}
	}

	public SegmentedPoolRef(EventRewardBookData.Pool poolData)
	{
		_poolEventData = poolData;
		_items = new List<ItemRef>();
		foreach (EventRewardBookData.PoolItem item in poolData.lstItem)
		{
			_items.Add(ItemBook.Lookup(item.id, ItemRef.getItemType(item.type)));
		}
	}

	public bool Equals(SegmentedPoolRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(SegmentedPoolRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
