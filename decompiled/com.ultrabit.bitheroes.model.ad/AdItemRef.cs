using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.probability;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.ad;

[DebuggerDisplay("{name} (AdItemRef)")]
public class AdItemRef : IEquatable<AdItemRef>, IComparable<AdItemRef>
{
	private int _minTier;

	private int _maxTier;

	private List<ProbabilityLine> _items;

	public int minTier => _minTier;

	public int maxTier => _maxTier;

	public List<ProbabilityLine> items => _items;

	public AdItemRef(AdBookData.AdItem adItem)
	{
		_minTier = int.Parse(adItem.minTier);
		_maxTier = ((adItem.maxTier != null) ? int.Parse(adItem.maxTier) : (-1));
		if (adItem.lstItem == null)
		{
			return;
		}
		_items = new List<ProbabilityLine>();
		foreach (AdBookData.Item item in adItem.lstItem)
		{
			_items.Add(new ProbabilityLine(item.perc, item.link));
		}
	}

	public bool Equals(AdItemRef other)
	{
		if (other == null)
		{
			return false;
		}
		bool flag = minTier.Equals(other.minTier) && minTier.Equals(other.minTier);
		if (!flag)
		{
			return false;
		}
		foreach (ProbabilityLine item in items)
		{
			if (!other.items.Exists((ProbabilityLine a) => a.Equals(item)))
			{
				return false;
			}
		}
		return flag;
	}

	public int CompareTo(AdItemRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = minTier.CompareTo(other.minTier);
		if (num == 0)
		{
			return maxTier.CompareTo(other.maxTier);
		}
		return num;
	}
}
