using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.fish;

[DebuggerDisplay("{name} (FishRef)")]
public class FishRef : ItemRef, IEquatable<FishRef>, IComparable<FishRef>
{
	public FishRef(int id, FishBookData.Fish data)
		: base(id, 12)
	{
		LoadDetails(data);
	}

	public bool Equals(FishRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(FishRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
