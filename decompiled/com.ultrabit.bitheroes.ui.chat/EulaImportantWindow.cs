using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class EulaImportantWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI contentTxt;

	public Button agreeBtn;

	public Button declineBtn;

	public Button eulaBtn;

	public Toggle agreeCheckBox;

	public TextContainer txtcont;

	private UnityAction _callback;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_important");
		agreeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
		declineBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cancel");
		eulaBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("eula");
		contentTxt.text = Util.parseMultiLine(Language.GetString("eula_important"));
		agreeCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("eula_dont_show");
		agreeCheckBox.isOn = false;
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void LoadDetails(UnityAction callback = null)
	{
		_callback = callback;
	}

	public void OnAgreeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.SAVE_STATE.eulaVerified = agreeCheckBox.isOn;
		CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		_callback?.Invoke();
		base.OnClose();
	}

	public void OnDeclineBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
	}

	public void OnEulaBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("eula"), Language.GetString("eula_notice"), base.gameObject);
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
