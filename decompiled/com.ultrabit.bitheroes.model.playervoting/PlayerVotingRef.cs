using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.playervoting;

[DebuggerDisplay("{name} (PlayerVotingRef)")]
public class PlayerVotingRef : BaseRef, IEquatable<PlayerVotingRef>, IComparable<PlayerVotingRef>
{
	private string _value;

	private int _maxVotesPerUser;

	public List<CandidateRef> candidates = new List<CandidateRef>();

	public string value => _value;

	public int maxVotesPerUser
	{
		get
		{
			return _maxVotesPerUser;
		}
		set
		{
			_maxVotesPerUser = value;
		}
	}

	public PlayerVotingRef(int id, string value)
		: base(id)
	{
		_value = value;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(PlayerVotingRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(PlayerVotingRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
