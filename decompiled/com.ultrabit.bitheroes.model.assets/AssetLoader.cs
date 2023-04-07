using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.assets;

public class AssetLoader : MonoBehaviour
{
	public const string SUFFIX_SWF = ".swf";

	public const string SUFFIX_PREFAB = ".prefab";

	[SerializeField]
	private Sprite _buttonGreenUp;

	[SerializeField]
	private Sprite _buttonGreenDown;

	[SerializeField]
	private Sprite _buttonGreenOver;

	[SerializeField]
	private Sprite _buttonRedUp;

	[SerializeField]
	private Sprite _buttonRedDown;

	[SerializeField]
	private Sprite _buttonRedOver;

	[SerializeField]
	private Sprite _buttonOrangeUp;

	[SerializeField]
	private Sprite _buttonOrangeDown;

	[SerializeField]
	private Sprite _buttonOrangeOver;

	[SerializeField]
	private Sprite _buttonBlueUp;

	[SerializeField]
	private Sprite _buttonBlueDown;

	[SerializeField]
	private Sprite _buttonBlueOver;

	[SerializeField]
	private Sprite _buttonDarkBlueUp;

	[SerializeField]
	private Sprite _buttonDarkBlueDown;

	[SerializeField]
	private Sprite _buttonDarkBlueOver;

	[SerializeField]
	private Sprite _buttonPurpleUp;

	[SerializeField]
	private Sprite _buttonPurpleDown;

	[SerializeField]
	private Sprite _buttonPurpleOver;

	private Dictionary<int, Sprite> _squareButtonColors;

	private void Start()
	{
		_squareButtonColors = new Dictionary<int, Sprite>
		{
			{ 0, _buttonGreenUp },
			{ 1, _buttonGreenDown },
			{ 2, _buttonGreenOver },
			{ 3, _buttonRedUp },
			{ 4, _buttonRedDown },
			{ 5, _buttonRedOver },
			{ 6, _buttonOrangeUp },
			{ 7, _buttonOrangeDown },
			{ 8, _buttonOrangeOver },
			{ 9, _buttonBlueUp },
			{ 10, _buttonBlueDown },
			{ 11, _buttonBlueOver },
			{ 12, _buttonDarkBlueUp },
			{ 13, _buttonDarkBlueDown },
			{ 14, _buttonDarkBlueOver },
			{ 15, _buttonPurpleUp },
			{ 16, _buttonPurpleDown },
			{ 17, _buttonPurpleOver }
		};
	}

	public Transform GetTransformAsset(string url, bool instantiate = true)
	{
		GameObject gameObjectAsset = GetGameObjectAsset(url, instantiate);
		if (gameObjectAsset != null)
		{
			return gameObjectAsset.transform;
		}
		return null;
	}

	public Transform GetTransformAsset(int assetType, string asset, bool instantiate = true)
	{
		if (UrlIsSWF(asset))
		{
			asset.Replace(".swf", ".prefab");
		}
		string path = AssetURL.GetPath(assetType, asset);
		if (path != null)
		{
			return GetTransformAsset(path, instantiate);
		}
		return null;
	}

	public GameObject GetGameObjectAsset(string assetURL, bool instantiate = true)
	{
		GameObject gameObject = null;
		switch (AssetURL.GetAssetFileType(assetURL))
		{
		case 1:
			gameObject = GetPrefab(assetURL);
			break;
		case 2:
		{
			Sprite spriteAsset = GetSpriteAsset(assetURL);
			if (spriteAsset != null)
			{
				gameObject = GetPrefab("_base/SpriteRendererBase");
				gameObject.GetComponent<SpriteRenderer>().sprite = spriteAsset;
			}
			break;
		}
		}
		if (gameObject == null)
		{
			return null;
		}
		if (instantiate)
		{
			return UnityEngine.Object.Instantiate(gameObject);
		}
		return gameObject;
	}

	public GameObject GetGameObjectAsset(int assetType, string asset, bool instantiate = true)
	{
		if (UrlIsSWF(asset))
		{
			asset.Replace(".swf", ".prefab");
		}
		string path = AssetURL.GetPath(assetType, asset);
		return GetGameObjectAsset(path, instantiate);
	}

	public GameObject GetGameObjectPrefabAsset(int assetType, string asset, bool instantiate = true)
	{
		if (!asset.EndsWith(".prefab"))
		{
			asset += ".prefab";
		}
		string path = AssetURL.GetPath(assetType, asset);
		return GetGameObjectAsset(path, instantiate);
	}

	public Sprite GetSpriteAsset(string url)
	{
		if (!Util.IsEmptyString(url))
		{
			return GetSprite(url);
		}
		return null;
	}

	public Sprite GetSpriteAsset(string url, out bool isPrefab)
	{
		isPrefab = false;
		if (!Util.IsEmptyString(url))
		{
			Sprite sprite = GetSprite(url, out isPrefab);
			if (!isPrefab)
			{
				return sprite;
			}
		}
		return null;
	}

