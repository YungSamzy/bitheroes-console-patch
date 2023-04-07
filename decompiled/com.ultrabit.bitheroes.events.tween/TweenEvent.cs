using UnityEngine;

namespace com.ultrabit.bitheroes.events.tween;

public class TweenEvent
{
	public const string SCROLL_IN_START = "SCROLL_IN_START";

	public const string SCROLL_IN_COMPLETE = "SCROLL_IN_COMPLETE";

	public const string SCROLL_OUT_START = "SCROLL_OUT_START";

	public const string SCROLL_OUT_COMPLETE = "SCROLL_OUT_COMPLETE";

	private string _eventType;

	private Object _parameters;

	public string eventType => _eventType;

	public Object parameters => _parameters;

	public TweenEvent(string type, Object parameters = null)
	{
		_eventType = type;
		_parameters = parameters;
	}
}
