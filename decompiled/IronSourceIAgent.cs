using System.Collections.Generic;

public interface IronSourceIAgent
{
	void onApplicationPause(bool pause);

	void setMediationSegment(string segment);

	string getAdvertiserId();

	void validateIntegration();

	void shouldTrackNetworkState(bool track);

	bool setDynamicUserId(string dynamicUserId);

	void setAdaptersDebug(bool enabled);

	void setMetaData(string key, string value);

	void setMetaData(string key, params string[] values);

	int? getConversionValue();

	void setManualLoadRewardedVideo(bool isOn);

	void setNetworkData(string networkKey, string networkData);

	void SetPauseGame(bool pause);

	void setUserId(string userId);

	void init(string appKey);

	void init(string appKey, params string[] adUnits);

	void initISDemandOnly(string appKey, params string[] adUnits);

	void loadRewardedVideo();

	void showRewardedVideo();

	void showRewardedVideo(string placementName);

	bool isRewardedVideoAvailable();

	bool isRewardedVideoPlacementCapped(string placementName);

	IronSourcePlacement getPlacementInfo(string name);

	void setRewardedVideoServerParams(Dictionary<string, string> parameters);

	void clearRewardedVideoServerParams();

	void showISDemandOnlyRewardedVideo(string instanceId);

	void loadISDemandOnlyRewardedVideo(string instanceId);

	bool isISDemandOnlyRewardedVideoAvailable(string instanceId);

	void loadInterstitial();

	void showInterstitial();

	void showInterstitial(string placementName);

	bool isInterstitialReady();

	bool isInterstitialPlacementCapped(string placementName);

	void loadISDemandOnlyInterstitial(string instanceId);

	void showISDemandOnlyInterstitial(string instanceId);

	bool isISDemandOnlyInterstitialReady(string instanceId);

	void showOfferwall();

	void showOfferwall(string placementName);

	bool isOfferwallAvailable();

	void getOfferwallCredits();

	void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position);

	void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName);

	void destroyBanner();

	void displayBanner();

	void hideBanner();

	bool isBannerPlacementCapped(string placementName);

	void setSegment(IronSourceSegment segment);

	void setConsent(bool consent);

	void loadConsentViewWithType(string consentViewType);

	void showConsentViewWithType(string consentViewType);

	void setAdRevenueData(string dataSource, Dictionary<string, string> impressionData);
}
