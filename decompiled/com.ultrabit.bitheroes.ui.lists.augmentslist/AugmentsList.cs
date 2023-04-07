using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.augmentslist;

public class AugmentsList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	[HideInInspector]
	public Augments _augments;

	[HideInInspector]
	public int _slot;

	[HideInInspector]
	public bool _changeable;

	[HideInInspector]
	public FamiliarRef _familiarRef;

	[HideInInspector]
	public UnityAction<BaseModelData> onAugmentSelected;

	public SimpleDataHelper<AugmentItem> Data { get; private set; }

	public void InitList(UnityAction<BaseModelData> onAugmentSelected)
	{
		this.onAugmentSelected = onAugmentSelected;
		if (Data == null)
		{
			Data = new SimpleDataHelper<AugmentItem>(this);
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
		AugmentItem augmentItem = Data[newOrRecycled.ItemIndex];
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetAugmentData(augmentItem.augmentData);
		itemIcon.DisableItem(disable: false);
		if (augmentItem.slotRef != null && augmentItem.augmentData.augmentRef.typeRef.id != augmentItem.slotRef.typeRef.id)
		{
			itemIcon.DisableItem(disable: true);
		}
		else if (_slot < 0)
		{
			if (_changeable)
			{
				itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
				itemIcon.SetItemActionType(7);
			}
		}
		else
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
			if (_augments.getFamiliarAugmentSlot(_familiarRef, _slot) == augmentItem.augmentData)
			{
				itemIcon.DisableItem(disable: true);
			}
			else
			{
				itemIcon.SetItemActionType(10, onAugmentSelected);
			}
		}
		if (augmentItem.augmentData.equipped)
		{
			itemIcon.PlayComparison("E");
		}
	}

	protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<AugmentItem> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<AugmentItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		AugmentItem[] newItems = new AugmentItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		AugmentItem[] newItems = new AugmentItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(AugmentItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
