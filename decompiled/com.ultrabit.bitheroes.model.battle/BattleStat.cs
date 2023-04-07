namespace com.ultrabit.bitheroes.model.battle;

public class BattleStat
{
	private int _damageDone;

	private int _damageTaken;

	private int _healingDone;

	private int _healingTaken;

	private int _damageBlocked;

	private int _shielding;

	private int _id = -1;

	private int _value = -1;

	private int _max = -1;

	private int _idx = -1;

	private int _type = -1;

	private bool _attacker;

	public int damageDone
	{
		get
		{
			return _damageDone;
		}
		set
		{
			_damageDone = value;
		}
	}

	public int damageTaken
	{
		get
		{
			return _damageTaken;
		}
		set
		{
			_damageTaken = value;
		}
	}

	public int healingDone
	{
		get
		{
			return _healingDone;
		}
		set
		{
			_healingDone = value;
		}
	}

	public int healingTaken
	{
		get
		{
			return _healingTaken;
		}
		set
		{
			_healingTaken = value;
		}
	}

	public int damageBlocked
	{
		get
		{
			return _damageBlocked;
		}
		set
		{
			_damageBlocked = value;
		}
	}

	public int shielding
	{
		get
		{
			return _shielding;
		}
		set
		{
			_shielding = value;
		}
	}

	public int value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public int max
	{
		get
		{
			return _max;
		}
		set
		{
			_max = value;
		}
	}

	public int id => _id;

	public int idx => _idx;

	public int type => _type;

	public bool attacker => _attacker;

	public BattleStat(int id, int type, int idx, bool attacker)
	{
		_id = id;
		_idx = idx;
		_type = type;
		_attacker = attacker;
	}

	public void Dump()
	{
	}
}
