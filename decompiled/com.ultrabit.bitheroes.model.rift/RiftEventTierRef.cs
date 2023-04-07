using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.rift;

[DebuggerDisplay("{tierName} (RiftEventTierRef)")]
public class RiftEventTierRef : BaseRef, IEquatable<RiftEventTierRef>, IComparable<RiftEventTierRef>
{
	private RarityRef _rarityRef;

	private int _difficulty;

	private int _tierName;

	public int difficulty => _difficulty;

	public RarityRef rarityRef => _rarityRef;

	public int tierName => _tierName;

	public RiftEventTierRef(int id, BaseEventBookData.Tier tierData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(tierData.rarity);
		_difficulty = tierData.difficulty;
		_tierName = Util.GetIntFromStringProperty(tierData.name);
		base.LoadDetails(tierData);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(RiftEventTierRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(RiftEventTierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
