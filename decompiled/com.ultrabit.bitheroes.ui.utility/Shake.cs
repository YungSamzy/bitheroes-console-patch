using DG.Tweening;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class Shake : MonoBehaviour
{
	private int times;

	private float[] movements;

	private float startX;

	public void StartShake(float speed = 1f, Vector2? point = null)
	{
		if (!(speed > 2f))
		{
			if (!point.HasValue)
			{
				point = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y);
			}
			float duration = 0.075f / speed;
			startX = point.Value.x;
			movements = new float[5] { 8f, -6f, 4f, -2f, 0f };
			DoShake(movements[0], duration);
		}
	}

	private void DoShake(float movement, float duration)
	{
		times++;
		float currentX = startX;
		float endValue = currentX + movement;
		DOTween.To(() => currentX, delegate(float x)
		{
			currentX = x;
		}, endValue, duration).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			base.transform.localPosition = new Vector3(currentX, base.transform.localPosition.y, base.transform.localPosition.z);
		})
			.OnComplete(delegate
			{
				if (times < movements.Length)
				{
					DoShake(movements[times], duration);
				}
				else
				{
					EndShake();
				}
			});
	}

	private void EndShake()
	{
		base.transform.localPosition = new Vector3(startX, base.transform.localPosition.y, base.transform.localPosition.z);
	}

	private void OnDestroy()
	{
	}
}
