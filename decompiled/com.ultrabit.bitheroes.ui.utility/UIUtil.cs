using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class UIUtil
{
	public static void LockButton(Button button, bool locked)
	{
		Color color = button.GetComponent<Image>().color;
		color.a = (locked ? 0.5f : 1f);
		button.GetComponent<Image>().color = color;
		button.interactable = !locked;
		Image[] componentsInChildren = button.gameObject.GetComponentsInChildren<Image>();
		if (componentsInChildren.Length != 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Color color2 = componentsInChildren[i].color;
				color2.a = (locked ? 0.5f : 1f);
				componentsInChildren[i].color = color2;
			}
		}
	}

	public static void LockImage(Image image, bool locked)
	{
		if (image != null)
		{
			Color color = image.color;
			color.a = (locked ? 0.5f : 1f);
			image.color = color;
			image.raycastTarget = !locked;
		}
	}
}
