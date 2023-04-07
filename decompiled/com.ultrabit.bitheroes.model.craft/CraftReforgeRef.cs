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

[DebuggerDisplay("{id} (CraftReforgeRef)")]
public class CraftReforgeRef : BaseRef, IEquatable<CraftReforgeRef>, IComparable<CraftReforgeRef>
{
	private RarityRef _rarityRef;

	private List<ItemData> _requiredItems;

	private int _rank;

	public new string link => _link;

	public int rank => _rank;

	public RarityRef rarityRef => _rarityRef;

	public List<ItemData> requiredItems => _requiredItems;

	public CraftReforgeRef(int id, CraftBookData.Reforge reforgeData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(reforgeData.rarity);
		_requiredItems = new List<ItemData>();
		foreach (CraftBookData.Item item in reforgeData.requirements.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_requiredItems.Add(new ItemData(itemRef, item.qty));
			}
		}
		_rank = Util.GetIntFromStringProperty(reforgeData.rank, -1);
		LoadDetails(reforgeData);
	}

	public static List<ItemReforgeRef> GetItemsReforgeRef(int itemType)
	{
		List<ItemReforgeRef> list = null;
		foreach (CraftReforgeRef reforge in CraftBook.reforges)
		{
			if (list == null)
			{
				list = new List<ItemReforgeRef>();
			}
			int itemID = reforge.id;
			string reforgeLink = reforge.link;
			ItemReforgeRef item = new ItemReforgeRef(itemID, itemType, reforgeLink);
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

	public bool Equals(CraftReforgeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CraftReforgeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
