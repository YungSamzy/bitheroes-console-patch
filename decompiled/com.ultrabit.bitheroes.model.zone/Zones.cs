using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.zone;

public class Zones
{
	private List<ZoneData> _zones;

	public List<ZoneData> zones => _zones;

	public Zones(List<ZoneData> zones)
	{
		_zones = zones;
	}

	public int getZoneDifficultyCompletes(ZoneRef zoneRef)
	{
		int num = 0;
		if (zoneRef == null)
		{
			return num;
		}
		foreach (ZoneNodeRef node in zoneRef.nodes)
		{
			if (node != null)
			{
				num += getNodeDifficultyCompletes(node);
			}
		}
		return num;
	}

	public int getNodeDifficultyCompletes(ZoneNodeRef nodeRef)
	{
		int num = 0;
		if (nodeRef == null)
		{
			return num;
		}
		foreach (ZoneNodeDifficultyRef difficulty in nodeRef.difficulties)
		{
			int num2 = GameData.instance.PROJECT.character.zones.getZoneData(difficulty.zoneID, difficulty.difficultyRef.id)?.getNodeCompleteCount(nodeRef.nodeID) ?? 0;
			num += ((num2 > 0) ? 1 : 0);
		}
		return num;
	}

	public int getTotalStars()
	{
		int num = 0;
		for (int i = 0; i < ZoneBook.zoneCount; i++)
		{
			ZoneRef zoneRef = ZoneBook.Lookup(i);
			if (zoneRef != null)
			{
				num += getZoneStars(zoneRef);
			}
		}
		return num;
	}

	public int getZoneStars(ZoneRef zoneRef)
	{
		int num = 0;
		if (zoneRef == null)
		{
			return num;
		}
		foreach (ZoneNodeRef node in zoneRef.nodes)
		{
			if (node != null && node.repeatable)
			{
				num += getNodeStars(node);
			}
		}
		return num;
	}

	public int getNodeStars(ZoneNodeRef nodeRef)
	{
		int num = 0;
		if (nodeRef == null || !nodeRef.repeatable)
		{
			return num;
		}
		foreach (ZoneNodeDifficultyRef difficulty in nodeRef.difficulties)
		{
			int num2 = getZoneData(difficulty.zoneID, difficulty.difficultyRef.id)?.getNodeCompleteCount(nodeRef.nodeID) ?? 0;
			num += ((num2 > 0) ? 1 : 0);
		}
		return num;
	}

	public ZoneData getZoneData(int zoneID, int difficulty)
	{
		foreach (ZoneData zone in _zones)
		{
			if (zone.zoneRef.id == zoneID && zone.zoneDifficultyRef.id == difficulty)
			{
				return zone;
			}
		}
		return null;
	}

	public ZoneData addZoneData(ZoneData zoneData)
	{
		if (getZoneData(zoneData.zoneRef.id, zoneData.zoneDifficultyRef.id) != null)
		{
			return null;
		}
		_zones.Add(zoneData);
		return zoneData;
	}

	public bool zoneIsCompleted(ZoneRef zoneRef)
	{
		if (zoneRef == null)
		{
			return false;
		}
		ZoneNodeRef completeNode = zoneRef.getCompleteNode();
		if (completeNode == null)
		{
			return false;
		}
		if (!nodeIsCompleted(completeNode))
		{
			return false;
		}
		return true;
	}

	public bool zoneIsUnlocked(ZoneRef zoneRef)
	{
		if (zoneRef == null)
		{
			return false;
		}
		foreach (int requiredZone in zoneRef.requiredZones)
		{
			ZoneRef zoneRef2 = ZoneBook.Lookup(requiredZone);
			ZoneDifficultyRef firstDifficultyRef = ZoneBook.GetFirstDifficultyRef();
			if (getZoneData(requiredZone, firstDifficultyRef.id) == null)
			{
				return false;
			}
			if (!zoneIsCompleted(zoneRef2))
			{
				return false;
			}
		}
		return true;
	}

	public bool nodeIsCompleted(ZoneNodeRef nodeRef)
	{
		if (nodeRef == null)
		{
			return false;
		}
		ZoneDifficultyRef firstDifficultyRef = ZoneBook.GetFirstDifficultyRef();
		ZoneData zoneData = getZoneData(nodeRef.getZoneRef().id, firstDifficultyRef.id);
		if (zoneData == null)
		{
			return false;
		}
		if (zoneData.getNodeCompleteCount(nodeRef.nodeID) <= 0)
		{
			return false;
		}
		return true;
	}

