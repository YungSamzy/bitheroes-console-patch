using System.Collections;
using System.Globalization;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

public class InputBoxMobileBehaviour : MonoBehaviour
{
	private TMP_InputField inputField;

	public bool tempFocusStatus;

	private bool previousFocusStatus;

	private const float KEYBOARD_UPDATE_TIME = 0.5f;

	private const float COROUTINE_UPDATE_RATE = 0.1f;

	private RectTransform mainWindowRectTransform;

	private RectTransform rectTransform;

	private Vector3 originalAnchoredPosition;

	private bool allowKeyboardUpdate;

	private float updateTime;

	private Coroutine updateCanvasCoroutine;

	private int editorHeight;

	private void Start()
	{
		Object.Destroy(this);
	}

	private IEnumerator dontAllowKeyboardUpdate()
	{
		yield return new WaitForSeconds(0.1f);
		allowKeyboardUpdate = true;
	}

	private bool IsFocused()
	{
		return inputField.isFocused;
	}

	private void Update()
	{
		if (allowKeyboardUpdate && previousFocusStatus != IsFocused())
		{
			D.Log($"Focus Changed from {previousFocusStatus} to {IsFocused()}");
			previousFocusStatus = IsFocused();
			if (updateTime > 0f && updateCanvasCoroutine != null)
			{
				updateTime = 0.5f;
				return;
			}
			updateTime = 0.5f;
			updateCanvasCoroutine = StartCoroutine("UpdateCanvas");
		}
	}

	private IEnumerator UpdateCanvas()
	{
		if (!IsKeyboardVisible())
		{
			mainWindowRectTransform.anchoredPosition = originalAnchoredPosition;
			yield break;
		}
		while (!IsKeyboardAvailable())
		{
			yield return null;
		}
		while (updateTime > 0f)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			if (!IsKeyboardVisible())
			{
				updateTime = 0f;
				mainWindowRectTransform.anchoredPosition = originalAnchoredPosition;
				break;
			}
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			Vector3 position = array[0];
			RectTransform component = GameData.instance.windowGenerator.canvas.GetComponent<RectTransform>();
			Vector3 vector = component.InverseTransformPoint(position);
			float num = component.rect.height * component.pivot.y;
			float num2 = Mathf.Max(component.rect.height * GetKeyboardSizePerc() - (num + vector.y), 0f);
			mainWindowRectTransform.anchoredPosition = new Vector2(mainWindowRectTransform.anchoredPosition.x, mainWindowRectTransform.anchoredPosition.y + num2);
			updateTime = Mathf.Max(updateTime - 0.1f, 0f);
		}
	}

	private bool IsKeyboardAvailable()
	{
		if (IsKeyboardVisible())
		{
			return GetKeyboardSize() > 0;
		}
		return false;
	}

	private bool IsKeyboardVisible()
	{
		return TouchScreenKeyboard.visible;
	}

	private float GetKeyboardSizePerc()
	{
		int keyboardSize = GetKeyboardSize();
		if (keyboardSize > 0)
		{
			return (float)keyboardSize / (float)Screen.height;
		}
		return 0f;
	}

	private int GetKeyboardSize()
	{
		return 0;
	}

	private char OnValidateInput(string text, int charIndex, char addedChar)
	{
		UnicodeCategory unicodeCategory = char.GetUnicodeCategory(addedChar);
		if (unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.Surrogate || unicodeCategory == UnicodeCategory.OtherSymbol)
		{
			return '\0';
		}
		return addedChar;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
