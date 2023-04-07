using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.guildmemberslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildMembersPanel : MonoBehaviour
{
	public GameObject guildMembersListView;

	public GameObject guildMembersListScroll;

	public GuildMembersList guildMembersList;

	public Button inviteBtn;

	public Button contributionBtn;

	public SpriteMask topMask;

	public SpriteMask bottomMask;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private GuildWindow _guildWindow;

	private List<GuildMemberItem> items;

	public void LoadDetails(GuildWindow guildWindow)
	{
		_guildWindow = guildWindow;
		inviteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_invite");
		contributionBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_contribution");
		GameData.instance.PROJECT.character.AddListener("GUILD_RANK_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.AddListener("GUILD_MEMBER_CHANGE", OnGuildMemberChange);
		guildMembersList.InitList(guildWindow);
		CreateList();
		UpdateButtons();
	}

	public void UpdateLayers()
	{
		topMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		topMask.frontSortingOrder = _guildWindow.sortingLayer + guildMembersList.Data.Count + 1;
		topMask.backSortingLayerID = SortingLayer.NameToID("UI");
		topMask.backSortingOrder = _guildWindow.sortingLayer;
		bottomMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.frontSortingOrder = _guildWindow.sortingLayer + guildMembersList.Data.Count + 1;
		bottomMask.backSortingLayerID = SortingLayer.NameToID("UI");
		bottomMask.backSortingOrder = _guildWindow.sortingLayer;
		if (_guildWindow.currentTab == 1)
		{
			CreateList();
		}
		else
		{
			_guildWindow.guildMembersRefreshPending = true;
		}
	}

	public void OnInviteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildInviteWindow();
	}

	public void OnContributionBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoContribution();
	}

	private void DoContribution()
	{
		DoDisable();
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(33), OnContribution);
		GuildDALC.instance.doContribution();
	}

	private void OnContribution(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		DoEnable();
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(33), OnContribution);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<GuildContributionData> list = GuildContributionData.listFromSFSObject(sfsob);
		int offset = 60;
		bool outline = true;
		List<CharacterInfoData> list2 = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData(Language.GetString("ui_experience"), outline, offset);
		list2.Add(characterInfoData);
		foreach (GuildContributionData item in list)
		{
			characterInfoData.addValue(item.name, Util.NumberFormat(item.exp, abbreviate: false), item.getNameColor(GameData.instance.PROJECT.character.guildData.id));
		}
		GameData.instance.windowGenerator.NewCharacterInfoListWindow(list2, Language.GetString("ui_contribution"));
	}

	private void OnGuildChange()
	{
		UpdateButtons();
	}

	public void OnGuildMemberChange()
	{
		if (_guildWindow.currentTab == 1)
		{
			CreateList();
		}
		else
		{
			_guildWindow.guildMembersRefreshPending = true;
		}
	}

	private void UpdateButtons()
	{
		if (GameData.instance.PROJECT.character.guildData.hasPermission(1))
		{
			Util.SetButton(inviteBtn);
		}
		else
		{
			Util.SetButton(inviteBtn, enabled: false);
		}
	}

	public void CreateList()
	{
		guildMembersList.ClearList();
		List<GuildMemberData> list = Util.SortVector(GameData.instance.PROJECT.character.guildData.members, new string[3] { "offline", "loginMilliseconds", "rank" }, Util.ARRAY_ASCENDING);
		items = new List<GuildMemberItem>();
		for (int i = 0; i < list.Count; i++)
		{
			items.Add(new GuildMemberItem
			{
				memberData = list[i],
				guildMembersPanel = this
			});
		}
		guildMembersList.Data.InsertItemsAtEnd(items);
		guildMembersList.Refresh();
	}

	private void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("GUILD_RANK_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.RemoveListener("GUILD_MEMBER_CHANGE", OnGuildMemberChange);
	}

	public void Show()
	{
		guildMembersListView.SetActive(value: true);
		guildMembersListScroll.SetActive(value: true);
		inviteBtn.gameObject.SetActive(value: true);
		contributionBtn.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		guildMembersListView.SetActive(value: false);
		guildMembersListScroll.SetActive(value: false);
		inviteBtn.gameObject.SetActive(value: false);
		contributionBtn.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		inviteBtn.interactable = true;
		contributionBtn.interactable = true;
	}

	public void DoDisable()
	{
		inviteBtn.interactable = false;
		contributionBtn.interactable = false;
	}
}
