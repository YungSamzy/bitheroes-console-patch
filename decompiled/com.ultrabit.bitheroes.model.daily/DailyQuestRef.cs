using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.daily;

[DebuggerDisplay("{name} (DailyQuestRef)")]
public class DailyQuestRef : BaseRef, IEquatable<DailyQuestRef>, IComparable<DailyQuestRef>
{
	private RarityRef _rarityRef;

	private List<ItemData> _items;

	private int _amount;

	public string coloredName => _rarityRef.ConvertString(name);

	public int amount => _amount;

	public RarityRef rarityRef => _rarityRef;

	public List<ItemData> items => _items;

	public DailyQuestRef(int id, DailyQuestBookData.Quest questData)
		: base(id)
	{
		_rarityRef = RarityBook.Lookup(questData.rarity);
		_items = new List<ItemData>();
		foreach (DailyQuestBookData.Item item in questData.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_items.Add(new ItemData(itemRef, item.qty));
			}
		}
		_amount = questData.amount;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(DailyQuestRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(DailyQuestRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
