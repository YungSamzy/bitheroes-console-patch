using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class GuildShopBookData : BaseBook
{
	public class Item : BaseBookItem
	{
		[XmlAttribute("costHonor")]
		public int costHonor { get; set; }

		[XmlAttribute("guildLvlReq")]
		public int guildLvlReq { get; set; }
	}

	[XmlElement("item")]
	public List<Item> lstItem { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
