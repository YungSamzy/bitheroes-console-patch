using System;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlTierDifficultyRef : BaseRef
{
	private BrawlDifficultyRef _difficultyRef;

	private BrawlRef _brawlRef;

	private BrawlTierRef _tierRef;

	public BrawlDifficultyRef difficultyRef => _difficultyRef;

	public BrawlRef brawlRef => _brawlRef;

	public BrawlTierRef tierRef => _tierRef;

	public BrawlTierDifficultyRef(int id, BrawlDifficultyRef difficultyRef, BrawlBookData.Difficulty difficultyData)
		: base(id)
	{
		_difficultyRef = difficultyRef;
		LoadDetails(difficultyData);
	}

	public void setBrawlRef(BrawlRef brawlRef)
	{
		_brawlRef = brawlRef;
	}

	public void setTierRef(BrawlTierRef tierRef)
	{
		_tierRef = tierRef;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
