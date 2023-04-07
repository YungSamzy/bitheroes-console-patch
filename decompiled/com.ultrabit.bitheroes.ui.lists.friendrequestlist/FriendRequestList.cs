using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

namespace com.ultrabit.bitheroes.ui.lists.friendrequestlist;

public class FriendRequestList : OSA<BaseParamsWithPrefab, FriendTileVH>
{
	public SimpleDataHelper<FriendTileModel> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<FriendTileModel>(this);
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

	protected override FriendTileVH CreateViewsHolder(int itemIndex)
	{
		_ = Data[itemIndex].CachedType;
		FriendTileVH friendTileVH = new FriendTileVH();
		friendTileVH.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return friendTileVH;
	}

	protected override void UpdateViewsHolder(FriendTileVH newOrRecycled)
	{
		BaseModel model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UpdateViews(model);
	}

	public void AddItemsAt(int index, IList<FriendTileModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<FriendTileModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FriendTileModel[] newItems = new FriendTileModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FriendTileModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
