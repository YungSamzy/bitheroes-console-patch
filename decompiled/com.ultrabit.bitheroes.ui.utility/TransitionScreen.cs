using System;
using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.ui.utility;

public class TransitionScreen : WindowsMain
{
	public TextMeshProUGUI loadingTxt;

	public TextMeshProUGUI percTxt;

	public Animator anim;

	private string _sceneToLoad;

	private string _sceneToDestroy;

	private bool _unloadFirst;

	private UnityAction _completeAction;

	private UnityAction _toggleAction;

	private float _loadPerc;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public virtual void LoadDetails(string sceneName, string currentSceneName, UnityAction completeAction = null, UnityAction toggleAction = null, bool unloadFirst = true)
	{
		_sceneToLoad = sceneName;
		_sceneToDestroy = ((sceneName == currentSceneName) ? null : currentSceneName);
		_completeAction = completeAction;
		_toggleAction = toggleAction;
		_unloadFirst = unloadFirst;
		_loadPerc = 0f;
		loadingTxt.text = Language.GetString("ui_loading");
		ToggleLoadingVisibility(visible: false);
		CreateWindow(closeWord: false, "", scroll: false);
	}

	private void ToggleLoadingVisibility(bool visible)
	{
		if (percTxt != null)
		{
			percTxt.gameObject.SetActive(visible);
		}
		if (loadingTxt != null)
		{
			loadingTxt.gameObject.SetActive(visible);
		}
	}

	private void UpdateProgress()
	{
		if (percTxt != null)
		{
			percTxt.gameObject.SetActive(value: false);
			percTxt.text = $"{GetCurrentProgress()}%";
			percTxt.gameObject.SetActive(value: true);
		}
	}

	public void IncreaseProgress(float perc)
	{
		if (this != null && base.gameObject != null)
		{
			_loadPerc += perc;
			UpdateProgress();
		}
	}

	private float GetCurrentProgress()
	{
		if (_loadPerc > 100f)
		{
			_loadPerc = 100f;
		}
		return _loadPerc;
	}

	private IEnumerator CleanScene(string name, UnityAction callback)
	{
		Scene sceneByName = SceneManager.GetSceneByName(name);
		D.Log("___CleanScreen " + sceneByName.name);
		if (sceneByName.name == "player_armory")
		{
			GameData.instance.PROJECT.CleanLastArmoryCharacterLoaded();
		}
		GameObject[] rootObjects = sceneByName.GetRootGameObjects();
		if (rootObjects != null)
		{
			int count = rootObjects.Length;
			int destroyCount = 0;
			_loadPerc = 0f;
			for (int i = 0; i < count; i++)
			{
				UnityEngine.Object.Destroy(rootObjects[i]);
				destroyCount++;
				_loadPerc = Mathf.Round((float)destroyCount * 100f / (float)count);
				UpdateProgress();
				yield return new WaitForEndOfFrame();
			}
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		yield return new WaitForEndOfFrame();
		callback();
	}

	public void OnToggle()
	{
		anim.speed = 0f;
		ToggleLoadingVisibility(visible: true);
		if (_toggleAction != null)
		{
			_toggleAction();
		}
		if (_unloadFirst)
		{
			UnloadFirstFlow();
		}
		else
		{
			LoadFirstFlow();
		}
	}

	public void OnComplete()
	{
		base.OnClose();
		if (this != null && base.gameObject != null)
		{
			base.DoDestroy();
		}
	}

	private void UnloadFirstFlow()
	{
		if (_sceneToDestroy != null)
		{
			StartCoroutine(CleanScene(_sceneToDestroy, InvokeAndResumeAnimation));
		}
		else
		{
			InvokeAndResumeAnimation();
		}
	}

	private void InvokeAndResumeAnimation()
	{
		if (_completeAction != null && _completeAction.Target != null)
		{
			_completeAction();
		}
		if (anim != null)
		{
			anim.speed = 1f;
		}
	}

	private void LoadFirstFlow()
	{
		if (_sceneToDestroy != null)
		{
			SceneManager.GetSceneByName(_sceneToDestroy);
			if (SceneManager.GetSceneByName(_sceneToDestroy).IsValid())
			{
				StartCoroutine(CleanScene(_sceneToDestroy, InvokeAndResumeAnimation));
				return;
			}
		}
		InvokeAndResumeAnimation();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
