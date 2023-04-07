using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.pvp;

public class PvPEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		BaseEventBookData.Event[] array = XMLBook.instance.PVPEventBook.events.lstEvent.ToArray();
		foreach (BaseEventBookData.Event @event in array)
		{
			if (@event != null)
			{
				_events.Add(@event.id, new PvPEventRef(@event));
			}
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.PVPEventBook.bonuses);
		yield return null;
		if (onUpdatedProgress != null && onUpdatedProgress.Target != null && !onUpdatedProgress.Target.Equals(null))
		{
			onUpdatedProgress(XMLBook.instance.UpdateProgress());
		}
	}

	public static PvPEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as PvPEventRef;
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

	public static PvPEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as PvPEventRef;
	}

	public static PvPEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as PvPEventRef;
	}

	public static PvPEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as PvPEventRef;
	}
}
