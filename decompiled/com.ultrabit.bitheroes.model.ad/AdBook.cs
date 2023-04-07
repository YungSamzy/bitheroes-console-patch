using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.ad;

public class AdBook
{
	public static List<AdItemRef> _adItems;

	public static List<AdItemRef> _adChestItems;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_adItems = new List<AdItemRef>();
		foreach (AdBookData.AdItem item in XMLBook.instance.adBook.lstAdItem)
		{
			_adItems.Add(new AdItemRef(item));
		}
		_adChestItems = new List<AdItemRef>();
		foreach (AdBookData.AdItem item2 in XMLBook.instance.adBook.lstAdChestItem)
		{
			_adChestItems.Add(new AdItemRef(item2));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static AdItemRef lookupTier(int tier)
	{
		foreach (AdItemRef adItem in _adItems)
		{
			if (adItem.maxTier == -1)
			{
				if (tier >= adItem.minTier)
				{
					return adItem;
				}
			}
			else if (tier >= adItem.minTier && tier <= adItem.maxTier)
			{
				return adItem;
			}
		}
		return null;
	}

	public static AdItemRef lookupChestTier(int tier)
	{
		foreach (AdItemRef adChestItem in _adChestItems)
		{
			if (adChestItem.maxTier == -1)
			{
				if (tier >= adChestItem.minTier)
				{
					return adChestItem;
				}
			}
			else if (tier >= adChestItem.minTier && tier <= adChestItem.maxTier)
			{
				return adChestItem;
			}
		}
		return null;
	}
}
