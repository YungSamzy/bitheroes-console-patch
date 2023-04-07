using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.guildapplicationlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildApplicationWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI nameListTxt;

	public TextMeshProUGUI membersListTxt;

	public TextMeshProUGUI lvlListTxt;

	public Button searchBtn;

	public Button refreshBtn;

	public TMP_InputField searchTxt;

	public Image loadingIcon;

	public GuildApplicationList guildApplicationList;

	private bool selected;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_guilds");
		nameListTxt.text = Language.GetString("ui_name");
		membersListTxt.text = Language.GetString("ui_members");
		lvlListTxt.text = Language.GetString("ui_lvl");
		searchBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_search");
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		searchTxt.placeholder.GetComponent<TextMeshProUGUI>().text = Language.GetString("ui_search");
		searchTxt.characterLimit = VariableBook.guildNameLength;
		Debug.LogWarning("Check InputText Submit on mobile");
		searchTxt.onSubmit.AddListener(OnSearchBtn);
		guildApplicationList.InitList();
		DoList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnSelected()
	{
		if (!selected)
		{
			selected = true;
			searchTxt.placeholder.GetComponent<TextMeshProUGUI>().text = "";
		}
	}

	public void OnValueChanged()
	{
		for (int i = 0; i < searchTxt.text.Length; i++)
		{
			if (!Util.GuildNameAllowed(searchTxt.text[i]))
			{
				searchTxt.text = searchTxt.text.Remove(i, 1);
				searchTxt.caretPosition = i;
			}
		}
	}

	public void OnSearchBtn(string args = null)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoList();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoList();
	}

	public void DoList()
	{
		guildApplicationList?.ClearList();
		Util.SetButton(searchBtn, enabled: false);
		Util.SetButton(refreshBtn, enabled: false);
		loadingIcon?.gameObject?.SetActive(value: true);
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(31), OnList);
		if (searchTxt != null)
		{
			GuildDALC.instance.doList(searchTxt.text);
		}
	}

	private void OnList(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Util.SetButton(searchBtn);
		Util.SetButton(refreshBtn);
		if (loadingIcon != null && loadingIcon.gameObject != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(31), OnList);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<GuildInfo> guilds = GuildInfo.listFromSFSObject(sfsob);
		CreateList(guilds);
	}

	private void CreateList(List<GuildInfo> guilds)
	{
		for (int i = 0; i < guilds.Count; i++)
		{
			guildApplicationList.Data.InsertOneAtEnd(new GuildApplicationItem
			{
				guildInfo = guilds[i]
			});
		}
	}

	public void RemoveGuild(int guildID)
	{
		for (int i = 0; i < guildApplicationList.Data.Count; i++)
		{
			if (guildApplicationList.Data[i].guildInfo.id == guildID)
			{
				guildApplicationList.RemoveItems(i, 1);
				break;
			}
		}
	}

	public override void DoDestroy()
	{
		searchTxt.onSubmit.RemoveListener(OnSearchBtn);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		searchBtn.interactable = true;
		refreshBtn.interactable = true;
		searchTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		searchBtn.interactable = false;
		refreshBtn.interactable = false;
		searchTxt.interactable = false;
	}
}
