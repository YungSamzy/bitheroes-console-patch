using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class FriendDALC : BaseDALC
{
	public const int SEND_REQUEST = 1;

	public const int RECEIVE_REQUEST = 2;

	public const int ACCEPT_REQUEST = 3;

	public const int DENY_REQUEST = 4;

	public const int FRIEND_ADDED = 5;

	public const int FRIEND_REMOVE = 6;

	public const int FRIEND_REMOVED = 7;

	public const int FRIEND_RECOMMEND = 8;

	public const int DENY_ALL_REQUESTS = 9;

	private static FriendDALC _instance;

	public static FriendDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new FriendDALC();
			}
			return _instance;
		}
	}

	public void doSendRequestByID(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doSendRequestByName(string playerName)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutUtfString("cha2", playerName);
		send(sFSObject);
	}

	public void doAcceptRequest(int charid)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("cha1", charid);
		send(sFSObject);
	}

	public void doDenyRequest(int charid)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutInt("cha1", charid);
		send(sFSObject);
	}

	public void doFriendRemove(int charid)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutInt("cha1", charid);
		send(sFSObject);
	}

	public void doFriendRecommend()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		send(sFSObject);
	}

	public void doDenyAllRequests()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		send(sFSObject);
	}
}
