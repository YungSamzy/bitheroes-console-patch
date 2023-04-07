using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.game;

public class GameTextSprite : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	public GameObject textTooltipPrefab;

	private static int WEB_DELAY = 100;

	private static int MOBILE_DELAY = 500;

	private string _text;

	private bool _click;

	private int _arrowPosition;

	private GameObject _target;

	private float _offset;

	private float _width;

	private IEnumerator _timer;

	private bool _active;

	private bool _onEnter;

	private bool _onExit;

	private bool _onClick;

	private bool _onDown;

	private bool _onUp;

	private GameObject _tooltip;

	public bool active => _active;

	public void LoadGameTextSprite(string text = null, bool click = false, int arrowPosition = 0, GameObject target = null, float offset = 0f, float width = 200f)
	{
		SetTooltipText(text);
		_click = click;
		_arrowPosition = arrowPosition;
		_target = target;
		_offset = offset;
		_width = width;
		if (!AppInfo.IsMobile())
		{
			_onEnter = true;
			_onExit = true;
		}
		else if (click)
		{
			_onClick = true;
		}
		else
		{
			_onDown = true;
		}
		if (_tooltip == null)
		{
			_tooltip = Object.Instantiate(textTooltipPrefab, GameData.instance.windowGenerator.canvas.transform);
			Main.CONTAINER.AddToLayer(_tooltip, 11, front: true, center: false, resize: false);
			SetActive(active: false);
		}
	}

	private void Update()
	{
		if (active && Input.touches.Length != 0 && Input.touches[0].phase == TouchPhase.Began)
		{
			SetActive(active: false);
		}
		if (_onUp && Input.touches.Length != 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled))
		{
			OnPointerUp(null);
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (_onEnter && _text != null && _text.Length > 0)
		{
			if (WEB_DELAY > 0)
			{
				StartTimer(WEB_DELAY);
			}
			else
			{
				ShowTooltip();
			}
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		if (_onExit)
		{
			StopTimer();
			SetActive(active: false);
		}
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (_onClick)
		{
			ShowTooltip();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (_onDown && _text != null && _text.Length > 0)
		{
			if (MOBILE_DELAY > 0)
			{
				StartTimer(MOBILE_DELAY);
				_onDown = false;
				_onUp = true;
			}
			else
			{
				ShowTooltip();
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (_onUp)
		{
			StopTimer();
			SetActive(active: false);
			_onDown = true;
			_onUp = false;
		}
	}

	public void StartTimer(float delay)
	{
		StopTimer();
		_timer = OnTimer(delay);
		StartCoroutine(_timer);
	}

	public void StopTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	private IEnumerator OnTimer(float delay)
	{
		yield return new WaitForSeconds(delay / 1000f);
		if (AppInfo.IsMobile())
		{
			_onDown = true;
			_onUp = false;
		}
		ShowTooltip();
	}

	private void ShowTooltip()
	{
		SetActive(active: true);
		_tooltip.GetComponent<TextTooltip>().LoadDetails(_text, _arrowPosition, _target, _offset, _width);
	}

	public void SetActive(bool active)
	{
		_active = active;
		if (_tooltip != null)
		{
			_tooltip.SetActive(active);
		}
	}

	public virtual void OnDestroy()
	{
		if (_tooltip != null)
		{
			Object.Destroy(_tooltip);
		}
	}

	public void SetTooltipText(string text)
	{
		_text = text;
	}
}
