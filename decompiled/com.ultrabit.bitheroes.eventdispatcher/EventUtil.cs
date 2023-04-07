namespace com.ultrabit.bitheroes.eventdispatcher;

public static class EventUtil
{
	private static EventDispatcher dispatcher = new EventDispatcher();

	public static void AddListener(string eventType, EventListener.EventHandler eventHandler)
	{
		dispatcher.AddListener(eventType, eventHandler);
	}

	public static void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
	{
		dispatcher.RemoveListener(eventType, eventHandler);
	}

	public static bool HasListener(string eventType)
	{
		return dispatcher.HasListener(eventType);
	}

	public static bool HandlerHasListener(string eventType, EventListener.EventHandler eventHandler)
	{
		return dispatcher.HandlerHasListener(eventType, eventHandler);
	}

	public static void DispatchEvent(string eventType, params object[] args)
	{
		dispatcher.DispatchEvent(eventType, args);
	}

	public static void Clear()
	{
		dispatcher.Clear();
	}
}
