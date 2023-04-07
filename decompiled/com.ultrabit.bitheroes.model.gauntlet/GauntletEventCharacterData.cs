using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.gauntlet;

public class GauntletEventCharacterData : EventCharacterData
{
	private int _highest;

	private int _difficulty;

	private int _zone;

	public int highest => _highest;

	public int difficulty => _difficulty;

	public GauntletEventCharacterData(int rank, int points, int highest, int difficulty)
		: base(3, rank, points)
	{
		_highest = highest;
		_difficulty = difficulty;
		_zone = -1;
	}

	public new static GauntletEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve10");
		int int4 = sfsob.GetInt("eve11");
		return new GauntletEventCharacterData(@int, int2, int3, int4)
		{
			zone = sfsob.GetInt("eve15")
		};
	}
}
