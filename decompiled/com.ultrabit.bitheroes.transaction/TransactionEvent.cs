using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.transaction;

public class TransactionEvent : CustomSFSXEvent
{
	public const string PURCHASE_COMPLETE = "PURCHASE_COMPLETE";

	private string _eventType;

	public TransactionEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new TransactionEvent(eventType, _dispatcher);
	}
}
