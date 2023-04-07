using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.fusion;

[DebuggerDisplay("{name} (FusionRef)")]
public class FusionRef : ItemRef, IEquatable<FusionRef>, IComparable<FusionRef>
{
	private ItemTradeRef _tradeRef;

	private bool _craftLocked;

	public ItemTradeRef tradeRef => _tradeRef;

	public bool craftLocked => _craftLocked;

	public FusionRef(int id, FusionBookData.Fusion fusionData)
		: base(id, 7)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (FusionBookData.Item item in fusionData.requirements.lstItem)
		{
			list.Add(new ItemData(ItemBook.Lookup(item.id, ItemRef.getItemType(item.type)), int.Parse(item.qty)));
		}
		ItemData resultItem = new ItemData(ItemBook.Lookup(fusionData.result.item.id, fusionData.result.item.type), fusionData.result.item.qty);
		_tradeRef = new ItemTradeRef(list, resultItem);
		_rarityRef = RarityBook.Lookup(fusionData.rarity);
		_craftLocked = Util.GetBoolFromStringProperty(fusionData.craftlocked);
	}

	public bool isLocked()
	{
		return !GameData.instance.PROJECT.character.inventory.hasOwnedItem(this);
	}

	public bool Equals(FusionRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(FusionRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
