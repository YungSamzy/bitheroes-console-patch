using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.ability;

public class AbilityTileOverlay : MonoBehaviour
{
	public Image notch;

	public Image fill;

	public Image overFill;

	public Image overlay;

	public Sprite[] notches;

	public Animator[] notchesAnimation;

	private RectTransform rectTransform;

	private int _qty;

	private Sprite target;

	private Color fillColor;

	private Color overFillColor;

	private Color overlayColor;

	private Color fillColorDisabled = new Color(0f, 0.3843137f, 0.4823529f);

	private Color overFillColorDisabled = new Color(0.4032f, 0.3331606f, 0.1060056f);

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		fillColor = fill.color;
		overFillColor = overFill.color;
		overlayColor = overlay.color;
	}

	public void SetTile(int qty)
	{
		_qty = qty;
		for (int i = 0; i < notchesAnimation.Length; i++)
		{
			notchesAnimation[i].gameObject.SetActive(i < qty);
		}
		notch.gameObject.SetActive(qty > 0);
		fill.gameObject.SetActive(qty > 0);
		overlay.gameObject.SetActive(qty > 0);
		overFill.gameObject.SetActive(qty > 4);
		if (qty > 0)
		{
			target = notches[Mathf.Clamp(qty, 1, 4) - 1];
			rectTransform.sizeDelta = new Vector2(target.bounds.size.x * 100f, target.bounds.size.y * 100f);
			notch.sprite = target;
		}
		if (qty > 4)
		{
			overFill.fillAmount = (float)(qty - 4) / 4f;
		}
	}

	public void OverlayVisible(bool visible)
	{
	}

	public void SetColor(bool on)
	{
		if (notchesAnimation != null)
		{
			for (int i = 0; i < notchesAnimation.Length; i++)
			{
				if (notchesAnimation[i] != null && notchesAnimation[i].gameObject != null)
				{
					notchesAnimation[i].gameObject.SetActive(i < _qty);
				}
			}
		}
		if (on)
		{
			if (fill != null)
			{
				fill.color = fillColor;
			}
			if (overFill != null)
			{
				overFill.color = overFillColor;
			}
			if (overlay != null && (bool)overlay.gameObject)
			{
				overlay.gameObject.SetActive(value: false);
			}
			if (notch != null)
			{
				notch.color = Color.white;
			}
			if (overlay != null)
			{
				overlay.color = overlayColor;
			}
			return;
		}
		if (fill != null)
		{
			fill.color = fillColorDisabled;
		}
		if (overFill != null)
		{
			overFill.color = overFillColorDisabled;
		}
		if (_qty > 0)
		{
			if (notch != null && notch.gameObject != null)
			{
				notch.gameObject.SetActive(value: true);
			}
			if (overlay != null && overlay.gameObject != null)
			{
				overlay.gameObject.SetActive(value: false);
			}
			for (int j = 0; j < notchesAnimation.Length; j++)
			{
				if (notchesAnimation[j] != null && notchesAnimation[j].gameObject != null)
				{
					notchesAnimation[j].gameObject.SetActive(value: false);
				}
			}
		}
		if (notch != null)
		{
			notch.color = Color.gray;
		}
		if (overlay != null)
		{
			overlay.color = Color.gray;
		}
	}
}
