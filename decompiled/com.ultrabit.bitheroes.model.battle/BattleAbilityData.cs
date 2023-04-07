using System.Collections.Generic;
using com.ultrabit.bitheroes.model.ability;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.battle;

public class BattleAbilityData
{
	private AbilityRef _abilityRef;

	private int _uses;

	public AbilityRef abilityRef => _abilityRef;

	public int uses => _uses;

	public BattleAbilityData(AbilityRef abilityRef, int uses = 0)
	{
		_abilityRef = abilityRef;
		_uses = uses;
	}

	public static List<BattleAbilityData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("bat59");
		List<BattleAbilityData> list = new List<BattleAbilityData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}

	public static BattleAbilityData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat22");
		return new BattleAbilityData(uses: sfsob.GetInt("bat60"), abilityRef: AbilityBook.Lookup(@int));
	}
}
