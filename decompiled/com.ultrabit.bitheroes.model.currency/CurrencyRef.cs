using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.currency;

[DebuggerDisplay("{name} (CurrencyRef)")]
public class CurrencyRef : ItemRef, IEquatable<CurrencyRef>, IComparable<CurrencyRef>
{
	public enum CURRENCY_TYPE
	{
		NONE,
		CURRENCY_GOLD,
		CURRENCY_CREDITS,
		CURRENCY_EXP,
		CURRENCY_ENERGY,
		CURRENCY_TICKETS,
		CURRENCY_POINTS,
		CURRENCY_HONOR,
		CURRENCY_SHARDS,
		CURRENCY_TOKENS,
		CURRENCY_BADGES,
		CURRENCY_GUILD_POINTS,
		CURRENCY_CHANGENAME,
		CURRENCY_SEALS,
		CURRENCY_MATERIAL
	}

	public const int CURRENCY_GOLD = 1;

	public const int CURRENCY_CREDITS = 2;

	public const int CURRENCY_EXP = 3;

	public const int CURRENCY_ENERGY = 4;

	public const int CURRENCY_TICKETS = 5;

	public const int CURRENCY_POINTS = 6;

	public const int CURRENCY_HONOR = 7;

	public const int CURRENCY_SHARDS = 8;

	public const int CURRENCY_TOKENS = 9;

	public const int CURRENCY_BADGES = 10;

	public const int CURRENCY_GUILD_POINTS = 11;

	public const int CURRENCY_CHANGENAME = 12;

	public const int CURRENCY_SEALS = 13;

	public const int CURRENCY_MATERIAL = 14;

	public CurrencyRef(int id, CurrencyBookData.Currency currencyData)
		: base(id, 3)
	{
	}

	public static string GetCurrencyName(int type)
	{
		string result = "???";
		CurrencyRef currencyRef = CurrencyBook.Lookup(type);
		if (currencyRef != null)
		{
			result = currencyRef.name;
		}
		return result;
	}

	public static string GetCurrencyDesc(int type)
	{
		CurrencyRef currencyRef = CurrencyBook.Lookup(type);
		if (currencyRef != null)
		{
			return currencyRef.desc;
		}
		return "???";
	}

	public bool Equals(CurrencyRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CurrencyRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
