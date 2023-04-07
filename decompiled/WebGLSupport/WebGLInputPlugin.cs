using System;

namespace WebGLSupport;

internal class WebGLInputPlugin
{
	public static void WebGLInputInit()
	{
	}

	public static int WebGLInputCreate(string canvasId, int x, int y, int width, int height, int fontsize, string text, string placeholder, bool isMultiLine, bool isPassword, bool isHidden, bool isMobile)
	{
		return 0;
	}

	public static void WebGLInputEnterSubmit(int id, bool flag)
	{
	}

	public static void WebGLInputTab(int id, Action<int, int> cb)
	{
	}

	public static void WebGLInputFocus(int id)
	{
	}

	public static void WebGLInputOnFocus(int id, Action<int> cb)
	{
	}

	public static void WebGLInputOnBlur(int id, Action<int> cb)
	{
	}

	public static void WebGLInputOnValueChange(int id, Action<int, string> cb)
	{
	}

	public static void WebGLInputOnEditEnd(int id, Action<int, string> cb)
	{
	}

	public static int WebGLInputSelectionStart(int id)
	{
		return 0;
	}

	public static int WebGLInputSelectionEnd(int id)
	{
		return 0;
	}

	public static int WebGLInputSelectionDirection(int id)
	{
		return 0;
	}

	public static void WebGLInputSetSelectionRange(int id, int start, int end)
	{
	}

	public static void WebGLInputMaxLength(int id, int maxlength)
	{
	}

	public static void WebGLInputText(int id, string text)
	{
	}

	public static bool WebGLInputIsFocus(int id)
	{
		return false;
	}

	public static void WebGLInputDelete(int id)
	{
	}

	public static void WebGLInputForceBlur(int id)
	{
	}
}
