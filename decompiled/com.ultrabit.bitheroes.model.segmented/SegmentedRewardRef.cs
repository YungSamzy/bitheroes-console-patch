using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.segmented;

[DebuggerDisplay("{link} (SegmentedRewardRef)")]
public class SegmentedRewardRef : IEquatable<SegmentedRewardRef>, IComparable<SegmentedRewardRef>
{
	private RewardBookData.Reward _rewardData;

	private List<SegmentedRewardRankRef> _ranks;

	public string link => _rewardData.link;

	public SegmentedRewardRef(RewardBookData.Reward rewardData)
	{
		_rewardData = rewardData;
		_ranks = new List<SegmentedRewardRankRef>();
		foreach (RewardBookData.Rank item in rewardData.ranks.lstRank)
		{
			_ranks.Add(new SegmentedRewardRankRef(item));
		}
	}

	public void GetEventRewardRef(int zone, List<EventRewardRef> baseRewardRef)
	{
		foreach (SegmentedRewardRankRef rank in _ranks)
		{
			EventRewardRef eventRewardRef = rank.GetEventRewardRef(zone);
			if (eventRewardRef == null)
			{
				continue;
			}
			bool flag = false;
			for (int i = 0; i < baseRewardRef.Count; i++)
			{
				if (baseRewardRef[i].min == eventRewardRef.min && baseRewardRef[i].max == eventRewardRef.max)
				{
					baseRewardRef[i].AddItems(eventRewardRef.items);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				baseRewardRef.Add(eventRewardRef);
			}
		}
	}

	public bool Equals(SegmentedRewardRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(SegmentedRewardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
