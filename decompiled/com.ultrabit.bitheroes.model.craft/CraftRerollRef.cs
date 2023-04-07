using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.craft;

[DebuggerDisplay("{id} (CraftRerollRef)")]
public class CraftRerollRef : BaseRef, IEquatable<CraftRerollRef>, IComparable<CraftRerollRef>
{
	private RarityRef _rarityRef;

	private List<ItemData> _requiredItems;

	private int _itemType;

	private int _itemSubtype;

	public int itemType => _itemType;

	public int itemSubtype => _itemSubtype;

	public RarityRef rarityRef => _rarityRef;

	public List<ItemData> requiredItems => _requiredItems;

	public CraftRerollRef(int id, CraftBookData.Reroll rerollData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(rerollData.rarity);
		_requiredItems = new List<ItemData>();
		foreach (CraftBookData.Item item in rerollData.requirements.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_requiredItems.Add(new ItemData(itemRef, item.qty));
			}
		}
		_itemType = ((rerollData.itemType != null) ? ItemRef.getItemType(rerollData.itemType) : 0);
		_itemSubtype = Util.GetIntFromStringProperty(rerollData.itemSubtype);
		LoadDetails(rerollData);
	}

	public bool RequirementsMet(int rerolls = 0)
	{
		foreach (ItemData requiredItem in _requiredItems)
		{
			int itemQty = GameData.instance.PROJECT.character.getItemQty(requiredItem.itemRef);
			int num = requiredItem.qty * (rerolls + 1);
			if (itemQty < num)
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

	public bool Equals(CraftRerollRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CraftRerollRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
