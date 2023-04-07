using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.augment;

[DebuggerDisplay("{name} (AugmentRef)")]
public class AugmentRef : ItemRef, IEquatable<AugmentRef>, IComparable<AugmentRef>
{
	public new string name { get; private set; }

	public int mods { get; }

	public AugmentTypeRef typeRef { get; private set; }

	public List<AugmentModifierRef> modifiers { get; private set; }

	public AugmentRef(int id, AugmentBookData.Augment augmentData)
		: base(id, 15)
	{
		name = augmentData.name;
		typeRef = AugmentBook.LookupTypeLink(augmentData.type);
		modifiers = typeRef.getRarityModifiers(RarityBook.Lookup(augmentData.rarity).id);
	}

	public bool Equals(AugmentRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((ItemRef)other);
	}

	public int CompareTo(AugmentRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((ItemRef)other);
	}
}
