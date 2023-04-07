using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.zone;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneNodePathRef
{
	private int _zoneID;

	private int _nodeID;

	private List<Vector2> _points;

	private ZoneXMLData.Path pathData;

	public int zoneID => _zoneID;

	public int nodeID => _nodeID;

	public List<Vector2> points => _points;

	public ZoneNodePathRef(int zoneID, int nodeID, ZoneXMLData.Path pathData)
	{
		_zoneID = zoneID;
		_nodeID = nodeID;
		_points = Util.StringToVector2List(pathData.points);
	}

	public ZoneNodePathRef(int zoneID, int nodeID, BaseEventBookData.Path pathData)
	{
		_zoneID = zoneID;
		_nodeID = nodeID;
		_points = Util.StringToVector2List(pathData.points, ',', ':');
	}
}
