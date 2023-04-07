using System;
using SimpleJSON;
using UnityEngine;

public class EnvironmentManager : BaseManager<EnvironmentManager>
{
	public JSONNode config { get; private set; }

	public bool initialized { get; private set; }

	public TextAsset configText { get; private set; }

	public bool developmentMode
	{
		get
		{
			JSONNode configValue = GetConfigValue("DevelopmentMode");
			if (configValue != null)
			{
				return configValue.AsBool;
			}
			return false;
		}
	}

	public string environmentName
	{
		get
		{
			JSONNode configValue = GetConfigValue("name");
			if (!(configValue != null))
			{
				return null;
			}
			return configValue;
		}
	}

	public bool loadDLCFromBundledResources
	{
		get
		{
			JSONNode configValue = GetConfigValue("LoadDLCFromBundledResources");
			if (configValue != null)
			{
				return configValue.AsBool;
			}
			return false;
		}
	}

	private void Awake()
	{
		SetDontDestroyOnLoad();
		if (SingletonMonoBehaviour<EnvironmentManager>.exists)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void forceInit()
	{
		initialized = false;
		Initialize();
	}

	public void Initialize()
	{
		if (initialized)
		{
			return;
		}
		configText = SingletonMonoBehaviour<AssetManager>.instance.LoadAssetFromResources<TextAsset>("data/config/environment");
		if ((bool)configText)
		{
			try
			{
				config = JSON.Parse(configText.text);
				if (config == null)
				{
					Debug.LogWarning("Cannot parse config");
				}
				else
				{
					initialized = true;
				}
				return;
			}
			catch (Exception)
			{
				Debug.LogError("Failed to parse environment config.");
				return;
			}
		}
		Debug.LogError("Environment config is missing.");
	}

	public string GetConfigAsUrlCacheBuster(string key)
	{
		string text = GetConfigValue(key);
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text + "?t=" + (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public JSONNode GetConfigValue(string key)
	{
		JSONNode result = null;
		if (initialized)
		{
			if (config != null)
			{
				result = config[key];
			}
			else
			{
				Debug.LogWarning(GetType().Name + ".config is null");
			}
		}
		else
		{
			Debug.LogWarning(GetType().Name + " is not initialized yet.");
		}
		return result;
	}
}
