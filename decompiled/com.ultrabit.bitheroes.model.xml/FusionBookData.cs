using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class FusionBookData : BaseBook
{
	public class Fusion : BaseBookItem
	{
		[XmlAttribute("craftlocked")]
		public string craftlocked;

		[XmlElement("requirements")]
		public Requirements requirements { get; set; }

		[XmlElement("result")]
		public Result result { get; set; }
	}

	public class Requirements : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Result : BaseBookItem
	{
		[XmlElement("item")]
		public Item item { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	[XmlElement("fusion")]
	public List<Fusion> lstFusion { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstFusion.Find((Fusion item) => item.id.Equals(identifier));
	}
}
