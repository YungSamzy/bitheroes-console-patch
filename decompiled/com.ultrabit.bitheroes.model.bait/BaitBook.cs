using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.bait;

public class BaitBook
{
	private static Dictionary<int, BaitRef> _baits;

	public static int size => _baits.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_baits = new Dictionary<int, BaitRef>();
		foreach (BaitBookData.Bait item in XMLBook.instance.baitBook.lstBait)
		{
			BaitRef baitRef = new BaitRef(item.id, item);
			baitRef.LoadDetails(item);
			_baits.Add(item.id, baitRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static BaitRef Lookup(int id)
	{
		if (_baits.ContainsKey(id))
		{
			return _baits[id];
		}
		return null;
	}
}
