using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpriteManager : BaseManager<SpriteManager>
{
	private static readonly Regex m_referenceRegex = new Regex("[[]{2}(.*)[]]{2}");

	public TextAsset spritesCSV;

	public Sprite missingSprite;

	[HideInInspector]
	public Sprite[] sprites;

	private Dictionary<string, Sprite> m_spritesByName = new Dictionary<string, Sprite>();

	private Dictionary<string, string> m_spriteNamesByKey = new Dictionary<string, string>();

	private List<string> m_updatedSprites = new List<string>();

	public bool initialized { get; private set; }

	private void OnDisable()
	{
		AssetManager.assetsUpdatedEvent -= OnAssetsUpdated;
	}

	private void Awake()
	{
		SetDontDestroyOnLoad();
		if (SingletonMonoBehaviour<SpriteManager>.exists)
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
			Sprite[] array = sprites;
			foreach (Sprite sprite in array)
			{
				if (sprite != null)
				{
					if (!m_spritesByName.ContainsKey(sprite.name))
					{
						m_spritesByName[sprite.name] = sprite;
					}
					else
					{
						Debug.LogWarning("Duplicate sprite " + sprite.name, base.gameObject);
					}
				}
			}
			yield return null;
			if (missingSprite == null)
			{
				Debug.LogWarning("No missing sprite is undefined.", base.gameObject);
			}
			if (spritesCSV != null)
			{
				SingletonMonoBehaviour<SpriteManager>.instance.LoadCSV(spritesCSV);
			}
			else
			{
				Debug.LogError("sprites CSV is empty.", base.gameObject);
				StopAllCoroutines();
				yield return null;
			}
			AssetManager.assetsUpdatedEvent += OnAssetsUpdated;
			initialized = true;
			yield return null;
		}
		else
		{
			Debug.LogWarning(GetType().Name + " is already initialized.", this);
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
		m_spriteNamesByKey.Clear();
		while (betterList != null)
		{
			if (betterList.size == 2)
			{
				if (!m_spriteNamesByKey.ContainsKey(betterList[0]))
				{
					m_spriteNamesByKey.Add(betterList[0], betterList[1]);
				}
				else
				{
					Debug.LogWarning("Duplicate key in sprites file: " + betterList[0] + " - ignoring", this);
				}
			}
			else
			{
				Debug.LogWarning("reading sprite config, incorrect row size: " + betterList.size, this);
			}
			betterList = byteReader.ReadCSV();
		}
	}

	private void OnAssetsUpdated()
	{
		m_updatedSprites.Clear();
	}

	public string ReplaceTokens(string spriteName)
	{
		string text = spriteName;
		if (!string.IsNullOrEmpty(text))
		{
			MatchCollection matchCollection = m_referenceRegex.Matches(text);
			if (matchCollection != null)
			{
				foreach (Match item in matchCollection)
				{
					_ = item.Value;
					string value = item.Groups[1].Value;
					text = GetSpriteName(value);
				}
				return text;
			}
		}
		return text;
	}

	public string GetSpriteName(string key, bool suppressDebugLog = false)
	{
		string value = null;
		if (m_spriteNamesByKey != null)
		{
			bool flag = false;
			if (!flag)
			{
				flag = m_spriteNamesByKey.TryGetValue(key, out value);
			}
			if (flag)
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = ReplaceTokens(value);
				}
				else if (!suppressDebugLog)
				{
					Debug.LogWarning("Sprite value is empty for " + key, this);
				}
			}
			else if (!suppressDebugLog)
			{
				Debug.LogWarning("Sprite key not found " + key, this);
			}
		}
		return value;
	}

	public string GetPrefixedSpriteName(string prefix, string key, bool suppressDebugLog = false)
	{
		return GetSpriteName(prefix + key, suppressDebugLog);
	}

	private void CheckBundlesForSprite(string spriteName)
	{
		if (m_updatedSprites.Contains(spriteName))
		{
			return;
		}
		Sprite sprite = null;
		Object @object = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset(spriteName, null, typeof(Sprite));
		if (@object != null)
		{
			sprite = (Sprite)@object;
		}
		else
		{
			@object = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset(spriteName);
			if (@object is Texture2D)
			{
				Texture2D texture2D = (Texture2D)@object;
				sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			}
		}
		if (sprite != null)
		{
			if (m_spritesByName.ContainsKey(spriteName))
			{
				m_spritesByName[spriteName] = sprite;
			}
			else
			{
				m_spritesByName.Add(spriteName, sprite);
			}
		}
		m_updatedSprites.Add(spriteName);
	}

	public Sprite LoadSprite(string spriteName, bool suppressDebugLog = false)
	{
		Sprite value = null;
		if (!string.IsNullOrEmpty(spriteName))
		{
			CheckBundlesForSprite(spriteName);
			m_spritesByName.TryGetValue(spriteName, out value);
		}
		if (value == null)
		{
			value = missingSprite;
			if (!string.IsNullOrEmpty(spriteName) && !suppressDebugLog)
			{
				Debug.LogWarning("Missing sprite " + spriteName, this);
			}
		}
		return value;
	}

	public Sprite LoadPrefixedSprite(string prefix, string key, bool suppressDebugLog = false)
	{
		Sprite result = null;
		string prefixedSpriteName = GetPrefixedSpriteName(prefix, key, suppressDebugLog);
		if (!string.IsNullOrEmpty(prefixedSpriteName))
		{
			result = LoadSprite(prefixedSpriteName, suppressDebugLog);
		}
		return result;
	}

	public Sprite LoadPrefixedSpriteForCurrentGameMode(string prefix, string key, bool suppressDebugLog = false)
	{
		return LoadPrefixedSprite(prefix, key, suppressDebugLog);
	}
}
