namespace com.ultrabit.bitheroes.eventdispatcher;

public class EventListener
{
	public delegate void EventHandler(EventArgs eventArgs);

	public EventHandler eventHandler;

	public void Invoke(EventArgs eventArgs)
	{
		if (eventHandler != null)
		{
			eventHandler(eventArgs);
		}
	}

	public void Clear()
	{
		eventHandler = null;
	}
}
