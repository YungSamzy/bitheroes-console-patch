using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.admin;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminGuildWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button refreshBtn;

	public Button renameBtn;

	public Button reinitialsBtn;

	public Button inventoryBtn;

	public Transform infoListContent;

	public CharacterInfoTile characterInfoItemPrefab;

	private AdminGuildData _guildData;

	private List<CharacterInfoTile> _tiles = new List<CharacterInfoTile>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(AdminGuildData guildData)
	{
		renameBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_name");
		reinitialsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_initials");
		inventoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Inventory";
		Util.SetButton(inventoryBtn, enabled: false);
		SetGuildData(guildData);
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: false, stayUp: true);
	}

	private void SetGuildData(AdminGuildData guildData)
	{
		_guildData = guildData;
		DoUpdate();
	}

	public void DoUpdate()
	{
		topperTxt.text = _guildData.name;
		int offset = 100;
		bool outline = false;
		bool abbreviate = false;
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData("Guild", outline, offset);
		characterInfoData.addValue("ID", _guildData.id);
		characterInfoData.addValue("Name", _guildData.name);
		characterInfoData.addValue("Initials", _guildData.initials);
		characterInfoData.addValue("Level", Util.NumberFormat(_guildData.level, abbreviate));
		characterInfoData.addValue("Exp", Util.NumberFormat(_guildData.exp, abbreviate));
		characterInfoData.addValue("Points", Util.NumberFormat(_guildData.points, abbreviate));
		characterInfoData.addValue("Members", Util.NumberFormat(_guildData.members.Count, abbreviate) + "/" + Util.NumberFormat(_guildData.getMemberLimit(), abbreviate));
		characterInfoData.addValue("Create Date", Util.dateFormat(Util.localizeDate(_guildData.createDate)));
		characterInfoData.addValue("Login Date", Util.dateFormat(Util.localizeDate(_guildData.loginDate)));
		list.Add(characterInfoData);
		if (_guildData.members.Count > 0)
		{
			List<GuildMemberData> list2 = Util.SortVector(_guildData.members, new string[2] { "rank", "loginMilliseconds" }, Util.ARRAY_ASCENDING);
			CharacterInfoData characterInfoData2 = new CharacterInfoData("Members", outline, offset);
			foreach (GuildMemberData item in list2)
			{
				characterInfoData2.addValue(item.characterData.name, Guild.getRankColoredName(item.rank));
			}
			if (characterInfoData2.info.Count > 0)
			{
				list.Add(characterInfoData2);
			}
		}
		CreateTiles(list);
		StartCoroutine(WaitToFixText());
	}

	private void CreateTiles(List<CharacterInfoData> info)
	{
		ClearTiles();
		for (int i = 0; i < info.Count; i++)
		{
			CharacterInfoData statData = info[i];
			CharacterInfoTile characterInfoTile = Object.Instantiate(characterInfoItemPrefab, infoListContent);
			characterInfoTile.LoadDetails(statData);
			_tiles.Add(characterInfoTile);
		}
	}

	private void ClearTiles()
	{
		foreach (CharacterInfoTile tile in _tiles)
		{
			Object.Destroy(tile.gameObject);
		}
		_tiles.Clear();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForSeconds(0.1f);
		infoListContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
		ForceScrollDown();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void OnRenameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminRenameWindow(_guildData.name, 1, "", base.gameObject);
	}

	public void OnReinitialsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminRenameWindow(_guildData.name, 2, "", base.gameObject);
	}

	public void OnInventoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void DoRefresh()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(23), OnRefresh);
		AdminDALC.instance.doGuildSearch(_guildData.id);
	}

	private void OnRefresh(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(23), OnRefresh);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AdminGuildData guildData = AdminGuildData.fromSFSObject(sfsob);
		SetGuildData(guildData);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
		renameBtn.interactable = true;
		reinitialsBtn.interactable = true;
		inventoryBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
		renameBtn.interactable = false;
		reinitialsBtn.interactable = false;
		inventoryBtn.interactable = false;
	}
}
