using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.fishing;

public class FishingEventCharacterData : EventCharacterData
{
	private int _highest;

	public int highest => _highest;

	public FishingEventCharacterData(int rank, int points, int highest)
		: base(6, rank, points)
	{
		_highest = highest;
	}

	public new static FishingEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve10");
		return new FishingEventCharacterData(@int, int2, int3);
	}
}
