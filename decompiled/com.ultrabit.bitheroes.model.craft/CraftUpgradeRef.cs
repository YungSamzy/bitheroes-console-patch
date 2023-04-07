using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.craft;

[DebuggerDisplay("{id} (CraftUpgradeRef)")]
public class CraftUpgradeRef : BaseRef, IEquatable<CraftUpgradeRef>, IComparable<CraftUpgradeRef>
{
	private List<ItemData> _requiredItems;

	private List<ItemRef> _upgradeRequiredItems;

	public new string link => _link;

	public List<ItemData> requiredItems => _requiredItems;

	public List<ItemRef> UpgradeRequiredItems => _upgradeRequiredItems;

	public CraftUpgradeRef(int id, CraftBookData.Upgrade upgradeData)
		: base(id)
	{
		_requiredItems = new List<ItemData>();
		foreach (CraftBookData.Item item in upgradeData.requirements.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_requiredItems.Add(new ItemData(itemRef, item.qty));
			}
		}
		_upgradeRequiredItems = new List<ItemRef>();
		if (upgradeData.requirements.lstItem.Count > 0)
		{
			foreach (CraftBookData.Item item2 in upgradeData.requirements.lstItem)
			{
				_upgradeRequiredItems.Add(ItemBook.Lookup(item2.id, item2.type));
			}
		}
		LoadDetails(upgradeData);
	}

	public static List<ItemUpgradeRef> GetItemsReforgeRef(int itemType)
	{
		List<ItemUpgradeRef> list = null;
		foreach (CraftUpgradeRef upgrade in CraftBook.upgrades)
		{
			if (list == null)
			{
				list = new List<ItemUpgradeRef>();
			}
			int count = list.Count;
			int itemID = upgrade.id;
			string upgradeLink = upgrade.link;
			ItemUpgradeRef item = new ItemUpgradeRef(count, itemID, itemType, upgradeLink);
			list.Add(item);
		}
		return list;
	}

	public bool RequirementsMet()
	{
		foreach (ItemData requiredItem in _requiredItems)
		{
			if (GameData.instance.PROJECT.character.getItemQty(requiredItem.itemRef) < requiredItem.qty)
			{
				return false;
			}
		}
		return true;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(CraftUpgradeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CraftUpgradeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
