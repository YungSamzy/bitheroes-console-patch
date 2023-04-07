using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class HoverImage : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Image image;

	private Material mat;

	public TextMeshProUGUI textToBright;

	private void Start()
	{
		image = GetComponent<Image>();
		if (image == null)
		{
			image = GetComponentInChildren<Image>();
			if (image == null)
			{
				base.enabled = false;
				return;
			}
			mat = GetComponentInChildren<Image>().material;
			image.material = new Material(mat);
		}
		else
		{
			mat = GetComponent<Image>().material;
			image.material = new Material(mat);
		}
	}

	public void ForceInit()
	{
		image = GetComponent<Image>();
		if (image == null)
		{
			image = GetComponentInChildren<Image>();
			if (image == null)
			{
				base.enabled = false;
				return;
			}
			mat = GetComponentInChildren<Image>().material;
			image.material = new Material(mat);
		}
		else
		{
			mat = GetComponent<Image>().material;
			image.material = new Material(mat);
		}
	}

	public void OnEnter()
	{
		Util.adjustBrightness(image, 1.5f);
		if (textToBright != null)
		{
			textToBright.GetComponent<TextBrightness>().AddBrightness();
		}
	}

	public void OnExit()
	{
		Util.adjustBrightness(image);
		if (textToBright != null)
		{
			textToBright.GetComponent<TextBrightness>().ClearBrightness();
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		Util.adjustBrightness(image, 1.5f);
		if (textToBright != null)
		{
			textToBright.GetComponent<TextBrightness>().AddBrightness();
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		Util.adjustBrightness(image);
		if (textToBright != null)
		{
			textToBright.GetComponent<TextBrightness>().ClearBrightness();
		}
	}

	private void OnDestroy()
	{
		if (image != null)
		{
			Object.Destroy(image.material);
		}
	}
}
