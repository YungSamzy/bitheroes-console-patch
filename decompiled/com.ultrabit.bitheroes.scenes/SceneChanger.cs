using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.scenes;

public class SceneChanger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	public enum SCENES
	{
		MAIN,
		FISHING
	}

	public SCENES goToScene;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		switch (goToScene)
		{
		case SCENES.MAIN:
			SceneManager.LoadScene("Main");
			break;
		case SCENES.FISHING:
			SceneManager.LoadScene("Fishing");
			break;
		}
	}
}
