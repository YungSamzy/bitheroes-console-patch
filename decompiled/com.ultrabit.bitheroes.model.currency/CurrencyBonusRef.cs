using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.currency;

[DebuggerDisplay("{name} (CurrencyBonusRef)")]
public class CurrencyBonusRef : BaseRef, IEquatable<CurrencyBonusRef>, IComparable<CurrencyBonusRef>
{
	private List<GameModifier> _modifiers;

	private int _multiplier;

	private int _rolls;

	public int multiplier => _multiplier;

	public int rolls => _rolls;

	public List<GameModifier> modifiers => _modifiers;

	public CurrencyBonusRef(int id, BaseEventBookData.Bonus bonusData)
		: base(id)
	{
		_multiplier = bonusData.multiplier;
		_rolls = bonusData.rolls;
		_modifiers = GameModifier.GetGameModifierFromData(bonusData.modifiers, bonusData.lstModifier);
		LoadDetails(bonusData);
	}

	public static Dictionary<int, CurrencyBonusRef> FromXMLData(BaseEventBookData.Bonuses bonuses)
	{
		Dictionary<int, CurrencyBonusRef> dictionary = new Dictionary<int, CurrencyBonusRef>();
		foreach (BaseEventBookData.Bonus lstBonu in bonuses.lstBonus)
		{
			dictionary.Add(lstBonu.id, new CurrencyBonusRef(lstBonu.id, lstBonu));
		}
		return dictionary;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(CurrencyBonusRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CurrencyBonusRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
