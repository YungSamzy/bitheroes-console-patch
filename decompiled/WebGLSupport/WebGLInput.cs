using System;
using System.Collections;
using System.Collections.Generic;
using AOT;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WebGLSupport;

public class WebGLInput : MonoBehaviour, IComparable<WebGLInput>
{
	private static class WebGLInputTabFocus
	{
		private static List<WebGLInput> inputs = new List<WebGLInput>();

		public static void Add(WebGLInput input)
		{
			inputs.Add(input);
			inputs.Sort();
		}

		public static void Remove(WebGLInput input)
		{
			inputs.Remove(input);
		}

		public static void OnTab(WebGLInput input, int value)
		{
			if (inputs.Count > 1)
			{
				int num = inputs.IndexOf(input);
				num += value;
				if (num < 0)
				{
					num = inputs.Count - 1;
				}
				else if (num >= inputs.Count)
				{
					num = 0;
				}
				inputs[num].input.ActivateInputField();
			}
		}
	}

	private static Dictionary<int, WebGLInput> instances;

	internal int id = -1;

	public IInputField input;

	private bool blurBlock;

	[Tooltip("show input element on canvas. this will make you select text by drag.")]
	public bool showHtmlElement;

	public static string CanvasId { get; set; }

	public int Id => id;

	static WebGLInput()
	{
		instances = new Dictionary<int, WebGLInput>();
		CanvasId = "unityContainer";
		WebGLInputPlugin.WebGLInputInit();
	}

	private IInputField Setup()
	{
		if ((bool)GetComponent<InputField>())
		{
			return new WrappedInputField(GetComponent<InputField>());
		}
		if ((bool)GetComponent<TMP_InputField>())
		{
			return new WrappedTMPInputField(GetComponent<TMP_InputField>());
		}
		throw new Exception("Can not Setup WebGLInput!!");
	}

	private void Awake()
	{
		input = Setup();
		base.enabled = false;
		if (Application.isMobilePlatform)
		{
			base.gameObject.AddComponent<WebGLInputMobile>();
		}
	}

	private RectInt GetElemetRect()
	{
		Rect screenCoordinates = GetScreenCoordinates(input.RectTransform());
		if (showHtmlElement || Application.isMobilePlatform)
		{
			int xMin = (int)screenCoordinates.x;
			int yMin = (int)((float)Screen.height - (screenCoordinates.y + screenCoordinates.height));
			return new RectInt(xMin, yMin, (int)screenCoordinates.width, (int)screenCoordinates.height);
		}
		int xMin2 = (int)screenCoordinates.x;
		int yMin2 = (int)((float)Screen.height - screenCoordinates.y);
		return new RectInt(xMin2, yMin2, (int)screenCoordinates.width, 1);
	}

	public void OnSelect()
	{
		if (id != -1)
		{
			throw new Exception("OnSelect : id != -1");
		}
		RectInt elemetRect = GetElemetRect();
		bool isPassword = input.contentType == ContentType.Password;
		int fontsize = Mathf.Max(14, input.fontSize);
		bool isHidden = !showHtmlElement && !Application.isMobilePlatform;
		id = WebGLInputPlugin.WebGLInputCreate(CanvasId, elemetRect.x, elemetRect.y, elemetRect.width, elemetRect.height, fontsize, input.text, input.placeholder, input.lineType != LineType.SingleLine, isPassword, isHidden, Application.isMobilePlatform);
		instances[id] = this;
		WebGLInputPlugin.WebGLInputEnterSubmit(id, input.lineType != LineType.MultiLineNewline);
		WebGLInputPlugin.WebGLInputOnFocus(id, OnFocus);
		WebGLInputPlugin.WebGLInputOnBlur(id, OnBlur);
		WebGLInputPlugin.WebGLInputOnValueChange(id, OnValueChange);
		WebGLInputPlugin.WebGLInputOnEditEnd(id, OnEditEnd);
		WebGLInputPlugin.WebGLInputTab(id, OnTab);
		WebGLInputPlugin.WebGLInputMaxLength(id, (input.characterLimit > 0) ? input.characterLimit : 524288);
		WebGLInputPlugin.WebGLInputFocus(id);
		if (input.OnFocusSelectAll)
		{
			WebGLInputPlugin.WebGLInputSetSelectionRange(id, 0, input.text.Length);
		}
		WebGLWindow.OnBlurEvent += OnWindowBlur;
	}

	private void OnWindowBlur()
	{
		blurBlock = true;
	}

