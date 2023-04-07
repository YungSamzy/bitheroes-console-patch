using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.probability;

[DebuggerDisplay("{name} (ProbabilityRef)")]
public class ProbabilityRef : BaseRef, IEquatable<ProbabilityRef>, IComparable<ProbabilityRef>
{
	private List<ProbabilityRarityRef> _rarities;

	private List<ProbabilityLine> _lines;

	public List<ProbabilityRarityRef> rarities => _rarities;

	public List<ProbabilityLine> lines => _lines;

	public ProbabilityRef(int id, ProbabilityBookData.Probability probability)
		: base(id)
	{
		_rarities = new List<ProbabilityRarityRef>();
		foreach (ProbabilityBookData.Rarity item in probability.lstRarity)
		{
			_rarities.Add(new ProbabilityRarityRef(item.perc, RarityBook.Lookup(item.link)));
		}
		if (probability.lstLine != null && probability.lstLine.Count > 0)
		{
			_lines = new List<ProbabilityLine>();
			foreach (ProbabilityBookData.Line item2 in probability.lstLine)
			{
				_lines.Add(new ProbabilityLine(item2.perc, item2.link));
			}
		}
		LoadDetails(probability);
	}

	public bool Equals(ProbabilityRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(ProbabilityRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
