using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class CharacterArmoryNameChange : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent ARMORY_NAME_CHANGE = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public TMP_InputField nameTxt;

	public TextMeshProUGUI titleNameTxt;

	public Button changeBtn;

	private string _currentName;

	public string currentName => _currentName;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string currentName = null)
	{
		if (currentName != null)
		{
			SetCurrentName(currentName);
		}
		topperTxt.text = Language.GetString("ui_armory_change_armor_title");
		titleNameTxt.text = Language.GetString("ui_armory_change_armor_name");
		nameTxt.characterLimit = VariableBook.guildNameLength;
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_change_armor_name_button");
		Debug.LogWarning("Check InputText Submit on mobile");
		nameTxt.onSubmit.AddListener(delegate
		{
			OnNameTxtEnter();
		});
		nameTxt.ActivateInputField();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void SetCurrentName(string currentName)
	{
		_currentName = currentName;
		nameTxt.text = currentName;
	}

	public void OnChangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SendChanges();
	}

	private void OnNameTxtEnter()
	{
		SendChanges();
	}

	private void SendChanges()
	{
		if (nameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
			return;
		}
		if (!Filter.allowArmory(nameTxt.text))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_name"));
			return;
		}
		_currentName = nameTxt.text;
		ARMORY_NAME_CHANGE.Invoke(null);
	}

	public void OnNameTxtValueChange()
	{
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		nameTxt.interactable = true;
		changeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		nameTxt.interactable = false;
		changeBtn.interactable = false;
	}
}
