using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot(ElementName = "data")]
public class RaidBookData : BaseBook
{
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
		public string SkinColor { get; set; }

		[XmlAttribute("gender")]
		public string gender { get; set; }

		[XmlAttribute("flipped")]
		public string flipped { get; set; }
	}

	public class Equipment : BaseBookItem
	{
	}

	public class Promo : BaseBookItem
	{
		[XmlElement("object")]
		public List<Object> lstObject { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }
	}

	public class Difficulty : BaseBookItem
	{
		[XmlAttribute("shards")]
		public string shards { get; set; }

		[XmlAttribute("dungeon")]
		public string dungeon { get; set; }

		[XmlAttribute("slots")]
		public string slots { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlAttribute("size")]
		public string size { get; set; }

		[XmlAttribute("allowFamiliars")]
		public string allowFamiliars { get; set; }

		[XmlAttribute("allowFriends")]
		public string allowFriends { get; set; }

		[XmlAttribute("allowGuildmates")]
		public string allowGuildmates { get; set; }

		[XmlAttribute("statBalance")]
		public string statBalance { get; set; }
	}

	public class Difficulties : BaseBookItem
	{
		[XmlElement("difficulty")]
		public List<Difficulty> lstDifficulty { get; set; }
	}

	public class Raid : BaseBookItem
	{
		[XmlElement("promo")]
		public ShopBookData.Promo promo { get; set; }

		[XmlElement("difficulties")]
		public Difficulties difficulties { get; set; }

		[XmlElement("items")]
		public string items { get; set; }

		[XmlAttribute("textColor")]
		public string textColor { get; set; }

		[XmlAttribute("dialogEnter")]
		public string dialogEnter { get; set; }

		[XmlAttribute("requiredZone")]
		public int requiredZone { get; set; }
	}

	[XmlElement("raid")]
	public List<Raid> lstRaid { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstRaid.Find((Raid item) => item.name.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstRaid.Find((Raid item) => item.id.Equals(identifier));
	}
}