	public bool nodeIsUnlocked(ZoneNodeRef nodeRef)
	{
		if (nodeRef == null)
		{
			return false;
		}
		if (!nodeRef.repeatable)
		{
			ZoneDifficultyRef firstDifficultyRef = ZoneBook.GetFirstDifficultyRef();
			ZoneData zoneData = getZoneData(nodeRef.getZoneRef().id, firstDifficultyRef.id);
			if (zoneData != null && zoneData.getNodeCompleteCount(nodeRef.nodeID) > 0)
			{
				return false;
			}
		}
		ZoneRef zoneRef = nodeRef.getZoneRef();
		foreach (int requiredNode in nodeRef.requiredNodes)
		{
			ZoneNodeRef nodeRef2 = zoneRef.getNodeRef(requiredNode);
			if (nodeRef2 != null && !nodeIsCompleted(nodeRef2))
			{
				return false;
			}
		}
		if (nodeRef.requiredStars > 0 && getZoneStars(nodeRef.getZoneRef()) < nodeRef.requiredStars)
		{
			return false;
		}
		if (nodeRef.unlockNodes.Count > 0)
		{
			foreach (int unlockNode in nodeRef.unlockNodes)
			{
				ZoneNodeRef nodeRef3 = zoneRef.getNodeRef(unlockNode);
				if (nodeRef3 != null && nodeIsCompleted(nodeRef3))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public bool nodeDifficultyIsUnlocked(ZoneNodeDifficultyRef nodeDifficultyRef)
	{
		if (!zoneIsUnlocked(nodeDifficultyRef.getZoneRef()))
		{
			return false;
		}
		if (!nodeIsUnlocked(nodeDifficultyRef.getNodeRef()))
		{
			return false;
		}
		foreach (int requiredDifficulty in nodeDifficultyRef.requiredDifficulties)
		{
			ZoneData zoneData = getZoneData(nodeDifficultyRef.zoneID, requiredDifficulty);
			if (zoneData == null)
			{
				return false;
			}
			if (zoneData.getNodeCompleteCount(nodeDifficultyRef.nodeID) <= 0)
			{
				return false;
			}
		}
		return true;
	}

	public int getHighestCompletedZoneID()
	{
		int num = 0;
		foreach (ZoneData zone in _zones)
		{
			if (zone != null && zoneIsCompleted(zone.zoneRef) && zone.zoneRef.id > num)
			{
				num = zone.zoneRef.id;
			}
		}
		return num;
	}

	public ZoneRef getHighestCompletedZoneRef()
	{
		ZoneRef result = null;
		int num = 0;
		foreach (ZoneData zone in _zones)
		{
			if (zone != null && zoneIsCompleted(zone.zoneRef) && zone.zoneRef.id > num)
			{
				num = zone.zoneRef.id;
				result = zone.zoneRef;
			}
		}
		return result;
	}

	public int getZoneCompleteCount()
	{
		int num = 0;
		if (_zones == null)
		{
			return num;
		}
		if (_zones.Count <= 0)
		{
			return num;
		}
		foreach (ZoneData zone in _zones)
		{
			if (zoneIsCompleted(zone.zoneRef))
			{
				num++;
			}
		}
		return num;
	}

	public bool getFirstNodeCompleted()
	{
		foreach (ZoneData zone in _zones)
		{
			if (zone == null)
			{
				continue;
			}
			foreach (int nodeComplete in zone.nodeCompletes)
			{
				if (nodeComplete > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool isEmpty()
	{
		if (_zones == null)
		{
			return true;
		}
		if (_zones.Count <= 0)
		{
			return true;
		}
		foreach (ZoneData zone in _zones)
		{
			foreach (int nodeComplete in zone.nodeCompletes)
			{
				if (nodeComplete > 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static Zones fromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("cha23");
		List<ZoneData> list = new List<ZoneData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(ZoneData.fromSFSObject(sFSObject));
		}
		return new Zones(list);
	}

	public int getHighestMultFlagUnlocked()
	{
		int num = 1;
		foreach (ZoneData zone in _zones)
		{
			foreach (ZoneNodeRef node in zone.zoneRef.nodes)
			{
				if (node != null && !node.repeatable && nodeIsCompleted(node) && node.getFirstDifficultyRef() != null && node.getFirstDifficultyRef().dungeonRef != null && (float)num < node.getFirstDifficultyRef().dungeonRef.statMult)
				{
					num = (int)node.getFirstDifficultyRef().dungeonRef.statMult;
				}
			}
		}
		return num;
	}

	public ZoneRef getZoneRefByID(int id)
	{
		foreach (ZoneData zone in zones)
		{
			if (zone.zoneRef.id == id)
			{
				return zone.zoneRef;
			}
		}
		return null;
	}
}
