using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class NewsBookData : BaseBook
{
	public class Promos : BaseBookItem
	{
		[XmlElement("promo")]
		public List<ShopBookData.Promo> lstPromo { get; set; }
	}

	[XmlElement("promos")]
	public Promos promos { get; set; }

	[XmlElement("notes")]
	public string notes { get; set; }

	[XmlAttribute("version")]
	public string version { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
