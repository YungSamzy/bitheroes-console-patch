using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class DestroyedEvent : MonoBehaviour
{
	[HideInInspector]
	public UnityCustomEvent DESTROYED = new UnityCustomEvent();

	private void OnDestroy()
	{
		DESTROYED.Invoke(base.gameObject);
	}
}
