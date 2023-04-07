using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildPermissionsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image rankDropdown;

	public Toggle promoteCheckBox;

	public Toggle demoteCheckBox;

	public Toggle inviteCheckBox;

	public Toggle editHallCheckBox;

	public Toggle applicationsCheckBox;

	public Toggle permissionsCheckBox;

	public Toggle kickCheckBox;

	public Toggle messageCheckBox;

	private GuildPermissions _permissions;

	private int currentRank;

	private Transform dropdownWindow;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_permissions");
		promoteCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_promote");
		demoteCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_demote");
		inviteCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invite");
		editHallCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_edit_hall");
		applicationsCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_applications");
		permissionsCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_permissions");
		kickCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_kick");
		messageCheckBox.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_message");
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_RANK_CHANGE", OnChange);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_PERMISSIONS_CHANGE", OnChange);
		for (int i = 0; i < 4; i++)
		{
			int num = i;
			if (num != 0)
			{
				rankDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Guild.getRankColoredName(num);
				currentRank = num;
				break;
			}
		}
		DoGetPermissions();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void DoGetPermissions()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(29), OnGetPermissions);
		GuildDALC.instance.doGetPermissions();
	}

	private void OnGetPermissions(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(29), OnGetPermissions);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		_permissions = GuildPermissions.fromSFSObject(sfsob);
		DoUpdate();
	}

	private void OnChange()
	{
		DoUpdate();
	}

	private void DoUpdate()
	{
		if (!GameData.instance.PROJECT.character.guildData.hasPermission(5))
		{
			OnClose();
		}
		if (_permissions == null)
		{
			return;
		}
		int num = currentRank;
		kickCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 0));
		inviteCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 1));
		applicationsCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 2));
		promoteCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 3));
		demoteCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 4));
		permissionsCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 5));
		editHallCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 6));
		messageCheckBox.SetIsOnWithoutNotify(_permissions.getRankPermission(num, 8));
		Toggle[] componentsInChildren = panel.GetComponentsInChildren<Toggle>();
		foreach (Toggle toggle in componentsInChildren)
		{
			if (num <= GameData.instance.PROJECT.character.guildData.rank)
			{
				toggle.interactable = false;
				toggle.enabled = false;
				toggle.GetComponentInChildren<TextMeshProUGUI>().color = Util.WHITE_ALPHA_50;
				toggle.image.color = Util.WHITE_ALPHA_50;
			}
			else
			{
				toggle.interactable = true;
				toggle.enabled = true;
				toggle.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
				toggle.image.color = Color.white;
			}
		}
	}

	public void OnRankDropdow()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_rank"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentRank, OnRankDropdownChange);
		for (int i = 0; i < 4; i++)
		{
			int num = i;
			if (num != 0)
			{
				componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
				{
					id = num,
					title = Guild.getRankColoredName(num)
				});
			}
		}
	}

	private void OnRankDropdownChange(MyDropdownItemModel model)
	{
		currentRank = model?.id ?? 2;
		rankDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
		DoUpdate();
	}

	public void OnPromoteCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 3, promoteCheckBox.isOn);
	}

	public void OnDemoteCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 4, demoteCheckBox.isOn);
	}

	public void OnInviteCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 1, inviteCheckBox.isOn);
	}

	public void OnEditHallCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 6, editHallCheckBox.isOn);
	}

	public void OnApplicationsCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 2, applicationsCheckBox.isOn);
	}

	public void OnPermissionsCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 5, permissionsCheckBox.isOn);
	}

	public void OnKickCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 0, kickCheckBox.isOn);
	}

	public void OnMessageCheckBox()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_permissions.setRankPermission(currentRank, 8, messageCheckBox.isOn);
	}

	public override void OnClose()
	{
		if (_permissions.changed)
		{
			GuildDALC.instance.doSavePermissions(_permissions);
		}
		base.OnClose();
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
		promoteCheckBox.interactable = true;
		demoteCheckBox.interactable = true;
		inviteCheckBox.interactable = true;
		editHallCheckBox.interactable = true;
		applicationsCheckBox.interactable = true;
		permissionsCheckBox.interactable = true;
		kickCheckBox.interactable = true;
		messageCheckBox.interactable = true;
		rankDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		promoteCheckBox.interactable = false;
		demoteCheckBox.interactable = false;
		inviteCheckBox.interactable = false;
		editHallCheckBox.interactable = false;
		applicationsCheckBox.interactable = false;
		permissionsCheckBox.interactable = false;
		kickCheckBox.interactable = false;
		messageCheckBox.interactable = false;
		rankDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
