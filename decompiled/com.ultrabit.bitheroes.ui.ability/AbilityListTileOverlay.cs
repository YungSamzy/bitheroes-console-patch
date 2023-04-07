using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.ability;

public class AbilityListTileOverlay : MonoBehaviour
{
	public Image notch;

	public Image fill;

	public Image overFill;

	public Sprite[] notches;

	private RectTransform rectTransform;

	private Sprite target;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetTile(int qty)
	{
		notch.gameObject.SetActive(qty > 0);
		fill.gameObject.SetActive(qty > 0);
		overFill.gameObject.SetActive(qty > 4);
		if (qty > 0)
		{
			target = notches[Mathf.Clamp(qty, 1, 4) - 1];
			notch.sprite = target;
		}
		if (qty > 4)
		{
			overFill.fillAmount = (float)(qty - 4) / 4f;
		}
	}

	public void SetAlpha(float alpha)
	{
		notch.color = new Color(notch.color.r, notch.color.g, notch.color.b, alpha);
		fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, alpha);
	}
}
