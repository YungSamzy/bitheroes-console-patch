using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{name} (GuildHallCosmeticRef)")]
public class GuildHallCosmeticRef : BaseRef, IEquatable<GuildHallCosmeticRef>, IComparable<GuildHallCosmeticRef>
{
	private const string GENERIC_ICON = "generic.png";

	private GuildHallCosmeticTypeRef _typeRef;

	private int _frame;

	private bool _display;

	private int _guildLvlReq;

	private string _parentType;

	private List<string> _objects;

	private GuildHallCosmeticRef _parentRef;

	public GuildHallCosmeticRef parentRef
	{
		get
		{
			if (_parentRef != null)
			{
				return _parentRef;
			}
			if (_parentType != null)
			{
				GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = GuildHallBook.LookupCosmeticType(_parentType);
				if (guildHallCosmeticTypeRef != null)
				{
					GuildHallCosmeticRef guildHallCosmeticRef = GuildHallBook.LookupCosmetic(base.id, guildHallCosmeticTypeRef.id);
					if (guildHallCosmeticRef != null)
					{
						_parentRef = guildHallCosmeticRef;
						return _parentRef;
					}
				}
			}
			return null;
		}
	}

	public override string name
	{
		get
		{
			if (parentRef == null)
			{
				return base.name;
			}
			return parentRef.name;
		}
	}

	public override string desc
	{
		get
		{
			if (parentRef == null)
			{
				return base.desc;
			}
			return parentRef.desc;
		}
	}

	public int type => _typeRef.id;

	public GuildHallCosmeticTypeRef typeRef => _typeRef;

	public int frame => _frame;

	public bool display => _display;

	public int guildLvlReq => _guildLvlReq;

	public List<string> objects => _objects;

	public void Init()
	{
		_guildLvlReq = ((parentRef != null) ? parentRef.guildLvlReq : _guildLvlReq);
	}

	public GuildHallCosmeticRef(int id, GuildHallBookData.Cosmetic cosmeticData)
		: base(id)
	{
		_typeRef = GuildHallBook.LookupCosmeticType(cosmeticData.type);
		_frame = cosmeticData.id + 1;
		_display = Util.GetBoolFromStringProperty(cosmeticData.display, defaultValue: true);
		_guildLvlReq = cosmeticData.guildLvlReq;
		_parentType = cosmeticData.parent;
		_objects = Util.GetStringListFromStringProperty(cosmeticData.objects);
		LoadDetails(cosmeticData);
	}

	public void loadAssets()
	{
		parentRef?.loadAssets();
	}

	public override Sprite GetSpriteIcon()
	{
		if (parentRef != null)
		{
			return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GUILD_HALL_COSMETIC_ICON, parentRef.icon);
		}
		if (icon != null && icon != "")
		{
			return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GUILD_HALL_COSMETIC_ICON, icon);
		}
		return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GUILD_HALL_COSMETIC_ICON, "generic.png");
	}

	public bool Equals(GuildHallCosmeticRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(GuildHallCosmeticRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
