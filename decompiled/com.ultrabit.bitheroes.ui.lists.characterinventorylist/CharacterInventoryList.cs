using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.characterinventorylist;

public class CharacterInventoryList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<CharacterInventoryListItem> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<CharacterInventoryListItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_item_list_empty");
	}

	protected override void Start()
	{
		StartList();
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
		CharacterInventoryListItem characterInventoryListItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		switch (characterInventoryListItem.itemData.type)
		{
		case 1:
			itemIcon.SetEquipmentData(characterInventoryListItem.itemData, null);
			break;
		case 16:
			itemIcon.SetEquipmentData(characterInventoryListItem.itemData, null);
			break;
		default:
			itemIcon.SetItemData(characterInventoryListItem.itemData);
			break;
		}
		itemIcon.SetItemActionType(characterInventoryListItem.itemData.itemRef.getItemTooltipType(GameData.instance.PROJECT.character));
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	public void AddItemsAt(int index, IList<CharacterInventoryListItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<CharacterInventoryListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		CharacterInventoryListItem[] newItems = new CharacterInventoryListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		CharacterInventoryListItem[] newItems = new CharacterInventoryListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(CharacterInventoryListItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
