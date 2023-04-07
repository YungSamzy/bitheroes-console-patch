using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.battle;

public class BattleEvent : CustomSFSXEvent
{
	public const string HEALTH_CHANGE = "HEALTH_CHANGE";

	public const string TIME_CHANGE = "TIME_CHANGE";

	public const string METER_CHANGE = "METER_CHANGE";

	public const string CONSUMABLE_CHANGE = "CONSUMABLE_CHANGE";

	public const string POSITION_CHANGE = "POSITION_CHANGE";

	public const string ALPHA_CHANGE = "ALPHA_CHANGE";

	public const string SHIELD_CHANGE = "SHIELD_CHANGE";

	public const string DAMAGE_GAINED_CHANGE = "DAMAGE_GAINED_CHANGE";

	public const string ABILITY_DATA_CHANGE = "ABILITY_DATA_CHANGE";

	private string _eventType;

	public BattleEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new BattleEvent(eventType, _dispatcher);
	}
}
