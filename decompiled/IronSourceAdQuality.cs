using UnityEngine;

public class IronSourceAdQuality : CodeGeneratedSingleton
{
	private static GameObject adQualityGameObject = new GameObject("IronSourceAdQuality");

	private const string TAG = "IronSource AdQuality";

	private const bool DEBUG = false;

	protected override bool DontDestroySingleton => true;

	protected override void InitAfterRegisteringAsSingleInstance()
	{
		base.InitAfterRegisteringAsSingleInstance();
	}

	public static void Initialize(string appKey)
	{
		Initialize(appKey, new ISAdQualityConfig());
	}

	public static void Initialize(string appKey, ISAdQualityConfig adQualityConfig)
	{
		Initialize(appKey, adQualityConfig.UserId, adQualityConfig.UserIdSet, adQualityConfig.TestMode, adQualityConfig.LogLevel, adQualityConfig.AdQualityInitCallback);
	}

	private static void Initialize(string appKey, string userId, bool userIdSet, bool testMode, ISAdQualityLogLevel logLevel, ISAdQualityInitCallback adQualityInitCallback)
	{
		UnitySingleton.GetSynchronousCodeGeneratedInstance<IronSourceAdQuality>();
		adQualityGameObject.AddComponent<ISAdQualityInitCallbackWrapper>().AdQualityInitCallback = adQualityInitCallback;
		ISAdQualityUtils.LogWarning("IronSource AdQuality", "Ad Quality SDK works only on Android or iOS devices.");
	}

	public static void ChangeUserId(string userId)
	{
	}

	public static void AddExtraUserId(string userId)
	{
	}

	public static void SetUserConsent(bool userConsent)
	{
	}
}
