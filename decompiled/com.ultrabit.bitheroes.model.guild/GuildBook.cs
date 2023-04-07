using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildBook
{
	private static Dictionary<int, GuildItemRef> _items;

	private static Dictionary<int, GuildPerkRef> _perks;

	public static int sizePerks => _perks.Count;

	public static int sizeItems => _items.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_items = new Dictionary<int, GuildItemRef>();
		_perks = new Dictionary<int, GuildPerkRef>();
		foreach (GuildBookData.Item item in XMLBook.instance.guildBook.items.lstItem)
		{
			GuildItemRef guildItemRef = new GuildItemRef(item.id, item);
			guildItemRef.LoadDetails(item);
			_items.Add(item.id, guildItemRef);
		}
		foreach (GuildBookData.Perk item2 in XMLBook.instance.guildBook.perks.lstPerk)
		{
			GuildPerkRef guildPerkRef = new GuildPerkRef(item2.id, item2);
			guildPerkRef.LoadDetails(item2);
			_perks.Add(item2.id, guildPerkRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static GuildItemRef LookupItem(int id)
	{
		if (_items.ContainsKey(id))
		{
			return _items[id];
		}
		return null;
	}

	public static GuildPerkRef LookupPerk(int id)
	{
		if (_perks.ContainsKey(id))
		{
			return _perks[id];
		}
		return null;
	}
}
