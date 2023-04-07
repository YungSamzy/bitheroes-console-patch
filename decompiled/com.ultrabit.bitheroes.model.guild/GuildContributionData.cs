using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildContributionData
{
	private int _charID;

	private int _guildID;

	private string _name;

	private long _exp;

	public int charID => _charID;

	public int guildID => _guildID;

	public string name => _name;

	public long exp => _exp;

	public GuildContributionData(int charID, int guildID, string name, long exp)
	{
		_charID = charID;
		_guildID = guildID;
		_name = name;
		_exp = exp;
	}

	public string getNameColor(int guildID)
	{
		if (guildID == _guildID)
		{
			return "#FFFF00";
		}
		return "#666666";
	}

	public static GuildContributionData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		int int2 = sfsob.GetInt("gui0");
		string utfString = sfsob.GetUtfString("cha2");
		long @long = sfsob.GetLong("gui8");
		return new GuildContributionData(@int, int2, utfString, @long);
	}

	public static List<GuildContributionData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("gui5");
		List<GuildContributionData> list = new List<GuildContributionData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
