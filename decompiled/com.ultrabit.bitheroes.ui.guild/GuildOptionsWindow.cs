using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildOptionsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button disbandBtn;

	public Button leaveBtn;

	public Button permissionsBtn;

	public Button applicationsBtn;

	public Toggle openCheckBox;

	private GuildData _guildData;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GuildData guildData)
	{
		_guildData = guildData;
		topperTxt.text = Language.GetString("ui_options");
		disbandBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_disband");
		leaveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_leave");
		permissionsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_permissions");
		applicationsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_applications");
		openCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_open");
		openCheckBox.SetIsOnWithoutNotify(_guildData.open);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_RANK_CHANGE", OnChange);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		DoUpdate();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnChange()
	{
		DoUpdate();
	}

	public void DoUpdate()
	{
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			if (!GameData.instance.PROJECT.character.guildData.hasPermission(5))
			{
				Util.SetButton(permissionsBtn, enabled: false);
			}
			else
			{
				Util.SetButton(permissionsBtn);
			}
			if (!GameData.instance.PROJECT.character.guildData.hasPermission(2))
			{
				Util.SetButton(applicationsBtn, enabled: false);
			}
			else
			{
				Util.SetButton(applicationsBtn);
			}
			if (GameData.instance.PROJECT.character.guildData.rank <= 0)
			{
				Util.SetButton(disbandBtn);
				Util.SetButton(leaveBtn, enabled: false);
				EnableToggle(openCheckBox, enable: true);
			}
			else
			{
				Util.SetButton(disbandBtn, enabled: false);
				Util.SetButton(leaveBtn);
				EnableToggle(openCheckBox, enable: false);
			}
		}
	}

	private void EnableToggle(Toggle toggle, bool enable)
	{
		if (enable)
		{
			toggle.interactable = enable;
			toggle.GetComponentInChildren<TextMeshProUGUI>().color = new Color(toggle.GetComponentInChildren<TextMeshProUGUI>().color.r, toggle.GetComponentInChildren<TextMeshProUGUI>().color.g, toggle.GetComponentInChildren<TextMeshProUGUI>().color.b, 1f);
			toggle.gameObject.GetComponent<HoverImage>().enabled = true;
		}
		else
		{
			toggle.interactable = enable;
			toggle.GetComponentInChildren<TextMeshProUGUI>().color = new Color(toggle.GetComponentInChildren<TextMeshProUGUI>().color.r, toggle.GetComponentInChildren<TextMeshProUGUI>().color.g, toggle.GetComponentInChildren<TextMeshProUGUI>().color.b, 0.5f);
			toggle.gameObject.GetComponent<HoverImage>().ForceInit();
			toggle.gameObject.GetComponent<HoverImage>().OnExit();
			toggle.gameObject.GetComponent<HoverImage>().enabled = false;
		}
	}

	public void OnOpenCheckbox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoOpen(openCheckBox.isOn);
	}

	public void OnDisbandBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisbandConfirm();
	}

	public void OnLeaveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoLeaveConfirm();
	}

	public void OnPermissionsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildPermissionsWindow(base.gameObject);
	}

	public void OnApplicationsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildApplicantsWindow(base.gameObject);
	}

	private void DoLeaveConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_guild_leave_confirm"), null, null, DoLeave);
	}

	private void DoLeave()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnLeave);
		GuildDALC.instance.doLeave();
	}

	private void OnLeave(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnLeave);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			GameData.instance.PROJECT.character.guildData = null;
		}
	}

	private void DoDisbandConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_guild_disband_confirm"), null, null, DoDisband);
	}

	private void DoDisband()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnDisband);
		GuildDALC.instance.doDisband();
	}

	private void OnDisband(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnDisband);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void DoOpen(bool open)
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(39), OnOpen);
		GuildDALC.instance.doOpen(open);
	}

	private void OnOpen(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(39), OnOpen);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		bool @bool = sfsob.GetBool("gui20");
		_guildData.setOpen(@bool);
		DoUpdate();
	}

	public override void DoDestroy()
	{
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_RANK_CHANGE", OnChange);
			GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		SetButtonsState(state: true);
		DoUpdate();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		SetButtonsState(state: false);
	}

	private void SetButtonsState(bool state)
	{
		if (disbandBtn != null)
		{
			disbandBtn.interactable = state;
		}
		if (leaveBtn != null)
		{
			leaveBtn.interactable = state;
		}
		if (permissionsBtn != null)
		{
			permissionsBtn.interactable = state;
		}
		if (applicationsBtn != null)
		{
			applicationsBtn.interactable = state;
		}
		if (openCheckBox != null)
		{
			openCheckBox.interactable = state;
		}
	}
}
