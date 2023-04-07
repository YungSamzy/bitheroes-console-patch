using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class StarHandler : MonoBehaviour
{
	public void ReplaceStar(Image origin, bool fill)
	{
		origin.sprite = GameData.instance.starComponent.GetStar(fill);
	}
}
