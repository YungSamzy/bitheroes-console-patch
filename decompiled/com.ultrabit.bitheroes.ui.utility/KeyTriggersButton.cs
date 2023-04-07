using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class KeyTriggersButton : MonoBehaviour
{
	private Button _button;

	private KeyCode _key;

	private bool _set;

	private TMP_InputField _requiredInputText;

	public KeyCode key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = value;
			_set = true;
		}
	}

	public TMP_InputField requiredInputText
	{
		get
		{
			return _requiredInputText;
		}
		set
		{
			_requiredInputText = value;
		}
	}

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	private void Update()
	{
		if (!_set || !Input.GetKeyUp(_key))
		{
			return;
		}
		if (requiredInputText != null)
		{
			if (requiredInputText.isFocused && _button.interactable && _button.enabled)
			{
				_button.onClick.Invoke();
				Debug.Log(_button.name + " was trigger by " + _key.ToString() + "requiring " + _requiredInputText.name + " to be focused/selected");
			}
		}
		else if (_button.interactable && _button.enabled)
		{
			_button.onClick.Invoke();
			Debug.Log(_button.name + " was trigger by " + _key);
		}
	}
}
