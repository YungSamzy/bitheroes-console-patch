using System.Collections.Generic;
using UnityEngine;

public class IronSourceConfig
{
	private const string unsupportedPlatformStr = "Unsupported Platform";

	private static IronSourceConfig _instance;

	public static IronSourceConfig Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new IronSourceConfig();
			}
			return _instance;
		}
	}

	public void setLanguage(string language)
	{
		Debug.Log("Unsupported Platform");
	}

	public void setClientSideCallbacks(bool status)
	{
		Debug.Log("Unsupported Platform");
	}

	public void setRewardedVideoCustomParams(Dictionary<string, string> rewardedVideoCustomParams)
	{
		Debug.Log("Unsupported Platform");
	}

	public void setOfferwallCustomParams(Dictionary<string, string> offerwallCustomParams)
	{
		Debug.Log("Unsupported Platform");
	}

	public IronSourceConfig()
	{
		Debug.Log("Unsupported Platform");
	}
}
