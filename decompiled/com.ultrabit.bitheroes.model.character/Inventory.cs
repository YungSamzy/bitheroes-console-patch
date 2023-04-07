using System.Collections.Generic;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class Inventory
{
	private List<ItemData> _items;

	public List<ItemData> items => _items;

	public Inventory(List<ItemData> items)
	{
		_items = items;
	}

	public ItemData getFirstItemByType(int type)
	{
		foreach (ItemData item in _items)
		{
			if (item.itemRef.itemType == type && item.qty > 0)
			{
				return item;
			}
		}
		return null;
	}

	public ItemData getItem(int id, int type, int minQty = 1)
	{
		foreach (ItemData item in _items)
		{
			if (item.itemRef == null)
			{
				D.LogWarning("Inventory::GetItem:: Not Found " + item.id + " -> " + item.type);
			}
			if (item.itemRef.id == id && item.itemRef.itemType == type && item.qty >= minQty)
			{
				return item;
			}
		}
		return null;
	}

	public ItemData getItemAdgorPurchase(int id)
	{
		foreach (ItemData item in _items)
		{
			if (item.itemRef.id == id && item.qty < 1)
			{
				return item;
			}
		}
		return null;
	}

	public List<ItemData> GetItemsByType(int type, int subtype = -1, bool checkQty = true)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in _items)
		{
			item.itemRef.OverrideItemType(-1);
			if (item != null && (!checkQty || item.qty > 0) && item.itemRef.itemType == type && item.itemRef.MatchesSubtype(subtype))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public int getItemTypeQty(int type, int subtype = -1)
	{
		int num = 0;
		foreach (ItemData item in _items)
		{
			if (item != null && item.itemRef.itemType == type && item.itemRef.MatchesSubtype(subtype))
			{
				num += item.qty;
			}
		}
		return num;
	}

	public List<ItemData> getConsumablesByCurrencyID(int currencyID)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in _items)
		{
			if (item != null && item.qty > 0 && item.itemRef.itemType == 4 && (item.itemRef as ConsumableRef).currencyID == currencyID)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<ItemData> getReforgableItems()
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in _items)
		{
			if (item != null && item.qty > 0 && item.itemRef.getReforgeableItems().Count >= 1)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public ItemData insertItem(ItemRef itemRef, int qty = 1)
	{
		if (getItem(itemRef.id, itemRef.itemType, int.MinValue) != null)
		{
			return null;
		}
		ItemData itemData = new ItemData(itemRef, qty);
		_items.Add(itemData);
		return itemData;
	}

	public bool hasOwnedItem(ItemRef itemRef)
	{
		if (itemRef == null)
		{
			return false;
		}
		return getItem(itemRef.id, itemRef.itemType, int.MinValue) != null;
	}

	public static Inventory fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha16"))
		{
			return null;
		}
		List<ItemData> list = new List<ItemData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha16");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(ItemData.fromSFSObject(sFSObject));
		}
		return new Inventory(list);
	}

	public List<Dictionary<string, object>> statItems(Character character, int type = -1)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		foreach (ItemData item in _items)
		{
			if (type == -1 || item.type == type)
			{
				if (item.type == 6)
				{
					list.Add(character.familiarStable.familiarToStat(item));
				}
				else
				{
					list.Add(itemToStat(item, character));
				}
			}
		}
		return list;
	}

	public Dictionary<string, object> itemToStat(ItemData item, Character character)
	{
		bool flag = character?.equipment.getItemSetEquipped(item.itemRef) ?? false;
		return new Dictionary<string, object>
		{
			{ "item_id", item.id },
			{
				"item_name",
				item.itemRef.statName
			},
			{ "quantity", item.qty },
			{
				"upgrade_level",
				item.itemRef.rank
			},
			{ "rarity", item.rarity },
			{ "type", item.type },
			{ "tier", item.tier },
			{ "is_equipped", flag }
		};
	}
}
