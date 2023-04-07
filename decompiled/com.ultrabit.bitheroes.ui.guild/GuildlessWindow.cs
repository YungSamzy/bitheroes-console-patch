using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.service;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildlessWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button invitesBtn;

	public Button findBtn;

	public Button createBtn;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_guild");
		invitesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invites");
		findBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_find");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_create");
		GameData.instance.PROJECT.PauseDungeon();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnInvitesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildInvitesWindow(base.gameObject);
	}

	public void OnFindBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildApplicationWindow(base.gameObject);
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ServiceRef firstServiceByType = ServiceBook.GetFirstServiceByType(7);
		GameData.instance.windowGenerator.NewGuildCreationWindow(base.gameObject, (firstServiceByType.costCreditsRaw > 0) ? new int[1] : new int[1] { 1 });
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (invitesBtn != null)
		{
			invitesBtn.interactable = state;
		}
		if (findBtn != null)
		{
			findBtn.interactable = state;
		}
		if (createBtn != null)
		{
			createBtn.interactable = state;
		}
	}
}
