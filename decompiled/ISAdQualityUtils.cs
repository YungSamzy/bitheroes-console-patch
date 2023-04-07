using System;
using UnityEngine;

public static class ISAdQualityUtils
{
	private static bool isDebugBuild;

	private static bool isDebugBuildSet;

	public static void LogDebug(string tag, string message)
	{
		if (!isDebugBuildSet)
		{
			try
			{
				isDebugBuild = Debug.isDebugBuild;
			}
			catch (Exception ex)
			{
				isDebugBuild = true;
				Debug.Log($"{tag} {ex.Message}");
			}
			isDebugBuildSet = true;
		}
		if (isDebugBuild)
		{
			Debug.Log($"{tag} {message}");
		}
	}

	public static void LogError(string tag, string message)
	{
		Debug.LogError($"{tag} {message}");
	}

	public static void LogWarning(string tag, string message)
	{
		Debug.LogWarning($"{tag} {message}");
	}

	public static string GetClassName(object target)
	{
		return target.GetType().Name;
	}
}
