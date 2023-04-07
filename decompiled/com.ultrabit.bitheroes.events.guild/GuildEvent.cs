using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.guild;

public class GuildEvent : CustomSFSXEvent
{
	private string _eventType;

	public GuildEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new GuildEvent(eventType, _dispatcher);
	}
}
