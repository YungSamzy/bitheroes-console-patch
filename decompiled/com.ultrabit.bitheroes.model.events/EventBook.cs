using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

public class EventBook
{
	public static List<EventRef> GetEvents(int eventType)
	{
		return eventType switch
		{
			1 => PvPEventBook.events, 
			2 => RiftEventBook.events, 
			3 => GauntletEventBook.events, 
			4 => GvGEventBook.events, 
			5 => InvasionEventBook.events, 
			6 => FishingEventBook.events, 
			7 => GvEEventBook.events, 
			_ => null, 
		};
	}

	public static List<EventRef> GetSortedEvents(int eventType)
	{
		return GetSortedEvents(GetEvents(eventType));
	}

	public static List<EventRef> GetSortedEvents(int eventType, int qty)
	{
		List<EventRef> sortedEvents = GetSortedEvents(GetEvents(eventType));
		if (sortedEvents.Count > qty)
		{
			sortedEvents.RemoveRange(qty, sortedEvents.Count - qty);
		}
		return sortedEvents;
	}

	protected static List<EventRef> GetSortedEvents(List<EventRef> events)
	{
		List<EventRef> list = new List<EventRef>();
		foreach (EventRef @event in events)
		{
			if (@event.GetDateRef().getMillisecondsUntilStart() < 0)
			{
				list.Add(@event);
			}
		}
		list.Sort((EventRef itemA, EventRef itemB) => itemB.GetDateRef().startMilliseconds.CompareTo(itemA.GetDateRef().startMilliseconds));
		return list;
	}

	public static EventRef GetCurrentEventRef(List<EventRef> events, bool debug = false)
	{
		DateTime date = ServerExtension.instance.GetDate();
		foreach (EventRef @event in events)
		{
			if (date.CompareTo(@event.GetDateRef().startDate) > 0 && date.CompareTo(@event.GetDateRef().endDate) < 0)
			{
				return @event;
			}
		}
		return null;
	}

	protected static EventRef GetNextEventRef(List<EventRef> events)
	{
		EventRef eventRef = null;
		foreach (EventRef @event in events)
		{
			long millisecondsUntilStart = @event.GetDateRef().getMillisecondsUntilStart();
			if (millisecondsUntilStart > 0 && (eventRef == null || millisecondsUntilStart < eventRef.GetDateRef().getMillisecondsUntilStart()))
			{
				eventRef = @event;
			}
		}
		return eventRef;
	}

	protected static EventRef GetPreviousEventRef(List<EventRef> events)
	{
		EventRef eventRef = null;
		foreach (EventRef @event in events)
		{
			long millisecondsUntilStart = @event.GetDateRef().getMillisecondsUntilStart();
			if (millisecondsUntilStart < 0 && (eventRef == null || millisecondsUntilStart > eventRef.GetDateRef().getMillisecondsUntilStart()))
			{
				eventRef = @event;
			}
		}
		return eventRef;
	}

	public static EventRef GetEventRefByID(List<EventRef> events, int id)
	{
		foreach (EventRef @event in events)
		{
			if (@event.id.Equals(id))
			{
				return @event;
			}
		}
		return null;
	}
}
