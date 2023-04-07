using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterBase
{
	private int _power;

	private int _stamina;

	private int _agility;

	private List<GameModifier> _modifiers;

	public int power => _power;

	public int stamina => _stamina;

	public int agility => _agility;

	public List<GameModifier> modifiers => _modifiers;

	public CharacterBase(int power, int stamina, int agility, List<GameModifier> modifiers)
	{
		_power = power;
		_stamina = stamina;
		_agility = agility;
		_modifiers = modifiers;
	}
}
