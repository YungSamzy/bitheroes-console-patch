using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.utility;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private TextMeshProUGUI image;

	private Material mat;

	private float brightness = 20f;

	private void Start()
	{
		image = GetComponent<TextMeshProUGUI>();
		if (image == null)
		{
			image = GetComponentInChildren<TextMeshProUGUI>();
			if (image == null)
			{
				base.enabled = false;
				return;
			}
			mat = GetComponentInChildren<TextMeshProUGUI>().material;
			image.material = new Material(mat);
		}
		else
		{
			mat = GetComponent<TextMeshProUGUI>().material;
			image.material = new Material(mat);
		}
	}

	public void ForceInit()
	{
		image = GetComponent<TextMeshProUGUI>();
		if (image == null)
		{
			image = GetComponentInChildren<TextMeshProUGUI>();
			if (image == null)
			{
				base.enabled = false;
				return;
			}
			mat = GetComponentInChildren<TextMeshProUGUI>().material;
			image.material = new Material(mat);
		}
		else
		{
			mat = GetComponent<TextMeshProUGUI>().material;
			image.material = new Material(mat);
		}
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		Debug.Log("MOUSE ENTER");
		Util.adjustTextSaturation(image, -0.3f);
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		Debug.Log("MOUSE EXIT");
		Util.adjustTextSaturation(image, 0.3f);
	}

	private void OnDestroy()
	{
		if (image != null)
		{
			Object.Destroy(image.material);
		}
	}
}
