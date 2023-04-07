using System.Collections.Generic;
using UnityEngine;

public class IronSourceDemoScript : MonoBehaviour
{
	public void Start()
	{
		string appKey = "unexpected_platform";
		Debug.Log("unity-script: IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration();
		Debug.Log("unity-script: unity version" + IronSource.unityVersion());
		Debug.Log("unity-script: IronSource.Agent.init");
		IronSource.Agent.init(appKey);
	}

	private void OnEnable()
	{
		IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
		IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
		IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
		IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
		IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
		IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
		IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
		IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
		IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
		IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent += RewardedVideoAdLoadedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent += RewardedVideoAdLoadFailedDemandOnlyEvent;
		IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
		IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
		IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
		IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
		IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
		IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;
		IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
		IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
		IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
		IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
		IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
		IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
		IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
		IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;
		IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
		IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
		IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
		IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
		IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
		IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
		IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;
		IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
		IronSourceRewardedVideoEvents.onAdOpenedEvent += ReardedVideoOnAdOpenedEvent;
		IronSourceRewardedVideoEvents.onAdClosedEvent += ReardedVideoOnAdClosedEvent;
		IronSourceRewardedVideoEvents.onAdAvailableEvent += ReardedVideoOnAdAvailable;
		IronSourceRewardedVideoEvents.onAdUnavailableEvent += ReardedVideoOnAdUnavailable;
		IronSourceRewardedVideoEvents.onAdShowFailedEvent += ReardedVideoOnAdShowFailedEvent;
		IronSourceRewardedVideoEvents.onAdRewardedEvent += ReardedVideoOnAdRewardedEvent;
		IronSourceRewardedVideoEvents.onAdClickedEvent += ReardedVideoOnAdClickedEvent;
		IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
		IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
		IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
		IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
		IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
		IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
		IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
		IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
		IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
		IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
		IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
		IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
		IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
	}

	private void OnApplicationPause(bool isPaused)
	{
		Debug.Log("unity-script: OnApplicationPause = " + isPaused);
		IronSource.Agent.onApplicationPause(isPaused);
	}

	public void OnGUI()
	{
		GUI.backgroundColor = Color.blue;
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.width);
		if (GUI.Button(new Rect(0.1f * (float)Screen.width, 0.15f * (float)Screen.height, 0.8f * (float)Screen.width, 0.08f * (float)Screen.height), "Show Rewarded Video"))
		{
			Debug.Log("unity-script: ShowRewardedVideoButtonClicked");
			if (IronSource.Agent.isRewardedVideoAvailable())
			{
				IronSource.Agent.showRewardedVideo();
			}
			else
			{
				Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
			}
		}
		if (GUI.Button(new Rect(0.1f * (float)Screen.width, 0.25f * (float)Screen.height, 0.8f * (float)Screen.width, 0.08f * (float)Screen.height), "Show Offerwall"))
		{
			if (IronSource.Agent.isOfferwallAvailable())
			{
				IronSource.Agent.showOfferwall();
			}
			else
			{
				Debug.Log("IronSource.Agent.isOfferwallAvailable - False");
			}
		}
		if (GUI.Button(new Rect(0.1f * (float)Screen.width, 0.35f * (float)Screen.height, 0.35f * (float)Screen.width, 0.08f * (float)Screen.height), "Load Interstitial"))
		{
			Debug.Log("unity-script: LoadInterstitialButtonClicked");
			IronSource.Agent.loadInterstitial();
		}
		if (GUI.Button(new Rect(0.55f * (float)Screen.width, 0.35f * (float)Screen.height, 0.35f * (float)Screen.width, 0.08f * (float)Screen.height), "Show Interstitial"))
		{
			Debug.Log("unity-script: ShowInterstitialButtonClicked");
			if (IronSource.Agent.isInterstitialReady())
			{
				IronSource.Agent.showInterstitial();
			}
			else
			{
				Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
			}
		}
		if (GUI.Button(new Rect(0.1f * (float)Screen.width, 0.45f * (float)Screen.height, 0.35f * (float)Screen.width, 0.08f * (float)Screen.height), "Load Banner"))
		{
			Debug.Log("unity-script: loadBannerButtonClicked");
			IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
		}
		if (GUI.Button(new Rect(0.55f * (float)Screen.width, 0.45f * (float)Screen.height, 0.35f * (float)Screen.width, 0.08f * (float)Screen.height), "Destroy Banner"))
		{
			Debug.Log("unity-script: loadBannerButtonClicked");
			IronSource.Agent.destroyBanner();
		}
	}

