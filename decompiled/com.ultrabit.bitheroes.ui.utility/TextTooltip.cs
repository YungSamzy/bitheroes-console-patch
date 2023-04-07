using System.Collections;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class TextTooltip : MonoBehaviour
{
	public const int ARROW_POSITION_NONE = 0;

	public const int ARROW_POSITION_LEFT = 1;

	public const int ARROW_POSITION_RIGHT = 2;

	public const int ARROW_POSITION_UP = 3;

	public const int ARROW_POSITION_DOWN = 4;

	public TextMeshProUGUI displayTxt;

	public Image border;

	public Image arrowTop;

	public Image arrowLeft;

	public Image arrowRight;

	public Image arrowBottom;

	private string _text;

	private int _arrowPosition;

	private object _target;

	private float _offset;

	private float _width;

	private Image _arrow;

	private RectTransform rectTransform;

	private AsianLanguageFontManager asianLangManager;

	public void LoadDetails(string text, int arrowPosition, object target, float offset, float width)
	{
		_text = text;
		_arrowPosition = arrowPosition;
		_target = target;
		_offset = offset;
		_width = width;
		rectTransform = GetComponent<RectTransform>();
		Vector2? size = null;
		if (_target is Vector2)
		{
			Vector2? vector = _target as Vector2?;
			base.transform.position = new Vector3(vector.Value.x, vector.Value.y, base.transform.position.z);
		}
		if (_target is GameObject)
		{
			GameObject gameObject = _target as GameObject;
			if (gameObject.GetComponent<RectTransform>() != null)
			{
				Vector3 vector2 = gameObject.transform.TransformPoint(gameObject.GetComponent<RectTransform>().rect.center);
				base.transform.position = new Vector3(vector2.x, vector2.y, base.transform.position.z);
			}
			else
			{
				base.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, base.transform.position.z);
			}
			size = gameObject.GetComponent<RectTransform>().sizeDelta;
		}
		displayTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, displayTxt.GetComponent<RectTransform>().sizeDelta.y);
		displayTxt.text = Util.ParseString(_text);
		displayTxt.ForceMeshUpdate();
		GetComponent<CanvasGroup>().alpha = 0f;
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(WaitToFixText(size));
		}
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private IEnumerator WaitToFixText(Vector2? size)
	{
		yield return new WaitForEndOfFrame();
		int num = (int)(10f * Main.SCREEN_SCALE);
		displayTxt.ForceMeshUpdate();
		int num2 = 10;
		int num3 = 10;
		float x = displayTxt.textBounds.size.x + (float)num2;
		float y = displayTxt.textBounds.size.y + (float)num3;
		border.rectTransform.sizeDelta = new Vector2(x, y);
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
		GetComponent<CanvasGroup>().alpha = 1f;
	}
}
