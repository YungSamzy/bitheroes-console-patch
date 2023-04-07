using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml.zone;

[XmlRoot("data")]
public class ZoneXMLData : BaseBook
{
	public class Zone : BaseBookItem
	{
		[XmlElement("nodes")]
		public Nodes nodes { get; set; }

		[XmlElement("notification")]
		public Notification notification { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("requiredZones")]
		public string requiredZones { get; set; }

		[XmlAttribute("starter")]
		public string starter { get; set; }

		[XmlElement("boosters")]
		public Boosters boosters { get; set; }
	}

	public class Difficulty : BaseBookItem
	{
		[XmlElement("rewards")]
		public Rewards rewards { get; set; }

		[XmlAttribute("energy")]
		public string energy { get; set; }

		[XmlAttribute("dungeon")]
		public string dungeon { get; set; }

		[XmlAttribute("slots")]
		public string slots { get; set; }

		[XmlAttribute("size")]
		public string size { get; set; }

		[XmlAttribute("allowFamiliars")]
		public string allowFamiliars { get; set; }

		[XmlAttribute("allowFriends")]
		public string allowFriends { get; set; }

		[XmlAttribute("allowGuildmates")]
		public string allowGuildmates { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlAttribute("requiredDifficulties")]
		public string requiredDifficulties { get; set; }

		[XmlAttribute("statBalance")]
		public string statBalance { get; set; }
	}

	public class Rewards : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	public class Difficulties : BaseBookItem
	{
		[XmlElement("difficulty")]
		public List<Difficulty> lstDifficulty { get; set; }
	}

	public class Nodes : BaseBookItem
	{
		[XmlElement("node")]
		public List<Node> lstNodes { get; set; }
	}

	public class Node : BaseBookItem
	{
		[XmlElement("difficulties")]
		public Difficulties difficulties { get; set; }

		[XmlElement("paths")]
		public Paths paths { get; set; }

		[XmlAttribute("position")]
		public string position { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("repeatable")]
		public string repeatable { get; set; }

		[XmlAttribute("missionID")]
		public string missionID { get; set; }

		[XmlAttribute("unlockNodes")]
		public string unlockNodes { get; set; }

		[XmlAttribute("requiredNodes")]
		public string requiredNodes { get; set; }

		[XmlAttribute("assetOffset")]
		public string assetOffset { get; set; }

		[XmlAttribute("completeZone")]
		public string completeZone { get; set; }

		[XmlAttribute("requiredStars")]
		public string requiredStars { get; set; }

		[XmlAttribute("hidden")]
		public string hidden { get; set; }
	}

	public class Paths : BaseBookItem
	{
		[XmlElement("path")]
		public List<Path> lstPath { get; set; }
	}

	public class Path : BaseBookItem
	{
		[XmlAttribute("points")]
		public string points { get; set; }
	}

	public class Notification : BaseBookItem
	{
	}

	public class Boosters : BaseBookItem
	{
		[XmlElement("payment")]
		public List<Payment> lstPayment { get; set; }
	}

	public class Payment
	{
		[XmlAttribute("id")]
		public string id { get; set; }
	}

	[XmlElement("zone")]
	public Zone zone { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
