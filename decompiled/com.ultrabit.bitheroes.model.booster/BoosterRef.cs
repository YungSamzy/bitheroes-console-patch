using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.booster;

[DebuggerDisplay("{id} (BoosterRef)")]
public class BoosterRef : IEquatable<BoosterRef>, IComparable<BoosterRef>
{
	private int _id;

	private DateTime? _endDate;

	private List<ItemData> _items;

	private List<ItemData> _cosmetics;

	private DateTime _startDate;

	public int id => _id;

	public DateTime? endDate
	{
		get
		{
			return _endDate;
		}
		set
		{
			_endDate = value;
		}
	}

	public DateTime startDate
	{
		get
		{
			return _startDate;
		}
		set
		{
			_startDate = value;
		}
	}

	public List<ItemData> items => _items;

	public List<ItemData> cosmetics => _cosmetics;

	public BoosterRef(BoosterBookData.Booster data)
	{
		_id = data.id;
		_items = new List<ItemData>();
		foreach (BoosterBookData.Item lstItem in data.lstItems)
		{
			ItemRef itemRef = ItemBook.Lookup(lstItem.id, lstItem.type);
			_items.Add(new ItemData(itemRef, lstItem.qty));
		}
		_cosmetics = new List<ItemData>();
		foreach (BoosterBookData.Item item in data.lstCosmetic)
		{
			ItemRef itemRef2 = ItemBook.Lookup(item.id, item.type);
			_cosmetics.Add(new ItemData(itemRef2, item.qty));
		}
	}

	public static BoosterRef fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("booster01"))
		{
			return null;
		}
		BoosterRef boosterRef = BoosterBook.Lookup(sfsob.GetInt("booster01"));
		if (sfsob.ContainsKey("booster03"))
		{
			boosterRef.endDate = Util.GetDateFromString(sfsob.GetUtfString("booster03"));
		}
		boosterRef.startDate = Util.GetDateFromString(sfsob.GetUtfString("booster04"));
		return boosterRef;
	}

	public static List<BoosterRef> listFromSFSObjectActives(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return null;
		}
		if (!sfsob.ContainsKey("booster02"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("booster02");
		List<BoosterRef> list = new List<BoosterRef>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			if (sFSObject != null)
			{
				BoosterRef item = fromSFSObject(sFSObject);
				list.Add(item);
			}
		}
		return list;
	}

	public static List<BoosterRef> listFromSFSObjectPassives(ISFSObject sfsob)
	{
		if (sfsob == null)
		{
			return null;
		}
		if (!sfsob.ContainsKey("booster05"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("booster05");
		List<BoosterRef> list = new List<BoosterRef>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			if (sFSObject != null)
			{
				BoosterRef item = fromSFSObject(sFSObject);
				list.Add(item);
			}
		}
		return list;
	}

	public bool Equals(BoosterRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(BoosterRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