	public Sprite GetSpriteAsset(int assetType, string asset)
	{
		if (Util.IsEmptyString(asset))
		{
			return null;
		}
		string path = AssetURL.GetPath(assetType, asset);
		return GetSpriteAsset(path);
	}

	public Sprite GetSpriteAsset(int assetType, string asset, out bool isPrefab)
	{
		isPrefab = false;
		if (Util.IsEmptyString(asset))
		{
			return null;
		}
		string path = AssetURL.GetPath(assetType, asset);
		return GetSpriteAsset(path, out isPrefab);
	}

	public GameObject GetSpriteLoaderPlaceholder(GameObject root, int index = 0)
	{
		SpriteLoaderPlaceholder[] componentsInChildren = root.GetComponentsInChildren<SpriteLoaderPlaceholder>();
		if (componentsInChildren != null && componentsInChildren.Length != 0 && componentsInChildren[index] != null)
		{
			return componentsInChildren[index].gameObject;
		}
		return null;
	}

	public AudioClip GetAudioClip(string url)
	{
		if (Util.IsEmptyString(url))
		{
			return null;
		}
		AudioClip asset = GetAsset<AudioClip>("assets/" + url);
		if (asset == null)
		{
			D.LogError("[MISSING] AssetLoader::GetAudioClip " + url + " not found");
		}
		return asset;
	}

	private string GetCleanURL(string url)
	{
		if (url != null)
		{
			return url.Split(new string[6] { ".mp3", ".prefab", ".png", ".jpg", ".swf", ".json" }, StringSplitOptions.None)[0];
		}
		return null;
	}

	public T GetAsset<T>(string path, string url) where T : UnityEngine.Object
	{
		if (path == null)
		{
			return null;
		}
		return GetAsset<T>(path + url);
	}

	public T GetAsset<T>(string url) where T : UnityEngine.Object
	{
		if (Util.IsEmptyString(url))
		{
			return null;
		}
		return Resources.Load<T>(GetCleanURL(url));
	}

	private GameObject GetPrefab(string url)
	{
		if (Util.IsEmptyString(url))
		{
			return null;
		}
		GameObject gameObject = null;
		if (!AppInfo.live)
		{
			gameObject = GetAsset<GameObject>("_testing-dlc/", url);
			if (gameObject != null)
			{
				return gameObject;
			}
		}
		gameObject = SingletonMonoBehaviour<PrefabManager>.instance.GetPrefab(url, null, suppressDebugLog: true);
		if (gameObject == null)
		{
			gameObject = GetAsset<GameObject>("assets/", url);
		}
		if (gameObject == null)
		{
			D.LogWarning("[MISSING] Asset - Prefab - Not Found: " + url, forceLoggly: true);
		}
		return gameObject;
	}

	private Sprite GetSprite(string url)
	{
		if (Util.IsEmptyString(url))
		{
			return null;
		}
		Sprite sprite = null;
		if (!AppInfo.live)
		{
			sprite = GetAsset<Sprite>("_testing-dlc/", url);
			if (sprite != null)
			{
				return sprite;
			}
		}
		sprite = SingletonMonoBehaviour<SpriteManager>.instance.LoadSprite(url, suppressDebugLog: true);
		if (sprite == null)
		{
			sprite = GetAsset<Sprite>("assets/", url);
		}
		if (sprite == null)
		{
			D.LogWarning("[MISSING] Asset - Prefab - Not Found: " + url, forceLoggly: true);
		}
		return sprite;
	}

	private Sprite GetSprite(string url, out bool isPrefab)
	{
		isPrefab = false;
		Sprite sprite = null;
		if (Util.IsEmptyString(url))
		{
			return null;
		}
		if (UrlIsPrefab(url))
		{
			isPrefab = true;
		}
		else
		{
			sprite = SingletonMonoBehaviour<SpriteManager>.instance.LoadSprite(url);
			if (sprite == null)
			{
				sprite = GetAsset<Sprite>("assets/", url);
			}
		}
		if (sprite == null)
		{
			D.LogWarning("[MISSING] Asset - Sprite - Not Found: " + url, forceLoggly: true);
		}
		return sprite;
	}

	public static bool UrlIsSWF(string url)
	{
		if (url == null || url.Length < ".swf".Length)
		{
			return false;
		}
		return url.IndexOf(".swf") >= 0;
	}

	public static bool UrlIsPrefab(string url)
	{
		if (url == null || url.Length < ".prefab".Length)
		{
			return false;
		}
		return url.IndexOf(".prefab") >= 0;
	}

	public Sprite GetButtonSprite(ButtonSquareColor color, ButtonSquareState state)
	{
		if (_squareButtonColors.TryGetValue((int)color + (int)state, out var value))
		{
			return value;
		}
		D.Log("all", $"ButtonSquare{color}{state} Reference not found in default references");
		return null;
	}
}
