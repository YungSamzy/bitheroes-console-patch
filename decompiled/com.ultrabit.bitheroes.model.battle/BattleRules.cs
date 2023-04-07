using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.battle;

public class BattleRules
{
	private bool _allowSwitch;

	public bool allowSwitch => _allowSwitch;

	public BattleRules(bool allowSwitch)
	{
		_allowSwitch = allowSwitch;
	}

	public static BattleRules fromSFSObject(ISFSObject sfsob)
	{
		return new BattleRules(sfsob.GetBool("bat39"));
	}
}
