using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.equipment;

[DebuggerDisplay("{id} (EquipmentSubtypeRef)")]
public class EquipmentSubtypeRef : BaseRef, IEquatable<EquipmentSubtypeRef>, IComparable<EquipmentSubtypeRef>
{
	private bool _tooltip;

	public bool tooltip => _tooltip;

	public EquipmentSubtypeRef(int id, EquipmentBookData.Subtype subtype)
		: base(id)
	{
		_tooltip = Util.GetBoolFromStringProperty(subtype.tooltip);
		base.LoadDetails(subtype);
	}

	public bool Equals(EquipmentSubtypeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(EquipmentSubtypeRef other)
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
