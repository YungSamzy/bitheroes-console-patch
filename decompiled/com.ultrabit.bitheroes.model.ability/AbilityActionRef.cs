using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.particle;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.ability;

[DebuggerDisplay("{typeLink} (AbilityActionRef)")]
public class AbilityActionRef : IEquatable<AbilityActionRef>, IComparable<AbilityActionRef>
{
	private const float VALUE_DIVIDER = 100f;

	public const float DEFAULT_DURATION = 0.05f;

	public const int TYPE_NONE = 0;

	public const int TYPE_HEALTH_CHANGE = 1;

	public const int TYPE_SHIELD_CHANGE = 2;

	public const int TYPE_METER_CHANGE = 3;

	public const int TYPE_HEALTH_SET = 4;

	public const int TYPE_SHIELD_SET = 5;

	public const int TYPE_DAMAGE_GAIN_CHANGE = 6;

	public const int TYPE_HEALTH_PERC_CHANGE = 7;

	public const int TYPE_SHIELD_PERC_CHANGE = 8;

	public const int TYPE_HEALTH_TO_SHIELD_PERC_CHANGE = 9;

	public const int TYPE_SHIELD_TO_HEALTH_PERC_CHANGE = 10;

	public const int TYPE_FIRE_HEALTH_CHANGE = 11;

	public const int TYPE_WATER_HEALTH_CHANGE = 12;

	public const int TYPE_ELECTRIC_HEALTH_CHANGE = 13;

	public const int TYPE_EARTH_HEALTH_CHANGE = 14;

	public const int TYPE_AIR_HEALTH_CHANGE = 15;

	public const int TYPE_BUFF_DAMAGE = 16;

	public const int TYPE_SKIP_TURN = 17;

	public static int STACK_TYPE_COMBUSTION = 1;

	public static int STACK_TYPE_SHOCK = 2;

	public static int STACK_TYPE_FREEZE = 3;

	public static int STACK_TYPE_BLEED = 4;

	public static int STACK_TYPE_ROOT = 5;

	public static int STACK_TYPE_CORRUPT = 6;

	public static int STACK_TYPE_BLINDED = 7;

	public const int STACK_STATUS_ADDED = 1;

	public const int STACK_STATUS_CONSUMED = 2;

