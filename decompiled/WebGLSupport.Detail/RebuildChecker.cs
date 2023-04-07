using UnityEngine;

namespace WebGLSupport.Detail;

public class RebuildChecker
{
	private IInputField input;

	private string beforeString;

	private int beforeCaretPosition;

	private int beforeSelectionFocusPosition;

	private int beforeSelectionAnchorPosition;

	public RebuildChecker(IInputField input)
	{
		this.input = input;
	}

	public bool NeedRebuild(bool debug = false)
	{
		bool result = false;
		if (beforeString != input.text)
		{
			if (debug)
			{
				Debug.Log($"beforeString : {beforeString} != {input.text}");
			}
			beforeString = input.text;
			result = true;
		}
		if (beforeCaretPosition != input.caretPosition)
		{
			if (debug)
			{
				Debug.Log($"beforeCaretPosition : {beforeCaretPosition} != {input.caretPosition}");
			}
			beforeCaretPosition = input.caretPosition;
			result = true;
		}
		if (beforeSelectionFocusPosition != input.selectionFocusPosition)
		{
			if (debug)
			{
				Debug.Log($"beforeSelectionFocusPosition : {beforeSelectionFocusPosition} != {input.selectionFocusPosition}");
			}
			beforeSelectionFocusPosition = input.selectionFocusPosition;
			result = true;
		}
		if (beforeSelectionAnchorPosition != input.selectionAnchorPosition)
		{
			if (debug)
			{
				Debug.Log($"beforeSelectionAnchorPosition : {beforeSelectionAnchorPosition} != {input.selectionAnchorPosition}");
			}
			beforeSelectionAnchorPosition = input.selectionAnchorPosition;
			result = true;
		}
		return result;
	}
}
