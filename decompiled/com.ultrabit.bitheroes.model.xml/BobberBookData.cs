using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BobberBookData : BaseBook
{
	public class Bobber : BaseBookItem
	{
		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	[XmlElement("bobber")]
	public List<Bobber> lstBobber { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstBobber.Find((Bobber item) => item.id.Equals(identifier));
	}

	public void Clear()
	{
		lstBobber.Clear();
		lstBobber = null;
	}
}
