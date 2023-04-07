using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class AdminDALC : BaseDALC
{
	public const int LOGIN = 1;

	public const int UPLOAD_XMLS = 2;

	public const int CLEAR_XMLS = 3;

	public const int GLOBAL_MESSAGE = 4;

	public const int DISABLE_BATTLES = 5;

	public const int DISABLE_DUNGEONS = 6;

	public const int KICK_PLAYER_NAME = 7;

	public const int BAN_PLAYER_NAME = 8;

	public const int CHECK_INFO = 9;

	public const int DISABLE_STATISTICS = 10;

	public const int INFO_GENERAL = 11;

	public const int INFO_SERVER = 12;

	public const int UPDATE_THREAD_POOL = 13;

	public const int CHARACTER_RENAME = 14;

	public const int CHARACTER_SEARCH = 15;

	public const int CHARACTER_INVENTORY = 16;

	public const int CHARACTER_PAYMENTS = 17;

	public const int CHARACTER_PURCHASES = 18;

	public const int CHARACTER_ADJUST_ITEMS = 19;

	public const int CHARACTER_PLATFORM_ENABLE = 20;

	public const int CHARACTER_PLATFORM_DISABLE = 21;

	public const int CHARACTER_PLATFORM_ADD = 22;

	public const int GUILD_SEARCH = 23;

	public const int UPLOAD_PROPERTIES = 24;

	public const int GUILD_RENAME = 25;

	public const int GUILD_REINITIALS = 26;

	public const int INFO_EXCHANGE = 27;

	public const int CHARACTER_OFFERWALLS = 28;

	public const int UNBAN_PLAYER_NAME = 29;

	public const int SHUTDOWN = 30;

	private static string _adminPassword;

	private static AdminDALC _instance;

	public static AdminDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AdminDALC();
			}
			return _instance;
		}
	}

	public void doLogin(string password)
	{
		_adminPassword = password;
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doClearXMLs()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doGlobalMessage(string message, bool instance = true)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutUtfString("not1", message);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doDisableBattles(bool disabled, bool instance = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutBool("bat26", disabled);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doDisableDungeons(bool disabled, bool instance = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutBool("dun29", disabled);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doKickPlayerName(string playerName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutUtfString("cha2", playerName);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doBanPlayerName(string playerName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutUtfString("cha2", playerName);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doUnbanPlayerName(string playerName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 29);
		sFSObject.PutUtfString("cha2", playerName);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doShutdown(bool start, bool instance = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 30);
		sFSObject.PutBool("serv10", start);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCheckInfo()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doDisableStatistics(bool disabled, bool instance = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutBool("sta0", disabled);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doInfoGeneral()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doInfoServer()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 12);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doUpdateThreadPool(int threadPool)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 13);
		sFSObject.PutInt("serv2", threadPool);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterRename(string oldName, string newName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 14);
		sFSObject.PutUtfString("use0", oldName);
		sFSObject.PutUtfString("cha2", newName);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterSearch(int charID = 0, string name = null, int platform = -1, string userID = null, string orderID = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 15);
		sFSObject.PutInt("cha1", charID);
		if (name != null)
		{
			sFSObject.PutUtfString("cha2", name);
		}
		if (platform > 0)
		{
			sFSObject.PutInt("use2", platform);
		}
		if (userID != null)
		{
			sFSObject.PutUtfString("use3", userID);
		}
		if (orderID != null)
		{
			sFSObject.PutUtfString("pay7", orderID);
		}
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterInventory(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 16);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterPayments(int charID, int limit)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 17);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("pay9", limit);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterPurchases(int charID, int limit)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 18);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("pur9", limit);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterAdjustItems(int charID, List<ItemData> itemsAdded = null, List<ItemData> itemsRemoved = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 19);
		sFSObject.PutInt("cha1", charID);
		if (itemsAdded != null && itemsAdded.Count > 0)
		{
			sFSObject.PutSFSObject("ite4", ItemData.listToSFSObject(new SFSObject(), itemsAdded));
		}
		if (itemsRemoved != null && itemsRemoved.Count > 0)
		{
			sFSObject.PutSFSObject("ite5", ItemData.listToSFSObject(new SFSObject(), itemsRemoved));
		}
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterPlatformEnable(int charID, int id, int platform, string userID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 20);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("plat0", id);
		sFSObject.PutInt("plat1", platform);
		sFSObject.PutUtfString("plat2", userID);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterPlatformDisable(int charID, int platformID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 21);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("plat0", platformID);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterPlatformAdd(int charID, int platform, string userID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 22);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("plat1", platform);
		sFSObject.PutUtfString("plat2", userID);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doGuildSearch(int guildID = 0, string name = null, string initials = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 23);
		sFSObject.PutInt("gui0", guildID);
		if (name != null)
		{
			sFSObject.PutUtfString("gui2", name);
		}
		if (initials != null)
		{
			sFSObject.PutUtfString("gui3", initials);
		}
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doUploadProperties(string properties, bool instance = true)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 24);
		sFSObject.PutUtfString("serv8", properties);
		sFSObject.PutBool("not2", instance);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doGuildRename(string oldName, string newName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 25);
		sFSObject.PutUtfString("use0", oldName);
		sFSObject.PutUtfString("gui2", newName);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doGuildInitials(string oldInitials, string newInitials)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 26);
		sFSObject.PutUtfString("use0", oldInitials);
		sFSObject.PutUtfString("gui3", newInitials);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doInfoExchange(ItemRef itemRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 27);
		sFSObject.PutInt("ite0", itemRef.id);
		sFSObject.PutInt("ite1", itemRef.itemType);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}

	public void doCharacterOfferwalls(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 28);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutUtfString("use1", _adminPassword);
		send(sFSObject);
	}
}
