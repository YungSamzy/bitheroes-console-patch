using System;
using System.Collections;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.game;

public class GameText : MonoBehaviour
{
	private TextMeshProUGUI contentTxt;

	private CutoutMaskTextMeshProUGUI maskTxt;

	private string _content;

	private string _color;

	private int _size;

	private DateTime? _date;

	private DateTime? _customDate;

	private RectTransform rectTransform;

	private GameTextRef _gameTextRef;

	public void LoadDetails(GameTextRef gameTextRef, int sizeModifier = 0, DateTime? customDate = null)
	{
		_gameTextRef = gameTextRef;
		_content = gameTextRef.content;
		_color = gameTextRef.color;
		_size = gameTextRef.size + sizeModifier;
		_date = gameTextRef.date;
		_customDate = customDate;
		contentTxt = GetComponent<TextMeshProUGUI>();
		maskTxt = GetComponentInChildren<CutoutMaskTextMeshProUGUI>();
		rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2((float)gameTextRef.width * (1f + (1f - rectTransform.localScale.x * 3f)), rectTransform.sizeDelta.y);
		TextMeshProUGUI textMeshProUGUI = contentTxt;
		bool enableWordWrapping = (maskTxt.enableWordWrapping = gameTextRef.multiline);
		textMeshProUGUI.enableWordWrapping = enableWordWrapping;
		if (gameTextRef.multiline)
		{
			TextMeshProUGUI textMeshProUGUI2 = contentTxt;
			float lineSpacing = (maskTxt.lineSpacing = -18f);
			textMeshProUGUI2.lineSpacing = lineSpacing;
		}
		TextMeshProUGUI textMeshProUGUI3 = contentTxt;
		enableWordWrapping = (maskTxt.enableAutoSizing = gameTextRef.autoSize != "none");
		textMeshProUGUI3.enableAutoSizing = enableWordWrapping;
		if (gameTextRef.align != "center")
		{
			Debug.LogWarning("Not implemented");
		}
		else
		{
			TextMeshProUGUI textMeshProUGUI4 = contentTxt;
			TextAlignmentOptions alignment = (maskTxt.alignment = TextAlignmentOptions.Center);
			textMeshProUGUI4.alignment = alignment;
		}
		if (_date.HasValue && base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(Timer());
		}
		rectTransform.anchoredPosition = new Vector2(gameTextRef.position.x / 3f, gameTextRef.position.y / -3f);
		if (base.gameObject.activeSelf)
		{
			UpdateText();
		}
	}

	private IEnumerator Timer()
	{
		yield return new WaitForSeconds(1f);
		UpdateText();
		StartCoroutine(Timer());
	}

	private string GetText()
	{
		string text = _content;
		if (_date.HasValue)
		{
			text += Util.TimeFormatClean(GetMilliseconds(_customDate) / 1000f);
		}
		return Util.parseStringSize(Util.colorString(Util.ParseString(text), _color), _size);
	}

	private void UpdateText()
	{
		TextMeshProUGUI textMeshProUGUI = contentTxt;
		string text2 = (maskTxt.text = GetText());
		textMeshProUGUI.text = text2;
		contentTxt.ForceMeshUpdate();
		maskTxt.ForceMeshUpdate();
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(WaitToFixText());
		}
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForEndOfFrame();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (contentTxt.GetParsedText() != null && contentTxt.GetParsedText() != "") ? contentTxt.renderedHeight : 0f);
	}

	public float GetMilliseconds(DateTime? customDate = null)
	{
		if (!_date.HasValue)
		{
			return 0f;
		}
		DateTime dateTime = (customDate.HasValue ? customDate.Value : ServerExtension.instance.GetDate());
		float num = (float)(_date.Value - dateTime).TotalMilliseconds;
		if (num < 0f)
		{
			return 0f;
		}
		return num;
	}
}
