using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildMainPanel : MonoBehaviour
{
	[Header("LeftContainer")]
	public GameObject leftContainer;

	public TextMeshProUGUI initialsTxt;

	public TextMeshProUGUI levelTitleTxt;

	public TextMeshProUGUI levelTxt;

	public TextMeshProUGUI expTxt;

	public RegularBarFill expBar;

	public TextMeshProUGUI membersTitleTxt;

	public TextMeshProUGUI membersTxt;

	[Header("Top")]
	public GameObject gradient;

	public TextMeshProUGUI onlineMembersTxt;

	[Header("GuildMessageBox")]
	public GuildMessageBox guildMessageBox;

	[Header("Bottom")]
	public Button optionsBtn;

	public Button inventoryBtn;

	public Button perksBtn;

	public Button hallBtn;

	private string _message = "";

	private GuildData _guildData;

	private const string BLANK = "-";

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private GuildWindow _guildWindow;

	public void LoadDetails(GuildWindow guildWindow)
	{
		_guildWindow = guildWindow;
		GameData.instance.PROJECT.character.AddListener("GUILD_MEMBER_CHANGE", OnMemberChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnPermissionsChange);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_PERKS_CHANGE", OnPerksChange);
		levelTitleTxt.text = Language.GetString("ui_level");
		membersTitleTxt.text = Language.GetString("ui_members");
		initialsTxt.text = Util.ParseGuildInitials(GameData.instance.PROJECT.character.guildData.initials);
		optionsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_options");
		inventoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_inventory");
		perksBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_perks");
		guildMessageBox.LoadDetails();
		UpdateButtons();
		UpdateMembers();
		DoLoadData();
	}

	public void OnOptionsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildOptionsWindow(_guildData);
	}

	public void OnInventoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildInventoryWindow(_guildData);
	}

	public void OnPerksBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildPerksWindow(_guildData);
	}

	public void OnHallBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		CheckInstance();
	}

	private void CheckInstance()
	{
		if (GameData.instance.PROJECT.instance != null)
		{
			if (GameData.instance.PROJECT.instance.instanceRef.type == 2)
			{
				GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(1), transition: false);
			}
			else
			{
				GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(2), transition: false);
			}
			_guildWindow.OnClose();
		}
	}

	private void SetData(GuildData guildData = null)
	{
		_guildData = guildData;
		if (_guildData == null)
		{
			DoDisable();
		}
		else
		{
			guildMessageBox.SetMessage(guildData.message);
			DoEnable();
		}
		levelTxt.text = ((_guildData != null) ? Util.NumberFormat(_guildData.level) : "-");
		UpdateExp();
	}

	private void UpdateExp()
	{
		if (_guildData != null)
		{
			long levelExp = Guild.getLevelExp(_guildData.level);
			long num = _guildData.exp - levelExp;
			long num2 = Guild.getLevelExp(_guildData.level + 1) - levelExp;
			expTxt.text = Util.NumberFormat(num) + " / " + Util.NumberFormat(num2);
			expBar.UpdateBar(num, num2);
		}
		else
		{
			expTxt.text = "-";
			expBar.UpdateBar(0f, 10f);
		}
	}

	private void DoLoadData()
	{
		SetData();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnLoadData);
		GuildDALC.instance.doLoadData();
	}

	private void OnLoadData(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnLoadData);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GuildData data = GuildData.fromSFSObject(sfsob);
		SetData(data);
	}

	private void OnMemberChange()
	{
		UpdateMembers();
		SetData(_guildData);
	}

	private void OnPermissionsChange()
	{
		if (guildMessageBox != null)
		{
			guildMessageBox.DoUpdate();
		}
	}

	private void OnPerksChange()
	{
		UpdateMembers();
	}

	public void UpdateButtons()
	{
		if (GameData.instance.PROJECT.instance == null)
		{
			hallBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_hall");
			Util.SetButton(hallBtn, enabled: false);
			return;
		}
		if (GameData.instance.PROJECT.instance.instanceRef.type == 2)
		{
			hallBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_town");
		}
		else
		{
			hallBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_hall");
		}
		Util.SetButton(hallBtn);
	}

	private void UpdateMembers()
	{
		List<GuildMemberData> onlineMembers = GameData.instance.PROJECT.character.guildData.getOnlineMembers();
		onlineMembersTxt.text = Language.GetString((onlineMembers.Count > 1) ? "ui_guildmates_online_count" : "ui_guildmate_online_count", new string[1] { Util.NumberFormat(onlineMembers.Count) });
		membersTxt.text = Util.NumberFormat(GameData.instance.PROJECT.character.guildData.GetUniqueMembersCount()) + "/" + Util.NumberFormat(GameData.instance.PROJECT.character.getGuildMemberLimit());
	}

	public void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("GUILD_MEMBER_CHANGE", OnMemberChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnPermissionsChange);
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_PERKS_CHANGE", OnPerksChange);
		}
	}

	public void Show()
	{
		leftContainer.SetActive(value: true);
		gradient.SetActive(value: true);
		guildMessageBox.gameObject.SetActive(value: true);
		optionsBtn.gameObject.SetActive(value: true);
		inventoryBtn.gameObject.SetActive(value: true);
		perksBtn.gameObject.SetActive(value: true);
		hallBtn.gameObject.SetActive(value: true);
		DoEnable();
	}

	public void Hide()
	{
		leftContainer.SetActive(value: false);
		gradient.SetActive(value: false);
		guildMessageBox.gameObject.SetActive(value: false);
		optionsBtn.gameObject.SetActive(value: false);
		inventoryBtn.gameObject.SetActive(value: false);
		perksBtn.gameObject.SetActive(value: false);
		hallBtn.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		if (optionsBtn != null)
		{
			optionsBtn.interactable = true;
		}
		if (inventoryBtn != null)
		{
			inventoryBtn.interactable = true;
		}
		if (perksBtn != null)
		{
			perksBtn.interactable = true;
		}
		if (hallBtn != null)
		{
			hallBtn.interactable = true;
		}
		if (guildMessageBox != null)
		{
			guildMessageBox.DoEnable();
		}
	}

	public void DoDisable()
	{
		optionsBtn.interactable = false;
		inventoryBtn.interactable = false;
		perksBtn.interactable = false;
		hallBtn.interactable = false;
		guildMessageBox.DoDisable();
	}
}
