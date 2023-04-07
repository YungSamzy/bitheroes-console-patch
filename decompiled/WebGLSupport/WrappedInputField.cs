using UnityEngine;
using UnityEngine.UI;
using WebGLSupport.Detail;

namespace WebGLSupport;

internal class WrappedInputField : IInputField
{
	private InputField input;

	private RebuildChecker checker;

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
			Text component = input.placeholder.GetComponent<Text>();
			if (!component)
			{
				return "";
			}
			return component.text;
		}
	}

	public int fontSize => input.textComponent.fontSize;

	public ContentType contentType => (ContentType)input.contentType;

	public LineType lineType => (LineType)input.lineType;

	public int characterLimit => input.characterLimit;

	public int caretPosition => input.caretPosition;

	public bool isFocused => input.isFocused;

	public int selectionFocusPosition
	{
		get
		{
			return input.selectionFocusPosition;
		}
		set
		{
			input.selectionFocusPosition = value;
		}
	}

	public int selectionAnchorPosition
	{
		get
		{
			return input.selectionAnchorPosition;
		}
		set
		{
			input.selectionAnchorPosition = value;
		}
	}

	public bool OnFocusSelectAll => true;

	public WrappedInputField(InputField input)
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
			input.textComponent.SetAllDirty();
			input.Rebuild(CanvasUpdate.LatePreRender);
		}
	}
}
