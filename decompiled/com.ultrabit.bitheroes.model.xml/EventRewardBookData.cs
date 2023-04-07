using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class EventRewardBookData : BaseBook
{
	public class Pools : BaseBookItem
	{
		[XmlElement("pool")]
		public List<Pool> lstPool;
	}

	public class Pool : BaseBookItem
	{
		[XmlElement("item")]
		public List<PoolItem> lstItem;
	}

	public class PoolItem : BaseBookItem
	{
	}

	public class Rewards : BaseBookItem
	{
		[XmlElement("reward")]
		public List<EventReward> lstReward;
	}

	public class EventReward : BaseBookItem
	{
		[XmlElement("tier")]
		public List<Tier> lstTier;

		[XmlElement("ranks")]
		public RanksOrPoints ranks;

		[XmlElement("points")]
		public RanksOrPoints points;
	}

	public class RanksOrPoints : BaseBookItem
	{
		[XmlElement("reward")]
		public List<BaseEventBookData.Reward> lstRewards;
	}

	public class Tier : BaseBookItem
	{
		[XmlElement("rank")]
		public List<Rank> lstRanks;

		[XmlAttribute("min")]
		public string min { get; set; }

		[XmlAttribute("max")]
		public string max { get; set; }
	}

	public class Rank : BaseBookItem
	{
		[XmlElement("pool")]
		public List<ZonePool> lstPools;

		[XmlAttribute("min")]
		public string min { get; set; }

		[XmlAttribute("max")]
		public string max { get; set; }
	}

	public class ZonePool : BaseBookItem
	{
		[XmlAttribute("qty")]
		public string qty { get; set; }
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
