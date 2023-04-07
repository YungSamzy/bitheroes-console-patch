using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character.achievements;

public class CharacterAchievements
{
	private Dictionary<int, CharacterAchievementData> _achievements;

	private List<CharacterAchievementData> _activeAchievements;

	public List<CharacterAchievementData> lootedAchievements
	{
		get
		{
			List<CharacterAchievementData> list = new List<CharacterAchievementData>();
			foreach (CharacterAchievementData value in _achievements.Values)
			{
				if (value.looted)
				{
					list.Add(value);
				}
			}
			return list;
		}
	}

	public Dictionary<int, CharacterAchievementData> achievements => _achievements;

	public List<CharacterAchievementData> activeAchievements => _activeAchievements;

	public CharacterAchievements(Dictionary<int, CharacterAchievementData> achievements, List<CharacterAchievementData> activeAchievements)
	{
		_achievements = achievements;
		_activeAchievements = activeAchievements;
	}

	public static CharacterAchievements fromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("achive0");
		if (sFSArray == null)
		{
			return null;
		}
		Dictionary<int, CharacterAchievementData> dictionary = new Dictionary<int, CharacterAchievementData>();
		List<CharacterAchievementData> list = new List<CharacterAchievementData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			CharacterAchievementData characterAchievementData = CharacterAchievementData.fromSFSObject(sFSArray.GetSFSObject(i));
			dictionary.Add(i, characterAchievementData);
			if (!characterAchievementData.looted)
			{
				list.Add(characterAchievementData);
			}
		}
		return new CharacterAchievements(dictionary, list);
	}
}
