using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class DialogBookData : BaseBook
{
	public class Content : BaseBookItem
	{
	}

	public class Frame : BaseBookItem
	{
		[XmlElement("content")]
		public List<Content> lstContent { get; set; }

		[XmlAttribute("position")]
		public string position { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("portrait")]
		public string portrait { get; set; }

		[XmlAttribute("textOffset")]
		public int textOffset { get; set; }

		[XmlAttribute("textBG")]
		public string textBG { get; set; }
	}

	public class Dialog : BaseBookItem
	{
		[XmlElement("frame")]
		public List<Frame> lstFrame { get; set; }
	}

	[XmlElement("asset")]
	public List<AssetDisplayData> lstAsset { get; set; }

	[XmlElement("dialog")]
	public List<Dialog> lstDialog { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstDialog.Find((Dialog item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
