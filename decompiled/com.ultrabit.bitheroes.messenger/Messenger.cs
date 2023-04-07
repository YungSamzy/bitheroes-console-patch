using System.Collections.Generic;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.messenger;

public class Messenger
{
	private Dictionary<string, UnityEvent> eventTable = new Dictionary<string, UnityEvent>();

	public void AddListener(string eventType, UnityAction handler)
	{
		if (!eventTable.ContainsKey(eventType))
		{
			eventTable.Add(eventType, new UnityEvent());
		}
		eventTable[eventType].AddListener(handler);
	}

	public void RemoveListener(string eventType, UnityAction handler)
	{
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType].RemoveListener(handler);
		}
	}

	public void Broadcast(string eventType)
	{
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType].Invoke();
		}
	}
}
