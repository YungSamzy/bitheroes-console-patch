using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.daily;

public class DailyQuestData
{
	private DailyQuestRef _questRef;

	private int _progress;

	private bool _completed;

	private bool _looted;

	public float percentage
	{
		get
		{
			if (_completed)
			{
				return 1f;
			}
			if (_progress <= 0)
			{
				return 0f;
			}
			return (float)_progress / (float)_questRef.amount;
		}
	}

	public DailyQuestRef questRef => _questRef;

	public int progress => _progress;

	public bool completed => _completed;

	public bool looted => _looted;

	public int id => _questRef.id;

	public int rarity => _questRef.rarityRef.id;

	public DailyQuestData(DailyQuestRef questRef, int progress, bool completed, bool looted)
	{
		_questRef = questRef;
		_progress = progress;
		_completed = completed;
		_looted = looted;
	}

	public static DailyQuestData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("dail1");
		return new DailyQuestData(progress: sfsob.GetInt("dail2"), completed: sfsob.GetBool("dail3"), looted: sfsob.GetBool("dail4"), questRef: DailyQuestBook.Lookup(@int));
	}
}
