using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.item;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.familiar;

public class FamiliarStableData : BaseModelData
{
	private FamiliarRef _familiarRef;

	private int _qty;

	public FamiliarRef familiarRef => _familiarRef;

	public int rarity => _familiarRef.rarityRef.id;

	public int id => _familiarRef.id;

	public override ItemRef itemRef => familiarRef;

	public override int power => familiarRef.getPower(GameData.instance.PROJECT.character.getTotalStats());

	public override int stamina => familiarRef.getStamina(GameData.instance.PROJECT.character.getTotalStats());

	public override int agility => familiarRef.getAgility(GameData.instance.PROJECT.character.getTotalStats());

	public override object data => null;

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

	public override int type => 6;

	public FamiliarStableData(FamiliarRef familiarRef, int qty)
	{
		_familiarRef = familiarRef;
		_qty = qty;
	}

	public static FamiliarStableData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("fam1"))
		{
			return null;
		}
		int @int = sfsob.GetInt("fam1");
		return new FamiliarStableData(qty: sfsob.GetInt("fam2"), familiarRef: FamiliarBook.Lookup(@int));
	}

	public static List<FamiliarStableData> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("fam0"))
		{
			return null;
		}
		ISFSArray sFSArray = sfsob.GetSFSArray("fam0");
		List<FamiliarStableData> list = new List<FamiliarStableData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			FamiliarStableData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
