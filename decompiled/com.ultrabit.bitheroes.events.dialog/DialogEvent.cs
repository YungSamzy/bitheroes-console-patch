using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.dialog;

public class DialogEvent : CustomSFSXEvent
{
	public const string DIALOG_CLOSE = "DIALOG_CLOSE";

	public const string DIALOG_YES = "DIALOG_YES";

	public const string DIALOG_NO = "DIALOG_NO";

	public const string PURCHASE_COMPLETE = "PURCHASE_COMPLETE";

	private string _eventType;

	public DialogEvent(string type, EventDispatcher parameters = null)
		: base(type)
	{
		_eventType = type;
		_dispatcher = parameters;
		eventType = _eventType;
	}

	public override CustomSFSXEvent clone()
	{
		return new DialogEvent(eventType, _dispatcher);
	}
}
