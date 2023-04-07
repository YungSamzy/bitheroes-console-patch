using com.ultrabit.bitheroes.model.application;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class ChatDALC : BaseDALC
{
	public const int CHAT_MESSAGE = 1;

	public const int PRIVATE_MESSAGE = 2;

	public const int IGNORE = 3;

	public const int UNIGNORE = 4;

	public const int MUTE = 5;

	public const int UNMUTE = 6;

	public const int MUTE_LIST = 7;

	public const int REPORT = 8;

	public const int PLAYER_INFO = 9;

	public const int MUTE_LOG_LIST = 10;

	public const int MUTE_LOG_TEXT = 11;

	private static ChatDALC _instance;

	public static ChatDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ChatDALC();
			}
			return _instance;
		}
	}

	public void doChatMessage(string message)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutUtfString("chat0", message);
		send(sFSObject);
	}

	public void doPrivateMessage(string message, int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutUtfString("chat0", message);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doIgnore(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doUnignore(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doMute(int charID, int seconds, int reason, string chatLog)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("cha38", seconds);
		sFSObject.PutInt("cha39", reason);
		sFSObject.PutUtfString("chat6", chatLog);
		send(sFSObject);
	}

	public void doUnmute(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doMuteList()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		send(sFSObject);
	}

	public void doReport(int charID, string message)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutUtfString("chat0", message);
		send(sFSObject);
	}

	public void doPlayerInfo(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doMuteLogList(int charID = 0, int limit = 50)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutInt("chat16", limit);
		send(sFSObject);
	}

	public void doMuteLogText(int id)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		sFSObject.PutInt("chat8", id);
		sFSObject.PutInt("cli0", AppInfo.GetClientPlatform());
		send(sFSObject);
	}
}
