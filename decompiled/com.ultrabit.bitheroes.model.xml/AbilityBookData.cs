using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class AbilityBookData : BaseBook
{
	public class Position : BaseBookItem
	{
		[XmlAttribute("offset")]
		public string offset;

		[XmlAttribute("flip")]
		public bool flip;

		[XmlAttribute("startAnimation")]
		public string startAnimation;

		[XmlAttribute("startSpeed")]
		public float startSpeed;

		[XmlAttribute("startSpeedScale")]
		public bool startSpeedScale;

		[XmlAttribute("endAnimation")]
		public string endAnimation;

		[XmlAttribute("endSpeed")]
		public float endSpeed;

		[XmlAttribute("endSpeedScale")]
		public bool endSpeedScale;

		[XmlAttribute("startDelay")]
		public float startDelay;

		[XmlAttribute("endDelay")]
		public float endDelay;
	}

	public class Ability : BaseBookItem
	{
		[XmlElement("action")]
		public List<Action> lstAction;

		[XmlAttribute("perc")]
		public int perc;

		[XmlAttribute("animation")]
		public string animation;

		[XmlAttribute("meter")]
		public float meter;

		[XmlAttribute("position")]
		public string position;

		[XmlAttribute("select")]
		public string select;

		[XmlAttribute("sound")]
		public string sound;

		[XmlAttribute("subanimate")]
		public string subanimate;

		[XmlAttribute("uses")]
		public int uses;

		[XmlAttribute("selectDead")]
		public string selectDead;

		[XmlAttribute("selectAlive")]
		public string selectAlive;
	}

	public class Abilities : BaseBookItem
	{
		[XmlAttribute("links")]
		public string links;
	}

	public class Action : BaseBookItem
	{
		[XmlAttribute("value")]
		public float value;

		[XmlAttribute("spread")]
		public string spread;

		[XmlAttribute("target")]
		public string target;

		[XmlAttribute("effectEnd")]
		public string effectEnd;

		[XmlAttribute("effectStart")]
		public string effectStart;

		[XmlAttribute("pierce")]
		public string pierce;

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifiers;

		[XmlElement("modifiers")]
		public GameModifiersData modifiers;

		[XmlAttribute("projectile")]
		public string projectile;

		[XmlAttribute("projectileSource")]
		public string projectileSource;

		[XmlAttribute("ricochet")]
		public bool ricochet;

		[XmlAttribute("split")]
		public bool split;

		[XmlAttribute("random")]
		public string random;

		[XmlAttribute("bounces")]
		public string bounces;

		[XmlAttribute("animate")]
		public string animate;

		[XmlAttribute("targetDead")]
		public bool targetDead;

		[XmlAttribute("targetAlive")]
		public bool targetAlive;

		[XmlAttribute("projectileOffset")]
		public string projectileOffset;

		[XmlAttribute("allowCrit")]
		public bool allowCrit;

		[XmlAttribute("duration")]
		public string duration;
	}

	[XmlElement("position")]
	public List<Position> lstPosition;

	[XmlElement("abilities")]
	public List<Abilities> lstAbilities;

	[XmlElement("ability")]
	public List<Ability> lstAbility;

	public void Clear()
	{
		lstPosition.Clear();
		lstPosition = null;
		lstAbilities.Clear();
		lstAbilities = null;
		lstAbility.Clear();
		lstAbility = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
