using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.dungeon;

public class DungeonBook : MonoBehaviour
{
	private static List<DungeonTreasureRef> _treasures;

	private static List<DungeonShrineRef> _shrines;

	private static List<DungeonLootableRef> _lootables;

	private static List<DungeonMerchantRef> _merchants;

	private static List<DungeonAdRef> _ads;

	private static Dictionary<string, DungeonRef> _dungeons;

	private static List<string> _dungeonIndex;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_treasures = new List<DungeonTreasureRef>();
		_shrines = new List<DungeonShrineRef>();
		_lootables = new List<DungeonLootableRef>();
		_merchants = new List<DungeonMerchantRef>();
		_ads = new List<DungeonAdRef>();
		_dungeons = new Dictionary<string, DungeonRef>();
		_dungeonIndex = new List<string>();
		int num = 0;
		foreach (DungeonBookData.Treasure item in XMLBook.instance.dungeonBook.lstTreasure)
		{
			DungeonTreasureRef dungeonTreasureRef = new DungeonTreasureRef(num, item);
			dungeonTreasureRef.LoadDetails(item);
			_treasures.Add(dungeonTreasureRef);
			num++;
		}
		num = 0;
		foreach (DungeonBookData.Shrine item2 in XMLBook.instance.dungeonBook.lstShrine)
		{
			DungeonShrineRef dungeonShrineRef = new DungeonShrineRef(num, item2);
			dungeonShrineRef.LoadDetails(item2);
			_shrines.Add(dungeonShrineRef);
			num++;
		}
		num = 0;
		foreach (DungeonBookData.Lootable item3 in XMLBook.instance.dungeonBook.lstLootable)
		{
			DungeonLootableRef dungeonLootableRef = new DungeonLootableRef(num, item3);
			dungeonLootableRef.LoadDetails(item3);
			_lootables.Add(dungeonLootableRef);
			num++;
		}
		num = 0;
		foreach (DungeonBookData.Merchant item4 in XMLBook.instance.dungeonBook.lstMerchant)
		{
			DungeonMerchantRef dungeonMerchantRef = new DungeonMerchantRef(num, item4);
			dungeonMerchantRef.LoadDetails(item4);
			_merchants.Add(dungeonMerchantRef);
			num++;
		}
		num = 0;
		foreach (DungeonBookData.Ad item5 in XMLBook.instance.dungeonBook.lstAd)
		{
			DungeonAdRef dungeonAdRef = new DungeonAdRef(num, item5);
			dungeonAdRef.LoadDetails(item5);
			_ads.Add(dungeonAdRef);
			num++;
		}
		foreach (DungeonBookData.Dungeon item6 in XMLBook.instance.dungeonBook.lstDungeon)
		{
			DungeonRef value = new DungeonRef(_dungeonIndex.Count, item6);
			_dungeons.Add(item6.link, value);
			_dungeonIndex.Add(item6.link);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static DungeonRef Lookup(int id)
	{
		if (id >= 0 && id < _dungeonIndex.Count)
		{
			return LookupLink(_dungeonIndex[id]);
		}
		return null;
	}

	public static DungeonRef LookupLink(string link)
	{
		if (_dungeons.ContainsKey(link))
		{
			return _dungeons[link];
		}
		D.LogError("Cannot find dungeonRef for Link: " + link);
		return null;
	}

	public static DungeonTreasureRef LookupTreasure(int id)
	{
		if (id >= 0 && id < _treasures.Count)
		{
			return _treasures[id];
		}
		return null;
	}

	public static DungeonShrineRef LookupShrine(int id)
	{
		if (id >= 0 && id < _shrines.Count)
		{
			return _shrines[id];
		}
		return null;
	}

	public static DungeonLootableRef LookupLootable(int id)
	{
		if (id > 0 || id < _lootables.Count)
		{
			return _lootables[id];
		}
		return null;
	}

	public static DungeonMerchantRef LookupMerchant(int id)
	{
		if (id >= 0 || id < _merchants.Count)
		{
			return _merchants[id];
		}
		return null;
	}

	public static DungeonAdRef LookupAd(int id)
	{
		if (id >= 0 && id < _ads.Count)
		{
			return _ads[id];
		}
		return null;
	}
}
