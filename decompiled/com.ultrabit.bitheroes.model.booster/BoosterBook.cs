using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.booster;

public class BoosterBook
{
	public static List<BoosterRef> _boosters;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_boosters = new List<BoosterRef>();
		foreach (BoosterBookData.Booster lstBooster in XMLBook.instance.boosterBook.boosters.lstBoosters)
		{
			_boosters.Add(new BoosterRef(lstBooster));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static BoosterRef Lookup(int id)
	{
		foreach (BoosterRef booster in _boosters)
		{
			if (booster.id == id)
			{
				return booster;
			}
		}
		return null;
	}
}
