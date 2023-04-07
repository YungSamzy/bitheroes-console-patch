using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.language;

[XmlRoot("data")]
public class LanguageXML
{
	public class Item
	{
		[XmlAttribute("l")]
		public string link;

		[XmlText]
		public string translation;

		[XmlAttribute("t")]
		public string translationAttribute;
	}

	[XmlAttribute("id")]
	public int id;

	[XmlElement("s")]
	public List<Item> items;
}
