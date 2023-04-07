using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminLoginWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI passwordNameTxt;

	public Button loginBtn;

	public TMP_InputField passwordTxt;

	private bool firstTime = true;

	public override void Start()
	{
		base.Start();
		Disable();
		passwordTxt.text = "";
		passwordTxt.contentType = TMP_InputField.ContentType.Password;
		topperTxt.text = Language.GetString("ui_admin");
		passwordNameTxt.text = Language.GetString("ui_password");
		loginBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_login");
		passwordTxt.onSubmit.AddListener(DoLogin);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnValueChanged()
	{
		for (int i = 0; i < passwordTxt.text.Length; i++)
		{
			if (!Util.CharacterNameAllowed(passwordTxt.text[i]))
			{
				passwordTxt.text = passwordTxt.text.Remove(i, 1);
				passwordTxt.caretPosition = i;
			}
		}
	}

	public void OnLoginBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoLogin();
	}

	private void DoLogin(string args = null)
	{
		if (passwordTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_password"));
			return;
		}
		EventSystem.current.SetSelectedGameObject(null);
		Disable();
		GameData.instance.main.UpdateLoading(Language.GetString("ui_logging_in"));
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnLogin);
		AdminDALC.instance.doLogin(ServerExtension.instance.GenerateHash(passwordTxt.text));
	}

	private void OnLogin(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.UpdateLoading();
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnLogin);
		EventSystem.current.SetSelectedGameObject(null);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GameData.instance.SAVE_STATE.adminPassword = passwordTxt.text;
		GameData.instance.PROJECT.character.adminLoggedIn = true;
		GameData.instance.windowGenerator.NewAdminWindow();
		base.OnClose();
	}

	public override void DoDestroy()
	{
		passwordTxt.onSubmit.RemoveListener(DoLogin);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		loginBtn.interactable = true;
		passwordTxt.interactable = true;
		if (firstTime)
		{
			firstTime = false;
			string adminPassword = GameData.instance.SAVE_STATE.adminPassword;
			if (adminPassword.Length > 0)
			{
				passwordTxt.text = adminPassword;
				StartCoroutine(CheckAutoLogin());
			}
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		loginBtn.interactable = false;
		passwordTxt.interactable = false;
	}

	private IEnumerator CheckAutoLogin()
	{
		yield return new WaitForEndOfFrame();
		if (passwordTxt.text.Length > 0)
		{
			DoLogin();
		}
	}
}
