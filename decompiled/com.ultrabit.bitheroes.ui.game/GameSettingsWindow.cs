using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.game;

public class GameSettingsWindow : WindowsMain
{
	public const int TAB_GENERAL = 0;

	public const int TAB_LANGUAGE = 1;

	public const int TAB_SUPPORT = 3;

	public TextMeshProUGUI topperTxt;

	public Button adminBtn;

	public Button testBtn;

	public Button generalBtn;

	public Button languageBtn;

	public Button supportBtn;

	public Button newsBtn;

	public Button ignoresBtn;

	public Button forumsBtn;

	public Button googleBtn;

	public Button logoutBtn;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	public GameSettingsGeneralPanel gameSettingsGeneralPanel;

	public GameSettingsLanguagePanel gameSettingsLanguagePanel;

	public GameSettingsSupportPanel gameSettingsSupportPanel;

	public TextMeshProUGUI termsOfUse;

	public TextMeshProUGUI privacyPolicy;

	public TextMeshProUGUI deleteAccount;

	private int _currentTab = -1;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_settings");
		forumsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_forums");
		if (GameData.instance.PROJECT.character != null)
		{
			newsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_news");
			ignoresBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ignores");
		}
		else
		{
			DisableButton(newsBtn);
			DisableButton(ignoresBtn);
		}
		if (GameData.instance.PROJECT.character != null && AppInfo.allowLogout)
		{
			googleBtn.gameObject.SetActive(value: false);
			logoutBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_logout");
		}
		else
		{
			float x = logoutBtn.GetComponent<RectTransform>().sizeDelta.x / 2f;
			newsBtn.GetComponent<RectTransform>().anchoredPosition += new Vector2(x, 0f);
			ignoresBtn.GetComponent<RectTransform>().anchoredPosition += new Vector2(x, 0f);
			forumsBtn.GetComponent<RectTransform>().anchoredPosition += new Vector2(x, 0f);
			logoutBtn.gameObject.SetActive(value: false);
			googleBtn.gameObject.SetActive(value: false);
		}
		googleBtn.gameObject.SetActive(value: false);
		if (GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.admin)
		{
			adminBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_a");
			testBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_t");
		}
		else
		{
			adminBtn.gameObject.SetActive(value: false);
			testBtn.gameObject.SetActive(value: false);
		}
		GameData.instance.PROJECT.PauseDungeon();
		generalBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_general");
		languageBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_language");
		supportBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_support");
		termsOfUse.text = Language.GetString("ui_terms_of_use");
		privacyPolicy.text = Language.GetString("ui_privacy_policy");
		deleteAccount.text = Language.GetString("ui_delete_account");
		SetTab(0);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnGeneralBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnLanguageBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	public void OnSupportBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(3);
	}

	public void OnAdminBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowAdminTools();
	}

	public void OnTestBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Main.ToggleTesting();
	}

	public void OnNewsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewNewsWindow(base.gameObject);
	}

	public void OnIgnoresBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewChatIgnoresWindow(base.gameObject);
	}

	public void OnForumsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL(VariableBook.gameForumsURL);
	}

	public void OnGoogleBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void OnLogoutBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_logout"), Language.GetString("ui_logout_confirm"), Language.GetString("ui_yes"), Language.GetString("ui_no"), delegate
		{
			GameData.instance.main.Logout(relog: false, reloadXMLfiles: false);
		});
	}

	private void SetTab(int tab)
	{
		switch (tab)
		{
		case 0:
			_currentTab = 0;
			generalBtn.image.color = Color.white;
			generalBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			generalBtn.enabled = false;
			languageBtn.image.color = alpha;
			languageBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			languageBtn.enabled = true;
			supportBtn.image.color = alpha;
			supportBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			supportBtn.enabled = true;
			gameSettingsGeneralPanel.Show();
			gameSettingsLanguagePanel.Hide();
			gameSettingsSupportPanel.Hide();
			break;
		case 1:
			_currentTab = 1;
			languageBtn.image.color = Color.white;
			languageBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			languageBtn.enabled = false;
			generalBtn.image.color = alpha;
			generalBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			generalBtn.enabled = true;
			supportBtn.image.color = alpha;
			supportBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			supportBtn.enabled = true;
			gameSettingsLanguagePanel.Show();
			gameSettingsGeneralPanel.Hide();
			gameSettingsSupportPanel.Hide();
			break;
		case 3:
			_currentTab = 3;
			supportBtn.image.color = Color.white;
			supportBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			supportBtn.enabled = false;
			generalBtn.image.color = alpha;
			generalBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			generalBtn.enabled = true;
			languageBtn.image.color = alpha;
			languageBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			languageBtn.enabled = true;
			gameSettingsSupportPanel.Show();
			gameSettingsLanguagePanel.Hide();
			gameSettingsGeneralPanel.Hide();
			break;
		case 2:
			break;
		}
	}

	private void SaveChanges()
	{
		if (gameSettingsGeneralPanel.SaveChanges())
		{
			CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		}
	}

	private void DisableButton(Button button)
	{
		button.image.color = alpha;
		button.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		button.enabled = false;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		adminBtn.interactable = true;
		testBtn.interactable = true;
		generalBtn.interactable = true;
		languageBtn.interactable = true;
		newsBtn.interactable = true;
		ignoresBtn.interactable = true;
		forumsBtn.interactable = true;
		googleBtn.interactable = true;
		logoutBtn.interactable = true;
		gameSettingsGeneralPanel.DoEnable();
		gameSettingsLanguagePanel.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		adminBtn.interactable = false;
		testBtn.interactable = false;
		generalBtn.interactable = false;
		languageBtn.interactable = false;
		newsBtn.interactable = false;
		ignoresBtn.interactable = false;
		forumsBtn.interactable = false;
		googleBtn.interactable = false;
		logoutBtn.interactable = false;
		gameSettingsGeneralPanel.DoDisable();
		gameSettingsLanguagePanel.DoDisable();
	}

	public override void OnClose()
	{
		SaveChanges();
		base.OnClose();
	}

	public override void DoDestroy()
	{
		gameSettingsGeneralPanel.RemoveListeners();
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}
}
