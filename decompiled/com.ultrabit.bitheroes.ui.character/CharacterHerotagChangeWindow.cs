using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterHerotagChangeWindow : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent NAME_CHANGE = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public TMP_InputField nameTxt;

	public TextMeshProUGUI oldNameTxt;

	public TextMeshProUGUI newNameTxt;

	public TextMeshProUGUI titleNameTxt;

	public Button changeBtn;

	public Button helpBtn;

	private string _currentName;

	public string currentName => _currentName;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		_currentName = GameData.instance.PROJECT.character.name;
		topperTxt.text = Language.GetString("ui_herotag_title");
		titleNameTxt.text = Language.GetString("ui_new_name");
		oldNameTxt.text = GameData.instance.PROJECT.character.name + "<color=#9FA9B5>#" + GameData.instance.PROJECT.character.herotag + "</color>";
		newNameTxt.text = GameData.instance.PROJECT.character.name + "<color=#9FA9B5>#" + GameData.instance.PROJECT.character.herotag + "</color>";
		nameTxt.characterLimit = VariableBook.characterNameLength;
		nameTxt.text = _currentName;
		changeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_change");
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		Debug.LogWarning("Check InputText Submit on mobile");
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
		newNameTxt.text = nameTxt.text + "<color=#9FA9B5>#" + GameData.instance.PROJECT.character.herotag + "</font>";
	}

	private void OnNameTxtEnter()
	{
		OnNameTxtValueChanged();
		DoChangeName();
	}

	private void DoChangeName()
	{
		nameTxt.text = Util.RemoveWhiteSpace(nameTxt.text);
		if (nameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
			return;
		}
		if (!Filter.allow(nameTxt.text))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_name"));
			return;
		}
		if (nameTxt.text == GameData.instance.PROJECT.character.name)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("herotag_changename_repeat_error"));
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_namechange_confirm", new string[2]
		{
			GameData.instance.PROJECT.character.name,
			nameTxt.text
		}, color: true), null, null, delegate
		{
			SendChanges();
		});
	}

	private void SendChanges()
	{
		if (_currentName == nameTxt.text)
		{
			base.OnClose();
			return;
		}
		if (nameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_name"));
			return;
		}
		if (!Filter.allow(nameTxt.text) || !Filter.allowedName(nameTxt.text))
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_invalid_name"));
			return;
		}
		_currentName = nameTxt.text;
		OnCharacterHerotagChange();
	}

	private void OnCharacterHerotagChange()
	{
		Disable();
		GameData.instance.main.UpdateLoading(Language.GetString("ui_saving"));
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(58), OnCustomize);
		CharacterDALC.instance.doSaveHerotagName(_currentName);
	}

	private void OnCustomize(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(58), OnCustomize);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			if (sfsob.ContainsKey(ServerConstants.CHARACTER_CHANGENAME_COOLDOWN))
			{
				GameData.instance.PROJECT.character.changenameCooldown = sfsob.GetLong(ServerConstants.CHARACTER_CHANGENAME_COOLDOWN);
			}
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			string utfString = sfsob.GetUtfString("cha2");
			ItemData itemData = ItemData.fromSFSObject(sfsob);
			if (itemData != null)
			{
				GameData.instance.PROJECT.character.removeItem(itemData);
			}
			_currentName = utfString;
			GameData.instance.PROJECT.character.name = utfString;
			NAME_CHANGE.Invoke(null);
		}
		base.OnClose();
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
