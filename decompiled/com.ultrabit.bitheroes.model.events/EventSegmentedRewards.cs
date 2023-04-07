using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.model.events;

public class EventSegmentedRewards
{
	private string _pool;

	private List<EventSegmentedRewardRef> _segmentedRewardRef;

	public EventSegmentedRewards(string pool)
	{
		_pool = pool;
		_segmentedRewardRef = new List<EventSegmentedRewardRef>();
		string[] array = _pool.Split(',');
		foreach (string text in array)
		{
			EventSegmentedRewardRef segmentedRewardByLink = EventRewardBook.GetSegmentedRewardByLink(text);
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

	public EventRewards GetEventRewards(int tier)
	{
		List<EventRewardRef> list = new List<EventRewardRef>();
		foreach (EventSegmentedRewardRef item in _segmentedRewardRef)
		{
			item.GetRewardsforTier(tier, list);
			foreach (EventRewardRef item2 in list)
			{
				_ = item2;
			}
		}
		return new EventRewards(list);
	}
}
