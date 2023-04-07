using System.Collections.Generic;
using System.Xml.Serialization;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class DungeonBookData : BaseBook
{
	public class Direction : BaseBookItem
	{
		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }
	}

	public class Directions : BaseBookItem
	{
		[XmlElement("direction")]
		public List<Direction> lstDirection { get; set; }
	}

	public class Path : BaseBookItem
	{
		[XmlElement("directions")]
		public Directions directions { get; set; }
	}

	public class Size : BaseBookItem
	{
		[XmlAttribute("minRows")]
		public int minRows { get; set; }

		[XmlAttribute("maxRows")]
		public int maxRows { get; set; }

		[XmlAttribute("minColumns")]
		public int minColumns { get; set; }

		[XmlAttribute("maxColumns")]
		public int maxColumns { get; set; }

		[XmlAttribute("minTiles")]
		public int minTiles { get; set; }

		[XmlAttribute("maxTiles")]
		public int maxTiles { get; set; }
	}

	public class Treasure : AssetDisplayData
	{
		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("locked")]
		public string locked { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }
	}

	public class TreasurePool : BaseBookItem
	{
		[XmlElement("treasure")]
		public List<Treasure> lstTreasure { get; set; }
	}

	public class Shrine : AssetDisplayData
	{
		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlAttribute("shrineType")]
		public int shrineType { get; set; }
	}

	public class ShrinePool : BaseBookItem
	{
		[XmlElement("shrine")]
		public List<Shrine> lstShrine { get; set; }
	}

	public class Lootable : AssetDisplayData
	{
		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }
	}

	public class LootablePool : BaseBookItem
	{
		[XmlElement("lootable")]
		public List<Lootable> lstLootable { get; set; }
	}

	public class Equipment : BaseBookItem
	{
	}

	public class Merchant : AssetDisplayData
	{
		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }
	}

	public class MerchantPool : BaseBookItem
	{
		[XmlElement("merchant")]
		public Merchant merchant { get; set; }
	}

	public class Ad : AssetDisplayData
	{
		[XmlAttribute("loot")]
		public string loot { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("count")]
		public int count { get; set; }
	}

	public class AdPool : BaseBookItem
	{
		[XmlElement("ad")]
		public Ad ad { get; set; }
	}

	public class Enemy : BaseBookItem
	{
		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlAttribute("perc")]
		public float perc { get; set; }

		[XmlAttribute("encounter")]
		public string encounter { get; set; }
	}

	public class Count : BaseBookItem
	{
		[XmlElement("enemy")]
		public List<Enemy> lstEnemy { get; set; }

		[XmlElement("treasure")]
		public List<Treasure> lstTreasure { get; set; }

		[XmlElement("shrine")]
		public List<Shrine> lstShrine { get; set; }

		[XmlElement("merchant")]
		public List<Merchant> lstMerchant { get; set; }

		[XmlElement("lootable")]
		public List<Lootable> lstLootable { get; set; }

		[XmlElement("ad")]
		public List<Ad> lstAd { get; set; }
	}

	public class Enemies : BaseBookItem
	{
		[XmlElement("enemy")]
		public List<Enemy> lstEnemy { get; set; }
	}

	public class Boss : BaseBookItem
	{
		[XmlAttribute("encounter")]
		public string encounter { get; set; }
	}

	public class Overlays : BaseBookItem
	{
		[XmlElement("overlay")]
		public List<Overlay> lstOverlay { get; set; }
	}

	public class Overlay : BaseBookItem
	{
		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("scroll")]
		public string scrollString { get; set; }
	}

	public class Dungeon : BaseBookItem
	{
		[XmlElement("enemies")]
		public Enemies enemies { get; set; }

		[XmlElement("boss")]
		public Boss boss { get; set; }

		[XmlAttribute("asset")]
		public string asset { get; set; }

		[XmlAttribute("battleBG")]
		public string battleBG { get; set; }

		[XmlAttribute("footstepsDefault")]
		public string footstepsDefault { get; set; }

		[XmlAttribute("color")]
		public string color { get; set; }

		[XmlAttribute("statMult")]
		public float statMult { get; set; }

		[XmlAttribute("path")]
		public string path { get; set; }

		[XmlAttribute("treasurePool")]
		public string treasurePool { get; set; }

		[XmlAttribute("shrinePool")]
		public string shrinePool { get; set; }

		[XmlAttribute("lootablePool")]
		public string lootablePool { get; set; }

		[XmlAttribute("merchantPool")]
		public string merchantPool { get; set; }

		[XmlAttribute("adPool")]
		public string adPool { get; set; }

		[XmlAttribute("size")]
		public string size { get; set; }

		[XmlAttribute("allowCapture")]
		public string allowCapture { get; set; }

		[XmlAttribute("allowReconnect")]
		public string allowReconnect { get; set; }

		[XmlAttribute("counts")]
		public string counts { get; set; }

		[XmlAttribute("lootEncounter")]
		public string lootEncounter { get; set; }

		[XmlAttribute("lootRolls")]
		public string lootRolls { get; set; }

		[XmlAttribute("music")]
		public string music { get; set; }

		[XmlAttribute("playRandom")]
		public string playRandom { get; set; }

		[XmlAttribute("pauseChildren")]
		public string pauseChildren { get; set; }

		[XmlAttribute("battleMusic")]
		public string battleMusic { get; set; }

		[XmlAttribute("bossMusic")]
		public string bossMusic { get; set; }

		[XmlElement("overlays")]
		public Overlays overlays { get; set; }

		[XmlAttribute("bossComplete")]
		public string bossComplete { get; set; }
	}

	[XmlElement("path")]
	public List<Path> lstPath { get; set; }

	[XmlElement("size")]
	public List<Size> lstSize { get; set; }

	[XmlElement("treasure")]
	public List<Treasure> lstTreasure { get; set; }

	[XmlElement("treasurePool")]
	public List<TreasurePool> lstTreasurePool { get; set; }

	[XmlElement("shrine")]
	public List<Shrine> lstShrine { get; set; }

	[XmlElement("shrinePool")]
	public ShrinePool shrinePool { get; set; }

	[XmlElement("lootable")]
	public List<Lootable> lstLootable { get; set; }

	[XmlElement("lootablePool")]
	public List<LootablePool> lootablePool { get; set; }

	[XmlElement("merchant")]
	public List<Merchant> lstMerchant { get; set; }

	[XmlElement("merchantPool")]
	public List<MerchantPool> lstMerchantPool { get; set; }

	[XmlElement("ad")]
	public List<Ad> lstAd { get; set; }

	[XmlElement("adPool")]
	public List<AdPool> lstAdPool { get; set; }

	[XmlElement("count")]
	public List<Count> lstCount { get; set; }

	[XmlElement("dungeon")]
	public List<Dungeon> lstDungeon { get; set; }

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		return lstDungeon.Find((Dungeon item) => item.link.Equals(identifier));
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		if (identifier > 0 && identifier < lstDungeon.Count)
		{
			return lstDungeon[identifier];
		}
		return null;
	}
}
