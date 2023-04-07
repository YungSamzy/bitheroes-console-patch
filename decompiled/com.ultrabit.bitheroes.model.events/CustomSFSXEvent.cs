using Sfs2X.Core;

namespace com.ultrabit.bitheroes.model.events;

public class CustomSFSXEvent : BaseEvent
{
	public static readonly string COMPLETE = "complete";

	public static readonly string CLOSE = "close";

	public static readonly string CHANGE = "change";

	protected EventDispatcher _dispatcher;

	public string eventType;

	public object currentTarget;

	public EventDispatcher dispatcher => _dispatcher;

	public CustomSFSXEvent(string type, EventDispatcher dispatcher = null)
		: base(type)
	{
		_dispatcher = dispatcher;
	}

	public static string getEvent(int id)
	{
		return id.ToString();
	}

	public virtual CustomSFSXEvent clone()
	{
		return this;
	}
}
