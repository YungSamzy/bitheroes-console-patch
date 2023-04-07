using System;
using System.Diagnostics;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.utility;

public class D
{
	public const string ALL = "all";

	public const string DAVID = "david";

	public const string NACHO = "nacho";

	public const string ROSTY = "rosty";

	public const string MARI = "mari";

	public const string MANU = "manu";

	public const string MERCA = "merca";

	public const string TONCHO = "toncho";

	public const string NANN = "nann";

	private static string _show;

	public static string show
	{
		get
		{
			if (_show == null)
			{
				SingletonMonoBehaviour<EnvironmentManager>.instance.Initialize();
				_show = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("debug")["dev"];
			}
			return _show;
		}
		set
		{
			_show = value;
		}
	}

	public static void ShowOnly(string dev)
	{
		show = dev;
	}

	public static void Log(string dev, object message, bool forceLoggly = false)
	{
		if (dev == "all" || dev == show)
		{
			UnityEngine.Debug.Log(GetDev(dev) + message);
		}
		if (!UnityEngine.Debug.unityLogger.logEnabled && forceLoggly)
		{
			Loggly(dev, message, LogType.Log);
		}
	}

	public static void LogWarning(string dev, object message, bool forceLoggly = false)
	{
		if (dev == "all" || dev == show)
		{
			UnityEngine.Debug.LogWarning(GetDev(dev) + message);
		}
		if (!UnityEngine.Debug.unityLogger.logEnabled && forceLoggly)
		{
			Loggly(dev, message, LogType.Warning);
		}
	}

	public static void LogError(string dev, object message, bool forceLoggly = true)
	{
		if (dev == "all" || dev == show)
		{
			UnityEngine.Debug.LogError(GetDev(dev) + message);
		}
		if (!UnityEngine.Debug.unityLogger.logEnabled && forceLoggly)
		{
			Loggly(dev, message, LogType.Error);
		}
	}

	private static void Loggly(string dev, object message, LogType logType)
	{
		StackTrace stackTrace = new StackTrace(fNeedFileInfo: true);
		SingletonMonoBehaviour<LogManager>.instance.SendLog(GetDev(dev) + message, stackTrace.ToString(), logType);
	}

	public static void LogException(string dev, string message, Exception e)
	{
		LogError(dev, message + " :: " + e.ToString());
	}

	public static void Log(string message, bool forceLoggly = false)
	{
		Log("all", message);
	}

	public static void LogWarning(string message, bool forceLoggly = false)
	{
		LogWarning("all", message);
	}

	public static void LogError(string message, bool forceLoggly = true)
	{
		LogError("all", message);
	}

	public static void LogException(string message, Exception e)
	{
		LogError("all", message + " :: " + e.ToString());
	}

	private static string GetDev(string dev)
	{
		return "::" + dev.ToUpper() + "::";
	}
}
