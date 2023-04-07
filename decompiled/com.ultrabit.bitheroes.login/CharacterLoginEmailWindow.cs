using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.login;

public class CharacterLoginEmailWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI pEmailTxt;

	public TextMeshProUGUI pPasswordTxt;

	public TMP_InputField emailTxt;

	public TMP_InputField passwordTxt;

	public Button loginBtn;

	private string _userID;

	private Main main;

	private bool doOnDestroy;

	private string savedEmail = "";

	private string savedPassword = "";

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
	}

	public void LoadDetails()
	{
		Disable();
		D.LogWarning("Check InputText Submit on mobile");
		emailTxt.onSubmit.AddListener(DoLogin);
		passwordTxt.onSubmit.AddListener(DoLogin);
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

	public override void DoStartWindow()
	{
		base.DoStartWindow();
		savedEmail = GameData.instance.SAVE_STATE.email;
		savedPassword = GameData.instance.SAVE_STATE.password;
		emailTxt.text = savedEmail;
		passwordTxt.text = savedPassword;
		loginBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_login");
		topperTxt.text = Language.GetString("ui_login");
		pEmailTxt.text = Language.GetString("ui_email");
		pPasswordTxt.text = Language.GetString("ui_password");
		if (emailTxt.text.Length > 0 && passwordTxt.text.Length > 0)
		{
			DoLogin();
		}
	}

	public void DoLogin(string args = null)
	{
		GameData.instance.logInManager.OnLoginAttempt();
		if (emailTxt.text == "" || emailTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_email"));
			GameData.instance.logInManager.OnLoginError();
		}
		else if (passwordTxt.text == "" || passwordTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_password"));
			GameData.instance.logInManager.OnLoginError();
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
			main.DoLoginEmail(emailTxt.text, passwordTxt.text);
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (passwordTxt.isFocused)
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

	public override void OnClose()
	{
		emailTxt.onSubmit.RemoveListener(DoLogin);
		passwordTxt.onSubmit.RemoveListener(DoLogin);
		base.OnClose();
	}

	public void DoClose(bool goToTown)
	{
		doOnDestroy = goToTown;
		Disable();
		base.OnClose();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		emailTxt.enabled = false;
		passwordTxt.enabled = false;
		loginBtn.interactable = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		emailTxt.enabled = true;
		passwordTxt.enabled = true;
		loginBtn.interactable = true;
	}

	public override void DoDestroy()
	{
		base.DoDestroy();
	}
}
