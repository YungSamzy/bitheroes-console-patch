using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class DotsComponent : MonoBehaviour
{
	public GameObject[] dots;

	public const int RED_DOT = 0;

	public const int GREEN_DOT = 1;

	public const int BLUE_DOT = 2;

	public Sprite GetDot(int index)
	{
		return dots[index].GetComponent<Image>().sprite;
	}
}
