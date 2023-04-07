using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.payment;

public class PaymentEvent : CustomSFSXEvent
{
	public const string SUCCESS = "SUCCESS";

	public const string FAIL = "FAIL";

	private string _eventType;

	public PaymentEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new PaymentEvent(eventType, _dispatcher);
	}
}
