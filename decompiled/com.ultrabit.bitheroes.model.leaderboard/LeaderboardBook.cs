using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.leaderboard;

public class LeaderboardBook
{
	private static List<LeaderboardRef> _leaderboards;

	public static int size => _leaderboards.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_leaderboards = new List<LeaderboardRef>();
		int num = 0;
		foreach (LeaderboardBookData.Leaderboard item in XMLBook.instance.leaderboardBook.leaderboards.lstLeaderboard)
		{
			_leaderboards.Add(new LeaderboardRef(num, item));
			num++;
		}
		yield return null;
		if (onUpdatedProgress != null && onUpdatedProgress.Target != null && !onUpdatedProgress.Target.Equals(null))
		{
			onUpdatedProgress(XMLBook.instance.UpdateProgress());
		}
	}

	public static LeaderboardRef Lookup(int id)
	{
		if (id < 0 || id >= size)
		{
			return null;
		}
		return _leaderboards[id];
	}
}
