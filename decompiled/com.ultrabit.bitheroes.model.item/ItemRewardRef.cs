using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.item;

[DebuggerDisplay("{name} (ItemRewardRef)")]
public class ItemRewardRef : BaseRef
{
	public const int REWARD_TYPE_NONE = 0;

	public const int REWARD_TYPE_LOGIN = 1;

	private int _type;

	private string _dialog;

	private ItemData _itemData;

	private DateRef _dateRef;

	private static Dictionary<string, int> REWARD_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["login"] = 1
	};

	public bool isAvailable
	{
		get
		{
			if (!isActive)
			{
				return false;
			}
			if (_itemData == null || _itemData.itemRef == null)
			{
				return false;
			}
			if (!_itemData.itemRef.unique || (_itemData.itemRef.unique && !GameData.instance.PROJECT.character.inventory.hasOwnedItem(_itemData.itemRef)))
			{
				return true;
			}
			return false;
		}
	}

	public bool isActive
	{
		get
		{
			if (_dateRef != null)
			{
				return _dateRef.getActive();
			}
			return true;
		}
	}

	private new string name => _itemData.itemRef.name;

	public int type => _type;

	public DialogRef dialog => DialogBook.Lookup(_dialog);

	public ItemRewardRef(int id, VariableBookData.Item bookItem)
		: base(id)
	{
		_type = ((bookItem.rewardType != null) ? getRewardType(bookItem.rewardType) : 0);
		_dialog = bookItem.dialog;
		_itemData = new ItemData(ItemBook.Lookup(bookItem.id, ItemRef.getItemType(bookItem.type)), bookItem.qty);
		_dateRef = new DateRef(bookItem.startDate, bookItem.endDate);
		LoadDetails(bookItem);
	}

	public static int getRewardType(string type)
	{
		if (!REWARD_TYPES.ContainsKey(type))
		{
			return 0;
		}
		return REWARD_TYPES[type.ToLowerInvariant()];
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
