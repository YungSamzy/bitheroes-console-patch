using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.friend;

public class FriendInviteWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI playerLabelTxt;

	public TMP_InputField playerNameTxt;

	public Button addBtn;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_player");
		playerLabelTxt.text = Language.GetString("ui_name");
		playerNameTxt.text = "";
		playerNameTxt.characterLimit = VariableBook.characterNameLength;
		Debug.LogWarning("Check InputText Submit on mobile");
		playerNameTxt.onSubmit.AddListener(delegate
		{
			AddFriend();
		});
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_add");
		ListenForBack(OnClose);
		GameData.instance.PROJECT.SetCurrentFriendInviteWindow(this);
		CreateWindow();
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

	public void OnAddBtn(string args = null)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AddFriend();
	}

	private void AddFriend()
	{
		if (playerNameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_name"));
			return;
		}
		playerNameTxt.DeactivateInputField();
		GameData.instance.PROJECT.DoSendRequestByName(playerNameTxt.text);
	}

	public override void DoDestroy()
	{
		playerNameTxt.onSubmit.RemoveListener(delegate
		{
			AddFriend();
		});
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
		playerNameTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
		playerNameTxt.interactable = false;
	}
}
