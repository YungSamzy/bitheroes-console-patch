using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.wallet;

[DebuggerDisplay("{name} (NFTItemRef)")]
public class NFTItemRef : IEquatable<NFTItemRef>, IComparable<NFTItemRef>
{
	private string _identifier;

	private string _source;

	private List<ItemData> _items;

	private string name
	{
		get
		{
			if (_items != null && _items.Count > 0)
			{
				return _identifier;
			}
			string text = _items[0].itemRef.name;
			if (_items.Count > 1)
			{
				text += $" [+{_items.Count - 1}]";
			}
			return text;
		}
	}

	public NFTItemRef(string identifier, string source, List<ItemData> items)
	{
		_identifier = identifier;
		_source = source;
		_items = items;
	}

	public bool Equals(NFTItemRef other)
	{
		if (other == null)
		{
			return false;
		}
		return getIdentifier().Equals(other.getIdentifier());
	}

	public int CompareTo(NFTItemRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return getIdentifier().CompareTo(other.getIdentifier());
	}

	public string getIdentifier()
	{
		return _identifier;
	}

	public string getSource()
	{
		return _source;
	}

	public List<ItemData> getItems()
	{
		return _items;
	}
}
