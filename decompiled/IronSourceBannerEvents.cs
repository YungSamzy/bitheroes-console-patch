using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronSourceJSON;
using UnityEngine;

public class IronSourceBannerEvents : MonoBehaviour
{
	private static event Action<IronSourceAdInfo> _onAdLoadedEvent;

	public static event Action<IronSourceAdInfo> onAdLoadedEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdLoadedEvent == null || !IronSourceBannerEvents._onAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadedEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdLoadedEvent != null || IronSourceBannerEvents._onAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onAdLoadFailedEvent;

	public static event Action<IronSourceError> onAdLoadFailedEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdLoadFailedEvent == null || !IronSourceBannerEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdLoadFailedEvent != null && IronSourceBannerEvents._onAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdClickedEvent;

	public static event Action<IronSourceAdInfo> onAdClickedEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdClickedEvent == null || !IronSourceBannerEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdClickedEvent != null && IronSourceBannerEvents._onAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onAdClickedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdScreenPresentedEvent;

	public static event Action<IronSourceAdInfo> onAdScreenPresentedEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdScreenPresentedEvent == null || !IronSourceBannerEvents._onAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onAdScreenPresentedEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdScreenPresentedEvent != null && IronSourceBannerEvents._onAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onAdScreenPresentedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdScreenDismissedEvent;

	public static event Action<IronSourceAdInfo> onAdScreenDismissedEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdScreenDismissedEvent == null || !IronSourceBannerEvents._onAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onAdScreenDismissedEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdScreenDismissedEvent != null && IronSourceBannerEvents._onAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onAdScreenDismissedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceAdInfo> _onAdLeftApplicationEvent;

	public static event Action<IronSourceAdInfo> onAdLeftApplicationEvent
	{
		add
		{
			if (IronSourceBannerEvents._onAdLeftApplicationEvent == null || !IronSourceBannerEvents._onAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onAdLeftApplicationEvent += value;
			}
		}
		remove
		{
			if (IronSourceBannerEvents._onAdLeftApplicationEvent != null && IronSourceBannerEvents._onAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onAdLeftApplicationEvent -= value;
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "IronSourceBannerEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onAdLoaded(string args)
	{
		if (IronSourceBannerEvents._onAdLoadedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceBannerEvents._onAdLoadedEvent(obj);
		}
	}

	public void onAdLoadFailed(string description)
	{
		if (IronSourceBannerEvents._onAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceBannerEvents._onAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onAdClicked(string args)
	{
		if (IronSourceBannerEvents._onAdClickedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceBannerEvents._onAdClickedEvent(obj);
		}
	}

	public void onAdScreenPresented(string args)
	{
		if (IronSourceBannerEvents._onAdScreenPresentedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceBannerEvents._onAdScreenPresentedEvent(obj);
		}
	}

	public void onAdScreenDismissed(string args)
	{
		if (IronSourceBannerEvents._onAdScreenDismissedEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceBannerEvents._onAdScreenDismissedEvent(obj);
		}
	}

	public void onAdLeftApplication(string args)
	{
		if (IronSourceBannerEvents._onAdLeftApplicationEvent != null)
		{
			IronSourceAdInfo obj = new IronSourceAdInfo(args);
			IronSourceBannerEvents._onAdLeftApplicationEvent(obj);
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
