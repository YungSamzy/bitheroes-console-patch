using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.fishing;

[DebuggerDisplay("{name} (FishingShopItemRef)")]
public class FishingShopItemRef : IEquatable<FishingShopItemRef>, IComparable<FishingShopItemRef>
{
	private int _id;

	private ItemData _itemData;

	private int _cost;

	public int id => _id;

	public ItemData itemData => _itemData;

	private string name => itemData.itemRef.name;

	public int cost => _cost;

	public FishingShopItemRef(int id, ItemData itemData, int cost)
	{
		_id = id;
		_itemData = itemData;
		_cost = cost;
	}

	public bool Equals(FishingShopItemRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(FishingShopItemRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