	private Rect GetScreenCoordinates(RectTransform uiElement)
	{
		Vector3[] array = new Vector3[4];
		uiElement.GetWorldCorners(array);
		Canvas componentInParent = uiElement.GetComponentInParent<Canvas>();
		bool flag = componentInParent.renderMode != RenderMode.ScreenSpaceOverlay;
		if ((bool)componentInParent && flag)
		{
			Camera camera = componentInParent.worldCamera;
			if (!camera)
			{
				camera = Camera.main;
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = camera.WorldToScreenPoint(array[i]);
			}
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue);
		for (int j = 0; j < array.Length; j++)
		{
			vector.x = Mathf.Min(vector.x, array[j].x);
			vector.y = Mathf.Min(vector.y, array[j].y);
			vector2.x = Mathf.Max(vector2.x, array[j].x);
			vector2.y = Mathf.Max(vector2.y, array[j].y);
		}
		return new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
	}

	internal void DeactivateInputField()
	{
		if (instances.ContainsKey(id))
		{
			WebGLInputPlugin.WebGLInputDelete(id);
			input.DeactivateInputField();
			instances.Remove(id);
			id = -1;
			WebGLWindow.OnBlurEvent -= OnWindowBlur;
		}
	}

	[MonoPInvokeCallback(typeof(Action<int>))]
	private static void OnFocus(int id)
	{
	}

	[MonoPInvokeCallback(typeof(Action<int>))]
	private static void OnBlur(int id)
	{
		instances[id].StartCoroutine(Blur(id));
	}

	private static IEnumerator Blur(int id)
	{
		yield return null;
		if (instances.ContainsKey(id))
		{
			bool num = instances[id].blurBlock;
			instances[id].blurBlock = false;
			if (!num)
			{
				instances[id].DeactivateInputField();
			}
		}
	}

	[MonoPInvokeCallback(typeof(Action<int, string>))]
	private static void OnValueChange(int id, string value)
	{
		if (instances.ContainsKey(id))
		{
			WebGLInput webGLInput = instances[id];
			if (!webGLInput.input.ReadOnly)
			{
				webGLInput.input.text = value;
			}
			if (webGLInput.input.contentType == ContentType.Name && string.Compare(webGLInput.input.text, value, ignoreCase: true) == 0)
			{
				value = webGLInput.input.text;
			}
			if (value != webGLInput.input.text)
			{
				int num = WebGLInputPlugin.WebGLInputSelectionStart(id);
				int num2 = WebGLInputPlugin.WebGLInputSelectionEnd(id);
				int num3 = webGLInput.input.text.Length - value.Length;
				WebGLInputPlugin.WebGLInputText(id, webGLInput.input.text);
				WebGLInputPlugin.WebGLInputSetSelectionRange(id, num + num3, num2 + num3);
			}
		}
	}

	[MonoPInvokeCallback(typeof(Action<int, string>))]
	private static void OnEditEnd(int id, string value)
	{
		if (!instances[id].input.ReadOnly)
		{
			instances[id].input.text = value;
		}
	}

	[MonoPInvokeCallback(typeof(Action<int, int>))]
	private static void OnTab(int id, int value)
	{
		WebGLInputTabFocus.OnTab(instances[id], value);
	}

	private void Update()
	{
		if (input == null || !input.isFocused)
		{
			CheckOutFocus();
			return;
		}
		if (!instances.ContainsKey(id))
		{
			if (Application.isMobilePlatform)
			{
				return;
			}
			OnSelect();
		}
		else if (!WebGLInputPlugin.WebGLInputIsFocus(id))
		{
			if (Application.isMobilePlatform)
			{
				return;
			}
			WebGLInputPlugin.WebGLInputFocus(id);
		}
		int num = WebGLInputPlugin.WebGLInputSelectionStart(id);
		int num2 = WebGLInputPlugin.WebGLInputSelectionEnd(id);
		if (WebGLInputPlugin.WebGLInputSelectionDirection(id) == -1)
		{
			input.selectionFocusPosition = num;
			input.selectionAnchorPosition = num2;
		}
		else
		{
			input.selectionFocusPosition = num2;
			input.selectionAnchorPosition = num;
		}
		input.Rebuild();
	}

	private void OnDestroy()
	{
		if (instances.ContainsKey(id))
		{
			DeactivateInputField();
		}
	}

	private void OnEnable()
	{
		WebGLInputTabFocus.Add(this);
	}

	private void OnDisable()
	{
		WebGLInputTabFocus.Remove(this);
	}

	public int CompareTo(WebGLInput other)
	{
		Rect screenCoordinates = GetScreenCoordinates(input.RectTransform());
		Rect screenCoordinates2 = GetScreenCoordinates(other.input.RectTransform());
		int num = screenCoordinates2.y.CompareTo(screenCoordinates.y);
		if (num == 0)
		{
			num = screenCoordinates.x.CompareTo(screenCoordinates2.x);
		}
		return num;
	}

	public void CheckOutFocus()
	{
		if (Application.isMobilePlatform && instances.ContainsKey(id) && !(EventSystem.current.currentSelectedGameObject != null))
		{
			WebGLInputPlugin.WebGLInputForceBlur(id);
		}
	}
}
