using System;
using System.Collections;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	public string initialSceneName = "Login";

	public Action onDownloadComplete;

	public Action<float> onProgressUpdated;

	public bool DLCEnabled => SingletonMonoBehaviour<EnvironmentManager>.instance.GetConfigValue("EnableAssetBundles").AsBool;

	public void Init()
	{
		try
		{
			if (DLCEnabled)
			{
				initialSceneName = "Main";
				StartCoroutine(Initialize());
			}
			else
			{
				onDownloadComplete();
			}
		}
		catch (NullReferenceException ex)
		{
			D.LogException("all", ex.Message, ex);
			onDownloadComplete();
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (SingletonMonoBehaviour<GameManager>.exists)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		AssetBundleManager.onProgressUpdate += OnProgressUpdated;
	}

	private void OnDisable()
	{
		AssetBundleManager.onProgressUpdate -= OnProgressUpdated;
	}

	private void OnDownloadComplete()
	{
		if (onDownloadComplete != null)
		{
			onDownloadComplete();
		}
	}

	private void OnProgressUpdated(float value)
	{
		if (onProgressUpdated != null)
		{
			onProgressUpdated(value);
		}
	}

	public IEnumerator Initialize()
	{
		yield return StartCoroutine(AppConfigManager.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<SpriteManager>.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<PrefabManager>.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<AssetBundleManager>.instance.PreInitialize());
		yield return StartCoroutine(SingletonMonoBehaviour<AssetManager>.instance.PreInitialize());
		yield return StartCoroutine(SingletonMonoBehaviour<SessionManager>.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<AssetBundleManager>.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<AssetManager>.instance.Initialize());
		yield return StartCoroutine(SingletonMonoBehaviour<ViewManager>.instance.Initialize());
		OnDownloadComplete();
	}

	public IEnumerator HardRestart(bool updateDLC = false)
	{
		yield return false;
	}
}
