using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlTierRef : BaseRef
{
	private int _zoneCompleteReq;

	private List<BrawlTierDifficultyRef> _difficulties;

	private BrawlRef _brawlRef;

	public List<BrawlTierDifficultyRef> difficulties => _difficulties;

	public BrawlRef brawlRef => _brawlRef;

	public BrawlTierRef(int id, BrawlBookData.Tier tierData)
		: base(id)
	{
		_zoneCompleteReq = tierData.zoneCompleteReq;
		_difficulties = new List<BrawlTierDifficultyRef>();
		foreach (BrawlBookData.Difficulty item in tierData.lstDifficulty)
		{
			BrawlDifficultyRef brawlDifficultyRef = BrawlBook.LookupDifficultyLink(item.link);
			BrawlTierDifficultyRef brawlTierDifficultyRef = new BrawlTierDifficultyRef(brawlDifficultyRef.id, brawlDifficultyRef, item);
			brawlTierDifficultyRef.setTierRef(this);
			brawlTierDifficultyRef.setBrawlRef(_brawlRef);
			_difficulties.Add(brawlTierDifficultyRef);
		}
		LoadDetails(tierData);
	}

	public void setBrawlRef(BrawlRef brawlRef)
	{
		_brawlRef = brawlRef;
		foreach (BrawlTierDifficultyRef difficulty in _difficulties)
		{
			difficulty.setBrawlRef(brawlRef);
		}
	}

	public BrawlTierDifficultyRef getDifficulty(int id)
	{
		foreach (BrawlTierDifficultyRef difficulty in _difficulties)
		{
			if (difficulty.id == id)
			{
				return difficulty;
			}
		}
		return null;
	}

	public bool requirementsMet()
	{
		if (GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID() < _zoneCompleteReq)
		{
			return false;
		}
		return true;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
