using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.adgor;

public class AdgorRef : BaseBook
{
	public const int TYPE_NONE = 0;

	public const int TYPE_ADGOR = 1;

	public const int TYPE_VIPGOR = 5;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		[""] = 0,
		["adgor"] = 1,
		["vipgor"] = 5
	};

	private List<AdgorItemRef> items;

	private List<int> adgorIDs;

	private List<int> vipgorIDs;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public AdgorRef(List<VariableBookData.Item> xml)
	{
		adgorIDs = new List<int>();
		vipgorIDs = new List<int>();
		items = new List<AdgorItemRef>();
		foreach (VariableBookData.Item item in xml)
		{
			AdgorItemRef adgorItemRef = new AdgorItemRef(item);
			if (TYPES.ContainsKey(adgorItemRef.type))
			{
				switch (TYPES[adgorItemRef.type])
				{
				case 1:
					adgorIDs.Add(adgorItemRef.id);
					break;
				case 5:
					vipgorIDs.Add(adgorItemRef.id);
					break;
				}
				items.Add(adgorItemRef);
			}
		}
	}

	public bool IsAdgorConsumable(int id)
	{
		foreach (AdgorItemRef item in items)
		{
			if (item.id == id)
			{
				return true;
			}
		}
		return false;
	}

	public List<int> GetAdGorIDs()
	{
		List<int> list = new List<int>();
		list.AddRange(adgorIDs);
		return list;
	}

	public List<int> getVipgorIDs()
	{
		return vipgorIDs;
	}

	public int getFirstVipGor()
	{
		return vipgorIDs[0];
	}

	public int GetFirstAdGor()
	{
		return adgorIDs[0];
	}
}
