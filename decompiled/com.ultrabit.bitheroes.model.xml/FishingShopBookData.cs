using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FishingShopBookData : BaseBook
{
	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public int qty { get; set; }

		[XmlAttribute("cost")]
		public int cost { get; set; }
	}

	public class Tab : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Tabs : BaseBookItem
	{
		[XmlElement("tab")]
		public List<Tab> lstTab { get; set; }
	}

	[XmlElement("tabs")]
	public Tabs tabs { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
