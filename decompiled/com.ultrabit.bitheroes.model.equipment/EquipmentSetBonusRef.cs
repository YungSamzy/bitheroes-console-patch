using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.equipment;

[DebuggerDisplay("{name} (EquipmentSetBonusRef)")]
public class EquipmentSetBonusRef : IEquatable<EquipmentSetBonusRef>, IComparable<EquipmentSetBonusRef>
{
	private List<AbilityRef> _abilities;

	private List<GameModifier> _modifiers;

	private EquipmentBookData.Bonus _bonusData;

	private string name => _bonusData.name;

	private int id => _bonusData.id;

	public string desc => Language.GetString(_bonusData.desc);

	public int count => _bonusData.count;

	public List<AbilityRef> abilities => _abilities;

	public List<GameModifier> modifiers => _modifiers;

	public EquipmentSetBonusRef(EquipmentBookData.Bonus bonusData)
	{
		_bonusData = bonusData;
		_abilities = AbilityBook.LookupAbilities(bonusData.abilities);
		_modifiers = GameModifier.GetGameModifierFromData(bonusData.modifiers, bonusData.lstModifier);
	}

	public bool getBonusEnabled(int total)
	{
		return total >= _bonusData.count;
	}

	public AbilityRef getFirstAbility()
	{
		if (_abilities != null && _abilities.Count > 0)
		{
			return _abilities[0];
		}
		return GameModifier.getFirstAbility(_modifiers);
	}

	public bool Equals(EquipmentSetBonusRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(EquipmentSetBonusRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
