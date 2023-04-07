using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.gvg;

public class GvGEventCharacterData : EventCharacterData
{
	private int _guildRank;

	private int _guildPoints;

	public int guildRank => _guildRank;

	public int guildPoints => _guildPoints;

	public GvGEventCharacterData(int characterRank, int characterPoints, int guildRank, int guildPoints)
		: base(4, characterRank, characterPoints)
	{
		_guildRank = guildRank;
		_guildPoints = guildPoints;
	}

	public new static GvGEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		D.Log("GvGEventCharacterData fromSFSObject");
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve12");
		int int4 = sfsob.GetInt("eve13");
		return new GvGEventCharacterData(@int, int2, int3, int4)
		{
			zone = sfsob.GetInt("eve15")
		};
	}
}
