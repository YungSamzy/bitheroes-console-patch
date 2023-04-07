using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildMemberWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI playerNameTitleTxt;

	public TextMeshProUGUI playerRankTitleTxt;

	public TextMeshProUGUI playerNameTxt;

	public TextMeshProUGUI playerRankTxt;

	public Button viewBtn;

	public Button promoteBtn;

	public Button demoteBtn;

	public Button kickBtn;

	private GuildMemberData _memberData;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GuildMemberData memberData)
	{
		_memberData = memberData;
		topperTxt.text = Language.GetString("ui_member");
		playerNameTxt.text = _memberData.characterData.name;
		playerNameTitleTxt.text = Language.GetString("ui_name");
		playerRankTitleTxt.text = Language.GetString("ui_rank");
		promoteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_promote");
		demoteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_demote");
		kickBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_kick");
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		GameData.instance.PROJECT.character.AddListener("GUILD_RANK_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_MEMBER_CHANGE", OnGuildMemberChange);
		DoUpdate();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnGuildChange()
	{
		DoUpdate();
	}

	private void OnGuildMemberChange()
	{
		DoUpdate();
		if (GameData.instance.PROJECT.character.guildData.getMember(_memberData.characterData.charID) == null)
		{
			base.OnClose();
		}
	}

	public void DoUpdate()
	{
		playerRankTxt.text = Guild.getRankColoredName(_memberData.rank);
		bool num = GameData.instance.PROJECT.character.guildData.hasPermission(3);
		bool flag = GameData.instance.PROJECT.character.guildData.hasPermission(4);
		bool active = GameData.instance.PROJECT.character.guildData.hasPermission(0);
		if (!num || _memberData.rank <= 0 || _memberData.rank <= GameData.instance.PROJECT.character.guildData.rank)
		{
			Util.SetButton(promoteBtn, enabled: false);
		}
		else
		{
			Util.SetButton(promoteBtn);
		}
		if (!flag || _memberData.rank >= 3 || _memberData.rank <= GameData.instance.PROJECT.character.guildData.rank)
		{
			Util.SetButton(demoteBtn, enabled: false);
		}
		else
		{
			Util.SetButton(demoteBtn);
		}
		kickBtn.gameObject.SetActive(active);
		if (_memberData.rank <= 0 || _memberData.rank <= GameData.instance.PROJECT.character.guildData.rank)
		{
			Util.SetButton(kickBtn, enabled: false);
		}
		else
		{
			Util.SetButton(kickBtn);
		}
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(_memberData.characterData.charID);
	}

	public void OnPromoteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoPromoteConfirm();
	}

	public void OnDemoteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDemoteConfirm();
	}

	private void DoPromoteConfirm()
	{
		string rankColoredName = Guild.getRankColoredName(_memberData.rank - 1);
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_promote"), Language.GetString("ui_guild_promote_confirm", new string[2]
		{
			_memberData.characterData.name,
			rankColoredName
		}), null, null, delegate
		{
			OnPromoteConfirm();
		});
	}

	private void OnPromoteConfirm()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.doPromote(_memberData.characterData.charID);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(25), OnPromote);
	}

	private void OnPromote(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(25), OnPromote);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey("gui21"))
		{
			_memberData.setRank(sfsob.GetInt("gui21"));
		}
	}

	private void DoDemoteConfirm()
	{
		string rankColoredName = Guild.getRankColoredName(_memberData.rank + 1);
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_demote"), Language.GetString("ui_guild_demote_confirm", new string[2]
		{
			_memberData.characterData.name,
			rankColoredName
		}), null, null, delegate
		{
			OnDemoteConfirm();
		});
	}

	private void OnDemoteConfirm()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.doDemote(_memberData.characterData.charID);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(26), OnDemote);
	}

	private void OnDemote(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(26), OnDemote);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey("gui21"))
		{
			_memberData.setRank(sfsob.GetInt("gui21"));
		}
	}

	public void OnKickBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_kick"), Language.GetString("ui_guild_kick_confirm", new string[1] { _memberData.characterData.name }), null, null, delegate
		{
			OnKickConfirm();
		});
	}

	private void OnKickConfirm()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(24), OnKick);
		GuildDALC.instance.doKick(_memberData.characterData.charID);
	}

	private void OnKick(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(24), OnKick);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("GUILD_RANK_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_MEMBER_CHANGE", OnGuildMemberChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		viewBtn.interactable = true;
		promoteBtn.interactable = true;
		demoteBtn.interactable = true;
		kickBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		viewBtn.interactable = false;
		promoteBtn.interactable = false;
		demoteBtn.interactable = false;
		kickBtn.interactable = false;
	}
}
