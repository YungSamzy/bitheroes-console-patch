using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildHallBook
{
	private static List<GuildHallCosmeticRef> _COSMETICS;

	private static List<GuildHallCosmeticTypeRef> _COSMETICS_TYPES;

	public static int sizeCosmeticTypes => _COSMETICS_TYPES.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_COSMETICS = new List<GuildHallCosmeticRef>();
		_COSMETICS_TYPES = new List<GuildHallCosmeticTypeRef>();
		foreach (GuildHallBookData.Type item2 in XMLBook.instance.guildHallBook.cosmetics.lstType)
		{
			GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = new GuildHallCosmeticTypeRef(item2.id, item2);
			guildHallCosmeticTypeRef.LoadDetails(item2);
			_COSMETICS_TYPES.Add(guildHallCosmeticTypeRef);
		}
		foreach (GuildHallBookData.Cosmetic item3 in XMLBook.instance.guildHallBook.cosmetics.lstCosmetic)
		{
			GuildHallCosmeticRef item = new GuildHallCosmeticRef(item3.id, item3);
			_COSMETICS.Add(item);
		}
		for (int i = 0; i < _COSMETICS.Count; i++)
		{
			_COSMETICS[i].Init();
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<GuildHallCosmeticRef> GetOrderedCosmetics(int type = -1, int level = -1)
	{
		List<GuildHallCosmeticRef> list = new List<GuildHallCosmeticRef>();
		foreach (GuildHallCosmeticRef cOSMETIC in _COSMETICS)
		{
			if ((type < 0 || cOSMETIC.typeRef.id == type) && (level < 0 || cOSMETIC.guildLvlReq <= level))
			{
				list.Add(cOSMETIC);
			}
		}
		return Util.SortVector(list, new string[3] { "guildLvlReq", "id", "type" });
	}

	public static GuildHallCosmeticTypeRef LookupCosmeticType(string link)
	{
		foreach (GuildHallCosmeticTypeRef cOSMETICS_TYPE in _COSMETICS_TYPES)
		{
			if (cOSMETICS_TYPE.link.ToLowerInvariant() == link.ToLowerInvariant())
			{
				return cOSMETICS_TYPE;
			}
		}
		D.LogWarning("WARNING: Unable to locate cosmetic type '" + link + "'");
		return null;
	}

	public static GuildHallCosmeticTypeRef LookupCosmeticTypeID(int type)
	{
		foreach (GuildHallCosmeticTypeRef cOSMETICS_TYPE in _COSMETICS_TYPES)
		{
			if (cOSMETICS_TYPE.id == type)
			{
				return cOSMETICS_TYPE;
			}
		}
		D.LogWarning("WARNING: Unable to locate cosmetic type id " + type);
		return null;
	}

	public static GuildHallCosmeticRef LookupCosmetic(int id, int type)
	{
		foreach (GuildHallCosmeticRef cOSMETIC in _COSMETICS)
		{
			if (cOSMETIC.id == id && cOSMETIC.typeRef.id == type)
			{
				return cOSMETIC;
			}
		}
		D.LogWarning("WARNING: Unable to locate cosmetic " + id + " / " + type);
		return null;
	}
}
