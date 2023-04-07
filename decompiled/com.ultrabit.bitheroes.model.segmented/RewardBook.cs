using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.segmented;

public class RewardBook
{
	private static List<SegmentedPoolRef> _pools;

	private static List<SegmentedRewardRef> _rewards;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_pools = new List<SegmentedPoolRef>();
		_rewards = new List<SegmentedRewardRef>();
		foreach (RewardBookData.Pool item in XMLBook.instance.rewardBook.pools.lstPool)
		{
			_pools.Add(new SegmentedPoolRef(item));
		}
		foreach (RewardBookData.Reward item2 in XMLBook.instance.rewardBook.rewards.lstReward)
		{
			_rewards.Add(new SegmentedRewardRef(item2));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static SegmentedRewardRef GetSegmentedRewardByLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (SegmentedRewardRef reward in _rewards)
		{
			if (reward.link.ToLower().Equals(link.ToLower()))
			{
				return reward;
			}
		}
		return null;
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
}
