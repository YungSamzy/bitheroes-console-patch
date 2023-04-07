using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class AugmentsComponent : MonoBehaviour
{
	public GameObject[] bgs;

	public GameObject[] colors;

	public Sprite GetAugmentBG(int index)
	{
		switch (index)
		{
		case 1:
		case 4:
			return bgs[0].GetComponent<Image>().sprite;
		case 0:
		case 2:
			return bgs[1].GetComponent<Image>().sprite;
		case 3:
		case 5:
			return bgs[2].GetComponent<Image>().sprite;
		default:
			return bgs[0].GetComponent<Image>().sprite;
		}
	}

	public Sprite GetAugmentColor(int index)
	{
		switch (index)
		{
		case 1:
		case 4:
			return colors[0].GetComponent<Image>().sprite;
		case 0:
		case 2:
			return colors[1].GetComponent<Image>().sprite;
		case 3:
		case 5:
			return colors[2].GetComponent<Image>().sprite;
		default:
			return colors[0].GetComponent<Image>().sprite;
		}
	}
}
