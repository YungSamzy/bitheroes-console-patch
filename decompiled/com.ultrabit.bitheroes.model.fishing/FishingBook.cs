using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingBook
{
	private static Dictionary<string, FishingBarRef> _bars;

	private static Dictionary<int, FishingItemRef> _items;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_bars = new Dictionary<string, FishingBarRef>();
		_items = new Dictionary<int, FishingItemRef>();
		foreach (FishingBookData.Bar item in XMLBook.instance.fishingBook.bars.lstBar)
		{
			_bars.Add(item.link, new FishingBarRef(item.id, item));
		}
		int num = 0;
		foreach (FishingBookData.Item lstItem in XMLBook.instance.fishingBook.lstItems)
		{
			_items.Add(num, new FishingItemRef(lstItem.id, lstItem));
			num++;
		}
		yield return null;
		if (onUpdatedProgress != null && onUpdatedProgress.Target != null && !onUpdatedProgress.Target.Equals(null))
		{
			onUpdatedProgress(XMLBook.instance.UpdateProgress());
		}
	}

	public static FishingItemRef LookupItem(int id)
	{
		if (_items.ContainsKey(id))
		{
			return _items[id];
		}
		return null;
	}

	public static FishingBarRef LookupBar(string link)
	{
		if (link != null && _bars.ContainsKey(link))
		{
			return _bars[link];
		}
		return null;
	}
}
