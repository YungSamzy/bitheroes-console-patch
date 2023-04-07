using System;
using System.Collections.Generic;

namespace com.ultrabit.bitheroes.eventdispatcher;

public class EventDispatcher
{
	private Dictionary<string, EventListener> dic = new Dictionary<string, EventListener>();

	public void AddListener(string eventType, EventListener.EventHandler eventHandler)
	{
		if (!dic.TryGetValue(eventType, out var value))
		{
			value = new EventListener();
			dic.Add(eventType, value);
		}
		EventListener eventListener = value;
		eventListener.eventHandler = (EventListener.EventHandler)Delegate.Combine(eventListener.eventHandler, eventHandler);
	}

	public void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
	{
		if (dic.TryGetValue(eventType, out var value))
		{
			EventListener eventListener = value;
			eventListener.eventHandler = (EventListener.EventHandler)Delegate.Remove(eventListener.eventHandler, eventHandler);
		}
	}

	public bool HasListener(string eventType)
	{
		return dic.ContainsKey(eventType);
	}

	public bool HandlerHasListener(string eventType, EventListener.EventHandler eventHandler)
	{
		if (dic.TryGetValue(eventType, out var value))
		{
			if (value.eventHandler == eventHandler)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void DispatchEvent(string eventType, params object[] args)
	{
		if (dic.TryGetValue(eventType, out var value))
		{
			EventArgs eventArgs = ((args != null && args.Length != 0) ? new EventArgs(eventType, args) : new EventArgs(eventType));
			value.Invoke(eventArgs);
		}
	}

	public void Clear()
	{
		foreach (EventListener value in dic.Values)
		{
			value.Clear();
		}
		dic.Clear();
	}
}
