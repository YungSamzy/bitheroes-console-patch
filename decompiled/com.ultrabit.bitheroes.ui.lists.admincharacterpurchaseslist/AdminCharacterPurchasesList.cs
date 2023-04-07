using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.admincharacterpurchaseslist;

public class AdminCharacterPurchasesList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<CharacterPurchaseData> _selectedcallBack;

	public SimpleDataHelper<AdminPurchaseItem> Data { get; private set; }

	public void InitList(UnityAction<CharacterPurchaseData> selectedcallBack)
	{
		_selectedcallBack = selectedcallBack;
		if (Data == null)
		{
			Data = new SimpleDataHelper<AdminPurchaseItem>(this);
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
		AdminPurchaseItem model = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.icon.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.icon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(model.purchaseData.itemData, locked: false, model.purchaseData.itemData.qty);
		itemIcon.SetItemActionType(0);
		int num = 0;
		if (model.purchaseData.creditsSpent > 0)
		{
			num = model.purchaseData.creditsSpent;
			newOrRecycled.creditsIcon.gameObject.SetActive(value: true);
			newOrRecycled.goldIcon.gameObject.SetActive(value: false);
			newOrRecycled.honorIcon.gameObject.SetActive(value: false);
		}
		else if (model.purchaseData.honorSpent > 0)
		{
			num = model.purchaseData.honorSpent;
			newOrRecycled.creditsIcon.gameObject.SetActive(value: false);
			newOrRecycled.goldIcon.gameObject.SetActive(value: false);
			newOrRecycled.honorIcon.gameObject.SetActive(value: true);
		}
		else
		{
			num = model.purchaseData.goldSpent;
			newOrRecycled.creditsIcon.gameObject.SetActive(value: false);
			newOrRecycled.goldIcon.gameObject.SetActive(value: true);
			newOrRecycled.honorIcon.gameObject.SetActive(value: false);
		}
		newOrRecycled.costTxt.text = Util.NumberFormat(num);
		newOrRecycled.nameTxt.text = model.purchaseData.itemData.itemRef.coloredName;
		newOrRecycled.dateTxt.text = Util.dateFormat(Util.localizeDate(model.purchaseData.createDate));
		newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
		newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
		{
			_selectedcallBack(model.purchaseData);
		});
	}

	public void AddItemsAt(int index, IList<AdminPurchaseItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<AdminPurchaseItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		AdminPurchaseItem[] newItems = new AdminPurchaseItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		AdminPurchaseItem[] newItems = new AdminPurchaseItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(AdminPurchaseItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
