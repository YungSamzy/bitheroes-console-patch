using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingCatchingFish : MonoBehaviour
{
	private const float CAPTURE_START = 0.9f;

	private const float CAPTURE_GAIN = 0.1f;

	private const float DURATION_MIN = 0.25f;

	private FishingItemRef _itemRef;

	private FishingBarRef _barRef;

	private float _w;

	private int _offset;

	private bool _capturing;

	private float _captureMult;

	private RectTransform rectTransform;

	public UnityEvent COMPLETE = new UnityEvent();

	public void LoadDetails(FishingItemRef itemRef, float w)
	{
		_itemRef = itemRef;
		_barRef = itemRef.barRef;
		_w = w;
		rectTransform = GetComponent<RectTransform>();
		StartMovement();
	}

	private void StartMovement()
	{
		if (_capturing && _captureMult <= 0f)
		{
			COMPLETE.Invoke();
			return;
		}
		float num = _itemRef.getRandomDistance() / 3f;
		float num2 = 1f / _itemRef.getRandomSpeed();
		if (_capturing)
		{
			num *= _captureMult;
			num2 /= 3f;
			_captureMult -= 0.1f;
		}
		if (num2 <= 0.25f)
		{
			num2 = 0.25f;
		}
		float num3 = ((!Util.randomBoolean()) ? 1 : (-1));
		if (rectTransform.anchoredPosition.x - num <= 0f)
		{
			num3 = 1f;
		}
		if (rectTransform.anchoredPosition.x + num >= _w)
		{
			num3 = -1f;
		}
		float num4 = rectTransform.anchoredPosition.x + num * num3;
		if (num4 <= 0f)
		{
			num4 = 0f;
		}
		if (num4 >= _w)
		{
			num4 = _w;
		}
		DoTween(num4, num2);
	}

	public void DoTween(float position, float duration)
	{
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		Vector2 endValue = new Vector2(position, anchoredPosition.y);
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.DOAnchorPos(endValue, duration).SetEase(_itemRef.GetEase()).OnComplete(delegate
		{
			OnMovementComplete();
		});
	}

	private void OnMovementComplete()
	{
		StartMovement();
	}

	public void StopMovement()
	{
	}

	public void StartCapture()
	{
		if (!_capturing)
		{
			_capturing = true;
			_captureMult = 0.9f;
			StopMovement();
			StartMovement();
		}
	}
}
