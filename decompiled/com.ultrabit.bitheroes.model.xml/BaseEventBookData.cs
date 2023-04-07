using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class BaseEventBookData : BaseBook
{
	public class Bonus : BaseBookItem
	{
		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }

		[XmlElement("modifiers")]
		public GameModifiersData modifiers { get; set; }

		[XmlAttribute("multiplier")]
		public int multiplier { get; set; }

		[XmlAttribute("rolls")]
		public int rolls { get; set; }
	}

	public class Bonuses : BaseBookItem
	{
		[XmlElement("bonus")]
		public List<Bonus> lstBonus { get; set; }
	}

	public class LevelPools : BaseBookItem
	{
		[XmlElement("pool")]
		public List<Pool> lstPool { get; set; }
	}

	public class Pool : BaseBookItem
	{
		[XmlElement("level")]
		public List<Level> lstLevel { get; set; }
	}

	public class Level : BaseBookItem
	{
		[XmlAttribute("points")]
		public long points { get; set; }

		[XmlElement("modifier")]
		public List<GameModifierData> lstModifier { get; set; }
	}

	public class InvasionTierData : BaseBookItem
	{
		[XmlAttribute("points")]
		public long points;

		[XmlElement("rewards")]
		public Rewards rewards { get; set; }
	}

	public class Tier : BaseBookItem
	{
		[XmlAttribute("difficulty")]
		public int difficulty { get; set; }

		[XmlAttribute("lootNPC")]
		public string lootNPC { get; set; }

		[XmlAttribute("lootNPCBoss")]
		public string lootNPCBoss { get; set; }
	}

	public class Tiers : BaseBookItem
	{
		[XmlElement("tier")]
		public List<Tier> lstTier { get; set; }
	}

	public class Encounters : BaseBookItem
	{
		[XmlAttribute("links")]
		public string links { get; set; }
	}

	public class Item : BaseBookItem
	{
		[XmlAttribute("qty")]
		public int qty { get; set; }
	}

	public class Reward : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }

		[XmlAttribute("min")]
		public string min { get; set; }

		[XmlAttribute("max")]
		public string max { get; set; }
	}

	public class Ranks : BaseBookItem
	{
		[XmlElement("reward")]
		public List<Reward> lstReward { get; set; }
	}

	public class Points : BaseBookItem
	{
		[XmlElement("reward")]
		public List<Reward> lstReward { get; set; }
	}

	public class Rewards : BaseBookItem
	{
		[XmlElement("ranks")]
		public Ranks ranks { get; set; }

		[XmlElement("points")]
		public Points points { get; set; }
	}

	public class Event : BaseBookItem
	{
		[XmlElement("guildRewards")]
		public Rewards guildRewards { get; set; }

		[XmlElement("tier")]
		public List<InvasionTierData> lstTierRewards { get; set; }

		[XmlElement("rewards")]
		public Rewards rewards { get; set; }

		[XmlAttribute("battleBG")]
		public string battleBG { get; set; }

		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("completeLoot")]
		public string completeLoot { get; set; }

		[XmlAttribute("startDate")]
		public string startDate { get; set; }

		[XmlAttribute("endDate")]
		public string endDate { get; set; }

		[XmlAttribute("slots")]
		public string slots { get; set; }

		[XmlAttribute("allowFamiliars")]
		public string allowFamiliars { get; set; }

		[XmlAttribute("allowFriends")]
		public string allowFriends { get; set; }

		[XmlAttribute("allowGuildmates")]
		public string allowGuildmates { get; set; }

		[XmlAttribute("allowSwitch")]
		public string allowSwitch { get; set; }

		[XmlAttribute("familiarMult")]
		public string familiarMult { get; set; }

		[XmlAttribute("familiarRarities")]
		public string familiarRarities { get; set; }

		[XmlElement("modifier")]
		public GameModifierData modifier { get; set; }

		[XmlAttribute("divider")]
		public float divider { get; set; }

		[XmlAttribute("tickets")]
		public int tickets { get; set; }

		[XmlAttribute("size")]
		public string size { get; set; }

		[XmlAttribute("statBalance")]
		public string statBalance { get; set; }

		[XmlAttribute("zoneID")]
		public int zoneID { get; set; }

		[XmlAttribute("battleMusic")]
		public string battleMusic { get; set; }

		[XmlElement("segmented")]
		public Segmented segmented { get; set; }

		[XmlAttribute("tokens")]
		public int tokens { get; set; }

		[XmlAttribute("badges")]
		public int badges { get; set; }

		[XmlAttribute("familiarsAdded")]
		public string familiarsAdded { get; set; }

		[XmlAttribute("book")]
		public string book { get; set; }

		[XmlElement("notSegmented")]
		public Segmented notSegmented { get; set; }

		[XmlElement("levels")]
		public Levels levels { get; set; }
	}

	public class Levels : BaseBookItem
	{
		[XmlAttribute("pool")]
		public string pool { get; set; }
	}

	public class Events : BaseBookItem
	{
		[XmlElement("event")]
		public List<Event> lstEvent { get; set; }
	}

	public class Zones : BaseBookItem
	{
		[XmlElement("zone")]
		public List<Zone> lstZone { get; set; }
	}

	public class Zone : BaseBookItem
	{
		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlElement("nodes")]
		public Nodes nodes { get; set; }
	}

	public class Nodes : BaseBookItem
	{
		[XmlElement("node")]
		public List<Node> lstNode { get; set; }
	}

	public class Node : BaseBookItem
	{
		[XmlAttribute("position")]
		public string position { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("assetOffset")]
		public string assetOffset { get; set; }

		[XmlAttribute("points")]
		public int points { get; set; }

		[XmlAttribute("hidden")]
		public string hidden { get; set; }

		[XmlAttribute("requiredNodes")]
		public string requiredNodes { get; set; }

		[XmlAttribute("unlockNodes")]
		public string unlockNodes { get; set; }

		[XmlAttribute("dungeons")]
		public string dungeons { get; set; }

		[XmlElement("rewards")]
		public ZoneRewards zoneRewards { get; set; }

		[XmlElement("paths")]
		public Paths paths { get; set; }
	}

	public class ZoneRewards : BaseBookItem
	{
		[XmlElement("item")]
		public List<Item> lstItem { get; set; }
	}

	public class Paths : BaseBookItem
	{
		[XmlElement("path")]
		public List<Path> lstPath { get; set; }
	}

	public class Path : BaseBookItem
	{
		[XmlAttribute("points")]
		public string points { get; set; }
	}

	public class Segmented : BaseBookItem
	{
		[XmlElement("ranksReward")]
		public RanksReward ranksReward { get; set; }

		[XmlElement("pointsReward")]
		public PointsReward pointsReward { get; set; }
	}

	public class RanksReward : BaseBookItem
	{
		[XmlAttribute("pool")]
		public string pool { get; set; }
	}

	public class PointsReward : BaseBookItem
	{
		[XmlAttribute("pool")]
		public string pool { get; set; }
	}

	[XmlElement("bonuses")]
	public Bonuses bonuses { get; set; }

	[XmlElement("events")]
	public Events events { get; set; }

	[XmlElement("tiers")]
	public Tiers tiers { get; set; }

	[XmlElement("encounters")]
	public Encounters encounters { get; set; }

	[XmlElement("zones")]
	public Zones zones { get; set; }

	[XmlElement("levelPools")]
	public LevelPools levelPools { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		if (bonuses != null && bonuses.lstBonus != null)
		{
			foreach (Bonus lstBonu in bonuses.lstBonus)
			{
				lstBonu.lstModifier.Clear();
				lstBonu.lstModifier = null;
			}
			bonuses.lstBonus.Clear();
		}
		foreach (Event item in events.lstEvent)
		{
			if (item.rewards != null)
			{
				if (item.rewards.ranks != null)
				{
					foreach (Reward item2 in item.rewards.ranks.lstReward)
					{
						if (item2.lstItem != null)
						{
							item2.lstItem.Clear();
							item2.lstItem = null;
						}
					}
				}
				if (item.rewards.points != null)
				{
					foreach (Reward item3 in item.rewards.points.lstReward)
					{
						if (item3.lstItem != null)
						{
							item3.lstItem.Clear();
							item3.lstItem = null;
						}
					}
				}
				item.rewards = null;
			}
			if (item.guildRewards == null)
			{
				continue;
			}
			if (item.guildRewards.ranks != null)
			{
				foreach (Reward item4 in item.guildRewards.ranks.lstReward)
				{
					if (item4.lstItem != null)
					{
						item4.lstItem.Clear();
						item4.lstItem = null;
					}
				}
			}
			if (item.guildRewards.points != null)
			{
				foreach (Reward item5 in item.guildRewards.points.lstReward)
				{
					if (item5.lstItem != null)
					{
						item5.lstItem.Clear();
						item5.lstItem = null;
					}
				}
			}
			item.guildRewards = null;
		}
		events.lstEvent.Clear();
		tiers?.lstTier.Clear();
		if (zones == null)
		{
			return;
		}
		foreach (Zone item6 in zones.lstZone)
		{
			if (item6.nodes == null)
			{
				continue;
			}
			foreach (Node item7 in item6.nodes.lstNode)
			{
				if (item7.zoneRewards != null)
				{
					item7.zoneRewards.lstItem.Clear();
					item7.zoneRewards.lstItem = null;
				}
			}
		}
		zones.lstZone.Clear();
	}
}
