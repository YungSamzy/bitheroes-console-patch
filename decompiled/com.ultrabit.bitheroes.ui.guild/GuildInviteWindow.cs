using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildInviteWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI playerNameTitleTxt;

	public Button inviteBtn;

	public TMP_InputField playerNameTxt;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_invite");
		playerNameTitleTxt.text = Language.GetString("ui_name");
		playerNameTxt.text = "";
		playerNameTxt.characterLimit = VariableBook.characterNameLength;
		inviteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invite");
		Debug.LogWarning("Check InputText Submit on mobile");
		playerNameTxt.onSubmit.AddListener(delegate
		{
			DoInviteSend();
		});
		ListenForBack(OnClose);
		GameData.instance.PROJECT.SetCurrentGuildInviteWindow(this);
		CreateWindow();
	}

	public void OnInviteBtn(string args = null)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoInviteSend();
	}

	public void OnValueChanged()
	{
		for (int i = 0; i < playerNameTxt.text.Length; i++)
		{
			if (!Util.CharacterNameAllowedWithHashtag(playerNameTxt.text[i]))
			{
				playerNameTxt.text = playerNameTxt.text.Remove(i, 1);
				playerNameTxt.caretPosition = i;
			}
		}
	}

	private void DoInviteSend()
	{
		if (playerNameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_name"));
			return;
		}
		playerNameTxt.DeactivateInputField();
		GameData.instance.PROJECT.DoGuildInviteByName(playerNameTxt.text);
	}

	public override void DoDestroy()
	{
		playerNameTxt.onSubmit.RemoveListener(delegate
		{
			DoInviteSend();
		});
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		inviteBtn.interactable = true;
		playerNameTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		inviteBtn.interactable = false;
		playerNameTxt.interactable = false;
	}
}
