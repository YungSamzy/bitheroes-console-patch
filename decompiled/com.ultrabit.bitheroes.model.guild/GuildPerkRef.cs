using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{name} (GuildPerkRef)")]
public class GuildPerkRef : BaseRef, IEquatable<GuildPerkRef>, IComparable<GuildPerkRef>
{
	private List<GuildPerkRankRef> _ranks;

	public GuildPerkRef(int id, GuildBookData.Perk itemData)
		: base(id)
	{
		_ranks = new List<GuildPerkRankRef>();
		foreach (GuildBookData.Rank item in itemData.lstRank)
		{
			GuildPerkRankRef guildPerkRankRef = new GuildPerkRankRef(item.id, item);
			guildPerkRankRef.SetPerkRef(this);
			_ranks.Add(guildPerkRankRef);
		}
	}

	public GuildPerkRankRef getPerkRank(int rank)
	{
		foreach (GuildPerkRankRef rank2 in _ranks)
		{
			if (rank2 != null && rank2.id == rank)
			{
				return rank2;
			}
		}
		return null;
	}

	public override Sprite GetSpriteIcon()
	{
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GUILD_PERK_ICON, icon);
	}

	public bool Equals(GuildPerkRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GuildPerkRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
