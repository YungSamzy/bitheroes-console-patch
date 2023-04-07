using System;
using System.Diagnostics;

namespace com.ultrabit.bitheroes.model.filter;

[DebuggerDisplay("{word} (FilterRef)")]
public class FilterRef : IEquatable<FilterRef>, IComparable<FilterRef>
{
	private string _word;

	private string _replacement;

	public string word => _word;

	public string replacement => _replacement;

	public FilterRef(string word, string replacement)
	{
		_word = word;
		_replacement = replacement;
	}

	public bool Equals(FilterRef other)
	{
		if (other == null)
		{
			return false;
		}
		return word.Equals(other.word);
	}

	public int CompareTo(FilterRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return word.CompareTo(other.word);
	}
}
