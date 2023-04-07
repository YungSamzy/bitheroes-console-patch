using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.crafttradelist;

public class CraftTradeList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<CraftTradeListItem> Data { get; private set; }

	public void ClearList()
	{
		if (Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<CraftTradeListItem>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		InitList();
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		CraftTradeListItem craftTradeListItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.craftBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_craft");
		ItemIcon itemIcon = newOrRecycled.result.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.result.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(craftTradeListItem.craftTradeRef.tradeRef.resultItem);
		bool flag = true;
		for (int i = 0; i < newOrRecycled.requirementsContainer.transform.childCount; i++)
		{
			bool flag2 = i + 1 <= craftTradeListItem.craftTradeRef.tradeRef.requiredItems.Count;
			Transform child = newOrRecycled.requirementsContainer.GetChild(i);
			Transform child2 = newOrRecycled.quantityContainer.GetChild(i);
			child.gameObject.SetActive(flag2);
			child2.gameObject.SetActive(flag2);
			if (flag2)
			{
				ItemIcon itemIcon2 = child.gameObject.GetComponent<ItemIcon>();
				if (itemIcon2 == null)
				{
					itemIcon2 = child.gameObject.AddComponent<ItemIcon>();
				}
				int qty = craftTradeListItem.craftTradeRef.tradeRef.requiredItems[i].qty;
				int itemQty = GameData.instance.PROJECT.character.getItemQty(craftTradeListItem.craftTradeRef.tradeRef.requiredItems[i].itemRef);
				bool flag3 = qty > itemQty;
				itemIcon2.SetItemData(craftTradeListItem.craftTradeRef.tradeRef.requiredItems[i], flag3, itemQty);
				TextMeshProUGUI componentInChildren = child2.GetComponentInChildren<TextMeshProUGUI>();
				componentInChildren.text = Util.NumberFormat(qty, abbreviate: true, shortbool: true);
				componentInChildren.color = (flag3 ? Color.red : Color.white);
				flag = flag && !flag3;
			}
		}
		Util.SetButton(newOrRecycled.craftBtn, flag);
	}

	public void AddItemsAt(int index, IList<CraftTradeListItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<CraftTradeListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		CraftTradeListItem[] newItems = new CraftTradeListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		CraftTradeListItem[] newItems = new CraftTradeListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(CraftTradeListItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
