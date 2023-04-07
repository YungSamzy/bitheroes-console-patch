namespace com.ultrabit.bitheroes.model.mount;

public class MountStats
{
	private float _power;

	private float _stamina;

	private float _agility;

	public float power => _power;

	public float stamina => _stamina;

	public float agility => _agility;

	public MountStats(float power, float stamina, float agility)
	{
		_power = power;
		_stamina = stamina;
		_agility = agility;
	}
}
