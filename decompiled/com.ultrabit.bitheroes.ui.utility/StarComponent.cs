using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class StarComponent : MonoBehaviour
{
	public GameObject starEmpty;

	public GameObject starFill;

	public Sprite GetStar(bool fill)
	{
		if (fill)
		{
			return starFill.GetComponent<Image>().sprite;
		}
		return starEmpty.GetComponent<Image>().sprite;
	}
}
