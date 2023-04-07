using UnityEngine;

namespace Ultrabit;

public class PlatformBindings
{
	public static byte[] GetPlatformImageByteArray(string imageName)
	{
		Debug.Log("Not supported for this platform");
		return null;
	}

	public static string GetSharedKeyValue(string key, string group)
	{
		return null;
	}

	public static void SetSharedKeyValue(string key, string value, string group)
	{
	}

	public static string GetBundleVersion()
	{
		Debug.Log("Bundle version does not require a binding for this platform");
		return string.Empty;
	}

	public static string GetBundleIdentifier()
	{
		return Application.identifier;
	}

	public static void RegisterForPushNotifications()
	{
	}

	public static bool AreLocalNotificationsEnabled()
	{
		if (PlatformUtil.isEditor)
		{
			return !PlatformUtil.isWeb;
		}
		return false;
	}

	public static void RegisterForLocalNotifications()
	{
	}

	public static void CancelAllNotifications()
	{
	}

	public static void PresentGameCenterLogin()
	{
	}

	public static bool IsUserGCConnected()
	{
		return false;
	}

	public static void AutoAuthenticateGameCenter()
	{
	}

	public static void SignOutOfGameCenter()
	{
	}

	public static void ShowGameCenterAchievements()
	{
	}

	public static void UnlockGameCenterAchievement(string achievementId)
	{
	}

	public static bool HasStorageAvailableForBytes(long bytes)
	{
		return true;
	}

	public static void RequestAdTracking()
	{
	}
}
