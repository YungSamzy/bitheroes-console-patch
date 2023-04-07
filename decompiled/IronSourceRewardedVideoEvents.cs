using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronSourceJSON;
using UnityEngine;

public class IronSourceRewardedVideoEvents : MonoBehaviour
{
	private static event Action<IronSourceError, IronSourceAdInfo> _onAdShowFailedEvent;

	public static event Action<IronSourceError, IronSourceAdInfo> onAdShowFailedEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdShowFailedEvent == null || !IronSourceRewardedVideoEvents._onAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdShowFailedEvent != null && IronSourceRewardedVideoEvents._onAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdOpenedEvent;

	public static event Action<IronSourceAdInfo> onAdOpenedEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdOpenedEvent == null || !IronSourceRewardedVideoEvents._onAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdOpenedEvent != null && IronSourceRewardedVideoEvents._onAdOpenedEvent.GetInvocationList().Contains(value))
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
			if (IronSourceRewardedVideoEvents._onAdClosedEvent == null || !IronSourceRewardedVideoEvents._onAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdClosedEvent != null && IronSourceRewardedVideoEvents._onAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onAdClosedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement, IronSourceAdInfo> _onAdRewardedEvent;

	public static event Action<IronSourcePlacement, IronSourceAdInfo> onAdRewardedEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdRewardedEvent == null || !IronSourceRewardedVideoEvents._onAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onAdRewardedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdRewardedEvent != null && IronSourceRewardedVideoEvents._onAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onAdRewardedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement, IronSourceAdInfo> _onAdClickedEvent;

	public static event Action<IronSourcePlacement, IronSourceAdInfo> onAdClickedEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdClickedEvent == null || !IronSourceRewardedVideoEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdClickedEvent != null && IronSourceRewardedVideoEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdAvailableEvent;

	public static event Action<IronSourceAdInfo> onAdAvailableEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdAvailableEvent == null || !IronSourceRewardedVideoEvents._onAdAvailableEvent.GetInvocationList().Contains(value))
			{
				_onAdAvailableEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdAvailableEvent != null && IronSourceRewardedVideoEvents._onAdAvailableEvent.GetInvocationList().Contains(value))
			{
				_onAdAvailableEvent -= value;
			}
		}
	}

	private static event Action _onAdUnavailableEvent;

	public static event Action onAdUnavailableEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdUnavailableEvent == null || !IronSourceRewardedVideoEvents._onAdUnavailableEvent.GetInvocationList().Contains(value))
			{
				_onAdUnavailableEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdUnavailableEvent != null && IronSourceRewardedVideoEvents._onAdUnavailableEvent.GetInvocationList().Contains(value))
			{
				_onAdUnavailableEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onAdLoadFailedEvent;

	public static event Action<IronSourceError> onAdLoadFailedEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdLoadFailedEvent == null || !IronSourceRewardedVideoEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdLoadFailedEvent != null && IronSourceRewardedVideoEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdReadyEvent;

	public static event Action<IronSourceAdInfo> onAdReadyEvent
	{
		add
		{
			if (IronSourceRewardedVideoEvents._onAdReadyEvent == null || !IronSourceRewardedVideoEvents._onAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onAdReadyEvent += value;
			}
		}
		remove
		{
			if (IronSourceRewardedVideoEvents._onAdReadyEvent != null && IronSourceRewardedVideoEvents._onAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onAdReadyEvent -= value;
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "IronSourceRewardedVideoEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onAdShowFailed(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdShowFailedEvent != null)
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[0]);
			IronSourceAdInfo arg = new IronSourceAdInfo(list[1].ToString());
			IronSourceRewardedVideoEvents._onAdShowFailedEvent(errorFromErrorObject, arg);
		}
	}

	public void onAdOpened(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdOpenedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceRewardedVideoEvents._onAdOpenedEvent(obj);
		}
	}

	public void onAdClosed(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdClosedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceRewardedVideoEvents._onAdClosedEvent(obj);
		}
	}

	public void onAdRewarded(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdRewardedEvent != null)
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[0]);
			IronSourceAdInfo arg = new IronSourceAdInfo(list[1].ToString());
			IronSourceRewardedVideoEvents._onAdRewardedEvent(placementFromObject, arg);
		}
	}

	public void onAdClicked(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdClickedEvent != null)
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[0]);
			IronSourceAdInfo arg = new IronSourceAdInfo(list[1].ToString());
			IronSourceRewardedVideoEvents._onAdClickedEvent(placementFromObject, arg);
		}
	}

	public void onAdAvailable(string args)
	{
		if (IronSourceRewardedVideoEvents._onAdAvailableEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceRewardedVideoEvents._onAdAvailableEvent(obj);
		}
	}

	public void onAdUnavailable()
	{
		if (IronSourceRewardedVideoEvents._onAdUnavailableEvent != null)
		{
			IronSourceRewardedVideoEvents._onAdUnavailableEvent();
		}
	}

	public void onAdLoadFailed(string description)
	{
		if (IronSourceRewardedVideoEvents._onAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceRewardedVideoEvents._onAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onAdReady(string adinfo)
	{
		if (IronSourceRewardedVideoEvents._onAdReadyEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(adinfo);
			IronSourceRewardedVideoEvents._onAdReadyEvent(obj);
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
