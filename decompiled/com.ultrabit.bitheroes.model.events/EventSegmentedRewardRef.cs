using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{link} (EventSegmentedRewardRef)")]
public class EventSegmentedRewardRef : IEquatable<EventSegmentedRewardRef>, IComparable<EventSegmentedRewardRef>
{
	private string _link;

	private List<EventSegmentedTierRef> _tiers;

	public string link => _link;

	public List<EventSegmentedTierRef> tiers => _tiers;

	public EventSegmentedRewardRef(EventRewardBookData.EventReward data)
	{
		_link = data.link;
		_tiers = new List<EventSegmentedTierRef>();
		foreach (EventRewardBookData.Tier item in data.lstTier)
		{
			_tiers.Add(new EventSegmentedTierRef(item));
		}
	}

	public List<EventRewardRef> GetRewardsforTier(int tier, List<EventRewardRef> rewards)
	{
		foreach (EventSegmentedTierRef tier2 in _tiers)
		{
			if (tier < tier2.min || tier > tier2.max)
			{
				continue;
			}
			foreach (EventSegmentedRankRef rank in tier2.ranks)
			{
				bool flag = false;
				for (int i = 0; i < rewards.Count; i++)
				{
					EventRewardRef eventRewardRef = rewards[i];
					if (eventRewardRef.min == rank.min && eventRewardRef.max == rank.max)
					{
						eventRewardRef.AddItems(rank.getAllPoolItems());
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					rewards.Add(new EventRewardRef(rank.min, rank.max, rank.getAllPoolItems()));
				}
			}
			return rewards;
		}
		return null;
	}

	public bool Equals(EventSegmentedRewardRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(EventSegmentedRewardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
