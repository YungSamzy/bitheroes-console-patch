using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class DotsHandler : MonoBehaviour
{
	public void ReplaceDot(Image origin, int index)
	{
		origin.sprite = GameData.instance.dotsComponent.GetDot(index);
	}
}
