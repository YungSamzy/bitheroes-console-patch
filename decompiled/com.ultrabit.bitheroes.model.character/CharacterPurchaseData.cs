using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterPurchaseData
{
	private int _id;

	private DateTime _createDate;

	private ItemData _itemData;

	private int _platform;

	private int _goldSpent;

	private int _creditsSpent;

	private int _honorSpent;

	private List<ItemData> _itemsAdded;

	public int id => _id;

	public DateTime createDate => _createDate;

	public ItemData itemData => _itemData;

	public int platform => _platform;

	public int goldSpent => _goldSpent;

	public int creditsSpent => _creditsSpent;

	public int honorSpent => _honorSpent;

	public List<ItemData> itemsAdded => _itemsAdded;

	public CharacterPurchaseData(int id, DateTime createDate, ItemData itemData, int platform, int goldSpent, int creditsSpent, int honorSpent, List<ItemData> itemsAdded)
	{
		_id = id;
		_createDate = createDate;
		_itemData = itemData;
		_platform = platform;
		_goldSpent = goldSpent;
		_creditsSpent = creditsSpent;
		_honorSpent = honorSpent;
		_itemsAdded = itemsAdded;
	}

	public static CharacterPurchaseData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("pur0");
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("pur1"));
		ItemData itemData = ItemData.fromSFSObject(sfsob.GetSFSObject("pur2"));
		int int2 = sfsob.GetInt("pur3");
		int int3 = sfsob.GetInt("pur4");
		int int4 = sfsob.GetInt("pur5");
		int int5 = sfsob.GetInt("pur6");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("pur7"));
		return new CharacterPurchaseData(@int, dateFromString, itemData, int2, int3, int4, int5, list);
	}

	public static List<CharacterPurchaseData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("pur8");
		List<CharacterPurchaseData> list = new List<CharacterPurchaseData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
