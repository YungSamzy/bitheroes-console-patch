using UnityEngine;

namespace WebGLSupport;

public interface IInputField
{
	ContentType contentType { get; }

	LineType lineType { get; }

	int fontSize { get; }

	string text { get; set; }

	string placeholder { get; }

	int characterLimit { get; }

	int caretPosition { get; }

	bool isFocused { get; }

	int selectionFocusPosition { get; set; }

	int selectionAnchorPosition { get; set; }

	bool ReadOnly { get; }

	bool OnFocusSelectAll { get; }

	RectTransform RectTransform();

	void ActivateInputField();

	void DeactivateInputField();

	void Rebuild();
}
