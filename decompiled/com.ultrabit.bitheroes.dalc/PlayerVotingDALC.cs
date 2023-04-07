using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class PlayerVotingDALC : BaseDALC
{
	public const int GET_VOTES = 1;

	public const int DO_VOTE = 2;

	public const int REMOVE_VOTE = 3;

	private static PlayerVotingDALC _instance;

	public static PlayerVotingDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlayerVotingDALC();
			}
			return _instance;
		}
	}

	public void doVote(int votingId, int posId, int charId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("vot0", votingId);
		sFSObject.PutInt("vot1", posId);
		sFSObject.PutInt("vot2", charId);
		send(sFSObject);
	}

	public void doUnvote(int votingId, int posId, int charId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("vot0", votingId);
		sFSObject.PutInt("vot1", posId);
		sFSObject.PutInt("vot2", charId);
		send(sFSObject);
	}

	public void getMyVotes(int votingId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutInt("vot0", votingId);
		send(sFSObject);
	}
}
