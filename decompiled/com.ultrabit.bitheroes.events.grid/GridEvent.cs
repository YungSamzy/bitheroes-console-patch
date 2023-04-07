using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.grid;

public class GridEvent : CustomSFSXEvent
{
	public const string SCALE_UPDATE = "SCALE_UPDATE";

	public const string PATH_UPDATE = "PATH_UPDATE";

	public const string POSITION_UPDATE = "POSITION_UPDATE";

	public const string MOVEMENT_UPDATE = "MOVEMENT_UPDATE";

	public const string MOVEMENT_START = "MOVEMENT_START";

	public const string MOVEMENT_END = "MOVEMENT_END";

	public const string MOVEMENT_CHANGE = "MOVEMENT_CHANGE";

	public const string MOVEMENT_STOP = "MOVEMENT_STOP";

	private string _eventType;

	public GridEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new GridEvent(eventType, _dispatcher);
	}
}
