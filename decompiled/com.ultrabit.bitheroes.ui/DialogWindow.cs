using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui;

public class DialogWindow : WindowsMain
{
	public enum TYPE
	{
		TYPE_OK,
		TYPE_YES_NO
	}

	public Button yesBtn;

	public Button noBtn;

	public Button acceptBtn;

	[SerializeField]
	private Button eulaBtn;

	public TextMeshProUGUI titleTxt;

	public TextMeshProUGUI descriptionTxt;

	public TYPE type;

	private GameObject _parent;

	private string _callerLink;

	private UnityAction onConfirmCallback;

	private UnityAction onCancelCallback;

	private object _data;

	public object data => _data;

	public void LoadPromptMessage(string title, string description, GameObject parent = null, string okButtonLabel = null, string cancelButtonLabel = null, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default, UnityAction onConfirmCallback = null, UnityAction onCancelCallback = null, object data = null, bool showCloseBtn = true)
	{
		if (okButtonLabel == null)
		{
			okButtonLabel = Language.GetString("ui_yes");
		}
		if (cancelButtonLabel == null)
		{
			cancelButtonLabel = Language.GetString("ui_no");
		}
		if (onConfirmCallback != null)
		{
			this.onConfirmCallback = onConfirmCallback;
		}
		if (onCancelCallback != null)
		{
			this.onCancelCallback = onCancelCallback;
		}
		_data = data;
		closeBtn.gameObject.SetActive(showCloseBtn);
		LoadMessage(TYPE.TYPE_YES_NO, title, description, parent, ButtonSquareColor.Default, okBtnColor, cancelBtnColor, "", _data);
		yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = okButtonLabel;
		noBtn.GetComponentInChildren<TextMeshProUGUI>().text = cancelButtonLabel;
	}

	public void LoadPromptMessageBig(string title, string description, GameObject parent = null, string okButtonLabel = null, string cancelButtonLabel = null, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default, UnityAction onConfirmCallback = null, UnityAction onCancelCallback = null, object data = null)
	{
		if (okButtonLabel == null)
		{
			okButtonLabel = Language.GetString("ui_yes");
		}
		if (cancelButtonLabel == null)
		{
			cancelButtonLabel = Language.GetString("ui_no");
		}
		if (onConfirmCallback != null)
		{
			this.onConfirmCallback = onConfirmCallback;
		}
		if (onCancelCallback != null)
		{
			this.onCancelCallback = onCancelCallback;
		}
		_data = data;
		LoadMessage(TYPE.TYPE_YES_NO, title, description, parent, ButtonSquareColor.Default, okBtnColor, cancelBtnColor, "", _data, closeTop: false, big: true);
		yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = okButtonLabel;
		noBtn.GetComponentInChildren<TextMeshProUGUI>().text = cancelButtonLabel;
	}

	public void LoadClosablePromptMessage(string title, string description, GameObject parent = null, string okButtonLabel = null, string cancelButtonLabel = null, UnityAction onConfirmCallback = null, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default, UnityAction onCancelCallback = null, object data = null)
	{
		if (okButtonLabel == null)
		{
			okButtonLabel = Language.GetString("ui_yes");
		}
		if (cancelButtonLabel == null)
		{
			cancelButtonLabel = Language.GetString("ui_no");
		}
		if (onConfirmCallback != null)
		{
			this.onConfirmCallback = onConfirmCallback;
		}
		if (onCancelCallback != null)
		{
			this.onCancelCallback = onCancelCallback;
		}
		_data = data;
		LoadMessage(TYPE.TYPE_YES_NO, title, description, parent, ButtonSquareColor.Default, okBtnColor, cancelBtnColor, "", _data, closeTop: true);
		yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = okButtonLabel;
		noBtn.GetComponentInChildren<TextMeshProUGUI>().text = cancelButtonLabel;
	}

