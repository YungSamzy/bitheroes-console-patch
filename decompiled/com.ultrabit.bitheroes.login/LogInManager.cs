using com.ultrabit.bitheroes.core;
using UnityEngine;

namespace com.ultrabit.bitheroes.login;

public class LogInManager : MonoBehaviour
{
	private Transform loginWindow;

	private Transform newUserWindow;

	private Transform userCreationWindow;

	private Main main;

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

	public void Awake()
	{
		GameData.instance.logInManager = this;
	}

	public void CreateLogin()
	{
		newUserWindow = GameData.instance.windowGenerator.NewUserWindow();
		loginWindow = GameData.instance.windowGenerator.NewLogInWindow();
		loginWindow.GetComponent<CharacterLoginEmailWindow>().SetMain = main;
	}

	public void CreateUserCreation()
	{
		CloseLogin(toNextScene: false);
		string email = GetEmail();
		userCreationWindow = GameData.instance.windowGenerator.NewUserCreationWindow(email);
		userCreationWindow.GetComponent<CharacterCreateWindow>().SetMain = main;
	}

	public void OnLoginAttempt()
	{
		loginWindow.GetComponent<CharacterLoginEmailWindow>().loginBtn.enabled = false;
		newUserWindow.GetComponent<CharacterLoginEmailNewUserWindow>().createBtn.enabled = false;
	}

	public void OnLoginError()
	{
		loginWindow.GetComponent<CharacterLoginEmailWindow>().loginBtn.enabled = true;
		newUserWindow.GetComponent<CharacterLoginEmailNewUserWindow>().createBtn.enabled = true;
	}

	public void OnCreationAttempt()
	{
		if (newUserWindow != null)
		{
			newUserWindow.GetComponent<CharacterLoginEmailNewUserWindow>().createBtn.enabled = false;
		}
	}

	public void OnCreationError()
	{
		newUserWindow.GetComponent<CharacterLoginEmailNewUserWindow>().createBtn.enabled = true;
	}

	public string GetEmail()
	{
		return loginWindow.GetComponent<CharacterLoginEmailWindow>().emailTxt.text;
	}

	public void CloseLogin(bool toNextScene)
	{
		loginWindow.GetComponent<CharacterLoginEmailWindow>().DoClose(toNextScene);
		newUserWindow.GetComponent<CharacterLoginEmailNewUserWindow>().DoClose();
	}

	public void CloseUserCreation()
	{
		userCreationWindow.GetComponent<CharacterCreateWindow>().DoClose();
	}
}
