using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterNFTNameChangeWindow : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent NAME_CHANGE = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public TMP_InputField nameTxt;

	public TextMeshProUGUI titleNameTxt;

	public Button changeBtn;

	public Button helpBtn;

	private string _currentName;

	public string currentName => _currentName;

	private bool isTextInputValid
	{
		get
		{
			if (nameTxt.text.Length <= 0)
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
				return false;
			}
			if (!Filter.allow(nameTxt.text) || !Filter.allowedName(nameTxt.text))
			{
				GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_name"));
				return false;
			}
			return true;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		_currentName = "";
		topperTxt.text = Language.GetString("ui_rename");
		titleNameTxt.text = Language.GetString("ui_new_name");
		nameTxt.characterLimit = VariableBook.characterNameLength;
		nameTxt.text = _currentName;
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_change");
		changeBtn.onClick.AddListener(OnChangeBtn);
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		helpBtn.onClick.AddListener(OnHelpBtn);
		nameTxt.onSubmit.AddListener(delegate
		{
			OnNameTxtEnter();
		});
		nameTxt.ActivateInputField();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoChangeName();
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("herotag_help_title"), Util.parseMultiLine(Language.GetString("herotag_changename_help_desc")), base.gameObject);
	}

	public void OnNameTxtValueChanged()
	{
		for (int i = 0; i < nameTxt.text.Length; i++)
		{
			if (!Util.CharacterNameAllowed(nameTxt.text[i]))
			{
				nameTxt.text = nameTxt.text.Remove(i, 1);
				nameTxt.caretPosition = i;
			}
		}
	}

	private void OnNameTxtEnter()
	{
		OnNameTxtValueChanged();
		DoChangeName();
	}

	private void DoChangeName()
	{
		nameTxt.text = Util.RemoveWhiteSpace(nameTxt.text);
		if (isTextInputValid)
		{
			Disable();
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_name_confirm", new string[1] { nameTxt.text }, color: true), null, null, delegate
			{
				SendChanges();
			}, delegate
			{
				Enable();
			});
		}
	}

	private void SendChanges()
	{
		if (_currentName == nameTxt.text)
		{
			base.OnClose();
		}
		else if (isTextInputValid)
		{
			_currentName = nameTxt.text;
			DoCharacterNFTNameChange();
		}
	}

	private void DoCharacterNFTNameChange()
	{
		NAME_CHANGE?.Invoke(_currentName);
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		nameTxt.interactable = true;
		changeBtn.interactable = true;
		helpBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		nameTxt.interactable = false;
		changeBtn.interactable = false;
		helpBtn.interactable = false;
	}
}
