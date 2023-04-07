using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.utility;

public class LogoTween : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent COMPLETE = new UnityEvent();

	private float _scale;

	public virtual void LoadDetails(float scale = 1f, bool center = true)
	{
		_scale = scale;
		if (center)
		{
			Center();
		}
		UpdateScale();
	}

	public void UpdateScale()
	{
		base.transform.localScale = new Vector3(Main.BORDER_SCALE * _scale, Main.BORDER_SCALE * _scale, 1f);
	}

	private void Center()
	{
		base.transform.position = (GameData.instance.main.mainCamera.transform.position -= new Vector3(0f, 0f, 50f));
	}

	public void OnAnimationComplete()
	{
		COMPLETE.Invoke();
	}
}
