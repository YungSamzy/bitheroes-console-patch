using System.Collections.Generic;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class GvGDALC : BaseDALC
{
	public const int EVENT_ENTER = 0;

	public const int EVENT_FLEE = 1;

	public const int EVENT_BATTLE = 2;

	public const int EVENT_STATS = 3;

	public const int EVENT_LOOT_CHECK = 4;

	public const int EVENT_LOOT_ITEMS = 5;

	private static GvGDALC _instance;

	public static GvGDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GvGDALC();
			}
			return _instance;
		}
	}

	public void doEventEnter(int bonusID, bool confirm = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("curr3", bonusID);
		sFSObject.PutBool("act7", confirm);
		send(sFSObject);
	}

	public void doEventFlee()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		send(sFSObject);
	}

	public void doEventBattle(int charID, List<TeammateData> teammates)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("cha1", charID);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doEventStats(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}

	public void doEventLootCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		send(sFSObject);
	}

	public void doEventLootItems(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutInt("eve0", eventID);
		send(sFSObject);
	}
}
