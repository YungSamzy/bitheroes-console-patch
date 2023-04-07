using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildInfo
{
	private int _id;

	private string _name;

	private string _initials;

	private int _members;

	private int _level;

	private bool _open;

	public int id => _id;

	public string name => _name;

	public string initials => _initials;

	public int members => _members;

	public int level => _level;

	public bool open => _open;

	public GuildInfo(int id, string name, string initials, int members, int level, bool open)
	{
		_id = id;
		_name = name;
		_initials = initials;
		_members = members;
		_level = level;
		_open = open;
	}

	public static GuildInfo fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("gui0");
		string utfString = sfsob.GetUtfString("gui2");
		string utfString2 = sfsob.GetUtfString("gui3");
		int int2 = sfsob.GetInt("gui5");
		int int3 = sfsob.GetInt("gui7");
		bool @bool = sfsob.GetBool("gui20");
		return new GuildInfo(@int, utfString, utfString2, int2, int3, @bool);
	}

	public static List<GuildInfo> listFromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui9"))
		{
			return null;
		}
		List<GuildInfo> list = new List<GuildInfo>();
		ISFSArray sFSArray = sfsob.GetSFSArray("gui9");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			GuildInfo item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
