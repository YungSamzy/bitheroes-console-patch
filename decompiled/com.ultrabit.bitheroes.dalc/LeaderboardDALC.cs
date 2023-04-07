using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class LeaderboardDALC : BaseDALC
{
	public const int GET_LIST = 1;

	public const int GET_EVENT = 2;

	public const int TYPE_PLAYERS = 0;

	public const int TYPE_GUILDS = 1;

	public const int TYPE_GUILD_MEMBERS = 2;

	private static LeaderboardDALC _instance;

	public static LeaderboardDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LeaderboardDALC();
			}
			return _instance;
		}
	}

	public void doGetList(int id)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("lea0", id);
		send(sFSObject);
	}

	public void doGetEvent(int eventType, int eventID, int leaderboardType = 0, bool allowSegmented = true)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("eve9", eventType);
		sFSObject.PutInt("eve0", eventID);
		sFSObject.PutInt("lea1", leaderboardType);
		sFSObject.PutBool("lea3", allowSegmented);
		send(sFSObject);
	}
}
