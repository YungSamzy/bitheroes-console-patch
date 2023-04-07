using System.Collections.Generic;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.segmented;

public class SegmentedRewardRankRef
{
	private int _min;

	private int _max;

	private List<SegmentedRewardZoneCompleteRef> _zoneComplete;

	public SegmentedRewardRankRef(RewardBookData.Rank rankData)
	{
		if (rankData.min == "")
		{
			rankData.min = null;
		}
		if (rankData.max == "")
		{
			rankData.max = null;
		}
		_min = ((rankData.min != null) ? int.Parse(rankData.min) : int.MinValue);
		_max = ((rankData.max != null) ? int.Parse(rankData.max) : int.MaxValue);
		_zoneComplete = new List<SegmentedRewardZoneCompleteRef>();
		foreach (RewardBookData.ZoneCompleted item in rankData.lstZoneCompleted)
		{
			_zoneComplete.Add(new SegmentedRewardZoneCompleteRef(item));
		}
	}

	public EventRewardRef GetEventRewardRef(int zone)
	{
		foreach (SegmentedRewardZoneCompleteRef item in _zoneComplete)
		{
			if (item.ContainsZone(zone))
			{
				return new EventRewardRef(_min, _max, new List<ItemData>(item.items));
			}
		}
		return null;
	}
}
