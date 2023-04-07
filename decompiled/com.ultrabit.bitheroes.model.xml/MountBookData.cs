using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class MountBookData : BaseBook
{
	public class Rarities : BaseBookItem
	{
		[XmlElement("rarity")]
		public List<Rarity> lstRarity { get; set; }
	}

	public class Rarity : BaseBookItem
	{
		[XmlElement("rank")]
		public List<Rank> lstRank { get; set; }

		[XmlAttribute("modsMin")]
		public int modsMin { get; set; }

		[XmlAttribute("modsMax")]
		public int modsMax { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	public class Rank : BaseBookItem
	{
		[XmlElement("tier")]
		public List<Tier> lstTier { get; set; }

		[XmlAttribute("upgrade")]
		public string upgrade { get; set; }
	}

	public class Tier : BaseBookItem
	{
		[XmlAttribute("stats")]
		public int stats { get; set; }
	}

	public class Modifiers : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<Modifier> lstModifier { get; set; }
	}

	public class Modifier : BaseBookItem
	{
		[XmlElement("modifier")]
		public GameModifierData modifier { get; set; }
	}

	public class Mounts
	{
		[XmlElement("mount")]
		public List<Mount> lstMount { get; set; }
	}

	public class Mount : BaseBookItem
	{
		[XmlElement("modifiers")]
		public List<GameModifierData> modifiers;

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("abilities")]
		public string abilities { get; set; }

		[XmlAttribute("exchangeLoot")]
		public string exchangeLoot { get; set; }

		[XmlElement("modifier")]
		public GameModifierData modifier { get; set; }

		[XmlAttribute("rerollable")]
		public string rerollable { get; set; }

		[XmlAttribute("power")]
		public string power { get; set; }

		[XmlAttribute("stamina")]
		public string stamina { get; set; }

		[XmlAttribute("agility")]
		public string agility { get; set; }

		[XmlAttribute("purchaseLimit")]
		public int purchaseLimit { get; set; }

		[XmlAttribute("hideFoot")]
		public string hideFoot { get; set; }
	}

	[XmlElement("rarities")]
	public Rarities rarities { get; set; }

	[XmlElement("modifiers")]
	public Modifiers modifiers { get; set; }

	[XmlElement("mounts")]
	public Mounts mounts { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return mounts.lstMount.Find((Mount item) => item.id.Equals(identifier));
	}
}
