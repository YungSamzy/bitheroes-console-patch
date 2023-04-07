using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.augment;

[DebuggerDisplay("{id} (AugmentSlotRef)")]
public class AugmentSlotRef : BaseRef, IEquatable<AugmentSlotRef>, IComparable<AugmentSlotRef>
{
	private AugmentTypeRef _typeRef;

	private int _levelReq;

	public int levelReq => _levelReq;

	public AugmentTypeRef typeRef => _typeRef;

	public AugmentSlotRef(int id, AugmentBookData.Slot slotData)
		: base(id)
	{
		_typeRef = AugmentBook.LookupTypeLink(slotData.type);
		_levelReq = slotData.levelReq;
		LoadDetails(slotData);
	}

	public bool Equals(AugmentSlotRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(AugmentSlotRef other)
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
