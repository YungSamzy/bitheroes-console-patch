using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.guild;

[DebuggerDisplay("{name} (GuildShopRef)")]
public class GuildShopRef : IEquatable<GuildShopRef>, IComparable<GuildShopRef>
{
	private ItemRef _itemRef;

	private int _costHonor;

	private int _guildLvlReq;

	private string name => itemRef.name;

	public ItemRef itemRef => _itemRef;

	public int costHonor => _costHonor;

	public int guildLvlReq => _guildLvlReq;

	public GuildShopRef(ItemRef itemRef, int costHonor, int guildLvlReq)
	{
		_itemRef = itemRef;
		_costHonor = costHonor;
		_guildLvlReq = guildLvlReq;
	}

	public bool Equals(GuildShopRef other)
	{
		if (other == null)
		{
			return false;
		}
		return itemRef.Equals(other.itemRef);
	}

	public int CompareTo(GuildShopRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return itemRef.CompareTo(other.itemRef);
	}
}
