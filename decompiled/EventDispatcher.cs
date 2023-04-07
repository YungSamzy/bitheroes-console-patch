using com.ultrabit.bitheroes.model.events;
using Sfs2X.Core;

public class EventDispatcher : Sfs2X.Core.EventDispatcher
{
	public EventDispatcher(object target = null)
		: base(target)
	{
	}

	public void AddEventListener(string eventType, EventListenerDelegate listener, bool weak = true)
	{
		base.AddEventListener(eventType, listener);
	}

	public new void DispatchEvent(BaseEvent evt)
	{
		if (evt is CustomSFSXEvent)
		{
			base.DispatchEvent((BaseEvent)(evt as CustomSFSXEvent));
		}
		else
		{
			base.DispatchEvent(evt);
		}
	}

	public static EventDispatcher fromBaseEvent(BaseEvent e)
	{
		if (e is CustomSFSXEvent)
		{
			return (e as CustomSFSXEvent).dispatcher;
		}
		return null;
	}
}
