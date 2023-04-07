using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.adGor;

public class AdGorEvent : CustomSFSXEvent
{
	public const string ADGOR_STEP_CHANGE = "ADGOR_STEP_CHANGE";

	public const string ADGOR_TIMER_FINISH = "ADGOR_TIMER_FINISH";

	public const string ADGOR_TIMER_INIT = "ADGOR_TIMER_INIT";

	public const string ADGOR_UPDATE = "ADGOR_UPDATE";

	public const string ADGOR_COOLDOWN_UPDATE = "ADGOR_COOLDOWN_UPDATE";

	private string _eventType;

	private object _parameters;

	public object parameters => _parameters;

	public AdGorEvent(string type, object parameters = null)
		: base(type)
	{
		_eventType = type;
		_parameters = parameters;
	}

	public override CustomSFSXEvent clone()
	{
		return new AdGorEvent(eventType, parameters);
	}
}
