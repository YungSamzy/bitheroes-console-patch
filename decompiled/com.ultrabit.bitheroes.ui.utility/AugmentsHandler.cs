using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class AugmentsHandler : MonoBehaviour
{
	public void ReplaceAugmentBG(Image origin, int type)
	{
		origin.sprite = GameData.instance.augmentsComponent.GetAugmentBG(type);
		switch (type)
		{
		case 0:
		case 1:
		case 3:
		case 4:
			origin.rectTransform.localScale = new Vector3(1f, 1f, 1f);
			break;
		case 2:
		case 5:
			origin.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
			break;
		}
	}

	public void ReplaceAugmentColor(Image origin, int type)
	{
		origin.sprite = GameData.instance.augmentsComponent.GetAugmentColor(type);
		switch (type)
		{
		case 0:
		case 1:
		case 3:
		case 4:
			origin.rectTransform.localScale = new Vector3(1f, 1f, 1f);
			break;
		case 2:
		case 5:
			origin.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
			break;
		}
	}
}
