using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		foreach (BaseEventBookData.Event item in XMLBook.instance.fishingEventBook.events.lstEvent)
		{
			_events.Add(item.id, new FishingEventRef(item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static FishingEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as FishingEventRef;
		}
		return null;
	}

	public static FishingEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as FishingEventRef;
	}

	public static FishingEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as FishingEventRef;
	}

	public static FishingEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as FishingEventRef;
	}

	public static FishingEventRef GetEventRefByID(int id)
	{
		return EventBook.GetEventRefByID(events, id) as FishingEventRef;
	}
}
