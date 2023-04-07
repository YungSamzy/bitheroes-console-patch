using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class DailyBonusBookData : BaseBook
{
	public class Daily : BaseBookItem
	{
		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlAttribute("day")]
		public string day { get; set; }

		[XmlAttribute("date")]
		public string date { get; set; }
	}

	[XmlElement("daily")]
	public List<Daily> lstDaily { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstDaily.Find((Daily item) => item.name.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
