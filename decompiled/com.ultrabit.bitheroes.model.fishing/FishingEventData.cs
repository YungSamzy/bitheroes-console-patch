using System.Collections.Generic;
using com.ultrabit.bitheroes.model.leaderboard;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingEventData
{
	private List<LeaderboardData> _leaders;

	private FishingEventRef _eventRef;

	public List<LeaderboardData> leaders => _leaders;

	public FishingEventRef eventRef => _eventRef;

	public FishingEventData(List<LeaderboardData> leaders, FishingEventRef eventRef)
	{
		_leaders = leaders;
		_eventRef = eventRef;
	}

	public static FishingEventData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("fise2"))
		{
			return null;
		}
		ISFSObject sFSObject = sfsob.GetSFSObject("fise2");
		if (!sFSObject.ContainsKey("eve0"))
		{
			return null;
		}
		List<LeaderboardData> list = LeaderboardData.listFromSFSObject(sFSObject);
		FishingEventRef fishingEventRef = FishingEventBook.Lookup(sFSObject.GetInt("eve0"));
		return new FishingEventData(list, fishingEventRef);
	}
}
