using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronSourceJSON;
using UnityEngine;

public class IronSourceInterstitialEvents : MonoBehaviour
{
	private static event Action<IronSourceAdInfo> _onAdReadyEvent;

	public static event Action<IronSourceAdInfo> onAdReadyEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdReadyEvent == null || !IronSourceInterstitialEvents._onAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onAdReadyEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdReadyEvent != null && IronSourceInterstitialEvents._onAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onAdReadyEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onAdLoadFailedEvent;

	public static event Action<IronSourceError> onAdLoadFailedEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdLoadFailedEvent == null || !IronSourceInterstitialEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdLoadFailedEvent != null && IronSourceInterstitialEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdOpenedEvent;

	public static event Action<IronSourceAdInfo> onAdOpenedEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdOpenedEvent == null || !IronSourceInterstitialEvents._onAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdOpenedEvent != null && IronSourceInterstitialEvents._onAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onAdOpenedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdClosedEvent;

	public static event Action<IronSourceAdInfo> onAdClosedEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdClosedEvent == null || !IronSourceInterstitialEvents._onAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdClosedEvent != null && IronSourceInterstitialEvents._onAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onAdClosedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdShowSucceededEvent;

	public static event Action<IronSourceAdInfo> onAdShowSucceededEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdShowSucceededEvent == null || !IronSourceInterstitialEvents._onAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onAdShowSucceededEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdShowSucceededEvent != null && IronSourceInterstitialEvents._onAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onAdShowSucceededEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError, IronSourceAdInfo> _onAdShowFailedEvent;

	public static event Action<IronSourceError, IronSourceAdInfo> onAdShowFailedEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdShowFailedEvent == null || !IronSourceInterstitialEvents._onAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdShowFailedEvent != null && IronSourceInterstitialEvents._onAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdClickedEvent;

	public static event Action<IronSourceAdInfo> onAdClickedEvent
	{
		add
		{
			if (IronSourceInterstitialEvents._onAdClickedEvent == null || !IronSourceInterstitialEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceInterstitialEvents._onAdClickedEvent != null && IronSourceInterstitialEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent -= value;
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "IronSourceInterstitialEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onAdReady(string args)
	{
		if (IronSourceInterstitialEvents._onAdReadyEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceInterstitialEvents._onAdReadyEvent(obj);
		}
	}

	public void onAdLoadFailed(string description)
	{
		if (IronSourceInterstitialEvents._onAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceInterstitialEvents._onAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onAdOpened(string args)
	{
		if (IronSourceInterstitialEvents._onAdOpenedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceInterstitialEvents._onAdOpenedEvent(obj);
		}
	}

	public void onAdClosed(string args)
	{
		if (IronSourceInterstitialEvents._onAdClosedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceInterstitialEvents._onAdClosedEvent(obj);
		}
	}

	public void onAdShowSucceeded(string args)
	{
		if (IronSourceInterstitialEvents._onAdShowSucceededEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceInterstitialEvents._onAdShowSucceededEvent(obj);
		}
	}

	public void onAdShowFailed(string args)
	{
		if (IronSourceInterstitialEvents._onAdShowFailedEvent != null)
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[0]);
			IronSourceAdInfo arg = new IronSourceAdInfo(list[1].ToString());
			IronSourceInterstitialEvents._onAdShowFailedEvent(errorFromErrorObject, arg);
		}
	}

	public void onAdClicked(string args)
	{
		if (IronSourceInterstitialEvents._onAdClickedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceInterstitialEvents._onAdClickedEvent(obj);
		}
	}

	private IronSourceError getErrorFromErrorObject(object descriptionObject)
	{
		Dictionary<string, object> dictionary = null;
		if (descriptionObject is IDictionary)
		{
			dictionary = descriptionObject as Dictionary<string, object>;
		}
		else if (descriptionObject is string && !string.IsNullOrEmpty(descriptionObject.ToString()))
		{
			dictionary = Json.Deserialize(descriptionObject.ToString()) as Dictionary<string, object>;
		}
		IronSourceError result = new IronSourceError(-1, "");
		if (dictionary != null && dictionary.Count > 0)
		{
			int errorCode = Convert.ToInt32(dictionary["error_code"].ToString());
			string errorDescription = dictionary["error_description"].ToString();
			result = new IronSourceError(errorCode, errorDescription);
		}
		return result;
	}

	private IronSourcePlacement getPlacementFromObject(object placementObject)
	{
		Dictionary<string, object> dictionary = null;
		if (placementObject is IDictionary)
		{
			dictionary = placementObject as Dictionary<string, object>;
		}
		else if (placementObject is string)
		{
			dictionary = Json.Deserialize(placementObject.ToString()) as Dictionary<string, object>;
		}
		IronSourcePlacement result = null;
		if (dictionary != null && dictionary.Count > 0)
		{
			int rewardAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
			string rewardName = dictionary["placement_reward_name"].ToString();
			result = new IronSourcePlacement(dictionary["placement_name"].ToString(), rewardName, rewardAmount);
		}
		return result;
	}
}
