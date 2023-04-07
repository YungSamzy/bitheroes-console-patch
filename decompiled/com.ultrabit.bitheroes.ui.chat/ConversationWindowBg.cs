using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ConversationWindowBg : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDragHandler
{
	private ConversationWindow _conversationWindow;

	private Image[] image;

	private Material[] mat;

	private RectTransform rectTransform;

	private RectTransform bgRectTransform;

	private bool hovered;

	private float dragAmount = 0.5f;

	private Vector2 touchPosition = Vector2.zero;

	private bool touchInside = true;

	public void LoadDetails(ConversationWindow conversationWindow)
	{
		if (AppInfo.IsMobile())
		{
			base.gameObject.AddComponent<Button>();
		}
		_conversationWindow = conversationWindow;
		image = conversationWindow.panel.GetComponentsInChildren<Image>();
		mat = new Material[image.Length];
		for (int i = 0; i < image.Length; i++)
		{
			mat[i] = image[i].material;
			image[i].material = new Material(mat[i]);
		}
		rectTransform = _conversationWindow.GetComponent<RectTransform>();
		bgRectTransform = base.transform.GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData pointer)
	{
		hovered = true;
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i], 1.3f);
		}
	}

	public void OnPointerExit(PointerEventData pointer)
	{
		hovered = false;
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!AppInfo.IsMobile())
		{
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition += eventData.delta;
			float num = bgRectTransform.rect.width + 10f;
			if (AppInfo.IsMobile())
			{
				rectTransform.anchoredPosition += eventData.delta;
			}
			else if (anchoredPosition.x > 0f - num && anchoredPosition.x < num && anchoredPosition.y > 0f - bgRectTransform.rect.height && anchoredPosition.y < bgRectTransform.rect.height)
			{
				rectTransform.anchoredPosition += eventData.delta;
			}
		}
	}

	private void Update()
	{
		if (!AppInfo.IsMobile())
		{
			return;
		}
		if (Input.touchCount >= 1)
		{
			Touch touch = Input.touches[0];
			int fingerId = touch.fingerId;
			if (EventSystem.current.IsPointerOverGameObject(fingerId))
			{
				if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.CompareTag("conversationbg"))
				{
					touchInside = false;
				}
				if (hovered && touchInside)
				{
					touchInside = true;
					rectTransform.anchoredPosition += touch.deltaPosition * dragAmount;
				}
			}
			else
			{
				touchInside = false;
			}
		}
		else
		{
			touchInside = true;
		}
	}

	public void BeforeDestroy()
	{
		if (image == null)
		{
			return;
		}
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i].material != null)
			{
				Object.Destroy(image[i].material);
			}
		}
	}
}
