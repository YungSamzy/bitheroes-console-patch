using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.rift;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.gauntlet;

public class GauntletEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, GauntletEventTierRef> _tiers;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		_tiers = new Dictionary<int, GauntletEventTierRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		foreach (BaseEventBookData.Tier item in XMLBook.instance.gauntletEventBook.tiers.lstTier)
		{
			GauntletEventTierRef gauntletEventTierRef = new GauntletEventTierRef(item.id, item);
			gauntletEventTierRef.LoadDetails(item);
			_tiers.Add(item.id, gauntletEventTierRef);
		}
		foreach (BaseEventBookData.Event item2 in XMLBook.instance.gauntletEventBook.events.lstEvent)
		{
			_events.Add(item2.id, new GauntletEventRef(item2));
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.gauntletEventBook.bonuses);
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GauntletEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as GauntletEventRef;
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

	public static GauntletEventTierRef GetDifficultyTier(int difficulty)
	{
		GauntletEventTierRef gauntletEventTierRef = null;
		foreach (KeyValuePair<int, GauntletEventTierRef> tier in _tiers)
		{
			GauntletEventTierRef value = tier.Value;
			if (gauntletEventTierRef == null || (value.difficulty <= difficulty && value.difficulty > gauntletEventTierRef.difficulty))
			{
				gauntletEventTierRef = value;
			}
		}
		return gauntletEventTierRef;
	}

	public static GauntletEventTierRef getDifficultyTierLimitedByCharTier(int difficulty)
	{
		GauntletEventTierRef gauntletEventTierRef = null;
		int tier = GameData.instance.PROJECT.character.tier;
		foreach (KeyValuePair<int, GauntletEventTierRef> tier2 in _tiers)
		{
			if (tier2.Value != null && tier2.Value.tierName <= tier && (gauntletEventTierRef == null || (tier2.Value.difficulty <= difficulty && tier2.Value.difficulty > gauntletEventTierRef.difficulty)))
			{
				gauntletEventTierRef = tier2.Value;
			}
		}
		return gauntletEventTierRef;
	}

	public static GauntletEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as GauntletEventRef;
	}

	public static GauntletEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as GauntletEventRef;
	}

	public static GauntletEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as GauntletEventRef;
	}
}
