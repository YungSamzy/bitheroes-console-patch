using System;
using AOT;
using UnityEngine;

namespace WebGLSupport;

public static class WebGLWindow
{
	public static bool Focus { get; private set; }

	public static event Action OnFocusEvent;

	public static event Action OnBlurEvent;

	private static void Init()
	{
		Focus = true;
		WebGLWindowPlugin.WebGLWindowOnFocus(OnWindowFocus);
		WebGLWindowPlugin.WebGLWindowOnBlur(OnWindowBlur);
		WebGLWindowPlugin.WebGLWindowInjectFullscreen();
	}

	[MonoPInvokeCallback(typeof(Action))]
	private static void OnWindowFocus()
	{
		Focus = true;
		WebGLWindow.OnFocusEvent();
	}

	[MonoPInvokeCallback(typeof(Action))]
	private static void OnWindowBlur()
	{
		Focus = false;
		WebGLWindow.OnBlurEvent();
	}

	[RuntimeInitializeOnLoadMethod]
	private static void RuntimeInitializeOnLoadMethod()
	{
		Init();
	}

	static WebGLWindow()
	{
		WebGLWindow.OnFocusEvent = delegate
		{
		};
		WebGLWindow.OnBlurEvent = delegate
		{
		};
	}
}