	private void SdkInitializationCompletedEvent()
	{
		Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
	}

	private void ReardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdOpenedEvent With AdInfo " + adInfo.ToString());
	}

	private void ReardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdClosedEvent With AdInfo " + adInfo.ToString());
	}

	private void ReardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdAvailable With AdInfo " + adInfo.ToString());
	}

	private void ReardedVideoOnAdUnavailable()
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdUnavailable");
	}

	private void ReardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError.ToString() + "And AdInfo " + adInfo.ToString());
	}

	private void ReardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
	}

	private void ReardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
	}

	private void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
	{
		Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
	}

	private void RewardedVideoAdOpenedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
	}

	private void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
	{
		Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
	}

	private void RewardedVideoAdClosedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
	}

	private void RewardedVideoAdStartedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
	}

	private void RewardedVideoAdEndedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
	}

	private void RewardedVideoAdShowFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
	}

	private void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
	}

	private void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
	}

	private void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
	}

	private void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
	}

	private void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
	}

	private void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdReadyEvent With AdInfo " + adInfo.ToString());
	}

	private void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
	{
		Debug.Log("unity-script: I got InterstitialOnAdLoadFailed With Error " + ironSourceError.ToString());
	}

	private void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdOpenedEvent With AdInfo " + adInfo.ToString());
	}

	private void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo.ToString());
	}

	private void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdShowSucceededEvent With AdInfo " + adInfo.ToString());
	}

	private void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdShowFailedEvent With Error " + ironSourceError.ToString() + " And AdInfo " + adInfo.ToString());
	}

	private void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo.ToString());
	}

	private void InterstitialAdReadyEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdReadyEvent");
	}

	private void InterstitialAdLoadFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
	}

	private void InterstitialAdShowSucceededEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
	}

	private void InterstitialAdShowFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void InterstitialAdClickedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdClickedEvent");
	}

	private void InterstitialAdOpenedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
	}

	private void InterstitialAdClosedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdClosedEvent");
	}

	private void InterstitialAdReadyDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
	}

	private void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
	}

	private void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
	}

	private void InterstitialAdClickedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
	}

	private void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
	}

	private void InterstitialAdClosedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
	}

	private void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo.ToString());
	}

	private void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
	{
		Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError.ToString());
	}

	private void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo.ToString());
	}

	private void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got BannerOnAdScreenPresentedEvent With AdInfo " + adInfo.ToString());
	}

	private void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got BannerOnAdScreenDismissedEvent With AdInfo " + adInfo.ToString());
	}

	private void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
	{
		Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo.ToString());
	}

	private void BannerAdLoadedEvent()
	{
		Debug.Log("unity-script: I got BannerAdLoadedEvent");
	}

	private void BannerAdLoadFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
	}

	private void BannerAdClickedEvent()
	{
		Debug.Log("unity-script: I got BannerAdClickedEvent");
	}

	private void BannerAdScreenPresentedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
	}

	private void BannerAdScreenDismissedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
	}

	private void BannerAdLeftApplicationEvent()
	{
		Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
	}

	private void OfferwallOpenedEvent()
	{
		Debug.Log("I got OfferwallOpenedEvent");
	}

	private void OfferwallClosedEvent()
	{
		Debug.Log("I got OfferwallClosedEvent");
	}

	private void OfferwallShowFailedEvent(IronSourceError error)
	{
		Debug.Log("I got OfferwallShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
	{
		Debug.Log("I got OfferwallAdCreditedEvent, current credits = " + dict["credits"]?.ToString() + " totalCredits = " + dict["totalCredits"]);
	}

	private void GetOfferwallCreditsFailedEvent(IronSourceError error)
	{
		Debug.Log("I got GetOfferwallCreditsFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	private void OfferwallAvailableEvent(bool canShowOfferwal)
	{
		Debug.Log("I got OfferwallAvailableEvent, value = " + canShowOfferwal);
	}

	private void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
	{
		Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
		Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);
	}

	private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
	{
		Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
		Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
	}
}
