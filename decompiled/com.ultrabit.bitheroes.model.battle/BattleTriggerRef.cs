using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.battle;

[DebuggerDisplay("{link} (BattleTriggerRef)")]
public class BattleTriggerRef : IEquatable<BattleTriggerRef>, IComparable<BattleTriggerRef>
{
	private List<AbilityRef> _abilities;

	private BattleTriggerData battleTriggerData;

	public int id => battleTriggerData.id;

	public string link => battleTriggerData.link;

	public float perc => battleTriggerData.perc;

	public List<AbilityRef> abilities => _abilities;

	public BattleTriggerRef(BattleTriggerData battleTriggerData)
	{
		this.battleTriggerData = battleTriggerData;
		if (battleTriggerData.ability != null && !battleTriggerData.ability.Trim().Equals(""))
		{
			_abilities = AbilityBook.LookupAbilities(battleTriggerData.ability);
		}
	}

	public void loadAssets()
	{
		if (_abilities == null)
		{
			return;
		}
		foreach (AbilityRef ability in _abilities)
		{
			if (ability.actions == null)
			{
				continue;
			}
			foreach (AbilityActionRef action in ability.actions)
			{
				action.loadAssets();
			}
		}
	}

	public bool Equals(BattleTriggerRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(BattleTriggerRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
