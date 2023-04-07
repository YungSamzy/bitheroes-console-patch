using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.instance;

[DebuggerDisplay("{typeName} (InstanceRef)")]
public class InstanceRef : BaseRef, IEquatable<InstanceRef>, IComparable<InstanceRef>
{
	public const int INSTANCE_TYPE_TOWN = 1;

	public const int INSTANCE_TYPE_GUILD = 2;

	public const int INSTANCE_TYPE_FISHING = 3;

	public const int INSTANCE_TYPE_ARMORY = 4;

	public const int INSTANCE_TYPE_PLAYER_ARMORY = 5;

	private static Dictionary<string, int> INSTANCE_TYPES = new Dictionary<string, int>
	{
		["town"] = 1,
		["guild"] = 2,
		["fishing"] = 3,
		["armory"] = 4,
		["player_armory"] = 5
	};

	private int _type;

	private string _typeName;

	private int _players;

	private int _size;

	private int _width;

	private int _height;

	private string _asset;

	private string _music;

	private SoundPoolRef _footstepsDefault;

	private List<bool> _walkable;

	private List<int> _spawns;

	private List<SoundPoolRef> _footsteps;

	private List<InstanceObjectRef> _objects;

	private List<InstanceOffsetRef> _offsets;

	public int type => _type;

	public string typeName => _typeName;

	public int players => _players;

	public int size => _size;

	public int width => _width;

	public int height => _height;

	public List<InstanceObjectRef> objects => _objects;

	public MusicRef musicRef => MusicBook.Lookup(_music);

	public SoundPoolRef footstepsDefault => _footstepsDefault;

	public string asset => _asset;

	public InstanceRef(int id, InstanceBookData.Instance instanceData)
		: base(id)
	{
		_type = getInstanceType(instanceData.type);
		_typeName = instanceData.type;
		_players = instanceData.players;
		_size = instanceData.size;
		_width = instanceData.width;
		_height = instanceData.height;
		_asset = instanceData.asset;
		_music = instanceData.music;
		char[] array = instanceData.walkable.ToCharArray();
		_walkable = new List<bool>();
		char[] array2 = array;
		foreach (char c in array2)
		{
			if (c == '1' || c == '0')
			{
				_walkable.Add(c == '0');
			}
		}
		string[] stringArrayFromStringProperty = Util.GetStringArrayFromStringProperty(instanceData.spawns);
		_spawns = new List<int>();
		string[] array3 = stringArrayFromStringProperty;
		foreach (string s in array3)
		{
			_spawns.Add(int.Parse(s));
		}
		int.TryParse(instanceData.footstepsDefault, out var result);
		_footstepsDefault = SoundBook.LookupPool(result);
		_footsteps = new List<SoundPoolRef>();
		if (instanceData.footsteps != null)
		{
			array2 = instanceData.footsteps.ToCharArray();
			foreach (char c2 in array2)
			{
				_footsteps.Add(SoundBook.LookupPool(int.Parse(c2.ToString())));
			}
		}
		_objects = new List<InstanceObjectRef>();
		foreach (AssetDisplayData item in instanceData.objects.lstObject)
		{
			if (item != null)
			{
				_objects.Add(new InstanceObjectRef(_objects.Count, item));
			}
		}
		_offsets = new List<InstanceOffsetRef>();
		foreach (InstanceBookData.Offset item2 in instanceData.offsets.lstOffset)
		{
			if (item2 != null)
			{
				_offsets.Add(new InstanceOffsetRef(item2));
			}
		}
		LoadDetails(instanceData);
	}

	public bool getWalkable(int id)
	{
		if (id < 0 || id >= _walkable.Count)
		{
			return true;
		}
		return _walkable[id];
	}

	public bool getSpawn(int id)
	{
		foreach (int spawn in _spawns)
		{
			if (spawn == id)
			{
				return true;
			}
		}
		return false;
	}

	public SoundPoolRef getFootstep(int id)
	{
		if (id < 0 || id >= _footsteps.Count)
		{
			return _footstepsDefault;
		}
		SoundPoolRef soundPoolRef = _footsteps[id];
		if (soundPoolRef == null)
		{
			return _footstepsDefault;
		}
		return soundPoolRef;
	}

	public Vector2? getTileOffset(int tileID)
	{
		foreach (InstanceOffsetRef offset in _offsets)
		{
			if (offset.hasTile(tileID))
			{
				return offset.offset;
			}
		}
		return null;
	}

	public InstanceObjectRef getObjectByLink(string link)
	{
		foreach (InstanceObjectRef @object in _objects)
		{
			if (@object != null && @object.link != null && @object.link.ToLowerInvariant() == link.ToLowerInvariant())
			{
				return @object;
			}
		}
		return null;
	}

	public Asset getAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	public static int getInstanceType(string type)
	{
		return INSTANCE_TYPES[type.ToLowerInvariant()];
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(InstanceRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(InstanceRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public InstanceObjectRef GetFirstObjectByType(int type)
	{
		foreach (InstanceObjectRef @object in _objects)
		{
			if (@object.type == type)
			{
				return @object;
			}
		}
		return null;
	}
}
