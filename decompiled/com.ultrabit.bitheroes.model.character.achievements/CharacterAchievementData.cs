using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character.achievements;

public class CharacterAchievementData
{
	private CharacterAchievementRef _achievementRef;

	private int _progress;

	private bool _completed;

	private bool _looted;

	public CharacterAchievementRef achievementRef => _achievementRef;

	public int progress => _progress;

	public bool completed => _completed;

	public bool looted => _looted;

	public CharacterAchievementData(CharacterAchievementRef achievementRef, int progress, bool completed, bool looted)
	{
		_achievementRef = achievementRef;
		_progress = progress;
		_completed = completed;
		_looted = looted;
	}

	public static CharacterAchievementData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("achive1");
		return new CharacterAchievementData(progress: sfsob.GetInt("achive2"), completed: sfsob.GetBool("achive3"), looted: sfsob.GetBool("achive4"), achievementRef: CharacterAchievementBook.Lookup(@int));
	}
}
