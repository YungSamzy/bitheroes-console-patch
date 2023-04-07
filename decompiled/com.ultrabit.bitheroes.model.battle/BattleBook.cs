using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.particle;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.battle;

public class BattleBook
{
	private static Dictionary<string, ParticleRef> _effects;

	private static Dictionary<string, BattleProjectileRef> _projectiles;

	private static List<BattleTriggerRef> _triggers;

	private static List<BattleConditionRef> _conditions;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_effects = new Dictionary<string, ParticleRef>();
		_projectiles = new Dictionary<string, BattleProjectileRef>();
		_triggers = new List<BattleTriggerRef>();
		_conditions = new List<BattleConditionRef>();
		foreach (BattleBookData.Effect lstEffect in XMLBook.instance.battleBook.lstEffects)
		{
			_effects.Add(lstEffect.link, new ParticleRef(lstEffect));
		}
		foreach (BattleBookData.Projectile lstProjectile in XMLBook.instance.battleBook.lstProjectiles)
		{
			_projectiles.Add(lstProjectile.link, new BattleProjectileRef(lstProjectile));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static ParticleRef LookupEffect(string link)
	{
		if (link == null || link.Length <= 0 || !_effects.ContainsKey(link))
		{
			return null;
		}
		return _effects[link];
	}

	public static BattleProjectileRef LookupProjectile(string link)
	{
		if (link == null || link.Length <= 0 || !_projectiles.ContainsKey(link))
		{
			return null;
		}
		return _projectiles[link];
	}
}
