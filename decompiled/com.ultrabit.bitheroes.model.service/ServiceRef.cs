using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.service;

[DebuggerDisplay("{name} (ServiceRef)")]
public class ServiceRef : ItemRef, IEquatable<ServiceRef>, IComparable<ServiceRef>
{
	public const int SERVICE_TYPE_GOLD = 1;

	public const int SERVICE_TYPE_ENERGY = 2;

	public const int SERVICE_TYPE_TICKETS = 3;

	public const int SERVICE_TYPE_STAT_RESET = 4;

	public const int SERVICE_TYPE_CUSTOMIZE = 5;

	public const int SERVICE_TYPE_SHARDS = 6;

	public const int SERVICE_TYPE_GUILD_CREATE = 7;

	public const int SERVICE_TYPE_CAPTURE_FAMILIAR = 8;

	public const int SERVICE_TYPE_MERCHANT = 9;

	public const int SERVICE_TYPE_TOKENS = 10;

	public const int SERVICE_TYPE_BADGES = 11;

	public const int SERVICE_TYPE_CHANGENAME = 12;

	public const int SERVICE_TYPE_SEALS = 15;

	private static Dictionary<string, int> SERVICE_TYPES = new Dictionary<string, int>
	{
		["gold"] = 1,
		["energy"] = 2,
		["tickets"] = 3,
		["statreset"] = 4,
		["customize"] = 5,
		["shards"] = 6,
		["guildcreate"] = 7,
		["capturefamiliar"] = 8,
		["merchant"] = 9,
		["tokens"] = 10,
		["badges"] = 11,
		["changename"] = 12,
		["seals"] = 15
	};

	private int _serviceType;

	private string _value;

	private bool _visible;

	public int serviceType => _serviceType;

	public string value => _value;

	public bool visible => _visible;

	public ServiceRef(int id, ServiceBookData.Service serviceData)
		: base(id, 5)
	{
		_serviceType = GetServiceType(serviceData.type);
		_value = serviceData.value.ToString();
		_visible = Util.GetBoolFromStringProperty(serviceData.visible, defaultValue: true);
		LoadDetails(serviceData);
	}

	public int getCurrencyID()
	{
		return serviceType switch
		{
			2 => 4, 
			3 => 5, 
			12 => 12, 
			6 => 8, 
			10 => 9, 
			11 => 10, 
			15 => 13, 
			_ => -1, 
		};
	}

	public static int GetServiceType(string type)
	{
		if (type != null && SERVICE_TYPES.ContainsKey(type.ToLowerInvariant()))
		{
			return SERVICE_TYPES[type.ToLowerInvariant()];
		}
		return -1;
	}

	public static string GetServiceName(int type)
	{
		return Language.GetString("service_type_" + type + "_name");
	}

	public int GetServiceCost()
	{
		if (base.costCredits <= 0)
		{
			return base.costGold;
		}
		return base.costCredits;
	}

	public CurrencyRef GetPurchaseCurrencyRef()
	{
		return CurrencyBook.Lookup((base.costCredits <= 0) ? 1 : 2);
	}

	public bool Equals(ServiceRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(ServiceRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
