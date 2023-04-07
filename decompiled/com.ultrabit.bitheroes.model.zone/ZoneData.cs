using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneData
{
	private ZoneRef _zoneRef;

	private ZoneDifficultyRef _zoneDifficultyRef;

	private List<int> _nodeCompletes;

	private ZoneRef zoneRef1;

	private ZoneDifficultyRef zoneDifficultyRef1;

	private List<int> nodeCompletes1;

	public ZoneRef zoneRef => _zoneRef;

	public ZoneDifficultyRef zoneDifficultyRef => _zoneDifficultyRef;

	public List<int> nodeCompletes => _nodeCompletes;

	public ZoneData(ZoneRef zoneRef, ZoneDifficultyRef zoneDifficultyRef, List<int> nodeCompletes)
	{
		_zoneRef = zoneRef;
		_zoneDifficultyRef = zoneDifficultyRef;
		_nodeCompletes = nodeCompletes;
	}

	public int getNodeCompleteCount(int nodeID)
	{
		if (nodeID < 0 || nodeID >= _nodeCompletes.Count)
		{
			return 0;
		}
		return _nodeCompletes[nodeID];
	}

	public void setNodeCompleteCount(int nodeID, int count)
	{
		while (_nodeCompletes.Count <= nodeID)
		{
			_nodeCompletes.Add(0);
		}
		_nodeCompletes[nodeID] = count;
	}

	public void clearNodes()
	{
		_nodeCompletes.Clear();
	}

	public static ZoneData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("zon0");
		int int2 = sfsob.GetInt("zon2");
		return new ZoneData(nodeCompletes: Util.arrayToIntegerVector(sfsob.GetIntArray("zon3")), zoneRef: ZoneBook.Lookup(@int), zoneDifficultyRef: ZoneBook.LookupDifficulty(int2));
	}
}
