using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.enchant;

[DebuggerDisplay("{id} (EnchantSlotRef)")]
public class EnchantSlotRef : BaseRef, IEquatable<EnchantSlotRef>, IComparable<EnchantSlotRef>
{
	private int _levelReq;

	public int levelReq => _levelReq;

	public EnchantSlotRef(int id, int levelReq)
		: base(id)
	{
		_levelReq = levelReq;
	}

	public bool Equals(EnchantSlotRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EnchantSlotRef other)
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
