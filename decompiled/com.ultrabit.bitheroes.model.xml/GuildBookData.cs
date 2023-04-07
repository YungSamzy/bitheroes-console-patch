using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class GuildBookData : BaseBook
{
	public class Perks : BaseBookItem
	{
		[XmlElement("perk")]
		public List<Perk> lstPerk { get; set; }
	}

	public class Perk : BaseBookItem
	{
		[XmlElement("rank")]
		public List<Rank> lstRank { get; set; }
	}

	public class Rank : BaseBookItem
	{
		[XmlAttribute("cost")]
		public int cost { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	public class Items : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	[XmlRoot("item")]
	public class Item : BaseBookItem
	{
		[XmlAttribute("values")]
		public string values { get; set; }
	}

	[XmlElement("items")]
	public Items items { get; set; }

	[XmlElement("perks")]
	public Perks perks { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
