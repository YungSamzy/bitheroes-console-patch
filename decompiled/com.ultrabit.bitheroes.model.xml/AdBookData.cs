using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class AdBookData : BaseBook
{
	public class AdItem : BaseBookItem
	{
		[XmlAttribute("minTier")]
		public string minTier;

		[XmlAttribute("maxTier")]
		public string maxTier;

		[XmlElement("item")]
		public List<Item> lstItem;
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("perc")]
		public float perc { get; set; }
	}

	[XmlElement("adItem")]
	public List<AdItem> lstAdItem;

	[XmlElement("adChestItem")]
	public List<AdItem> lstAdChestItem;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
