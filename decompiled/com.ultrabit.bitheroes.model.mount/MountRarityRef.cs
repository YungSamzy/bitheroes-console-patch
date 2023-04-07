using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.mount;

[DebuggerDisplay("{name} (MountRarityRef)")]
public class MountRarityRef : IEquatable<MountRarityRef>, IComparable<MountRarityRef>
{
	private RarityRef _rarityRef;

	private List<MountRarityRankRef> _ranks;

	private List<GameModifier> _modifiers;

	private MountBookData.Rarity _rarityData;

	public int rankMax
	{
		get
		{
			int num = 0;
			foreach (MountRarityRankRef rank in _ranks)
			{
				if (rank.id > num)
				{
					num = rank.id;
				}
			}
			return num;
		}
	}

	private string name => _rarityData.name;

	public RarityRef rarityRef => _rarityRef;

	public float modsMin => _rarityData.modsMin;

	public float modsMax => _rarityData.modsMax;

	public List<GameModifier> modifiers => _modifiers;

	public MountRarityRef(MountBookData.Rarity rarityData)
	{
		_rarityData = rarityData;
		_rarityRef = RarityBook.Lookup(rarityData.link);
		_ranks = new List<MountRarityRankRef>();
		foreach (MountBookData.Rank item in rarityData.lstRank)
		{
			_ranks.Add(new MountRarityRankRef(item.id, item));
		}
		_modifiers = GameModifier.GetGameModifierFromData(rarityData.modifiers, rarityData.lstModifier);
	}

	public int getStats(int rank, int tier)
	{
		return getRankRef(rank)?.getStats(tier) ?? 0;
	}

	public MountRarityRankRef getRankRef(int rank)
	{
		foreach (MountRarityRankRef rank2 in _ranks)
		{
			if (rank2.id == rank)
			{
				return rank2;
			}
		}
		return null;
	}

	public bool Equals(MountRarityRef other)
	{
		if (other == null)
		{
			return false;
		}
		return _rarityData.id.Equals(other._rarityData.id);
	}

	public int CompareTo(MountRarityRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return _rarityData.id.CompareTo(other._rarityData.id);
	}
}
