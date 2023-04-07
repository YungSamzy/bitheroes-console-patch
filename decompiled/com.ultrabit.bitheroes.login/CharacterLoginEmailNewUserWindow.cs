using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.login;

public class CharacterLoginEmailNewUserWindow : WindowsMain
{
	public Button createBtn;

	public TextMeshProUGUI newUserTxt;

	public override void Start()
	{
		base.Start();
		Disable();
		CreateWindow(closeWord: false, "", scroll: true, stayUp: false, 1f, 1f, sound: false);
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_create");
		newUserTxt.text = Language.GetString("ui_new_user");
	}

	public void GoToUserCreation()
	{
		GameData.instance.logInManager.CreateUserCreation();
	}

	public void DoClose()
	{
		base.OnClose();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		createBtn.interactable = false;
		closeBtn.interactable = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		createBtn.interactable = true;
		closeBtn.interactable = false;
	}
}
