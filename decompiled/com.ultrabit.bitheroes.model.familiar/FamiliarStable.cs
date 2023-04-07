using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.familiar;

public class FamiliarStable
{
	private List<FamiliarStableData> _familiars;

	public List<FamiliarStableData> familiars => _familiars;

	public FamiliarStable(List<FamiliarStableData> familiars)
	{
		_familiars = familiars;
	}

	public float getFamiliarMult(FamiliarRef familiarRef, int adjust = 0)
	{
		int num = getFamiliarQty(familiarRef) + adjust;
		return VariableBook.familiarStableBonus * (float)num;
	}

	public int getFamiliarQty(FamiliarRef familiarRef)
	{
		if (familiarRef == null)
		{
			return 0;
		}
		foreach (FamiliarStableData familiar in _familiars)
		{
			if (familiar.familiarRef == familiarRef)
			{
				return familiar.qty;
			}
		}
		return 0;
	}

	public static FamiliarStable fromSFSObject(ISFSObject sfsob)
	{
		return new FamiliarStable(FamiliarStableData.listFromSFSObject(sfsob));
	}

	public Dictionary<string, object> familiarToStat(ItemData item)
	{
		bool flag = getFamiliarQty(item.itemRef as FamiliarRef) > 0;
		return new Dictionary<string, object>
		{
			{ "familiar_id", item.id },
			{
				"familiar_name",
				(item.itemRef as FamiliarRef).statName
			},
			{ "quantity", item.qty },
			{
				"upgrade_level",
				(item.itemRef as FamiliarRef).rank
			},
			{ "rarity", item.rarity },
			{ "is_stabled", flag }
		};
	}
}
