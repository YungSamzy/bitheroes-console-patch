using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : BaseManager<ConfigManager>
{
	private Dictionary<string, Dictionary<string, object>> m_cache = new Dictionary<string, Dictionary<string, object>>();

	public void Awake()
	{
		SetDontDestroyOnLoad();
		if (SingletonMonoBehaviour<ConfigManager>.exists)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public T GetConfig<T>(string directory = "config", string key = null, bool suppressError = false)
	{
		T result = default(T);
		if (string.IsNullOrEmpty(key))
		{
			key = typeof(T).ToString();
		}
		Dictionary<string, object> value = null;
		if (!m_cache.TryGetValue(directory, out value))
		{
			value = new Dictionary<string, object>();
			m_cache[directory] = value;
		}
		object value2 = null;
		if (value.TryGetValue(key, out value2) && value2 != null)
		{
			result = (T)value2;
		}
		else
		{
			TextAsset textAsset = null;
			string patchConfig = GetPatchConfig(key);
			if (!string.IsNullOrEmpty(patchConfig))
			{
				textAsset = new TextAsset(patchConfig);
			}
			if (textAsset == null)
			{
				textAsset = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset<TextAsset>(key);
			}
			string text = null;
			if (textAsset == null)
			{
				text = "data/" + directory + "/" + key;
				textAsset = SingletonMonoBehaviour<AssetManager>.instance.LoadAssetFromResources<TextAsset>(text);
			}
			if (textAsset == null)
			{
				text = directory + "/" + key;
				textAsset = SingletonMonoBehaviour<AssetManager>.instance.LoadAssetFromResources<TextAsset>(text);
			}
			if (textAsset != null)
			{
				try
				{
					T val = JsonUtil.DeserializeObject<T>(textAsset.text);
					value[key] = val;
					result = val;
					return result;
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to deserialize config " + key + ": " + ex.Message);
					return result;
				}
			}
			if (!suppressError)
			{
				Debug.LogError("Failed to load config " + key);
			}
		}
		return result;
	}

	public string GetPatchConfig(string key)
	{
		return null;
	}
}
