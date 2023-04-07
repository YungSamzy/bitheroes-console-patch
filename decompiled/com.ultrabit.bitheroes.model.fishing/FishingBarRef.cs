using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.fishing;

[DebuggerDisplay("{name} (FishingBarRef)")]
public class FishingBarRef : BaseRef, IEquatable<FishingBarRef>, IComparable<FishingBarRef>
{
	private List<FishingBarChanceRef> _chances;

	public List<FishingBarChanceRef> chances => _chances;

	public FishingBarRef(int id, FishingBookData.Bar data)
		: base(id)
	{
		if (data.lstChance != null)
		{
			_chances = new List<FishingBarChanceRef>();
			foreach (FishingBookData.Chance item in data.lstChance)
			{
				_chances.Add(new FishingBarChanceRef(_chances.Count, item));
			}
		}
		LoadDetails(data);
	}

	public FishingBarChanceRef getChance(int id)
	{
		foreach (FishingBarChanceRef chance in _chances)
		{
			if (chance.id == id)
			{
				return chance;
			}
		}
		return null;
	}

	public bool Equals(FishingBarRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(FishingBarRef other)
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
