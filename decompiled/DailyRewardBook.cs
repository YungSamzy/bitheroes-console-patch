using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardBook
{
	private static Dictionary<string, Dictionary<int, DailyRewardRef>> _dailies;

	public static int size => _dailies.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_dailies = new Dictionary<string, Dictionary<int, DailyRewardRef>>();
		foreach (DailyRewardBookData.Rewards lstReward in XMLBook.instance.dailyRewardBook.lstRewards)
		{
			string link = lstReward.link;
			Dictionary<int, DailyRewardRef> dictionary = new Dictionary<int, DailyRewardRef>();
			foreach (DailyRewardBookData.Daily item in lstReward.lstDaily)
			{
				dictionary.Add(item.day, new DailyRewardRef(item.day, item.items));
			}
			_dailies.Add(link, dictionary);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static DailyRewardRef Lookup(int day, string rewardKey)
	{
		if (_dailies.ContainsKey(rewardKey))
		{
			Dictionary<int, DailyRewardRef> dictionary = _dailies[rewardKey];
			if (dictionary.ContainsKey(day))
			{
				return dictionary[day];
			}
			Debug.Log("DailyRewardBook::Lookup ::::::::: DAY NOT EXIST");
			return null;
		}
		return null;
	}

	public static int getSizeByRewardKey(string rewardKey)
	{
		if (_dailies.ContainsKey(rewardKey))
		{
			return _dailies[rewardKey].Count;
		}
		return 0;
	}
}
