using System.Collections.Generic;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.model.segmented;

public class SegmentedRewards
{
	private string _pool;

	private List<SegmentedRewardRef> _segmentedRewardRef;

	public SegmentedRewards(string pool)
	{
		_segmentedRewardRef = new List<SegmentedRewardRef>();
		string[] array = pool.Split(',');
		foreach (string text in array)
		{
			SegmentedRewardRef segmentedRewardByLink = RewardBook.GetSegmentedRewardByLink(text);
			if (segmentedRewardByLink != null)
			{
				_segmentedRewardRef.Add(segmentedRewardByLink);
			}
			else
			{
				D.LogWarning("WARNING: Error when getting SegmentedRewardRef for link: " + text);
			}
		}
	}

	public EventRewards GetEventRewards(int zoneComplete)
	{
		List<EventRewardRef> list = new List<EventRewardRef>();
		foreach (SegmentedRewardRef item in _segmentedRewardRef)
		{
			item.GetEventRewardRef(zoneComplete, list);
			foreach (EventRewardRef item2 in list)
			{
				_ = item2;
			}
		}
		return new EventRewards(list);
	}
}
