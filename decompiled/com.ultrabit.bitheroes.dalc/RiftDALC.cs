using System.Collections.Generic;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class RiftDALC : BaseDALC
{
	public static int EVENT_ENTER = 0;

	public static int EVENT_STATS = 1;

	public static int EVENT_LOOT_CHECK = 2;

	public static int EVENT_LOOT_ITEMS = 3;

	private static RiftDALC _instance;

	public static RiftDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RiftDALC();
			}
			return _instance;
		}
	}

	public void doEventEnter(int difficulty, int bonusID, List<TeammateData> teammates)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_ENTER);
		sFSObject.PutInt("eve11", difficulty);
		sFSObject.PutInt("curr3", bonusID);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doEventStats(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_STATS);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventLootCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_LOOT_CHECK);
		send(sFSObject);
	}

	public void doEventLootItems(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_LOOT_ITEMS);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}
}
