using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.invasion;

[DebuggerDisplay("{link} (InvasionEventPoolRef)")]
public class InvasionEventPoolRef : IEquatable<InvasionEventPoolRef>, IComparable<InvasionEventPoolRef>
{
	private string _link;

	private List<InvasionEventLevelRef> _levels;

	public string link => _link;

	public List<InvasionEventLevelRef> levels => _levels;

	public InvasionEventPoolRef(BaseEventBookData.Pool data)
	{
		_link = ((data.link != null) ? data.link.ToLowerInvariant() : null);
		_levels = new List<InvasionEventLevelRef>();
		foreach (BaseEventBookData.Level item in data.lstLevel)
		{
			_levels.Add(new InvasionEventLevelRef(item));
		}
	}

	public bool Equals(InvasionEventPoolRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(InvasionEventPoolRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
