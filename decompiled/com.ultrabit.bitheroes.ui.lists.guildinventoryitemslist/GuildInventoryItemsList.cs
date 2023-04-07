using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.guild;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.guildinventoryitemslist;

public class GuildInventoryItemsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	private GuildData _data;

	private GuildInventoryWindow _guildInventoryWindow;

	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<GuildItem> Data { get; private set; }

	public void InitList(GuildData guildData, GuildInventoryWindow guildInventoryWindow)
	{
		_data = guildData;
		_guildInventoryWindow = guildInventoryWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_item_list_empty");
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		GuildItem guildItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(guildItem.itemData.itemRef, guildItem.itemData.qty), locked: false, guildItem.itemData.qty);
		bool equipped = _data.hallData.getEquipped(guildItem.itemData.itemRef as GuildItemRef);
		if (GameData.instance.PROJECT.character.guildData.hasPermission(6))
		{
			if (equipped)
			{
				itemIcon.SetItemActionType(11, OnDeselect);
			}
			else
			{
				itemIcon.SetItemActionType(10, OnSelect);
			}
		}
		else
		{
			itemIcon.SetItemActionType(0);
		}
		if (equipped)
		{
			itemIcon.PlayComparison("E");
		}
		else
		{
			itemIcon.HideComparison();
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnDeselect(BaseModelData e)
	{
		GuildItemRef guildItemRef = (e as ItemData).itemRef as GuildItemRef;
		DoUpdateHallItemType(0, guildItemRef.guildItemType);
	}

	private void OnSelect(BaseModelData e)
	{
		GuildItemRef guildItemRef = (e as ItemData).itemRef as GuildItemRef;
		DoUpdateHallItemType(guildItemRef.id, guildItemRef.guildItemType);
	}

	private void DoUpdateHallItemType(int itemID, int itemType)
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(35), OnUpdateHallItemType);
		GuildDALC.instance.doUpdateHallItemType(itemID, itemType);
	}

	private void OnUpdateHallItemType(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(35), OnUpdateHallItemType);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GuildData data = GuildData.fromSFSObject(sfsob);
		_data.updateData(data);
		_guildInventoryWindow.OnGuildChange();
	}

	public void AddItemsAt(int index, IList<GuildItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<GuildItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildItem[] newItems = new GuildItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildItem[] newItems = new GuildItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
