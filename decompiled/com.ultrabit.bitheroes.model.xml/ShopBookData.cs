using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ShopBookData : BaseBook
{
	public class Text : BaseBookItem
	{
		[XmlAttribute("content")]
		public string content { get; set; }

		[XmlAttribute("size")]
		public int size { get; set; }

		[XmlAttribute("position")]
		public string position { get; set; }

		[XmlAttribute("date")]
		public string date { get; set; }

		[XmlAttribute("color")]
		public string color { get; set; }

		[XmlAttribute("width")]
		public string width { get; set; }

		[XmlAttribute("align")]
		public string align { get; set; }

		[XmlAttribute("autoSize")]
		public string autoSize { get; set; }

		[XmlAttribute("multiline")]
		public string multiline { get; set; }

		[XmlAttribute("center")]
		public string center { get; set; }

		[XmlAttribute("rotation")]
		public string rotation { get; set; }
	}

	public class Equipment : BaseBookItem
	{
	}

	public class Promo : BaseBookItem
	{
		[XmlElement("promoServiceTab")]
		public string promoServiceTab;

		[XmlElement("text")]
		public List<Text> lstText { get; set; }

		[XmlElement("object")]
		public List<AssetDisplayData> lstObject { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("startDate")]
		public string startDate { get; set; }

		[XmlAttribute("endDate")]
		public string endDate { get; set; }

		[XmlAttribute("value")]
		public string value { get; set; }
	}

	public class Mount : BaseBookItem
	{
	}

	public class Promos : BaseBookItem
	{
		[XmlElement("promo")]
		public List<Promo> lstPromo { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("mult")]
		public string mult { get; set; }

		[XmlAttribute("startDate")]
		public string startDate { get; set; }

		[XmlAttribute("endDate")]
		public string endDate { get; set; }
	}

	public class Sales : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Rotations : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class ProgressionOffers : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class VipGor : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
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

	public class Action : BaseBookItem
	{
		[XmlAttribute("values")]
		public string values { get; set; }
	}

	[XmlElement("promos")]
	public Promos promos { get; set; }

	[XmlElement("sales")]
	public Sales sales { get; set; }

	[XmlElement("rotations")]
	public Rotations rotations { get; set; }

	[XmlElement("progressionoffer")]
	public ProgressionOffers progressions { get; set; }

	[XmlElement("vipgor")]
	public VipGor vipgor { get; set; }

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
