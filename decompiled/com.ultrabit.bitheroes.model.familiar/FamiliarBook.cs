using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.familiar;

public class FamiliarBook
{
	private static Dictionary<int, FamiliarRef> _FAMILIARS;

	public static int count => _FAMILIARS.Count;

	public static int size => _FAMILIARS.Count;

	public static object StreamWritter { get; private set; }

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_FAMILIARS = new Dictionary<int, FamiliarRef>();
		FamiliarBookData.Familiar[] array = XMLBook.instance.familiarBook.lstFamiliar.ToArray();
		foreach (FamiliarBookData.Familiar familiar in array)
		{
			if (!_FAMILIARS.ContainsKey(familiar.id))
			{
				_FAMILIARS.Add(familiar.id, new FamiliarRef(familiar.id, familiar));
			}
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<string> CheckAsset()
	{
		List<string> list = new List<string>();
		FamiliarRef[] array = GetCompleteFamiliarList().ToArray();
		foreach (FamiliarRef familiarRef in array)
		{
			if (familiarRef != null && familiarRef.icon != null && !familiarRef.icon.Trim().Equals("") && GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.FAMILIAR_ICON, familiarRef.icon) == null)
			{
				string item = "Missing Familiar Icon: " + familiarRef.icon + " (" + familiarRef.name + ")";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static FamiliarRef Lookup(int id)
	{
		if (_FAMILIARS.ContainsKey(id))
		{
			return _FAMILIARS[id];
		}
		return null;
	}

	public static int GetOwnedCount()
	{
		int num = 0;
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			if (fAMILIAR.Value.obtainable && GameData.instance.PROJECT.character.inventory.hasOwnedItem(fAMILIAR.Value))
			{
				num++;
			}
		}
		return num;
	}

	public static int GetRarityCount(RarityRef rarity)
	{
		int num = 0;
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			if (fAMILIAR.Value.obtainable && fAMILIAR.Value.rarityRef.id == rarity.id && fAMILIAR.Value.rarityRef.link == rarity.link)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetOwnedFusionCount()
	{
		int num = 0;
		if (GameData.instance.PROJECT.character == null)
		{
			return num;
		}
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			if (fAMILIAR.Value.obtainable && fAMILIAR.Value.isFusionFamiliar() && GameData.instance.PROJECT.character.inventory.hasOwnedItem(fAMILIAR.Value))
			{
				num++;
			}
		}
		return num;
	}

	public static List<FamiliarRef> GetCompleteFamiliarList()
	{
		List<FamiliarRef> list = new List<FamiliarRef>();
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			list.Add(fAMILIAR.Value);
		}
		return list;
	}

	public static int GetFamiliarIdFromNPCLink(string link)
	{
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			if (!(fAMILIAR.Value == null) && fAMILIAR.Value.obtainable && fAMILIAR.Value.npc == link)
			{
				return fAMILIAR.Value.id;
			}
		}
		return 0;
	}

	public static void preloadFamiliarAssets()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<int, FamiliarRef> fAMILIAR in _FAMILIARS)
		{
			if (!(fAMILIAR.Value == null) && fAMILIAR.Value.intro && fAMILIAR.Value.displayRef != null)
			{
				list.Add(fAMILIAR.Value.displayRef.assetURL);
			}
		}
		GameData.instance.SAVE_STATE.familiarAssets = list;
	}

	public static List<FamiliarRef> LookupIDs(List<int> ids)
	{
		List<FamiliarRef> list = new List<FamiliarRef>();
		if (ids == null || ids.Count <= 0)
		{
			return list;
		}
		foreach (int id in ids)
		{
			FamiliarRef familiarRef = Lookup(id);
			if (familiarRef != null)
			{
				list.Add(familiarRef);
			}
		}
		return list;
	}
}
