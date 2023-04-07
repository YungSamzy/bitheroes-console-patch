namespace com.ultrabit.bitheroes.eventdispatcher;

public class EventArgs
{
	public readonly string type;

	public readonly object[] args;

	public EventArgs(string type)
	{
		this.type = type;
	}

	public EventArgs(string type, params object[] args)
	{
		this.type = type;
		this.args = args;
	}
}
