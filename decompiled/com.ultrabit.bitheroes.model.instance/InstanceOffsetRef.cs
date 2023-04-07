using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.instance;

public class InstanceOffsetRef
{
	private Vector2 _offset;

	private List<int> _tiles;

	public Vector2 offset => _offset;

	public InstanceOffsetRef(InstanceBookData.Offset offsetData)
	{
		_offset = new Vector2(offsetData.x, offsetData.y);
		string[] array = offsetData.tiles.Split(',');
		_tiles = new List<int>();
		string[] array2 = array;
		foreach (string s in array2)
		{
			_tiles.Add(int.Parse(s));
		}
	}

	public bool hasTile(int tileID)
	{
		foreach (int tile in _tiles)
		{
			if (tile == tileID)
			{
				return true;
			}
		}
		return false;
	}
}
