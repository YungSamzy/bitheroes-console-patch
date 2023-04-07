using System;
using System.Diagnostics;

namespace com.ultrabit.bitheroes.model.probability;

[DebuggerDisplay("{link} (ProbabilityLine)")]
public class ProbabilityLine : IEquatable<ProbabilityLine>, IComparable<ProbabilityLine>
{
	private float _perc;

	private string _link;

	public float perc => _perc;

	public string link => _link;

	public ProbabilityLine(float perc, string link)
	{
		_perc = perc;
		_link = link;
	}

	public bool Equals(ProbabilityLine other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(ProbabilityLine other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
