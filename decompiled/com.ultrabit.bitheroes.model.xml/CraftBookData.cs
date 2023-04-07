using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class CraftBookData : BaseBook
{
	public class Trades : BaseBookItem
	{
		[XmlElement("trade")]
		public List<Trade> lstTrade { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	public class Requirements : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Upgrade : BaseBookItem
	{
		[XmlElement("requirements")]
		public Requirements requirements { get; set; }
	}

	public class Upgrades : BaseBookItem
	{
		[XmlElement("upgrade")]
		public List<Upgrade> lstUpgrade { get; set; }
	}

	public class Reforge : BaseBookItem
	{
		[XmlElement("rank")]
		public string rank { get; set; }

		[XmlElement("requirements")]
		public Requirements requirements { get; set; }
	}

	public class Reforges : BaseBookItem
	{
		[XmlElement("reforge")]
		public List<Reforge> lstReforge { get; set; }
	}

	public class Reroll : BaseBookItem
	{
		[XmlElement("requirements")]
		public Requirements requirements { get; set; }

		[XmlAttribute("itemType")]
		public string itemType { get; set; }

		[XmlAttribute("itemSubtype")]
		public string itemSubtype { get; set; }
	}

	public class Rerolls : BaseBookItem
	{
		[XmlElement("reroll")]
		public List<Reroll> lstReroll { get; set; }
	}

	public class Result : BaseBookItem
	{
		[XmlElement("item")]
		public Item item { get; set; }
	}

	public class Trade : BaseBookItem
	{
		[XmlElement("requirements")]
		public Requirements requirements { get; set; }

		[XmlElement("result")]
		public Result result { get; set; }

		[XmlAttribute("requirement")]
		public string requirement { get; set; }

		[XmlElement("reveal")]
		public List<Reveal> lstReveal { get; set; }

		[XmlAttribute("crafter")]
		public string crafter { get; set; }

		[XmlAttribute("version")]
		public string version { get; set; }
	}

	public class Reveal : BaseBookItem
	{
	}

	[XmlElement("upgrades")]
	public Upgrades upgrades { get; set; }

	[XmlElement("reforges")]
	public Reforges reforges { get; set; }

	[XmlElement("rerolls")]
	public Rerolls rerolls { get; set; }

	[XmlElement("trades")]
	public Trades trades { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
