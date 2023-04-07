using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.promo;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.news;

[DebuggerDisplay("{name} (NewsRef)")]
public class NewsRef : PromoRef, IEquatable<NewsRef>, IComparable<NewsRef>
{
	public const int TYPE_NONE = 0;

	public const int TYPE_URL = 1;

	public const int TYPE_ITEM = 2;

	public const int TYPE_SHOP = 3;

	public const int TYPE_PVP = 4;

	public const int TYPE_ZONE = 5;

	public const int TYPE_RAID = 6;

	public const int TYPE_FUSION = 7;

	public const int TYPE_RIFT = 8;

	public const int TYPE_GAUNTLET = 9;

	public const int TYPE_GVG = 10;

	public const int TYPE_GUILD = 11;

	public const int TYPE_FAMILIAR_STABLE = 12;

	public const int TYPE_INVASION = 13;

	public const int TYPE_ENCHANT = 14;

	public const int TYPE_BRAWL = 15;

	public const int TYPE_OFFERWALL = 16;

	public const int TYPE_FISHING = 17;

	public const int TYPE_GVE = 18;

	public const int TYPE_AUGMENT = 19;

	public const int TYPE_PLAYER_VOTING = 20;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		[""] = 0,
		["url"] = 1,
		["none"] = 0,
		["item"] = 2,
		["shop"] = 3,
		["pvp"] = 4,
		["zone"] = 5,
		["raid"] = 6,
		["fusion"] = 7,
		["rift"] = 8,
		["gauntlet"] = 9,
		["gvg"] = 10,
		["guild"] = 11,
		["familiarstable"] = 12,
		["invasion"] = 13,
		["enchant"] = 14,
		["brawl"] = 15,
		["offerwall"] = 16,
		["fishing"] = 17,
		["gve"] = 18,
		["augment"] = 19,
		["playervoting"] = 20
	};

	private int _type;

	private string _value;

	private PromoRef _promoRef;

	public int type => _type;

	public string value => _value;

	public PromoRef promoRef => _promoRef;

	public NewsRef(int id, ShopBookData.Promo promo)
		: base(id, promo)
	{
		_promoRef = new PromoRef(id, promo);
		_type = getType(promo.type);
		_value = promo.value;
	}

	public static int getType(string type)
	{
		if (type == null || type.Equals(""))
		{
			return 0;
		}
		if (!TYPES.ContainsKey(type))
		{
			D.LogWarning("all", "NewsRef::getType -> not found key: " + type);
			return 0;
		}
		return TYPES[type.ToLowerInvariant()];
	}

	public bool Equals(NewsRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(NewsRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