	public void LoadConfirmMessage(string title, string description, GameObject parent = null, string buttonLabel = null, ButtonSquareColor btnColor = ButtonSquareColor.Default, UnityAction onConfirmCallback = null, object data = null)
	{
		if (onConfirmCallback != null)
		{
			this.onConfirmCallback = onConfirmCallback;
		}
		_data = data;
		closeBtn.gameObject.SetActive(value: false);
		LoadMessage(TYPE.TYPE_OK, title, description, parent, btnColor, ButtonSquareColor.Default, ButtonSquareColor.Default, "", _data);
		if (buttonLabel != null)
		{
			acceptBtn.GetComponentInChildren<TextMeshProUGUI>().text = buttonLabel;
		}
		else
		{
			acceptBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_close");
		}
	}

	public void LoadMessage(TYPE type, string title, string description, GameObject parent = null, ButtonSquareColor btnColor = ButtonSquareColor.Default, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default, string callerLink = "", object data = null, bool closeTop = false, bool big = false)
	{
		base.Start();
		Disable();
		_parent = parent;
		_callerLink = callerLink;
		_data = data;
		base.SetNextScene = "";
		switch (type)
		{
		case TYPE.TYPE_OK:
			yesBtn.gameObject.SetActive(value: false);
			noBtn.gameObject.SetActive(value: false);
			acceptBtn.gameObject.SetActive(value: true);
			acceptBtn.GetComponent<OverrideUIButtonColor>().SetCustom(btnColor);
			acceptBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_close");
			break;
		case TYPE.TYPE_YES_NO:
			yesBtn.gameObject.SetActive(value: true);
			noBtn.gameObject.SetActive(value: true);
			acceptBtn.gameObject.SetActive(value: false);
			yesBtn.GetComponent<OverrideUIButtonColor>().SetCustom(okBtnColor);
			yesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_yes");
			noBtn.GetComponent<OverrideUIButtonColor>().SetCustom(cancelBtnColor);
			noBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_no");
			break;
		}
		titleTxt.text = title;
		descriptionTxt.enableAutoSizing = true;
		descriptionTxt.text = description;
		descriptionTxt.ForceMeshUpdate();
		if (big)
		{
			panel.GetComponent<RectTransform>().sizeDelta += new Vector2(13.4f, 16.6f);
		}
		ListenForBack(DoNo);
		ListenForForward(DoYes);
		forceAnimation = true;
		CreateWindow();
	}

	public void ShowEulaButton()
	{
		eulaBtn.gameObject.SetActive(GameData.instance.PROJECT.character.toCharacterData().isIMXG0);
		eulaBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("eula");
	}

	public void OnEulaBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("eula"), Language.GetString("eula_notice"), base.gameObject);
	}

	private void DoYes()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
		if (_parent != null && !_callerLink.Equals(""))
		{
			_parent.BroadcastMessage("DialogYesReciever" + _callerLink, SendMessageOptions.DontRequireReceiver);
		}
		if (onConfirmCallback != null)
		{
			onConfirmCallback();
		}
	}

	private void DoNo()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
		if (_parent != null)
		{
			_parent.BroadcastMessage("DialogNoReciever" + _callerLink, SendMessageOptions.DontRequireReceiver);
		}
		if (onCancelCallback != null)
		{
			onCancelCallback();
		}
	}

	public void OnAcceptBtn()
	{
		DoYes();
	}

	public void OnYesBtn()
	{
		DoYes();
	}

	public void OnNoBtn()
	{
		DoNo();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (yesBtn != null)
		{
			yesBtn.interactable = true;
		}
		if (noBtn != null)
		{
			noBtn.interactable = true;
		}
		if (acceptBtn != null)
		{
			acceptBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (yesBtn != null)
		{
			yesBtn.interactable = false;
		}
		if (noBtn != null)
		{
			noBtn.interactable = false;
		}
		if (acceptBtn != null)
		{
			acceptBtn.interactable = false;
		}
	}

	public void SetSortingOrder(int value)
	{
		Canvas component = GetComponent<Canvas>();
		if (component != null)
		{
			component.sortingOrder = value;
		}
	}
}
