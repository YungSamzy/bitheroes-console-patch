using System.Collections.Generic;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.leaderboard;

public class LeaderboardData
{
	public const int TYPE_PLAYER = 0;

	public const int TYPE_GUILD = 1;

	private int _id;

	private int _type;

	private string _name;

	private string _guildInitials;

	private int _rank;

	private int _value;

	private CharacterData _data;

	public string parsedName => Util.ParseName(_name, _guildInitials);

	public int id => _id;

	public int type => _type;

	public string name => _name;

	public int rank => _rank;

	public int value => _value;

	public CharacterData data => _data;

	public LeaderboardData(int id, int type, string name, string guildInitials, int rank, int value, CharacterData data)
	{
		_id = id;
		_type = type;
		_name = name;
		_guildInitials = guildInitials;
		_rank = rank;
		_value = value;
		_data = data;
	}

	public static LeaderboardData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("lea5");
		int int2 = sfsob.GetInt("lea6");
		string utfString = sfsob.GetUtfString("lea7");
		utfString = AdGor.ConvertNameplateAS3ToUnity(utfString);
		string guildInitials = (sfsob.ContainsKey("gui3") ? sfsob.GetUtfString("gui3") : null);
		int int3 = sfsob.GetInt("lea2");
		int int4 = sfsob.GetInt("lea3");
		CharacterData characterData = CharacterData.fromSFSObject(sfsob);
		return new LeaderboardData(@int, int2, utfString, guildInitials, int3, int4, characterData);
	}

	public static List<LeaderboardData> listFromSFSObject(ISFSObject sfsob)
	{
		List<LeaderboardData> list = new List<LeaderboardData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("lea4");
		if (sFSArray != null)
		{
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				list.Add(fromSFSObject(sFSObject));
			}
		}
		return list;
	}
}
