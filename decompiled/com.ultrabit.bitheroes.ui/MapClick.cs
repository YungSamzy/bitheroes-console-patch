using com.ultrabit.bitheroes.charactermov;
using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui;

public class MapClick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerClickHandler, IDragHandler
{
	public GameObject player;

	public GameObject target;

	private CharacterMov playerMov;

	private Camera cam;

	private Vector3 worldPos;

	private Vector2 mousePos;

	private bool mapClicked;

	private void Start()
	{
		cam = GameData.instance.main.mainCamera;
		playerMov = player.GetComponent<CharacterMov>();
	}

	private void Update()
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		MoveTo(eventData.position);
		mapClicked = true;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (mapClicked)
		{
			MoveTo(eventData.position);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	private void MoveTo(Vector2 pointerPos)
	{
		if (Physics.Raycast(GameData.instance.main.mainCamera.ScreenPointToRay(pointerPos), out var hitInfo))
		{
			if (hitInfo.collider.tag == "Map")
			{
				playerMov.MoveTo(hitInfo.point.x, hitInfo.point.z, fromInstance: false, null);
			}
			else
			{
				Debug.Log(hitInfo.collider.name + "CLICKED");
			}
		}
	}
}
