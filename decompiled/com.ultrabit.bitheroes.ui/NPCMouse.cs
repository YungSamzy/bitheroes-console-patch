using com.ultrabit.bitheroes.charactermov;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui;

public class NPCMouse : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler, IPointerExitHandler
{
	private NPCBehaviour theParent;

	public NPCBehaviour SetNPCParent
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
		theParent.NPCDialog(eventData.position);
	}
}
