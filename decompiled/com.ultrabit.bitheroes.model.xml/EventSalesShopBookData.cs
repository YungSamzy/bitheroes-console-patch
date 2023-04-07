using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class EventSalesShopBookData : BaseBook
{
	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public int qty { get; set; }

		[XmlAttribute("cost")]
		public int cost { get; set; }

		[XmlAttribute("purchaseLimit")]
		public int purchaseLimit { get; set; }
	}

	public class Event : BaseBookItem
	{
		[XmlAttribute("hudSprite")]
		public string hudSprite { get; set; }

		[XmlAttribute("hudLabel")]
		public string hudLabel { get; set; }

		[XmlAttribute("background")]
		public string background { get; set; }

		[XmlAttribute("topper")]
		public string topper { get; set; }

		[XmlAttribute("material")]
		public int material { get; set; }

		[XmlAttribute("startDate")]
		public string startDate { get; set; }

		[XmlAttribute("endDate")]
		public string endDate { get; set; }

		[XmlElement("tabs")]
		public Tabs tabs { get; set; }
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

	[XmlElement("event")]
	public List<Event> lstEvents { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
