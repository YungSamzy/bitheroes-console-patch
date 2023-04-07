using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.rift;

public class RiftEventCharacterData : EventCharacterData
{
	private int _highest;

	private int _difficulty;

	private int _zone;

	public int highest => _highest;

	public int difficulty => _difficulty;

	public RiftEventCharacterData(int rank, int points, int highest, int difficulty)
		: base(2, rank, points)
	{
		_highest = highest;
		_difficulty = difficulty;
		_zone = -1;
	}

	public new static RiftEventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		D.Log("RiftEventCharacterData fromSFSObject");
		int @int = sfsob.GetInt("eve1");
		int int2 = sfsob.GetInt("eve2");
		int int3 = sfsob.GetInt("eve10");
		int int4 = sfsob.GetInt("eve11");
		return new RiftEventCharacterData(@int, int2, int3, int4)
		{
			zone = sfsob.GetInt("eve15")
		};
	}
}
