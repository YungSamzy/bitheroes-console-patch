using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronSourceJSON;
using UnityEngine;

public class IronSourceEvents : MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	private const string INSTANCE_ID_KEY = "instanceId";

	private const string PLACEMENT_KEY = "placement";

	public static event Action<IronSourceImpressionData> onImpressionDataReadyEvent;

	private static event Action _onSdkInitializationCompletedEvent;

	public static event Action onSdkInitializationCompletedEvent
	{
		add
		{
			if (IronSourceEvents._onSdkInitializationCompletedEvent == null || !IronSourceEvents._onSdkInitializationCompletedEvent.GetInvocationList().Contains(value))
			{
				_onSdkInitializationCompletedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onSdkInitializationCompletedEvent != null && IronSourceEvents._onSdkInitializationCompletedEvent.GetInvocationList().Contains(value))
			{
				_onSdkInitializationCompletedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onRewardedVideoAdShowFailedEvent;

	public static event Action<IronSourceError> onRewardedVideoAdShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedEvent == null || !IronSourceEvents._onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedEvent != null && IronSourceEvents._onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdOpenedEvent;

	public static event Action onRewardedVideoAdOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedEvent == null || !IronSourceEvents._onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedEvent != null && IronSourceEvents._onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdClosedEvent;

	public static event Action onRewardedVideoAdClosedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClosedEvent == null || !IronSourceEvents._onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClosedEvent != null && IronSourceEvents._onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdStartedEvent;

	public static event Action onRewardedVideoAdStartedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdStartedEvent == null || !IronSourceEvents._onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdStartedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdStartedEvent != null && IronSourceEvents._onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdStartedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdEndedEvent;

	public static event Action onRewardedVideoAdEndedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdEndedEvent == null || !IronSourceEvents._onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdEndedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdEndedEvent != null && IronSourceEvents._onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdEndedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement> _onRewardedVideoAdRewardedEvent;

	public static event Action<IronSourcePlacement> onRewardedVideoAdRewardedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedEvent == null || !IronSourceEvents._onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedEvent != null && IronSourceEvents._onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedEvent -= value;
			}
		}
	}

	private static event Action<IronSourcePlacement> _onRewardedVideoAdClickedEvent;

	public static event Action<IronSourcePlacement> onRewardedVideoAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClickedEvent == null || !IronSourceEvents._onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClickedEvent != null && IronSourceEvents._onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedEvent -= value;
			}
		}
	}

	private static event Action<bool> _onRewardedVideoAvailabilityChangedEvent;

	public static event Action<bool> onRewardedVideoAvailabilityChangedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent == null || !IronSourceEvents._onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent != null && IronSourceEvents._onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAvailabilityChangedEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdLoadedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdLoadedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onRewardedVideoAdLoadFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onRewardedVideoAdLoadFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdOpenedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdOpenedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdOpenedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdClosedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdClosedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClosedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdRewardedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdRewardedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdRewardedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onRewardedVideoAdShowFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onRewardedVideoAdShowFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdShowFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onRewardedVideoAdClickedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdClickedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent == null || !IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent != null && IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdClickedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onSegmentReceivedEvent;

	public static event Action<string> onSegmentReceivedEvent
	{
		add
		{
			if (IronSourceEvents._onSegmentReceivedEvent == null || !IronSourceEvents._onSegmentReceivedEvent.GetInvocationList().Contains(value))
			{
				_onSegmentReceivedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onSegmentReceivedEvent != null && IronSourceEvents._onSegmentReceivedEvent.GetInvocationList().Contains(value))
			{
				_onSegmentReceivedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdReadyEvent;

	public static event Action onInterstitialAdReadyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdReadyEvent == null || !IronSourceEvents._onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdReadyEvent != null && IronSourceEvents._onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onInterstitialAdLoadFailedEvent;

	public static event Action<IronSourceError> onInterstitialAdLoadFailedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedEvent == null || !IronSourceEvents._onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedEvent != null && IronSourceEvents._onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdOpenedEvent;

	public static event Action onInterstitialAdOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdOpenedEvent == null || !IronSourceEvents._onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdOpenedEvent != null && IronSourceEvents._onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdClosedEvent;

	public static event Action onInterstitialAdClosedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClosedEvent == null || !IronSourceEvents._onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClosedEvent != null && IronSourceEvents._onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdShowSucceededEvent;

	public static event Action onInterstitialAdShowSucceededEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededEvent == null || !IronSourceEvents._onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowSucceededEvent != null && IronSourceEvents._onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowSucceededEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onInterstitialAdShowFailedEvent;

	public static event Action<IronSourceError> onInterstitialAdShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowFailedEvent == null || !IronSourceEvents._onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowFailedEvent != null && IronSourceEvents._onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onInterstitialAdClickedEvent;

	public static event Action onInterstitialAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClickedEvent == null || !IronSourceEvents._onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClickedEvent != null && IronSourceEvents._onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdReadyDemandOnlyEvent;

	public static event Action<string> onInterstitialAdReadyDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdReadyDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onInterstitialAdLoadFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onInterstitialAdLoadFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdLoadFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdOpenedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdOpenedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdOpenedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdClosedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdClosedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClosedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onInterstitialAdShowFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onInterstitialAdShowFailedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdShowFailedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action<string> _onInterstitialAdClickedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdClickedDemandOnlyEvent
	{
		add
		{
			if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent == null || !IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedDemandOnlyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent != null && IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
			{
				_onInterstitialAdClickedDemandOnlyEvent -= value;
			}
		}
	}

	private static event Action _onOfferwallOpenedEvent;

	public static event Action onOfferwallOpenedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallOpenedEvent == null || !IronSourceEvents._onOfferwallOpenedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallOpenedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallOpenedEvent != null && IronSourceEvents._onOfferwallOpenedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallOpenedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onOfferwallShowFailedEvent;

	public static event Action<IronSourceError> onOfferwallShowFailedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallShowFailedEvent == null || !IronSourceEvents._onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallShowFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallShowFailedEvent != null && IronSourceEvents._onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallShowFailedEvent -= value;
			}
		}
	}

	private static event Action _onOfferwallClosedEvent;

	public static event Action onOfferwallClosedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallClosedEvent == null || !IronSourceEvents._onOfferwallClosedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallClosedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallClosedEvent != null && IronSourceEvents._onOfferwallClosedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallClosedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onGetOfferwallCreditsFailedEvent;

	public static event Action<IronSourceError> onGetOfferwallCreditsFailedEvent
	{
		add
		{
			if (IronSourceEvents._onGetOfferwallCreditsFailedEvent == null || !IronSourceEvents._onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
			{
				_onGetOfferwallCreditsFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onGetOfferwallCreditsFailedEvent != null && IronSourceEvents._onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
			{
				_onGetOfferwallCreditsFailedEvent -= value;
			}
		}
	}

	private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

	public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallAdCreditedEvent == null || !IronSourceEvents._onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAdCreditedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallAdCreditedEvent != null && IronSourceEvents._onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAdCreditedEvent -= value;
			}
		}
	}

	private static event Action<bool> _onOfferwallAvailableEvent;

	public static event Action<bool> onOfferwallAvailableEvent
	{
		add
		{
			if (IronSourceEvents._onOfferwallAvailableEvent == null || !IronSourceEvents._onOfferwallAvailableEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAvailableEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onOfferwallAvailableEvent != null && IronSourceEvents._onOfferwallAvailableEvent.GetInvocationList().Contains(value))
			{
				_onOfferwallAvailableEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdLoadedEvent;

	public static event Action onBannerAdLoadedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLoadedEvent == null || !IronSourceEvents._onBannerAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLoadedEvent != null && IronSourceEvents._onBannerAdLoadedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadedEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onBannerAdLoadFailedEvent;

	public static event Action<IronSourceError> onBannerAdLoadFailedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLoadFailedEvent == null || !IronSourceEvents._onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLoadFailedEvent != null && IronSourceEvents._onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdClickedEvent;

	public static event Action onBannerAdClickedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdClickedEvent == null || !IronSourceEvents._onBannerAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdClickedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdClickedEvent != null && IronSourceEvents._onBannerAdClickedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdClickedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdScreenPresentedEvent;

	public static event Action onBannerAdScreenPresentedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdScreenPresentedEvent == null || !IronSourceEvents._onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenPresentedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdScreenPresentedEvent != null && IronSourceEvents._onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenPresentedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdScreenDismissedEvent;

	public static event Action onBannerAdScreenDismissedEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdScreenDismissedEvent == null || !IronSourceEvents._onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenDismissedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdScreenDismissedEvent != null && IronSourceEvents._onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdScreenDismissedEvent -= value;
			}
		}
	}

	private static event Action _onBannerAdLeftApplicationEvent;

	public static event Action onBannerAdLeftApplicationEvent
	{
		add
		{
			if (IronSourceEvents._onBannerAdLeftApplicationEvent == null || !IronSourceEvents._onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLeftApplicationEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onBannerAdLeftApplicationEvent != null && IronSourceEvents._onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
			{
				_onBannerAdLeftApplicationEvent -= value;
			}
		}
	}

	private static event Action<IronSourceImpressionData> _onImpressionSuccessEvent;

	public static event Action<IronSourceImpressionData> onImpressionSuccessEvent
	{
		add
		{
			if (IronSourceEvents._onImpressionSuccessEvent == null || !IronSourceEvents._onImpressionSuccessEvent.GetInvocationList().Contains(value))
			{
				_onImpressionSuccessEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onImpressionSuccessEvent != null && IronSourceEvents._onImpressionSuccessEvent.GetInvocationList().Contains(value))
			{
				_onImpressionSuccessEvent -= value;
			}
		}
	}

	private static event Action<IronSourceError> _onRewardedVideoAdLoadFailedEvent;

	public static event Action<IronSourceError> onRewardedVideoAdLoadFailedEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdLoadFailedEvent == null || !IronSourceEvents._onRewardedVideoAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadFailedEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdLoadFailedEvent != null && IronSourceEvents._onRewardedVideoAdLoadFailedEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdLoadFailedEvent -= value;
			}
		}
	}

	private static event Action _onRewardedVideoAdReadyEvent;

	public static event Action onRewardedVideoAdReadyEvent
	{
		add
		{
			if (IronSourceEvents._onRewardedVideoAdReadyEvent == null || !IronSourceEvents._onRewardedVideoAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdReadyEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onRewardedVideoAdReadyEvent != null && IronSourceEvents._onRewardedVideoAdReadyEvent.GetInvocationList().Contains(value))
			{
				_onRewardedVideoAdReadyEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onConsentViewDidFailToLoadWithErrorEvent;

	public static event Action<string, IronSourceError> onConsentViewDidFailToLoadWithErrorEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent == null || !IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidFailToLoadWithErrorEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent != null && IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidFailToLoadWithErrorEvent -= value;
			}
		}
	}

	private static event Action<string, IronSourceError> _onConsentViewDidFailToShowWithErrorEvent;

	public static event Action<string, IronSourceError> onConsentViewDidFailToShowWithErrorEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent == null || !IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidFailToShowWithErrorEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent != null && IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidFailToShowWithErrorEvent -= value;
			}
		}
	}

	private static event Action<string> _onConsentViewDidAcceptEvent;

	public static event Action<string> onConsentViewDidAcceptEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidAcceptEvent == null || !IronSourceEvents._onConsentViewDidAcceptEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidAcceptEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidAcceptEvent != null && IronSourceEvents._onConsentViewDidAcceptEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidAcceptEvent -= value;
			}
		}
	}

	private static event Action<string> _onConsentViewDidDismissEvent;

	public static event Action<string> onConsentViewDidDismissEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidDismissEvent == null || !IronSourceEvents._onConsentViewDidDismissEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidDismissEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidDismissEvent != null && IronSourceEvents._onConsentViewDidDismissEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidDismissEvent -= value;
			}
		}
	}

	private static event Action<string> _onConsentViewDidLoadSuccessEvent;

	public static event Action<string> onConsentViewDidLoadSuccessEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidLoadSuccessEvent == null || !IronSourceEvents._onConsentViewDidLoadSuccessEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidLoadSuccessEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidLoadSuccessEvent != null && IronSourceEvents._onConsentViewDidLoadSuccessEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidLoadSuccessEvent -= value;
			}
		}
	}

	private static event Action<string> _onConsentViewDidShowSuccessEvent;

	public static event Action<string> onConsentViewDidShowSuccessEvent
	{
		add
		{
			if (IronSourceEvents._onConsentViewDidShowSuccessEvent == null || !IronSourceEvents._onConsentViewDidShowSuccessEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidShowSuccessEvent += value;
			}
		}
		remove
		{
			if (IronSourceEvents._onConsentViewDidShowSuccessEvent != null && IronSourceEvents._onConsentViewDidShowSuccessEvent.GetInvocationList().Contains(value))
			{
				_onConsentViewDidShowSuccessEvent -= value;
			}
		}
	}

	private void Awake()
	{
		base.gameObject.name = "IronSourceEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onSdkInitializationCompleted(string empty)
	{
		if (IronSourceEvents._onSdkInitializationCompletedEvent != null)
		{
			IronSourceEvents._onSdkInitializationCompletedEvent();
		}
	}

	public void onRewardedVideoAdShowFailed(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onRewardedVideoAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onRewardedVideoAdStarted(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdStartedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdStartedEvent();
		}
	}

	public void onRewardedVideoAdEnded(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdEndedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdEndedEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdRewardedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAdClicked(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdClickedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = ((stringAvailable == "true") ? true : false);
		if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onRewardedVideoAdLoadedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdLoadedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdLoadFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onRewardedVideoAdLoadFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdRewardedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdClickedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent(instanceId);
		}
	}

	public void onSegmentReceived(string segmentName)
	{
		if (IronSourceEvents._onSegmentReceivedEvent != null)
		{
			IronSourceEvents._onSegmentReceivedEvent(segmentName);
		}
	}

	public void onInterstitialAdReady()
	{
		if (IronSourceEvents._onInterstitialAdReadyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyEvent();
		}
	}

	public void onInterstitialAdLoadFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpened(string empty)
	{
		if (IronSourceEvents._onInterstitialAdOpenedEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedEvent();
		}
	}

	public void onInterstitialAdClosed(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClosedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedEvent();
		}
	}

	public void onInterstitialAdShowSucceeded(string empty)
	{
		if (IronSourceEvents._onInterstitialAdShowSucceededEvent != null)
		{
			IronSourceEvents._onInterstitialAdShowSucceededEvent();
		}
	}

	public void onInterstitialAdShowFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdClicked(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClickedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedEvent();
		}
	}

	public void onInterstitialAdReadyDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdLoadFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdClickedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent(instanceId);
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (IronSourceEvents._onOfferwallOpenedEvent != null)
		{
			IronSourceEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFailed(string description)
	{
		if (IronSourceEvents._onOfferwallShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onOfferwallShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (IronSourceEvents._onOfferwallClosedEvent != null)
		{
			IronSourceEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFailed(string description)
	{
		if (IronSourceEvents._onGetOfferwallCreditsFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onGetOfferwallCreditsFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (IronSourceEvents._onOfferwallAdCreditedEvent != null)
		{
			IronSourceEvents._onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public void onOfferwallAvailable(string stringAvailable)
	{
		bool obj = ((stringAvailable == "true") ? true : false);
		if (IronSourceEvents._onOfferwallAvailableEvent != null)
		{
			IronSourceEvents._onOfferwallAvailableEvent(obj);
		}
	}

	public void onBannerAdLoaded()
	{
		if (IronSourceEvents._onBannerAdLoadedEvent != null)
		{
			IronSourceEvents._onBannerAdLoadedEvent();
		}
	}

	public void onBannerAdLoadFailed(string description)
	{
		if (IronSourceEvents._onBannerAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onBannerAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onBannerAdClicked()
	{
		if (IronSourceEvents._onBannerAdClickedEvent != null)
		{
			IronSourceEvents._onBannerAdClickedEvent();
		}
	}

	public void onBannerAdScreenPresented()
	{
		if (IronSourceEvents._onBannerAdScreenPresentedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenPresentedEvent();
		}
	}

	public void onBannerAdScreenDismissed()
	{
		if (IronSourceEvents._onBannerAdScreenDismissedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenDismissedEvent();
		}
	}

	public void onBannerAdLeftApplication()
	{
		if (IronSourceEvents._onBannerAdLeftApplicationEvent != null)
		{
			IronSourceEvents._onBannerAdLeftApplicationEvent();
		}
	}

	public void onImpressionSuccess(string args)
	{
		IronSourceImpressionData obj = new IronSourceImpressionData(args);
		if (IronSourceEvents._onImpressionSuccessEvent != null)
		{
			IronSourceEvents._onImpressionSuccessEvent(obj);
		}
	}

	public void onRewardedVideoAdLoadFailed(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onRewardedVideoAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdReady(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdReadyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdReadyEvent();
		}
	}

	public void onConsentViewDidFailToLoadWithError(string args)
	{
		if (IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onConsentViewDidFailToLoadWithErrorEvent(arg, errorFromErrorObject);
		}
	}

	public void onConsentViewDidFailToShowWithError(string args)
	{
		if (IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onConsentViewDidFailToShowWithErrorEvent(arg, errorFromErrorObject);
		}
	}

	public void onConsentViewDidAccept(string consentViewType)
	{
		if (IronSourceEvents._onConsentViewDidAcceptEvent != null)
		{
			IronSourceEvents._onConsentViewDidAcceptEvent(consentViewType);
		}
	}

	public void onConsentViewDidDismiss(string consentViewType)
	{
		if (IronSourceEvents._onConsentViewDidDismissEvent != null)
		{
			IronSourceEvents._onConsentViewDidDismissEvent(consentViewType);
		}
	}

	public void onConsentViewDidLoadSuccess(string consentViewType)
	{
		if (IronSourceEvents._onConsentViewDidLoadSuccessEvent != null)
		{
			IronSourceEvents._onConsentViewDidLoadSuccessEvent(consentViewType);
		}
	}

	public void onConsentViewDidShowSuccess(string consentViewType)
	{
		if (IronSourceEvents._onConsentViewDidShowSuccessEvent != null)
		{
			IronSourceEvents._onConsentViewDidShowSuccessEvent(consentViewType);
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

	private static void InvokeEvent(Action<IronSourceImpressionData> evt, string args)
	{
		IronSourceImpressionData obj = new IronSourceImpressionData(args);
		evt(obj);
	}
}
