using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonMerchantRef)")]
public class DungeonMerchantRef : DungeonObjectRef, IEquatable<DungeonMerchantRef>, IComparable<DungeonMerchantRef>
{
	public DungeonMerchantRef(int id, DungeonBookData.Merchant merchantData)
		: base(id, merchantData)
	{
	}

	public bool Equals(DungeonMerchantRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((DungeonObjectRef)other);
	}

	public int CompareTo(DungeonMerchantRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((DungeonObjectRef)other);
	}
}
