using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.rarity;

namespace com.ultrabit.bitheroes.model.probability;

[DebuggerDisplay("{name} (ProbabilityRarityRef)")]
public class ProbabilityRarityRef : IEquatable<ProbabilityRarityRef>, IComparable<ProbabilityRarityRef>
{
	private float _perc;

	private RarityRef _rarityRef;

	private string name => $"{_perc * 100f:0.00}% {_rarityRef.name}";

	public float perc => _perc;

	public RarityRef rarityRef => _rarityRef;

	public ProbabilityRarityRef(float perc, RarityRef rarityRef)
	{
		_perc = perc;
		_rarityRef = rarityRef;
	}

	public bool Equals(ProbabilityRarityRef other)
	{
		if (other == null)
		{
			return false;
		}
		if (perc.Equals(other.perc))
		{
			return rarityRef.Equals(other.rarityRef);
		}
		return false;
	}

	public int CompareTo(ProbabilityRarityRef other)
	{
		if (other == null)
		{
			return -1;
		}
		int num = rarityRef.CompareTo(other.rarityRef);
		if (num == 0)
		{
			return perc.CompareTo(other.perc);
		}
		return num;
	}
}
