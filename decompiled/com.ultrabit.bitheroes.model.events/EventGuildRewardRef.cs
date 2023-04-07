using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{link} (EventGuildRewardRef)")]
public class EventGuildRewardRef : IEquatable<EventGuildRewardRef>, IComparable<EventGuildRewardRef>
{
	private string _link;

	private List<EventRewardRef> _rewards;

	public string link => _link;

	public List<EventRewardRef> rewards => _rewards;

	public EventGuildRewardRef(EventRewardBookData.EventReward data)
	{
		_link = data.link;
		_rewards = new List<EventRewardRef>();
		if (data.ranks != null && data.ranks.lstRewards != null)
		{
			foreach (BaseEventBookData.Reward lstReward in data.ranks.lstRewards)
			{
				_rewards.Add(new EventRewardRef(lstReward));
			}
		}
		if (data.points == null || data.points.lstRewards == null)
		{
			return;
		}
		foreach (BaseEventBookData.Reward lstReward2 in data.points.lstRewards)
		{
			_rewards.Add(new EventRewardRef(lstReward2));
		}
	}

	public bool Equals(EventGuildRewardRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(EventGuildRewardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
