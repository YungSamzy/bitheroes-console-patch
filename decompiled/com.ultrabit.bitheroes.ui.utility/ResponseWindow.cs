using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class ResponseWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Button yesBtn;

	public Button noBtn;

	private bool _flipped;

	private UnityAction _onConfirmCallback;

	private UnityAction _onCancelCallback;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string name, string desc, string yesText = null, string noText = null, bool flipped = false, UnityAction onConfirmCallback = null, UnityAction onCancelCallback = null)
	{
		_flipped = flipped;
		if (onConfirmCallback != null)
		{
			_onConfirmCallback = onConfirmCallback;
		}
		if (onCancelCallback != null)
		{
			_onCancelCallback = onCancelCallback;
		}
		if (yesText == null)
		{
			yesText = Language.GetString("ui_yes");
		}
		if (noText == null)
		{
			noText = Language.GetString("ui_no");
		}
		topperTxt.text = name;
		if (!flipped)
		{
			yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
			noBtn.GetComponentInChildren<TextMeshProUGUI>().text = noText;
		}
		else
		{
			yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = noText;
			noBtn.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnYesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_flipped)
		{
			if (_onCancelCallback != null)
			{
				_onCancelCallback();
			}
		}
		else if (_onConfirmCallback != null)
		{
			_onConfirmCallback();
		}
		base.OnClose();
	}

	public void OnNoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_flipped)
		{
			if (_onConfirmCallback != null)
			{
				_onConfirmCallback();
			}
		}
		else if (_onCancelCallback != null)
		{
			_onCancelCallback();
		}
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		yesBtn.interactable = true;
		noBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		yesBtn.interactable = false;
		noBtn.interactable = false;
	}
}
