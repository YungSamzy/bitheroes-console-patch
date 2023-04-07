using System.Collections.Generic;
using Ultrabit;
using UnityEngine;

public static class SystemInfoHelper
{
	private static string m_bundleIdentifier;

	private static string m_bundleVersion;

	private static string m_deviceUniqueIdentifier;

	private static Dictionary<string, string> m_systemInfo;

	private static Dictionary<string, string> m_versionInfo;

	public static string bundleIdentifier
	{
		get
		{
			if (m_bundleIdentifier == null)
			{
				m_bundleIdentifier = PlatformBindings.GetBundleIdentifier();
			}
			return m_bundleIdentifier;
		}
	}

	public static string bundleVersion
	{
		get
		{
			if (m_bundleVersion == null)
			{
				TextAsset textAsset = Resources.Load("bundleVersion") as TextAsset;
				if (textAsset != null)
				{
					m_bundleVersion = textAsset.text;
				}
			}
			return m_bundleVersion;
		}
	}

	public static string deviceUniqueIdentifier
	{
		get
		{
			if (m_deviceUniqueIdentifier == null)
			{
				m_deviceUniqueIdentifier = platform.ToString().Substring(0, 3).ToLower() + ":" + SystemInfo.deviceUniqueIdentifier;
			}
			return m_deviceUniqueIdentifier;
		}
	}

	public static RuntimePlatform platform => Application.platform;

	public static Dictionary<string, string> systemInfo
	{
		get
		{
			if (m_systemInfo == null)
			{
				m_systemInfo = new Dictionary<string, string>
				{
					{
						"deviceModel",
						SystemInfo.deviceModel
					},
					{
						"deviceType",
						SystemInfo.deviceType.ToString()
					},
					{
						"operatingSystem",
						SystemInfo.operatingSystem
					}
				};
			}
			return m_systemInfo;
		}
	}

	public static Dictionary<string, string> versionInfo
	{
		get
		{
			if (m_versionInfo == null)
			{
				m_versionInfo = new Dictionary<string, string>
				{
					{ "bundleVersion", bundleVersion },
					{
						"platform",
						platform.ToString()
					}
				};
			}
			return m_versionInfo;
		}
	}
}
