using com.ultrabit.bitheroes.model.application;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.game;

public class GameLogo2022 : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent COMPLETE = new UnityEvent();

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SpriteRenderer _background;

	public void LoadDetails()
	{
		if (AppInfo.TESTING)
		{
			_animator.speed = 1f / 3f;
		}
		_background.transform.localScale = new Vector3((float)Screen.width / Main.DEFAULT_BOUNDS.width, (float)Screen.height / Main.DEFAULT_BOUNDS.height, 1f);
	}

	public void OnComplete()
	{
		COMPLETE?.Invoke();
	}

	private void OnDestroy()
	{
		COMPLETE.RemoveAllListeners();
	}
}
