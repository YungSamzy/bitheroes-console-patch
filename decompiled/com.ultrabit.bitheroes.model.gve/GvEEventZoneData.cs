using System.Collections.Generic;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.gve;

public class GvEEventZoneData
{
	private GvEEventRef _eventRef;

	private List<int> _nodePoints;

	public GvEEventRef eventRef => _eventRef;

	public List<int> nodePoints => _nodePoints;

	public GvEEventZoneData(GvEEventRef eventRef, List<int> nodePoints)
	{
		_eventRef = eventRef;
		_nodePoints = nodePoints;
	}

	public int getNodePoints(int nodeID)
	{
		if (nodeID < 0 || nodeID >= _nodePoints.Count)
		{
			return 0;
		}
		return _nodePoints[nodeID];
	}

	public void setNodePoints(int nodeID, int points)
	{
		while (_nodePoints.Count <= nodeID)
		{
			_nodePoints.Add(0);
		}
		_nodePoints[nodeID] = points;
	}

	public void clearNodes()
	{
		_nodePoints.Clear();
	}

	public bool nodeIsCompleted(GvEZoneNodeRef nodeRef)
	{
		if (nodeRef == null)
		{
			return false;
		}
		if (getNodePoints(nodeRef.nodeID) < nodeRef.points)
		{
			return false;
		}
		return true;
	}

	public bool nodeIsUnlocked(GvEZoneNodeRef nodeRef)
	{
		if (nodeRef == null)
		{
			return false;
		}
		GvEZoneRef zoneRef = nodeRef.GetZoneRef();
		foreach (int requiredNode in nodeRef.requiredNodes)
		{
			GvEZoneNodeRef nodeRef2 = zoneRef.getNodeRef(requiredNode);
			if (nodeRef2 != null && !nodeIsCompleted(nodeRef2))
			{
				return false;
			}
		}
		if (nodeRef.unlockNodes.Count > 0)
		{
			foreach (int unlockNode in nodeRef.unlockNodes)
			{
				GvEZoneNodeRef nodeRef3 = zoneRef.getNodeRef(unlockNode);
				if (nodeRef3 != null && nodeIsCompleted(nodeRef3))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public static GvEEventZoneData fromSFSObject(ISFSObject sfsob)
	{
		GvEEventRef gvEEventRef = GvEEventBook.Lookup(sfsob.GetInt("gve0"));
		List<int> list = Util.arrayToIntegerVector(sfsob.GetIntArray("gve2"));
		return new GvEEventZoneData(gvEEventRef, list);
	}
}
