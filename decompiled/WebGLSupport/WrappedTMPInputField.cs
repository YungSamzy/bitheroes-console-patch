using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebGLSupport.Detail;

namespace WebGLSupport;

internal class WrappedTMPInputField : IInputField
{
	private TMP_InputField input;

	private RebuildChecker checker;

	private Coroutine delayedGraphicRebuild;

	public bool ReadOnly => input.readOnly;

	public string text
	{
		get
		{
			return input.text;
		}
		set
		{
			input.text = value;
		}
	}

	public string placeholder
	{
		get
		{
			if (!input.placeholder)
			{
				return "";
			}
			TMP_Text component = input.placeholder.GetComponent<TMP_Text>();
			if (!component)
			{
				return "";
			}
			return component.text;
		}
	}

	public int fontSize => (int)input.textComponent.fontSize;

	public ContentType contentType => (ContentType)input.contentType;

	public LineType lineType => (LineType)input.lineType;

	public int characterLimit => input.characterLimit;

	public int caretPosition => input.caretPosition;

	public bool isFocused => input.isFocused;

	public int selectionFocusPosition
	{
		get
		{
			return input.selectionStringFocusPosition;
		}
		set
		{
			input.selectionStringFocusPosition = value;
		}
	}

	public int selectionAnchorPosition
	{
		get
		{
			return input.selectionStringAnchorPosition;
		}
		set
		{
			input.selectionStringAnchorPosition = value;
		}
	}

	public bool OnFocusSelectAll => input.onFocusSelectAll;

	public WrappedTMPInputField(TMP_InputField input)
	{
		this.input = input;
		checker = new RebuildChecker(this);
	}

	public RectTransform RectTransform()
	{
		return input.GetComponent<RectTransform>();
	}

	public void ActivateInputField()
	{
		input.ActivateInputField();
	}

	public void DeactivateInputField()
	{
		input.DeactivateInputField();
	}

	public void Rebuild()
	{
		if (checker.NeedRebuild())
		{
			input.textComponent.SetVerticesDirty();
			input.textComponent.SetLayoutDirty();
			input.Rebuild(CanvasUpdate.LatePreRender);
		}
	}

	private bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
	{
		Rect rect = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
		Rect other = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);
		return rect.Overlaps(other);
	}
}
