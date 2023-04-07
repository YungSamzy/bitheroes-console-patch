using System.Collections.Generic;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class BrawlDALC : BaseDALC
{
	public const int CREATE = 0;

	public const int JOIN = 1;

	public const int LEAVE = 2;

	public const int SEARCH = 3;

	public const int START = 4;

	public const int CHANGE = 5;

	public const int MESSAGE = 6;

	public const int READY = 7;

	public const int KICK = 8;

	public const int REMOVED = 9;

	public const int ORDER = 10;

	public const int BEGIN = 11;

	public const int INVITE = 12;

	public const int RULES = 13;

	public const int REJOIN = 14;

	private static BrawlDALC _instance;

	public static BrawlDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new BrawlDALC();
			}
			return _instance;
		}
	}

	public void doCreate(BrawlTierDifficultyRef difficultyRef, BrawlRules rules)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 0);
		sFSObject.PutInt("bra2", difficultyRef.brawlRef.id);
		sFSObject.PutInt("bra3", difficultyRef.tierRef.id);
		sFSObject.PutInt("bra4", difficultyRef.difficultyRef.id);
		sFSObject = rules.toSFSObject(sFSObject);
		send(sFSObject);
	}

	public void doJoin(int index, bool invited = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("bra1", index);
		sFSObject.PutBool("bra13", invited);
		send(sFSObject);
	}

	public void doLeave()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		send(sFSObject);
	}

	public void doSearch(int brawl = -1, int tier = -1, int difficulty = -1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("bra2", brawl);
		sFSObject.PutInt("bra3", tier);
		sFSObject.PutInt("bra4", difficulty);
		send(sFSObject);
	}

	public void doStart()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		send(sFSObject);
	}

	public void doMessage(string message)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutUtfString("chat0", message);
		send(sFSObject);
	}

	public void doReady(bool ready)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutBool("bra7", ready);
		send(sFSObject);
	}

	public void doKick(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doOrder(List<int> order)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutIntArray("bra6", Util.intVectorToArray(order));
		send(sFSObject);
	}

	public void doInvite(int charID = 0, string name = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 12);
		sFSObject.PutInt("cha1", charID);
		if (name != null)
		{
			sFSObject.PutUtfString("cha2", name);
		}
		send(sFSObject);
	}

	public void doRules(BrawlRules rules)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 13);
		sFSObject = rules.toSFSObject(sFSObject);
		send(sFSObject);
	}

	public void doRejoin()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 14);
		send(sFSObject);
	}
}
