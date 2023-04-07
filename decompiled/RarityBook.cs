using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

public class RarityBook
{
	private static Dictionary<string, RarityRef> _rarities;

	private static List<string> _rarityIndex;

	public static int size => _rarities.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_rarities = new Dictionary<string, RarityRef>();
		_rarityIndex = new List<string>();
		int num = 0;
		foreach (RarityBookData.Rarity item in XMLBook.instance.rarityBook.lstRarity)
		{
			_rarities.Add(item.link.ToLower(), new RarityRef(num, item));
			_rarityIndex.Add(item.link);
			num++;
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static RarityRef Lookup(string rarity)
	{
		if (rarity == null)
		{
			return null;
		}
		rarity = rarity.ToLower();
		if (_rarities.ContainsKey(rarity))
		{
			return _rarities[rarity];
		}
		return null;
	}

	public static int RarityCount()
	{
		return size;
	}

	public static RarityRef LookupID(int id)
	{
		if (id >= 0 && id < _rarityIndex.Count)
		{
			return _rarities[_rarityIndex[id]];
		}
		return null;
	}

	public static List<RarityRef> LookupLinks(List<string> links)
	{
		List<RarityRef> list = new List<RarityRef>();
		foreach (string link in links)
		{
			RarityRef rarityRef = Lookup(link);
			if (rarityRef != null)
			{
				list.Add(rarityRef);
			}
		}
		return list;
	}
}
