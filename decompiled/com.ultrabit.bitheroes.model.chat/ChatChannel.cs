using System;
using System.Diagnostics;

namespace com.ultrabit.bitheroes.model.chat;

[DebuggerDisplay("{name} (ChatChannel)")]
public class ChatChannel : IEquatable<ChatChannel>, IComparable<ChatChannel>
{
	private int _id;

	private string _name;

	public int id => _id;

	public string name => _name;

	public ChatChannel(int id, string name)
	{
		_id = id;
		_name = name;
	}

	public bool Equals(ChatChannel other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(ChatChannel other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
