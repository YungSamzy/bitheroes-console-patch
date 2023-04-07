using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.login;

public class CharacterCreateWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI pEmailTxt;

	public TextMeshProUGUI pPasswordTxt;

	public TextMeshProUGUI pPasswordRepeatTxt;

	public TMP_InputField emailTxt;

	public TMP_InputField passwordTxt;

	public TMP_InputField passwordReTxt;

	public Button createBtn;

	private Main main;

	private bool doOnDestroy;

	public Main SetMain
	{
		get
		{
			return main;
		}
		set
		{
			main = value;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string email)
	{
		emailTxt.text = email;
		D.LogWarning("Check InputText Submit on mobile");
		topperTxt.text = Language.GetString("ui_create");
		pEmailTxt.text = Language.GetString("ui_email");
		pPasswordTxt.text = Language.GetString("ui_password");
		pPasswordRepeatTxt.text = Language.GetString("ui_password_re_enter");
		emailTxt.onSubmit.AddListener(DoCreation);
		passwordTxt.onSubmit.AddListener(DoCreation);
		passwordReTxt.onSubmit.AddListener(DoCreation);
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: true, stayUp: false, 1f, 1f, sound: false);
	}

	public void OnEmailTxtValueChanged()
	{
		for (int i = 0; i < emailTxt.text.Length; i++)
		{
			if (!Util.EmailAllowed(emailTxt.text[i]))
			{
				emailTxt.text = emailTxt.text.Remove(i, 1);
				emailTxt.caretPosition = i;
			}
		}
	}

	public override void OnClose()
	{
		Disable();
		GameData.instance.logInManager.CreateLogin();
		base.transform.SetAsLastSibling();
		base.OnClose();
	}

	public void DoClose()
	{
		Disable();
		base.OnClose();
		doOnDestroy = true;
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoCreation();
	}

	public void DoCreation(string args = null)
	{
		GameData.instance.logInManager.OnCreationAttempt();
		if (emailTxt.text == "" || emailTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_email"));
			GameData.instance.logInManager.OnCreationError();
		}
		else if (!emailTxt.text.Contains("@") || !emailTxt.text.Contains(".") || !Util.emailIsValid(emailTxt.text))
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_invalid_email"));
			GameData.instance.logInManager.OnCreationError();
		}
		else if (passwordTxt.text == "" || passwordTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_password"));
			GameData.instance.logInManager.OnCreationError();
		}
		else if (passwordReTxt.text == "" || passwordReTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_re_enter_password"));
			GameData.instance.logInManager.OnCreationError();
		}
		else if (passwordTxt.text != passwordReTxt.text)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_mismatch_password"));
			GameData.instance.logInManager.OnCreationError();
		}
		else
		{
			GameData.instance.SAVE_STATE.email = emailTxt.text;
			GameData.instance.SAVE_STATE.password = passwordTxt.text;
			main.DoCreateEmailUser(emailTxt.text, passwordTxt.text);
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (passwordTxt.isFocused)
			{
				passwordReTxt.ActivateInputField();
				passwordReTxt.selectionAnchorPosition = 0;
				passwordReTxt.selectionFocusPosition = passwordReTxt.text.Length;
			}
			if (passwordReTxt.isFocused)
			{
				emailTxt.ActivateInputField();
				emailTxt.selectionAnchorPosition = 0;
				emailTxt.selectionFocusPosition = emailTxt.text.Length;
			}
			if (emailTxt.isFocused)
			{
				passwordTxt.ActivateInputField();
				passwordTxt.selectionAnchorPosition = 0;
				passwordTxt.selectionFocusPosition = passwordTxt.text.Length;
			}
		}
	}

	public override void DoDestroy()
	{
		emailTxt.onSubmit.RemoveListener(DoCreation);
		passwordTxt.onSubmit.RemoveListener(DoCreation);
		passwordReTxt.onSubmit.RemoveListener(DoCreation);
		base.DoDestroy();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		emailTxt.enabled = false;
		passwordTxt.enabled = false;
		passwordReTxt.enabled = false;
		createBtn.interactable = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		emailTxt.enabled = true;
		passwordTxt.enabled = true;
		passwordReTxt.enabled = true;
		createBtn.interactable = true;
	}
}
