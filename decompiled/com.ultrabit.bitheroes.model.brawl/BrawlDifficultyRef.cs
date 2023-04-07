using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlDifficultyRef : BaseRef
{
	private int _seals;

	private RarityRef _rarity;

	private List<GameModifier> _modifiers;

	public string coloredName
	{
		get
		{
			if (_rarity != null)
			{
				return _rarity.ConvertString(name);
			}
			return name;
		}
	}

	public new string link => _link;

	public int seals => _seals;

	public List<GameModifier> modifiers => _modifiers;

	public BrawlDifficultyRef(int id, BrawlBookData.Difficulty difficultyData)
		: base(id)
	{
		_seals = difficultyData.seals;
		_rarity = RarityBook.Lookup(difficultyData.rarity);
		_modifiers = GameModifier.GetGameModifierFromData(difficultyData.modifiers, difficultyData.lstModifier);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
