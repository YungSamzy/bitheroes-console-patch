using System.Collections.Generic;
using com.ultrabit.bitheroes.model.leaderboard;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.events;

public class EventLeaderboardData
{
	private int _eventID;

	private List<LeaderboardData> _leaderboardList;

	private EventCharacterData _eventData;

	public int eventID => _eventID;

	public List<LeaderboardData> leaderboardList => _leaderboardList;

	public EventCharacterData eventData => _eventData;

	public EventLeaderboardData(int eventID, List<LeaderboardData> leaderboardList, EventCharacterData eventData)
	{
		_eventID = eventID;
		_leaderboardList = leaderboardList;
		_eventData = eventData;
	}

	public static EventLeaderboardData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve0");
		List<LeaderboardData> list = LeaderboardData.listFromSFSObject(sfsob);
		EventCharacterData eventCharacterData = EventCharacterData.fromSFSObject(sfsob);
		return new EventLeaderboardData(@int, list, eventCharacterData);
	}
}
