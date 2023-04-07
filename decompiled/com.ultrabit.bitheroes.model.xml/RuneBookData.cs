using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class RuneBookData : BaseBook
{
	public class Rune : BaseBookItem
	{
		[XmlElement("modifier")]
		public GameModifierData modifier;

		[XmlAttribute("abilities")]
		public string abilities;

		[XmlAttribute("exchangeLoot")]
		public string exchangeLoot;

		[XmlAttribute("values")]
		public string values;
	}

	[XmlElement("rune")]
	public List<Rune> lstRune;

	public void Clear()
	{
		lstRune.Clear();
		lstRune = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstRune.Find((Rune item) => item.id.Equals(identifier));
	}
}
