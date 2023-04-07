using UnityEngine;

public static class PlatformUtil
{
	public static bool isEditor => false;

	public static bool isAndroid => false;

	public static bool isIPhone => false;

	public static bool isWeb => false;

	public static bool isMobile
	{
		get
		{
			if (!isAndroid)
			{
				return isIPhone;
			}
			return true;
		}
	}

	public static BuildPlatform GetBuildPlatform()
	{
		BuildPlatform buildPlatform = BuildPlatform.None;
		switch (Application.platform)
		{
		case RuntimePlatform.OSXPlayer:
			return BuildPlatform.StandaloneOSX;
		case RuntimePlatform.WindowsPlayer:
			return BuildPlatform.StandaloneWindows64;
		case RuntimePlatform.IPhonePlayer:
			return BuildPlatform.IOS;
		case RuntimePlatform.Android:
			return BuildPlatform.Android;
		case RuntimePlatform.WebGLPlayer:
			return BuildPlatform.WebGL;
		default:
			Debug.LogError("Platform " + Application.platform.ToString() + " is not supported.");
			return BuildPlatform.None;
		}
	}
}
