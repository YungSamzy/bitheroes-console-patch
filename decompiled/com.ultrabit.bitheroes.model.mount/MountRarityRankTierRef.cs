using System;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.mount;

public class MountRarityRankTierRef : BaseRef
{
	private int _stats;

	public int stats => _stats;

	public MountRarityRankTierRef(int id, MountBookData.Tier tierData)
		: base(id)
	{
		_stats = tierData.stats;
		LoadDetails(tierData);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
