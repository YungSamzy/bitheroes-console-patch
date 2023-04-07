using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.augment;

[DebuggerDisplay("{name} (AugmentModifierRef)")]
public class AugmentModifierRef : BaseRef, IEquatable<AugmentModifierRef>, IComparable<AugmentModifierRef>
{
	private RarityRef _rarityRef;

	private List<AugmentModifierRankRef> _ranks;

	public RarityRef rarityRef => _rarityRef;

	public List<AugmentModifierRankRef> ranks => _ranks;

	public AugmentModifierRef(int id, AugmentBookData.Modifier modifierData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(modifierData.rarity);
		_ranks = new List<AugmentModifierRankRef>();
		foreach (AugmentBookData.Rank item in modifierData.lstRank)
		{
			_ranks.Add(new AugmentModifierRankRef(item.id, item));
		}
		LoadDetails(modifierData);
	}

	public AugmentModifierRankRef getRank(int id)
	{
		if (id < 0 || id >= _ranks.Count)
		{
			return null;
		}
		return _ranks[id];
	}

	public AugmentModifierRankRef getFirstModifier()
	{
		foreach (AugmentModifierRankRef rank in _ranks)
		{
			if (rank != null)
			{
				return rank;
			}
		}
		return null;
	}

	public List<GameModifier> getRankModifiers(int rank)
	{
		AugmentModifierRankRef augmentModifierRankRef = getRank(rank);
		if (augmentModifierRankRef == null)
		{
			augmentModifierRankRef = getFirstModifier();
		}
		if (augmentModifierRankRef == null)
		{
			return new List<GameModifier>();
		}
		return augmentModifierRankRef.modifiers;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(AugmentModifierRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AugmentModifierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
