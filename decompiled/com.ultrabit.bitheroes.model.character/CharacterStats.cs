namespace com.ultrabit.bitheroes.model.character;

public class CharacterStats
{
	private int _power;

	private int _stamina;

	private int _agility;

	public int total => power + stamina + agility;

	public int power => _power;

	public int stamina => _stamina;

	public int agility => _agility;

	public CharacterStats(int power, int stamina, int agility)
	{
		_power = power;
		_stamina = stamina;
		_agility = agility;
	}

	public void balance(int stats)
	{
		int num = stats - total;
		if (num != 0)
		{
			adjustHighestStat(num);
		}
	}

	private void adjustHighestStat(int change)
	{
		switch (getHighestStat())
		{
		case 0:
			_power += change;
			break;
		case 1:
			_stamina += change;
			break;
		case 2:
			_agility += change;
			break;
		}
	}

	private int getHighestStat()
	{
		if (_power > _stamina && _power > _agility)
		{
			return 0;
		}
		if (_stamina > _power && _stamina > _agility)
		{
			return 1;
		}
		if (_agility > _power && _agility > _stamina)
		{
			return 2;
		}
		return 0;
	}
}
