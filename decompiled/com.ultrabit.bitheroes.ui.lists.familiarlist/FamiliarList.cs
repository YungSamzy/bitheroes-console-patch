using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.familiarlist;

public class FamiliarList : GridAdapter<GridParams, MyGridItemViewsHolder>
{
	private Dictionary<int, bool> _seen;

	public SimpleDataHelper<FamiliarListItem> Data { get; private set; }

	public Dictionary<int, bool> seen => _seen;

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<FamiliarListItem>(this);
			_seen = GameData.instance.SAVE_STATE.GetSeenFamiliars(GameData.instance.PROJECT.character.id);
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

	protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
	{
		FamiliarListItem familiarListItem = Data[newOrRecycled.ItemIndex];
		newOrRecycled.asset.color = Color.white;
		ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
		}
		bool flag = !GameData.instance.PROJECT.character.inventory.hasOwnedItem(familiarListItem.itemData.itemRef);
		itemIcon.SetItemData(new ItemData(familiarListItem.itemData.itemRef, -1), flag);
		itemIcon.ResetHidden();
		bool flag2 = GameData.instance.PROJECT.character.inventory.hasOwnedItem(familiarListItem.itemData.itemRef as FamiliarRef);
		bool flag3 = !(familiarListItem.itemData.itemRef as FamiliarRef).getFusionVisible();
		if (!flag2 && !AppInfo.TESTING)
		{
			itemIcon.SetHidden(v: true, flag3, flag3);
		}
		if (!flag && flag2)
		{
			itemIcon.setQty(familiarListItem.itemData.qty, show: true);
		}
		itemIcon.SetLongNotify(notify: false);
		if (flag2 && !familiarListItem.seen && !familiarListItem.notifyShown)
		{
			itemIcon.SetLongNotify(GameData.instance.SAVE_STATE.notificationsFamiliars && !GameData.instance.SAVE_STATE.notificationsDisabled);
			familiarListItem.notifyShown = true;
			if (!_seen.ContainsKey(itemIcon.itemRef.id))
			{
				_seen.Add(itemIcon.itemRef.id, value: true);
			}
			else
			{
				_seen[itemIcon.itemRef.id] = true;
			}
		}
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FamiliarListItem[] newItems = new FamiliarListItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		FamiliarListItem[] newItems = new FamiliarListItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FamiliarListItem[] newItems)
	{
		Data.List.AddRange(newItems);
		Data.NotifyListChangedExternally();
	}
}
