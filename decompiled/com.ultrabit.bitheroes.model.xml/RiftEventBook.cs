using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.rift;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.xml;

public class RiftEventBook : EventBook
{
	private static Dictionary<int, EventRef> _events;

	private static Dictionary<int, RiftEventTierRef> _tiers;

	private static Dictionary<int, CurrencyBonusRef> _bonuses;

	public static List<EventRef> events => new List<EventRef>(_events.Values);

	public static int sizeBonuses => _bonuses.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_events = new Dictionary<int, EventRef>();
		_tiers = new Dictionary<int, RiftEventTierRef>();
		_bonuses = new Dictionary<int, CurrencyBonusRef>();
		foreach (BaseEventBookData.Tier item in XMLBook.instance.riftEventBook.tiers.lstTier)
		{
			_tiers.Add(item.id, new RiftEventTierRef(item.id, item));
		}
		foreach (BaseEventBookData.Event item2 in XMLBook.instance.riftEventBook.events.lstEvent)
		{
			_events.Add(item2.id, new RiftEventRef(item2));
		}
		_bonuses = CurrencyBonusRef.FromXMLData(XMLBook.instance.riftEventBook.bonuses);
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static RiftEventRef Lookup(int id)
	{
		if (_events.ContainsKey(id))
		{
			return _events[id] as RiftEventRef;
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

	public static RiftEventTierRef GetDifficultyTier(int difficulty)
	{
		RiftEventTierRef riftEventTierRef = null;
		foreach (KeyValuePair<int, RiftEventTierRef> tier in _tiers)
		{
			RiftEventTierRef value = tier.Value;
			if (riftEventTierRef == null || (value.difficulty <= difficulty && value.difficulty > riftEventTierRef.difficulty))
			{
				riftEventTierRef = value;
			}
		}
		return riftEventTierRef;
	}

	public static RiftEventTierRef getDifficultyTierLimitedByCharTier(int difficulty)
	{
		RiftEventTierRef riftEventTierRef = null;
		int tier = GameData.instance.PROJECT.character.tier;
		foreach (KeyValuePair<int, RiftEventTierRef> tier2 in _tiers)
		{
			if (tier2.Value != null && tier2.Value.tierName <= tier && (riftEventTierRef == null || (tier2.Value.difficulty <= difficulty && tier2.Value.difficulty > riftEventTierRef.difficulty)))
			{
				riftEventTierRef = tier2.Value;
			}
		}
		return riftEventTierRef;
	}

	public static RiftEventRef GetPreviousEventRef()
	{
		return EventBook.GetPreviousEventRef(events) as RiftEventRef;
	}

	public static RiftEventRef GetNextEventRef()
	{
		return EventBook.GetNextEventRef(events) as RiftEventRef;
	}

	public static RiftEventRef GetCurrentEventRef()
	{
		return EventBook.GetCurrentEventRef(events) as RiftEventRef;
	}
}
