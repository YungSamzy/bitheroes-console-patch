using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class DailyRewardBookData : BaseBook
{
	public class Rewards : BaseBookItem
	{
		[XmlElement("daily")]
		public List<Daily> lstDaily { get; set; }
	}

	public class Daily : BaseBookItem
	{
		[XmlElement("items")]
		public Items items { get; set; }

		[XmlAttribute("day")]
		public int day { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	public class Items : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	[XmlElement("rewards")]
	public List<Rewards> lstRewards { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return null;
	}
}
