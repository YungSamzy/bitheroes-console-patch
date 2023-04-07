using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.fusionlist;

public class FusionList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<FusiontItemModel> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<FusiontItemModel>(this);
			base.Start();
		}
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
		FusiontItemModel fusiontItemModel = Data[newOrRecycled.ItemIndex];
		ItemCraftIcon itemCraftIcon = newOrRecycled.root.gameObject.GetComponent<ItemCraftIcon>();
		if (itemCraftIcon == null)
		{
			itemCraftIcon = newOrRecycled.root.gameObject.AddComponent<ItemCraftIcon>();
		}
		itemCraftIcon.SetItemData(fusiontItemModel.fusionRef);
	}

	private void OpenDialog(FusiontItemModel model)
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_fuse_confirm", new string[1] { model.fusionRef.tradeRef.requiredItems[0].itemRef.coloredName + ", " + model.fusionRef.tradeRef.requiredItems[1].itemRef.coloredName }), Language.GetString("ui_confirm"), Language.GetString("ui_cancel"), OnConfirmFusion);
	}

	private void OnConfirmFusion()
	{
	}

	public void AddItemsAt(int index, IList<FusiontItemModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<FusiontItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FusiontItemModel[] newItems = new FusiontItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		FusiontItemModel[] newItems = new FusiontItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FusiontItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
