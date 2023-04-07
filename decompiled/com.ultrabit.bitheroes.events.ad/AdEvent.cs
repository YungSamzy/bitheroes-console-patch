namespace com.ultrabit.bitheroes.events.ad;

public class AdEvent
{
	public const string AD_COMPLETE = "AD_COMPLETE";

	public const string AD_ABANDONED = "AD_ABANDONED";

	public const string AD_REWARDED = "AD_REWARDED";

	private string _eventType;

	private float _duration;

	private IronSourcePlacement _ironSourcePlacement;

	private IronSourceError _ironSourceError;

	public bool success => eventType.Equals("AD_REWARDED");

	public string eventType => _eventType;

	public float duration => _duration;

	public IronSourcePlacement ironSourcePlacement => _ironSourcePlacement;

	public IronSourceError ironSourceError => _ironSourceError;

	public AdEvent(string type, float duration, IronSourcePlacement ironSourcePlacement)
	{
		_eventType = type;
		_duration = duration;
		_ironSourcePlacement = ironSourcePlacement;
	}

	public AdEvent(string type, float duration, IronSourceError ironSourceError)
	{
		_eventType = type;
		_duration = duration;
		_ironSourceError = ironSourceError;
	}

	public AdEvent(string type, float duration)
	{
		_duration = duration;
		_eventType = type;
	}
}
