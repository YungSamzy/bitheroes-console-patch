using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.invasion;

public class InvasionEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	private static List<InvasionEventPoolRef> _level_pools;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		_level_pools = new List<InvasionEventPoolRef>();
		foreach (BaseEventBookData.Pool item in XMLBook.instance.invasionEventBook.levelPools.lstPool)
		{
			_level_pools.Add(new InvasionEventPoolRef(item));
		}
		foreach (BaseEventBookData.Event item2 in XMLBook.instance.invasionEventBook.events.lstEvent)
		{
			_events.Add(item2.id, new InvasionEventRef(item2));
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.invasionEventBook.bonuses);
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static InvasionEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as InvasionEventRef;
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

	public static InvasionEventPoolRef LookupPool(string link)
	{
		foreach (InvasionEventPoolRef level_pool in _level_pools)
		{
			if (level_pool.link.Equals(link.ToLowerInvariant()))
			{
				return level_pool;
			}
		}
		return null;
	}

	public static InvasionEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as InvasionEventRef;
	}

	public static InvasionEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as InvasionEventRef;
	}

	public static InvasionEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as InvasionEventRef;
	}
}
