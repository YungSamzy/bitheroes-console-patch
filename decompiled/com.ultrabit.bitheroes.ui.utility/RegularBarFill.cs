using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class RegularBarFill : MonoBehaviour
{
	public float totalWidth;

	private float height;

	public float newWidth;

	private Image image;

	private RectTransform rectTransform;

	private void Awake()
	{
		if (image == null)
		{
			Init();
		}
	}

	public void Init()
	{
		image = GetComponent<Image>();
		rectTransform = image.GetComponent<RectTransform>();
		totalWidth = image.GetComponent<RectTransform>().sizeDelta.x;
		height = image.GetComponent<RectTransform>().sizeDelta.y;
	}

	public void UpdateBar(double currentValue, double maxValue)
	{
		newWidth = 0f;
		if (currentValue < 0.0)
		{
			currentValue = 0.0;
		}
		if (currentValue > maxValue)
		{
			currentValue = maxValue;
		}
		newWidth = (float)(currentValue * (double)totalWidth / maxValue);
		if (rectTransform != null)
		{
			rectTransform.sizeDelta = new Vector2(newWidth, height);
		}
	}

	public void UpdateBar(float currentValue, float maxValue)
	{
		newWidth = 0f;
		if (currentValue < 0f)
		{
			currentValue = 0f;
		}
		if (currentValue > maxValue)
		{
			currentValue = maxValue;
		}
		newWidth = currentValue * totalWidth / maxValue;
		if (rectTransform != null)
		{
			rectTransform.sizeDelta = new Vector2(newWidth, height);
		}
	}

	public void UpdateBarByPerc(float percent)
	{
		rectTransform.sizeDelta = new Vector2(percent * totalWidth, height);
	}
}
