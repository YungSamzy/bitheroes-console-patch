using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.daily;

public class DailyQuestBook
{
	private static List<DailyQuestRef> _quests;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_quests = new List<DailyQuestRef>();
		int num = 0;
		foreach (DailyQuestBookData.Quest item in XMLBook.instance.dailyQuestBook.lstQuest)
		{
			DailyQuestRef dailyQuestRef = new DailyQuestRef(item.id, item);
			dailyQuestRef.LoadDetails(item);
			_quests.Insert(item.id, dailyQuestRef);
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static DailyQuestRef Lookup(int id)
	{
		if (id < 0 || id >= _quests.Count)
		{
			return null;
		}
		return _quests[id];
	}
}
