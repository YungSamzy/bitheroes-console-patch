using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildHallData
{
	private int _guildID;

	private List<int> _cosmetics;

	private GuildItemRef _throne;

	private GuildItemRef _leftRoom;

	private GuildItemRef _rightRoom;

	public int guildID => _guildID;

	public List<int> cosmetics => _cosmetics;

	public GuildItemRef throne => _throne;

	public GuildItemRef leftRoom => _leftRoom;

	public GuildItemRef rightRoom => _rightRoom;

	public GuildHallData(int guildID, List<int> cosmetics, GuildItemRef throne, GuildItemRef leftRoom, GuildItemRef rightRoom)
	{
		_guildID = guildID;
		_cosmetics = cosmetics;
		_throne = throne;
		_leftRoom = leftRoom;
		_rightRoom = rightRoom;
	}

	public void setCosmetic(int type, int cosmetic)
	{
		while (_cosmetics.Count < type)
		{
			_cosmetics.Add(0);
		}
		_cosmetics[type] = cosmetic;
	}

	public int getCosmetic(int type)
	{
		if (type < 0 || type >= _cosmetics.Count)
		{
			return 0;
		}
		return _cosmetics[type];
	}

	public bool getEquipped(GuildItemRef itemRef)
	{
		foreach (GuildItemRef item in getItems())
		{
			if (item == itemRef)
			{
				return true;
			}
		}
		return false;
	}

	public List<GuildItemRef> getItems()
	{
		List<GuildItemRef> list = new List<GuildItemRef>();
		if (throne != null)
		{
			list.Add(throne);
		}
		if (leftRoom != null)
		{
			list.Add(leftRoom);
		}
		if (rightRoom != null)
		{
			list.Add(rightRoom);
		}
		return list;
	}

	public static GuildHallData fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("gui0");
		List<int> list = Util.arrayToIntegerVector(sfsob.GetIntArray("gui12"));
		GuildItemRef guildItemRef = GuildBook.LookupItem(sfsob.GetInt("gui13"));
		GuildItemRef guildItemRef2 = GuildBook.LookupItem(sfsob.GetInt("gui17"));
		GuildItemRef guildItemRef3 = GuildBook.LookupItem(sfsob.GetInt("gui18"));
		return new GuildHallData(@int, list, guildItemRef, guildItemRef2, guildItemRef3);
	}
}
