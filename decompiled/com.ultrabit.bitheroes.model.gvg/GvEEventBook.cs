using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.gvg;

public class GvEEventBook : EventBook
{
	private static Dictionary<int, GvEZoneRef> _zones;

	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_zones = new Dictionary<int, GvEZoneRef>();
		_events = new Dictionary<int, EventRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		foreach (BaseEventBookData.Zone item in XMLBook.instance.GVEEventBook.zones.lstZone)
		{
			_zones.Add(item.id, new GvEZoneRef(item.id, item));
		}
		foreach (BaseEventBookData.Event item2 in XMLBook.instance.GVEEventBook.events.lstEvent)
		{
			_events.Add(item2.id, new GvEEventRef(item2));
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.GVEEventBook.bonuses);
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GvEEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as GvEEventRef;
		}
		return null;
	}

	public static GvEZoneRef LookupZone(int id)
	{
		if (_zones.ContainsKey(id))
		{
			return _zones[id];
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

	public static GvEEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as GvEEventRef;
	}

	public static GvEEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as GvEEventRef;
	}

	public static GvEEventRef GetCurrentEventRef(bool debug = false)
	{
		return EventBook.GetCurrentEventRef(events, debug) as GvEEventRef;
	}
}
