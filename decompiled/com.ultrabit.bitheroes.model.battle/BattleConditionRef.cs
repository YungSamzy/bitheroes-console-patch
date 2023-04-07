using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.game;

namespace com.ultrabit.bitheroes.model.battle;

[DebuggerDisplay("{id} (BattleConditionRef)")]
public class BattleConditionRef : IEquatable<BattleConditionRef>, IComparable<BattleConditionRef>
{
	private int _id;

	private List<AbilityRef> _abilities;

	private List<GameModifier> _modifiers;

	private int _type;

	private float _perc;

	private string _value;

	public const int TYPE_MAINHANDELEMENTAL_TYPE = 59;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int> { ["mainhandelementaltype"] = 59 };

	public int id => _id;

	public List<AbilityRef> abilities => _abilities;

	public List<GameModifier> modifiers => _modifiers;

	public BattleConditionRef(int id, int type, float perc, string value, List<AbilityRef> abilities, List<GameModifier> modifiers)
	{
		_id = id;
		_type = type;
		_perc = perc;
		_value = value;
		_abilities = abilities;
		_modifiers = modifiers;
	}

	public bool getNonBattleConditionMet(List<EquipmentRef> equipmentSlots)
	{
		if (_type == 59)
		{
			foreach (EquipmentRef equipmentSlot in equipmentSlots)
			{
				if (equipmentSlot != null && equipmentSlot.equipmentType == 1 && equipmentSlot.elemental == EquipmentRef.getEquipmentElementalType(_value.ToLower()))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static int getType(string type)
	{
		if (type != null && TYPES.ContainsKey(type))
		{
			return TYPES[type];
		}
		return 0;
	}

	public bool Equals(BattleConditionRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(BattleConditionRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
