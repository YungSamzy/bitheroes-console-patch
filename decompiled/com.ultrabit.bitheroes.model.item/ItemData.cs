using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.item;

[DebuggerDisplay("{_itemRef.name} (ItemData)")]
public class ItemData : BaseModelData, IEquatable<ItemData>
{
	private ItemRef _itemRef;

	private int _qty;

	private object _data;

	private int powerCalculated;

	private int staminaCalculated;

	private int agilityCalculated;

	private bool _isCosmetic;

	public override ItemRef itemRef => _itemRef;

	public int id => _itemRef.id;

	public int typeInverse
	{
		get
		{
			int num = _itemRef.itemType;
			switch (type)
			{
			case 16:
				num = 1;
				break;
			case 17:
				num = 1;
				break;
			case 18:
				num = 11;
				break;
			case 19:
				num = 9;
				break;
			}
			return num * -1;
		}
	}

	public int total => power + stamina + agility;

	public int totalCalculated => powerCalculated + staminaCalculated + agilityCalculated;

	public int tier => _itemRef.tier;

	public int rarity
	{
		get
		{
			if (_itemRef.rarityRef == null)
			{
				D.LogError("ItemData::rarity:: " + _itemRef.name + " does not have a rarity");
			}
			return _itemRef.rarityRef.id;
		}
	}

	public bool isCosmetic => _isCosmetic;

	public override int power
	{
		get
		{
			if (!_isCosmetic && (_itemRef.itemType.Equals(1) || _itemRef.itemType.Equals(16)))
			{
				return (itemRef as EquipmentRef).power;
			}
			return 0;
		}
	}

	public override int stamina
	{
		get
		{
			if (!_isCosmetic && (_itemRef.itemType.Equals(1) || _itemRef.itemType.Equals(16)))
			{
				return (itemRef as EquipmentRef).stamina;
			}
			return 0;
		}
	}

	public override int agility
	{
		get
		{
			if (!_isCosmetic && (_itemRef.itemType.Equals(1) || _itemRef.itemType.Equals(16)))
			{
				return (itemRef as EquipmentRef).agility;
			}
			return 0;
		}
	}

	public override object data => _data;

	public override int qty
	{
		get
		{
			return _qty;
		}
		set
		{
			_qty = value;
		}
	}

	public override int type => _itemRef.itemType;

	public ItemData(ItemRef itemRef, string qty)
	{
		_itemRef = itemRef;
		_qty = Util.GetIntFromStringProperty(qty);
	}

	public ItemData(ItemRef itemRef, int qty)
	{
		_itemRef = itemRef;
		_qty = qty;
	}

	public void SetPrecalculateds()
	{
		powerCalculated = power;
		staminaCalculated = stamina;
		agilityCalculated = agility;
	}

	public ItemData copy()
	{
		return new ItemData(_itemRef, _qty);
	}

	public static ItemData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("ite0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("ite0");
		int int2 = sfsob.GetInt("ite1");
		int int3 = sfsob.GetInt("ite2");
		ItemRef obj = ItemBook.Lookup(@int, int2);
		if (obj == null)
		{
			D.LogWarning("ItemData::fromSFSObject::itemRef is null: " + @int + "-> " + int2);
		}
		return new ItemData(obj, int3);
	}

	public static ItemData fromString(string theString)
	{
		if (theString == null || theString.Length <= 0)
		{
			return null;
		}
		string[] array = theString.Split(':');
		if (array.Length == 0)
		{
			return null;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			string s = array[i];
			switch (i)
			{
			case 0:
				num = int.Parse(s);
				break;
			case 1:
				num2 = int.Parse(s);
				break;
			case 2:
				num3 = int.Parse(s);
				break;
			}
		}
		ItemRef itemRef = ItemBook.Lookup(num, num2);
		if (itemRef == null)
		{
			return null;
		}
		return new ItemData(itemRef, num3);
	}

	public static bool listHasItem(ItemRef itemRef, List<ItemData> list)
	{
		if (list == null || itemRef == null)
		{
			return false;
		}
		foreach (ItemData item in list)
		{
			if (item.itemRef == itemRef)
			{
				return true;
			}
		}
		return false;
	}

	public static List<ItemData> listFromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return null;
		}
		if (!sfsob.ContainsKey("ite3"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("ite3");
		List<ItemData> list = new List<ItemData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			if (sFSObject != null)
			{
				ItemData item = fromSFSObject(sFSObject);
				list.Add(item);
			}
		}
		return list;
	}

	public static List<ItemData> listFromString(string theString)
	{
		List<ItemData> list = new List<ItemData>();
		if (theString == null || theString.Length <= 0)
		{
			return list;
		}
		string[] array = theString.Split(',');
		if (array.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < array.Length; i++)
		{
			ItemData itemData = fromString(array[i]);
			if (itemData != null)
			{
				list.Add(itemData);
			}
		}
		return list;
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("ite0", _itemRef.id);
		sfsob.PutInt("ite1", _itemRef.itemType);
		sfsob.PutInt("ite2", _qty);
		return sfsob;
	}

	public static SFSObject listToSFSObject(SFSObject sfsob, List<ItemData> items)
	{
		ISFSArray iSFSArray = new SFSArray();
		for (int i = 0; i < items.Count; i++)
		{
			ItemData itemData = items[i];
			iSFSArray.AddSFSObject(itemData.toSFSObject(new SFSObject()));
		}
		sfsob.PutSFSArray("ite3", iSFSArray);
		return sfsob;
	}

	public static int getItemRefQuantity(List<ItemData> list, ItemRef itemRef)
	{
		int num = 0;
		if (list == null || itemRef == null)
		{
			return num;
		}
		foreach (ItemData item in list)
		{
			if (item.itemRef == itemRef)
			{
				num += item.qty;
			}
		}
		return num;
	}

	public static string parseSummary(List<ItemData> itemsRemoved, List<ItemData> itemsAdded)
	{
		string text = "";
		if (itemsRemoved != null)
		{
			for (int i = 0; i < itemsRemoved.Count; i++)
			{
				ItemData itemData = itemsRemoved[i];
				if (text.Length > 0)
				{
					text += ", ";
				}
				text = text + "-" + itemData.qty + " " + itemData.itemRef.statName;
			}
		}
		if (itemsAdded != null)
		{
			for (int j = 0; j < itemsAdded.Count; j++)
			{
				ItemData itemData2 = itemsAdded[j];
				if (text.Length > 0)
				{
					text += ", ";
				}
				text = text + "+" + itemData2.qty + " " + itemData2.itemRef.statName;
			}
		}
		return text;
	}

	public static List<ItemData> SortLoot(List<ItemData> list)
	{
		return Util.SortVector(list, new string[4] { "rarity", "typeInverse", "total", "id" }, Util.ARRAY_DESCENDING);
	}

	public static List<ItemData> SortLoot(List<ItemData> list, string[] sort)
	{
		return Util.SortVector(list, sort, Util.ARRAY_DESCENDING);
	}

	public int CompareTo(ItemData other)
	{
		int num = typeInverse.CompareTo(other.typeInverse);
		if (num == 0)
		{
			int num2 = total.CompareTo(other.total);
			if (num2 == 0)
			{
				return rarity.CompareTo(other.rarity);
			}
			return num2;
		}
		return num;
	}

	public void SetCosmeticStats()
	{
		_isCosmetic = true;
	}

	public bool Equals(ItemData itemData)
	{
		if (itemData.itemRef.Equals(itemRef))
		{
			return itemData.qty.Equals(qty);
		}
		return false;
	}
}
