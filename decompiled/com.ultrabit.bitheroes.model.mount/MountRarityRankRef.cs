using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.mount;

[DebuggerDisplay("{name} (MountRarityRankRef)")]
public class MountRarityRankRef : BaseRef, IEquatable<MountRarityRankRef>, IComparable<MountRarityRankRef>
{
	private List<MountRarityRankTierRef> _tiers;

	private string _upgrade;

	public CraftUpgradeRef upgradeRef => CraftBook.LookupUpgradeLink(_upgrade);

	public MountRarityRankRef(int id, MountBookData.Rank rankData)
		: base(id)
	{
		_tiers = new List<MountRarityRankTierRef>();
		foreach (MountBookData.Tier item in rankData.lstTier)
		{
			_tiers.Add(new MountRarityRankTierRef(item.id, item));
		}
		_upgrade = rankData.upgrade;
		LoadDetails(rankData);
	}

	public MountRarityRankTierRef getTierRef(int tier)
	{
		foreach (MountRarityRankTierRef tier2 in _tiers)
		{
			if (tier2 != null && tier2.id == tier)
			{
				return tier2;
			}
		}
		return null;
	}

	public int getStats(int tier)
	{
		return getTierRef(tier)?.stats ?? 0;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(MountRarityRankRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(MountRarityRankRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
