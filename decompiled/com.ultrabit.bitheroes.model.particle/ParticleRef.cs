using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.particle;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.particle;

[DebuggerDisplay("{link} (ParticleRef)")]
public class ParticleRef : IEquatable<ParticleRef>, IComparable<ParticleRef>
{
	private string PARTICLE_EFFECT_URL = "Prefabs/Particle/ParticleEffect";

	private BattleBookData.Effect _effectData;

	private GameObject _effectGO;

	private ParticleEffect _particleEffect;

	public ParticleRef(BattleBookData.Effect effectData)
	{
		_effectData = effectData;
	}

	public void createEffects(float speed, Vector2 point, bool flipped, Transform container, float scale = 1f)
	{
		if (_effectData.minParticles > 0 || _effectData.maxParticles > 0)
		{
			_effectGO = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<GameObject>(PARTICLE_EFFECT_URL));
			_particleEffect = _effectGO.GetComponent<ParticleEffect>();
			_particleEffect.startColor = "0X" + _effectData.startColor;
			_particleEffect.endColor = "0X" + _effectData.endColor;
			_particleEffect.minParticles = _effectData.minParticles;
			_particleEffect.maxParticles = _effectData.maxParticles;
			_particleEffect.minSize = _effectData.minSize;
			_particleEffect.maxSize = _effectData.maxSize;
			_particleEffect.minDuration = _effectData.minDuration;
			_particleEffect.maxDuration = _effectData.maxDuration;
			_particleEffect.minAlpha = _effectData.minAlpha;
			_particleEffect.maxAlpha = _effectData.maxAlpha;
			_particleEffect.minVelocityX = _effectData.minVelocityX;
			_particleEffect.maxVelocityX = _effectData.maxVelocityX;
			_particleEffect.minVelocityY = _effectData.minVelocityY;
			_particleEffect.maxVelocityY = _effectData.maxVelocityY;
			_particleEffect.gravityX = _effectData.gravityX;
			_particleEffect.gravityY = _effectData.gravityY;
			_particleEffect.spread = _effectData.spread;
			_particleEffect.dampening = _effectData.dampening;
			Vector3 zero = Vector3.zero;
			zero.x = point.x;
			zero.y = point.y;
			_particleEffect.transform.position = zero;
			_particleEffect.transform.SetParent(container, worldPositionStays: true);
			_particleEffect.CreateParticles(speed);
		}
	}

	public bool Equals(ParticleRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.Equals(other);
	}

	public int CompareTo(ParticleRef other)
	{
		if (other == null)
		{
			return -1;
		}
		if (_effectGO == null || _particleEffect == null)
		{
			return 0;
		}
		int num = _particleEffect.transform.position.y.CompareTo(other._particleEffect.transform.position.y);
		if (num == 0)
		{
			return _particleEffect.transform.position.x.CompareTo(other._particleEffect.transform.position.x);
		}
		return num;
	}

	public void loadAssets()
	{
	}
}
