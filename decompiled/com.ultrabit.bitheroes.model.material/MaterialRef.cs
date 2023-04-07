using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.material;

[DebuggerDisplay("{name} (MaterialRef)")]
public class MaterialRef : ItemRef, IEquatable<MaterialRef>, IComparable<MaterialRef>
{
	public MaterialRef(int id, MaterialBookData.Material item)
		: base(id, 2)
	{
		LoadDetails(item);
	}

	public bool Equals(MaterialRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(MaterialRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
