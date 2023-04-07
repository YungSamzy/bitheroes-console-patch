using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.adgor;

public class AdGorBar : MonoBehaviour
{
	public const float TOTAL_FRAMES = 6f;

	public Sprite[] adgorBarColors = new Sprite[2];

	public Image bgBar;

	public Animator bgAnimator;

	public void GoToAndStopBgBar(int frame)
	{
		bgBar.sprite = adgorBarColors[frame - 1];
	}

	public void GoToAndStop(int frame)
	{
		float num = (float)frame / 6f;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		bgAnimator.speed = 0f;
		bgAnimator.Play("AdgorBar", 0, num);
	}
}
