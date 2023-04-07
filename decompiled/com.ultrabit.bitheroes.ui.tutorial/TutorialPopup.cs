using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.tutorial;

public class TutorialPopup : MonoBehaviour
{
	public const int ARROW_POSITION_NONE = 0;

	public const int ARROW_POSITION_LEFT = 1;

	public const int ARROW_POSITION_RIGHT = 2;

	public const int ARROW_POSITION_UP = 3;

	public const int ARROW_POSITION_DOWN = 4;

	private readonly Vector2 BUTTON_OFFSET = new Vector2(0f, 6f);

	public TextMeshProUGUI displayTxt;

	public Image arrowTop;

	public Image arrowLeft;

	public Image arrowRight;

	public Image arrowBottom;

	public Image border;

	public Button closeBtn;

	public Transform clickIndicatorPrefab;

	private string _text;

	private int _arrowPosition;

	private object _target;

	private float _offset;

	private bool _indicator;

	private bool _button;

	private bool _glow;

	private int _width;

	private Vector2? _position;

	private Image _arrow;

	private Transform _clickIndicator;

	private RectTransform rectTransform;

	private RectTransform parentRectTransform;

	private CanvasGroup canvasGroup;

	private GlowNoOver _glowComponent;

	public object target => _target;

	public float x
	{
		get
		{
			return base.transform.position.x;
		}
		set
		{
			base.transform.position = new Vector3(value, base.transform.position.y, base.transform.position.z);
		}
	}

	public float y
	{
		get
		{
			return base.transform.position.y;
		}
		set
		{
			base.transform.position = new Vector3(base.transform.position.x, value, base.transform.position.z);
		}
	}

	public void LoadDetails(string text, int arrowPosition = 0, object target = null, float offset = 0f, bool indicator = false, bool button = false, bool glow = true, int width = 700, Vector2? position = null)
	{
		_text = text;
		_arrowPosition = arrowPosition;
		_target = target;
		_offset = offset;
		_indicator = indicator;
		_button = button;
		_glow = glow;
		_width = Mathf.RoundToInt((float)width / border.transform.localScale.x);
		_position = position;
		if (!_button)
		{
			closeBtn.gameObject.SetActive(value: false);
		}
		else
		{
			closeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_got_it");
		}
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		SetValues();
	}

	public void SetParentRectTransform(RectTransform parentRT)
	{
		parentRectTransform = parentRT;
	}

