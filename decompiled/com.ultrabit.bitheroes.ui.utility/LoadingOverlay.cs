using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class LoadingOverlay : MonoBehaviour
{
	private Animator animator;

	public bool hidden => base.gameObject.activeSelf;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		if (base.gameObject.activeInHierarchy)
		{
			animator.Play("LoadingDot");
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
