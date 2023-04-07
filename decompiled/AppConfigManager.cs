using System;
using System.Collections;
using com.ultrabit.bitheroes.model.utility;
using SimpleJSON;
using UnityEngine.Networking;

public class AppConfigManager
{
	private JSONNode config;

	private static AppConfigManager _instance;

	public static AppConfigManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AppConfigManager();
			}
			return _instance;
		}
	}

	public IEnumerator Initialize()
	{
		string text = PlatformUtil.GetBuildPlatform().ToString() + ".json";
		string text2 = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("dlcPinningURL");
		string text3 = $"?x-bitheroes-rpg={DateTime.Now:yyyyMMddHHmmss}";
		D.Log("Initializing DLC Version " + text2 + text + text3);
		UnityWebRequest currentVersionWWW = UnityWebRequest.Get(text2 + text + text3);
		yield return currentVersionWWW.SendWebRequest();
		if (!string.IsNullOrEmpty(currentVersionWWW.error))
		{
			D.Log("Error requesting current DLC version: " + currentVersionWWW.error);
			yield break;
		}
		D.Log("DLC Version Download Complete");
		config = JSON.Parse(currentVersionWWW.downloadHandler.text);
	}

	public string GetDLCVersion(string clientVersion)
	{
		if (config != null)
		{
			return config["dlcver"][clientVersion];
		}
		return null;
	}

	public JSONNode GetAppConfig()
	{
		return config;
	}
}