	public const int STACK_STATUS_UNAVAILABLE = 3;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		[""] = 0,
		["healthchange"] = 1,
		["shieldchange"] = 2,
		["meterchange"] = 3,
		["healthset"] = 4,
		["shieldset"] = 5,
		["damagegainchange"] = 6,
		["healthpercchange"] = 7,
		["shieldpercchange"] = 8,
		["healthtoshieldpercchange"] = 9,
		["shieldtohealthpercchange"] = 10,
		["firehealthchange"] = 11,
		["waterhealthchange"] = 12,
		["electrichealthchange"] = 13,
		["earthhealthchange"] = 14,
		["airhealthchange"] = 15,
		["buffdamage"] = 16,
		["skipturn"] = 17
	};

	private static Dictionary<int, string> STACK_TYPES = new Dictionary<int, string>
	{
		[STACK_TYPE_COMBUSTION] = "combustion",
		[STACK_TYPE_SHOCK] = "shock",
		[STACK_TYPE_FREEZE] = "freeze",
		[STACK_TYPE_BLEED] = "bleed",
		[STACK_TYPE_ROOT] = "root",
		[STACK_TYPE_CORRUPT] = "corrupt",
		[STACK_TYPE_BLINDED] = "blinded"
	};

	private int _id;

	private float _value;

	private int _type;

	private string _typeLink;

	private float _spread;

	private int _random;

	private int _pierce;

	private int _bounces;

	private float _duration;

	private bool _animate;

	private BattleProjectileRef _projectileRef;

	private int _projectileSource;

	private Vector2 _projectileOffset;

	private ParticleRef _effectStart;

	private ParticleRef _effectEnd;

	public int id => _id;

	public float value => _value;

	private string typeLink => _typeLink;

	public float spread => _spread;

	public int random => _random;

	public int pierce => _pierce;

	public int bounces => _bounces;

	public float duration => _duration;

	public bool animate => _animate;

	public BattleProjectileRef projectileRef => _projectileRef;

	public int projectileSource => _projectileSource;

	public Vector2 projectileOffset => _projectileOffset;

	public ParticleRef effectStart => _effectStart;

	public ParticleRef effectEnd => _effectEnd;

	public AbilityActionRef(int id, AbilityBookData.Action actionData)
	{
		_id = id;
		_value = actionData.value;
		_spread = ((actionData.spread != null) ? Util.ParseFloat(actionData.spread) : 0.1f);
		_random = ((actionData.random != null) ? int.Parse(actionData.random) : 0);
		_pierce = ((actionData.pierce != null) ? int.Parse(actionData.pierce) : 0);
		_bounces = ((actionData.bounces != null) ? int.Parse(actionData.bounces) : 0);
		_duration = ((actionData.duration != null) ? Util.ParseFloat(actionData.duration) : 0.05f);
		_animate = Util.parseBoolean(actionData.animate, defaultVal: false);
		_projectileRef = ((actionData.projectile != null) ? BattleBook.LookupProjectile(actionData.projectile) : null);
		_projectileSource = ((actionData.projectileSource != null) ? EquipmentRef.getEquipmentType(actionData.projectileSource) : 0);
		_projectileOffset = ((actionData.projectileOffset != null) ? Util.pointFromString(actionData.projectileOffset) : new Vector2(0f, 0f));
		_effectStart = ((actionData.effectStart != null) ? BattleBook.LookupEffect(actionData.effectStart) : null);
		_effectEnd = ((actionData.effectEnd != null) ? BattleBook.LookupEffect(actionData.effectEnd) : null);
		_typeLink = actionData.type;
		_type = getType(actionData.type);
	}

	public static int getType(string type)
	{
		if (!TYPES.ContainsKey(type))
		{
			D.LogWarning("Ability Type not found: " + type);
		}
		return TYPES[type.ToLowerInvariant()];
	}

	public int getType()
	{
		return _type;
	}

	public Vector2 getValueRange(int power, float mult)
	{
		float valueAdjusted = getValueAdjusted(_value, power, mult);
		if (_spread <= 0f)
		{
			return new Vector2(valueAdjusted / 100f, valueAdjusted / 100f);
		}
		float num = ((valueAdjusted < 0f) ? (valueAdjusted * -1f) : valueAdjusted);
		float num2 = valueAdjusted - num * _spread;
		return new Vector2(y: (valueAdjusted + num * _spread) / 100f, x: num2 / 100f);
	}

	private float getValueAdjusted(float value, int power, float mult)
	{
		float num = value * mult;
		return (float)power * num;
	}

	public float getValueRandom(int power, float mult)
	{
		Vector2 valueRange = getValueRange(power, mult);
		return Util.RandomNumber(valueRange.x, valueRange.y);
	}

	public int getValueTotal(int power, float mult)
	{
		float valueRandom = getValueRandom(power, mult);
		int num = (int)Mathf.Round(valueRandom);
		if (num == 0)
		{
			if (valueRandom > 0f)
			{
				return 1;
			}
			return -1;
		}
		return num;
	}

	public Vector2 getTotalValueRange(int power, float mult = 0f, bool normalize = true)
	{
		Vector2 valueRange = getValueRange(power, mult);
		if (normalize)
		{
			bool num = valueRange.x < 0f || valueRange.y < 0f;
			if (valueRange.x < 0f)
			{
				valueRange.x *= -1f;
			}
			if (valueRange.y < 0f)
			{
				valueRange.y *= -1f;
			}
			valueRange.x = Mathf.Round(valueRange.x);
			valueRange.y = Mathf.Round(valueRange.y);
			if (num)
			{
				int num2 = (int)valueRange.x;
				int num3 = (int)valueRange.y;
				valueRange.x = num3;
				valueRange.y = num2;
			}
		}
		return valueRange;
	}

	public void loadAssets()
	{
		if (_projectileRef != null)
		{
			_projectileRef.loadAssets();
		}
		if (_effectStart != null)
		{
			_effectStart.loadAssets();
		}
		if (_effectEnd != null)
		{
			_effectEnd.loadAssets();
		}
	}

	public static string getStackLabel(int type, int count, int status)
	{
		if (!STACK_TYPES.ContainsKey(type))
		{
			return null;
		}
		string text = "stack_ability_";
		string text2 = "_stack_added";
		switch (status)
		{
		case 2:
			text2 = "_stack_consumed";
			break;
		case 3:
			text2 = "_stack_unavailable";
			break;
		}
		return text + STACK_TYPES[type] + text2;
	}

	public bool Equals(AbilityActionRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(AbilityActionRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
