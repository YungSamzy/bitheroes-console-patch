using System.Collections.Generic;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class GuildDALC : BaseDALC
{
	public const int CREATE = 1;

	public const int LEAVE = 2;

	public const int DISBAND = 3;

	public const int SEARCH = 4;

	public const int LOAD_DATA = 5;

	public const int CHAT_LOAD = 6;

	public const int CHAT_MESSAGE = 7;

	public const int INVITE_SEND = 8;

	public const int INVITE_ACCEPT = 9;

	public const int INVITE_DECLINE = 10;

	public const int INVITE_DECLINE_ALL = 11;

	public const int INVITES_LOAD = 12;

	public const int APPLICATION_SEND = 13;

	public const int APPLICATION_ACCEPT = 14;

	public const int APPLICATION_DECLINE = 15;

	public const int APPLICATION_DECLINE_ALL = 16;

	public const int APPLICATIONS_LOAD = 17;

	public const int PLAYER_RANK = 18;

	public const int PLAYER_REMOVE = 19;

	public const int PLAYER_ADDED = 20;

	public const int PLAYER_REMOVED = 21;

	public const int LOAD_SHOP = 22;

	public const int CHANGE = 23;

	public const int KICK = 24;

	public const int PROMOTE = 25;

	public const int DEMOTE = 26;

	public const int UPDATE_RANK = 27;

	public const int UPDATE_PERMISSIONS = 28;

	public const int GET_PERMISSIONS = 29;

	public const int SAVE_PERMISSIONS = 30;

	public const int LIST = 31;

	public const int MUTINY = 32;

	public const int CONTRIBUTION = 33;

	public const int UPDATE_HALL_COSMETICS = 34;

	public const int UPDATE_HALL_ITEM_TYPE = 35;

	public const int PERK_UPGRADE = 36;

	public const int UPDATE_PERKS = 37;

	public const int SAVE_MESSAGE = 38;

	public const int OPEN = 39;

	private static GuildDALC _instance;

	public static GuildDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GuildDALC();
			}
			return _instance;
		}
	}

	public void doCreate(string name, string initials, int currencyID, int currencyCost)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutUtfString("gui2", name);
		sFSObject.PutUtfString("gui3", initials);
		sFSObject.PutInt("curr0", currencyID);
		sFSObject.PutInt("curr2", currencyCost);
		send(sFSObject);
	}

	public void doLeave()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		send(sFSObject);
	}

	public void doDisband()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		send(sFSObject);
	}

	public void doLoadData()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		send(sFSObject);
	}

	public void doChatLoad()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		send(sFSObject);
	}

	public void doChatMessage(string message)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutUtfString("chat0", message);
		send(sFSObject);
	}

	public void doInviteSendByID(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doInviteSendByName(string playerName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutUtfString("cha2", playerName);
		send(sFSObject);
	}

	public void doInviteAccept(int guildID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		sFSObject.PutInt("gui0", guildID);
		send(sFSObject);
	}

	public void doInviteDecline(int guildID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutInt("gui0", guildID);
		send(sFSObject);
	}

	public void doInviteDeclineAll()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		send(sFSObject);
	}

	public void doInvitesLoad()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 12);
		send(sFSObject);
	}

	public void doApplicationSend(int guildID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 13);
		sFSObject.PutInt("gui0", guildID);
		send(sFSObject);
	}

	public void doApplicationAccept(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 14);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doApplicationDecline(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 15);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doApplicationDeclineAll()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 16);
		send(sFSObject);
	}

	public void doApplicationsLoad()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 17);
		send(sFSObject);
	}

	public void doLoadShop()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 22);
		send(sFSObject);
	}

	public void doKick(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 24);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doPromote(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 25);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doDemote(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 26);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doGetPermissions()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 29);
		send(sFSObject);
	}

	public void doSavePermissions(GuildPermissions permissions)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 30);
		sFSObject = permissions.toSFSObject(sFSObject);
		send(sFSObject);
	}

	public void doList(string name = "")
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 31);
		sFSObject.PutUtfString("gui2", name);
		send(sFSObject);
	}

	public void doMutiny()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 32);
		send(sFSObject);
	}

	public void doContribution()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 33);
		send(sFSObject);
	}

	public void doUpdateHallCosmetics(List<int> cosmetics)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 34);
		sFSObject.PutIntArray("gui12", Util.intVectorToArray(cosmetics));
		send(sFSObject);
	}

	public void doUpdateHallItemType(int itemID, int itemType)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 35);
		sFSObject.PutInt("ite0", itemID);
		sFSObject.PutInt("ite1", itemType);
		send(sFSObject);
	}

	public void doPerkUpgrade(int perkID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 36);
		sFSObject.PutInt("gui16", perkID);
		send(sFSObject);
	}

	public void doSaveMessage(string message)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 38);
		sFSObject.PutUtfString("gui19", message);
		send(sFSObject);
	}

	public void doOpen(bool open)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 39);
		sFSObject.PutBool("gui20", open);
		send(sFSObject);
	}
}
