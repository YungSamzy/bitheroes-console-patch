using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.raid;

public class RaidBook
{
	private static Dictionary<int, RaidRef> _raids;

	public static int size => _raids.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_raids = new Dictionary<int, RaidRef>();
		foreach (RaidBookData.Raid item in XMLBook.instance.raidBook.lstRaid)
		{
			_raids.Add(item.id, new RaidRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<RaidRef> GetAllRaids()
	{
		return new List<RaidRef>(_raids.Values);
	}

	public static RaidRef LookUp(int id)
	{
		if (_raids.ContainsKey(id))
		{
			return _raids[id];
		}
		return null;
	}
}
