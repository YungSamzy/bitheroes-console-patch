using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{name} (GuildItemRef)")]
public class GuildItemRef : ItemRef, IEquatable<GuildItemRef>, IComparable<GuildItemRef>
{
	public const int GUILD_ITEM_TYPE_THRONE = 1;

	public const int GUILD_ITEM_TYPE_LEFT_ROOM = 2;

	public const int GUILD_ITEM_TYPE_RIGHT_ROOM = 3;

	private static Dictionary<string, int> GUILD_ITEM_TYPES = new Dictionary<string, int>
	{
		["throne"] = 1,
		["leftroom"] = 2,
		["rightroom"] = 3
	};

	private int _guildItemType;

	private List<string> _values;

	public int guildItemType => _guildItemType;

	public List<string> values => _values;

	public GuildItemRef(int id, GuildBookData.Item itemData)
		: base(id, 10)
	{
		_guildItemType = getGuildItemType(itemData.type);
		_values = Util.GetStringListFromStringProperty(itemData.values);
	}

	public bool Equals(GuildItemRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GuildItemRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public static int getGuildItemType(string type)
	{
		return GUILD_ITEM_TYPES[type.ToLowerInvariant()];
	}

	public static string getGuildTypeName(int type)
	{
		return Language.GetString("guild_item_type_" + type + "_name");
	}

	public override Sprite GetSpriteIcon()
	{
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GUILD_ITEM_ICON, icon);
	}
}
