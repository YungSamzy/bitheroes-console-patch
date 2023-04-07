using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

public class EventSegmentedRankRef
{
	private int _min;

	private int _max;

	private List<EventSegmentedPoolRef> _pools;

	public int min => _min;

	public int max => _max;

	public List<EventSegmentedPoolRef> pools => _pools;

	public EventSegmentedRankRef(EventRewardBookData.Rank data)
	{
		_min = ((data.min != null) ? int.Parse(data.min) : int.MinValue);
		_max = ((data.max != null) ? int.Parse(data.max) : int.MaxValue);
		_pools = new List<EventSegmentedPoolRef>();
		foreach (EventRewardBookData.ZonePool lstPool in data.lstPools)
		{
			_pools.Add(new EventSegmentedPoolRef(lstPool));
		}
	}

	public List<ItemData> getAllPoolItems()
	{
		List<ItemData> list = new List<ItemData>();
		foreach (EventSegmentedPoolRef pool in _pools)
		{
			list.AddRange(pool.items);
		}
		return list;
	}
}
