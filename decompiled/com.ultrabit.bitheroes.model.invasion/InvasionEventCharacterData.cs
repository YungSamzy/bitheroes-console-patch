using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.invasion;

public class InvasionEventCharacterData : EventCharacterData
{
	private int _highest;

	private int _difficulty;

	private long _communityPoints;

	public int highest => _highest;

	public int difficulty => _difficulty;

	public long communityPoints => _communityPoints;

	public InvasionEventCharacterData(int rank, int points, int highest, int difficulty, long communityPoints)
		: base(5, rank, points)
	{
		_highest = highest;
		_difficulty = difficulty;
		_communityPoints = communityPoints;
	}

	public new static InvasionEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		D.Log("InvasionEventCharacterData fromSFSObject");
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve10");
		int int4 = sfsob.GetInt("eve11");
		long @long = sfsob.GetLong("inv2");
		return new InvasionEventCharacterData(@int, int2, int3, int4, @long)
		{
			zone = sfsob.GetInt("eve15")
		};
	}
}
