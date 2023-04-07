using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.tutorial;

public class StageTrigger : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent POINTER_CLICK = new UnityEvent();

	[HideInInspector]
	public UnityEvent POINTER_DOWN = new UnityEvent();

	public void OnStageTriggerPointerClick()
	{
		POINTER_CLICK.Invoke();
	}

	public void OnStageTriggerPointerDown()
	{
		POINTER_DOWN.Invoke();
	}
}
