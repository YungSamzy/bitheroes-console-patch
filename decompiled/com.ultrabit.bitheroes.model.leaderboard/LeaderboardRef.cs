using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.leaderboard;

[DebuggerDisplay("{name} (LeaderboardRef)")]
public class LeaderboardRef : BaseRef, IEquatable<LeaderboardRef>, IComparable<LeaderboardRef>
{
	private string _value;

	public string value => Language.GetString(_value);

	public LeaderboardRef(int id, LeaderboardBookData.Leaderboard leaderboard)
		: base(id)
	{
		_value = leaderboard.value;
		LoadDetails(leaderboard);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(LeaderboardRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(LeaderboardRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
