using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.common;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{displayLink} (DungeonObjectRef)")]
public class DungeonObjectRef : BaseRef, IEquatable<DungeonObjectRef>, IComparable<DungeonObjectRef>
{
	protected AssetDisplayRef _displayRef;

	private bool _collision;

	private int _spread;

	private int _distance;

	private bool _instant;

	private bool _clickable;

	private new string _icon;

	private string assetURL;

	public AssetDisplayRef displayRef => _displayRef;

	private string displayLink => displayRef.link;

	public bool collision => _collision;

	public int spread => _spread;

	public int distance => _distance;

	public bool instant => _instant;

	public bool clickable => _clickable;

	public DungeonObjectRef(int id, AssetDisplayData objectData)
		: base(id)
	{
		_displayRef = new AssetDisplayRef(objectData, AssetURL.DUNGEON_OBJECT);
		_collision = Util.parseBoolean(objectData.collision);
		_spread = objectData.spread;
		_distance = objectData.distance;
		_instant = Util.parseBoolean(objectData.instant, defaultVal: false);
		_clickable = Util.parseBoolean(objectData.clickable);
		_icon = objectData.icon;
		base.LoadDetails(objectData);
	}

	public override Sprite GetSpriteIcon()
	{
		if (_icon != null)
		{
			return GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.DUNGEON_OBJECT_ICON, _icon);
		}
		return null;
	}

	public bool Equals(DungeonObjectRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(DungeonObjectRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
