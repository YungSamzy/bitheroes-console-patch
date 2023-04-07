using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.itemselectlist;

public class ItemSelectList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public UnityAction<ItemRef> onItemSelected;

	public SimpleDataHelper<ItemSelectItem> Data { get; private set; }

	public void InitList(UnityAction<ItemRef> onItemSelected)
	{
		this.onItemSelected = onItemSelected;
		if (Data == null)
		{
			Data = new SimpleDataHelper<ItemSelectItem>(this);
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
		ItemSelectItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.nameTxt.text = model.itemRef.coloredName;
		newOrRecycled.descTxt.text = Util.ParseString(model.itemRef.desc);
		int itemQty = GameData.instance.PROJECT.character.getItemQty(model.itemRef);
		ItemIcon itemIcon = newOrRecycled.itemIcon.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.itemIcon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.itemRef, 0), locked: false, itemQty);
		itemIcon.SetItemActionType(0);
		itemIcon.enabled = false;
		newOrRecycled.disabledImage.gameObject.SetActive(itemQty <= 0);
		bool flag = true;
		if (model.itemRef.itemType == 4)
		{
			ConsumableRef consumableRef = model.itemRef as ConsumableRef;
			if (!consumableRef.displayUnlocked && GameData.instance.PROJECT.character.getItemQty(consumableRef) <= 0)
			{
				flag = false;
			}
		}
		if (flag)
		{
			newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				onItemSelected(model.itemRef);
			});
		}
		HoverImages component = newOrRecycled.root.gameObject.GetComponent<HoverImages>();
		if (component == null)
		{
			newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		component.ForceStart();
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		inRecycleBinOrVisible.root.GetComponent<Button>().onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ItemSelectItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ItemSelectItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ItemSelectItem[] newItems = new ItemSelectItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ItemSelectItem[] newItems = new ItemSelectItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ItemSelectItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
