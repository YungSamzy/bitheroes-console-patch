using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneNodeStar : MonoBehaviour
{
	public Sprite starFill;

	public Sprite starEmpty;

	public void Setup(int count)
	{
		count--;
		if (count < 0)
		{
			count = 0;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).GetComponent<Image>().sprite = ((i <= count) ? starFill : starEmpty);
		}
	}
}
