using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class GuildHallBookData : BaseBook
{
	public class Cosmetics
	{
		[XmlElement("type")]
		public List<Type> lstType { get; set; }

		[XmlElement("cosmetic")]
		public List<Cosmetic> lstCosmetic { get; set; }
	}

	public class Cosmetic : BaseBookItem
	{
		[XmlAttribute("guildLvlReq")]
		public int guildLvlReq { get; set; }

		[XmlAttribute("parent")]
		public string parent { get; set; }

		[XmlAttribute("display")]
		public string display { get; set; }

		[XmlAttribute("objects")]
		public string objects { get; set; }
	}

	public class Type : BaseBookItem
	{
		[XmlAttribute("locationName")]
		public string locationName { get; set; }

		[XmlAttribute("displayName")]
		public string displayName { get; set; }

		[XmlAttribute("autoSelect")]
		public string AutoSelect { get; set; }
	}

	[XmlElement("cosmetics")]
	public Cosmetics cosmetics { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
