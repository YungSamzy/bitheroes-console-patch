using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class RewardBookData : BaseBook
{
	public class Rewards : BaseBookItem
	{
		[XmlElement("reward")]
		public List<Reward> lstReward;
	}

	public class Reward : BaseBookItem
	{
		[XmlElement("ranks")]
		public Ranks ranks;
	}

	public class Ranks : BaseBookItem
	{
		[XmlElement("rank")]
		public List<Rank> lstRank;
	}

	public class Rank : BaseBookItem
	{
		[XmlElement("zonecomplete")]
		public List<ZoneCompleted> lstZoneCompleted;

		[XmlAttribute("min")]
		public string min { get; set; }

		[XmlAttribute("max")]
		public string max { get; set; }
	}

	public class ZoneCompleted : BaseBookItem
	{
		[XmlElement("pool")]
		public List<ZonePool> lstPool;

		[XmlAttribute("min")]
		public int min { get; set; }

		[XmlAttribute("max")]
		public int max { get; set; }
	}

	public class ZonePool : BaseBookItem
	{
		[XmlAttribute("qty")]
		public int qty { get; set; }
	}

	public class Pools : BaseBookItem
	{
		[XmlElement("pool")]
		public List<Pool> lstPool;
	}

	public class Pool : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem;
	}

	public class Item : BaseBookItem
	{
	}

	[XmlElement("rewards")]
	public Rewards rewards { get; set; }

	[XmlElement("pools")]
	public Pools pools { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}
}
