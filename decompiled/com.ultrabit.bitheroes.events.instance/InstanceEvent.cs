using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.instance;

public class InstanceEvent : CustomSFSXEvent
{
	public const string BOBBER_COMPLETE = "BOBBER_COMPLETE";

	private string _eventType;

	public InstanceEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new InstanceEvent(eventType, _dispatcher);
	}
}
