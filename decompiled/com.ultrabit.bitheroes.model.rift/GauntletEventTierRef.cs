using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.rift;

[DebuggerDisplay("{tierName} (GauntletEventTierRef)")]
public class GauntletEventTierRef : BaseRef, IEquatable<GauntletEventTierRef>, IComparable<GauntletEventTierRef>
{
	private RarityRef _rarityRef;

	private int _difficulty;

	private int _tierName;

	public int difficulty => _difficulty;

	public RarityRef rarityRef => _rarityRef;

	public int tierName => _tierName;

	public GauntletEventTierRef(int id, BaseEventBookData.Tier tierData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(tierData.rarity);
		_difficulty = tierData.difficulty;
		_tierName = Util.GetIntFromStringProperty(tierData.name);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(GauntletEventTierRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GauntletEventTierRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
