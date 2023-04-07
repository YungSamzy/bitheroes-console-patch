using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.game;

public class GameEvent : CustomSFSXEvent
{
	public const string FRAME_UPDATE = "FRAME_UPDATE";

	public const string SCREEN_UPDATE = "SCREEN_UPDATE";

	public const string ORIENTATION_UPDATE = "ORIENTATION_UPDATE";

	private string _eventType;

	public GameEvent(string type, float timeDelta, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}
}
