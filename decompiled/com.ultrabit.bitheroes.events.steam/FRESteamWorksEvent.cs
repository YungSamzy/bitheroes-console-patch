using UnityEngine;

namespace com.ultrabit.bitheroes.events.steam;

public class FRESteamWorksEvent
{
	public const string MICRO_TXN_AUTH_RESPONSE = "MICRO_TXN_AUTH_RESPONSE";

	public const string OVERLAY_ACTIVATED = "OVERLAY_ACTIVATED";

	private string _eventType;

	private bool _success;

	private Object _parameters;

	public string eventType => _eventType;

	public bool success => _success;

	public Object parameters => _parameters;

	public FRESteamWorksEvent(string type, bool success = true, Object parameters = null)
	{
		_eventType = type;
		_success = success;
		_parameters = parameters;
	}
}
