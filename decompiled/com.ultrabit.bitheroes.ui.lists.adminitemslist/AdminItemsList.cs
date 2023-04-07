using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.adminitemslist;

public class AdminItemsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<AdminItemsItem> _removeCallback;

	public SimpleDataHelper<AdminItemsItem> Data { get; private set; }

	public void InitList(UnityAction<AdminItemsItem> removeCallback)
	{
		_removeCallback = removeCallback;
		if (Data == null)
		{
			Data = new SimpleDataHelper<AdminItemsItem>(this);
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

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		AdminItemsItem model = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.itemIcon.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.itemIcon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.itemRef, 0));
		newOrRecycled.qtyTxt.text = model.qty.ToString();
		newOrRecycled.qtyTxt.onValueChanged.RemoveAllListeners();
		newOrRecycled.qtyTxt.onValueChanged.AddListener(delegate(string value)
		{
			OnQtyChange(value, itemIcon, newOrRecycled.ItemIndex);
		});
		newOrRecycled.removeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_remove");
		newOrRecycled.removeBtn.onClick.RemoveAllListeners();
		newOrRecycled.removeBtn.onClick.AddListener(delegate
		{
			OnRemoveClick(model);
		});
		itemIcon.setQty(model.qty, show: true);
	}

	private void OnQtyChange(string value, ItemIcon itemIcon, int itemindex)
	{
		switch (value)
		{
		case "":
		case null:
		case " ":
			value = "0";
			break;
		}
		bool flag = false;
		string s = value;
		if (value.IndexOf('-') > -1)
		{
			s = value.Remove(value.IndexOf('-'), 1);
			flag = true;
		}
		if (!int.TryParse(s, out var result))
		{
			result = 0;
		}
		if (flag)
		{
			result *= -1;
		}
		itemIcon.setQty(result, show: true);
		Data[itemindex].qty = result;
	}

	private void OnRemoveClick(AdminItemsItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_removeCallback(model);
	}

	public void AddItemsAt(int index, IList<AdminItemsItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<AdminItemsItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		AdminItemsItem[] newItems = new AdminItemsItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		AdminItemsItem[] newItems = new AdminItemsItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(AdminItemsItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
