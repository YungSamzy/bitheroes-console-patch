using System.Collections.Generic;
using com.ultrabit.bitheroes.model.leaderboard;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.pvp;

public class PvPEventData
{
	private List<LeaderboardData> _leaders;

	private PvPEventRef _eventRef;

	public List<LeaderboardData> leaders => _leaders;

	public PvPEventRef eventRef => _eventRef;

	public PvPEventData(List<LeaderboardData> leaders, PvPEventRef eventRef)
	{
		_leaders = leaders;
		_eventRef = eventRef;
	}

	public static PvPEventData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("pvp2"))
		{
			return null;
		}
		ISFSObject sFSObject = sfsob.GetSFSObject("pvp2");
		if (!sFSObject.ContainsKey("eve0"))
		{
			return null;
		}
		List<LeaderboardData> list = LeaderboardData.listFromSFSObject(sFSObject);
		PvPEventRef pvPEventRef = PvPEventBook.Lookup(sFSObject.GetInt("eve0"));
		return new PvPEventData(list, pvPEventRef);
	}
}
