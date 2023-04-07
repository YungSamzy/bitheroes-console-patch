using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.invasion;

[DebuggerDisplay("{points} (InvasionEventTierRef)")]
public class InvasionEventTierRef : BaseRef, IEquatable<InvasionEventTierRef>, IComparable<InvasionEventTierRef>
{
	private EventRewards _rankRewards;

	private EventRewards _pointRewards;

	private long _points;

	public float points => _points;

	public EventRewards rankRewards => _rankRewards;

	public EventRewards pointRewards => _pointRewards;

	public InvasionEventTierRef(int id, BaseEventBookData.InvasionTierData tierData)
		: base(id)
	{
		List<EventRewardRef> list = new List<EventRewardRef>();
		foreach (BaseEventBookData.Reward item in tierData.rewards.ranks.lstReward)
		{
			list.Add(new EventRewardRef(item));
		}
		_rankRewards = new EventRewards(list);
		list = new List<EventRewardRef>();
		foreach (BaseEventBookData.Reward item2 in tierData.rewards.points.lstReward)
		{
			list.Add(new EventRewardRef(item2));
		}
		_pointRewards = new EventRewards(list);
		_points = tierData.points;
		LoadDetails(tierData);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(InvasionEventTierRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (points.Equals(other.points) && rankRewards.Equals(other.rankRewards))
		{
			return pointRewards.Equals(other.pointRewards);
		}
		return false;
	}

	public int CompareTo(InvasionEventTierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return points.CompareTo(other.points);
	}
}
