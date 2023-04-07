using System.Collections.Generic;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class InvasionDALC : BaseDALC
{
	public const int EVENT_ENTER = 0;

	public const int EVENT_STATS = 1;

	public const int EVENT_LOOT_CHECK = 2;

	public const int EVENT_LOOT_ITEMS = 3;

	private static InvasionDALC _instance;

	public static InvasionDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new InvasionDALC();
			}
			return _instance;
		}
	}

	public void doEventEnter(int difficulty, int bonusID, List<TeammateData> teammates)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("eve11", difficulty);
		sFSObject.PutInt("curr3", bonusID);
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
}
