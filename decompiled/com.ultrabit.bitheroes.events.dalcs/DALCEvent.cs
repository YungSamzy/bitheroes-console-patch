using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.events.dalcs;

public class DALCEvent : CustomSFSXEvent
{
	private SFSObject _sfsob;

	public SFSObject sfsob => _sfsob;

	public DALCEvent(string type, SFSObject sfsob)
		: base(type)
	{
		_sfsob = sfsob;
	}
}
