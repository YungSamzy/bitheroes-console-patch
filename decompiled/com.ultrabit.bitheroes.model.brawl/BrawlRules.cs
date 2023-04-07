using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlRules
{
	private bool _public;

	private int _minStats;

	private int _maxStats;

	public BrawlRules(bool pub, int minStats = 0, int maxStats = int.MaxValue)
	{
		_public = pub;
		_minStats = minStats;
		_maxStats = maxStats;
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutBool("bra9", _public);
		sfsob.PutInt("bra10", _minStats);
		sfsob.PutInt("bra11", _maxStats);
		return sfsob;
	}

	public static BrawlRules fromSFSObject(ISFSObject sfsob)
	{
		bool @bool = sfsob.GetBool("bra9");
		int @int = sfsob.GetInt("bra10");
		int int2 = sfsob.GetInt("bra11");
		return new BrawlRules(@bool, @int, int2);
	}

	public bool getPublic()
	{
		return _public;
	}
}
