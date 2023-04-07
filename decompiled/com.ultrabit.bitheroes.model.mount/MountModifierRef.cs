using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.rarity;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.mount;

[DebuggerDisplay("{name} (MountModifierRef)")]
public class MountModifierRef : BaseRef, IEquatable<MountModifierRef>, IComparable<MountModifierRef>
{
	private RarityRef _rarityRef;

	private List<GameModifier> _modifiers;

	public RarityRef rarityRef => _rarityRef;

	public List<GameModifier> modifiers => _modifiers;

	public MountModifierRef(int id, RarityRef rarityRef, List<GameModifier> modifiers)
		: base(id)
	{
		_rarityRef = rarityRef;
		_modifiers = modifiers;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(MountModifierRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(MountModifierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
