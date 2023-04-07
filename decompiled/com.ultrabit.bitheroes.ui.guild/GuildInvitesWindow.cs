using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.guildinviteslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildInvitesWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI membersListTxt;

	public TextMeshProUGUI lvlListTxt;

	public Button declineAllBtn;

	public Image loadingIcon;

	public GuildInvitesList guildInvitesList;

	private List<GuildInfo> _guilds;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_guild_invites");
		nameListTxt.text = Language.GetString("ui_name");
		membersListTxt.text = Language.GetString("ui_members");
		lvlListTxt.text = Language.GetString("ui_lvl");
		declineAllBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline_all");
		guildInvitesList.InitList();
		DoInvitesLoad();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void DoInvitesLoad()
	{
		guildInvitesList.ClearList();
		Util.SetButton(declineAllBtn, enabled: false);
		loadingIcon.gameObject.SetActive(value: true);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(12), OnInvitesLoad);
		GuildDALC.instance.doInvitesLoad();
	}

	private void OnInvitesLoad(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Util.SetButton(declineAllBtn);
		loadingIcon.gameObject.SetActive(value: false);
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(12), OnInvitesLoad);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		_guilds = GuildInfo.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.SetGuildInvite(_guilds);
		CreateList(_guilds);
	}

	public void DoInviteDeclineAll()
	{
		if (guildInvitesList.Data.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_guild_invites"));
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_decline_guild_invites_confirm"), null, null, delegate
		{
			OnInviteDeclineAllYes();
		});
	}

	private void OnInviteDeclineAllYes()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnInviteDeclineAll);
		GuildDALC.instance.doInviteDeclineAll();
	}

	private void OnInviteDeclineAll(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnInviteDeclineAll);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		guildInvitesList.ClearList();
		base.OnClose();
	}

	private void CreateList(List<GuildInfo> guilds)
	{
		for (int i = 0; i < guilds.Count; i++)
		{
			guildInvitesList.Data.InsertOneAtEnd(new GuildInviteItem
			{
				guildInfo = guilds[i]
			});
		}
	}

	public void RemoveGuild(int guildID)
	{
		for (int i = 0; i < guildInvitesList.Data.Count; i++)
		{
			if (guildInvitesList.Data[i].guildInfo.id == guildID)
			{
				guildInvitesList.RemoveItems(i, 1);
				break;
			}
		}
	}

	public void OnDeclineAll()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoInviteDeclineAll();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		declineAllBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		declineAllBtn.interactable = false;
	}
}
