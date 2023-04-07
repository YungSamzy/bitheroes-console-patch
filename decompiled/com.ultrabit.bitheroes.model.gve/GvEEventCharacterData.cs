using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.gve;

public class GvEEventCharacterData : EventCharacterData
{
	private int _guildRank;

	private int _guildPoints;

	private int _highest;

	private int _difficulty;

	public int guildRank => _guildRank;

	public int guildPoints => _guildPoints;

	public int highest => _highest;

	public int difficulty => _difficulty;

	public GvEEventCharacterData(int characterRank, int characterPoints, int guildRank, int guildPoints, int highest, int difficulty)
		: base(7, characterRank, characterPoints)
	{
		_guildRank = guildRank;
		_guildPoints = guildPoints;
		_highest = highest;
		_difficulty = difficulty;
	}

	public new static GvEEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve12");
		int int4 = sfsob.GetInt("eve13");
		int int5 = sfsob.GetInt("eve10");
		int int6 = sfsob.GetInt("eve11");
		return new GvEEventCharacterData(@int, int2, int3, int4, int5, int6)
		{
			zone = sfsob.GetInt("eve15")
		};
	}
}
