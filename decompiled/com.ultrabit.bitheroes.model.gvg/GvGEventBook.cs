using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.gvg;

public class GvGEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		foreach (BaseEventBookData.Event item in XMLBook.instance.GVGEventBook.events.lstEvent)
		{
			_events.Add(item.id, new GvGEventRef(item));
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.GVGEventBook.bonuses);
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GvGEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as GvGEventRef;
		}
		return null;
	}

	public static CurrencyBonusRef LookupBonus(int id)
	{
		if (_bonuses.ContainsKey(id))
		{
			return _bonuses[id];
		}
		return null;
	}

	public static GvGEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as GvGEventRef;
	}

	public static GvGEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as GvGEventRef;
	}

	public static GvGEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as GvGEventRef;
	}
}
