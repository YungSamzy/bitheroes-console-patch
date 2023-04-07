using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BattleBookData : BaseBook
{
	public class Effect : BaseBookItem
	{
		[XmlAttribute("startColor")]
		public string startColor;

		[XmlAttribute("endColor")]
		public string endColor;

		[XmlAttribute("minParticles")]
		public int minParticles;

		[XmlAttribute("maxParticles")]
		public int maxParticles;

		[XmlAttribute("minSize")]
		public int minSize;

		[XmlAttribute("maxSize")]
		public int maxSize;

		[XmlAttribute("minDuration")]
		public int minDuration;

		[XmlAttribute("maxDuration")]
		public int maxDuration;

		[XmlAttribute("minAlpha")]
		public float minAlpha;

		[XmlAttribute("maxAlpha")]
		public float maxAlpha;

		[XmlAttribute("minVelocityX")]
		public int minVelocityX;

		[XmlAttribute("maxVelocityX")]
		public int maxVelocityX;

		[XmlAttribute("minVelocityY")]
		public int minVelocityY;

		[XmlAttribute("maxVelocityY")]
		public int maxVelocityY;

		[XmlAttribute("gravityX")]
		public int gravityX;

		[XmlAttribute("gravityY")]
		public int gravityY;

		[XmlAttribute("spread")]
		public float spread;

		[XmlAttribute("dampening")]
		public float dampening;

		[XmlAttribute("asset")]
		public string asset;
	}

	public class Projectile : BaseBookItem
	{
		[XmlAttribute("asset")]
		public string asset;

		[XmlAttribute("definition")]
		public string definition;

		[XmlAttribute("rotate")]
		public string rotate;

		[XmlAttribute("weapon")]
		public string weapon;

		[XmlAttribute("speed")]
		public string speed;

		[XmlAttribute("trailEffect")]
		public string trailEffect;

		[XmlAttribute("trailDelay")]
		public string trailDelay;

		[XmlAttribute("rotation")]
		public string rotation;

		[XmlAttribute("spin")]
		public string spin;

		[XmlAttribute("spread")]
		public string spread;

		[XmlAttribute("center")]
		public string center;

		[XmlAttribute("offset")]
		public string offset;

		[XmlAttribute("scale")]
		public string scale;
	}

	[XmlArray("effects")]
	[XmlArrayItem("effect")]
	public List<Effect> lstEffects;

	[XmlArray("projectiles")]
	[XmlArrayItem("projectile")]
	public List<Projectile> lstProjectiles;

	public void Clear()
	{
		lstEffects.Clear();
		lstEffects = null;
		lstProjectiles.Clear();
		lstProjectiles = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstEffects.Find((Effect item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
