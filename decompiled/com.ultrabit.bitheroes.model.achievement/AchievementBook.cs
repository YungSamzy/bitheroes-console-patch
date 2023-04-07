using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.achievement;

public class AchievementBook
{
	private static List<AchievementRef> _achievements;

	public static int size => _achievements.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_achievements = new List<AchievementRef>();
		foreach (AchievementBookData.Achievement item in XMLBook.instance.achievementBook.lstAchievement)
		{
			int id = item.id;
			int achievementType = AchievementRef.getAchievementType(item.type);
			int intFromStringProperty = Util.GetIntFromStringProperty(item.value);
			string achievementID = "";
			switch (AppInfo.platform)
			{
			case 1:
				achievementID = item.androidID;
				break;
			case 2:
				achievementID = item.iosID;
				break;
			case 7:
				achievementID = item.steamID;
				break;
			}
			int[] intArrayFromStringProperty = Util.GetIntArrayFromStringProperty(item.revealIDs);
			AchievementRef achievementRef = new AchievementRef(id, achievementType, intFromStringProperty, achievementID, intArrayFromStringProperty);
			achievementRef.LoadDetails(item);
			_achievements.Add(achievementRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static AchievementRef Lookup(int id)
	{
		if (id < 0 || id > size)
		{
			return null;
		}
		return _achievements[id];
	}

	public static AchievementRef LookupAchievementID(string achievementID)
	{
		foreach (AchievementRef achievement in _achievements)
		{
			if (achievement.achievementID.Equals(achievementID))
			{
				return achievement;
			}
		}
		return null;
	}

	public static List<AchievementRef> GetAchievements()
	{
		return _achievements;
	}
}
