using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.guildperkslist;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildPerksWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI pointsTxt;

	public Button guildBucksBtn;

	public GuildPerksList guildPerksList;

	private GuildData _data;

	public GuildData guildData => _data;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GuildData data)
	{
		_data = data;
		topperTxt.text = Language.GetString("ui_perks");
		guildBucksBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.guildData.AddListener("GUILD_PERKS_CHANGE", OnGuildChange);
		guildPerksList.InitList();
		DoUpdate();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void DoUpdate()
	{
		pointsTxt.text = Util.NumberFormat(_data.points);
		UpdateList();
	}

	public void UpdateList()
	{
		double virtualAbstractNormalizedScrollPosition = guildPerksList.GetVirtualAbstractNormalizedScrollPosition();
		guildPerksList.ClearList();
		List<GuildPerkItem> list = new List<GuildPerkItem>();
		for (int i = 0; i < GuildBook.sizePerks; i++)
		{
			GuildPerkRef guildPerkRef = GuildBook.LookupPerk(i);
			if (guildPerkRef != null)
			{
				list.Add(new GuildPerkItem
				{
					perkRef = guildPerkRef,
					perkWindow = this
				});
			}
		}
		guildPerksList.Data.InsertItemsAtEnd(list);
		guildPerksList.SetVirtualAbstractNormalizedScrollPosition(virtualAbstractNormalizedScrollPosition, computeVisibilityNow: true, out var _);
	}

	public void OnGuildBucksBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewConfirmMessageWindow(CurrencyRef.GetCurrencyName(11), Util.ParseString(CurrencyRef.GetCurrencyDesc(11)));
	}

	public void OnGuildChange()
	{
		DoUpdate();
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		GameData.instance.PROJECT.character.guildData.RemoveListener("GUILD_PERKS_CHANGE", OnGuildChange);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		guildBucksBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		guildBucksBtn.interactable = false;
	}
}
