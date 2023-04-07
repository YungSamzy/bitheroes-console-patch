using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.particle;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.battle;

[DebuggerDisplay("{link} (BattleProjectileRef)")]
public class BattleProjectileRef : IEquatable<BattleProjectileRef>, IComparable<BattleProjectileRef>
{
	public const float DEFAULT_SPEED = 200f;

	public const float DEFAULT_DISTANCE = 50f;

	private string _link;

	private string _asset;

	private string _definition;

	private bool _loadLocal;

	private bool _weapon;

	private float _speed;

	private ParticleRef _trailEffectRef;

	private int _trailDelay;

	private bool _rotate;

	private float _rotation;

	private float _spin;

	private float _spread;

	private Vector2 _offset;

	private bool _center;

	private float _scale;

	private string link => _link;

	public bool weapon => _weapon;

	public float speed => _speed;

	public ParticleRef trailEffectRef => _trailEffectRef;

	public int trailDelay => _trailDelay;

	public bool rotate => _rotate;

	public float rotation => _rotation;

	public float spin => _spin;

	public float spread => _spread;

	public Vector2 offset => _offset;

	public bool center => _center;

	public BattleProjectileRef(BattleBookData.Projectile projectileData)
	{
		_link = projectileData.link.ToLowerInvariant();
		_asset = projectileData.asset;
		_definition = projectileData.definition;
		_weapon = Util.parseBoolean(projectileData.weapon, defaultVal: false);
		_speed = ((projectileData.speed != null) ? Util.ParseFloat(projectileData.speed) : 200f);
		_trailEffectRef = ((projectileData.trailEffect != null) ? BattleBook.LookupEffect(projectileData.trailEffect) : null);
		_trailDelay = ((projectileData.trailDelay != null) ? int.Parse(projectileData.trailDelay) : 100);
		_rotate = Util.parseBoolean(projectileData.rotate);
		_rotation = ((projectileData.rotation != null) ? Util.ParseFloat(projectileData.rotation) : 0f);
		_spin = ((projectileData.spin != null) ? Util.ParseFloat(projectileData.spin) : 0f);
		_spread = ((projectileData.spread != null) ? Util.ParseFloat(projectileData.spread) : 0f);
		_offset = Util.pointFromString(projectileData.offset);
		_center = Util.parseBoolean(projectileData.center);
		_scale = ((projectileData.scale != null) ? Util.ParseFloat(projectileData.scale) : 1f);
	}

	public void loadAssets()
	{
	}

	public GameObject getAsset(bool center = false, float scale = 1f)
	{
		Transform transformAsset = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.ABILITY_PROJECTILE, _asset);
		if (transformAsset != null)
		{
			transformAsset.transform.localScale = new Vector3(scale * _scale, scale * _scale, 1f);
			return transformAsset.gameObject;
		}
		return null;
	}

	public bool Equals(BattleProjectileRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(BattleProjectileRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
