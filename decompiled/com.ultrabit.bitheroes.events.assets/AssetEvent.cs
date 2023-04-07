using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.assets;

public class AssetEvent : CustomSFSXEvent
{
	public const string ANIMATION_END = "ANIMATION_END";

	public const string ANIMATION_TRIGGER = "ANIMATION_TRIGGER";

	private string _eventType;

	public AssetEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new AssetEvent(eventType, _dispatcher);
	}
}
