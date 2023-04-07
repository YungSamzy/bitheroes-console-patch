using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.fusion;

public class FusionBook
{
	private static Dictionary<int, FusionRef> _FUSIONS;

	public static int Size => _FUSIONS.Count;

	public static List<FusionRef> fusions => new List<FusionRef>(_FUSIONS.Values);

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_FUSIONS = new Dictionary<int, FusionRef>();
		foreach (FusionBookData.Fusion item in XMLBook.instance.fusionBook.lstFusion)
		{
			FusionRef fusionRef = new FusionRef(item.id, item);
			fusionRef.LoadDetails(item);
			if (fusionRef.tradeRef.resultItem.itemRef.itemType == 6)
			{
				(fusionRef.tradeRef.resultItem.itemRef as FamiliarRef).addFusionResult(fusionRef);
			}
			_FUSIONS.Add(item.id, fusionRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<FusionRef> GetAllFusionsRef()
	{
		return new List<FusionRef>(_FUSIONS.Values);
	}

	public static FusionRef Lookup(int id)
	{
		if (_FUSIONS.ContainsKey(id))
		{
			return _FUSIONS[id];
		}
		return null;
	}

	public static List<FusionRef> GetFusionRefs()
	{
		return new List<FusionRef>(_FUSIONS.Values);
	}

	public static FusionRef GetResultFusion(ItemRef itemRef)
	{
		foreach (KeyValuePair<int, FusionRef> fUSION in _FUSIONS)
		{
			if (fUSION.Value.tradeRef.resultItem.itemRef.Equals(itemRef))
			{
				return fUSION.Value;
			}
		}
		return null;
	}

	public static int GetRarityCount(RarityRef rarity)
	{
		int num = 0;
		foreach (KeyValuePair<int, FusionRef> fUSION in _FUSIONS)
		{
			if (fUSION.Value.rarityRef.name.ToLower().Equals(rarity.name.ToLower()))
			{
				num++;
			}
		}
		return num;
	}
}
