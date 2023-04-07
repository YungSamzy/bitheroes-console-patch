using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class AsianLanguageBookData : BaseBook
{
	public class Language : BaseBookItem
	{
		[XmlAttribute("lang")]
		public string lang;

		[XmlElement("fontClassName")]
		public string fontClassName;

		[XmlElement("fontName")]
		public string fontName;
	}

	[XmlElement("language")]
	public List<Language> languages = new List<Language>();

	public void Clear()
	{
		languages.Clear();
		languages = null;
	}

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return languages.Find((Language item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
