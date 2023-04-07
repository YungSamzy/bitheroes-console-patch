using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BoosterBookData : BaseBook
{
	public class Boosters : BaseBookItem
	{
		[XmlElement("booster")]
		public List<Booster> lstBoosters;
	}

	public class Booster : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItems;

		[XmlElement("cosmetic")]
		public List<Item> lstCosmetic;
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public int qty;
	}

	[XmlElement("boosters")]
	public Boosters boosters;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
