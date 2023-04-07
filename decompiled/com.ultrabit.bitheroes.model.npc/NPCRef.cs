using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.npc;

[DebuggerDisplay("{name} (NPCRef)")]
public class NPCRef : BaseRef, IEquatable<NPCRef>, IComparable<NPCRef>
{
	private AssetDisplayRef _displayRef;

	private List<AbilityRef> _abilities;

	private List<GameModifier> _modifiers;

	private Vector2 _offset;

	private bool _anchorTop;

	private bool _boss;

	private float _scale;

	private int _familiar;

	private string _asset;

	public Rect bounds => Rect.zero;

	public Vector2 offset => _offset;

	public bool anchorTop => _anchorTop;

	public bool boss => _boss;

	public AssetDisplayRef displayRef => _displayRef;

	public List<AbilityRef> abilities => _abilities;

	public List<GameModifier> modifiers => _modifiers;

	public float scale => _scale;

	public int familiar => _familiar;

	public string asset => _asset;

	public NPCRef(int id, NPCBookData.NPC data)
		: base(id)
	{
		_abilities = AbilityBook.LookupAbilities(data.abilities);
		_displayRef = new AssetDisplayRef(data, AssetURL.NPC);
		if (_displayRef.asset == null && _displayRef.equipment == null && (_displayRef.itemID < 0 || _displayRef.itemType < 0))
		{
			_displayRef = null;
		}
		_modifiers = GameModifier.GetGameModifierFromData(data.modifiers, data.lstModifier);
		_offset = Util.GetVector2FromStringProperty(data.offset);
		_anchorTop = Util.GetBoolFromStringProperty(data.anchorTop);
		_boss = Util.GetBoolFromStringProperty(data.boss);
		_scale = Util.GetFloatFromStringProperty(data.scale, 1f);
		_familiar = ((data.familiar != null) ? int.Parse(data.familiar) : 0);
		_asset = data.asset;
		LoadDetails(data);
	}

	public static List<NPCRef> getBossesFromList(List<NPCRef> list)
	{
		List<NPCRef> list2 = new List<NPCRef>();
		foreach (NPCRef item in list)
		{
			if (item.boss)
			{
				list2.Add(item);
			}
		}
		return list2;
	}

	public static List<NPCRef> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("npc0"))
		{
			return null;
		}
		int[] intArray = sfsob.GetIntArray("npc0");
		List<NPCRef> list = new List<NPCRef>();
		int[] array = intArray;
		for (int i = 0; i < array.Length; i++)
		{
			NPCRef nPCRef = NPCBook.Lookup(array[i]);
			if (nPCRef != null)
			{
				list.Add(nPCRef);
			}
		}
		return list;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(NPCRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(NPCRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
