using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.enchant;

[DebuggerDisplay("{name} (EnchantModifierRef)")]
public class EnchantModifierRef : BaseRef, IEquatable<EnchantModifierRef>, IComparable<EnchantModifierRef>
{
	private RarityRef _rarityRef;

	private List<GameModifier> _modifiers;

	public RarityRef rarityRef => _rarityRef;

	public List<GameModifier> modifiers => _modifiers;

	public EnchantModifierRef(int id, EnchantBookData.Modifier modifierData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(modifierData.rarity);
		_modifiers = new List<GameModifier>();
		foreach (GameModifierData item in modifierData.lstModifier)
		{
			_modifiers.Add(new GameModifier(item));
		}
		LoadDetails(modifierData);
	}

	public bool Equals(EnchantModifierRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EnchantModifierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
