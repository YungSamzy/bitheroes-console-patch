using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BrawlBookData : BaseBook
{
	[XmlRoot(ElementName = "difficulty")]
	public class Difficulty : BaseBookItem
	{
		[XmlAttribute("seals")]
		public int seals { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("statMult")]
		public float StatMult { get; set; }
	}

	public class Difficulties : BaseBookItem
	{
		[XmlElement("difficulty")]
		public List<Difficulty> lstDifficulty { get; set; }
	}

	public class Object : BaseBookItem
	{
		[XmlAttribute("itemID")]
		public string itemID { get; set; }

		[XmlAttribute("itemType")]
		public string itemType { get; set; }

		[XmlAttribute("position")]
		public string position { get; set; }

		[XmlAttribute("scale")]
		public float scale { get; set; }

		[XmlElement("equipment")]
		public List<Equipment> lstEquipment { get; set; }

		[XmlAttribute("hair")]
		public string hair { get; set; }

		[XmlAttribute("hairColor")]
		public string hairColor { get; set; }

		[XmlAttribute("skinColor")]
		public string skinColor { get; set; }

		[XmlAttribute("gender")]
		public string gender { get; set; }

		[XmlAttribute("flipped")]
		public string flipped { get; set; }
	}

	public class Equipment : BaseBookItem
	{
	}

	public class Tier : BaseBookItem
	{
		[XmlElement("difficulty")]
		public List<Difficulty> lstDifficulty { get; set; }

		[XmlAttribute("zoneCompleteReq")]
		public int zoneCompleteReq { get; set; }
	}

	public class Brawl : BaseBookItem
	{
		[XmlElement("promo")]
		public ShopBookData.Promo Promo { get; set; }

		[XmlElement("tier")]
		public List<Tier> lstTier { get; set; }

		[XmlAttribute("textColor")]
		public string textColor { get; set; }

		[XmlAttribute("dialogEnter")]
		public string dialogEnter { get; set; }

		[XmlAttribute("allowSwitch")]
		public string allowSwitch { get; set; }

		[XmlAttribute("battleBGs")]
		public string battleBGs { get; set; }

		[XmlAttribute("encounters")]
		public string encounters { get; set; }

		[XmlAttribute("slots")]
		public int slots { get; set; }

		[XmlAttribute("battleMusic")]
		public string battleMusic { get; set; }
	}

	public class Brawls : BaseBookItem
	{
		[XmlElement("brawl")]
		public List<Brawl> lstBrawl { get; set; }
	}

	[XmlElement("difficulties")]
	public Difficulties difficulties { get; set; }

	[XmlElement("brawls")]
	public Brawls brawls { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return brawls.lstBrawl.Find((Brawl item) => item.name.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return brawls.lstBrawl.Find((Brawl item) => item.id.Equals(identifier));
	}

	internal BrawlRef lookup(int brawlID)
	{
		throw new NotImplementedException();
	}
}
