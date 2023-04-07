using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.fish;

public class FishBook
{
	private static Dictionary<int, FishRef> _fishes;

	public static int size => _fishes.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_fishes = new Dictionary<int, FishRef>();
		foreach (FishBookData.Fish item in XMLBook.instance.fishBook.lstFish)
		{
			_fishes.Add(item.id, new FishRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static FishRef Lookup(int id)
	{
		if (_fishes.ContainsKey(id))
		{
			return _fishes[id];
		}
		return null;
	}
}
