using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class AchievementBookData : BaseBook
{
	public class Achievement : BaseBookItem
	{
		[XmlAttribute("value")]
		public string value { get; set; }

		[XmlAttribute("androidID")]
		public string androidID { get; set; }

		[XmlAttribute("iosID")]
		public string iosID { get; set; }

		[XmlAttribute("steamID")]
		public string steamID { get; set; }

		[XmlAttribute("revealIDs")]
		public string revealIDs { get; set; }
	}

	[XmlElement("achievement")]
	public List<Achievement> lstAchievement { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstAchievement.Find((Achievement item) => item.id.Equals(identifier));
	}
}
