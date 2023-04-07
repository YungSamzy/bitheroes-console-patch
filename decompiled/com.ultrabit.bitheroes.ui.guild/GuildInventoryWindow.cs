using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.guildinventorycosmeticslist;
using com.ultrabit.bitheroes.ui.lists.guildinventoryitemslist;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildInventoryWindow : WindowsMain
{
	public const int TAB_COSMETICS = 0;

	public const int TAB_ITEMS = 1;

	public TextMeshProUGUI topperTxt;

	public Button cosmeticsBtn;

	public Button itemsBtn;

	public GameObject guildInventoryCosmeticsListView;

	public GameObject guildInventoryCosmeticsListScroll;

	public GuildInventoryCosmeticsList guildInventoryCosmeticsList;

	public GameObject guildInventoryItemsListView;

	public GameObject guildInventoryItemsListScroll;

	public GameObject guildInventoryItemsListEmptyText;

	public GuildInventoryItemsList guildInventoryItemsList;

	private GuildData _data;

	private int _currentTab = -1;

	private bool itemsListRefreshPending;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(GuildData data, int tab = -1)
	{
		_data = data;
		_currentTab = tab;
		topperTxt.text = Language.GetString("ui_inventory");
		cosmeticsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cosmetics");
		itemsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_items");
		guildInventoryCosmeticsList.InitList();
		CreateCosmeticsList();
		guildInventoryItemsList.InitList(_data, this);
		CreateItemsList();
		SetTab((tab >= 0) ? tab : 0);
		GameData.instance.PROJECT.character.AddListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnGuildChange()
	{
		if (_currentTab == 0)
		{
			itemsListRefreshPending = true;
		}
		else
		{
			CreateItemsList();
		}
	}

	public void OnCosmeticsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnItemsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	private void SetTab(int tab)
	{
		switch (tab)
		{
		case 0:
			_currentTab = 0;
			Util.SetTab(cosmeticsBtn);
			Util.SetTab(itemsBtn, enabled: false);
			guildInventoryCosmeticsListView.SetActive(value: true);
			guildInventoryCosmeticsListScroll.SetActive(value: true);
			guildInventoryItemsListView.SetActive(value: false);
			guildInventoryItemsListScroll.SetActive(value: false);
			guildInventoryItemsListEmptyText.SetActive(value: false);
			guildInventoryCosmeticsList.Refresh();
			break;
		case 1:
			_currentTab = 1;
			Util.SetTab(cosmeticsBtn, enabled: false);
			Util.SetTab(itemsBtn);
			guildInventoryItemsListView.SetActive(value: true);
			guildInventoryItemsListScroll.SetActive(value: true);
			guildInventoryItemsListEmptyText.SetActive(value: true);
			guildInventoryCosmeticsListView.SetActive(value: false);
			guildInventoryCosmeticsListScroll.SetActive(value: false);
			guildInventoryItemsList.Refresh();
			if (itemsListRefreshPending)
			{
				itemsListRefreshPending = false;
				CreateItemsList();
			}
			break;
		}
	}

	public void CreateCosmeticsList()
	{
		guildInventoryCosmeticsList.ClearList();
		List<GuildHallCosmeticRef> orderedCosmetics = GuildHallBook.GetOrderedCosmetics();
		for (int i = 0; i < orderedCosmetics.Count; i++)
		{
			if (orderedCosmetics[i] != null && orderedCosmetics[i].display && orderedCosmetics[i].parentRef == null)
			{
				guildInventoryCosmeticsList.Data.InsertOneAtEnd(new GuildCosmeticItem
				{
					cosmeticRef = orderedCosmetics[i],
					data = _data
				});
			}
		}
	}

	public void CreateItemsList()
	{
		guildInventoryItemsList.ClearList();
		foreach (ItemData item in ItemData.SortLoot(_data.inventory.items))
		{
			if (item != null)
			{
				guildInventoryItemsList.Data.InsertOneAtEnd(new GuildItem
				{
					itemData = item,
					guildData = _data,
					permission = GameData.instance.PROJECT.character.guildData.hasPermission(6)
				});
			}
		}
	}

	private void OnDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("GUILD_PERMISSIONS_CHANGE", OnGuildChange);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		cosmeticsBtn.interactable = true;
		itemsBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		cosmeticsBtn.interactable = false;
		itemsBtn.interactable = false;
	}
}
