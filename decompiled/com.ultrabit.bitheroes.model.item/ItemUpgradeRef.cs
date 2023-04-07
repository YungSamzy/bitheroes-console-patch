using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.craft;

namespace com.ultrabit.bitheroes.model.item;

[DebuggerDisplay("{upgradeLink} (ItemUpgradeRef)")]
public class ItemUpgradeRef : IEquatable<ItemUpgradeRef>, IComparable<ItemUpgradeRef>
{
	private int _id;

	private int _itemID;

	private int _itemType;

	private string _upgradeLink;

	public int id => _id;

	public int itemID => _itemID;

	private string upgradeLink => _upgradeLink;

	public ItemUpgradeRef(int id, int itemID, int itemType, string upgradeLink)
	{
		_id = id;
		_itemID = itemID;
		_itemType = itemType;
		_upgradeLink = upgradeLink;
	}

	public ItemRef getUpgradeItemRef()
	{
		return ItemBook.Lookup(_itemID, _itemType);
	}

	public CraftUpgradeRef getUpgradeRef()
	{
		return CraftBook.LookupUpgradeLink(_upgradeLink);
	}

	public bool Equals(ItemUpgradeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(ItemUpgradeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
