using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.guildshoplist;

public class GuildShopList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public SimpleDataHelper<GuildShopItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildShopItem>(this);
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		GuildShopItem guildShopItem = Data[newOrRecycled.ItemIndex];
		int qty = GetQty(guildShopItem);
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new GuildShopRefModelData(guildShopItem.shopRef, qty), locked: false, qty, tintRarity: true, newOrRecycled.UIItemIconColor);
		itemIcon.SetItemActionType(4);
		if (guildShopItem.guildData.level < guildShopItem.shopRef.guildLvlReq)
		{
			newOrRecycled.UIReq.text = Language.GetString("ui_current_guild_lvl", new string[1] { Util.NumberFormat(guildShopItem.shopRef.guildLvlReq) });
			Util.SetImageAlpha(newOrRecycled.UIBlueButtonBack, alpha: true);
			Util.SetImageAlpha(newOrRecycled.UIBg, alpha: true);
			newOrRecycled.UIReq.gameObject.SetActive(value: true);
		}
		else
		{
			Util.SetImageAlpha(newOrRecycled.UIBlueButtonBack, alpha: false);
			Util.SetImageAlpha(newOrRecycled.UIBg, alpha: false);
			newOrRecycled.UIReq.gameObject.SetActive(value: false);
		}
		newOrRecycled.UIName.text = guildShopItem.shopRef.itemRef.coloredName;
		newOrRecycled.UICost.text = Util.NumberFormat(guildShopItem.shopRef.costHonor * qty);
	}

	private int GetQty(GuildShopItem model)
	{
		if (model.shopRef.itemRef.allowQty && model.guildShopPanel != null)
		{
			return model.guildShopPanel.GetQty();
		}
		return 1;
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildShopItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<GuildShopItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildShopItem[] newItems = new GuildShopItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildShopItem[] newItems = new GuildShopItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildShopItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
