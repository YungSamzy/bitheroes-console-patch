using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.familiarboardlist;

public class FamiliarBoardList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public UnityAction<BaseModelData> onSelectedCallback;

	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<FamiliarBoardItemModel> Data { get; private set; }

	public void StartList(UnityAction<BaseModelData> onSelectedCallback)
	{
		this.onSelectedCallback = onSelectedCallback;
		if (Data == null)
		{
			Data = new SimpleDataHelper<FamiliarBoardItemModel>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<FamiliarBoardItemModel>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_stable_familiars_empty");
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
		FamiliarBoardItemModel familiarBoardItemModel = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(familiarBoardItemModel.itemData);
		itemIcon.SetItemActionType(10, onSelectedCallback);
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	public void AddItemsAt(int index, IList<FamiliarBoardItemModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<FamiliarBoardItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FamiliarBoardItemModel[] newItems = new FamiliarBoardItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		FamiliarBoardItemModel[] newItems = new FamiliarBoardItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FamiliarBoardItemModel[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
