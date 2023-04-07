using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui;

public class InstanceMouse : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler, IPointerExitHandler
{
	private MapMenu theParent;

	public MapMenu SetInstanceParent
	{
		get
		{
			return theParent;
		}
		set
		{
			theParent = value;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		theParent.HighlightedColor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		theParent.NotHighlightedColor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		theParent.InstanceWindow(eventData.position);
	}
}
