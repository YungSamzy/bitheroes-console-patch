using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.character.achievements;

public class CharacterAchievementBook
{
	private static List<CharacterAchievementRef> _achievements;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_achievements = new List<CharacterAchievementRef>();
		int num = 0;
		foreach (CharacterAchievementBookData.Achievement item in XMLBook.instance.characterAchievementBook.achievements.lstAchievement)
		{
			CharacterAchievementRef characterAchievementRef = new CharacterAchievementRef(item.id, item);
			characterAchievementRef.LoadDetails(item);
			_achievements.Add(characterAchievementRef);
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static CharacterAchievementRef Lookup(int id)
	{
		foreach (CharacterAchievementRef achievement in _achievements)
		{
			if (achievement.id == id)
			{
				return achievement;
			}
		}
		return null;
	}
}
