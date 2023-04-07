using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.rift;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.events;

public class EventCharacterData
{
	private int _type;

	private int _rank;

	private int _points;

	private int _zone;

	public int type => _type;

	public int rank => _rank;

	public int points => _points;

	public int zone
	{
		get
		{
			return _zone;
		}
		set
		{
			_zone = value;
		}
	}

	public EventCharacterData(int type, int rank, int points)
	{
		_type = type;
		_rank = rank;
		_points = points;
	}

	public static EventCharacterData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("eve9");
		switch (@int)
		{
		case 2:
			return RiftEventCharacterData.fromSFSObject(sfsob);
		case 3:
			return GauntletEventCharacterData.fromSFSObject(sfsob);
		case 4:
			return GvGEventCharacterData.fromSFSObject(sfsob);
		case 6:
			return FishingEventCharacterData.fromSFSObject(sfsob);
		case 7:
			return GvEEventCharacterData.fromSFSObject(sfsob);
		case 5:
			return InvasionEventCharacterData.fromSFSObject(sfsob);
		default:
		{
			int int2 = sfsob.GetInt("eve1");
			int int3 = sfsob.GetInt("eve2");
			return new EventCharacterData(@int, int2, int3)
			{
				zone = sfsob.GetInt("eve15")
			};
		}
		}
	}
}
