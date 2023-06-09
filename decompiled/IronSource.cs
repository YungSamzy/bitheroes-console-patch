using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSource : IronSourceIAgent
{
	private IronSourceIAgent _platformAgent;

	private static IronSource _instance;

	public static string UNITY_PLUGIN_VERSION = "7.2.7-r";

	private static bool isUnsupportedPlatform;

	public static IronSource Agent
	{
		get
		{
			if (_instance == null)
			{
				_instance = new IronSource();
			}
			return _instance;
		}
	}

	private IronSource()
	{
		if (!isUnsupportedPlatform)
		{
			_platformAgent = new UnsupportedPlatformAgent();
		}
		else
		{
			_platformAgent = new UnsupportedPlatformAgent();
		}
		Type typeFromHandle = typeof(IronSourceEvents);
		Type typeFromHandle2 = typeof(IronSourceRewardedVideoEvents);
		Type typeFromHandle3 = typeof(IronSourceInterstitialEvents);
		Type typeFromHandle4 = typeof(IronSourceBannerEvents);
		new GameObject("IronSourceEvents", typeFromHandle).GetComponent<IronSourceEvents>();
		new GameObject("IronSourceRewardedVideoEvents", typeFromHandle2).GetComponent<IronSourceRewardedVideoEvents>();
		new GameObject("IronSourceInterstitialEvents", typeFromHandle3).GetComponent<IronSourceInterstitialEvents>();
		new GameObject("IronSourceBannerEvents", typeFromHandle4).GetComponent<IronSourceBannerEvents>();
	}

	public static string pluginVersion()
	{
		return UNITY_PLUGIN_VERSION;
	}

	public static string unityVersion()
	{
		return Application.unityVersion;
	}

	public static void setUnsupportedPlatform()
	{
		isUnsupportedPlatform = true;
	}

	public void onApplicationPause(bool pause)
	{
		_platformAgent.onApplicationPause(pause);
	}

	[Obsolete("This method has been deprecated and won’t be included in ironSource SDK versions 7.3.0 and above", false)]
	public void setMediationSegment(string segment)
	{
		_platformAgent.setMediationSegment(segment);
	}

	public string getAdvertiserId()
	{
		return _platformAgent.getAdvertiserId();
	}

	public void validateIntegration()
	{
		_platformAgent.validateIntegration();
	}

	public void shouldTrackNetworkState(bool track)
	{
		_platformAgent.shouldTrackNetworkState(track);
	}

	public bool setDynamicUserId(string dynamicUserId)
	{
		return _platformAgent.setDynamicUserId(dynamicUserId);
	}

	public void setAdaptersDebug(bool enabled)
	{
		_platformAgent.setAdaptersDebug(enabled);
	}

	public void setMetaData(string key, string value)
	{
		_platformAgent.setMetaData(key, value);
	}

	public void setMetaData(string key, params string[] values)
	{
		_platformAgent.setMetaData(key, values);
	}

	public int? getConversionValue()
	{
		return _platformAgent.getConversionValue();
	}

	public void setManualLoadRewardedVideo(bool isOn)
	{
		_platformAgent.setManualLoadRewardedVideo(isOn);
	}

	public void setNetworkData(string networkKey, string networkData)
	{
		_platformAgent.setNetworkData(networkKey, networkData);
	}

	public void SetPauseGame(bool pause)
	{
		_platformAgent.SetPauseGame(pause);
	}

	public void setUserId(string userId)
	{
		_platformAgent.setUserId(userId);
	}

	public void init(string appKey)
	{
		_platformAgent.init(appKey);
	}

	public void init(string appKey, params string[] adUnits)
	{
		_platformAgent.init(appKey, adUnits);
	}

	public void initISDemandOnly(string appKey, params string[] adUnits)
	{
		_platformAgent.initISDemandOnly(appKey, adUnits);
	}

	[Obsolete("This method has been deprecated and won’t be included in ironSource SDK versions 7.3.0 and above. Please use loadRewardedVideo() instead", false)]
	public void loadManualRewardedVideo()
	{
		_platformAgent.loadRewardedVideo();
	}

	public void loadRewardedVideo()
	{
		_platformAgent.loadRewardedVideo();
	}

	public void showRewardedVideo()
	{
		_platformAgent.showRewardedVideo();
	}

	public void showRewardedVideo(string placementName)
	{
		_platformAgent.showRewardedVideo(placementName);
	}

	public IronSourcePlacement getPlacementInfo(string placementName)
	{
		return _platformAgent.getPlacementInfo(placementName);
	}

	public bool isRewardedVideoAvailable()
	{
		return _platformAgent.isRewardedVideoAvailable();
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return _platformAgent.isRewardedVideoPlacementCapped(placementName);
	}

	public void setRewardedVideoServerParams(Dictionary<string, string> parameters)
	{
		_platformAgent.setRewardedVideoServerParams(parameters);
	}

	public void clearRewardedVideoServerParams()
	{
		_platformAgent.clearRewardedVideoServerParams();
	}

	public void showISDemandOnlyRewardedVideo(string instanceId)
	{
		_platformAgent.showISDemandOnlyRewardedVideo(instanceId);
	}

	public void loadISDemandOnlyRewardedVideo(string instanceId)
	{
		_platformAgent.loadISDemandOnlyRewardedVideo(instanceId);
	}

	public bool isISDemandOnlyRewardedVideoAvailable(string instanceId)
	{
		return _platformAgent.isISDemandOnlyRewardedVideoAvailable(instanceId);
	}

	public void loadInterstitial()
	{
		_platformAgent.loadInterstitial();
	}

	public void showInterstitial()
	{
		_platformAgent.showInterstitial();
	}

	public void showInterstitial(string placementName)
	{
		_platformAgent.showInterstitial(placementName);
	}

	public bool isInterstitialReady()
	{
		return _platformAgent.isInterstitialReady();
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return _platformAgent.isInterstitialPlacementCapped(placementName);
	}

	public void loadISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.loadISDemandOnlyInterstitial(instanceId);
	}

	public void showISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.showISDemandOnlyInterstitial(instanceId);
	}

	public bool isISDemandOnlyInterstitialReady(string instanceId)
	{
		return _platformAgent.isISDemandOnlyInterstitialReady(instanceId);
	}

	public void showOfferwall()
	{
		_platformAgent.showOfferwall();
	}

	public void showOfferwall(string placementName)
	{
		_platformAgent.showOfferwall(placementName);
	}

	public void getOfferwallCredits()
	{
		_platformAgent.getOfferwallCredits();
	}

	public bool isOfferwallAvailable()
	{
		return _platformAgent.isOfferwallAvailable();
	}

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position)
	{
		_platformAgent.loadBanner(size, position);
	}

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName)
	{
		_platformAgent.loadBanner(size, position, placementName);
	}

	public void destroyBanner()
	{
		_platformAgent.destroyBanner();
	}

	public void displayBanner()
	{
		_platformAgent.displayBanner();
	}

	public void hideBanner()
	{
		_platformAgent.hideBanner();
	}

	public bool isBannerPlacementCapped(string placementName)
	{
		return _platformAgent.isBannerPlacementCapped(placementName);
	}

	public void setSegment(IronSourceSegment segment)
	{
		_platformAgent.setSegment(segment);
	}

	public void setConsent(bool consent)
	{
		_platformAgent.setConsent(consent);
	}

	public void loadConsentViewWithType(string consentViewType)
	{
		_platformAgent.loadConsentViewWithType(consentViewType);
	}

	public void showConsentViewWithType(string consentViewType)
	{
		_platformAgent.showConsentViewWithType(consentViewType);
	}

	public void setAdRevenueData(string dataSource, Dictionary<string, string> impressionData)
	{
		_platformAgent.setAdRevenueData(dataSource, impressionData);
	}
}
