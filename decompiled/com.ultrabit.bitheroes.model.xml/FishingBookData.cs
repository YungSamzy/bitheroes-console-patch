using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FishingBookData : BaseBook
{
	public class Bars : BaseBookItem
	{
		[XmlElement("bar")]
		public List<Bar> lstBar;
	}

	public class Bar : BaseBookItem
	{
		[XmlElement("chance")]
		public List<Chance> lstChance;
	}

	public class Chance : BaseBookItem
	{
		[XmlAttribute("size")]
		public float size;

		[XmlAttribute("chance")]
		public float chance;

		[XmlAttribute("color")]
		public string color;
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("perc")]
		public float perc;

		[XmlAttribute("mult")]
		public float mult;

		[XmlAttribute("loot")]
		public string loot;

		[XmlElement("weight")]
		public List<Weight> lstWeight;

		[XmlAttribute("speed")]
		public string speed;

		[XmlAttribute("distance")]
		public string distance;

		[XmlAttribute("ease")]
		public string ease;

		[XmlAttribute("time")]
		public float time;

		[XmlAttribute("bar")]
		public string bar;

		[XmlAttribute("width")]
		public float width;
	}

	public class Weight : BaseBookItem
	{
		[XmlAttribute("perc")]
		public float perc;
	}

	[XmlElement("bars")]
	public Bars bars;

	[XmlArray("items")]
	[XmlArrayItem("item")]
	public List<Item> lstItems;

	public void Clear()
	{
		lstItems.Clear();
		lstItems = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstItems.Find((Item item) => item.id.Equals(identifier));
	}
}
