using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.zone;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneBook
{
	private static Dictionary<int, ZoneRef> _zones;

	private static List<ZoneDifficultyRef> _difficulties;

	public static int size => _zones.Count;

	public static int zoneCount => _zones.Values.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_zones = new Dictionary<int, ZoneRef>();
		_difficulties = new List<ZoneDifficultyRef>();
		foreach (ZoneBookData.DifficultyMode lstDifficulty in XMLBook.instance.zoneBook.difficultyModes.lstDifficulties)
		{
			_difficulties.Add(new ZoneDifficultyRef(lstDifficulty.id, lstDifficulty));
		}
		foreach (ZoneXMLData dictZone in XMLBook.instance.zoneBook.dictZones)
		{
			_zones.Add(dictZone.zone.id, new ZoneRef(dictZone.zone.id, dictZone.zone));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static ZoneRef Lookup(int id)
	{
		if (_zones.ContainsKey(id))
		{
			return _zones[id];
		}
		return null;
	}

	public static ZoneDifficultyRef GetFirstDifficultyRef()
	{
		return LookupDifficulty(0);
	}

	public static ZoneDifficultyRef LookupDifficulty(int difficulty)
	{
		return _difficulties[difficulty];
	}

	public static ZoneDifficultyRef LookupDifficultyLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (ZoneDifficultyRef difficulty in _difficulties)
		{
			if (difficulty.link.ToLower().Equals(link.ToLower()))
			{
				return difficulty;
			}
		}
		Debug.LogWarning("WARNING: Not found difficulty for link: " + link + " ZoneBook::LookupDifficultyLink");
		return null;
	}

	public static ZoneRef GetStarterZone()
	{
		foreach (KeyValuePair<int, ZoneRef> zone in _zones)
		{
			if (zone.Value.starter)
			{
				return zone.Value;
			}
		}
		Debug.LogWarning("WARNING: Not default zone has been found, ZoneBook::GetStarterZone");
		return null;
	}

	public static List<int> GetDifficultyList(string name)
	{
		List<int> list = new List<int>();
		if (name != null)
		{
			string[] array = name.Split(',');
			foreach (string link in array)
			{
				list.Add(LookupDifficultyLink(link).id);
			}
		}
		return list;
	}

	public static ZoneNodeDifficultyRef GetDungeonZoneNodeDifficultyRef(DungeonRef dungeonRef)
	{
		foreach (KeyValuePair<int, ZoneRef> zone in _zones)
		{
			foreach (ZoneNodeRef node in zone.Value.nodes)
			{
				foreach (ZoneNodeDifficultyRef difficulty in node.difficulties)
				{
					if (difficulty.dungeonRef.link.Equals(dungeonRef.link))
					{
						return difficulty;
					}
				}
			}
		}
		return null;
	}

	public static int GetLastZoneID()
	{
		return _zones.Count;
	}
}
