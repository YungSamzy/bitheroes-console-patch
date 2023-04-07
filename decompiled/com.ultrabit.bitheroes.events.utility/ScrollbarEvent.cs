using UnityEngine;

namespace com.ultrabit.bitheroes.events.utility;

public class ScrollbarEvent
{
	public const string SWIPE_BEGIN = "SWIPE_BEGIN";

	public const string SWIPE_END = "SWIPE_END";

	public const string TRIGGERED = "TRIGGERED";

	private string _eventType;

	private Object _parameters;

	public string eventType => _eventType;

	public Object parameters => _parameters;

	public ScrollbarEvent(string type, Object parameters = null)
	{
		_eventType = type;
		_parameters = parameters;
	}
}
