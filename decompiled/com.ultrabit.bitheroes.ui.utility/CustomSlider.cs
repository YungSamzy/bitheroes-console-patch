using com.ultrabit.bitheroes.core;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class CustomSlider : Slider
{
	public override void OnPointerUp(PointerEventData eventData)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnPointerUp(eventData);
	}
}
