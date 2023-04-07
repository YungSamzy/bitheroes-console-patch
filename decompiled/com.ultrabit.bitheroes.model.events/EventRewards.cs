using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace com.ultrabit.bitheroes.model.events;

[DebuggerDisplay("{name} (EventRewards)")]
public class EventRewards : IEquatable<EventRewards>, IComparable<EventRewards>
{
	private List<EventRewardRef> _rewards;

	public List<EventRewardRef> rewards => _rewards;

	public EventRewards(List<EventRewardRef> rewards)
	{
		_rewards = rewards;
	}

	public EventRewardRef getRewardRef(int value)
	{
		foreach (EventRewardRef reward in _rewards)
		{
			if (reward != null && value >= reward.min && value <= reward.max)
			{
				return reward;
			}
		}
		return null;
	}

	public bool Equals(EventRewards other)
	{
		if (other == null)
		{
			return false;
		}
		return rewards.SequenceEqual(other.rewards);
	}

	public int CompareTo(EventRewards other)
	{
		return 0;
	}
}
