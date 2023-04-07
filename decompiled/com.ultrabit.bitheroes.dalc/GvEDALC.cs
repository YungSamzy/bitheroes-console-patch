using System.Collections.Generic;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class GvEDALC : BaseDALC
{
	public const int EVENT_ENTER = 0;

	public const int EVENT_STATS = 1;

	public const int EVENT_LOOT_CHECK = 2;

	public const int EVENT_LOOT_ITEMS = 3;

	public const int EVENT_ZONE_DATA = 4;

	public const int EVENT_ZONE_NODE_DATA = 5;

	private static GvEDALC _instance;

	public static GvEDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GvEDALC();
			}
			return _instance;
		}
	}

	public void doEventEnter(int nodeID, int difficulty, int bonusID, List<TeammateData> teammates, bool confirm = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("zon1", nodeID);
		sFSObject.PutInt("eve11", difficulty);
		sFSObject.PutInt("curr3", bonusID);
		sFSObject.PutBool("act7", confirm);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doEventStats(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventLootCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		send(sFSObject);
	}

	public void doEventLootItems(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventZoneData(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventZoneNodeData(int eventID, int nodeID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutInt("eve0", eventID);
		sFSObject.PutInt("zon1", nodeID);
		send(sFSObject);
	}
}
