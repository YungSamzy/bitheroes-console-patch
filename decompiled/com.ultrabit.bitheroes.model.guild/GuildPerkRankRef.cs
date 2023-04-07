using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{name} (GuildPerkRankRef)")]
public class GuildPerkRankRef : BaseRef, IEquatable<GuildPerkRankRef>, IComparable<GuildPerkRankRef>
{
	private List<GameModifier> _modifiers;

	private GuildPerkRef _perkRef;

	private int _cost;

	public int cost => _cost;

	public List<GameModifier> modifiers => _modifiers;

	public GuildPerkRankRef(int id, GuildBookData.Rank rankData)
		: base(id)
	{
		_modifiers = GameModifier.GetGameModifierFromData(rankData.modifiers, rankData.lstModifier);
		_cost = rankData.cost;
		LoadDetails(rankData);
	}

	public GameModifier getFirstModifier()
	{
		if (_modifiers.Count <= 0)
		{
			return null;
		}
		return _modifiers[0];
	}

	public void SetPerkRef(GuildPerkRef perkRef)
	{
		_perkRef = perkRef;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(GuildPerkRankRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GuildPerkRankRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
