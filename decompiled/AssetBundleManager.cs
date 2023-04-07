using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : BaseManager<AssetBundleManager>
{
	public const string ISO8601_BASIC_DATE_TIME_FORMAT = "yyyyMMddTHHmmssZ";

	private const long MINIMUM_BYTES_REQUIRED = 100000000L;

	private const int DEFAULT_CONCURRENT_LIMIT = 3;

	private const int LOW_END_CONCURRENT_LIMIT = 2;

	private const string CLEAR_CACHE_OLD_BUNDLES_BY_HASH_ENABLED_KEY = "clearOldBundlesByHash";

	private const string CACHE_CLEAR_ALL_ORPHANED_BUNDLES_ENABLED_KEY = "clearAllOrphanedBundles";

	private const string CACHE_CLEARING_CONFIG_TITLEDATA_KEY = "CacheClearingConfig";

	private float _currentDownloadProgress;

	private UnityWebRequest m_manifestWWW;

	private string[] oldManifestBundles;

	private string[] m_manifestBundleNames;

	private bool m_disableFirstSessionDLC;

	public bool initializing { get; private set; }

	public bool initialized { get; private set; }

	public bool assetBundlesEnabled { get; private set; }

	public bool autoDLCDisabled { get; private set; }

	public string rootUrl { get; private set; }

	public int concurrentLimit { get; private set; }

	public float currentDownloadProgress
	{
		get
		{
			return _currentDownloadProgress;
		}
		private set
		{
			_currentDownloadProgress = value;
			if (AssetBundleManager.onProgressUpdate != null)
			{
				AssetBundleManager.onProgressUpdate(_currentDownloadProgress);
			}
		}
	}

	public bool lowStorageDetected { get; private set; }

	public AssetBundleManifest manifest { get; private set; }

	public Queue<AssetBundleRequestEntry> queuedRequestEntries { get; private set; }

	public Dictionary<string, AssetBundleRequestEntry> sentRequestEntriesByName { get; private set; }

	public Dictionary<string, AssetBundleRequestEntry> successfulRequestEntriesByName { get; private set; }

	public Dictionary<string, AssetBundleRequestEntry> failedRequestEntriesByName { get; private set; }

	public List<AssetBundleEntry> loadedAssetBundleEntries { get; private set; }

	public string cachedRootUrl { get; private set; }

	public string cachedManifestUrl { get; private set; }

	public int cachedManifestVersion { get; private set; }

	public string cachedClientVersion { get; private set; }

	public string cachedDLCVersion { get; private set; }

	public static event Action<float> onProgressUpdate;

	public AssetBundleManager()
	{
		queuedRequestEntries = new Queue<AssetBundleRequestEntry>();
		sentRequestEntriesByName = new Dictionary<string, AssetBundleRequestEntry>();
		successfulRequestEntriesByName = new Dictionary<string, AssetBundleRequestEntry>();
		failedRequestEntriesByName = new Dictionary<string, AssetBundleRequestEntry>();
		loadedAssetBundleEntries = new List<AssetBundleEntry>();
	}

	private void Awake()
	{
		concurrentLimit = 3;
		SetDontDestroyOnLoad();
	}

	private void Update()
	{
		if (assetBundlesEnabled && IsManifestLoaded())
		{
			UpdateSentRequests();
		}
	}

	private void OnDestroy()
	{
		UnloadAssetBundles();
	}

	public IEnumerator PreInitialize()
	{
		while (!Caching.ready)
		{
			yield return null;
		}
		assetBundlesEnabled = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("EnableAssetBundles").AsBool;
		autoDLCDisabled = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("DisableAutoDLC").AsBool;
		m_disableFirstSessionDLC = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("DisableFirstSessionDLC").AsBool;
		if (assetBundlesEnabled)
		{
			LoadCacheFieldsFromPlayerPrefs();
			if (CanLoadBundlesFromCache() && Caching.IsVersionCached(cachedManifestUrl, Hash128.Parse(cachedManifestVersion.ToString())))
			{
				m_manifestWWW = UnityWebRequestAssetBundle.GetAssetBundle(cachedManifestUrl, Hash128.Parse(cachedManifestVersion.ToString()));
				yield return m_manifestWWW.SendWebRequest();
				while (!m_manifestWWW.isDone)
				{
					yield return null;
				}
				if (DownloadHandlerAssetBundle.GetContent(m_manifestWWW) != null)
				{
					manifest = DownloadHandlerAssetBundle.GetContent(m_manifestWWW).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
					m_manifestBundleNames = manifest.GetAllAssetBundles();
				}
				string[] manifestBundleNames = GetManifestBundleNames();
				if (manifestBundleNames != null)
				{
					bool flag = true;
					string[] array = manifestBundleNames;
					foreach (string text in array)
					{
						string url = cachedRootUrl + "/" + text;
						Hash128 assetBundleHash = manifest.GetAssetBundleHash(text);
						if (!Caching.IsVersionCached(url, assetBundleHash))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						yield return StartCoroutine(LoadBundlesFromCache(manifestBundleNames));
					}
				}
			}
		}
		SetDirty();
	}

	public void Restore()
	{
		initialized = false;
		initializing = false;
	}

	public IEnumerator Initialize()
	{
		if (!initializing && !initialized)
		{
			initializing = true;
			SetDirty();
			bool flag = m_disableFirstSessionDLC && PlatformUtil.isIPhone;
			if (assetBundlesEnabled && !flag)
			{
				yield return StartCoroutine(CheckManifest());
			}
			initializing = false;
			initialized = true;
			SetDirty();
		}
		yield return null;
	}

	public IEnumerator Reset()
	{
		yield return StartCoroutine(PrepareForUpdate());
		initialized = false;
		initializing = false;
		SetDirty();
		yield return null;
	}

	private IEnumerator PrepareForUpdate()
	{
		successfulRequestEntriesByName.Clear();
		currentDownloadProgress = 0f;
		UnloadAssetBundles();
		SetDirty();
		yield return null;
		ClearManifest();
		SetDirty();
		yield return null;
	}

	private string GetManifestUrl()
	{
		BuildPlatform buildPlatform = PlatformUtil.GetBuildPlatform();
		return rootUrl + "/" + buildPlatform.ToString() + ".unity3d";
	}

	private string GetBundleUrl(string bundleName)
	{
		return rootUrl + "/" + bundleName;
	}

	public string GetDLCVersion()
	{
		string text = null;
		string text2 = PlatformUtil.GetBuildPlatform().ToString();
		string text3 = ((SystemInfoHelper.bundleVersion != null) ? SystemInfoHelper.bundleVersion.Replace(".", "_") : Application.version.Replace(".", "_"));
		string text4 = "DLC_" + text2 + "_" + text3;
		text = AppConfigManager.instance.GetDLCVersion(text4);
		D.Log("Get DLC Version :: for " + text4 + text);
		return text;
	}

	public bool IsNewDLCVersionAvailable()
	{
		bool result = false;
		string dLCVersion = GetDLCVersion();
		if (!string.IsNullOrEmpty(dLCVersion))
		{
			result = dLCVersion != cachedDLCVersion;
		}
		return result;
	}

	public bool IsManifestLoaded()
	{
		return manifest != null;
	}

	private string[] GetManifestBundleNames()
	{
		string[] result = null;
		if (IsManifestLoaded() && m_manifestBundleNames != null)
		{
			result = m_manifestBundleNames;
		}
		else
		{
			Debug.LogWarning("Manifest is not loaded.", this);
		}
		return result;
	}

	public string[] GetAllDependentBundleNames(string type)
	{
		List<string> list = new List<string>();
		string[] manifestBundleNames = GetManifestBundleNames();
		if (manifestBundleNames != null)
		{
			string[] array = manifestBundleNames;
			foreach (string text in array)
			{
				if ((!text.Contains(type) && !text.Contains("data")) || list.Contains(text))
				{
					continue;
				}
				list.Add(text);
				string[] allDependencies = manifest.GetAllDependencies(text);
				foreach (string item in allDependencies)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
		}
		return list.ToArray();
	}

	private bool DoesManifestContainBundle(string bundleName)
	{
		bool result = false;
		string[] manifestBundleNames = GetManifestBundleNames();
		if (manifestBundleNames != null)
		{
			string[] array = manifestBundleNames;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == bundleName)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public string GetDLCSize()
	{
		string result = "0";
		BuildPlatform buildPlatform = PlatformUtil.GetBuildPlatform();
		string text = SystemInfoHelper.bundleVersion.Replace(".", "_");
		string text2 = "";
		text2 = new Dictionary<string, string>
		{
			{ "DLC_ANDRIOD_1_0_SIZE", "123345678" },
			{ "DLC_IOS_1_0_SIZE", "123345678" }
		}["DLC_" + buildPlatform.ToString() + "_" + text + "_Size"];
		if (!string.IsNullOrEmpty(text2))
		{
			float result2 = 0f;
			float.TryParse(text2, out result2);
			if (result2 > 0f)
			{
				result = (result2 / 1000000f).ToString("##.##");
			}
		}
		return result;
	}

	private IEnumerator CheckManifest()
	{
		BuildPlatform buildPlatform = PlatformUtil.GetBuildPlatform();
		rootUrl = SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("downloadUrls")[buildPlatform.ToString()];
		string dLCVersion = GetDLCVersion();
		if (!string.IsNullOrEmpty(dLCVersion) && (string.IsNullOrEmpty(cachedDLCVersion) || !cachedDLCVersion.Equals(dLCVersion) || loadedAssetBundleEntries.Count == 0))
		{
			string[] array = dLCVersion.Split('_');
			DateTime date = DateTime.ParseExact(array[array.Length - 1], "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture);
			cachedManifestVersion = TimeUtil.ToUnixTimestamp(date);
			cachedDLCVersion = dLCVersion;
			rootUrl = rootUrl + "/" + cachedDLCVersion + "/" + buildPlatform;
			if (IsManifestLoaded())
			{
				oldManifestBundles = manifest.GetAllAssetBundles();
			}
			yield return StartCoroutine(PrepareForUpdate());
			string manifestUrl = GetManifestUrl();
			bool manifestBundleValid = false;
			_ = string.Empty;
			for (int i = 0; i < 3; i++)
			{
				UnityWebRequest initDataWWW = UnityWebRequestAssetBundle.GetAssetBundle(manifestUrl, Hash128.Parse(cachedManifestVersion.ToString()));
				yield return initDataWWW.SendWebRequest();
				while (!initDataWWW.isDone)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(initDataWWW.error))
				{
					string error = initDataWWW.error;
					initDataWWW.Dispose();
					string error2 = GetType().Name + " WWW Error (" + manifestUrl + "): " + error + "  Retries: " + (2 - i) + " remain";
					_ = i;
					LogError(error2);
					_ = i;
					yield return new WaitForSeconds(2f);
					continue;
				}
				m_manifestWWW = initDataWWW;
				manifestBundleValid = true;
				break;
			}
			if (manifestBundleValid)
			{
				if (m_manifestWWW != null && DownloadHandlerAssetBundle.GetContent(m_manifestWWW) != null)
				{
					manifest = DownloadHandlerAssetBundle.GetContent(m_manifestWWW).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
					m_manifestBundleNames = manifest.GetAllAssetBundles();
					if (ShouldClearAllOrphanedBundles() && manifest != null && oldManifestBundles != null && oldManifestBundles.Count() > 0)
					{
						string[] allAssetBundles = manifest.GetAllAssetBundles();
						if (allAssetBundles != null && allAssetBundles.Count() > 0)
						{
							IEnumerable<string> enumerable = oldManifestBundles.Except(allAssetBundles);
							if (enumerable != null && enumerable.Count() > 0)
							{
								foreach (string item in enumerable)
								{
									Caching.ClearAllCachedVersions(item);
								}
							}
						}
						oldManifestBundles = null;
					}
				}
				if (manifest == null)
				{
					string error3 = "<LOCALIZE>";
					LogError(error3);
				}
				SaveCacheFieldsToPlayerPrefs();
				bool flag = false;
				if (!flag)
				{
				}
				yield return StartCoroutine(DownloadBundles(flag));
				yield return SingletonMonoBehaviour<GameManager>.instance.StartCoroutine(SingletonMonoBehaviour<GameManager>.instance.HardRestart());
			}
		}
		yield return null;
	}

	private void ClearManifest()
	{
		manifest = null;
		m_manifestBundleNames = null;
		if (m_manifestWWW != null)
		{
			if (DownloadHandlerAssetBundle.GetContent(m_manifestWWW) != null)
			{
				DownloadHandlerAssetBundle.GetContent(m_manifestWWW).Unload(unloadAllLoadedObjects: true);
			}
			m_manifestWWW.Dispose();
			m_manifestWWW = null;
		}
	}

	private AssetBundleRequestEntry GetQueuedRequestEntry(string bundleName)
	{
		AssetBundleRequestEntry result = null;
		if (queuedRequestEntries != null)
		{
			foreach (AssetBundleRequestEntry queuedRequestEntry in queuedRequestEntries)
			{
				if (queuedRequestEntry != null && queuedRequestEntry.name == bundleName)
				{
					return queuedRequestEntry;
				}
			}
			return result;
		}
		return result;
	}

	private bool IsRequestQueued(string bundleName)
	{
		return GetQueuedRequestEntry(bundleName) != null;
	}

	private AssetBundleRequestEntry GetSentRequestEntry(string bundleName)
	{
		AssetBundleRequestEntry value = null;
		if (sentRequestEntriesByName != null)
		{
			sentRequestEntriesByName.TryGetValue(bundleName, out value);
		}
		return null;
	}

	private bool IsRequestSent(string bundleName)
	{
		return GetSentRequestEntry(bundleName) != null;
	}

	private void SendRequest(AssetBundleRequestEntry requestEntry)
	{
		if (requestEntry != null)
		{
			bool num = Caching.IsVersionCached(requestEntry.url, requestEntry.hash);
			D.Log("requestEntry.url " + requestEntry.url);
			requestEntry.www = UnityWebRequestAssetBundle.GetAssetBundle(requestEntry.url, requestEntry.hash);
			requestEntry.www.SendWebRequest();
			sentRequestEntriesByName.Add(requestEntry.name, requestEntry);
			if (!num && 1 == 0 && !SingletonMonoBehaviour<AssetBundleManager>.instance.lowStorageDetected)
			{
				lowStorageDetected = true;
			}
		}
	}

	private void UpdateSentRequests()
	{
		List<AssetBundleRequestEntry> list = null;
		List<AssetBundleRequestEntry> list2 = null;
		foreach (AssetBundleRequestEntry value in sentRequestEntriesByName.Values)
		{
			if (value.www == null)
			{
				continue;
			}
			if (value.www.error != null)
			{
				if (list2 == null)
				{
					list2 = new List<AssetBundleRequestEntry>();
				}
				list2.Add(value);
				Debug.LogError("Download " + value.url + " failed with \nError: " + value.www.error);
			}
			else if (value.www.isDone)
			{
				if (list == null)
				{
					list = new List<AssetBundleRequestEntry>();
				}
				list.Add(value);
			}
		}
		if (list != null)
		{
			foreach (AssetBundleRequestEntry item in list)
			{
				if (!failedRequestEntriesByName.ContainsKey(item.name))
				{
					successfulRequestEntriesByName.Add(item.name, item);
				}
				sentRequestEntriesByName.Remove(item.name);
			}
			SetDirty();
		}
		if (list2 != null)
		{
			foreach (AssetBundleRequestEntry item2 in list2)
			{
				if (!failedRequestEntriesByName.ContainsKey(item2.name))
				{
					failedRequestEntriesByName.Add(item2.name, item2);
				}
				sentRequestEntriesByName.Remove(item2.name);
			}
			SetDirty();
		}
		while (sentRequestEntriesByName.Count < concurrentLimit && queuedRequestEntries.Count > 0)
		{
			AssetBundleRequestEntry assetBundleRequestEntry = queuedRequestEntries.Dequeue();
			if (assetBundleRequestEntry != null)
			{
				SendRequest(assetBundleRequestEntry);
			}
			SetDirty();
		}
	}

	public AssetBundleEntry GetLoadedBundleEntry(string bundleName)
	{
		AssetBundleEntry result = null;
		if (loadedAssetBundleEntries != null)
		{
			foreach (AssetBundleEntry loadedAssetBundleEntry in loadedAssetBundleEntries)
			{
				if (loadedAssetBundleEntry != null && loadedAssetBundleEntry.name == bundleName)
				{
					return loadedAssetBundleEntry;
				}
			}
			return result;
		}
		return result;
	}

	private bool IsBundleLoaded(string bundleName)
	{
		return GetLoadedBundleEntry(bundleName) != null;
	}

	public UnityEngine.Object LoadAsset(string bundleName, string assetFileName, Type systemTypeReference = null)
	{
		UnityEngine.Object result = null;
		AssetBundleEntry loadedBundleEntry = GetLoadedBundleEntry(bundleName);
		if (loadedBundleEntry != null)
		{
			string text = "assets/bundledresources/" + Path.GetFileNameWithoutExtension(bundleName).ToLower() + "/" + assetFileName;
			result = ((systemTypeReference != null) ? loadedBundleEntry.assetBundle.LoadAsset(text, systemTypeReference) : loadedBundleEntry.assetBundle.LoadAsset(text));
		}
		return result;
	}

	public float GetDownloadProgress(string[] bundleNames)
	{
		float num = 0f;
		if (IsManifestLoaded())
		{
			bool flag = true;
			string[] array = bundleNames;
			foreach (string key in array)
			{
				if (!successfulRequestEntriesByName.ContainsKey(key))
				{
					flag = false;
					break;
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			array = bundleNames;
			foreach (string text in array)
			{
				string[] allDependencies = manifest.GetAllDependencies(text);
				foreach (string item in allDependencies)
				{
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
					}
				}
				if (!hashSet.Contains(text))
				{
					hashSet.Add(text);
				}
			}
			if (hashSet.Count > 0)
			{
				float num2 = 1f / Convert.ToSingle(hashSet.Count);
				foreach (string item2 in hashSet)
				{
					if (sentRequestEntriesByName.ContainsKey(item2))
					{
						num += sentRequestEntriesByName[item2].www.downloadProgress * num2;
					}
					if (successfulRequestEntriesByName.ContainsKey(item2))
					{
						num += num2;
					}
				}
			}
			if (num >= 1f && sentRequestEntriesByName.Count > 0)
			{
				num = 0.99f;
			}
			else if (num < 1f && flag)
			{
				num = 1f;
			}
		}
		return num;
	}

	private bool IsDownloadingBundle(string bundleName)
	{
		if (!IsRequestQueued(bundleName))
		{
			return IsRequestSent(bundleName);
		}
		return true;
	}

	private bool TryDownloadBundle(string bundleName)
	{
		bool result = false;
		if (!IsDownloadingBundle(bundleName) && !successfulRequestEntriesByName.ContainsKey(bundleName))
		{
			if (DoesManifestContainBundle(bundleName))
			{
				AssetBundleRequestEntry item = new AssetBundleRequestEntry
				{
					name = bundleName,
					url = GetBundleUrl(bundleName),
					hash = manifest.GetAssetBundleHash(bundleName)
				};
				queuedRequestEntries.Enqueue(item);
				result = true;
			}
			else
			{
				Debug.LogError("Cannot download bundle [" + bundleName + "]. It's not in the bundle manifest.", this);
			}
		}
		return result;
	}

	private void StartDownload(string bundleName)
	{
		if (IsManifestLoaded())
		{
			TryDownloadBundle(bundleName);
		}
		else
		{
			Debug.LogWarning("Manifest is not loaded yet.", this);
		}
	}

	private IEnumerator DownloadBundles(bool dataOnly = false)
	{
		string[] bundleNames = ((!dataOnly) ? GetManifestBundleNames() : GetAllDependentBundleNames("default_data"));
		if (bundleNames == null)
		{
			yield break;
		}
		string[] array = bundleNames;
		foreach (string bundleName in array)
		{
			StartDownload(bundleName);
		}
		do
		{
			currentDownloadProgress = GetDownloadProgress(bundleNames);
			yield return null;
		}
		while (currentDownloadProgress < 1f);
		loadedAssetBundleEntries.Clear();
		if (ShouldClearOldBundlesByHash())
		{
			foreach (AssetBundleRequestEntry value in successfulRequestEntriesByName.Values)
			{
				Caching.ClearOtherCachedVersions(value.name, value.hash);
			}
			if (manifest != null && cachedManifestVersion != 0 && !string.IsNullOrEmpty(cachedManifestUrl))
			{
				Caching.ClearOtherCachedVersions(Path.GetFileNameWithoutExtension(cachedManifestUrl), Hash128.Parse(cachedManifestVersion.ToString()));
			}
		}
		foreach (AssetBundleRequestEntry value2 in successfulRequestEntriesByName.Values)
		{
			AssetBundleEntry item = new AssetBundleEntry
			{
				name = value2.name,
				url = value2.url,
				hash = value2.hash,
				assetBundle = DownloadHandlerAssetBundle.GetContent(value2.www)
			};
			loadedAssetBundleEntries.Add(item);
		}
	}

	public bool AreAllBundlesLoaded()
	{
		bool result = false;
		if (!assetBundlesEnabled)
		{
			result = true;
		}
		else if (IsManifestLoaded())
		{
			string[] manifestBundleNames = GetManifestBundleNames();
			result = manifestBundleNames != null && loadedAssetBundleEntries != null && manifestBundleNames.Length == loadedAssetBundleEntries.Count;
		}
		return result;
	}

	private void UnloadAssetBundles()
	{
		if (loadedAssetBundleEntries == null)
		{
			return;
		}
		foreach (AssetBundleEntry loadedAssetBundleEntry in loadedAssetBundleEntries)
		{
			if (loadedAssetBundleEntry != null && loadedAssetBundleEntry.assetBundle != null)
			{
				loadedAssetBundleEntry.assetBundle.Unload(unloadAllLoadedObjects: true);
			}
		}
		loadedAssetBundleEntries.Clear();
	}

	public void ResetCacheFields()
	{
		cachedRootUrl = null;
		cachedManifestUrl = null;
		cachedManifestVersion = 0;
		cachedDLCVersion = string.Empty;
		cachedClientVersion = string.Empty;
		PlayerPrefsHelper.DeleteKey("ManifestUrl", saveImmediately: false);
		PlayerPrefsHelper.DeleteKey("CachedRootUrl", saveImmediately: false);
		PlayerPrefsHelper.DeleteKey("DLCVersion", saveImmediately: false);
		PlayerPrefsHelper.DeleteKey("ManifestVersion", saveImmediately: false);
		PlayerPrefsHelper.DeleteKey("LastCachedClientVersion", saveImmediately: false);
		PlayerPrefsHelper.Save();
		SetDirty();
	}

	private void LoadCacheFieldsFromPlayerPrefs()
	{
		cachedRootUrl = PlayerPrefsHelper.GetString("CachedRootUrl");
		cachedManifestUrl = PlayerPrefsHelper.GetString("ManifestUrl");
		cachedManifestVersion = PlayerPrefsHelper.GetInt("ManifestVersion");
		cachedDLCVersion = PlayerPrefsHelper.GetString("DLCVersion");
		cachedClientVersion = PlayerPrefsHelper.GetString("LastCachedClientVersion");
		SetDirty();
	}

	private void SaveCacheFieldsToPlayerPrefs()
	{
		PlayerPrefsHelper.SetString("ManifestUrl", GetManifestUrl(), saveImmediately: false);
		PlayerPrefsHelper.SetString("CachedRootUrl", rootUrl, saveImmediately: false);
		PlayerPrefsHelper.SetString("DLCVersion", cachedDLCVersion, saveImmediately: false);
		PlayerPrefsHelper.SetInt("ManifestVersion", cachedManifestVersion, saveImmediately: false);
		PlayerPrefsHelper.SetString("LastCachedClientVersion", SystemInfoHelper.bundleVersion, saveImmediately: false);
		PlayerPrefsHelper.Save();
	}

	private bool CanLoadBundlesFromCache()
	{
		if (!string.IsNullOrEmpty(cachedClientVersion) && cachedClientVersion.Equals(SystemInfoHelper.bundleVersion) && !string.IsNullOrEmpty(cachedDLCVersion) && !string.IsNullOrEmpty(cachedManifestUrl) && !string.IsNullOrEmpty(cachedRootUrl) && !string.IsNullOrEmpty(cachedClientVersion))
		{
			return loadedAssetBundleEntries.Count == 0;
		}
		return false;
	}

	private IEnumerator LoadBundlesFromCache(string[] bundleNames)
	{
		loadedAssetBundleEntries.Clear();
		foreach (string bundleName in bundleNames)
		{
			UnityWebRequest cachedWWW = UnityWebRequestAssetBundle.GetAssetBundle(GetBundleUrl(bundleName), manifest.GetAssetBundleHash(bundleName));
			yield return cachedWWW.SendWebRequest();
			while (!cachedWWW.isDone)
			{
				yield return null;
			}
			AssetBundleEntry item = new AssetBundleEntry
			{
				name = bundleName,
				url = GetBundleUrl(bundleName),
				hash = manifest.GetAssetBundleHash(bundleName),
				assetBundle = DownloadHandlerAssetBundle.GetContent(cachedWWW)
			};
			loadedAssetBundleEntries.Add(item);
		}
		SetDirty();
		yield return null;
	}

	private void LogError(string error)
	{
		Debug.LogError(error, this);
	}

	private bool ShouldClearOldBundlesByHash()
	{
		return GetTitleDataConfigBoolValueForKey("clearOldBundlesByHash");
	}

	private bool ShouldClearAllOrphanedBundles()
	{
		return GetTitleDataConfigBoolValueForKey("clearAllOrphanedBundles");
	}

	private bool GetTitleDataConfigBoolValueForKey(string key)
	{
		return Convert.ToBoolean(new Dictionary<string, string>
		{
			{ "clearOldBundlesByHash", "false" },
			{ "clearAllOrphanedBundles", "false" }
		}[key]);
	}
}
