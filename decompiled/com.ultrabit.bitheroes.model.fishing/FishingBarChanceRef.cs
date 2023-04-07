using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.fishing;

[DebuggerDisplay("{name} (FishingBarChanceRef)")]
public class FishingBarChanceRef : BaseRef, IEquatable<FishingBarChanceRef>, IComparable<FishingBarChanceRef>
{
	private float _size;

	private float _perc;

	private string _color;

	public float size => _size;

	public float perc => _perc;

	public string color => _color;

	public FishingBarChanceRef(int id, FishingBookData.Chance data)
		: base(id)
	{
		_size = data.size;
		_perc = data.chance;
		_color = data.color;
		LoadDetails(data);
	}

	public string getPercString()
	{
		return Util.colorString((_perc * 100f).ToString(), color);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(FishingBarChanceRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(FishingBarChanceRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = base.id.CompareTo(other.id);
		if (num == 0)
		{
			num = size.CompareTo(other.size);
			if (num == 0)
			{
				return perc.CompareTo(other.perc);
			}
		}
		return num;
	}
}
