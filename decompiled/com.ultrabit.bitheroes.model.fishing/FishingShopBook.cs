using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingShopBook
{
	private static List<FishingShopTabRef> _tabs;

	private static List<FishingShopItemRef> _items;

	public static int tabsSize => _tabs.Count;

	public static int itemsSize => _items.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_tabs = new List<FishingShopTabRef>();
		_items = new List<FishingShopItemRef>();
		int num = 0;
		foreach (FishingShopBookData.Tab item in XMLBook.instance.fishingShopBook.tabs.lstTab)
		{
			FishingShopTabRef fishingShopTabRef = new FishingShopTabRef(item.id, item.name, item.lstItem, num);
			_tabs.Add(fishingShopTabRef);
			num += fishingShopTabRef.items.Count;
			_items.AddRange(fishingShopTabRef.items);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static bool hasItem(ItemRef itemRef)
	{
		foreach (FishingShopItemRef item in _items)
		{
			if (item.itemData.itemRef.Equals(itemRef))
			{
				return true;
			}
		}
		return false;
	}

	public static FishingShopTabRef LookupTab(int id)
	{
		if (id >= 0 && id < _tabs.Count)
		{
			return _tabs[id];
		}
		return null;
	}

	public static FishingShopItemRef LookupItem(int id)
	{
		if (id >= 0 && id < _items.Count)
		{
			return _items[id];
		}
		return null;
	}
}
