using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BaitBookData : BaseBook
{
	public class Bait : BaseBookItem
	{
		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifiers { get; set; }
	}

	[XmlElement("bait")]
	public List<Bait> lstBait { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstBait.Find((Bait item) => item.id.Equals(identifier));
	}

	public void Clear()
	{
		lstBait.Clear();
		lstBait = null;
	}
}
