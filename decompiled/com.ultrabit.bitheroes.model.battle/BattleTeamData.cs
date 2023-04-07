using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.battle;

public class BattleTeamData
{
	private bool _attacker;

	private int _poolCurrent;

	private int _poolTotal;

	public bool attacker => _attacker;

	public int poolCurrent => _poolCurrent;

	public int poolTotal => _poolTotal;

	public BattleTeamData(bool attacker, int poolCurrent, int poolTotal)
	{
		_attacker = attacker;
		_poolCurrent = poolCurrent;
		_poolTotal = poolTotal;
	}

	public void setPoolCurrent(int pool)
	{
		_poolCurrent = pool;
	}

	public static BattleTeamData fromSFSObject(ISFSObject sfsob)
	{
		if (sfsob == null || !sfsob.ContainsKey("bat44"))
		{
			return null;
		}
		bool @bool = sfsob.GetBool("bat44");
		int @int = sfsob.GetInt("bat45");
		int int2 = sfsob.GetInt("bat46");
		return new BattleTeamData(@bool, @int, int2);
	}
}
