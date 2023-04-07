using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.mountselectlist;

public class MountSelectList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	public TextMeshProUGUI emptyText;

	public SimpleDataHelper<MountListItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<MountListItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_mount_empty");
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	protected override void Start()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<MountListItem>(this);
			base.Start();
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		MountListItem mountListItem = Data[newOrRecycled.ItemIndex];
		if (mountListItem != null && mountListItem.mountData != null)
		{
			ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
			if (itemIcon == null)
			{
				itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
			}
			itemIcon.SetMountData(mountListItem.mountData);
			itemIcon.SetItemActionType(7);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	public void AddItemsAt(int index, IList<MountListItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<MountListItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		MountListItem[] newItems = new MountListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		MountListItem[] newItems = new MountListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(MountListItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
