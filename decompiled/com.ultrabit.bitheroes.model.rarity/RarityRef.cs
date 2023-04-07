using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.battle;

namespace com.ultrabit.bitheroes.model.rarity;

[DebuggerDisplay("{name} (RarityRef)")]
public class RarityRef : IEquatable<RarityRef>, IComparable<RarityRef>
{
	public const int RARITY_TYPE_GENERIC = 0;

	public const int RARITY_TYPE_COMMON = 1;

	public const int RARITY_TYPE_RARE = 2;

	public const int RARITY_TYPE_EPIC = 3;

	public const int RARITY_TYPE_LEGENDARY = 4;

	public const int RARITY_TYPE_SET = 5;

	public const int RARITY_TYPE_MYTHIC = 6;

	public const int RARITY_TYPE_ANCIENT = 7;

	public const int RARITY_TYPE_COSMETIC = 8;

	public const int RARITY_TYPE_COUNT = 9;

	private RarityBookData.Rarity rarityData;

	private int _id;

	private float _notifyDuration;

	public string coloredName => ConvertString(Language.GetString(rarityData.name));

	public int id => _id;

	public string link => rarityData.link;

	public string objectColor
	{
		get
		{
			if (rarityData.objectColorUnity == null)
			{
				return rarityData.textColor;
			}
			return rarityData.objectColorUnity;
		}
	}

	public uint objectColorUint => Util.colorUint(rarityData.objectColor);

	public string textColor => rarityData.textColor;

	public uint textColorUint => Util.colorUint(rarityData.textColor);

	public float overlayAlpha => rarityData.overlayAlpha;

	public int augmentMax => rarityData.augmentMax;

	public float captureChancePerc => rarityData.captureChancePerc;

	public float captureSuccessPerc => rarityData.captureSuccessPerc;

	public ServiceRef captureChanceServiceRef => ServiceBook.Lookup(rarityData.captureChanceServiceID);

	public ServiceRef captureGuaranteeServiceRef => ServiceBook.Lookup(rarityData.captureGuaranteeServiceID);

	public ServiceRef merchantServiceRef => ServiceBook.Lookup(rarityData.merchantServiceID);

	public bool filtered => rarityData.filtered;

	public bool filterDefault => rarityData.filterDefault;

	public bool exchangeable => rarityData.exchangeable;

	public float notifyDuration => _notifyDuration;

	public string name => rarityData.name;

	public RarityRef(int id, RarityBookData.Rarity rarityData)
	{
		_id = id;
		this.rarityData = rarityData;
		_notifyDuration = ((rarityData.notifyDuration != null) ? Util.ParseFloat(rarityData.notifyDuration) : 1f);
	}

	public string ConvertString(string theString)
	{
		return "<color=#" + rarityData.textColor + ">" + theString + "</color>";
	}

	public string getCaptureSuccessString()
	{
		string text = rarityData.captureSuccessPerc.ToString();
		if (rarityData.captureSuccessPerc == 100f)
		{
			return Util.colorString(text, BattleText.COLOR_GREEN);
		}
		if (rarityData.captureSuccessPerc >= 30f)
		{
			return Util.colorString(text, BattleText.COLOR_YELLOW);
		}
		if (rarityData.captureSuccessPerc >= 15f)
		{
			return Util.colorString(text, BattleText.COLOR_ORANGE);
		}
		return Util.colorString(text, BattleText.COLOR_RED);
	}

	public bool Equals(RarityRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(RarityRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}

	public override int GetHashCode()
	{
		return 1969571243 + _id.GetHashCode();
	}
}
