using System.Collections.Generic;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class PvPDALC : BaseDALC
{
	public static int EVENT_ENTER = 0;

	public static int EVENT_FLEE = 1;

	public static int EVENT_BATTLE = 2;

	public static int EVENT_STATS = 3;

	public static int EVENT_HISTORY = 4;

	public static int EVENT_LOOT_CHECK = 5;

	public static int EVENT_LOOT_ITEMS = 6;

	public static int EVENT_REPLAY = 7;

	public static int DUEL_SEND = 8;

	public static int DUEL_ACCEPT = 9;

	public static int DUEL_DECLINE = 10;

	public static int DUEL_CANCEL = 11;

	public static int DUEL_TARGET = 12;

	public static int DUEL_SOURCE = 13;

	private static PvPDALC _instance;

	public static PvPDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PvPDALC();
			}
			return _instance;
		}
	}

	public void doEventEnter(int bonusID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_ENTER);
		sFSObject.PutInt("curr3", bonusID);
		send(sFSObject);
	}

	public void doEventFlee()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_FLEE);
		send(sFSObject);
	}

	public void doEventBattle(int charID, List<TeammateData> teammates)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_BATTLE);
		sFSObject.PutInt("cha1", charID);
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

	public void doEventHistory(int eventID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_HISTORY);
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

	public void doEventReplay(int historyID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", EVENT_REPLAY);
		sFSObject.PutInt("eve4", historyID);
		send(sFSObject);
	}

	public void doDuelSend(int charID, bool online)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", DUEL_SEND);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutBool("use5", online);
		send(sFSObject);
	}

	public void doDuelAccept(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", DUEL_ACCEPT);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doDuelDecline(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", DUEL_DECLINE);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doDuelCancel(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", DUEL_CANCEL);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}
}
