using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class ProbabilityBookData : BaseBook
{
	public class Probability : BaseBookItem
	{
		[XmlElement("rarity")]
		public List<Rarity> lstRarity;

		[XmlElement("line")]
		public List<Line> lstLine;
	}

	public class Rarity : BaseBookItem
	{
		[XmlAttribute("perc")]
		public float perc;
	}

	public class Line : BaseBookItem
	{
		[XmlAttribute("perc")]
		public float perc;
	}

	[XmlElement("probability")]
	public List<Probability> lstProbability;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstProbability.Find((Probability item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		lstProbability.Clear();
		lstProbability = null;
	}
}
