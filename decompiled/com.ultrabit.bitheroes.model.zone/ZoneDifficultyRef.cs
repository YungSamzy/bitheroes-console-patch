using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.zone;

[DebuggerDisplay("{difficultyName} (ZoneDifficultyRef)")]
public class ZoneDifficultyRef : BaseRef, IEquatable<ZoneDifficultyRef>, IComparable<ZoneDifficultyRef>
{
	private string _difficultyName;

	private string difficultyName => _difficultyName;

	public ZoneDifficultyRef(int id, ZoneBookData.DifficultyMode difficultyData)
		: base(id)
	{
		_difficultyName = difficultyData.name;
		LoadDetails(difficultyData);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(ZoneDifficultyRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(ZoneDifficultyRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
