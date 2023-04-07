using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.augment;

[DebuggerDisplay("{name} (AugmentModifierRankRef)")]
public class AugmentModifierRankRef : BaseRef, IEquatable<AugmentModifierRankRef>, IComparable<AugmentModifierRankRef>
{
	private List<GameModifier> _modifiers;

	public List<GameModifier> modifiers => _modifiers;

	public AugmentModifierRankRef(int id, AugmentBookData.Rank rankData)
		: base(id)
	{
		_modifiers = GameModifier.GetGameModifierFromData(rankData.modifiers, rankData.lstModifier);
		LoadDetails(rankData);
	}

	public bool Equals(AugmentModifierRankRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AugmentModifierRankRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
