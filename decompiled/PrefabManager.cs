using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : BaseManager<PrefabManager>
{
	public TextAsset prefabsCSV;

	public float yieldPeriod = 0.03f;

	[HideInInspector]
	public GameObject[] prefabs;

	private Dictionary<string, GameObject> m_prefabLookUp = new Dictionary<string, GameObject>();

	private Dictionary<string, string> m_prefabNamesByKey = new Dictionary<string, string>();

	private List<string> m_updatedPrefabs = new List<string>();

	public bool initialized { get; private set; }

	private void OnDisable()
	{
		AssetManager.assetsUpdatedEvent -= OnAssetsUpdated;
	}

	private void Awake()
	{
		SetDontDestroyOnLoad();
		if (SingletonMonoBehaviour<PrefabManager>.exists)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Restore()
	{
		initialized = false;
	}

	public IEnumerator Initialize()
	{
		if (!initialized)
		{
			float lastYieldTime = Time.realtimeSinceStartup;
			GameObject[] array = prefabs;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					if (m_prefabLookUp.ContainsKey(gameObject.name))
					{
						Debug.LogWarning("Duplicate prefab " + gameObject.name, base.gameObject);
					}
					else
					{
						m_prefabLookUp[gameObject.name] = gameObject;
					}
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (realtimeSinceStartup - lastYieldTime > yieldPeriod)
				{
					lastYieldTime = realtimeSinceStartup;
					yield return null;
				}
			}
			if (prefabsCSV != null)
			{
				LoadCSV(prefabsCSV);
			}
			else
			{
				Debug.LogError("prefabs CSV is empty.", base.gameObject);
				StopAllCoroutines();
				yield return null;
			}
			AssetManager.assetsUpdatedEvent += OnAssetsUpdated;
			initialized = true;
			yield return null;
		}
		else
		{
			Debug.LogWarning(GetType().Name + " is already initialized.");
		}
	}

	public void LoadCSV(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size != 2)
		{
			return;
		}
		m_prefabNamesByKey.Clear();
		while (betterList != null)
		{
			if (betterList.size == 2)
			{
				if (!m_prefabNamesByKey.ContainsKey(betterList[0]))
				{
					m_prefabNamesByKey.Add(betterList[0], betterList[1]);
				}
				else
				{
					Debug.LogWarning("Duplicate key in prefab file: " + betterList[0] + " - ignoring", this);
				}
			}
			else
			{
				Debug.LogWarning("reading prefab config, incorrect row size: " + betterList.size, this);
			}
			betterList = byteReader.ReadCSV();
		}
	}

	public GameObject GetPrefab(string prefabName, string prefabPath = null, bool suppressDebugLog = false)
	{
		GameObject value = null;
		if (!string.IsNullOrEmpty(prefabName))
		{
			CheckBundlesForPrefab(prefabName);
			m_prefabLookUp.TryGetValue(prefabName, out value);
			if (value == null)
			{
				value = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset<GameObject>(prefabName, prefabPath);
				if (!suppressDebugLog)
				{
					Debug.LogWarning("Prefab not found " + prefabName, this);
				}
			}
		}
		else if (!suppressDebugLog)
		{
			Debug.LogWarning("Cannot get prefab; prefab name is null or empty.", this);
		}
		return value;
	}

	public GameObject GetPrefabEditor(string prefabName)
	{
		GameObject gameObject = null;
		if (!string.IsNullOrEmpty(prefabName))
		{
			GameObject[] array = prefabs;
			foreach (GameObject gameObject2 in array)
			{
				if (gameObject2 != null && gameObject2.name == prefabName)
				{
					gameObject = gameObject2;
					break;
				}
			}
			if (gameObject == null)
			{
				Debug.LogWarning("Prefab not found: " + prefabName, this);
			}
		}
		else
		{
			Debug.LogWarning("Cannot get prefab; prefab name is null or empty.", this);
		}
		return gameObject;
	}

	public GameObject AddChildPrefab(GameObject parent, string childPrefabName, bool worldPositionStays = false, bool suppressDebugLog = false)
	{
		GameObject prefab = SingletonMonoBehaviour<PrefabManager>.instance.GetPrefab(childPrefabName, null, suppressDebugLog);
		return GameObjectUtil.AddChild(parent, prefab, worldPositionStays, suppressDebugLog);
	}

	private void OnAssetsUpdated()
	{
		m_updatedPrefabs.Clear();
	}

	private void CheckBundlesForPrefab(string prefabName)
	{
		if (m_updatedPrefabs.Contains(prefabName))
		{
			return;
		}
		GameObject gameObject = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset(prefabName) as GameObject;
		if (gameObject != null)
		{
			if (m_prefabLookUp.ContainsKey(prefabName))
			{
				m_prefabLookUp[prefabName] = gameObject;
			}
			else
			{
				m_prefabLookUp.Add(prefabName, gameObject);
			}
		}
		m_updatedPrefabs.Add(prefabName);
	}

	public string GetPrefabName(string key, bool suppressDebugLog = false)
	{
		string value = null;
		if (m_prefabNamesByKey != null)
		{
			bool flag = false;
			if (!flag)
			{
				flag = m_prefabNamesByKey.TryGetValue(key, out value);
			}
			if (flag)
			{
				if (string.IsNullOrEmpty(value) && !suppressDebugLog)
				{
					Debug.LogWarning("Prefab value is empty for " + key, this);
				}
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Prefab key not found " + key, this);
			}
		}
		return value;
	}
}
