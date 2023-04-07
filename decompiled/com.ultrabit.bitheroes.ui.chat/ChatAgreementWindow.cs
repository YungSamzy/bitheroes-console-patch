using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatAgreementWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI contentTxt;

	public Button agreeBtn;

	public Button declineBtn;

	public Toggle agreeCheckBox;

	public TextContainer txtcont;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("chat_tos_name");
		agreeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_agree");
		declineBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		contentTxt.text = Util.parseMultiLine(Language.GetString("chat_tos"));
		agreeCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("chat_tos_agree_checkbox");
		agreeCheckBox.isOn = false;
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnAgreeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (!agreeCheckBox.isOn)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("chat_tos_agree_checkbox_confirm"));
			return;
		}
		GameData.instance.SAVE_STATE.chatTosVerified = true;
		GameData.instance.PROJECT.character.chatEnabled = true;
		CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		base.OnClose();
	}

	public void OnDeclineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		txtcont.width = contentTxt.GetPixelAdjustedRect().width;
		txtcont.height = contentTxt.GetPreferredValues().y;
		ActivateCloseBtn();
		agreeBtn.interactable = true;
		declineBtn.interactable = true;
		agreeCheckBox.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		agreeBtn.interactable = false;
		declineBtn.interactable = false;
		agreeCheckBox.interactable = false;
	}
}
