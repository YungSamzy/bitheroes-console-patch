using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.core;

public class SceneLoader : MonoBehaviour
{
	public LoadingWindow loadingWindow;

	private IEnumerator Start()
	{
		float timeOffset = 0.01f;
		float progressOffset = 1f;
		float progress = 0f;
		loadingWindow.ShowPercentage(show: false);
		while (progress < 100f)
		{
			progress += progressOffset;
			yield return new WaitForSeconds(timeOffset);
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		SceneManager.LoadScene("Login");
	}
}
