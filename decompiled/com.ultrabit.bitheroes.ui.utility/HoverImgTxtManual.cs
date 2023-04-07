using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class HoverImgTxtManual : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Image[] image;

	private Material[] mat;

	private bool _active = true;

	public TextMeshProUGUI[] text;

	private Shader grayscaleShader;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	private void Awake()
	{
		mat = new Material[image.Length];
		for (int i = 0; i < image.Length; i++)
		{
			mat[i] = image[i].material;
			image[i].material = new Material(mat[i]);
		}
		grayscaleShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/Grayscale");
	}

	public void OnEnter()
	{
		if (!_active)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Util.adjustBrightness(image[i], 1.5f);
			}
		}
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] != null && (bool)text[j].GetComponent<TextBrightness>())
			{
				text[j].GetComponent<TextBrightness>().AddBrightness();
			}
		}
	}

	public void OnExit()
	{
		if (!_active)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Util.adjustBrightness(image[i]);
			}
		}
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] != null && (bool)text[j].GetComponent<TextBrightness>())
			{
				text[j].GetComponent<TextBrightness>().ClearBrightness();
			}
		}
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		if (!_active)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Util.adjustBrightness(image[i], 1.5f);
			}
		}
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] != null && (bool)text[j].GetComponent<TextBrightness>())
			{
				text[j].GetComponent<TextBrightness>().AddBrightness();
			}
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		if (!_active)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Util.adjustBrightness(image[i]);
			}
		}
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] != null && (bool)text[j].GetComponent<TextBrightness>())
			{
				text[j].GetComponent<TextBrightness>().ClearBrightness();
			}
		}
	}

	public void ClearBrightness()
	{
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Util.adjustBrightness(image[i]);
			}
		}
		for (int j = 0; j < text.Length; j++)
		{
			if (text[j] != null && (bool)text[j].GetComponent<TextBrightness>())
			{
				text[j].GetComponent<TextBrightness>().ClearBrightness();
			}
		}
	}

	public void AddGrayscale()
	{
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				image[i].material.shader = grayscaleShader;
			}
		}
	}

	public void RemoveGrayscale()
	{
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				image[i].material.shader = Canvas.GetDefaultCanvasMaterial().shader;
			}
		}
	}

	private void OnDestroy()
	{
		if (image == null)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i].material != null)
			{
				Object.Destroy(image[i].material);
			}
		}
	}
}
