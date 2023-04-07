using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

public class EventSegmentedTierRef
{
	private int _min;

	private int _max;

	private List<EventSegmentedRankRef> _ranks;

	public int min => _min;

	public int max => _max;

	public List<EventSegmentedRankRef> ranks => _ranks;

	public EventSegmentedTierRef(EventRewardBookData.Tier data)
	{
		_min = ((data.min != null) ? int.Parse(data.min) : int.MinValue);
		_max = ((data.max != null) ? int.Parse(data.max) : int.MaxValue);
		_ranks = new List<EventSegmentedRankRef>();
		foreach (EventRewardBookData.Rank lstRank in data.lstRanks)
		{
			_ranks.Add(new EventSegmentedRankRef(lstRank));
		}
	}
}
