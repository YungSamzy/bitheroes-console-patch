using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class CharacterAchievementBookData : BaseBook
{
	public class Achievements : BaseBookItem
	{
		[XmlElement("achievement")]
		public List<Achievement> lstAchievement { get; set; }
	}

	public class Achievement : BaseBookItem
	{
		[XmlAttribute("parent")]
		public string parent { get; set; }

		[XmlAttribute("category")]
		public string category { get; set; }

		[XmlAttribute("value")]
		public string value { get; set; }

		[XmlAttribute("reward")]
		public string reward { get; set; }

		[XmlAttribute("amount")]
		public string amount { get; set; }
	}

	[XmlElement("achievements")]
	public Achievements achievements { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return achievements.lstAchievement.Find((Achievement item) => item.name.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
