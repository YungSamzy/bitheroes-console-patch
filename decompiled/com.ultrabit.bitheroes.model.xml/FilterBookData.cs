using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FilterBookData : BaseBook
{
	[XmlElement("word")]
	public List<string> lstWord;

	[XmlElement("allow")]
	public List<string> lstAllow;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	internal void Clear()
	{
		lstWord.Clear();
		lstWord = null;
		lstAllow.Clear();
		lstAllow = null;
	}
}
