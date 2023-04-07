using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class DailyQuestBookData : BaseBook
{
	public class Quest : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }

		[XmlAttribute("amount")]
		public int amount { get; set; }

		[XmlAttribute("available")]
		public string available { get; set; }

		[XmlAttribute("value")]
		public string value { get; set; }

		[XmlAttribute("starter")]
		public string starter { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
	}

	[XmlElement("quest")]
	public List<Quest> lstQuest { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstQuest.Find((Quest item) => item.name.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
