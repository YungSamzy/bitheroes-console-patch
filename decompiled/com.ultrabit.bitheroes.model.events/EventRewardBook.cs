using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.segmented;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.events;

public class EventRewardBook
{
	private static List<SegmentedPoolRef> _pools;

	private static List<EventSegmentedRewardRef> _segmented_rewards;

	private static List<EventGuildRewardRef> _guild_rewards;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_pools = new List<SegmentedPoolRef>();
		_segmented_rewards = new List<EventSegmentedRewardRef>();
		_guild_rewards = new List<EventGuildRewardRef>();
		foreach (EventRewardBookData.Pool item in XMLBook.instance.eventRewardBook.pools.lstPool)
		{
			_pools.Add(new SegmentedPoolRef(item));
		}
		foreach (EventRewardBookData.EventReward item2 in XMLBook.instance.eventRewardBook.rewards.lstReward)
		{
			if (item2.lstTier.Count >= 1)
			{
				_segmented_rewards.Add(new EventSegmentedRewardRef(item2));
			}
			else
			{
				_guild_rewards.Add(new EventGuildRewardRef(item2));
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static SegmentedPoolRef GetPoolByLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (SegmentedPoolRef pool in _pools)
		{
			if (pool.link.ToLower().Equals(link.ToLower()))
			{
				return pool;
			}
		}
		return null;
	}

	public static EventSegmentedRewardRef GetSegmentedRewardByLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (EventSegmentedRewardRef segmented_reward in _segmented_rewards)
		{
			if (segmented_reward.link.ToLower().Equals(link.ToLower()))
			{
				return segmented_reward;
			}
		}
		return null;
	}

	public static EventGuildRewardRef LookupGuildReward(string link)
	{
		foreach (EventGuildRewardRef guild_reward in _guild_rewards)
		{
			if (guild_reward.link.Equals(link))
			{
				return guild_reward;
			}
		}
		return null;
	}
}
