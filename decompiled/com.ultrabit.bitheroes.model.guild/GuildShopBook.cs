using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildShopBook
{
	private static List<GuildShopRef> _items;

	public static int size => _items.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_items = new List<GuildShopRef>();
		foreach (GuildShopBookData.Item item in XMLBook.instance.guildShopBook.lstItem)
		{
			_items.Add(new GuildShopRef(ItemBook.Lookup(item.id, item.type), item.costHonor, item.guildLvlReq));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GuildShopRef Lookup(int index)
	{
		if (index >= 0 && index < _items.Count)
		{
			return _items[index];
		}
		return null;
	}

	public static GuildShopRef LookupItem(ItemRef itemRef)
	{
		foreach (GuildShopRef item in _items)
		{
			if (item.itemRef.Equals(itemRef))
			{
				return item;
			}
		}
		return null;
	}
}