	public void OnCloseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void SetValues()
	{
		Vector2? size = null;
		if (_target is Vector2)
		{
			GameData.instance.main.uiCamera.transform.position = GameData.instance.main.mainCamera.transform.position;
		}
		if (_target is GameObject gameObject && gameObject != null)
		{
			if (gameObject.TryGetComponent<RectTransform>(out var component))
			{
				Vector3 vector = gameObject.transform.TransformPoint(component.rect.center);
				base.transform.position = new Vector3(vector.x, vector.y, 0f);
			}
			else
			{
				Vector3 position = gameObject.transform.position;
				float num = position.y + 140f;
				Vector3 position2 = new Vector3(position.x, num, position.z);
				Vector2 screenPoint = GameData.instance.main.entitiesCamera.WorldToScreenPoint(position2);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPoint, GameData.instance.main.uiCamera, out var localPoint);
				base.transform.localPosition = localPoint;
			}
			if ((bool)gameObject.GetComponent<RectTransform>())
			{
				size = gameObject.GetComponent<RectTransform>().rect.size * 0.5f;
			}
			if (_glow)
			{
				_glowComponent = gameObject.GetComponent<GlowNoOver>();
				if (_glowComponent == null)
				{
					_glowComponent = gameObject.AddComponent<GlowNoOver>();
					_glowComponent.ForceStart(forceTextsComponents: true);
				}
				_glowComponent.StartGlow();
			}
		}
		if (displayTxt != null && displayTxt.TryGetComponent<RectTransform>(out var component2))
		{
			component2.sizeDelta = new Vector2(_width, component2.sizeDelta.y);
			displayTxt.text = Util.ParseString(_text);
			displayTxt.ForceMeshUpdate();
		}
		StartCoroutine(WaitToFixText(size));
	}

	private IEnumerator WaitToFixText(Vector2? size)
	{
		yield return new WaitForEndOfFrame();
		if (_target is Vector2)
		{
			Vector2? vector = _target as Vector2?;
			base.transform.position = new Vector3(vector.Value.x, vector.Value.y, 1f);
			base.transform.localPosition = new Vector3(base.transform.localPosition.x / 2.4f, base.transform.localPosition.y / 2.4f, 0f);
		}
		int num = (int)(10f * Main.SCREEN_SCALE);
		displayTxt.ForceMeshUpdate();
		int num2 = 10;
		int num3 = 10;
		float num4 = displayTxt.textBounds.size.x + (float)num2;
		float num5 = displayTxt.textBounds.size.y + (float)num3;
		border.rectTransform.sizeDelta = new Vector2(num4, num5);
		arrowTop.gameObject.SetActive(_arrowPosition == 3);
		arrowLeft.gameObject.SetActive(_arrowPosition == 1);
		arrowRight.gameObject.SetActive(_arrowPosition == 2);
		arrowBottom.gameObject.SetActive(_arrowPosition == 4);
		switch (_arrowPosition)
		{
		case 1:
			_arrow = arrowLeft;
			if (size.HasValue)
			{
				base.transform.position += new Vector3(size.Value.x / 2f + (float)num, 0f, 0f);
			}
			break;
		case 2:
			_arrow = arrowRight;
			if (size.HasValue)
			{
				base.transform.position -= new Vector3(size.Value.x / 2f + (float)num, 0f, 0f);
			}
			break;
		case 3:
			_arrow = arrowTop;
			if (size.HasValue)
			{
				base.transform.position -= new Vector3(0f, size.Value.y / 2f + (float)num, 0f);
			}
			break;
		case 4:
			_arrow = arrowBottom;
			if (size.HasValue)
			{
				base.transform.position += new Vector3(0f, size.Value.y / 2f + (float)num, 0f);
			}
			break;
		}
		if (_arrow != null)
		{
			rectTransform.localPosition += new Vector3(_arrow.rectTransform.localPosition.x * -1f * border.transform.localScale.x, _arrow.rectTransform.localPosition.y * -1f * border.transform.localScale.y, 0f);
		}
		Vector2 vector2 = rectTransform.localPosition;
		vector2.x *= Main.STAGE_SCALE;
		vector2.y *= Main.STAGE_SCALE;
		float num6 = vector2.x - border.rectTransform.sizeDelta.x * border.transform.localScale.x * Main.STAGE_SCALE / 2f - (float)(num / 2);
		float num7 = vector2.x + border.rectTransform.sizeDelta.x * border.transform.localScale.x * Main.STAGE_SCALE / 2f + (float)(num / 2);
		float num8 = vector2.y + border.rectTransform.sizeDelta.y * border.transform.localScale.y * Main.STAGE_SCALE / 2f + (float)(num / 2);
		float num9 = vector2.y - border.rectTransform.sizeDelta.y * border.transform.localScale.y * Main.STAGE_SCALE / 2f - (float)(num / 2);
		float num10 = 0f;
		float num11 = 0f;
		if (num6 < (float)(Screen.width / -2))
		{
			num10 = (float)(Screen.width / -2) - num6;
		}
		else if (num7 > (float)(Screen.width / 2))
		{
			num10 = (float)(Screen.width / 2) - num7;
		}
		if (num8 > (float)(Screen.height / 2))
		{
			num11 = (float)(Screen.height / 2) - num8;
		}
		else if (num9 < (float)(Screen.height / -2))
		{
			num11 = (float)(Screen.height / -2) - num9;
		}
		if (num10 != 0f)
		{
			rectTransform.anchoredPosition += new Vector2(num10 / Main.STAGE_SCALE, 0f);
			if ((bool)_arrow && (_arrowPosition == 3 || _arrowPosition == 4))
			{
				_arrow.rectTransform.anchoredPosition += new Vector2((0f - num10) / Main.STAGE_SCALE / border.transform.localScale.x, 0f);
			}
		}
		if (num11 != 0f)
		{
			rectTransform.anchoredPosition += new Vector2(0f, num11 / Main.STAGE_SCALE);
			if ((bool)_arrow && (_arrowPosition == 1 || _arrowPosition == 2))
			{
				_arrow.rectTransform.anchoredPosition += new Vector2(0f, (0f - num11) / Main.STAGE_SCALE / border.transform.localScale.y);
			}
		}
		if (closeBtn.gameObject.activeSelf)
		{
			if (_arrowPosition == 4)
			{
				closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, border.rectTransform.sizeDelta.y * closeBtn.transform.localScale.y / 2f + closeBtn.GetComponent<RectTransform>().sizeDelta.y * closeBtn.transform.localScale.y / 2f + BUTTON_OFFSET.y);
			}
			else
			{
				closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, border.rectTransform.sizeDelta.y * closeBtn.transform.localScale.y / -2f - closeBtn.GetComponent<RectTransform>().sizeDelta.y * closeBtn.transform.localScale.y / 2f - BUTTON_OFFSET.y);
			}
		}
		if (_position.HasValue)
		{
			base.transform.position += new Vector3(_position.Value.x * GameData.instance.windowGenerator.canvas.transform.localScale.x, _position.Value.y * GameData.instance.windowGenerator.canvas.transform.localScale.y, base.transform.position.z);
		}
		canvasGroup.alpha = 1f;
		ZoomIn();
	}

	public void ZoomIn()
	{
		Vector2 vector = rectTransform.localScale;
		vector *= (Vector2)new Vector3(0.9f, 0.9f, 1f);
		_ = (Vector2)rectTransform.localScale * (Vector2)new Vector3(1.5f, 1.5f, 1f);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, rectTransform.DOScale(vector, 0.25f).SetEase(Ease.InCubic));
		sequence.Insert(0.25f, rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InCubic));
		sequence.OnComplete(delegate
		{
			OnZoomComplete();
		});
		float initAlpha = 0f;
		DOTween.To(() => initAlpha, delegate(float x)
		{
			initAlpha = x;
		}, 1f, 0.5f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			canvasGroup.alpha = initAlpha;
		});
	}

	private void OnZoomComplete()
	{
		if ((bool)_arrow && _indicator)
		{
			int num = 40;
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GameData.instance.main.uiCamera, _arrow.transform.position);
			switch (_arrowPosition)
			{
			case 3:
				screenPoint.y += num;
				break;
			case 4:
				screenPoint.y -= num;
				break;
			case 1:
				screenPoint.x -= num;
				break;
			case 2:
				screenPoint.x += num;
				break;
			}
			_clickIndicator = Object.Instantiate(clickIndicatorPrefab);
			RectTransformUtility.ScreenPointToWorldPointInRectangle(_arrow.GetComponentInParent<RectTransform>(), screenPoint, GameData.instance.main.mainCamera, out var worldPoint);
			_clickIndicator.position = worldPoint;
		}
	}

	private void OnDestroy()
	{
		if (_glow && _target != null && _glowComponent != null)
		{
			_glowComponent.StopGlowing();
			_glowComponent.active = false;
		}
		if (_clickIndicator != null)
		{
			Object.Destroy(_clickIndicator.gameObject);
		}
	}
}
