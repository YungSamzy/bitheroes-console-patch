using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetManager : BaseManager<AssetManager>
{
	public const string BUNDLED_RESOURCES_FOLDER_NAME = "Assets/BundledResources";

	public const string PATH_INTRO = "intro/";

	public bool initializing { get; private set; }

	public bool initialized { get; private set; }

	public Dictionary<string, Hash128> bundleVersions { get; private set; }

	public Dictionary<string, string> bundleNamesByAssetName { get; private set; }

	public Dictionary<string, UnityEngine.Object> cachedResources { get; private set; }

	public static event Action assetsUpdatedEvent;

	public AssetManager()
	{
		bundleVersions = new Dictionary<string, Hash128>();
		bundleNamesByAssetName = new Dictionary<string, string>();
		cachedResources = new Dictionary<string, UnityEngine.Object>();
	}

	public IEnumerator PreInitialize()
	{
		UpdateAssets();
		yield return null;
	}

	public void Restore()
	{
		initializing = false;
	}

	public IEnumerator Initialize()
	{
		initializing = true;
		SetDirty();
		if (IsAssetUpdateRequired())
		{
			UpdateAssets();
		}
		initializing = false;
		initialized = true;
		SetDirty();
		yield return null;
	}

	private string GetAssetName(string assetPath)
	{
		return Path.GetFileNameWithoutExtension(assetPath).ToLower();
	}

	public bool IsAssetUpdateRequired()
	{
		bool result = false;
		if (SingletonMonoBehaviour<AssetBundleManager>.instance.assetBundlesEnabled)
		{
			List<AssetBundleEntry> loadedAssetBundleEntries = SingletonMonoBehaviour<AssetBundleManager>.instance.loadedAssetBundleEntries;
			if (loadedAssetBundleEntries != null)
			{
				foreach (AssetBundleEntry item in loadedAssetBundleEntries)
				{
					if (item != null)
					{
						Hash128 value = default(Hash128);
						if (!bundleVersions.TryGetValue(item.name, out value) || !value.Equals(item.hash))
						{
							return true;
						}
					}
				}
				return result;
			}
		}
		return result;
	}

	public void UpdateAssets()
	{
		if (!SingletonMonoBehaviour<AssetBundleManager>.instance.assetBundlesEnabled)
		{
			return;
		}
		List<AssetBundleEntry> loadedAssetBundleEntries = SingletonMonoBehaviour<AssetBundleManager>.instance.loadedAssetBundleEntries;
		if (loadedAssetBundleEntries == null || loadedAssetBundleEntries.Count <= 0)
		{
			return;
		}
		bundleVersions.Clear();
		bundleNamesByAssetName.Clear();
		foreach (AssetBundleEntry item in loadedAssetBundleEntries)
		{
			ProcessBundleAssets(item);
			bundleVersions.Add(item.name, item.hash);
		}
		_ = LoadAsset<TextAsset>("Localization") != null;
		TextAsset textAsset = LoadAsset<TextAsset>("Sprites");
		if (textAsset != null)
		{
			SingletonMonoBehaviour<SpriteManager>.instance.LoadCSV(textAsset);
		}
		TextAsset textAsset2 = LoadAsset<TextAsset>("Prefabs");
		if (textAsset2 != null)
		{
			SingletonMonoBehaviour<PrefabManager>.instance.LoadCSV(textAsset2);
		}
		if (AssetManager.assetsUpdatedEvent != null)
		{
			AssetManager.assetsUpdatedEvent();
		}
		SetDirty();
	}

	private void ProcessBundleAssets(AssetBundleEntry bundleEntry)
	{
		if (bundleEntry == null || !(bundleEntry.assetBundle != null))
		{
			return;
		}
		string value = bundleEntry.name;
		string[] allAssetNames = bundleEntry.assetBundle.GetAllAssetNames();
		for (int i = 0; i < allAssetNames.Length; i++)
		{
			string text = allAssetNames[i].Substring(24);
			string text2 = text.Substring(text.IndexOf("/") + 1).ToLower();
			if (!bundleNamesByAssetName.ContainsKey(text2))
			{
				bundleNamesByAssetName.Add(text2, value);
			}
			else
			{
				Debug.Log("Duplicate bundle asset found; " + text2, this);
			}
		}
	}

	private void Awake()
	{
		SetDontDestroyOnLoad();
		if (SingletonMonoBehaviour<AssetManager>.exists)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public UnityEngine.Object LoadAssetFromResources(string assetName, string assetPath = null, Type systemTypeReference = null)
	{
		if (!cachedResources.TryGetValue(assetName, out var value) || value == null)
		{
			if (value == null)
			{
				string path = assetPath + assetName;
				value = ((systemTypeReference != null) ? Resources.Load(path, systemTypeReference) : Resources.Load(path));
			}
			if (value == null)
			{
				string path2 = "intro/" + Path.GetFileNameWithoutExtension(assetName);
				value = ((systemTypeReference != null) ? Resources.Load(path2, systemTypeReference) : Resources.Load(path2));
			}
			if (value == null)
			{
				value = ((systemTypeReference != null) ? Resources.Load(assetName, systemTypeReference) : Resources.Load(assetName));
			}
			if (value != null)
			{
				cachedResources[assetName] = value;
				SetDirty();
			}
		}
		return value;
	}

	public T LoadAssetFromResources<T>(string assetName, string assetPath = null) where T : UnityEngine.Object
	{
		return LoadAssetFromResources(assetName, assetPath, typeof(T)) as T;
	}

	public UnityEngine.Object LoadAsset(string assetName, string assetPath = null, Type assetType = null)
	{
		UnityEngine.Object @object = null;
		if (!string.IsNullOrEmpty(assetName))
		{
			if (@object == null)
			{
				string text = assetName.ToLower();
				if (SingletonMonoBehaviour<AssetBundleManager>.instance.assetBundlesEnabled)
				{
					string value = null;
					bundleNamesByAssetName.TryGetValue(text, out value);
					if (!string.IsNullOrEmpty(value))
					{
						@object = SingletonMonoBehaviour<AssetBundleManager>.instance.LoadAsset(value, text, assetType);
					}
				}
				if (@object == null)
				{
					@object = LoadAssetFromResources(assetName, assetPath, assetType);
				}
			}
		}
		else
		{
			Debug.LogWarning("Cannot load asset; assetPath is null or empty.", this);
		}
		return @object;
	}

	public T LoadAsset<T>(string assetName, string assetPath = null) where T : UnityEngine.Object
	{
		return LoadAsset(assetName, assetPath, typeof(T)) as T;
	}

	public GameObject InstantiateAsset(string assetPath, Vector3 position = default(Vector3))
	{
		GameObject gameObject = null;
		GameObject gameObject2 = LoadAsset<GameObject>(assetPath);
		if (gameObject2 != null)
		{
			gameObject = UnityEngine.Object.Instantiate(gameObject2, position, Quaternion.identity);
			gameObject.name = Path.GetFileName(assetPath);
		}
		return gameObject;
	}
}
