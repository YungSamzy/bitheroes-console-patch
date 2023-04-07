using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlRoomInviteOtherPanel : MonoBehaviour
{
	public TextMeshProUGUI playerLabelTxt;

	public Image bg;

	public Button inviteBtn;

	public TMP_InputField playerNameTxt;

	private BrawlRoomInviteWindow _inviteWindow;

	private BrawlRoom _room;

	public void LoadDetails(BrawlRoomInviteWindow inviteWindow, BrawlRoom room)
	{
		_inviteWindow = inviteWindow;
		_room = room;
		playerLabelTxt.text = Language.GetString("ui_name");
		playerNameTxt.text = "";
		playerNameTxt.characterLimit = VariableBook.characterNameLength;
		inviteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invite");
		playerNameTxt.onSubmit.AddListener(delegate
		{
			DoInviteSend();
		});
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

	public void OnInviteBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoInviteSend();
	}

	public void DoInviteSend()
	{
		if (playerNameTxt.text.Length <= 0)
		{
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), Language.GetString("error_blank_name"));
		}
		else
		{
			_inviteWindow.DoInvite(0, playerNameTxt.text);
		}
	}

	public void Show()
	{
		playerLabelTxt.gameObject.SetActive(value: true);
		playerNameTxt.gameObject.SetActive(value: true);
		bg.gameObject.SetActive(value: true);
		inviteBtn.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		playerLabelTxt.gameObject.SetActive(value: false);
		playerNameTxt.gameObject.SetActive(value: false);
		bg.gameObject.SetActive(value: false);
		inviteBtn.gameObject.SetActive(value: false);
	}

	private void OnDestroy()
	{
		playerNameTxt.onSubmit.RemoveListener(delegate
		{
			DoInviteSend();
		});
	}

	public void DoEnable()
	{
		inviteBtn.interactable = true;
		playerNameTxt.interactable = true;
	}

	public void DoDisable()
	{
		inviteBtn.interactable = false;
		playerNameTxt.interactable = false;
	}
}
