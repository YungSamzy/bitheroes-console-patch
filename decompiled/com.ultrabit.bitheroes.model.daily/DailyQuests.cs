using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.daily;

public class DailyQuests
{
	private List<DailyQuestData> _quests;

	private bool _updated;

	public List<DailyQuestData> quests => _quests;

	public bool updated => _updated;

	public DailyQuests(List<DailyQuestData> quests)
	{
		_quests = quests;
	}

	public DailyQuestData getQuestDataFromID(int id)
	{
		foreach (DailyQuestData quest in _quests)
		{
			if (quest.questRef.id == id)
			{
				return quest;
			}
		}
		return null;
	}

	public void setUpdated(bool v)
	{
		_updated = v;
	}

	public bool hasQuestDataLootable()
	{
		foreach (DailyQuestData quest in _quests)
		{
			if (quest.completed && !quest.looted)
			{
				return true;
			}
		}
		return false;
	}

	public static DailyQuests fromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("dail0");
		List<DailyQuestData> list = new List<DailyQuestData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(DailyQuestData.fromSFSObject(sFSObject));
		}
		return new DailyQuests(list);
	}
}
