using System;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.character.achievements;

public class CharacterAchievementRef : BaseRef
{
	private RarityRef _rarityRef;

	private string _reward;

	private string _category;

	private int _amount;

	public string category => _category;

	public int amount => _amount;

	public RarityRef rarityRef => _rarityRef;

	public CharacterAchievementRef(int id, CharacterAchievementBookData.Achievement achievementData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(achievementData.rarity);
		_reward = achievementData.reward;
		_amount = -1;
		_category = achievementData.category;
		if (!string.IsNullOrEmpty(achievementData.amount))
		{
			_amount = int.Parse(achievementData.amount);
		}
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
