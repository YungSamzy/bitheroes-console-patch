using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class LeaderboardBookData : BaseBook
{
	public class Leaderboards : BaseBookItem
	{
		[XmlElement("leaderboard")]
		public List<Leaderboard> lstLeaderboard { get; set; }
	}

	public class Leaderboard : BaseBookItem
	{
		[XmlAttribute("listType")]
		public string listType { get; set; }

		[XmlAttribute("dataType")]
		public string dataType { get; set; }

		[XmlAttribute("value")]
		public string value { get; set; }

		[XmlAttribute("query")]
		public string query { get; set; }
	}

	[XmlElement("leaderboards")]
	public Leaderboards leaderboards { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
