using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.zone;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.gve;

public class GvEEventZoneNodePath : MonoBehaviour
{
	public const int CHECK_OFFSET = 2;

	public const int TILE_OFFSET = 18;

	public Transform nodePathPrefab;

	private GvEZoneNodeRef _nodeRef;

	private ZoneNodePathRef _pathRef;

	private GvEEventZoneWindow _gvEEventZoneWindow;

	private List<GameObject> _tiles;

	private GvEEventZoneData _zoneData;

	public void LoadDetails(GvEZoneNodeRef nodeRef, ZoneNodePathRef pathRef, GvEEventZoneWindow gvEEventZoneWindow)
	{
		_nodeRef = nodeRef;
		_pathRef = pathRef;
		_gvEEventZoneWindow = gvEEventZoneWindow;
	}

	public void SetZoneData(GvEEventZoneData zoneData)
	{
		_zoneData = zoneData;
		if (_zoneData.nodeIsCompleted(_nodeRef))
		{
			CreateTiles();
		}
	}

	public void CreateTiles()
	{
		if (_tiles != null)
		{
			return;
		}
		_tiles = new List<GameObject>();
		List<Vector2> list = new List<Vector2>();
		list.AddRange(_pathRef.points);
		Vector2? vector = list[0];
		list.RemoveAt(0);
		Vector2? vector2 = list[0];
		list.RemoveAt(0);
		Vector2? vector3 = new Vector2(-2.1474836E+09f, 2.1474836E+09f);
		while (vector.HasValue && vector2.HasValue)
		{
			float num = vector.Value.x - vector2.Value.x;
			float num2 = vector.Value.y - vector2.Value.y;
			int num3 = Mathf.FloorToInt(Mathf.Sqrt(num * num + num2 * num2) / 2f);
			float num4 = Mathf.Atan2(vector.Value.y - vector2.Value.y, vector.Value.x - vector2.Value.x) / (float)Math.PI * 180f;
			for (int i = 0; i < num3; i++)
			{
				float num5 = vector.Value.x - Mathf.Cos(num4 / 180f * (float)Math.PI) * (float)(2 * i);
				float num6 = vector.Value.y - Mathf.Sin(num4 / 180f * (float)Math.PI) * (float)(2 * i);
				float num7 = vector3.Value.x - num5;
				float num8 = vector3.Value.y - num6;
				if (Mathf.Sqrt(num7 * num7 + num8 * num8) >= 18f)
				{
					Transform transform = UnityEngine.Object.Instantiate(nodePathPrefab);
					transform.SetParent(base.transform, worldPositionStays: false);
					transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(num5 / _gvEEventZoneWindow.panel.transform.localScale.x, num6 / (0f - _gvEEventZoneWindow.panel.transform.localScale.y));
					_tiles.Add(transform.gameObject);
					vector3 = new Vector2(num5, num6);
				}
			}
			vector = vector2;
			if (list.Count > 0)
			{
				vector2 = list[0];
				list.RemoveAt(0);
			}
			else
			{
				vector2 = null;
			}
		}
	}
}
