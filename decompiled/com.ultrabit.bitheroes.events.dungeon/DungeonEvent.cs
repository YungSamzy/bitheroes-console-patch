using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.dungeon;

public class DungeonEvent : CustomSFSXEvent
{
	public const string ENTITY_SHIELD = "ENTITY_SHIELD";

	public const string ENTITY_HEALTH = "ENTITY_HEALTH";

	public const string ENTITY_UPDATE = "ENTITY_UPDATE";

	public const string ENTITY_METER = "ENTITY_METER";

	public const string CONSUMABLE_CHANGE = "CONSUMABLE_CHANGE";

	private string _eventType;

	public DungeonEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new DungeonEvent(eventType, _dispatcher);
	}
}
